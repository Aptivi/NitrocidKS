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
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using KS.Misc.Reflection;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for ColorBleed
    /// </summary>
    public static class ColorBleedSettings
    {
        private static bool colorBleedTrueColor = true;
        private static int colorBleedDelay = 10;
        private static int colorBleedMaxSteps = 25;
        private static int colorBleedDropChance = 40;
        private static int colorBleedMinimumRedColorLevel = 0;
        private static int colorBleedMinimumGreenColorLevel = 0;
        private static int colorBleedMinimumBlueColorLevel = 0;
        private static int colorBleedMinimumColorLevel = 0;
        private static int colorBleedMaximumRedColorLevel = 255;
        private static int colorBleedMaximumGreenColorLevel = 255;
        private static int colorBleedMaximumBlueColorLevel = 255;
        private static int colorBleedMaximumColorLevel = 255;

        /// <summary>
        /// [ColorBleed] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool ColorBleedTrueColor
        {
            get
            {
                return colorBleedTrueColor;
            }
            set
            {
                colorBleedTrueColor = value;
            }
        }
        /// <summary>
        /// [ColorBleed] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int ColorBleedDelay
        {
            get
            {
                return colorBleedDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                colorBleedDelay = value;
            }
        }
        /// <summary>
        /// [ColorBleed] How many fade steps to do?
        /// </summary>
        public static int ColorBleedMaxSteps
        {
            get
            {
                return colorBleedMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                colorBleedMaxSteps = value;
            }
        }
        /// <summary>
        /// [ColorBleed] Chance to drop a new falling color
        /// </summary>
        public static int ColorBleedDropChance
        {
            get
            {
                return colorBleedDropChance;
            }
            set
            {
                if (value <= 0)
                    value = 40;
                colorBleedDropChance = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum red color level (true color)
        /// </summary>
        public static int ColorBleedMinimumRedColorLevel
        {
            get
            {
                return colorBleedMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorBleedMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum green color level (true color)
        /// </summary>
        public static int ColorBleedMinimumGreenColorLevel
        {
            get
            {
                return colorBleedMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorBleedMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum blue color level (true color)
        /// </summary>
        public static int ColorBleedMinimumBlueColorLevel
        {
            get
            {
                return colorBleedMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorBleedMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ColorBleedMinimumColorLevel
        {
            get
            {
                return colorBleedMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                colorBleedMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum red color level (true color)
        /// </summary>
        public static int ColorBleedMaximumRedColorLevel
        {
            get
            {
                return colorBleedMaximumRedColorLevel;
            }
            set
            {
                if (value <= colorBleedMinimumRedColorLevel)
                    value = colorBleedMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                colorBleedMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum green color level (true color)
        /// </summary>
        public static int ColorBleedMaximumGreenColorLevel
        {
            get
            {
                return colorBleedMaximumGreenColorLevel;
            }
            set
            {
                if (value <= colorBleedMinimumGreenColorLevel)
                    value = colorBleedMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                colorBleedMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum blue color level (true color)
        /// </summary>
        public static int ColorBleedMaximumBlueColorLevel
        {
            get
            {
                return colorBleedMaximumBlueColorLevel;
            }
            set
            {
                if (value <= colorBleedMinimumBlueColorLevel)
                    value = colorBleedMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                colorBleedMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ColorBleedMaximumColorLevel
        {
            get
            {
                return colorBleedMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= colorBleedMinimumColorLevel)
                    value = colorBleedMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                colorBleedMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for ColorBleed
    /// </summary>
    public class ColorBleedDisplay : BaseScreensaver, IScreensaver
    {

        private static readonly List<BleedState> bleedStates = [];

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "ColorBleed";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            bleedStates.Clear();
            ColorTools.LoadBackDry("0;0;0");
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Now, determine the fall end position
            int FallEnd = ConsoleWrapper.WindowHeight - 1;

            // Invoke the "chance"-based random number generator to decide whether a line is about to fall.
            bool newFall = RandomDriver.RandomChance(ColorBleedSettings.ColorBleedDropChance);
            if (newFall)
                bleedStates.Add(new BleedState());

            // Now, iterate through the bleed states
            for (int bleedIdx = 0; bleedIdx < bleedStates.Count; bleedIdx++)
            {
                // Choose the column for the falling line
                var bleedState = bleedStates[bleedIdx];

                // Make the line fall down
                switch (bleedState.fallState)
                {
                    case BleedFallState.Falling:
                        bleedState.Fall();
                        bleedState.fallStep++;
                        if (bleedState.fallStep > FallEnd)
                            bleedState.fallState = BleedFallState.Fading;
                        break;
                    case BleedFallState.Fading:
                        bleedState.Fade();
                        bleedState.fadeStep++;
                        if (bleedState.fadeStep > ColorBleedSettings.ColorBleedMaxSteps)
                            bleedState.fallState = BleedFallState.Done;
                        break;
                }
            }

            // Purge the "Done" falls
            for (int bleedIdx = bleedStates.Count - 1; bleedIdx >= 0; bleedIdx--)
            {
                var bleedState = bleedStates[bleedIdx];
                if (bleedState.fallState == BleedFallState.Done)
                {
                    bleedStates.RemoveAt(bleedIdx);
                    bleedState.Unreserve(bleedState.ColumnLine);
                }
            }

            // Draw and clean the buffer
            string buffer = BleedState.bleedBuffer.ToString();
            BleedState.bleedBuffer.Clear();
            TextWriterRaw.WritePlain(buffer);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(ColorBleedSettings.ColorBleedDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }

    internal class BleedState
    {
        internal BleedFallState fallState = BleedFallState.Falling;
        internal int ColumnLine;
        internal int fallStep;
        internal int fadeStep;
        internal static StringBuilder bleedBuffer = new();
        internal static readonly List<int> reservedColumns = [];
        private readonly Color ColorStorage;
        private readonly List<Tuple<int, int>> CoveredPositions = [];

        internal void Fall()
        {
            // Check to see if user decided to resize
            if (ConsoleResizeHandler.WasResized(false))
                return;

            // Print a block and add the covered position to the list so fading down can be done
            bleedBuffer.Append(
                $"{CsiSequences.GenerateCsiCursorPosition(ColumnLine + 1, fallStep + 1)}" +
                $"{Color.Empty.VTSequenceForeground}" +
                $"{ColorStorage.VTSequenceBackground}" +
                $" "
            );
            TextWriterWhereColor.WriteWhereColorBack(" ", ColumnLine, fallStep, false, Color.Empty, ColorStorage);
            var PositionTuple = new Tuple<int, int>(ColumnLine, fallStep);
            CoveredPositions.Add(PositionTuple);
        }

        internal void Fade()
        {
            // Check to see if user decided to resize
            if (ConsoleResizeHandler.WasResized(false))
                return;

            // Set thresholds
            double ThresholdRed = ColorStorage.RGB.R / (double)ColorBleedSettings.ColorBleedMaxSteps;
            double ThresholdGreen = ColorStorage.RGB.G / (double)ColorBleedSettings.ColorBleedMaxSteps;
            double ThresholdBlue = ColorStorage.RGB.B / (double)ColorBleedSettings.ColorBleedMaxSteps;
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

            // Set color fade steps
            int CurrentColorRedOut = (int)Math.Round(ColorStorage.RGB.R - ThresholdRed * fadeStep);
            int CurrentColorGreenOut = (int)Math.Round(ColorStorage.RGB.G - ThresholdGreen * fadeStep);
            int CurrentColorBlueOut = (int)Math.Round(ColorStorage.RGB.B - ThresholdBlue * fadeStep);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);

            // Get the positions and write the block with new color
            var CurrentFadeColor = new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
            foreach (Tuple<int, int> PositionTuple in CoveredPositions)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                // Actually fade the line out
                int PositionLeft = PositionTuple.Item1;
                int PositionTop = PositionTuple.Item2;
                bleedBuffer.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(PositionLeft + 1, PositionTop + 1)}" +
                    $"{Color.Empty.VTSequenceForeground}" +
                    $"{CurrentFadeColor.VTSequenceBackground}" +
                     " ");
            }
        }

        internal void Unreserve(int column) =>
            reservedColumns.Remove(column);

        internal BleedState()
        {
            int columnLine = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            while (reservedColumns.Contains(columnLine))
                columnLine = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            reservedColumns.Add(columnLine);
            ColumnLine = columnLine;
            if (ColorBleedSettings.ColorBleedTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ColorBleedSettings.ColorBleedMinimumRedColorLevel, ColorBleedSettings.ColorBleedMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ColorBleedSettings.ColorBleedMinimumGreenColorLevel, ColorBleedSettings.ColorBleedMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ColorBleedSettings.ColorBleedMinimumBlueColorLevel, ColorBleedSettings.ColorBleedMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ColorBleedSettings.ColorBleedMinimumColorLevel, ColorBleedSettings.ColorBleedMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                ColorStorage = new Color(ColorNum);
            }
        }
    }

    internal enum BleedFallState
    {
        Falling,
        Fading,
        Done
    }
}
