﻿//
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
using KS.Misc.Threading;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using System;
using KS.Misc.Reflection;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for FlashColor
    /// </summary>
    public static class FlashColorSettings
    {
        private static bool _flashColor255Colors;
        private static bool _flashColorTrueColor = true;
        private static int _flashColorDelay = 20;
        private static string _flashColorBackgroundColor = new Color(ConsoleColor.Black).PlainSequence;
        private static int _flashColorMinimumRedColorLevel = 0;
        private static int _flashColorMinimumGreenColorLevel = 0;
        private static int _flashColorMinimumBlueColorLevel = 0;
        private static int _flashColorMinimumColorLevel = 0;
        private static int _flashColorMaximumRedColorLevel = 255;
        private static int _flashColorMaximumGreenColorLevel = 255;
        private static int _flashColorMaximumBlueColorLevel = 255;
        private static int _flashColorMaximumColorLevel = 0;

        /// <summary>
        /// [FlashColor] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool FlashColor255Colors
        {
            get
            {
                return _flashColor255Colors;
            }
            set
            {
                _flashColor255Colors = value;
            }
        }
        /// <summary>
        /// [FlashColor] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool FlashColorTrueColor
        {
            get
            {
                return _flashColorTrueColor;
            }
            set
            {
                _flashColorTrueColor = value;
            }
        }
        /// <summary>
        /// [FlashColor] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FlashColorDelay
        {
            get
            {
                return _flashColorDelay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                _flashColorDelay = value;
            }
        }
        /// <summary>
        /// [FlashColor] Screensaver background color
        /// </summary>
        public static string FlashColorBackgroundColor
        {
            get
            {
                return _flashColorBackgroundColor;
            }
            set
            {
                _flashColorBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum red color level (true color)
        /// </summary>
        public static int FlashColorMinimumRedColorLevel
        {
            get
            {
                return _flashColorMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _flashColorMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum green color level (true color)
        /// </summary>
        public static int FlashColorMinimumGreenColorLevel
        {
            get
            {
                return _flashColorMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _flashColorMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum blue color level (true color)
        /// </summary>
        public static int FlashColorMinimumBlueColorLevel
        {
            get
            {
                return _flashColorMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _flashColorMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FlashColorMinimumColorLevel
        {
            get
            {
                return _flashColorMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = _flashColor255Colors | _flashColorTrueColor ? 255 : 15;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _flashColorMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum red color level (true color)
        /// </summary>
        public static int FlashColorMaximumRedColorLevel
        {
            get
            {
                return _flashColorMaximumRedColorLevel;
            }
            set
            {
                if (value <= _flashColorMinimumRedColorLevel)
                    value = _flashColorMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _flashColorMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum green color level (true color)
        /// </summary>
        public static int FlashColorMaximumGreenColorLevel
        {
            get
            {
                return _flashColorMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _flashColorMinimumGreenColorLevel)
                    value = _flashColorMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _flashColorMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum blue color level (true color)
        /// </summary>
        public static int FlashColorMaximumBlueColorLevel
        {
            get
            {
                return _flashColorMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _flashColorMinimumBlueColorLevel)
                    value = _flashColorMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _flashColorMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FlashColorMaximumColorLevel
        {
            get
            {
                return _flashColorMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = _flashColor255Colors | _flashColorTrueColor ? 255 : 15;
                if (value <= _flashColorMinimumColorLevel)
                    value = _flashColorMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _flashColorMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for FlashColor
    /// </summary>
    public class FlashColorDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "FlashColor";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBackDry(new Color(FlashColorSettings.FlashColorBackgroundColor));
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select position
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            ConsoleWrapper.SetCursorPosition(Left, Top);

            // Make a flash color
            if (FlashColorSettings.FlashColorTrueColor)
            {
                int RedColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumRedColorLevel, FlashColorSettings.FlashColorMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumGreenColorLevel, FlashColorSettings.FlashColorMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumBlueColorLevel, FlashColorSettings.FlashColorMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ColorTools.SetConsoleColorDry(ColorStorage, true);
                    ConsoleWrapper.Write(" ");
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumColorLevel, FlashColorSettings.FlashColorMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ColorTools.SetConsoleColorDry(new Color(ColorNum), true);
                    ConsoleWrapper.Write(" ");
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(FlashColorSettings.FlashColorDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}