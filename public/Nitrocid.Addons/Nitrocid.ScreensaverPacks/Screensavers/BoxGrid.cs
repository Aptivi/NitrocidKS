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

using Terminaux.Writer.FancyWriters;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for BoxGrid
    /// </summary>
    public class BoxGridDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "BoxGrid";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Get how many boxes to write
            int boxWidthExterior = 4;
            int boxHeightExterior = 3;
            int boxWidth = boxWidthExterior - 2;
            int boxHeight = boxHeightExterior - 2;
            int boxColumns = 0;
            int boxRows = 0;
            for (int i = 0; i < ConsoleWrapper.WindowWidth - boxWidthExterior; i += boxWidthExterior + 1)
                boxColumns++;
            for (int i = 0; i < ConsoleWrapper.WindowHeight - boxHeightExterior + 1; i += boxHeightExterior)
                boxRows++;

            // Draw the boxes
            for (int i = 0; i < boxColumns; i++)
            {
                for (int j = 0; j < boxRows; j++)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;
                    var color = ColorTools.GetRandomColor(ColorType.TrueColor);
                    BorderColor.WriteBorder(i * (boxWidthExterior + 1), j * boxHeightExterior, boxWidth, boxHeight, color);
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(ScreensaverPackInit.SaversConfig.BoxGridDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
