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
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Terminaux.Colors;
using Terminaux.Colors.Models.Conversion;

namespace Nitrocid.Extras.ColorConvert.Commands
{
    /// <summary>
    /// Converts the color HSL numbers to YUV.
    /// </summary>
    /// <remarks>
    /// If you want to get the YUV representation of the color from the HSL color numbers, you can use this command.
    /// </remarks>
    class ColorHslToYuvCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we have the numeric arguments
            if (!int.TryParse(parameters.ArgumentsList[0], out int H))
            {
                TextWriters.Write(Translate.DoTranslation("The hue level must be numeric."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[1], out int S))
            {
                TextWriters.Write(Translate.DoTranslation("The saturation level must be numeric."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[2], out int L))
            {
                TextWriters.Write(Translate.DoTranslation("The luminance or lighting level must be numeric."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }

            // Do the job
            var color = new Color($"hsl:{H};{S};{L}");
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
