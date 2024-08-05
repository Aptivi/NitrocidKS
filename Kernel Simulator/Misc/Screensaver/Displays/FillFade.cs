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

using System;
using System.Threading;
using Terminaux.Writer.ConsoleWriters;
using KS.Misc.Reflection;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for FillFade
    /// </summary>
    public static class FillFadeSettings
    {
        private static bool fillFadeTrueColor = true;
        private static int fillFadeMinimumRedColorLevel = 0;
        private static int fillFadeMinimumGreenColorLevel = 0;
        private static int fillFadeMinimumBlueColorLevel = 0;
        private static int fillFadeMinimumColorLevel = 0;
        private static int fillFadeMaximumGreenColorLevel = 255;
        private static int fillFadeMaximumBlueColorLevel = 255;
        private static int fillFadeMaximumColorLevel = 255;
        private static int fillFadeMaximumRedColorLevel = 255;

        /// <summary>
        /// [FillFade] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool FillFadeTrueColor
        {
            get
            {
                return fillFadeTrueColor;
            }
            set
            {
                fillFadeTrueColor = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum red color level (true color)
        /// </summary>
        public static int FillFadeMinimumRedColorLevel
        {
            get
            {
                return fillFadeMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fillFadeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum green color level (true color)
        /// </summary>
        public static int FillFadeMinimumGreenColorLevel
        {
            get
            {
                return fillFadeMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fillFadeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum blue color level (true color)
        /// </summary>
        public static int FillFadeMinimumBlueColorLevel
        {
            get
            {
                return fillFadeMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fillFadeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FillFadeMinimumColorLevel
        {
            get
            {
                return fillFadeMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                fillFadeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum red color level (true color)
        /// </summary>
        public static int FillFadeMaximumRedColorLevel
        {
            get
            {
                return fillFadeMaximumRedColorLevel;
            }
            set
            {
                if (value <= fillFadeMinimumRedColorLevel)
                    value = fillFadeMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                fillFadeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum green color level (true color)
        /// </summary>
        public static int FillFadeMaximumGreenColorLevel
        {
            get
            {
                return fillFadeMaximumGreenColorLevel;
            }
            set
            {
                if (value <= fillFadeMinimumGreenColorLevel)
                    value = fillFadeMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                fillFadeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum blue color level (true color)
        /// </summary>
        public static int FillFadeMaximumBlueColorLevel
        {
            get
            {
                return fillFadeMaximumBlueColorLevel;
            }
            set
            {
                if (value <= fillFadeMinimumBlueColorLevel)
                    value = fillFadeMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                fillFadeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FillFadeMaximumColorLevel
        {
            get
            {
                return fillFadeMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= fillFadeMinimumColorLevel)
                    value = fillFadeMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                fillFadeMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for FillFade
    /// </summary>
    public class FillFadeDisplay : BaseScreensaver, IScreensaver
    {

        private bool ColorFilled;
        private Color currentColor = Color.Empty;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "FillFade";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorFilled = false;
            ChangeColor();
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            if (ColorFilled)
                Thread.Sleep(1);
            int EndLeft = ConsoleWrapper.WindowWidth - 1;
            int EndTop = ConsoleWrapper.WindowHeight - 1;
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            bool goAhead = true;
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Dissolving: {0}", ColorFilled);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "End left: {0} | End top: {1}", EndLeft, EndTop);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got left: {0} | Got top: {1}", Left, Top);

            // Fill the color if not filled
            if (!ColorFilled)
            {
                if (ConsoleResizeHandler.WasResized(false))
                {
                    // Refill, because the console is resized
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                    ColorFilled = false;
                    goAhead = false;
                    ColorTools.LoadBack();
                }

                if (goAhead)
                {
                    if (ConsoleWrapper.CursorLeft >= EndLeft && ConsoleWrapper.CursorTop >= EndTop)
                    {
                        ColorTools.SetConsoleColorDry(currentColor, true);
                        TextWriterRaw.WritePlain(" ", false);
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're now dissolving... L: {0} = {1} | T: {2} = {3}", ConsoleWrapper.CursorLeft, EndLeft, ConsoleWrapper.CursorTop, EndTop);
                        ColorFilled = true;
                    }
                    else
                    {
                        ColorTools.SetConsoleColorDry(currentColor, true);
                        TextWriterRaw.WritePlain(" ", false);
                    }
                }
            }
            else
            {
                // Now, fade out
                int maxSteps = 30;
                for (int CurrentStep = 1; CurrentStep <= maxSteps; CurrentStep++)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;

                    // Set the thresholds
                    int RedColorNum = currentColor.RGB.R;
                    int GreenColorNum = currentColor.RGB.G;
                    int BlueColorNum = currentColor.RGB.B;
                    double ThresholdRed = RedColorNum / (double)maxSteps;
                    double ThresholdGreen = GreenColorNum / (double)maxSteps;
                    double ThresholdBlue = BlueColorNum / (double)maxSteps;
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

                    // Fade out
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, maxSteps);
                    ThreadManager.SleepNoBlock(50, Thread.CurrentThread);
                    int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                    int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                    int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                    if (!ConsoleResizeHandler.WasResized(false))
                        ColorTools.LoadBackDry(new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut));
                }
                ChangeColor();
                ColorFilled = false;
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

        private void ChangeColor()
        {
            currentColor = Color.Empty;
            if (FillFadeSettings.FillFadeTrueColor)
            {
                int RedColorNum = RandomDriver.Random(FillFadeSettings.FillFadeMinimumRedColorLevel, FillFadeSettings.FillFadeMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(FillFadeSettings.FillFadeMinimumGreenColorLevel, FillFadeSettings.FillFadeMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(FillFadeSettings.FillFadeMinimumBlueColorLevel, FillFadeSettings.FillFadeMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                currentColor = new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}");
            }
            else
            {
                int ColorNum = RandomDriver.Random(FillFadeSettings.FillFadeMinimumColorLevel, FillFadeSettings.FillFadeMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                currentColor = new Color(ColorNum);
            }
        }

    }
}
