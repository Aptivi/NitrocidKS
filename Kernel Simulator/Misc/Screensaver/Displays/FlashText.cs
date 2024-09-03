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

using System;
using Terminaux.Writer.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;
using KS.Misc.Reflection;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for FlashText
    /// </summary>
    public static class FlashTextSettings
    {
        private static bool _flashText255Colors;
        private static bool _flashTextTrueColor = true;
        private static int _flashTextDelay = 20;
        private static string _flashTextWrite = "Kernel Simulator";
        private static string _flashTextBackgroundColor = new Color(ConsoleColor.Black).PlainSequence;
        private static int _flashTextMinimumRedColorLevel = 0;
        private static int _flashTextMinimumGreenColorLevel = 0;
        private static int _flashTextMinimumBlueColorLevel = 0;
        private static int _flashTextMinimumColorLevel = 0;
        private static int _flashTextMaximumRedColorLevel = 255;
        private static int _flashTextMaximumGreenColorLevel = 255;
        private static int _flashTextMaximumBlueColorLevel = 255;
        private static int _flashTextMaximumColorLevel = 0;

        /// <summary>
        /// [FlashText] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool FlashText255Colors
        {
            get
            {
                return _flashText255Colors;
            }
            set
            {
                _flashText255Colors = value;
            }
        }
        /// <summary>
        /// [FlashText] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool FlashTextTrueColor
        {
            get
            {
                return _flashTextTrueColor;
            }
            set
            {
                _flashTextTrueColor = value;
            }
        }
        /// <summary>
        /// [FlashText] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FlashTextDelay
        {
            get
            {
                return _flashTextDelay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                _flashTextDelay = value;
            }
        }
        /// <summary>
        /// [FlashText] Text for FlashText. Shorter is better.
        /// </summary>
        public static string FlashTextWrite
        {
            get
            {
                return _flashTextWrite;
            }
            set
            {
                _flashTextWrite = value;
            }
        }
        /// <summary>
        /// [FlashText] Screensaver background color
        /// </summary>
        public static string FlashTextBackgroundColor
        {
            get
            {
                return _flashTextBackgroundColor;
            }
            set
            {
                _flashTextBackgroundColor = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum red color level (true color)
        /// </summary>
        public static int FlashTextMinimumRedColorLevel
        {
            get
            {
                return _flashTextMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _flashTextMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum green color level (true color)
        /// </summary>
        public static int FlashTextMinimumGreenColorLevel
        {
            get
            {
                return _flashTextMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _flashTextMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum blue color level (true color)
        /// </summary>
        public static int FlashTextMinimumBlueColorLevel
        {
            get
            {
                return _flashTextMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _flashTextMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FlashTextMinimumColorLevel
        {
            get
            {
                return _flashTextMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = _flashText255Colors | _flashTextTrueColor ? 255 : 15;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _flashTextMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum red color level (true color)
        /// </summary>
        public static int FlashTextMaximumRedColorLevel
        {
            get
            {
                return _flashTextMaximumRedColorLevel;
            }
            set
            {
                if (value <= _flashTextMinimumRedColorLevel)
                    value = _flashTextMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _flashTextMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum green color level (true color)
        /// </summary>
        public static int FlashTextMaximumGreenColorLevel
        {
            get
            {
                return _flashTextMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _flashTextMinimumGreenColorLevel)
                    value = _flashTextMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _flashTextMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum blue color level (true color)
        /// </summary>
        public static int FlashTextMaximumBlueColorLevel
        {
            get
            {
                return _flashTextMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _flashTextMinimumBlueColorLevel)
                    value = _flashTextMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _flashTextMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FlashTextMaximumColorLevel
        {
            get
            {
                return _flashTextMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = _flashText255Colors | _flashTextTrueColor ? 255 : 15;
                if (value <= _flashTextMinimumColorLevel)
                    value = _flashTextMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _flashTextMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for FlashText
    /// </summary>
    public class FlashTextDisplay : BaseScreensaver, IScreensaver
    {

        private int Left, Top;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "FlashText";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBackDry(new Color(FlashTextSettings.FlashTextBackgroundColor));
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);

            // Select position
            Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth - FlashTextSettings.FlashTextWrite.Length);
            Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Make two delay halves to make up one half for screen with text and one half for screen with no text to make a flashing effect
            int HalfDelay = (int)Math.Round(FlashTextSettings.FlashTextDelay / 2d);

            // Make a flashing text
            if (FlashTextSettings.FlashTextTrueColor)
            {
                int RedColorNum = RandomDriver.Random(FlashTextSettings.FlashTextMinimumRedColorLevel, FlashTextSettings.FlashTextMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(FlashTextSettings.FlashTextMinimumGreenColorLevel, FlashTextSettings.FlashTextMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(FlashTextSettings.FlashTextMinimumBlueColorLevel, FlashTextSettings.FlashTextMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    TextWriterWhereColor.WriteWhereColorBack(FlashTextSettings.FlashTextWrite, Left, Top, true, ColorStorage, FlashTextSettings.FlashTextBackgroundColor);
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(FlashTextSettings.FlashTextMinimumColorLevel, FlashTextSettings.FlashTextMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    TextWriterWhereColor.WriteWhereColorBack(FlashTextSettings.FlashTextWrite, Left, Top, true, new Color(ColorNum), FlashTextSettings.FlashTextBackgroundColor);
                }
            }
            ThreadManager.SleepNoBlock(HalfDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            ThreadManager.SleepNoBlock(HalfDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}