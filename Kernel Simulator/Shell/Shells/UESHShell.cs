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
using KS.Misc.Screensaver;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Terminaux.Base;

namespace KS.Shell.Shells
{
    public class UESHShell : ShellExecutor, IShell
    {

        public override ShellType ShellType => ShellType.Shell;

        public override bool Bail { get; set; }

        public override void InitializeShell(params object[] ShellArgs)
        {
            while (!Bail)
            {
                if (Flags.LogoutRequested)
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Requested log out: {0}", Flags.LogoutRequested);
                    Flags.LogoutRequested = false;
                    Flags.LoggedIn = false;
                    Bail = true;
                }
                else if (!Screensaver.InSaver)
                {
                    try
                    {
                        // Try to probe injected commands
                        DebugWriter.Wdbg(DebugLevel.I, "Probing injected commands...");
                        if (Flags.CommandFlag == true)
                        {
                            Flags.CommandFlag = false;
                            if (Flags.ProbeInjectedCommands)
                            {
                                foreach (var cmd in Shell.InjectedCommands)
                                    Shell.GetLine(cmd, true);
                            }
                        }

                        // We need to put a synclock in the below steps, because the cancellation handlers seem to be taking their time to try to suppress the
                        // thread abort error messages. If the shell tried to write to the console while these handlers were still working, the command prompt
                        // would either be incomplete or not printed to the console at all. As a side effect, we wouldn't fire the shell initialization event
                        // despite us calling the RaiseShellInitialized() routine, causing some mods that rely on this event to believe that the shell was still
                        // waiting for the command.
                        lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                        {
                            // Enable cursor (We put it here to avoid repeated "CursorVisible = True" statements in different command codes)
                            ConsoleWrapper.CursorVisible = true;

                            // Write a prompt
                            if (Kernel.Kernel.DefConsoleOut is not null)
                            {
                                Console.SetOut(Kernel.Kernel.DefConsoleOut);
                            }
                            PromptPresetManager.WriteShellPrompt(ShellType);

                            // Raise shell initialization event
                            Kernel.Kernel.KernelEventManager.RaiseShellInitialized();
                        }

                        // Wait for command
                        DebugWriter.Wdbg(DebugLevel.I, "Waiting for command");
                        string strcommand = Input.ReadLine();

                        // Now, parse the line as necessary
                        if (!Screensaver.InSaver)
                        {
                            // Fire an event of PreExecuteCommand
                            Kernel.Kernel.KernelEventManager.RaisePreExecuteCommand(strcommand);

                            // Get the command
                            Shell.GetLine(strcommand);

                            // Fire an event of PostExecuteCommand
                            Kernel.Kernel.KernelEventManager.RaisePostExecuteCommand(strcommand);
                        }
                    }
                    catch (ThreadInterruptedException)
                    {
                        Flags.CancelRequested = false;
                        Bail = true;
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WStkTrc(ex);
                        TextWriterColor.Write(Translate.DoTranslation("There was an error in the shell.") + Kernel.Kernel.NewLine + "Error {0}: {1}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.GetType().FullName, ex.Message);
                        continue;
                    }
                }
            }
        }

    }
}