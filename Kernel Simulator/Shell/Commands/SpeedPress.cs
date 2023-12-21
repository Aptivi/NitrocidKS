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
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;

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

using KS.Misc.Games;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class SpeedPressCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var Difficulty = SpeedPress.SpeedPressDifficulty.Medium;
            int CustomTimeout = SpeedPress.SpeedPressTimeout;
            if (ListSwitchesOnly.Contains("-e"))
                Difficulty = SpeedPress.SpeedPressDifficulty.Easy;
            if (ListSwitchesOnly.Contains("-m"))
                Difficulty = SpeedPress.SpeedPressDifficulty.Medium;
            if (ListSwitchesOnly.Contains("-h"))
                Difficulty = SpeedPress.SpeedPressDifficulty.Hard;
            if (ListSwitchesOnly.Contains("-v"))
                Difficulty = SpeedPress.SpeedPressDifficulty.VeryHard;
            if (ListSwitchesOnly.Contains("-c") & ListArgsOnly.Count() > 0 && StringQuery.IsStringNumeric(ListArgsOnly[0]))
            {
                Difficulty = SpeedPress.SpeedPressDifficulty.Custom;
                CustomTimeout = Convert.ToInt32(ListArgsOnly[0]);
            }
            SpeedPress.InitializeSpeedPress(Difficulty, CustomTimeout);
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
            TextWriterColor.Write("  -e: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Starts the game in easy difficulty"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
            TextWriterColor.Write("  -m: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Starts the game in medium difficulty"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
            TextWriterColor.Write("  -h: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Starts the game in hard difficulty"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
            TextWriterColor.Write("  -v: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Starts the game in very hard difficulty"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
            TextWriterColor.Write("  -c: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Starts the game in custom difficulty. Please note that the custom timeout in milliseconds should be written as argument."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
        }

    }
}