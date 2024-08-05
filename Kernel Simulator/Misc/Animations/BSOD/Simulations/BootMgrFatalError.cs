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

using Terminaux.Writer.ConsoleWriters;
using KS.Misc.Reflection;
using System;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace KS.Misc.Animations.BSOD.Simulations
{
    internal class BootMgrFatalError : BaseBSOD
    {
        internal enum Variant
        {
            Missing,
            Compressed,
            Corrupted,
        }

        public override void Simulate()
        {
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            ColorTools.SetConsoleColor(new Color(ConsoleColors.White));

            // Select a variant
            int excVariantInt = RandomDriver.RandomIdx(Enum.GetNames(typeof(Variant)).Length);
            var excVariant = Enum.Parse(typeof(Variant), $"{excVariantInt}");
            switch (excVariant)
            {
                case Variant.Missing:
                    TextWriterRaw.WritePlain("BOOTMGR is missing.", true);
                    TextWriterRaw.WritePlain("Press Ctrl+Alt+Del to restart...", true);
                    break;
                case Variant.Compressed:
                    TextWriterRaw.WritePlain("BOOTMGR is compressed.", true);
                    TextWriterRaw.WritePlain("Press Ctrl+Alt+Del to restart...", true);
                    break;
                case Variant.Corrupted:
                    TextWriterRaw.WritePlain("BOOTMGR image is corrupt.  The system cannot boot.", true);
                    break;
            }
        }
    }
}
