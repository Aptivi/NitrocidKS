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

using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using Terminaux.Base;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Spin
    /// </summary>
    public static class SpinSettings
    {
        private static int spinDelay = 10;

        /// <summary>
        /// [Spin] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SpinDelay
        {
            get
            {
                return spinDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                spinDelay = value;
            }
        }
    }

    /// <summary>
    /// Display code for Spin
    /// </summary>
    public class SpinDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.Spin.SpinSettings SpinSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Spin";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
            SpinSettingsInstance = new Animations.Spin.SpinSettings()
            {
                SpinDelay = SpinSettings.SpinDelay
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Animations.Spin.Spin.Simulate(SpinSettingsInstance);
            ThreadManager.SleepNoBlock(SpinSettings.SpinDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}