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
using System.Collections.Generic;
using System.Linq;
using KS.Misc.Reflection;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using Terminaux.Base;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Lighter
    /// </summary>
    public static class LighterSettings
    {
        private static bool _lighter255Colors;
        private static bool _lighterTrueColor = true;
        private static int _lighterDelay = 100;
        private static int _lighterMaxPositions = 10;
        private static string _lighterBackgroundColor = new Color(ConsoleColor.Black).PlainSequence;
        private static int _lighterMinimumRedColorLevel = 0;
        private static int _lighterMinimumGreenColorLevel = 0;
        private static int _lighterMinimumBlueColorLevel = 0;
        private static int _lighterMinimumColorLevel = 0;
        private static int _lighterMaximumRedColorLevel = 255;
        private static int _lighterMaximumGreenColorLevel = 255;
        private static int _lighterMaximumBlueColorLevel = 255;
        private static int _lighterMaximumColorLevel = 255;

        /// <summary>
        /// [Lighter] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool Lighter255Colors
        {
            get
            {
                return _lighter255Colors;
            }
            set
            {
                _lighter255Colors = value;
            }
        }
        /// <summary>
        /// [Lighter] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool LighterTrueColor
        {
            get
            {
                return _lighterTrueColor;
            }
            set
            {
                _lighterTrueColor = value;
            }
        }
        /// <summary>
        /// [Lighter] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int LighterDelay
        {
            get
            {
                return _lighterDelay;
            }
            set
            {
                _lighterDelay = value;
            }
        }
        /// <summary>
        /// [Lighter] How many positions to write before starting to blacken them?
        /// </summary>
        public static int LighterMaxPositions
        {
            get
            {
                return _lighterMaxPositions;
            }
            set
            {
                _lighterMaxPositions = value;
            }
        }
        /// <summary>
        /// [Lighter] Screensaver background color
        /// </summary>
        public static string LighterBackgroundColor
        {
            get
            {
                return _lighterBackgroundColor;
            }
            set
            {
                _lighterBackgroundColor = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum red color level (true color)
        /// </summary>
        public static int LighterMinimumRedColorLevel
        {
            get
            {
                return _lighterMinimumRedColorLevel;
            }
            set
            {
                _lighterMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum green color level (true color)
        /// </summary>
        public static int LighterMinimumGreenColorLevel
        {
            get
            {
                return _lighterMinimumGreenColorLevel;
            }
            set
            {
                _lighterMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum blue color level (true color)
        /// </summary>
        public static int LighterMinimumBlueColorLevel
        {
            get
            {
                return _lighterMinimumBlueColorLevel;
            }
            set
            {
                _lighterMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int LighterMinimumColorLevel
        {
            get
            {
                return _lighterMinimumColorLevel;
            }
            set
            {
                _lighterMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum red color level (true color)
        /// </summary>
        public static int LighterMaximumRedColorLevel
        {
            get
            {
                return _lighterMaximumRedColorLevel;
            }
            set
            {
                _lighterMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum green color level (true color)
        /// </summary>
        public static int LighterMaximumGreenColorLevel
        {
            get
            {
                return _lighterMaximumGreenColorLevel;
            }
            set
            {
                _lighterMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum blue color level (true color)
        /// </summary>
        public static int LighterMaximumBlueColorLevel
        {
            get
            {
                return _lighterMaximumBlueColorLevel;
            }
            set
            {
                _lighterMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int LighterMaximumColorLevel
        {
            get
            {
                return _lighterMaximumColorLevel;
            }
            set
            {
                _lighterMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for Lighter
    /// </summary>
    public class LighterDisplay : BaseScreensaver, IScreensaver
    {

        private readonly List<Tuple<int, int>> CoveredPositions = [];

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Lighter";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            CoveredPositions.Clear();
            ColorTools.LoadBackDry(new Color(LighterSettings.LighterBackgroundColor));
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a position
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            ConsoleWrapper.SetCursorPosition(Left, Top);
            if (!CoveredPositions.Any(t => t.Item1 == Left & t.Item2 == Top))
            {
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Covering position...");
                CoveredPositions.Add(new Tuple<int, int>(Left, Top));
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Position covered. Covered positions: {0}", CoveredPositions.Count);
            }

            // Select a color and write the space
            if (LighterSettings.LighterTrueColor)
            {
                int RedColorNum = RandomDriver.Random(LighterSettings.LighterMinimumRedColorLevel, LighterSettings.LighterMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(LighterSettings.LighterMinimumGreenColorLevel, LighterSettings.LighterMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(LighterSettings.LighterMinimumBlueColorLevel, LighterSettings.LighterMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ColorTools.SetConsoleColorDry(ColorStorage, true);
                    ConsoleWrapper.Write(" ");
                }
                else
                {
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...");
                    CoveredPositions.Clear();
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(LighterSettings.LighterMinimumColorLevel, LighterSettings.LighterMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ColorTools.SetConsoleColorDry(new Color(ColorNum), true);
                    ConsoleWrapper.Write(" ");
                }
                else
                {
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...");
                    CoveredPositions.Clear();
                }
            }

            // Simulate a trail effect
            if (CoveredPositions.Count == LighterSettings.LighterMaxPositions)
            {
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Covered positions exceeded max positions of {0}", LighterSettings.LighterMaxPositions);
                int WipeLeft = CoveredPositions[0].Item1;
                int WipeTop = CoveredPositions[0].Item2;
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Wiping in {0}, {1}...", WipeLeft, WipeTop);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ConsoleWrapper.SetCursorPosition(WipeLeft, WipeTop);
                    ColorTools.SetConsoleColorDry(new Color(LighterSettings.LighterBackgroundColor), true);
                    ConsoleWrapper.Write(" ");
                    CoveredPositions.RemoveAt(0);
                }
                else
                {
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...");
                    CoveredPositions.Clear();
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(LighterSettings.LighterDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}