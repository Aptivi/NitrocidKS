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

using KS.Misc.Threading;
using KS.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Bloom
    /// </summary>
    public static class BloomSettings
    {
        private static int bloomDelay = 50;
        private static bool bloomDarkColors;
        private static int bloomSteps = 100;

        /// <summary>
        /// [Bloom] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BloomDelay
        {
            get
            {
                return bloomDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                bloomDelay = value;
            }
        }
        /// <summary>
        /// [Bloom] Whether to use dark colors or not
        /// </summary>
        public static bool BloomDarkColors
        {
            get
            {
                return bloomDarkColors;
            }
            set
            {
                bloomDarkColors = value;
            }
        }
        /// <summary>
        /// [Bloom] How many color steps for transitioning between two colors?
        /// </summary>
        public static int BloomSteps
        {
            get
            {
                return bloomSteps;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                bloomSteps = value;
            }
        }
    }

    /// <summary>
    /// Display code for Bloom
    /// </summary>
    public class BloomDisplay : BaseScreensaver, IScreensaver
    {

        private Color nextColor;
        private Color currentColor;

        private static int MaxLevel =>
            BloomSettings.BloomDarkColors ? 32 : 255;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Bloom";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            nextColor = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            currentColor = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Prepare the colors
            int steps = BloomSettings.BloomSteps;
            double thresholdR = (currentColor.RGB.R - nextColor.RGB.R) / (double)steps;
            double thresholdG = (currentColor.RGB.G - nextColor.RGB.G) / (double)steps;
            double thresholdB = (currentColor.RGB.B - nextColor.RGB.B) / (double)steps;

            // Now, transition from black to the target color
            double currentR = currentColor.RGB.R;
            double currentG = currentColor.RGB.G;
            double currentB = currentColor.RGB.B;
            for (int currentStep = 1; currentStep <= steps; currentStep++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                // Add the values according to the threshold
                currentR -= thresholdR;
                currentG -= thresholdG;
                currentB -= thresholdB;

                // Now, make a color and fill the console with it
                Color col = new((int)currentR, (int)currentG, (int)currentB);
                ColorTools.LoadBackDry(col);

                // Sleep
                ThreadManager.SleepNoBlock(BloomSettings.BloomDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Generate new colors
            currentColor = nextColor;
            nextColor = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            ThreadManager.SleepNoBlock(BloomSettings.BloomDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}
