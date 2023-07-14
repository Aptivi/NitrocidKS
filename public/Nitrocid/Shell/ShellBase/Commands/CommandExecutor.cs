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
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Shells;
using KS.Kernel.Events;
using System.Linq;

namespace KS.Shell.ShellBase.Commands
{
    /// <summary>
    /// Command parser module
    /// </summary>
    internal static class CommandExecutor
    {

        /// <summary>
        /// Parameters to pass to <see cref="ExecuteCommand(ExecuteCommandParameters)"/>
        /// </summary>
        internal class ExecuteCommandParameters
        {
            /// <summary>
            /// The requested command with arguments
            /// </summary>
            internal string RequestedCommand;
            /// <summary>
            /// The shell type
            /// </summary>
            internal string ShellType;
            /// <summary>
            /// Is the command the custom command?
            /// </summary>
            internal bool CustomCommand;
            /// <summary>
            /// Mod commands
            /// </summary>
            internal Dictionary<string, CommandInfo> ModCommands;

            internal ExecuteCommandParameters(string RequestedCommand, ShellType ShellType) : 
                this(RequestedCommand, Shell.GetShellTypeName(ShellType)) 
            { }

            internal ExecuteCommandParameters(string RequestedCommand, string ShellType)
            {
                this.RequestedCommand = RequestedCommand;
                this.ShellType = ShellType;
            }
        }

        internal static void StartCommandThread(ExecuteCommandParameters ThreadParams)
        {
            // Since we're probably trying to run a command using the alternative command threads, if the main shell command thread
            // is running, use that to execute the command. This ensures that commands like "wrap" that also execute commands from the
            // shell can do their job.
            var ShellInstance = ShellStart.ShellStack[^1];
            var StartCommandThread = ShellInstance.ShellCommandThread;
            bool CommandThreadValid = true;
            if (StartCommandThread.IsAlive)
            {
                if (ShellInstance.AltCommandThreads.Count > 0)
                {
                    StartCommandThread = ShellInstance.AltCommandThreads[^1];
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: Alt command threads are not there.");
                    CommandThreadValid = false;
                }
            }
            if (CommandThreadValid)
            {
                StartCommandThread.Start(ThreadParams);
                StartCommandThread.Wait();
                StartCommandThread.Stop();
            }
        }

        internal static void ExecuteCommand(ExecuteCommandParameters ThreadParams) =>
            ExecuteCommand(ThreadParams, ThreadParams.ModCommands is not null ? ThreadParams.ModCommands : CommandManager.GetCommands(ThreadParams.ShellType));

        private static void ExecuteCommand(ExecuteCommandParameters ThreadParams, Dictionary<string, CommandInfo> TargetCommands)
        {
            string RequestedCommand = ThreadParams.RequestedCommand;
            string ShellType = ThreadParams.ShellType;
            try
            {
                // Variables
                var ArgumentInfo = new ProvidedCommandArgumentsInfo(RequestedCommand, ShellType);
                string Command = ArgumentInfo.Command;
                var Args = ArgumentInfo.ArgumentsList;
                var Switches = ArgumentInfo.SwitchesList;
                string StrArgs = ArgumentInfo.ArgumentsText;
                bool RequiredArgumentsProvided = ArgumentInfo.RequiredArgumentsProvided;
                bool RequiredSwitchesProvided = ArgumentInfo.RequiredSwitchesProvided;

                // Check to see if a requested command is obsolete
                if (TargetCommands[Command].Flags.HasFlag(CommandFlags.Obsolete))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "The command requested {0} is obsolete", Command);
                    TextWriterColor.Write(Translate.DoTranslation("This command is obsolete and will be removed in a future release."));
                }

                // If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                if (TargetCommands[Command].CommandArgumentInfo is not null)
                {
                    var ArgInfo = TargetCommands[Command].CommandArgumentInfo;
                    if (ArgInfo.ArgumentsRequired & RequiredArgumentsProvided ||
                        !ArgInfo.ArgumentsRequired)
                    {
                        if (ArgInfo.Switches.Any((@switch) => @switch.IsRequired) && RequiredSwitchesProvided ||
                            !ArgInfo.Switches.Any((@switch) => @switch.IsRequired))
                        {
                            var CommandBase = TargetCommands[Command].CommandBase;
                            CommandBase.Execute(StrArgs, Args, Switches);
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "User hasn't provided enough switches for {0}", Command);
                            TextWriterColor.Write(Translate.DoTranslation("Required switches are not provided. See below for usage:"));
                            HelpSystem.ShowHelp(Command, ShellType);
                        }
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "User hasn't provided enough arguments for {0}", Command);
                        TextWriterColor.Write(Translate.DoTranslation("Required arguments are not provided. See below for usage:"));
                        HelpSystem.ShowHelp(Command, ShellType);
                    }
                }
                else
                {
                    var CommandBase = TargetCommands[Command].CommandBase;
                    CommandBase.Execute(StrArgs, Args, Switches);
                }
            }
            catch (ThreadInterruptedException)
            {
                Flags.CancelRequested = false;
                return;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.CommandError, ShellType, RequestedCommand, ex);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("Error trying to execute command") + " {2}." + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, KernelColorType.Error, ex.GetType().FullName, ex.Message, RequestedCommand);
            }
        }

    }
}
