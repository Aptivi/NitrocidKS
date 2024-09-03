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
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Misc.Text;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;

namespace KS.Misc.Splash.Splashes
{
    class SplashSimple : ISplash
    {

        // Standalone splash information
        public string SplashName => "Simple";

        private SplashInfo Info => SplashManager.Splashes[SplashName];

        // Property implementations
        public bool SplashClosing { get; set; }

        public bool SplashDisplaysProgress => Info.DisplaysProgress;

        public int ProgressWritePositionX => 3;

        public int ProgressWritePositionY
        {
            get
            {
                switch (SplashSettings.SimpleProgressTextLocation)
                {
                    case TextLocation.Top:
                        {
                            return 1;
                        }
                    case TextLocation.Bottom:
                        {
                            return ConsoleWrapper.WindowHeight - 2;
                        }

                    default:
                        {
                            return 1;
                        }
                }
            }
        }

        public int ProgressReportWritePositionX => 9;

        public int ProgressReportWritePositionY
        {
            get
            {
                switch (SplashSettings.SimpleProgressTextLocation)
                {
                    case TextLocation.Top:
                        {
                            return 1;
                        }
                    case TextLocation.Bottom:
                        {
                            return ConsoleWrapper.WindowHeight - 2;
                        }

                    default:
                        {
                            return 1;
                        }
                }
            }
        }

        // Actual logic
        public void Opening()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleWrapper.Clear();
        }

        public void Display()
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Splash displaying.");

                // Display the progress text
                UpdateProgressReport(SplashReport.Progress, SplashReport.ProgressText, ProgressWritePositionX, ProgressWritePositionY, ProgressReportWritePositionX, ProgressReportWritePositionY);

                // Loop until closing
                while (!SplashClosing)
                    Thread.Sleep(1);
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
            ConsoleWrapper.Clear();
        }

        public void Report(int Progress, string ProgressReport, params object[] Vars)
        {
            UpdateProgressReport(Progress, ProgressReport, Vars);
        }

        /// <summary>
        /// Updates the splash progress
        /// </summary>
        /// <param name="Progress">Progress percentage from 0 to 100</param>
        /// <param name="ProgressReport">The progress text</param>
        public void UpdateProgressReport(int Progress, string ProgressReport, params object[] Vars)
        {
            string RenderedText = ProgressReport.Truncate(ConsoleWrapper.WindowWidth - ProgressReportWritePositionX - ProgressWritePositionX - 3);
            TextWriters.WriteWhere("{0}%", ProgressWritePositionX, ProgressWritePositionY, true, KernelColorTools.ColTypes.Progress, Progress.ToString().PadLeft(3));
            TextWriters.WriteWhere(RenderedText, ProgressReportWritePositionX, ProgressReportWritePositionY, false, KernelColorTools.ColTypes.Neutral, Vars);
            ConsoleBase.ConsoleExtensions.ClearLineToRight();
        }

    }
}