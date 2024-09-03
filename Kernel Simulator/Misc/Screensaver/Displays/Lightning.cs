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

using Terminaux.Writer.ConsoleWriters;
using KS.Misc.Reflection;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using System;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Lightning
    /// </summary>
    public static class LightningSettings
    {
        private static int lightningDelay = 100;
        private static int lightningStrikeProbability = 5;

        /// <summary>
        /// [Lightning] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int LightningDelay
        {
            get
            {
                return lightningDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                lightningDelay = value;
            }
        }
        /// <summary>
        /// [Lightning] Chance, in percent, to strike
        /// </summary>
        public static int LightningStrikeProbability
        {
            get
            {
                return lightningStrikeProbability;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                lightningStrikeProbability = value;
            }
        }
    }

    /// <summary>
    /// Display code for Lightning
    /// </summary>
    public class LightningDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Lightning";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            ColorTools.LoadBackDry(0);
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            bool striking = RandomDriver.RandomChance(LightningSettings.LightningStrikeProbability);
            if (striking)
            {
                // First, determine the lightning bolt position
                int quarterWidth = ConsoleWrapper.WindowWidth / 4;
                int boltFirstHalfEndY = ConsoleWrapper.WindowHeight / 2 - 2;
                int boltSecondHalfEndY = ConsoleWrapper.WindowHeight / 2 + 2;
                int boltTopEdgeX = ConsoleWrapper.WindowWidth / 2 - RandomDriver.Random(-quarterWidth, quarterWidth);
                int boltBottomEdgeX = boltTopEdgeX;
                int boltFirstHalfEndX = boltTopEdgeX - 8;
                int boltSecondHalfEndX = boltBottomEdgeX + 8;

                // Determine the thresholds
                double boltFromTopToFirstHalfX = -((double)(boltTopEdgeX - boltFirstHalfEndX) / boltFirstHalfEndY);
                double boltFromFirstHalfToSecondHalfY = (double)(boltSecondHalfEndY - boltFirstHalfEndY) / (boltSecondHalfEndX - boltFirstHalfEndX);
                double boltFromSecondHalfToBottomX = (double)(boltBottomEdgeX - boltSecondHalfEndX) / boltSecondHalfEndY;

                for (int step = 1; step <= 5; step++)
                {
                    // If the step number is even, show the lightning. Else, show the white background.
                    bool showStrike = step % 2 == 0;
                    if (showStrike)
                    {
                        var strike = new StringBuilder();

                        // Draw the flashes first
                        strike.Append(new Color(ConsoleColors.Yellow3Alt).VTSequenceBackground);
                        for (int y = 0; y < boltFirstHalfEndY; y++)
                        {
                            int x = (int)Math.Round(boltTopEdgeX + boltFromTopToFirstHalfX * y);
                            strike.Append(
                                CsiSequences.GenerateCsiCursorPosition(x + 2, y + 1) +
                                " " +
                                CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1) +
                                " " +
                                CsiSequences.GenerateCsiCursorPosition(x, y + 1) +
                                " "
                            );
                        }
                        int xCount = 0;
                        for (int x = boltFirstHalfEndX; x < boltSecondHalfEndX; x++)
                        {
                            int y = (int)Math.Round(boltFirstHalfEndY + boltFromFirstHalfToSecondHalfY * xCount);
                            strike.Append(
                                CsiSequences.GenerateCsiCursorPosition(x + 2, y + 2) +
                                " " +
                                CsiSequences.GenerateCsiCursorPosition(x + 2, y + 1) +
                                " " +
                                CsiSequences.GenerateCsiCursorPosition(x + 2, y) +
                                " " +
                                CsiSequences.GenerateCsiCursorPosition(x + 1, y + 2) +
                                " " +
                                CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1) +
                                " " +
                                CsiSequences.GenerateCsiCursorPosition(x + 1, y) +
                                " " +
                                CsiSequences.GenerateCsiCursorPosition(x, y + 2) +
                                " " +
                                CsiSequences.GenerateCsiCursorPosition(x, y + 1) +
                                " " +
                                CsiSequences.GenerateCsiCursorPosition(x, y) +
                                " "
                            );
                            xCount++;
                        }
                        int yCount = 0;
                        for (int y = boltSecondHalfEndY; y < ConsoleWrapper.WindowHeight; y++)
                        {
                            int x = (int)Math.Round(boltSecondHalfEndX + boltFromSecondHalfToBottomX * yCount);
                            strike.Append(
                                CsiSequences.GenerateCsiCursorPosition(x + 2, y + 1) +
                                " " +
                                CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1) +
                                " " +
                                CsiSequences.GenerateCsiCursorPosition(x, y + 1) +
                                " "
                            );
                            yCount++;
                        }

                        // Draw the lightning!
                        strike.Append(new Color(ConsoleColors.LightYellow3).VTSequenceBackground);
                        for (int y = 0; y < boltFirstHalfEndY; y++)
                        {
                            int x = (int)Math.Round(boltTopEdgeX + boltFromTopToFirstHalfX * y);
                            strike.Append(
                                CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1) +
                                " "
                            );
                        }
                        xCount = 0;
                        for (int x = boltFirstHalfEndX; x < boltSecondHalfEndX; x++)
                        {
                            int y = (int)Math.Round(boltFirstHalfEndY + boltFromFirstHalfToSecondHalfY * xCount);
                            strike.Append(
                                CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1) +
                                " "
                            );
                            xCount++;
                        }
                        yCount = 0;
                        for (int y = boltSecondHalfEndY; y < ConsoleWrapper.WindowHeight; y++)
                        {
                            int x = (int)Math.Round(boltSecondHalfEndX + boltFromSecondHalfToBottomX * yCount);
                            strike.Append(
                                CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1) +
                                " "
                            );
                            yCount++;
                        }

                        // Write the rendered strike
                        TextWriterRaw.WritePlain(strike.ToString(), false);
                        ThreadManager.SleepNoBlock(LightningSettings.LightningDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                    }
                    else
                    {
                        // Show only the white background
                        ColorTools.LoadBackDry(new Color(ConsoleColors.White));
                        ThreadManager.SleepNoBlock(LightningSettings.LightningDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
                    }
                }
                ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            }
            ThreadManager.SleepNoBlock(LightningSettings.LightningDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}