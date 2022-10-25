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

using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using System;
using System.Linq;

namespace KS.Arguments.ArgumentBase
{
    /// <summary>
    /// Argument help system module
    /// </summary>
    public static class ArgumentHelpSystem
    {

        /// <summary>
        /// Shows the help of an argument, or argument list if nothing is specified
        /// </summary>
        /// <param name="ArgumentType">A specified argument type</param>
        public static void ShowArgsHelp(ArgumentType ArgumentType) => ShowArgsHelp("", ArgumentType);

        /// <summary>
        /// Shows the help of an argument, or argument list if nothing is specified
        /// </summary>
        /// <param name="Argument">A specified argument</param>
        public static void ShowArgsHelp(string Argument) => ShowArgsHelp(Argument, ArgumentType.KernelArgs);

        /// <summary>
        /// Shows the help of an argument, or argument list if nothing is specified
        /// </summary>
        /// <param name="Argument">A specified argument</param>
        /// <param name="ArgumentType">A specified argument type</param>
        public static void ShowArgsHelp(string Argument, ArgumentType ArgumentType)
        {
            // Determine argument type
            var ArgumentList = (ArgumentType == ArgumentType.CommandLineArgs ? CommandLineArgs.AvailableCMDLineArgs : ArgumentParse.AvailableArgs)
                                   .OrderBy((CommandValuePair) => CommandValuePair.Key)
                                   .ToDictionary((CommandValuePair) => CommandValuePair.Key, (CommandValuePair) => CommandValuePair.Value);

            // Check to see if argument exists
            if (!string.IsNullOrWhiteSpace(Argument) & ArgumentList.ContainsKey(Argument))
            {
                string HelpDefinition = ArgumentList[Argument].GetTranslatedHelpEntry();
                int UsageLength = Translate.DoTranslation("Usage:").Length;
                var HelpUsages = Array.Empty<string>();

                // Populate help usages
                if (ArgumentList[Argument].ArgArgumentInfo is not null)
                    HelpUsages = ArgumentList[Argument].ArgArgumentInfo.HelpUsages;

                // Print usage information
                if (HelpUsages.Length != 0)
                {
                    var Indent = default(bool);
                    TextWriterColor.Write(Translate.DoTranslation("Usage:"));

                    // Enumerate through the available help usages
                    foreach (string HelpUsage in HelpUsages)
                    {
                        // Indent, if necessary
                        if (Indent)
                            TextWriterColor.Write(" ".Repeat(UsageLength), false, ColorTools.ColTypes.ListEntry);
                        TextWriterColor.Write($" {Argument} {HelpUsage}", true, ColorTools.ColTypes.ListEntry);
                        Indent = true;
                    }
                }

                // Write the description now
                if (string.IsNullOrEmpty(HelpDefinition))
                    HelpDefinition = Translate.DoTranslation("Command defined by ") + Argument;
                TextWriterColor.Write(Translate.DoTranslation("Description:") + $" {HelpDefinition}", true, ColorTools.ColTypes.ListValue);

                // Extra help action for some arguments
                if (ArgumentList[Argument].AdditionalHelpAction is not null)
                    ArgumentList[Argument].AdditionalHelpAction.DynamicInvoke();
            }
            else if (string.IsNullOrWhiteSpace(Argument))
            {
                // List the available arguments
                if (!Flags.SimHelp)
                {
                    foreach (string cmd in ArgumentList.Keys)
                    {
                        TextWriterColor.Write("- {0}: ", false, ColorTools.ColTypes.ListEntry, cmd);
                        TextWriterColor.Write("{0}", true, ColorTools.ColTypes.ListValue, ArgumentList[cmd].GetTranslatedHelpEntry());
                    }
                }
                else
                {
                    foreach (string cmd in ArgumentList.Keys)
                        TextWriterColor.Write("{0}, ", false, ColorTools.ColTypes.ListEntry, cmd);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("No help for argument \"{0}\"."), true, ColorTools.ColTypes.Error, Argument);
            }
        }

    }
}
