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

using System;
using Textify.Figlet;
using Terminaux.Colors;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using KS.Languages;
using KS.Misc.Threading;
using Terminaux.Base;
using Terminaux.Colors.Data;
using KS.Kernel;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Display code for NewYear
    /// </summary>
    public class NewYearDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "NewYear";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            ConsoleWrapper.CursorVisible = false;
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Color darkGreen = new(ConsoleColors.DarkGreen);
            Color green = new(ConsoleColors.Green);
            Color black = new(ConsoleColors.Black);

            // Get the current year
            int currentYear = TimeDate.TimeDate.KernelDateTime.Year;
            var currentYearDate = new DateTime(currentYear, 1, 1);

            // Select mode
            if (TimeDate.TimeDate.KernelDateTime.Date == currentYearDate)
            {
                // We're at the new year!
                string currentYearStr = currentYear.ToString();
                var figFont = FigletTools.GetFigletFont(KernelTools.BannerFigletFont);
                int figHeight = FigletTools.GetFigletHeight(currentYearStr, figFont) / 2;
                CenteredFigletTextColor.WriteCenteredFigletColorBack(figFont, currentYearStr, green, black);

                // Congratulate!
                string cong = Translate.DoTranslation("Happy new year!");
                int consoleInfoX = ConsoleWrapper.WindowWidth / 2 - cong.Length / 2;
                int consoleInfoY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
                TextWriterWhereColor.WriteWhereColorBack(cong, consoleInfoX, consoleInfoY, green, black);
            }
            else
            {
                // Print the countdown, but print the next year first using Figlet
                string nextYearStr = $"{currentYear + 1}";
                var figFont = FigletTools.GetFigletFont(KernelTools.BannerFigletFont);
                int figHeight = FigletTools.GetFigletHeight(nextYearStr, figFont) / 2;
                CenteredFigletTextColor.WriteCenteredFigletColorBack(figFont, nextYearStr, darkGreen, black);

                // Print the time remaining
                var nextYearDate = new DateTime(currentYear + 1, 1, 1);
                var distance = nextYearDate - TimeDate.TimeDate.KernelDateTime;
                string distanceStr = distance.ToString("dd\\d\\ hh\\:mm\\:ss") + " left till " + nextYearStr;
                int consoleInfoY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
                CenteredTextColor.WriteCenteredColorBack(consoleInfoY, distanceStr, darkGreen, black);
            }

            // Reset
            ThreadManager.SleepNoBlock(500, ScreensaverDisplayer.ScreensaverDisplayerThread);
            ConsoleResizeHandler.WasResized();
        }

    }
}
