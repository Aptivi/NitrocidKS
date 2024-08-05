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

using KS.Misc.Animations.Glitch;
using KS.Misc.Reflection;
using KS.Misc.Screensaver;
using KS.Misc.Screensaver.Displays;
using KS.Misc.Threading;
using Terminaux.Base;

namespace KS.Misc.Animations.BSOD.Simulations
{
    internal class VGA : BaseBSOD
    {
        public override void Simulate()
        {
            int millisecondsElapsed = 0;
            while (millisecondsElapsed < Screensaver.Displays.BSODSettings.BSODDelay || !ConsoleResizeHandler.WasResized(false))
            {
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    // Select random position to cover
                    int CoverX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                    int CoverY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);

                    // Glitch!
                    Glitch.Glitch.GlitchAt(CoverX, CoverY);
                }
                else
                {
                    // We're resizing.
                    ConsoleWrapper.CursorVisible = false;
                    break;
                }
                ThreadManager.SleepNoBlock(Screensaver.Displays.GlitchSettings.GlitchDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                millisecondsElapsed += Screensaver.Displays.GlitchSettings.GlitchDelay;
            }
        }
    }
}
