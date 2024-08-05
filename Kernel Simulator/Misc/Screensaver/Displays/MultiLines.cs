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
using KS.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for MultiLines
    /// </summary>
    public static class MultiLinesSettings
    {
        private static bool multiLinesTrueColor = true;
        private static int multiLinesDelay = 500;
        private static string multiLinesLineChar = "-";
        private static string multiLinesBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private static int multiLinesMinimumRedColorLevel = 0;
        private static int multiLinesMinimumGreenColorLevel = 0;
        private static int multiLinesMinimumBlueColorLevel = 0;
        private static int multiLinesMinimumColorLevel = 0;
        private static int multiLinesMaximumRedColorLevel = 255;
        private static int multiLinesMaximumGreenColorLevel = 255;
        private static int multiLinesMaximumBlueColorLevel = 255;
        private static int multiLinesMaximumColorLevel = 255;

        /// <summary>
        /// [MultiLines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool MultiLinesTrueColor
        {
            get
            {
                return multiLinesTrueColor;
            }
            set
            {
                multiLinesTrueColor = value;
            }
        }
        /// <summary>
        /// [MultiLines] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int MultiLinesDelay
        {
            get
            {
                return multiLinesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                multiLinesDelay = value;
            }
        }
        /// <summary>
        /// [MultiLines] Line character
        /// </summary>
        public static string MultiLinesLineChar
        {
            get
            {
                return multiLinesLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                multiLinesLineChar = value;
            }
        }
        /// <summary>
        /// [MultiLines] Screensaver background color
        /// </summary>
        public static string MultiLinesBackgroundColor
        {
            get
            {
                return multiLinesBackgroundColor;
            }
            set
            {
                multiLinesBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum red color level (true color)
        /// </summary>
        public static int MultiLinesMinimumRedColorLevel
        {
            get
            {
                return multiLinesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                multiLinesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum green color level (true color)
        /// </summary>
        public static int MultiLinesMinimumGreenColorLevel
        {
            get
            {
                return multiLinesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                multiLinesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum blue color level (true color)
        /// </summary>
        public static int MultiLinesMinimumBlueColorLevel
        {
            get
            {
                return multiLinesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                multiLinesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int MultiLinesMinimumColorLevel
        {
            get
            {
                return multiLinesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                multiLinesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum red color level (true color)
        /// </summary>
        public static int MultiLinesMaximumRedColorLevel
        {
            get
            {
                return multiLinesMaximumRedColorLevel;
            }
            set
            {
                if (value <= multiLinesMinimumRedColorLevel)
                    value = multiLinesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                multiLinesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum green color level (true color)
        /// </summary>
        public static int MultiLinesMaximumGreenColorLevel
        {
            get
            {
                return multiLinesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= multiLinesMinimumGreenColorLevel)
                    value = multiLinesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                multiLinesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum blue color level (true color)
        /// </summary>
        public static int MultiLinesMaximumBlueColorLevel
        {
            get
            {
                return multiLinesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= multiLinesMinimumBlueColorLevel)
                    value = multiLinesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                multiLinesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int MultiLinesMaximumColorLevel
        {
            get
            {
                return multiLinesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= multiLinesMinimumColorLevel)
                    value = multiLinesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                multiLinesMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for MultiLines
    /// </summary>
    public class MultiLinesDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "MultiLines";

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
            ColorTools.LoadBackDry(new Color(MultiLinesSettings.MultiLinesBackgroundColor));

            // Draw few lines
            string lineString = !string.IsNullOrWhiteSpace(MultiLinesSettings.MultiLinesLineChar) ? MultiLinesSettings.MultiLinesLineChar : "-";
            string Line = new(lineString[0], ConsoleWrapper.WindowWidth);
            int linesCount = RandomDriver.Random(1, 10);
            for (int i = 0; i < linesCount; i++)
            {
                // Select a color
                if (MultiLinesSettings.MultiLinesTrueColor)
                {
                    int RedColorNum = RandomDriver.Random(MultiLinesSettings.MultiLinesMinimumRedColorLevel, MultiLinesSettings.MultiLinesMaximumRedColorLevel);
                    int GreenColorNum = RandomDriver.Random(MultiLinesSettings.MultiLinesMinimumGreenColorLevel, MultiLinesSettings.MultiLinesMaximumGreenColorLevel);
                    int BlueColorNum = RandomDriver.Random(MultiLinesSettings.MultiLinesMinimumBlueColorLevel, MultiLinesSettings.MultiLinesMaximumBlueColorLevel);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                    var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                    ColorTools.SetConsoleColor(ColorStorage);
                }
                else
                {
                    int color = RandomDriver.Random(MultiLinesSettings.MultiLinesMinimumColorLevel, MultiLinesSettings.MultiLinesMaximumColorLevel);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                    ColorTools.SetConsoleColor(new Color(color));
                }

                // Now, draw a line
                int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got top position ({0})", Top);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ConsoleWrapper.SetCursorPosition(0, Top);
                    ConsoleWrapper.WriteLine(Line);
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(MultiLinesSettings.MultiLinesDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
