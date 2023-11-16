﻿//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Threading;
using KS.Kernel.Debugging;
using KS.Misc.Splash;
using Nitrocid.ScreensaverPacks.Animations.FaderBack;

namespace Nitrocid.ScreensaverPacks.Splashes
{
    class SplashFaderBack : BaseSplash, ISplash
    {

        // Standalone splash information
        public override string SplashName => "FaderBack";

        internal FaderBackSettings FaderBackSettingsInstance;

        public SplashFaderBack() => FaderBackSettingsInstance = new FaderBackSettings();

        // Actual logic
        public override string Display(SplashContext context)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                while (!SplashClosing)
                    FaderBack.Simulate(FaderBackSettingsInstance);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
            return "";
        }

    }
}
