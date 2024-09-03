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

using System;
using System.Collections.Generic;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using KS.Misc.Reflection;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Matrix
    /// </summary>
    public static class MatrixSettings
    {
        private static int _matrixDelay = 1;
        private static int _matrixMaxSteps = 25;

        /// <summary>
        /// [Matrix] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int MatrixDelay
        {
            get
            {
                return _matrixDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                _matrixDelay = value;
            }
        }
        /// <summary>
        /// [Matrix] How many fade steps to do?
        /// </summary>
        public static int MatrixMaxSteps
        {
            get
            {
                return _matrixMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _matrixMaxSteps = value;
            }
        }

    }

    /// <summary>
    /// Display code for Matrix
    /// </summary>
    public class MatrixDisplay : BaseScreensaver, IScreensaver
    {

        private int ColumnLine;
        private readonly List<(int, int, string)> CoveredPositions = [];
        private readonly Color foreground = new("0;255;0");
        private readonly Color background = new("0;0;0");

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Matrix";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            ColorTools.LoadBackDry("0;0;0");
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;
            CoveredPositions.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ColorTools.LoadBackDry("0;0;0");
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;

            // Choose the column for the falling line
            ColumnLine = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);

            // Now, determine the fall start and end position
            int FallStart = 0;
            int FallEnd = ConsoleWrapper.WindowHeight - 1;

            // Make the line fall down
            for (int Fall = FallStart; Fall <= FallEnd; Fall++)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                // Print a block and add the covered position to the list so fading down can be done
                string renderedNumber = RandomDriver.Random(1).ToString();
                TextWriterWhereColor.WriteWhereColorBack(renderedNumber, ColumnLine, Fall, false, foreground, background);
                var PositionTuple = (ColumnLine, Fall, renderedNumber);
                CoveredPositions.Add(PositionTuple);

                // Delay
                ThreadManager.SleepNoBlock(MatrixSettings.MatrixDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Fade the line down. Please note that this requires true-color support in the terminal to work properly.
            for (int StepNum = 0; StepNum <= MatrixSettings.MatrixMaxSteps; StepNum++)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                // Set thresholds
                double ThresholdRed = foreground.RGB.R / (double)MatrixSettings.MatrixMaxSteps;
                double ThresholdGreen = foreground.RGB.G / (double)MatrixSettings.MatrixMaxSteps;
                double ThresholdBlue = foreground.RGB.B / (double)MatrixSettings.MatrixMaxSteps;
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

                // Set color fade steps
                int CurrentColorRedOut = (int)Math.Round(foreground.RGB.R - ThresholdRed * StepNum);
                int CurrentColorGreenOut = (int)Math.Round(foreground.RGB.G - ThresholdGreen * StepNum);
                int CurrentColorBlueOut = (int)Math.Round(foreground.RGB.B - ThresholdBlue * StepNum);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);

                // Get the positions and write the block with new color
                var CurrentFadeColor = new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                var bleedBuilder = new StringBuilder();
                foreach ((int, int, string) PositionTuple in CoveredPositions)
                {
                    // Check to see if user decided to resize
                    if (ConsoleResizeHandler.WasResized(false))
                        break;

                    // Actually fade the line out
                    int PositionLeft = PositionTuple.Item1;
                    int PositionTop = PositionTuple.Item2;
                    string renderedNumber = PositionTuple.Item3;
                    bleedBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(PositionLeft + 1, PositionTop + 1)}{renderedNumber}");
                }
                TextWriterWhereColor.WriteWhereColorBack(bleedBuilder.ToString(), ColumnLine, 0, false, CurrentFadeColor, background);

                // Delay
                ThreadManager.SleepNoBlock(MatrixSettings.MatrixDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Reset covered positions
            CoveredPositions.Clear();

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(MatrixSettings.MatrixDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}