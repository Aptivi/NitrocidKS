﻿
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Kernel.Threading;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using Terminaux.Reader;

namespace KS.Shell.ShellBase.Shells
{
    /// <summary>
    /// Shell starter module
    /// </summary>
    public static class ShellStart
    {

        internal static List<ShellExecuteInfo> ShellStack = new();

        /// <summary>
        /// Starts the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellArgs">Arguments to pass to shell</param>
        public static void StartShell(ShellType ShellType, params object[] ShellArgs) =>
            StartShell(ShellManager.GetShellTypeName(ShellType), ShellArgs);

        /// <summary>
        /// Starts the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellArgs">Arguments to pass to shell</param>
        public static void StartShell(string ShellType, params object[] ShellArgs)
        {
            if (ShellStack.Count >= 1)
            {
                // The shell stack has a mother shell. Start another shell.
                StartShellForced(ShellType, ShellArgs);
            }
        }

        /// <summary>
        /// Force starts the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellArgs">Arguments to pass to shell</param>
        public static void StartShellForced(ShellType ShellType, params object[] ShellArgs) =>
            StartShellForced(ShellManager.GetShellTypeName(ShellType), ShellArgs);

        /// <summary>
        /// Force starts the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellArgs">Arguments to pass to shell</param>
        public static void StartShellForced(string ShellType, params object[] ShellArgs)
        {
            int shellCount = ShellStack.Count;
            try
            {
                // Make a shell executor based on shell type to select a specific executor (if the shell type is not UESH, and if the new shell isn't a mother shell)
                // Please note that the remote debug shell is not supported because it works on its own space, so it can't be interfaced using the standard IShell.
                var ShellExecute = GetShellExecutor(ShellType);

                // Make a new instance of shell information
                var ShellCommandThread = new KernelThread($"{ShellType} Command Thread", false, (cmdThreadParams) => CommandExecutor.ExecuteCommand((CommandExecutorParameters)cmdThreadParams));
                var ShellInfo = new ShellExecuteInfo(ShellType, ShellExecute, ShellCommandThread);

                // Add a new shell to the shell stack to indicate that we have a new shell (a visitor)!
                ShellStack.Add(ShellInfo);
                if (!ShellManager.histories.ContainsKey(ShellType))
                    ShellManager.histories.Add(ShellType, new());
                ShellExecute.InitializeShell(ShellArgs);
            }
            catch (Exception ex)
            {
                // There is an exception trying to run the shell. Throw the message to the debugger and to the caller.
                DebugWriter.WriteDebug(DebugLevel.E, "Failed initializing shell!!! Type: {0}, Message: {1}", ShellType, ex.Message);
                DebugWriter.WriteDebug(DebugLevel.E, "Additional info: Args: {0} [{1}], Shell Stack: {2} shells, shellCount: {3} shells", ShellArgs.Length, string.Join(", ", ShellArgs), ShellStack.Count, shellCount);
                DebugWriter.WriteDebug(DebugLevel.E, "This shell needs to be killed in order for the shell manager to proceed. Passing exception to caller...");
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "If you don't see \"Purge\" from {0} after few lines, this indicates that we're in a seriously corrupted state.", nameof(StartShellForced));
                throw new KernelException(KernelExceptionType.ShellOperation, Translate.DoTranslation("Failed trying to initialize shell"), ex);
            }
            finally
            {
                // There is either an unknown shell error trying to be initialized or a shell has manually set Bail to true prior to exiting, like the JSON shell
                // that sets this property when it fails to open the JSON file due to syntax error or something. If we haven't added the shell to the shell stack,
                // do nothing. Else, purge that shell with KillShell(). Otherwise, we'll get another shell's commands in the wrong shell and other problems will
                // occur until the ghost shell has exited either automatically or manually, so check to see if we have added the newly created shell to the shell
                // stack and kill that faulted shell so that we can have the correct shell in the most recent shell, ^1, from the stack.
                int newShellCount = ShellStack.Count;
                DebugWriter.WriteDebug(DebugLevel.I, "Purge: newShellCount: {0} shells, shellCount: {1} shells", newShellCount, shellCount);
                if (newShellCount > shellCount)
                    KillShellForced();
                TermReaderTools.SetHistory(ShellManager.histories[ShellManager.LastShellType]);
            }
        }

        /// <summary>
        /// Kills the last running shell
        /// </summary>
        public static void KillShell()
        {
            // We must have at least two shells to kill the last shell. Else, we will have zero shells running, making us look like we've logged out!
            if (ShellStack.Count >= 2)
            {
                ShellStack[^1].ShellBase.Bail = true;
                PurgeShells();
            }
            else
            {
                throw new KernelException(KernelExceptionType.ShellOperation, Translate.DoTranslation("Can not kill the mother shell!"));
            }
        }

        /// <summary>
        /// Force kills the last running shell
        /// </summary>
        public static void KillShellForced()
        {
            if (ShellStack.Count >= 1)
            {
                ShellStack[^1].ShellBase.Bail = true;
                PurgeShells();
            }
        }

        /// <summary>
        /// Cleans up the shell stack
        /// </summary>
        public static void PurgeShells() =>
            // Remove these shells from the stack
            ShellStack.RemoveAll(x => x.ShellBase.Bail);

        /// <summary>
        /// Gets the shell executor based on the shell type
        /// </summary>
        /// <param name="ShellType">The requested shell type</param>
        public static BaseShell GetShellExecutor(ShellType ShellType) =>
            GetShellExecutor(ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets the shell executor based on the shell type
        /// </summary>
        /// <param name="ShellType">The requested shell type</param>
        public static BaseShell GetShellExecutor(string ShellType) =>
            ShellManager.GetShellInfo(ShellType).ShellBase;

    }
}