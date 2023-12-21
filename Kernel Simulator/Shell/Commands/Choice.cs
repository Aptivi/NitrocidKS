//
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
using System.Collections.Generic;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Scripting.Interaction;
using KS.Shell.ShellBase.Commands;
using Terminaux.Inputs.Styles.Choice;

namespace KS.Shell.Commands
{
    class ChoiceCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var Titles = new List<string>();
            var PressEnter = default(bool);
            var OutputType = ConsoleBase.Inputs.Styles.ChoiceStyle.DefaultChoiceOutputType;
            if (ListSwitchesOnly.Contains("-multiple"))
                PressEnter = true;
            if (ListSwitchesOnly.Contains("-single"))
                PressEnter = false;

            // Add the provided working titles
            if (ListArgsOnly.Length > 3)
            {
                Titles.AddRange(ListArgsOnly.Skip(3));
            }

            // Check for output type switches
            if (ListSwitchesOnly.Length > 0)
            {
                if (ListSwitchesOnly[0] == "-o")
                    OutputType = ChoiceOutputType.OneLine;
                if (ListSwitchesOnly[0] == "-t")
                    OutputType = ChoiceOutputType.TwoLines;
                if (ListSwitchesOnly[0] == "-m")
                    OutputType = ChoiceOutputType.Modern;
                if (ListSwitchesOnly[0] == "-a")
                    OutputType = ChoiceOutputType.Table;
            }

            // Prompt for choice
            UESHCommands.PromptChoiceAndSet(ListArgsOnly[2], ListArgsOnly[0], ListArgsOnly[1], [.. Titles], OutputType, PressEnter);
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("where <$variable> is any variable that will be used to store response") + Kernel.Kernel.NewLine + Translate.DoTranslation("where <answers> are one-lettered answers of the question separated in slashes"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
            TextWriterColor.Write("  -multiple: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Indicate that the answer can take more than one character"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
            TextWriterColor.Write("  -single: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Indicate that the answer can take just one character"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
            TextWriterColor.Write("  -o: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Print the question and the answers in one line"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
            TextWriterColor.Write("  -t: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Print the question and the answers in two lines"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
            TextWriterColor.Write("  -m: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Print the question and the answers in the modern way"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
            TextWriterColor.Write("  -a: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Print the question and the answers in a table"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
        }

    }
}