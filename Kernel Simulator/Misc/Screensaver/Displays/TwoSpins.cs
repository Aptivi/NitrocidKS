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

using KS.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using KS.Misc.Reflection;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using System.Collections.Generic;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for TwoSpins
    /// </summary>
    public static class TwoSpinsSettings
    {
        private static bool twoSpinsTrueColor = true;
        private static int twoSpinsDelay = 25;
        private static int twoSpinsMinimumRedColorLevel = 0;
        private static int twoSpinsMinimumGreenColorLevel = 0;
        private static int twoSpinsMinimumBlueColorLevel = 0;
        private static int twoSpinsMinimumColorLevel = 0;
        private static int twoSpinsMaximumRedColorLevel = 255;
        private static int twoSpinsMaximumGreenColorLevel = 255;
        private static int twoSpinsMaximumBlueColorLevel = 255;
        private static int twoSpinsMaximumColorLevel = 255;

        /// <summary>
        /// [TwoSpins] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool TwoSpinsTrueColor
        {
            get
            {
                return twoSpinsTrueColor;
            }
            set
            {
                twoSpinsTrueColor = value;
            }
        }
        /// <summary>
        /// [TwoSpins] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int TwoSpinsDelay
        {
            get
            {
                return twoSpinsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                twoSpinsDelay = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The minimum red color level (true color)
        /// </summary>
        public static int TwoSpinsMinimumRedColorLevel
        {
            get
            {
                return twoSpinsMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                twoSpinsMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The minimum green color level (true color)
        /// </summary>
        public static int TwoSpinsMinimumGreenColorLevel
        {
            get
            {
                return twoSpinsMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                twoSpinsMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The minimum blue color level (true color)
        /// </summary>
        public static int TwoSpinsMinimumBlueColorLevel
        {
            get
            {
                return twoSpinsMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                twoSpinsMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int TwoSpinsMinimumColorLevel
        {
            get
            {
                return twoSpinsMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                twoSpinsMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The maximum red color level (true color)
        /// </summary>
        public static int TwoSpinsMaximumRedColorLevel
        {
            get
            {
                return twoSpinsMaximumRedColorLevel;
            }
            set
            {
                if (value <= twoSpinsMinimumRedColorLevel)
                    value = twoSpinsMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                twoSpinsMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The maximum green color level (true color)
        /// </summary>
        public static int TwoSpinsMaximumGreenColorLevel
        {
            get
            {
                return twoSpinsMaximumGreenColorLevel;
            }
            set
            {
                if (value <= twoSpinsMinimumGreenColorLevel)
                    value = twoSpinsMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                twoSpinsMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The maximum blue color level (true color)
        /// </summary>
        public static int TwoSpinsMaximumBlueColorLevel
        {
            get
            {
                return twoSpinsMaximumBlueColorLevel;
            }
            set
            {
                if (value <= twoSpinsMinimumBlueColorLevel)
                    value = twoSpinsMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                twoSpinsMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int TwoSpinsMaximumColorLevel
        {
            get
            {
                return twoSpinsMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= twoSpinsMinimumColorLevel)
                    value = twoSpinsMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                twoSpinsMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for TwoSpins
    /// </summary>
    public class TwoSpinsDisplay : BaseScreensaver, IScreensaver
    {

        private static Color firstGroupDotsColor = ConsoleColors.Green;
        private static Color secondGroupDotsColor = ConsoleColors.Red;
        private static int maxDots = 0;
        private static int firstGroupHalfConsoleWidth = 0;
        private static int secondGroupHalfConsoleWidth = 0;
        private static bool firstGroupNewDotGenerate = true;
        private static bool secondGroupNewDotGenerate = true;
        private static int newDotGroup = 1;
        private static readonly List<(int x, int y, DotMovementDirection direction)> firstGroupDots = [];
        private static readonly List<(int x, int y, DotMovementDirection direction)> secondGroupDots = [];

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "TwoSpins";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();

            // Select two colors for two groups
            if (TwoSpinsSettings.TwoSpinsTrueColor)
            {
                int firstGroupRedColorNum = RandomDriver.Random(TwoSpinsSettings.TwoSpinsMinimumRedColorLevel, TwoSpinsSettings.TwoSpinsMaximumRedColorLevel);
                int firstGroupGreenColorNum = RandomDriver.Random(TwoSpinsSettings.TwoSpinsMinimumGreenColorLevel, TwoSpinsSettings.TwoSpinsMaximumGreenColorLevel);
                int firstGroupBlueColorNum = RandomDriver.Random(TwoSpinsSettings.TwoSpinsMinimumBlueColorLevel, TwoSpinsSettings.TwoSpinsMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color for first group (R;G;B: {0};{1};{2})", firstGroupRedColorNum, firstGroupGreenColorNum, firstGroupBlueColorNum);
                int secondGroupRedColorNum = RandomDriver.Random(TwoSpinsSettings.TwoSpinsMinimumRedColorLevel, TwoSpinsSettings.TwoSpinsMaximumRedColorLevel);
                int secondGroupGreenColorNum = RandomDriver.Random(TwoSpinsSettings.TwoSpinsMinimumGreenColorLevel, TwoSpinsSettings.TwoSpinsMaximumGreenColorLevel);
                int secondGroupBlueColorNum = RandomDriver.Random(TwoSpinsSettings.TwoSpinsMinimumBlueColorLevel, TwoSpinsSettings.TwoSpinsMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color for second group (R;G;B: {0};{1};{2})", secondGroupRedColorNum, secondGroupGreenColorNum, secondGroupBlueColorNum);
                var firstGroupColorStorage = new Color(firstGroupRedColorNum, firstGroupGreenColorNum, firstGroupBlueColorNum);
                var secondGroupColorStorage = new Color(secondGroupRedColorNum, secondGroupGreenColorNum, secondGroupBlueColorNum);
                firstGroupDotsColor = firstGroupColorStorage;
                secondGroupDotsColor = secondGroupColorStorage;
            }
            else
            {
                int firstGroupColorNum = RandomDriver.Random(TwoSpinsSettings.TwoSpinsMinimumColorLevel, TwoSpinsSettings.TwoSpinsMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color for first group ({0})", firstGroupColorNum);
                int secondGroupColorNum = RandomDriver.Random(TwoSpinsSettings.TwoSpinsMinimumColorLevel, TwoSpinsSettings.TwoSpinsMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color for second group ({0})", secondGroupColorNum);
                firstGroupDotsColor = firstGroupColorNum;
                secondGroupDotsColor = secondGroupColorNum;
            }

            // Determine the maximum amount of dots and the console half width
            int width = ConsoleWrapper.WindowWidth;
            maxDots = width / 2;
            firstGroupHalfConsoleWidth = width / 2;
            secondGroupHalfConsoleWidth = width / 2;
            if (width % 2 == 0)
            {
                firstGroupHalfConsoleWidth++;
                secondGroupHalfConsoleWidth--;
            }

            // Clear all the dots
            firstGroupDots.Clear();
            secondGroupDots.Clear();

            // Reset the state
            firstGroupNewDotGenerate = true;
            secondGroupNewDotGenerate = true;
            newDotGroup = RandomDriver.Random(1, 2);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Determine if we need to generate a new dot
            if (newDotGroup > 0)
            {
                // For the first group
                if (newDotGroup == 1 && firstGroupNewDotGenerate)
                {
                    // Switch the new dot creation to the second group
                    newDotGroup = 2;

                    // Check the maximum dot count
                    if (firstGroupDots.Count == maxDots)
                        firstGroupNewDotGenerate = false;

                    // Now, add the dot to the lower right corner
                    firstGroupDots.Add((ConsoleWrapper.WindowWidth - 1, ConsoleWrapper.WindowHeight - 1, DotMovementDirection.Left));
                }

                // For the second group
                else if (newDotGroup == 2 && secondGroupNewDotGenerate)
                {
                    // Switch the new dot creation to the first group
                    newDotGroup = 1;

                    // Check the maximum dot count
                    if (secondGroupDots.Count == maxDots)
                        secondGroupNewDotGenerate = false;

                    // Now, add the dot to the lower left corner
                    secondGroupDots.Add((0, ConsoleWrapper.WindowHeight - 1, DotMovementDirection.Right));
                }

                // Check the maximum dot count for both groups
                if (firstGroupDots.Count == maxDots && secondGroupDots.Count == maxDots)
                    newDotGroup = 0;
            }

            // Prepare the string buffer
            var spinBuffer = new StringBuilder();

            // Now, perform movement for each dot in two groups
            List<(int x, int y, DotMovementDirection direction)> lastFirstGroupDots = [];
            for (int i = 0; i < firstGroupDots.Count; i++)
            {
                var (x, y, direction) = firstGroupDots[i];

                // Draw a dot
                spinBuffer.Append(
                    CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1) +
                    firstGroupDotsColor.VTSequenceBackground +
                    " "
                );

                // Add a dot to the last first group dots before trying to change its values
                lastFirstGroupDots.Add((x, y, direction));

                // Now, perform a movement
                switch (direction)
                {
                    case DotMovementDirection.Left:
                        x--;
                        if (x == firstGroupHalfConsoleWidth)
                            direction = DotMovementDirection.Top;
                        break;
                    case DotMovementDirection.Top:
                        y--;
                        if (y == 0)
                            direction = DotMovementDirection.Right;
                        break;
                    case DotMovementDirection.Right:
                        x++;
                        if (x == ConsoleWrapper.WindowWidth - 1)
                            direction = DotMovementDirection.Bottom;
                        break;
                    case DotMovementDirection.Bottom:
                        y++;
                        if (y == ConsoleWrapper.WindowHeight - 1)
                            direction = DotMovementDirection.Left;
                        break;
                }

                // Save the changes
                firstGroupDots[i] = (x, y, direction);
            }

            // Same thing for the second group
            List<(int x, int y, DotMovementDirection direction)> lastSecondGroupDots = [];
            for (int i = 0; i < secondGroupDots.Count; i++)
            {
                var (x, y, direction) = secondGroupDots[i];

                // Draw a dot
                spinBuffer.Append(
                    CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1) +
                    secondGroupDotsColor.VTSequenceBackground +
                    " "
                );

                // Add a dot to the last second group dots before trying to change its values
                lastSecondGroupDots.Add((x, y, direction));

                // Now, perform a movement
                switch (direction)
                {
                    case DotMovementDirection.Right:
                        x++;
                        if (x == secondGroupHalfConsoleWidth)
                            direction = DotMovementDirection.Top;
                        break;
                    case DotMovementDirection.Top:
                        y--;
                        if (y == 0)
                            direction = DotMovementDirection.Left;
                        break;
                    case DotMovementDirection.Left:
                        x--;
                        if (x == 0)
                            direction = DotMovementDirection.Bottom;
                        break;
                    case DotMovementDirection.Bottom:
                        y++;
                        if (y == ConsoleWrapper.WindowHeight - 1)
                            direction = DotMovementDirection.Right;
                        break;
                }

                // Save the changes
                secondGroupDots[i] = (x, y, direction);
            }

            // Write the spin buffer
            TextWriterRaw.WritePlain(spinBuffer.ToString(), false);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(TwoSpinsSettings.TwoSpinsDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Clear the printed dots
            var clearBuffer = new StringBuilder();
            foreach (var (x, y, direction) in lastFirstGroupDots)
            {
                // Draw a cleared dot
                clearBuffer.Append(
                    CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1) +
                    KernelColorTools.BackgroundColor.VTSequenceBackground +
                    " "
                );
            }
            foreach (var (x, y, direction) in lastSecondGroupDots)
            {
                // Draw a cleared dot
                clearBuffer.Append(
                    CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1) +
                    KernelColorTools.BackgroundColor.VTSequenceBackground +
                    " "
                );
            }
            TextWriterRaw.WritePlain(clearBuffer.ToString(), false);
        }

    }

    enum DotMovementDirection
    {
        Left,
        Right,
        Top,
        Bottom,
    }
}
