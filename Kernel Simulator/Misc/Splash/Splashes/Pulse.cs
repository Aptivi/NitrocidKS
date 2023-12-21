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

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Misc.Animations.Pulse;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;

namespace KS.Misc.Splash.Splashes
{
    class SplashPulse : ISplash
    {

        // Standalone splash information
        public string SplashName => "Pulse";

        private SplashInfo Info => SplashManager.Splashes[SplashName];

        // Property implementations
        public bool SplashClosing { get; set; }

        public bool SplashDisplaysProgress => Info.DisplaysProgress;

        // Pulse-specific variables
        internal PulseSettings PulseSettings = new()
        {
            PulseDelay = 50,
            PulseMaxSteps = 30,
            PulseMinimumRedColorLevel = 0,
            PulseMinimumGreenColorLevel = 0,
            PulseMinimumBlueColorLevel = 0,
            PulseMaximumRedColorLevel = 255,
            PulseMaximumGreenColorLevel = 255,
            PulseMaximumBlueColorLevel = 255
        };
        internal Random RandomDriver;

        // Actual logic
        public void Opening()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Splash opening. Clearing console...");
            Console.BackgroundColor = ConsoleColor.Black;
            ConsoleWrapper.Clear();
            RandomDriver = new Random();
            PulseSettings.RandomDriver = RandomDriver;
        }

        public void Display()
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Splash displaying.");

                // Loop until we got a closing notification
                while (!SplashClosing)
                    Pulse.Simulate(PulseSettings);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Splash done.");
            }
        }

        public void Closing()
        {
            SplashClosing = true;
            DebugWriter.Wdbg(DebugLevel.I, "Splash closing. Clearing console...");
            KernelColorTools.SetConsoleColor(KernelColorTools.BackgroundColor, true);
            ConsoleWrapper.Clear();
        }

        public void Report(int Progress, string ProgressReport, params object[] Vars)
        {
        }

    }
}