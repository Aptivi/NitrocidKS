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

using Terminaux.Colors;
using System;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Reflection;
using KS.Misc.Screensaver;
using Terminaux.Writer.FancyWriters;
using KS.Misc.Threading;
using Terminaux.Base;

namespace KS.Misc.Animations.SquareCorner
{
    /// <summary>
    /// SquareCorner animation module
    /// </summary>
    public static class SquareCorner
    {

        private static SquareCornerDirection cornerDirection;

        /// <summary>
        /// Simulates the square corner animation
        /// </summary>
        public static void Simulate(SquareCornerSettings Settings)
        {
            int RedColorNum = RandomDriver.Random(Settings.SquareCornerMinimumRedColorLevel, Settings.SquareCornerMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(Settings.SquareCornerMinimumGreenColorLevel, Settings.SquareCornerMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(Settings.SquareCornerMinimumBlueColorLevel, Settings.SquareCornerMaximumBlueColorLevel);
            ConsoleWrapper.CursorVisible = false;

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)Settings.SquareCornerMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.SquareCornerMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.SquareCornerMaxSteps;
            DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

            // Determine direction based on value
            cornerDirection = (SquareCornerDirection)RandomDriver.Random(3);
            int left = 2;
            int top = 0;
            int width = 6;
            int height = 3;
            switch (cornerDirection)
            {
                case SquareCornerDirection.UpperLeft:
                    left = 2;
                    top = 0;
                    break;
                case SquareCornerDirection.UpperRight:
                    left = ConsoleWrapper.WindowWidth - width - 2;
                    top = 0;
                    break;
                case SquareCornerDirection.LowerLeft:
                    left = 2;
                    top = ConsoleWrapper.WindowHeight - height - 2;
                    break;
                case SquareCornerDirection.LowerRight:
                    left = ConsoleWrapper.WindowWidth - width - 2;
                    top = ConsoleWrapper.WindowHeight - height - 2;
                    break;
            }

            // Fade in
            int CurrentColorRedIn = 0;
            int CurrentColorGreenIn = 0;
            int CurrentColorBlueIn = 0;
            for (int CurrentStep = Settings.SquareCornerMaxSteps; CurrentStep >= 1; CurrentStep -= 1)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.SquareCornerMaxSteps);
                ThreadManager.SleepNoBlock(Settings.SquareCornerDelay, System.Threading.Thread.CurrentThread);
                CurrentColorRedIn = (int)Math.Round(CurrentColorRedIn + ThresholdRed);
                CurrentColorGreenIn = (int)Math.Round(CurrentColorGreenIn + ThresholdGreen);
                CurrentColorBlueIn = (int)Math.Round(CurrentColorBlueIn + ThresholdBlue);
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn);
                BoxColor.WriteBox(left, top, width, height, new Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn));
            }

            // Wait until fade out
            if (!ConsoleResizeHandler.WasResized(false))
            {
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Waiting {0} ms...", Settings.SquareCornerFadeOutDelay);
                ThreadManager.SleepNoBlock(Settings.SquareCornerFadeOutDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Fade out
            for (int CurrentStep = 1; CurrentStep <= Settings.SquareCornerMaxSteps; CurrentStep++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.SquareCornerMaxSteps);
                ThreadManager.SleepNoBlock(Settings.SquareCornerDelay, System.Threading.Thread.CurrentThread);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                BoxColor.WriteBox(left, top, width, height, new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut));
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(Settings.SquareCornerDelay, System.Threading.Thread.CurrentThread);
        }

    }
}
