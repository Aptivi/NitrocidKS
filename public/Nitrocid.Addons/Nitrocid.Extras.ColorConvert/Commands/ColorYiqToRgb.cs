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

namespace Nitrocid.Extras.ColorConvert.Commands
{
    /// <summary>
    /// Converts the color YIQ numbers to RGB.
    /// </summary>
    /// <remarks>
    /// If you want to get the RGB representation of the color from the HSL color numbers, you can use this command.
    /// </remarks>
    class ColorYiqToRgbCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we have the numeric arguments
            if (!int.TryParse(parameters.ArgumentsList[0], out int Y))
            {
                TextWriters.Write(Translate.DoTranslation("The Y component level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!int.TryParse(parameters.ArgumentsList[1], out int I))
            {
                TextWriters.Write(Translate.DoTranslation("The I component level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!int.TryParse(parameters.ArgumentsList[2], out int Q))
            {
                TextWriters.Write(Translate.DoTranslation("The Q component level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }

            // Do the job
            var color = new Color($"yiq:{Y};{I};{Q}");
            TextWriters.Write("- " + Translate.DoTranslation("Red color level:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{color.RGB.R}", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Green color level:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{color.RGB.G}", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Blue color level:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{color.RGB.B}", true, KernelColorType.ListValue);
            variableValue = color.PlainSequence;
            return 0;
        }

    }
}