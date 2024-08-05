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
using System;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Lines
    /// </summary>
    public static class LinesSettings
    {
        private static bool _lines255Colors;
        private static bool _linesTrueColor = true;
        private static int _linesDelay = 500;
        private static string _linesLineChar = "-";
        private static string _linesBackgroundColor = new Color(ConsoleColor.Black).PlainSequence;
        private static int _linesMinimumRedColorLevel = 0;
        private static int _linesMinimumGreenColorLevel = 0;
        private static int _linesMinimumBlueColorLevel = 0;
        private static int _linesMinimumColorLevel = 0;
        private static int _linesMaximumRedColorLevel = 255;
        private static int _linesMaximumGreenColorLevel = 255;
        private static int _linesMaximumBlueColorLevel = 255;
        private static int _linesMaximumColorLevel = 255;

        /// <summary>
        /// [Lines] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool Lines255Colors
        {
            get
            {
                return _lines255Colors;
            }
            set
            {
                _lines255Colors = value;
            }
        }
        /// <summary>
        /// [Lines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool LinesTrueColor
        {
            get
            {
                return _linesTrueColor;
            }
            set
            {
                _linesTrueColor = value;
            }
        }
        /// <summary>
        /// [Lines] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int LinesDelay
        {
            get
            {
                return _linesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                _linesDelay = value;
            }
        }
        /// <summary>
        /// [Lines] Line character
        /// </summary>
        public static string LinesLineChar
        {
            get
            {
                return _linesLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                _linesLineChar = value;
            }
        }
        /// <summary>
        /// [Lines] Screensaver background color
        /// </summary>
        public static string LinesBackgroundColor
        {
            get
            {
                return _linesBackgroundColor;
            }
            set
            {
                _linesBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Lines] The minimum red color level (true color)
        /// </summary>
        public static int LinesMinimumRedColorLevel
        {
            get
            {
                return _linesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _linesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The minimum green color level (true color)
        /// </summary>
        public static int LinesMinimumGreenColorLevel
        {
            get
            {
                return _linesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _linesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The minimum blue color level (true color)
        /// </summary>
        public static int LinesMinimumBlueColorLevel
        {
            get
            {
                return _linesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _linesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int LinesMinimumColorLevel
        {
            get
            {
                return _linesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = _lines255Colors | _linesTrueColor ? 255 : 15;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _linesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum red color level (true color)
        /// </summary>
        public static int LinesMaximumRedColorLevel
        {
            get
            {
                return _linesMaximumRedColorLevel;
            }
            set
            {
                if (value <= _linesMinimumRedColorLevel)
                    value = _linesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _linesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum green color level (true color)
        /// </summary>
        public static int LinesMaximumGreenColorLevel
        {
            get
            {
                return _linesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _linesMinimumGreenColorLevel)
                    value = _linesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _linesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum blue color level (true color)
        /// </summary>
        public static int LinesMaximumBlueColorLevel
        {
            get
            {
                return _linesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _linesMinimumBlueColorLevel)
                    value = _linesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _linesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int LinesMaximumColorLevel
        {
            get
            {
                return _linesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = _lines255Colors | _linesTrueColor ? 255 : 15;
                if (value <= _linesMinimumColorLevel)
                    value = _linesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _linesMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for Lines
    /// </summary>
    public class LinesDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Lines";

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

            // Select a color
            if (LinesSettings.LinesTrueColor)
            {
                ColorTools.LoadBackDry(new Color(LinesSettings.LinesBackgroundColor));
                int RedColorNum = RandomDriver.Random(LinesSettings.LinesMinimumRedColorLevel, LinesSettings.LinesMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(LinesSettings.LinesMinimumGreenColorLevel, LinesSettings.LinesMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(LinesSettings.LinesMinimumBlueColorLevel, LinesSettings.LinesMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                ColorTools.SetConsoleColor(ColorStorage);
            }
            else
            {
                ColorTools.LoadBackDry(new Color(LinesSettings.LinesBackgroundColor));
                int color = RandomDriver.Random(LinesSettings.LinesMinimumColorLevel, LinesSettings.LinesMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                ColorTools.SetConsoleColor(new Color(color));
            }

            // Draw a line
            string lineString = !string.IsNullOrWhiteSpace(LinesSettings.LinesLineChar) ? LinesSettings.LinesLineChar : "-";
            string Line = new(lineString[0], ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got top position ({0})", Top);
            if (!ConsoleResizeHandler.WasResized(false))
            {
                ConsoleWrapper.SetCursorPosition(0, Top);
                ConsoleWrapper.WriteLine(Line);
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(LinesSettings.LinesDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
