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
using System.Collections.Generic;
using System.Linq;
using KS.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using KS.Misc.Reflection;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Wave
    /// </summary>
    public static class WaveSettings
    {
        private static int waveDelay = 100;
        private static double waveFrequencyLevel = 3;
        private static int waveMinimumRedColorLevel = 0;
        private static int waveMinimumGreenColorLevel = 0;
        private static int waveMinimumBlueColorLevel = 0;
        private static int waveMinimumColorLevel = 0;
        private static int waveMaximumRedColorLevel = 255;
        private static int waveMaximumGreenColorLevel = 255;
        private static int waveMaximumBlueColorLevel = 255;
        private static int waveMaximumColorLevel = 255;

        /// <summary>
        /// [Wave] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int WaveDelay
        {
            get
            {
                return waveDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                waveDelay = value;
            }
        }
        /// <summary>
        /// [Wave] The level of the frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>.
        /// </summary>
        public static double WaveFrequencyLevel
        {
            get
            {
                return waveFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                waveFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum red color level (true color)
        /// </summary>
        public static int WaveMinimumRedColorLevel
        {
            get
            {
                return waveMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                waveMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum green color level (true color)
        /// </summary>
        public static int WaveMinimumGreenColorLevel
        {
            get
            {
                return waveMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                waveMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum blue color level (true color)
        /// </summary>
        public static int WaveMinimumBlueColorLevel
        {
            get
            {
                return waveMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                waveMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int WaveMinimumColorLevel
        {
            get
            {
                return waveMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                waveMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum red color level (true color)
        /// </summary>
        public static int WaveMaximumRedColorLevel
        {
            get
            {
                return waveMaximumRedColorLevel;
            }
            set
            {
                if (value <= waveMaximumRedColorLevel)
                    value = waveMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                waveMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum green color level (true color)
        /// </summary>
        public static int WaveMaximumGreenColorLevel
        {
            get
            {
                return waveMaximumGreenColorLevel;
            }
            set
            {
                if (value <= waveMaximumGreenColorLevel)
                    value = waveMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                waveMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum blue color level (true color)
        /// </summary>
        public static int WaveMaximumBlueColorLevel
        {
            get
            {
                return waveMaximumBlueColorLevel;
            }
            set
            {
                if (value <= waveMaximumBlueColorLevel)
                    value = waveMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                waveMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int WaveMaximumColorLevel
        {
            get
            {
                return waveMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= waveMaximumColorLevel)
                    value = waveMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                waveMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display for Wave
    /// </summary>
    public class WaveDisplay : BaseScreensaver, IScreensaver
    {
        private int posIdx = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Wave";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            posIdx = 0;
            ColorTools.LoadBack();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // First, prepare how many dots to render according to the console size
            int Height = ConsoleWrapper.WindowHeight - 4;
            int Count = ConsoleWrapper.WindowWidth;

            // Then, go ahead and make these bars wave themselves.
            List<int> CurrentPos = [];
            double Frequency = Math.PI / WaveSettings.WaveFrequencyLevel;

            // Set the current positions
            double TimeSecs = 0.0;
            bool isSet = false;
            for (int i = 0; i < Count; i++)
            {
                TimeSecs += 0.1;
                double calculatedHeight = Height * Math.Cos(Frequency * TimeSecs) / 2;
                CurrentPos.Add((int)calculatedHeight);
                if (calculatedHeight == Height / 2 && isSet)
                    break;
                if (!isSet)
                    isSet = true;
            }

            // Render the bars
            int RedColorNum = RandomDriver.Random(WaveSettings.WaveMinimumRedColorLevel, WaveSettings.WaveMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(WaveSettings.WaveMinimumGreenColorLevel, WaveSettings.WaveMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(WaveSettings.WaveMinimumBlueColorLevel, WaveSettings.WaveMaximumBlueColorLevel);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
            var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            for (int i = 0; i < Count; i++)
            {
                posIdx++;
                if (posIdx >= CurrentPos.Count)
                    posIdx = 0;
                int Pos = CurrentPos[posIdx] + Math.Abs(CurrentPos.Min()) + 2;
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ThreadManager.SleepNoBlock(WaveSettings.WaveDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                    for (int j = 0; j < ConsoleWrapper.WindowHeight; j++)
                        TextWriterWhereColor.WriteWhereColorBack(" ", i, j, Color.Empty, KernelColorTools.BackgroundColor);
                    TextWriterWhereColor.WriteWhereColorBack(" ", i, Pos, Color.Empty, ColorStorage);
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(WaveSettings.WaveDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
