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

using KS.Misc.Reflection;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for DanceLines
    /// </summary>
    public static class DanceLinesSettings
    {
        private static bool danceLinesTrueColor = true;
        private static int danceLinesDelay = 50;
        private static string danceLinesLineChar = "-";
        private static string danceLinesBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private static int danceLinesMinimumRedColorLevel = 0;
        private static int danceLinesMinimumGreenColorLevel = 0;
        private static int danceLinesMinimumBlueColorLevel = 0;
        private static int danceLinesMinimumColorLevel = 0;
        private static int danceLinesMaximumRedColorLevel = 255;
        private static int danceLinesMaximumGreenColorLevel = 255;
        private static int danceLinesMaximumBlueColorLevel = 255;
        private static int danceLinesMaximumColorLevel = 255;

        /// <summary>
        /// [DanceLines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool DanceLinesTrueColor
        {
            get
            {
                return danceLinesTrueColor;
            }
            set
            {
                danceLinesTrueColor = value;
            }
        }
        /// <summary>
        /// [DanceLines] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int DanceLinesDelay
        {
            get
            {
                return danceLinesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                danceLinesDelay = value;
            }
        }
        /// <summary>
        /// [DanceLines] Line character
        /// </summary>
        public static string DanceLinesLineChar
        {
            get
            {
                return danceLinesLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                danceLinesLineChar = value;
            }
        }
        /// <summary>
        /// [DanceLines] Screensaver background color
        /// </summary>
        public static string DanceLinesBackgroundColor
        {
            get
            {
                return danceLinesBackgroundColor;
            }
            set
            {
                danceLinesBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum red color level (true color)
        /// </summary>
        public static int DanceLinesMinimumRedColorLevel
        {
            get
            {
                return danceLinesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                danceLinesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum green color level (true color)
        /// </summary>
        public static int DanceLinesMinimumGreenColorLevel
        {
            get
            {
                return danceLinesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                danceLinesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum blue color level (true color)
        /// </summary>
        public static int DanceLinesMinimumBlueColorLevel
        {
            get
            {
                return danceLinesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                danceLinesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int DanceLinesMinimumColorLevel
        {
            get
            {
                return danceLinesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                danceLinesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum red color level (true color)
        /// </summary>
        public static int DanceLinesMaximumRedColorLevel
        {
            get
            {
                return danceLinesMaximumRedColorLevel;
            }
            set
            {
                if (value <= danceLinesMinimumRedColorLevel)
                    value = danceLinesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                danceLinesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum green color level (true color)
        /// </summary>
        public static int DanceLinesMaximumGreenColorLevel
        {
            get
            {
                return danceLinesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= danceLinesMinimumGreenColorLevel)
                    value = danceLinesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                danceLinesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum blue color level (true color)
        /// </summary>
        public static int DanceLinesMaximumBlueColorLevel
        {
            get
            {
                return danceLinesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= danceLinesMinimumBlueColorLevel)
                    value = danceLinesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                danceLinesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int DanceLinesMaximumColorLevel
        {
            get
            {
                return danceLinesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= danceLinesMinimumColorLevel)
                    value = danceLinesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                danceLinesMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for DanceLines
    /// </summary>
    public class DanceLinesDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "DanceLines";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            ColorTools.LoadBackDry(new Color(DanceLinesSettings.DanceLinesBackgroundColor));

            // Draw few lines
            string lineString = !string.IsNullOrWhiteSpace(DanceLinesSettings.DanceLinesLineChar) ? DanceLinesSettings.DanceLinesLineChar : "-";
            for (int i = 0; i < ConsoleWrapper.WindowHeight; i++)
            {
                // Draw a randomly-sized line
                string line = new(lineString[0], RandomDriver.Random(ConsoleWrapper.WindowWidth));

                // Select a color
                if (DanceLinesSettings.DanceLinesTrueColor)
                {
                    int RedColorNum = RandomDriver.Random(DanceLinesSettings.DanceLinesMinimumRedColorLevel, DanceLinesSettings.DanceLinesMaximumRedColorLevel);
                    int GreenColorNum = RandomDriver.Random(DanceLinesSettings.DanceLinesMinimumGreenColorLevel, DanceLinesSettings.DanceLinesMaximumGreenColorLevel);
                    int BlueColorNum = RandomDriver.Random(DanceLinesSettings.DanceLinesMinimumBlueColorLevel, DanceLinesSettings.DanceLinesMaximumBlueColorLevel);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                    var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                    ColorTools.SetConsoleColor(ColorStorage);
                }
                else
                {
                    int color = RandomDriver.Random(DanceLinesSettings.DanceLinesMinimumColorLevel, DanceLinesSettings.DanceLinesMaximumColorLevel);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                    ColorTools.SetConsoleColor(new Color(color));
                }

                // Now, draw a line
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got top position ({0})", i);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ConsoleWrapper.SetCursorPosition(0, i);
                    ConsoleWrapper.Write(line);
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(DanceLinesSettings.DanceLinesDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
