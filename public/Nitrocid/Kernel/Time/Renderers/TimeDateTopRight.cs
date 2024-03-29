﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using System;
using System.Threading;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;

namespace Nitrocid.Kernel.Time.Renderers
{
    /// <summary>
    /// Top right corner on time and date
    /// </summary>
    public static class TimeDateTopRight
    {

        // Variables
        internal static KernelThread TimeTopRightChange = new("Time/date top right corner updater thread", true, TimeTopRightChange_DoWork);

        /// <summary>
        /// Updates the time and date. Also updates the time and date corner if it was enabled in kernel configuration.
        /// </summary>
        public static void TimeTopRightChange_DoWork()
        {
            try
            {
                int oldWid = default, oldTop = default;
                while (TimeDateTools.CornerTimeDate)
                {
                    if (!ScreensaverManager.InSaver)
                    {
                        string TimeString = $"{TimeDateRenderers.RenderDate()} - {TimeDateRenderers.RenderTime()}";
                        oldWid = ConsoleWrapper.WindowWidth - TimeString.Length - 1;
                        oldTop = Console.WindowTop;
                        TextWriters.WriteWhere(TimeString, ConsoleWrapper.WindowWidth - TimeString.Length - 1, Console.WindowTop, true, KernelColorType.NeutralText);
                        Thread.Sleep(1000);
                        if (oldWid != 0)
                            TextWriters.WriteWhere(new string(' ', TimeString.Length), oldWid, oldTop, true, KernelColorType.NeutralText);
                    }
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Aborting time/date change thread.");
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Fatal error in time/date changer: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        /// <summary>
        /// Starts the time on top right corner
        /// </summary>
        public static void InitTopRightDate()
        {
            if (!TimeTopRightChange.IsAlive && TimeDateTools.CornerTimeDate)
                TimeTopRightChange.Start();
        }

    }
}
