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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
using System;
using System.Collections.Generic;
using Textify.Figlet;

namespace KS.TestShell.Commands
{
    class Test_PrintFigletFCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var Parts = new List<string>(ListArgs);
            KernelColorTools.ColTypes Color = (KernelColorTools.ColTypes)Convert.ToInt32(ListArgs[0]);
            var FigletFont = FigletTools.GetFigletFont(ListArgs[1]);
            object[] Vars = ListArgs[2].Split(';');
            string Text = ListArgs[3];
            TextFancyWriters.WriteFiglet(Text, FigletFont, Color, Vars: Vars);
        }

    }
}
