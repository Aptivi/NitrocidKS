﻿
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Arguments.KernelArguments;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using System;
using System.Collections.Generic;

namespace KS.Arguments.ArgumentBase
{
    public static class ArgumentParse
    {

        public readonly static Dictionary<string, ArgumentInfo> AvailableArgs = new() { { "quiet", new ArgumentInfo("quiet", ArgumentType.KernelArgs, "Starts the kernel quietly", "", false, 0, new QuietArgument()) }, { "debug", new ArgumentInfo("debug", ArgumentType.KernelArgs, "Enables debug mode", "", false, 0, new DebugArgument()) }, { "maintenance", new ArgumentInfo("maintenance", ArgumentType.KernelArgs, "Like safe mode, but also disables multi-user and some customization", "", false, 0, new MaintenanceArgument()) }, { "safe", new ArgumentInfo("safe", ArgumentType.KernelArgs, "Starts the kernel in safe mode, disabling all mods", "", false, 0, new SafeArgument()) }, { "testInteractive", new ArgumentInfo("testInteractive", ArgumentType.KernelArgs, "Opens a test shell", "", false, 0, new TestInteractiveArgument()) } };

        /// <summary>
        /// Parses specified arguments
        /// </summary>
        /// <param name="ArgumentsInput">Input Arguments</param>
        /// <param name="ArgumentType">Argument type</param>
        public static void ParseArguments(List<string> ArgumentsInput, ArgumentType ArgumentType)
        {
            // Check for the arguments written by the user
            try
            {
                // Select the argument dictionary
                var Arguments = AvailableArgs;
                switch (ArgumentType)
                {
                    case ArgumentType.PreBootCommandLineArgs:
                        {
                            Arguments = PreBootCommandLineArgsParse.AvailablePreBootCMDLineArgs;
                            break;
                        }
                    case ArgumentType.CommandLineArgs:
                        {
                            Arguments = CommandLineArgs.AvailableCMDLineArgs;
                            break;
                        }
                }

                // Parse them now
                for (int i = 0, loopTo = ArgumentsInput.Count - 1; i <= loopTo; i++)
                {
                    string Argument = ArgumentsInput[i];
                    if (Arguments.ContainsKey(Argument))
                    {
                        // Variables
                        var ArgumentInfo = new ProvidedArgumentArgumentsInfo(Argument, ArgumentType);
                        var Args = ArgumentInfo.ArgumentsList;
                        var Switches = ArgumentInfo.SwitchesList;
                        string strArgs = ArgumentInfo.ArgumentsText;
                        bool RequiredArgumentsProvided = ArgumentInfo.RequiredArgumentsProvided;

                        // If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                        if (Arguments[Argument].ArgumentsRequired & RequiredArgumentsProvided | !Arguments[Argument].ArgumentsRequired)
                        {
                            var ArgumentBase = Arguments[Argument].ArgumentBase;
                            ArgumentBase.Execute(strArgs, Args, Switches);
                        }
                        else
                        {
                            DebugWriter.Wdbg(DebugLevel.W, "User hasn't provided enough arguments for {0}", Argument);
                            TextWriterColor.Write(Translate.DoTranslation("There was not enough arguments."), true, ColorTools.ColTypes.Neutral);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KernelTools.KernelError(KernelErrorLevel.U, true, 5L, Translate.DoTranslation("Unrecoverable error in argument:") + " " + ex.Message, ex);
            }
        }

    }
}