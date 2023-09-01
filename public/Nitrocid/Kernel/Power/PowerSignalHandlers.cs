﻿
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KS.Kernel.Power
{
    internal static class PowerSignalHandlers
    {
        internal static List<PosixSignalRegistration> signalHandlers = new();
        internal static bool initialized = false;

        internal static void RegisterHandlers()
        {
            signalHandlers.Add(PosixSignalRegistration.Create(PosixSignal.SIGQUIT, SigQuit));
            signalHandlers.Add(PosixSignalRegistration.Create(PosixSignal.SIGTERM, SigQuit));
        }

        internal static void DisposeHandlers()
        {
            foreach (var signalHandler in signalHandlers)
                signalHandler.Dispose();
        }

        private static void SigQuit(PosixSignalContext psc)
        {
            PowerManager.PowerManage(PowerMode.Shutdown);
            psc.Cancel = true;
        }
    }
}
