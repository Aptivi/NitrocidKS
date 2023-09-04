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

using System.Threading;
using KS.ConsoleBase;
using KS.Languages;
using KS.ConsoleBase.Inputs;
using KS.Kernel.Power;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Starting.Environment;
using KS.ConsoleBase.Colors;
using System;
using KS.Kernel.Exceptions;
using KS.Kernel.Debugging;
using KS.Kernel.Starting;
using Terminaux.Sequences.Builder;

namespace KS.Kernel
{
    /// <summary>
    /// Kernel main class
    /// </summary>
    internal static class Kernel
    {
        /// <summary>
        /// Entry point
        /// </summary>
        internal static void Main(string[] Args)
        {
            // Set main thread name
            Thread.CurrentThread.Name = "Main Nitrocid Kernel Thread";

            // This is a kernel entry point
            while (!Flags.KernelShutdown)
            {
                try
                {
                    EnvironmentTools.ExecuteEnvironment(Args);
                }
                catch (KernelException icde) when (icde.ExceptionType == KernelExceptionType.InsaneConsoleDetected)
                {
                    ConsoleWrapper.WriteLine(icde.Message);
                    Flags.KernelShutdown = true;
                }
                catch (KernelErrorException kee)
                {
                    DebugWriter.WriteDebugStackTrace(kee);
                    Flags.SafeMode = false;
                }
                catch (Exception ex)
                {
                    KernelPanic.KernelError(KernelErrorLevel.U, true, 5, Translate.DoTranslation("Kernel environment error:") + $" {ex.Message}", ex);
                }
                finally
                {
                    // Reset everything to their initial state
                    KernelInitializers.ResetEverything();

                    // Clear the console
                    KernelColorTools.LoadBack();

                    // Always switch back to the main environment
                    if (EnvironmentTools.resetEnvironment)
                    {
                        EnvironmentTools.resetEnvironment = false;
                        EnvironmentTools.ResetEnvironment();
                    }
                }
            }

            // If "No APM" is enabled, simply print the text
            if (Flags.SimulateNoAPM)
            {
                ConsoleWrapper.WriteLine(Translate.DoTranslation("It's now safe to turn off your computer."));
                Input.DetectKeypress();
            }

            // Load main buffer
            if (!KernelPlatform.IsOnWindows() && Flags.UseAltBuffer && Flags.HasSetAltBuffer)
                TextWriterColor.Write("\u001b[?1049l");

            // Reset colors and clear the console
            ConsoleExtensions.ResetAll();

            // Reset cursor state and dispose handlers
            ConsoleWrapper.CursorVisible = true;
            PowerSignalHandlers.DisposeHandlers();
        }
    }
}
