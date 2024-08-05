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

using System.Runtime.Versioning;
using KS.Kernel;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using KS.Misc.Platform;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for ExcaliBeats
    /// </summary>
    public static class ExcaliBeatsSettings
    {
        private static bool excaliBeatsTrueColor = true;
        private static int excaliBeatsDelay = 140;
        private static bool excaliBeatsCycleColors = true;
        private static bool excaliBeatsExplicit = true;
        private static bool excaliBeatsTranceMode;
        private static string excaliBeatsBeatColor = "17";
        private static int excaliBeatsMaxSteps = 25;
        private static int excaliBeatsMinimumRedColorLevel = 0;
        private static int excaliBeatsMinimumGreenColorLevel = 0;
        private static int excaliBeatsMinimumBlueColorLevel = 0;
        private static int excaliBeatsMinimumColorLevel = 0;
        private static int excaliBeatsMaximumRedColorLevel = 255;
        private static int excaliBeatsMaximumGreenColorLevel = 255;
        private static int excaliBeatsMaximumBlueColorLevel = 255;
        private static int excaliBeatsMaximumColorLevel = 255;

        /// <summary>
        /// [ExcaliBeats] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public static bool ExcaliBeatsTrueColor
        {
            get
            {
                return excaliBeatsTrueColor;
            }
            set
            {
                excaliBeatsTrueColor = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] Enable color cycling (uses RNG. If disabled, uses the <see cref="ExcaliBeatsBeatColor"/> color.)
        /// </summary>
        public static bool ExcaliBeatsCycleColors
        {
            get
            {
                return excaliBeatsCycleColors;
            }
            set
            {
                excaliBeatsCycleColors = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] Explicitly change the text to Excalibur
        /// </summary>
        public static bool ExcaliBeatsExplicit
        {
            get
            {
                return excaliBeatsExplicit;
            }
            set
            {
                excaliBeatsExplicit = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] [Linux only] Trance mode - Multiplies the BPM by 2 to simulate the trance music style
        /// </summary>
        public static bool ExcaliBeatsTranceMode
        {
            get
            {
                return excaliBeatsTranceMode;
            }
            set
            {
                if (PlatformDetector.IsOnUnix())
                    excaliBeatsTranceMode = value;
                else
                    excaliBeatsTranceMode = false;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ExcaliBeatsBeatColor
        {
            get
            {
                return excaliBeatsBeatColor;
            }
            set
            {
                excaliBeatsBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ExcaliBeats] How many beats per minute to wait before making the next write?
        /// </summary>
        public static int ExcaliBeatsDelay
        {
            get
            {
                return excaliBeatsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 140;
                excaliBeatsDelay = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] How many fade steps to do?
        /// </summary>
        public static int ExcaliBeatsMaxSteps
        {
            get
            {
                return excaliBeatsMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                excaliBeatsMaxSteps = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum red color level (true color)
        /// </summary>
        public static int ExcaliBeatsMinimumRedColorLevel
        {
            get
            {
                return excaliBeatsMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                excaliBeatsMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum green color level (true color)
        /// </summary>
        public static int ExcaliBeatsMinimumGreenColorLevel
        {
            get
            {
                return excaliBeatsMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                excaliBeatsMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum blue color level (true color)
        /// </summary>
        public static int ExcaliBeatsMinimumBlueColorLevel
        {
            get
            {
                return excaliBeatsMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                excaliBeatsMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ExcaliBeatsMinimumColorLevel
        {
            get
            {
                return excaliBeatsMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                excaliBeatsMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum red color level (true color)
        /// </summary>
        public static int ExcaliBeatsMaximumRedColorLevel
        {
            get
            {
                return excaliBeatsMaximumRedColorLevel;
            }
            set
            {
                if (value <= excaliBeatsMinimumRedColorLevel)
                    value = excaliBeatsMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                excaliBeatsMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum green color level (true color)
        /// </summary>
        public static int ExcaliBeatsMaximumGreenColorLevel
        {
            get
            {
                return excaliBeatsMaximumGreenColorLevel;
            }
            set
            {
                if (value <= excaliBeatsMinimumGreenColorLevel)
                    value = excaliBeatsMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                excaliBeatsMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum blue color level (true color)
        /// </summary>
        public static int ExcaliBeatsMaximumBlueColorLevel
        {
            get
            {
                return excaliBeatsMaximumBlueColorLevel;
            }
            set
            {
                if (value <= excaliBeatsMinimumBlueColorLevel)
                    value = excaliBeatsMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                excaliBeatsMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ExcaliBeatsMaximumColorLevel
        {
            get
            {
                return excaliBeatsMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= excaliBeatsMinimumColorLevel)
                    value = excaliBeatsMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                excaliBeatsMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for ExcaliBeats
    /// </summary>
    public class ExcaliBeatsDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.ExcaliBeats.ExcaliBeatsSettings ExcaliBeatsSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "ExcaliBeats";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
            ExcaliBeatsSettingsInstance = new Animations.ExcaliBeats.ExcaliBeatsSettings()
            {
                ExcaliBeatsTrueColor = ExcaliBeatsSettings.ExcaliBeatsTrueColor,
                ExcaliBeatsBeatColor = ExcaliBeatsSettings.ExcaliBeatsBeatColor,
                ExcaliBeatsDelay = ExcaliBeatsSettings.ExcaliBeatsDelay,
                ExcaliBeatsMaxSteps = ExcaliBeatsSettings.ExcaliBeatsMaxSteps,
                ExcaliBeatsCycleColors = ExcaliBeatsSettings.ExcaliBeatsCycleColors,
                ExcaliBeatsExplicit = ExcaliBeatsSettings.ExcaliBeatsExplicit,
                ExcaliBeatsMinimumRedColorLevel = ExcaliBeatsSettings.ExcaliBeatsMinimumRedColorLevel,
                ExcaliBeatsMinimumGreenColorLevel = ExcaliBeatsSettings.ExcaliBeatsMinimumGreenColorLevel,
                ExcaliBeatsMinimumBlueColorLevel = ExcaliBeatsSettings.ExcaliBeatsMinimumBlueColorLevel,
                ExcaliBeatsMinimumColorLevel = ExcaliBeatsSettings.ExcaliBeatsMinimumColorLevel,
                ExcaliBeatsMaximumRedColorLevel = ExcaliBeatsSettings.ExcaliBeatsMaximumRedColorLevel,
                ExcaliBeatsMaximumGreenColorLevel = ExcaliBeatsSettings.ExcaliBeatsMaximumGreenColorLevel,
                ExcaliBeatsMaximumBlueColorLevel = ExcaliBeatsSettings.ExcaliBeatsMaximumBlueColorLevel,
                ExcaliBeatsMaximumColorLevel = ExcaliBeatsSettings.ExcaliBeatsMaximumColorLevel
            };
            ColorTools.LoadBackDry(0);
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.ExcaliBeats.ExcaliBeats.Simulate(ExcaliBeatsSettingsInstance);

    }
}
