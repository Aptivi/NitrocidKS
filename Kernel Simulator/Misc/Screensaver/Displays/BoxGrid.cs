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

using Terminaux.Writer.FancyWriters;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for BoxGrid
    /// </summary>
    public static class BoxGridSettings
    {
        private static int boxGridDelay = 5000;
        private static int boxGridMinimumRedColorLevel = 0;
        private static int boxGridMinimumGreenColorLevel = 0;
        private static int boxGridMinimumBlueColorLevel = 0;
        private static int boxGridMaximumRedColorLevel = 255;
        private static int boxGridMaximumGreenColorLevel = 255;
        private static int boxGridMaximumBlueColorLevel = 255;

        /// <summary>
        /// [BoxGrid] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BoxGridDelay
        {
            get
            {
                return boxGridDelay;
            }
            set
            {
                if (value <= 0)
                    value = 5000;
                boxGridDelay = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The minimum red color level (true color)
        /// </summary>
        public static int BoxGridMinimumRedColorLevel
        {
            get
            {
                return boxGridMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                boxGridMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The minimum green color level (true color)
        /// </summary>
        public static int BoxGridMinimumGreenColorLevel
        {
            get
            {
                return boxGridMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                boxGridMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The minimum blue color level (true color)
        /// </summary>
        public static int BoxGridMinimumBlueColorLevel
        {
            get
            {
                return boxGridMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                boxGridMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The maximum red color level (true color)
        /// </summary>
        public static int BoxGridMaximumRedColorLevel
        {
            get
            {
                return boxGridMaximumRedColorLevel;
            }
            set
            {
                if (value <= boxGridMinimumRedColorLevel)
                    value = boxGridMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                boxGridMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The maximum green color level (true color)
        /// </summary>
        public static int BoxGridMaximumGreenColorLevel
        {
            get
            {
                return boxGridMaximumGreenColorLevel;
            }
            set
            {
                if (value <= boxGridMinimumGreenColorLevel)
                    value = boxGridMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                boxGridMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The maximum blue color level (true color)
        /// </summary>
        public static int BoxGridMaximumBlueColorLevel
        {
            get
            {
                return boxGridMaximumBlueColorLevel;
            }
            set
            {
                if (value <= boxGridMinimumBlueColorLevel)
                    value = boxGridMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                boxGridMaximumBlueColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for BoxGrid
    /// </summary>
    public class BoxGridDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BoxGrid";

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
            ThreadManager.SleepNoBlock(BoxGridSettings.BoxGridDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
