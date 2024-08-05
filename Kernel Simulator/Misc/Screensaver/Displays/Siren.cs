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

using System.Collections.Generic;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Siren
    /// </summary>
    public static class SirenSettings
    {
        private static int sirenDelay = 500;
        private static string sirenStyle = "Cop";

        /// <summary>
        /// [Siren] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SirenDelay
        {
            get
            {
                return sirenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                sirenDelay = value;
            }
        }

        /// <summary>
        /// [Siren] The siren style
        /// </summary>
        public static string SirenStyle
        {
            get
            {
                return sirenStyle;
            }
            set
            {
                sirenStyle = SirenDisplay.sirens.ContainsKey(value) ? value : "Cop";
            }
        }
    }

    /// <summary>
    /// Display code for Siren
    /// </summary>
    public class SirenDisplay : BaseScreensaver, IScreensaver
    {

        internal readonly static Dictionary<string, Color[]> sirens = new()
        {
            { "Cop",        new Color[] { new(255, 0, 0), new(0, 0, 255) } },
            { "Ambulance",  new Color[] { new(255, 0, 0), new(128, 0, 0) } },
            { "Neon",       new Color[] { new(255, 0, 255), new(0, 255, 255) } }
        };
        private int step = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Siren";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            step = 0;
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get step color array from siren type
            Color[] sirenColors = sirens[SirenSettings.SirenStyle];

            // Step through the color
            step += 1;
            if (step >= sirenColors.Length)
                step = 0;

            // Set color
            ColorTools.LoadBackDry(sirenColors[step]);

            // Delay
            ThreadManager.SleepNoBlock(SirenSettings.SirenDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
