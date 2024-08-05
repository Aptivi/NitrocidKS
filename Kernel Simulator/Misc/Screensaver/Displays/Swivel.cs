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
    /// Settings for Swivel
    /// </summary>
    public static class SwivelSettings
    {
        private static int swivelDelay = 100;
        private static double swivelHorizontalFrequencyLevel = 3;
        private static double swivelVerticalFrequencyLevel = 8;
        private static int swivelMinimumRedColorLevel = 0;
        private static int swivelMinimumGreenColorLevel = 0;
        private static int swivelMinimumBlueColorLevel = 0;
        private static int swivelMinimumColorLevel = 0;
        private static int swivelMaximumRedColorLevel = 255;
        private static int swivelMaximumGreenColorLevel = 255;
        private static int swivelMaximumBlueColorLevel = 255;
        private static int swivelMaximumColorLevel = 255;

        /// <summary>
        /// [Swivel] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SwivelDelay
        {
            get
            {
                return swivelDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                swivelDelay = value;
            }
        }
        /// <summary>
        /// [Swivel] The level of the horizontal frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public static double SwivelHorizontalFrequencyLevel
        {
            get
            {
                return swivelHorizontalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                swivelHorizontalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The level of the vertical frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public static double SwivelVerticalFrequencyLevel
        {
            get
            {
                return swivelVerticalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 8;
                swivelVerticalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum red color level (true color)
        /// </summary>
        public static int SwivelMinimumRedColorLevel
        {
            get
            {
                return swivelMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                swivelMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum green color level (true color)
        /// </summary>
        public static int SwivelMinimumGreenColorLevel
        {
            get
            {
                return swivelMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                swivelMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum blue color level (true color)
        /// </summary>
        public static int SwivelMinimumBlueColorLevel
        {
            get
            {
                return swivelMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                swivelMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int SwivelMinimumColorLevel
        {
            get
            {
                return swivelMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                swivelMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum red color level (true color)
        /// </summary>
        public static int SwivelMaximumRedColorLevel
        {
            get
            {
                return swivelMaximumRedColorLevel;
            }
            set
            {
                if (value <= swivelMaximumRedColorLevel)
                    value = swivelMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                swivelMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum green color level (true color)
        /// </summary>
        public static int SwivelMaximumGreenColorLevel
        {
            get
            {
                return swivelMaximumGreenColorLevel;
            }
            set
            {
                if (value <= swivelMaximumGreenColorLevel)
                    value = swivelMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                swivelMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum blue color level (true color)
        /// </summary>
        public static int SwivelMaximumBlueColorLevel
        {
            get
            {
                return swivelMaximumBlueColorLevel;
            }
            set
            {
                if (value <= swivelMaximumBlueColorLevel)
                    value = swivelMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                swivelMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int SwivelMaximumColorLevel
        {
            get
            {
                return swivelMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= swivelMaximumColorLevel)
                    value = swivelMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                swivelMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display for Swivel
    /// </summary>
    public class SwivelDisplay : BaseScreensaver, IScreensaver
    {
        private int posIdxVertical = 0;
        private int posIdxHorizontal = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Swivel";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            posIdxVertical = 0;
            posIdxHorizontal = 0;
            ColorTools.LoadBack();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // First, prepare how many dots to render according to the console size
            int Height = ConsoleWrapper.WindowHeight - 4;
            int Width = ConsoleWrapper.WindowWidth - 4;

            // Then, go ahead and make these bars swivel themselves.
            List<int> CurrentPosVertical = [];
            List<int> CurrentPosHorizontal = [];
            double FrequencyVertical = Math.PI / SwivelSettings.SwivelVerticalFrequencyLevel;
            double FrequencyHorizontal = Math.PI / SwivelSettings.SwivelHorizontalFrequencyLevel;

            // Set the current vertical positions
            double TimeSecsVertical = 0.0;
            bool isSetVertical = false;
            while (true)
            {
                TimeSecsVertical += 0.1;
                double calculatedHeight = Height * Math.Cos(FrequencyVertical * TimeSecsVertical) / 2;
                CurrentPosVertical.Add((int)calculatedHeight);
                if ((int)calculatedHeight == Height / 2 && isSetVertical)
                    break;
                if (!isSetVertical && (int)calculatedHeight < Height / 2)
                    isSetVertical = true;
            }

            // Set the current horizontal positions
            double TimeSecsHorizontal = 0.0;
            bool isSetHorizontal = false;
            while (true)
            {
                TimeSecsHorizontal += 0.1;
                double calculatedWidth = Width * Math.Cos(FrequencyHorizontal * TimeSecsHorizontal) / 2;
                CurrentPosHorizontal.Add((int)calculatedWidth);
                if ((int)calculatedWidth == Width / 2 && isSetHorizontal)
                    break;
                if (!isSetHorizontal && (int)calculatedWidth < Width / 2)
                    isSetHorizontal = true;
            }

            // Render the bars
            int RedColorNum = RandomDriver.Random(SwivelSettings.SwivelMinimumRedColorLevel, SwivelSettings.SwivelMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(SwivelSettings.SwivelMinimumGreenColorLevel, SwivelSettings.SwivelMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(SwivelSettings.SwivelMinimumBlueColorLevel, SwivelSettings.SwivelMaximumBlueColorLevel);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
            var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            posIdxVertical++;
            if (posIdxVertical >= CurrentPosVertical.Count)
                posIdxVertical = 0;
            posIdxHorizontal++;
            if (posIdxHorizontal >= CurrentPosHorizontal.Count)
                posIdxHorizontal = 0;
            int PosVertical = CurrentPosVertical[posIdxVertical] + Math.Abs(CurrentPosVertical.Min()) + 2;
            int PosHorizontal = CurrentPosHorizontal[posIdxHorizontal] + Math.Abs(CurrentPosHorizontal.Min()) + 2;
            if (!ConsoleResizeHandler.WasResized(false))
            {
                ThreadManager.SleepNoBlock(SwivelSettings.SwivelDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                TextWriterWhereColor.WriteWhereColorBack(" ", PosHorizontal, PosVertical, Color.Empty, ColorStorage);
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(SwivelSettings.SwivelDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
