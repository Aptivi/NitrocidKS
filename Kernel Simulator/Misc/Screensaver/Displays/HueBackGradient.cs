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

using Terminaux.Writer.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using System;
using System.Text;
using Terminaux.Colors;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for HueBackGradient
    /// </summary>
    public static class HueBackGradientSettings
    {
        private static int hueBackGradientDelay = 50;
        private static int hueBackGradientSaturation = 100;
        private static int hueBackGradientLuminance = 50;

        /// <summary>
        /// [HueBackGradient] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int HueBackGradientDelay
        {
            get
            {
                return hueBackGradientDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                hueBackGradientDelay = value;
            }
        }
        /// <summary>
        /// [HueBackGradient] How intense is the color?
        /// </summary>
        public static int HueBackGradientSaturation
        {
            get
            {
                return hueBackGradientSaturation;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                if (value > 100)
                    value = 100;
                hueBackGradientSaturation = value;
            }
        }
        /// <summary>
        /// [HueBackGradient] How light is the color?
        /// </summary>
        public static int HueBackGradientLuminance
        {
            get
            {
                return hueBackGradientLuminance;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                if (value > 100)
                    value = 100;
                hueBackGradientLuminance = value;
            }
        }
    }

    /// <summary>
    /// Display code for HueBackGradient
    /// </summary>
    public class HueBackGradientDisplay : BaseScreensaver, IScreensaver
    {

        // Hue angle is in degrees and not radians
        private static int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "HueBackGradient";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            currentHueAngle = 0;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Prepare the color
            var color = new Color($"hsl:{currentHueAngle};{HueBackGradientSettings.HueBackGradientSaturation};{HueBackGradientSettings.HueBackGradientLuminance}");
            var hsl = ConversionTools.ToHsl(color.RGB);
            var reverseColor = new Color($"hsl:{hsl.ReverseHueWhole};{HueBackGradientSettings.HueBackGradientSaturation};{HueBackGradientSettings.HueBackGradientLuminance}");

            // Set thresholds for color ramp
            int RampFrameSpaces = ConsoleWrapper.WindowWidth;
            int RampColorRedThreshold = color.RGB.R - reverseColor.RGB.R;
            int RampColorGreenThreshold = color.RGB.G - reverseColor.RGB.G;
            int RampColorBlueThreshold = color.RGB.B - reverseColor.RGB.B;
            double RampColorRedSteps = RampColorRedThreshold / RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / RampFrameSpaces;
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2})", RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3})", RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);

            // Set the current colors
            double RampCurrentColorRed = color.RGB.R;
            double RampCurrentColorGreen = color.RGB.G;
            double RampCurrentColorBlue = color.RGB.B;

            // Fill the entire screen
            StringBuilder gradientBuilder = new();
            for (int x = 0; x < ConsoleWrapper.WindowWidth; x++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                // Write the background gradient!
                var RampCurrentColorInstance = new Color($"{Convert.ToInt32(RampCurrentColorRed)};{Convert.ToInt32(RampCurrentColorGreen)};{Convert.ToInt32(RampCurrentColorBlue)}");
                for (int y = 0; y < ConsoleWrapper.WindowHeight; y++)
                    gradientBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1)}{RampCurrentColorInstance.VTSequenceBackgroundTrueColor} ");

                // Change the colors
                RampCurrentColorRed -= RampColorRedSteps;
                RampCurrentColorGreen -= RampColorGreenSteps;
                RampCurrentColorBlue -= RampColorBlueSteps;
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);
            }
            TextWriterRaw.WritePlain(gradientBuilder.ToString(), false);

            // Set the hue angle
            ThreadManager.SleepNoBlock(HueBackGradientSettings.HueBackGradientDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            currentHueAngle++;
            if (currentHueAngle > 360)
                currentHueAngle = 0;

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}
