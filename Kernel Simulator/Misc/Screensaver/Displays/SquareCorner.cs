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

using KS.Misc.Writers.DebugWriters;
using KS.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for SquareCorner
    /// </summary>
    public static class SquareCornerSettings
    {
        private static int squareCornerDelay = 10;
        private static int squareCornerFadeOutDelay = 3000;
        private static int squareCornerMaxSteps = 25;
        private static int squareCornerMinimumRedColorLevel = 0;
        private static int squareCornerMinimumGreenColorLevel = 0;
        private static int squareCornerMinimumBlueColorLevel = 0;
        private static int squareCornerMaximumRedColorLevel = 255;
        private static int squareCornerMaximumGreenColorLevel = 255;
        private static int squareCornerMaximumBlueColorLevel = 255;

        /// <summary>
        /// [SquareCorner] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SquareCornerDelay
        {
            get
            {
                return squareCornerDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                squareCornerDelay = value;
            }
        }
        /// <summary>
        /// [SquareCorner] How many milliseconds to wait before fading the square out?
        /// </summary>
        public static int SquareCornerFadeOutDelay
        {
            get
            {
                return squareCornerFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                squareCornerFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [SquareCorner] How many fade steps to do?
        /// </summary>
        public static int SquareCornerMaxSteps
        {
            get
            {
                return squareCornerMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                squareCornerMaxSteps = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum red color level (true color)
        /// </summary>
        public static int SquareCornerMinimumRedColorLevel
        {
            get
            {
                return squareCornerMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                squareCornerMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum green color level (true color)
        /// </summary>
        public static int SquareCornerMinimumGreenColorLevel
        {
            get
            {
                return squareCornerMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                squareCornerMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum blue color level (true color)
        /// </summary>
        public static int SquareCornerMinimumBlueColorLevel
        {
            get
            {
                return squareCornerMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                squareCornerMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum red color level (true color)
        /// </summary>
        public static int SquareCornerMaximumRedColorLevel
        {
            get
            {
                return squareCornerMaximumRedColorLevel;
            }
            set
            {
                if (value <= squareCornerMinimumRedColorLevel)
                    value = squareCornerMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                squareCornerMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum green color level (true color)
        /// </summary>
        public static int SquareCornerMaximumGreenColorLevel
        {
            get
            {
                return squareCornerMaximumGreenColorLevel;
            }
            set
            {
                if (value <= squareCornerMinimumGreenColorLevel)
                    value = squareCornerMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                squareCornerMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum blue color level (true color)
        /// </summary>
        public static int SquareCornerMaximumBlueColorLevel
        {
            get
            {
                return squareCornerMaximumBlueColorLevel;
            }
            set
            {
                if (value <= squareCornerMinimumBlueColorLevel)
                    value = squareCornerMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                squareCornerMaximumBlueColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for SquareCorner
    /// </summary>
    public class SquareCornerDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.SquareCorner.SquareCornerSettings SquareCornerSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "SquareCorner";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
            SquareCornerSettingsInstance = new Animations.SquareCorner.SquareCornerSettings()
            {
                SquareCornerDelay = SquareCornerSettings.SquareCornerDelay,
                SquareCornerFadeOutDelay = SquareCornerSettings.SquareCornerFadeOutDelay,
                SquareCornerMaxSteps = SquareCornerSettings.SquareCornerMaxSteps,
                SquareCornerMinimumRedColorLevel = SquareCornerSettings.SquareCornerMinimumRedColorLevel,
                SquareCornerMinimumGreenColorLevel = SquareCornerSettings.SquareCornerMinimumGreenColorLevel,
                SquareCornerMinimumBlueColorLevel = SquareCornerSettings.SquareCornerMinimumBlueColorLevel,
                SquareCornerMaximumRedColorLevel = SquareCornerSettings.SquareCornerMaximumRedColorLevel,
                SquareCornerMaximumGreenColorLevel = SquareCornerSettings.SquareCornerMaximumGreenColorLevel,
                SquareCornerMaximumBlueColorLevel = SquareCornerSettings.SquareCornerMaximumBlueColorLevel,
            };
            ColorTools.LoadBackDry(0);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.SquareCorner.SquareCorner.Simulate(SquareCornerSettingsInstance);

    }
}
