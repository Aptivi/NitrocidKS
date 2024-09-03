﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Text;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Terminaux.Writer.FancyWriters;

namespace KS.Shell.Shells
{
    public class TestShell : ShellExecutor, IShell
    {

        public override ShellType ShellType => ShellType.TestShell;

        public override bool Bail { get; set; }

        public override void InitializeShell(params object[] ShellArgs)
        {
            // Show the welcome message
            TextWriters.Write("", KernelColorTools.ColTypes.Neutral);
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Welcome to Test Shell!"), true);

            // Actual shell logic
            while (!Bail)
            {
                // See UESHShell.vb for more info
                lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                {
                    if (Kernel.Kernel.DefConsoleOut is not null)
                    {
                        Console.SetOut(Kernel.Kernel.DefConsoleOut);
                    }

                    // Write the prompt
                    PromptPresetManager.WriteShellPrompt(ShellType);

                    // Raise the event
                    Kernel.Kernel.KernelEventManager.RaiseTestShellInitialized();
                }

                // Parse the command
                string FullCmd = Input.ReadLine();
                try
                {
                    if ((string.IsNullOrEmpty(FullCmd) | (FullCmd?.StartsWithAnyOf([" ", "#"]))) == false)
                    {
                        Kernel.Kernel.KernelEventManager.RaiseTestPreExecuteCommand(FullCmd);
                        Shell.GetLine(FullCmd, false, "", ShellType.TestShell);
                        Kernel.Kernel.KernelEventManager.RaiseTestPostExecuteCommand(FullCmd);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    Flags.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    TextWriters.Write(Translate.DoTranslation("Error in test shell: {0}"), true, KernelColorTools.ColTypes.Error, ex.Message);
                    DebugWriter.Wdbg(DebugLevel.E, "Error: {0}", ex.Message);
                    DebugWriter.WStkTrc(ex);
                }
            }
        }

    }
}