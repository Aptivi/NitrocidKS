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

namespace Nitrocid.Extras.ColorConvert.Commands
{
    /// <summary>
    /// Converts the color YUV numbers to RYB in KS format.
    /// </summary>
    /// <remarks>
    /// If you want to get the semicolon-delimited sequence of the RYB color numbers from the YUV representation of the color, you can use this command. You can use this to form a valid color sequence to generate new color instances for your mods.
    /// </remarks>
    class ColorYuvToRybKSCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we have the numeric arguments
            if (!double.TryParse(parameters.ArgumentsList[0], out double Y))
            {
                TextWriters.Write(Translate.DoTranslation("The Y component level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!double.TryParse(parameters.ArgumentsList[1], out double U))
            {
                TextWriters.Write(Translate.DoTranslation("The U component level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!double.TryParse(parameters.ArgumentsList[2], out double V))
            {
                TextWriters.Write(Translate.DoTranslation("The V component level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }

            // Do the job
            string ryb = KernelColorConversionTools.ConvertFromYuvToRyb(Y, U, V);
            TextWriters.Write("- " + Translate.DoTranslation("RYB color sequence:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(ryb, true, KernelColorType.ListValue);
            variableValue = ryb;
            return 0;
        }

    }
}