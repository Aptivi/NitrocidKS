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
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;

namespace KS.Misc.Splash.Splashes
{
    class SplashSystemd : ISplash
    {

        // Standalone splash information
        public string SplashName => "systemd";

        private SplashInfo Info => SplashManager.Splashes[SplashName];

        // Property implementations
        public bool SplashClosing { get; set; }

        public bool SplashDisplaysProgress => Info.DisplaysProgress;

        // Private variables
        private int IndicatorLeft;
        private int IndicatorTop;
        private bool Beginning = true;

        // Actual logic
        public void Opening()
        {
            Beginning = true;
            DebugWriter.Wdbg(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleWrapper.Clear();
        }

        public void Display()
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Splash displaying.");
                IndicatorLeft = ConsoleWrapper.CursorLeft + 2;
                IndicatorTop = ConsoleWrapper.CursorTop;
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
            if (!Beginning)
                TextWriters.WriteWhere("  OK  ", IndicatorLeft, IndicatorTop, true, KernelColorTools.ColTypes.Success);
            TextWriters.Write($" [      ] {ProgressReport}", true, KernelColorTools.ColTypes.Neutral, Vars);
            if (!Beginning)
            {
                IndicatorLeft = 2;
                IndicatorTop = ConsoleWrapper.CursorTop - 1;
            }
            Beginning = false;
        }

    }
}