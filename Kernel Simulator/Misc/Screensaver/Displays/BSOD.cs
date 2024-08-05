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

using KS.Misc.Writers.DebugWriters;
using KS.Misc.Screensaver;
using Terminaux.Base;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for BSOD
    /// </summary>
    public static class BSODSettings
    {
        private static int bsodDelay = 10000;

        /// <summary>
        /// [BSOD] How many beats per minute to wait before making the next write?
        /// </summary>
        public static int BSODDelay
        {
            get
            {
                return bsodDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                bsodDelay = value;
            }
        }
    }

    /// <summary>
    /// Display code for BSOD
    /// </summary>
    public class BSODDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.BSOD.BSODSettings BSODSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BSOD";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
            BSODSettingsInstance = new Animations.BSOD.BSODSettings()
            {
                BSODDelay = BSODSettings.BSODDelay
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.BSOD.BSOD.Simulate(BSODSettingsInstance);

    }
}
