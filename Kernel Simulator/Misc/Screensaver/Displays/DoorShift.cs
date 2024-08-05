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

using KS.Misc.Reflection;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for DoorShift
    /// </summary>
    public static class DoorShiftSettings
    {
        private static bool doorShiftTrueColor = true;
        private static int doorShiftDelay = 10;
        private static string doorShiftBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private static int doorShiftMinimumRedColorLevel = 0;
        private static int doorShiftMinimumGreenColorLevel = 0;
        private static int doorShiftMinimumBlueColorLevel = 0;
        private static int doorShiftMinimumColorLevel = 0;
        private static int doorShiftMaximumRedColorLevel = 255;
        private static int doorShiftMaximumGreenColorLevel = 255;
        private static int doorShiftMaximumBlueColorLevel = 255;
        private static int doorShiftMaximumColorLevel = 255;

        /// <summary>
        /// [DoorShift] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool DoorShiftTrueColor
        {
            get
            {
                return doorShiftTrueColor;
            }
            set
            {
                doorShiftTrueColor = value;
            }
        }
        /// <summary>
        /// [DoorShift] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int DoorShiftDelay
        {
            get
            {
                return doorShiftDelay;
            }
            set
            {
                doorShiftDelay = value;
            }
        }
        /// <summary>
        /// [DoorShift] Screensaver background color
        /// </summary>
        public static string DoorShiftBackgroundColor
        {
            get
            {
                return doorShiftBackgroundColor;
            }
            set
            {
                doorShiftBackgroundColor = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum red color level (true color)
        /// </summary>
        public static int DoorShiftMinimumRedColorLevel
        {
            get
            {
                return doorShiftMinimumRedColorLevel;
            }
            set
            {
                doorShiftMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum green color level (true color)
        /// </summary>
        public static int DoorShiftMinimumGreenColorLevel
        {
            get
            {
                return doorShiftMinimumGreenColorLevel;
            }
            set
            {
                doorShiftMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum blue color level (true color)
        /// </summary>
        public static int DoorShiftMinimumBlueColorLevel
        {
            get
            {
                return doorShiftMinimumBlueColorLevel;
            }
            set
            {
                doorShiftMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int DoorShiftMinimumColorLevel
        {
            get
            {
                return doorShiftMinimumColorLevel;
            }
            set
            {
                doorShiftMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum red color level (true color)
        /// </summary>
        public static int DoorShiftMaximumRedColorLevel
        {
            get
            {
                return doorShiftMaximumRedColorLevel;
            }
            set
            {
                doorShiftMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum green color level (true color)
        /// </summary>
        public static int DoorShiftMaximumGreenColorLevel
        {
            get
            {
                return doorShiftMaximumGreenColorLevel;
            }
            set
            {
                doorShiftMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum blue color level (true color)
        /// </summary>
        public static int DoorShiftMaximumBlueColorLevel
        {
            get
            {
                return doorShiftMaximumBlueColorLevel;
            }
            set
            {
                doorShiftMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int DoorShiftMaximumColorLevel
        {
            get
            {
                return doorShiftMaximumColorLevel;
            }
            set
            {
                doorShiftMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for DoorShift
    /// </summary>
    public class DoorShiftDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "DoorShift";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBackDry(new Color(DoorShiftSettings.DoorShiftBackgroundColor));
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Whether the door is closing or opening
            bool isClosing = RandomDriver.RandomChance(30);

            // Select a color
            if (DoorShiftSettings.DoorShiftTrueColor)
            {
                int RedColorNum = RandomDriver.Random(DoorShiftSettings.DoorShiftMinimumRedColorLevel, DoorShiftSettings.DoorShiftMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(DoorShiftSettings.DoorShiftMinimumGreenColorLevel, DoorShiftSettings.DoorShiftMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(DoorShiftSettings.DoorShiftMinimumBlueColorLevel, DoorShiftSettings.DoorShiftMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                    ColorTools.SetConsoleColorDry(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), true);
            }
            else
            {
                int ColorNum = RandomDriver.Random(DoorShiftSettings.DoorShiftMinimumColorLevel, DoorShiftSettings.DoorShiftMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                    ColorTools.SetConsoleColorDry(new Color(ColorNum), true);
            }

            // Set max height and width
            int MaxWindowHeight = ConsoleWrapper.WindowHeight - 1;
            int halfWidth = ConsoleWrapper.WindowWidth / 2;
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Max height {0}", MaxWindowHeight);
            if (isClosing)
            {
                for (int column = 0; column <= halfWidth; column++)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;
                    for (int Row = 0; Row <= MaxWindowHeight; Row++)
                    {
                        if (ConsoleResizeHandler.WasResized(false))
                            break;

                        // Check the positions
                        int leftDoorPos = column;
                        int rightDoorPos = ConsoleWrapper.WindowWidth - column - 1;
                        if (leftDoorPos > halfWidth)
                            leftDoorPos = halfWidth;
                        if (rightDoorPos < halfWidth)
                            rightDoorPos = halfWidth;

                        // Do the actual writing
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Setting position to {0}", column - 1, Row);
                        ConsoleWrapper.SetCursorPosition(leftDoorPos, Row);
                        ConsoleWrapper.Write(" ");
                        ConsoleWrapper.SetCursorPosition(rightDoorPos, Row);
                        ConsoleWrapper.Write(" ");
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", ConsoleWrapper.WindowWidth - column + 1);
                    }
                    ThreadManager.SleepNoBlock(DoorShiftSettings.DoorShiftDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                }
            }
            else
            {
                for (int column = 0; column <= halfWidth; column++)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;
                    for (int Row = 0; Row <= MaxWindowHeight; Row++)
                    {
                        if (ConsoleResizeHandler.WasResized(false))
                            break;

                        // Check the positions
                        int leftDoorPos = halfWidth - column;
                        int rightDoorPos = halfWidth + column;
                        if (leftDoorPos < 0)
                            leftDoorPos = 0;
                        if (rightDoorPos >= ConsoleWrapper.WindowWidth)
                            rightDoorPos = ConsoleWrapper.WindowWidth - 1;

                        // Do the actual writing
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Setting position to {0}", column - 1, Row);
                        ConsoleWrapper.SetCursorPosition(leftDoorPos, Row);
                        ConsoleWrapper.Write(" ");
                        ConsoleWrapper.SetCursorPosition(rightDoorPos, Row);
                        ConsoleWrapper.Write(" ");
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", ConsoleWrapper.WindowWidth - column + 1);
                    }
                    ThreadManager.SleepNoBlock(DoorShiftSettings.DoorShiftDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                }
            }

            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(DoorShiftSettings.DoorShiftDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
