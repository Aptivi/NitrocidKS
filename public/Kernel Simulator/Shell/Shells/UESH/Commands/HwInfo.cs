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

using KS.Hardware;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
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

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly) => HardwareList.ListHardware(ListArgsOnly[0]);

        public override void HelpHelper() => TextWriterColor.Write(Translate.DoTranslation("where HardwareType will be") + " HDD, LogicalParts, CPU, GPU, Sound, Network, System, Machine, BIOS, RAM, all.");

    }
}