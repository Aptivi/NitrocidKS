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
using Terminaux.Colors.Data;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for LaserBeams
    /// </summary>
    public static class LaserBeamsSettings
    {
        private static bool laserBeamsTrueColor = true;
        private static int laserBeamsDelay = 500;
        private static string laserBeamsLineChar = "-";
        private static string laserBeamsBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private static int laserBeamsMinimumRedColorLevel = 0;
        private static int laserBeamsMinimumGreenColorLevel = 0;
        private static int laserBeamsMinimumBlueColorLevel = 0;
        private static int laserBeamsMinimumColorLevel = 0;
        private static int laserBeamsMaximumRedColorLevel = 255;
        private static int laserBeamsMaximumGreenColorLevel = 255;
        private static int laserBeamsMaximumBlueColorLevel = 255;
        private static int laserBeamsMaximumColorLevel = 255;

        /// <summary>
        /// [LaserBeams] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool LaserBeamsTrueColor
        {
            get
            {
                return laserBeamsTrueColor;
            }
            set
            {
                laserBeamsTrueColor = value;
            }
        }
        /// <summary>
        /// [LaserBeams] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int LaserBeamsDelay
        {
            get
            {
                return laserBeamsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                laserBeamsDelay = value;
            }
        }
        /// <summary>
        /// [LaserBeams] Line character
        /// </summary>
        public static string LaserBeamsLineChar
        {
            get
            {
                return laserBeamsLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                laserBeamsLineChar = value;
            }
        }
        /// <summary>
        /// [LaserBeams] Screensaver background color
        /// </summary>
        public static string LaserBeamsBackgroundColor
        {
            get
            {
                return laserBeamsBackgroundColor;
            }
            set
            {
                laserBeamsBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum red color level (true color)
        /// </summary>
        public static int LaserBeamsMinimumRedColorLevel
        {
            get
            {
                return laserBeamsMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                laserBeamsMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum green color level (true color)
        /// </summary>
        public static int LaserBeamsMinimumGreenColorLevel
        {
            get
            {
                return laserBeamsMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                laserBeamsMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum blue color level (true color)
        /// </summary>
        public static int LaserBeamsMinimumBlueColorLevel
        {
            get
            {
                return laserBeamsMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                laserBeamsMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int LaserBeamsMinimumColorLevel
        {
            get
            {
                return laserBeamsMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                laserBeamsMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum red color level (true color)
        /// </summary>
        public static int LaserBeamsMaximumRedColorLevel
        {
            get
            {
                return laserBeamsMaximumRedColorLevel;
            }
            set
            {
                if (value <= laserBeamsMinimumRedColorLevel)
                    value = laserBeamsMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                laserBeamsMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum green color level (true color)
        /// </summary>
        public static int LaserBeamsMaximumGreenColorLevel
        {
            get
            {
                return laserBeamsMaximumGreenColorLevel;
            }
            set
            {
                if (value <= laserBeamsMinimumGreenColorLevel)
                    value = laserBeamsMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                laserBeamsMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum blue color level (true color)
        /// </summary>
        public static int LaserBeamsMaximumBlueColorLevel
        {
            get
            {
                return laserBeamsMaximumBlueColorLevel;
            }
            set
            {
                if (value <= laserBeamsMinimumBlueColorLevel)
                    value = laserBeamsMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                laserBeamsMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int LaserBeamsMaximumColorLevel
        {
            get
            {
                return laserBeamsMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= laserBeamsMinimumColorLevel)
                    value = laserBeamsMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                laserBeamsMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for LaserBeams
    /// </summary>
    public class LaserBeamsDisplay : BaseScreensaver, IScreensaver
    {

        private static readonly List<(int, int)> laserEnds = [];

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "LaserBeams";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            laserEnds.Clear();
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            ColorTools.LoadBackDry(new Color(LaserBeamsSettings.LaserBeamsBackgroundColor));

            // Populate the laser ends
            laserEnds.AddRange(
                [
                    (0, (ConsoleWrapper.WindowHeight * 5 / 5) - 1),
                    (0, (ConsoleWrapper.WindowHeight * 4 / 5) - 1),
                    (0, (ConsoleWrapper.WindowHeight * 3 / 5) - 1),
                    (0, (ConsoleWrapper.WindowHeight * 2 / 5) - 1),
                    (0, (ConsoleWrapper.WindowHeight / 5) - 1),
                    (0, 0),
                    ((ConsoleWrapper.WindowWidth / 5) - 1, 0),
                    ((ConsoleWrapper.WindowWidth * 2 / 5) - 1, 0),
                    ((ConsoleWrapper.WindowWidth * 3 / 5) - 1, 0),
                    ((ConsoleWrapper.WindowWidth * 4 / 5) - 1, 0),
                    ((ConsoleWrapper.WindowWidth * 5 / 5) - 1, 0),
                ]
            );

            // Draw few beams
            int laserStartPosX = ConsoleWrapper.WindowWidth - 1;
            int laserStartPosY = ConsoleWrapper.WindowHeight - 1;
            StringBuilder laserBeamsBuilder = new();
            for (int i = 0; i < 11; i++)
            {
                // Select a color
                Color colorStorage;
                if (LaserBeamsSettings.LaserBeamsTrueColor)
                {
                    int RedColorNum = RandomDriver.Random(LaserBeamsSettings.LaserBeamsMinimumRedColorLevel, LaserBeamsSettings.LaserBeamsMaximumRedColorLevel);
                    int GreenColorNum = RandomDriver.Random(LaserBeamsSettings.LaserBeamsMinimumGreenColorLevel, LaserBeamsSettings.LaserBeamsMaximumGreenColorLevel);
                    int BlueColorNum = RandomDriver.Random(LaserBeamsSettings.LaserBeamsMinimumBlueColorLevel, LaserBeamsSettings.LaserBeamsMaximumBlueColorLevel);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                    colorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                }
                else
                {
                    int color = RandomDriver.Random(LaserBeamsSettings.LaserBeamsMinimumColorLevel, LaserBeamsSettings.LaserBeamsMaximumColorLevel);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                    colorStorage = new Color(color);
                }

                // Get the laser end positions
                int laserEndPosX = laserEnds[i].Item1;
                int laserEndPosY = laserEnds[i].Item2;

                // Now, draw a laser beam
                double laserPosThresholdX = -(laserEndPosX - laserStartPosX) / (double)ConsoleWrapper.WindowWidth;
                double laserPosThresholdY = -(laserEndPosY - laserStartPosY) / ((double)ConsoleWrapper.WindowHeight * 4);
                int currentPosX = laserStartPosX;
                int currentPosY = laserStartPosY;
                int step = 0;
                while (currentPosX > 0 && currentPosY > 0)
                {
                    currentPosX = (int)(laserStartPosX - (laserPosThresholdX * step));
                    currentPosY = (int)(laserStartPosY - (laserPosThresholdY * step));
                    laserBeamsBuilder.Append($"{colorStorage.VTSequenceBackground}{CsiSequences.GenerateCsiCursorPosition(currentPosX + 1, currentPosY + 1)} ");
                    step++;
                }
            }

            // Make a beaming white particle to make it as if the lasers are fired
            laserBeamsBuilder.Append($"{new Color(ConsoleColors.White).VTSequenceBackground}{CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth + 1, ConsoleWrapper.WindowHeight + 1)} ");

            // Write the result
            TextWriterRaw.WritePlain(laserBeamsBuilder.ToString(), false);

            // Reset resize sync
            laserEnds.Clear();
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(LaserBeamsSettings.LaserBeamsDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
