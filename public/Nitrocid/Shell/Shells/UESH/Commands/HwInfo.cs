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

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Hardware;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows hardware information
    /// </summary>
    /// <remarks>
    /// This shows you the detailed hardware information, including the CPU information and its features.
    /// </remarks>
    class HwInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            HardwareList.ListHardware(ListArgsOnly[0]);
            return 0;
        }

        public override void HelpHelper() =>
            TextWriterColor.Write(Translate.DoTranslation("where HardwareType will be") +
                " HDD, LogicalParts, CPU, GPU, Sound, Network, System, Machine, BIOS, RAM, all.");

    }
}
