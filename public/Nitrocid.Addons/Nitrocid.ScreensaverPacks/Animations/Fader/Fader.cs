﻿
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using KS.ConsoleBase;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Animations.Fader
{
    /// <summary>
    /// Fader animation module
    /// </summary>
    public static class Fader
    {

        /// <summary>
        /// Simulates the fading animation
        /// </summary>
        public static void Simulate(FaderSettings Settings)
        {
            int RedColorNum = RandomDriver.Random(Settings.FaderMinimumRedColorLevel, Settings.FaderMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(Settings.FaderMinimumGreenColorLevel, Settings.FaderMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(Settings.FaderMinimumBlueColorLevel, Settings.FaderMaximumBlueColorLevel);
            ConsoleWrapper.CursorVisible = false;

            // Check the text
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            if (Settings.FaderWrite.Length + Left >= ConsoleWrapper.WindowWidth)
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Text length of {0} exceeded window width of {1}.", Settings.FaderWrite.Length + Left, ConsoleWrapper.WindowWidth);
                Left -= Settings.FaderWrite.Length + 1;
            }
            ConsoleWrapper.SetCursorPosition(Left, Top);
            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleExtensions.ClearKeepPosition();

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)Settings.FaderMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.FaderMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.FaderMaxSteps;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

            // Fade in
            int CurrentColorRedIn = 0;
            int CurrentColorGreenIn = 0;
            int CurrentColorBlueIn = 0;
            for (int CurrentStep = Settings.FaderMaxSteps; CurrentStep >= 1; CurrentStep -= 1)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.FaderMaxSteps);
                ThreadManager.SleepNoBlock(Settings.FaderDelay, System.Threading.Thread.CurrentThread);
                CurrentColorRedIn = (int)Math.Round(CurrentColorRedIn + ThresholdRed);
                CurrentColorGreenIn = (int)Math.Round(CurrentColorGreenIn + ThresholdGreen);
                CurrentColorBlueIn = (int)Math.Round(CurrentColorBlueIn + ThresholdBlue);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn);
                if (!ConsoleResizeListener.WasResized(false))
                    TextWriterWhereColor.WriteWhereColorBack(Settings.FaderWrite, Left, Top, true, new Color(CurrentColorRedIn + ";" + CurrentColorGreenIn + ";" + CurrentColorBlueIn), new Color(ConsoleColors.Black));
            }

            // Wait until fade out
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Waiting {0} ms...", Settings.FaderFadeOutDelay);
            ThreadManager.SleepNoBlock(Settings.FaderFadeOutDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Fade out
            for (int CurrentStep = 1; CurrentStep <= Settings.FaderMaxSteps; CurrentStep++)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.FaderMaxSteps);
                ThreadManager.SleepNoBlock(Settings.FaderDelay, System.Threading.Thread.CurrentThread);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                if (!ConsoleResizeListener.WasResized(false))
                    TextWriterWhereColor.WriteWhereColorBack(Settings.FaderWrite, Left, Top, true, new Color(CurrentColorRedOut + ";" + CurrentColorGreenOut + ";" + CurrentColorBlueOut), new Color(ConsoleColors.Black));
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(Settings.FaderDelay, System.Threading.Thread.CurrentThread);
        }

    }
}