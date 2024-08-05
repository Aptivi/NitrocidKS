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
using Terminaux.Writer.ConsoleWriters;
using KS.Misc.Reflection;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for GradientRot
    /// </summary>
    public static class GradientRotSettings
    {
        private static int gradientRotDelay = 10;
        private static int gradientRotNextRampDelay = 250;
        private static int gradientRotMinimumRedColorLevelStart = 0;
        private static int gradientRotMinimumGreenColorLevelStart = 0;
        private static int gradientRotMinimumBlueColorLevelStart = 0;
        private static int gradientRotMaximumRedColorLevelStart = 255;
        private static int gradientRotMaximumGreenColorLevelStart = 255;
        private static int gradientRotMaximumBlueColorLevelStart = 255;
        private static int gradientRotMinimumRedColorLevelEnd = 0;
        private static int gradientRotMinimumGreenColorLevelEnd = 0;
        private static int gradientRotMinimumBlueColorLevelEnd = 0;
        private static int gradientRotMaximumRedColorLevelEnd = 255;
        private static int gradientRotMaximumGreenColorLevelEnd = 255;
        private static int gradientRotMaximumBlueColorLevelEnd = 255;

        /// <summary>
        /// [GradientRot] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int GradientRotDelay
        {
            get
            {
                return gradientRotDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                gradientRotDelay = value;
            }
        }
        /// <summary>
        /// [GradientRot] How many milliseconds to wait before rotting the next screen?
        /// </summary>
        public static int GradientRotNextRampDelay
        {
            get
            {
                return gradientRotNextRampDelay;
            }
            set
            {
                if (value <= 0)
                    value = 250;
                gradientRotNextRampDelay = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum red color level (true color - start)
        /// </summary>
        public static int GradientRotMinimumRedColorLevelStart
        {
            get
            {
                return gradientRotMinimumRedColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum green color level (true color - start)
        /// </summary>
        public static int GradientRotMinimumGreenColorLevelStart
        {
            get
            {
                return gradientRotMinimumGreenColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum blue color level (true color - start)
        /// </summary>
        public static int GradientRotMinimumBlueColorLevelStart
        {
            get
            {
                return gradientRotMinimumBlueColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum red color level (true color - start)
        /// </summary>
        public static int GradientRotMaximumRedColorLevelStart
        {
            get
            {
                return gradientRotMaximumRedColorLevelStart;
            }
            set
            {
                if (value <= gradientRotMinimumRedColorLevelStart)
                    value = gradientRotMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                gradientRotMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum green color level (true color - start)
        /// </summary>
        public static int GradientRotMaximumGreenColorLevelStart
        {
            get
            {
                return gradientRotMaximumGreenColorLevelStart;
            }
            set
            {
                if (value <= gradientRotMinimumGreenColorLevelStart)
                    value = gradientRotMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                gradientRotMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum blue color level (true color - start)
        /// </summary>
        public static int GradientRotMaximumBlueColorLevelStart
        {
            get
            {
                return gradientRotMaximumBlueColorLevelStart;
            }
            set
            {
                if (value <= gradientRotMinimumBlueColorLevelStart)
                    value = gradientRotMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                gradientRotMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum red color level (true color - end)
        /// </summary>
        public static int GradientRotMinimumRedColorLevelEnd
        {
            get
            {
                return gradientRotMinimumRedColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum green color level (true color - end)
        /// </summary>
        public static int GradientRotMinimumGreenColorLevelEnd
        {
            get
            {
                return gradientRotMinimumGreenColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum blue color level (true color - end)
        /// </summary>
        public static int GradientRotMinimumBlueColorLevelEnd
        {
            get
            {
                return gradientRotMinimumBlueColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum red color level (true color - end)
        /// </summary>
        public static int GradientRotMaximumRedColorLevelEnd
        {
            get
            {
                return gradientRotMaximumRedColorLevelEnd;
            }
            set
            {
                if (value <= gradientRotMinimumRedColorLevelEnd)
                    value = gradientRotMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                gradientRotMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum green color level (true color - end)
        /// </summary>
        public static int GradientRotMaximumGreenColorLevelEnd
        {
            get
            {
                return gradientRotMaximumGreenColorLevelEnd;
            }
            set
            {
                if (value <= gradientRotMinimumGreenColorLevelEnd)
                    value = gradientRotMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                gradientRotMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum blue color level (true color - end)
        /// </summary>
        public static int GradientRotMaximumBlueColorLevelEnd
        {
            get
            {
                return gradientRotMaximumBlueColorLevelEnd;
            }
            set
            {
                if (value <= gradientRotMinimumBlueColorLevelEnd)
                    value = gradientRotMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                gradientRotMaximumBlueColorLevelEnd = value;
            }
        }
    }

    /// <summary>
    /// Display code for GradientRot
    /// </summary>
    public class GradientRotDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "GradientRot";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a color range for the ramp
            int RedColorNumFrom = RandomDriver.Random(GradientRotSettings.GradientRotMinimumRedColorLevelStart, GradientRotSettings.GradientRotMaximumRedColorLevelStart);
            int GreenColorNumFrom = RandomDriver.Random(GradientRotSettings.GradientRotMinimumGreenColorLevelStart, GradientRotSettings.GradientRotMaximumGreenColorLevelStart);
            int BlueColorNumFrom = RandomDriver.Random(GradientRotSettings.GradientRotMinimumBlueColorLevelStart, GradientRotSettings.GradientRotMaximumBlueColorLevelStart);
            int RedColorNumTo = RandomDriver.Random(GradientRotSettings.GradientRotMinimumRedColorLevelEnd, GradientRotSettings.GradientRotMaximumRedColorLevelEnd);
            int GreenColorNumTo = RandomDriver.Random(GradientRotSettings.GradientRotMinimumGreenColorLevelEnd, GradientRotSettings.GradientRotMaximumGreenColorLevelEnd);
            int BlueColorNumTo = RandomDriver.Random(GradientRotSettings.GradientRotMinimumBlueColorLevelEnd, GradientRotSettings.GradientRotMaximumBlueColorLevelEnd);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo);

            // Set thresholds for color ramp
            int RampFrameSpaces = ConsoleWrapper.WindowWidth;
            int RampColorRedThreshold = RedColorNumFrom - RedColorNumTo;
            int RampColorGreenThreshold = GreenColorNumFrom - GreenColorNumTo;
            int RampColorBlueThreshold = BlueColorNumFrom - BlueColorNumTo;
            double RampColorRedSteps = RampColorRedThreshold / (double)RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / (double)RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / (double)RampFrameSpaces;
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2})", RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3})", RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);

            // Set the current colors
            double RampCurrentColorRed = RedColorNumFrom;
            double RampCurrentColorGreen = GreenColorNumFrom;
            double RampCurrentColorBlue = BlueColorNumFrom;

            // Set the console color and fill the ramp!
            while (
                Convert.ToInt32(RampCurrentColorRed) != RedColorNumTo &&
                Convert.ToInt32(RampCurrentColorGreen) != GreenColorNumTo &&
                Convert.ToInt32(RampCurrentColorBlue) != BlueColorNumTo
            )
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                // Populate the variables for sub-gradients
                int RampSubgradientRedColorNumFrom = RedColorNumFrom;
                int RampSubgradientGreenColorNumFrom = GreenColorNumFrom;
                int RampSubgradientBlueColorNumFrom = BlueColorNumFrom;
                int RampSubgradientRedColorNumTo = (int)Math.Round(RampCurrentColorRed);
                int RampSubgradientGreenColorNumTo = (int)Math.Round(RampCurrentColorGreen);
                int RampSubgradientBlueColorNumTo = (int)Math.Round(RampCurrentColorBlue);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got subgradient color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RampSubgradientRedColorNumFrom, RampSubgradientGreenColorNumFrom, RampSubgradientBlueColorNumFrom, RampSubgradientRedColorNumTo, RampSubgradientGreenColorNumTo, RampSubgradientBlueColorNumTo);

                // Set the sub-gradient current colors
                double RampSubgradientCurrentColorRed = RampSubgradientRedColorNumFrom;
                double RampSubgradientCurrentColorGreen = RampSubgradientGreenColorNumFrom;
                double RampSubgradientCurrentColorBlue = RampSubgradientBlueColorNumFrom;
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got subgradient current colors (R;G;B: {0};{1};{2})", RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue);

                // Set the sub-gradient thresholds
                int RampSubgradientColorRedThreshold = RampSubgradientRedColorNumFrom - RampSubgradientRedColorNumTo;
                int RampSubgradientColorGreenThreshold = RampSubgradientGreenColorNumFrom - RampSubgradientGreenColorNumTo;
                int RampSubgradientColorBlueThreshold = RampSubgradientBlueColorNumFrom - RampSubgradientBlueColorNumTo;
                double RampSubgradientColorRedSteps = RampSubgradientColorRedThreshold / (double)RampFrameSpaces;
                double RampSubgradientColorGreenSteps = RampSubgradientColorGreenThreshold / (double)RampFrameSpaces;
                double RampSubgradientColorBlueSteps = RampSubgradientColorBlueThreshold / (double)RampFrameSpaces;
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Set subgradient thresholds (RGB: {0};{1};{2})", RampSubgradientColorRedThreshold, RampSubgradientColorGreenThreshold, RampSubgradientColorBlueThreshold);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces for subgradient (RGB: {1};{2};{3})", RampFrameSpaces, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps);

                // Make a new instance
                var RampSubgradientCurrentColorInstance = new Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}");
                ColorTools.SetConsoleColorDry(RampSubgradientCurrentColorInstance, true);

                // Try to fill the ramp
                int RampSubgradientStepsMade = 0;
                int RampCurrentPositionLeft = 0;
                while (RampSubgradientStepsMade != RampFrameSpaces)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;

                    // Fill the entire screen
                    for (int y = 0; y < ConsoleWrapper.WindowHeight; y++)
                        TextWriterWhereColor.WriteWhere(" ", RampCurrentPositionLeft, y);

                    // Update left position
                    RampCurrentPositionLeft = ConsoleWrapper.CursorLeft;
                    RampSubgradientStepsMade += 1;

                    // Change the colors
                    RampSubgradientCurrentColorRed -= RampSubgradientColorRedSteps;
                    RampSubgradientCurrentColorGreen -= RampSubgradientColorGreenSteps;
                    RampSubgradientCurrentColorBlue -= RampSubgradientColorBlueSteps;
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got new subgradient current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps);

                    // Check the values to make sure we don't go below zero
                    if (RampSubgradientCurrentColorRed < 0)
                    {
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "RampSubgradientCurrentColorRed is less than 0! Setting...");
                        RampSubgradientCurrentColorRed = 0;
                    }
                    if (RampSubgradientCurrentColorGreen < 0)
                    {
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "RampSubgradientCurrentColorGreen is less than 0! Setting...");
                        RampSubgradientCurrentColorGreen = 0;
                    }
                    if (RampSubgradientCurrentColorBlue < 0)
                    {
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "RampSubgradientCurrentColorBlue is less than 0! Setting...");
                        RampSubgradientCurrentColorBlue = 0;
                    }
                    RampSubgradientCurrentColorInstance = new Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}");
                    ColorTools.SetConsoleColorDry(RampSubgradientCurrentColorInstance, true);
                }

                // Change the colors
                RampCurrentColorRed -= RampColorRedSteps;
                RampCurrentColorGreen -= RampColorGreenSteps;
                RampCurrentColorBlue -= RampColorBlueSteps;

                // Check the values to make sure we don't go below zero
                if (RampCurrentColorRed < 0)
                {
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "RampCurrentColorRed is less than 0! Setting...");
                    RampCurrentColorRed = 0;
                }
                if (RampCurrentColorGreen < 0)
                {
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "RampCurrentColorGreen is less than 0! Setting...");
                    RampCurrentColorGreen = 0;
                }
                if (RampCurrentColorBlue < 0)
                {
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "RampCurrentColorBlue is less than 0! Setting...");
                    RampCurrentColorBlue = 0;
                }
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);

                // Delay writing
                RampCurrentPositionLeft = 0;
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", RampCurrentPositionLeft);
                ThreadManager.SleepNoBlock(GradientRotSettings.GradientRotDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Clear the scene
            ThreadManager.SleepNoBlock(GradientRotSettings.GradientRotNextRampDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}
