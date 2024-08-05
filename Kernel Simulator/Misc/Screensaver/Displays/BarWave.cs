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
using Terminaux.Writer.FancyWriters;
using KS.Misc.Reflection;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for BarWave
    /// </summary>
    public static class BarWaveSettings
    {
        private static bool barWaveTrueColor = true;
        private static int barWaveDelay = 100;
        private static double barWaveFrequencyLevel = 2;
        private static int barWaveMinimumRedColorLevel = 0;
        private static int barWaveMinimumGreenColorLevel = 0;
        private static int barWaveMinimumBlueColorLevel = 0;
        private static int barWaveMinimumColorLevel = 0;
        private static int barWaveMaximumRedColorLevel = 255;
        private static int barWaveMaximumGreenColorLevel = 255;
        private static int barWaveMaximumBlueColorLevel = 255;
        private static int barWaveMaximumColorLevel = 255;

        /// <summary>
        /// [BarWave] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool BarWaveTrueColor
        {
            get
            {
                return barWaveTrueColor;
            }
            set
            {
                barWaveTrueColor = value;
            }
        }
        /// <summary>
        /// [BarWave] The level of the frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>.
        /// </summary>
        public static double BarWaveFrequencyLevel
        {
            get
            {
                return barWaveFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 2;
                barWaveFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BarWaveDelay
        {
            get
            {
                return barWaveDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                barWaveDelay = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum red color level (true color)
        /// </summary>
        public static int BarWaveMinimumRedColorLevel
        {
            get
            {
                return barWaveMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barWaveMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum green color level (true color)
        /// </summary>
        public static int BarWaveMinimumGreenColorLevel
        {
            get
            {
                return barWaveMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barWaveMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum blue color level (true color)
        /// </summary>
        public static int BarWaveMinimumBlueColorLevel
        {
            get
            {
                return barWaveMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barWaveMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BarWaveMinimumColorLevel
        {
            get
            {
                return barWaveMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                barWaveMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum red color level (true color)
        /// </summary>
        public static int BarWaveMaximumRedColorLevel
        {
            get
            {
                return barWaveMaximumRedColorLevel;
            }
            set
            {
                if (value <= barWaveMaximumRedColorLevel)
                    value = barWaveMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                barWaveMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum green color level (true color)
        /// </summary>
        public static int BarWaveMaximumGreenColorLevel
        {
            get
            {
                return barWaveMaximumGreenColorLevel;
            }
            set
            {
                if (value <= barWaveMaximumGreenColorLevel)
                    value = barWaveMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                barWaveMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum blue color level (true color)
        /// </summary>
        public static int BarWaveMaximumBlueColorLevel
        {
            get
            {
                return barWaveMaximumBlueColorLevel;
            }
            set
            {
                if (value <= barWaveMaximumBlueColorLevel)
                    value = barWaveMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                barWaveMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BarWaveMaximumColorLevel
        {
            get
            {
                return barWaveMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= barWaveMaximumColorLevel)
                    value = barWaveMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                barWaveMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display for BarWave
    /// </summary>
    public class BarWaveDisplay : BaseScreensaver, IScreensaver
    {
        private double TimeSecs = 0.0d;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BarWave";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            TimeSecs = 0.0d;
            ColorTools.LoadBack();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // First, prepare how many bars to render according to the console size
            int BarHeight = ConsoleWrapper.WindowHeight - 4;
            int BarWidthOutside = 3;
            int BarCount = ConsoleWrapper.WindowWidth / 4;

            // Then, go ahead and make these bars wave themselves.
            int[] CurrentPos = new int[BarCount];
            double Frequency = Math.PI / BarWaveSettings.BarWaveFrequencyLevel;

            // Set the current positions
            for (int i = 0; i < BarCount; i++)
            {
                TimeSecs += 0.1;
                if (TimeSecs > 2.0)
                    TimeSecs = 0.0;
                CurrentPos[i] = (int)Math.Abs(BarHeight * Math.Cos(Frequency * TimeSecs));
            }

            // Render the bars
            for (int i = 0; i < BarCount; i++)
            {
                int ThisBarLeft = (BarWidthOutside + 1) * i + 1;
                int Pos = (int)(100 * (CurrentPos[i] / (double)BarHeight));
                int RedColorNum = RandomDriver.Random(BarWaveSettings.BarWaveMinimumRedColorLevel, BarWaveSettings.BarWaveMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(BarWaveSettings.BarWaveMinimumGreenColorLevel, BarWaveSettings.BarWaveMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(BarWaveSettings.BarWaveMinimumBlueColorLevel, BarWaveSettings.BarWaveMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                    ProgressBarVerticalColor.WriteVerticalProgress(Pos, ThisBarLeft, 1, 4, ColorStorage);
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(BarWaveSettings.BarWaveDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
