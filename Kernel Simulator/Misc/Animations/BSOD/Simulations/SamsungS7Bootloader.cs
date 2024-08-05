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

using Terminaux.Writer.ConsoleWriters;
using KS.Misc.Reflection;
using System;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace KS.Misc.Animations.BSOD.Simulations
{
    internal class SamsungS7Bootloader : BaseBSOD
    {
        internal enum Variant
        {
            Long,
            Short,
        }

        public override void Simulate()
        {
            // Bootloader exceptions use the following colors:
            var red = new Color(ConsoleColors.Red);
            var green = new Color(ConsoleColors.Green);
            var black = new Color(ConsoleColors.Black);

            // Load the background
            ColorTools.LoadBackDry(black);

            // Select a variant
            int excVariantInt = RandomDriver.RandomIdx(Enum.GetNames(typeof(Variant)).Length);
            var excVariant = Enum.Parse(typeof(Variant), $"{excVariantInt}");

            // Simulate a "Bootloader Exception" error message
            switch (excVariant)
            {
                case Variant.Long:
                    TextWriterColor.WriteColorBack("Bootloader exception", true, red, black);
                    TextWriterColor.WriteColorBack("[ RST_STAT = 0x10000 ]", true, red, black);
                    TextWriterColor.WriteColorBack("EVT 1.0", true, green, black);
                    TextWriterColor.WriteColorBack("ASV TBL VER = 0, Grade = C", true, green, black);
                    TextWriterColor.WriteColorBack("ECT : PARA005e", true, red, black);
                    TextWriterColor.WriteColorBack("LOT_ID = NAD8W", true, green, black);
                    TextWriterColor.WriteColorBack($"CHIP_ID = {RandomDriver.Random():X12}", true, green, black);
                    TextWriterColor.WriteColorBack("CHIP_ID2 = 00000000", true, green, black);
                    TextWriterColor.WriteColorBack("MNGS:19'C APOLLO:21'C G3D:21'C ISP:22'C\n", true, green, black);
                    TextWriterColor.WriteColorBack($"Exception: do_handler_serror: SERROR(esr: 0x{RandomDriver.Random():X8})\n", true, red, black);
                    TextWriterColor.WriteColorBack($"pc : 0x{RandomDriver.Random():X8}      ir : 0x{RandomDriver.Random():X8}      sp : 0x{RandomDriver.Random():X8}", true, red, black);
                    break;
                case Variant.Short:
                    TextWriterColor.WriteColorBack("Bootloader exception", true, red, black);
                    TextWriterColor.WriteColorBack("[ RST_STAT = 0x10000 ]\n", true, red, black);
                    TextWriterColor.WriteColorBack($"Exception: do_handler_sync: DABT_EL1(esr: 0x{RandomDriver.Random():X8})\n", true, red, black);
                    TextWriterColor.WriteColorBack($"pc : 0x{RandomDriver.Random():X8}      ir : 0x{RandomDriver.Random():X8}      sp : 0x{RandomDriver.Random():X8}", true, red, black);
                    break;
            }
        }
    }
}
