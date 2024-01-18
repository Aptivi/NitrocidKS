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
using System.Collections;
using System.Security.Principal;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Login;
using KS.Misc.Splash;
using KS.Misc.Writers.DebugWriters;
using SpecProbe.Hardware;
using SpecProbe.Platform;

namespace KS.Hardware
{
    public static class HardwareProbe
    {
        internal static IEnumerable processors;
        internal static IEnumerable pcMemory;
        internal static IEnumerable hardDrive;
        internal static IEnumerable graphics;

        /// <inheritdoc/>
        public static IEnumerable ProbeGraphics() =>
            HardwareProber.Video;

        /// <inheritdoc/>
        public static IEnumerable ProbeHardDrive() =>
            HardwareProber.HardDisk;

        /// <inheritdoc/>
        public static IEnumerable ProbePcMemory() =>
            HardwareProber.Memory;

        /// <inheritdoc/>
        public static IEnumerable ProbeProcessor() =>
            HardwareProber.Processors;

        /// <summary>
        /// Starts probing hardware
        /// </summary>
        public static void StartProbing()
        {
            // We will probe hardware
            Kernel.Kernel.KernelEventManager.RaiseHardwareProbing();
            try
            {
                if (!PlatformHelper.IsOnWindows() || PlatformHelper.IsOnWindows() && WindowsUserTools.IsAdministrator())
                {
                    processors = ProbeProcessor();
                    pcMemory = ProbePcMemory();
                    hardDrive = ProbeHardDrive();
                    graphics = ProbeGraphics();
                    DebugWriter.Wdbg(DebugLevel.I, "Probe finished.");
                }
                else
                {
                    processors = Array.Empty<object>();
                    pcMemory = Array.Empty<object>();
                    hardDrive = Array.Empty<object>();
                    graphics = Array.Empty<object>();
                    SplashReport.ReportProgress(Translate.DoTranslation("Hardware won't be parsed because of insufficient privileges. Use \"winelevate\" to restart Nitrocid in elevated mode."), 0, KernelColorTools.ColTypes.Warning);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed to probe hardware: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
                KernelTools.KernelError(KernelErrorLevel.F, true, 10L, Translate.DoTranslation("There was an error when probing hardware: {0}"), ex, ex.Message);
            }

            // Raise event
            Kernel.Kernel.KernelEventManager.RaiseHardwareProbed();
        }

    }
}
