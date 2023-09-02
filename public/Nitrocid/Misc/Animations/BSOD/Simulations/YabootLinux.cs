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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using System.Threading;
using Terminaux.Colors;

namespace KS.Misc.Animations.BSOD.Simulations
{
    internal class YabootLinux : BaseBSOD
    {
        public override void Simulate()
        {
            KernelColorTools.LoadBack(new Color(ConsoleColors.Black));
            KernelColorTools.SetConsoleColor(new Color(ConsoleColors.White));

            // Simulate a Yaboot failure
            TextWriterColor.WritePlain("Welcome to yaboot version 1.3.17", true);
            TextWriterColor.WritePlain("Enter \"help\" to get some basic usage information", true);
            TextWriterColor.WritePlain("boot: ", false);
            Thread.Sleep(3000);
            TextWriterSlowColor.WriteSlowly("Linux", true, 140, ConsoleColors.White);
            TextWriterColor.WritePlain("Please wait, loading kernel...", true);
            Thread.Sleep(60);
            TextWriterColor.WritePlain("/pci@f2000000/mac-io@17/ata-4@1f000/disk@0:4,/boot/kernel/genkernel-ppc-3.12.21-gentoo-r1: Unknown or corrupt filesystem", true);
            TextWriterColor.WritePlain("boot:", true);
        }
    }
}
