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
    /// Settings for HueBack
    /// </summary>
    public static class HueBackSettings
    {
        private static int hueBackDelay = 50;
        private static int hueBackSaturation = 100;
        private static int hueBackLuminance = 50;

        /// <summary>
        /// [HueBack] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int HueBackDelay
        {
            get
            {
                return hueBackDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                hueBackDelay = value;
            }
        }
        /// <summary>
        /// [HueBack] How intense is the color?
        /// </summary>
        public static int HueBackSaturation
        {
            get
            {
                return hueBackSaturation;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                if (value > 100)
                    value = 100;
                hueBackSaturation = value;
            }
        }
        /// <summary>
        /// [HueBack] How light is the color?
        /// </summary>
        public static int HueBackLuminance
        {
            get
            {
                return hueBackLuminance;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                if (value > 100)
                    value = 100;
                hueBackLuminance = value;
            }
        }
    }

    /// <summary>
    /// Display code for HueBack
    /// </summary>
    public class HueBackDisplay : BaseScreensaver, IScreensaver
    {

        // Hue angle is in degrees and not radians
        private static int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "HueBack";

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
            var color = new Color($"hsl:{currentHueAngle};{HueBackSettings.HueBackSaturation};{HueBackSettings.HueBackLuminance}");

            // Now, change the background color accordingly
            ColorTools.LoadBackDry(color);
            ThreadManager.SleepNoBlock(HueBackSettings.HueBackDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            currentHueAngle++;
            if (currentHueAngle > 360)
                currentHueAngle = 0;

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}
