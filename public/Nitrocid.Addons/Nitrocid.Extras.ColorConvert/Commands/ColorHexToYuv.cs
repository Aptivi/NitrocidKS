﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Terminaux.Colors;
using Terminaux.Colors.Models.Conversion;

namespace Nitrocid.Extras.ColorConvert.Commands
{
    /// <summary>
    /// Converts the hexadecimal representation of the color to YUV numbers.
    /// </summary>
    /// <remarks>
    /// If you want to get the YUV color numbers from the hexadecimal representation of the color, you can use this command.
    /// </remarks>
    class ColorHexToYuvCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string Hex = parameters.ArgumentsList[0];

            // Do the job
            Color color = new(Hex);
            var yuv = ConversionTools.ToYuv(color.RGB);
            TextWriters.Write("- " + Translate.DoTranslation("Luma:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{yuv.Luma}", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("U-Chroma:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{yuv.ChromaU}", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("V-Chroma:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{yuv.ChromaV}", true, KernelColorType.ListValue);
            variableValue = yuv.ToString();
            return 0;
        }

    }
}
