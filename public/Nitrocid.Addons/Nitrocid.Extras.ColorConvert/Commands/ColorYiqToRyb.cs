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
    /// Converts the color YIQ numbers to RYB.
    /// </summary>
    /// <remarks>
    /// If you want to get the RYB representation of the color from the YIQ color numbers, you can use this command.
    /// </remarks>
    class ColorYiqToRybCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we have the numeric arguments
            if (!int.TryParse(parameters.ArgumentsList[0], out int Y))
            {
                TextWriters.Write(Translate.DoTranslation("The Y component level must be numeric."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[1], out int I))
            {
                TextWriters.Write(Translate.DoTranslation("The I component level must be numeric."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[2], out int Q))
            {
                TextWriters.Write(Translate.DoTranslation("The Q component level must be numeric."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }

            // Do the job
            var color = new Color($"yiq:{Y};{I};{Q}");
            var ryb = ConversionTools.ToRyb(color.RGB);
            TextWriters.Write("- " + Translate.DoTranslation("Red color level:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{ryb.R}", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Yellow color level:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{ryb.Y}", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Blue color level:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{ryb.B}", true, KernelColorType.ListValue);
            variableValue = ryb.ToString();
            return 0;
        }

    }
}
