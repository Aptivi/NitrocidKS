﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Wipe
    /// </summary>
    public class WipeDisplay : BaseScreensaver, IScreensaver
    {

        private WipeDirections ToDirection = WipeDirections.Right;
        private int TimesWiped = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Wipe";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.WipeBackgroundColor));
            ConsoleWrapper.CursorVisible = false;
            TimesWiped = 0;
            ToDirection = WipeDirections.Right;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a color
            if (ScreensaverPackInit.SaversConfig.WipeTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WipeMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.WipeMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WipeMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.WipeMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WipeMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.WipeMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                    ColorTools.SetConsoleColorDry(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), true);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WipeMinimumColorLevel, ScreensaverPackInit.SaversConfig.WipeMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                    ColorTools.SetConsoleColorDry(new Color(ColorNum), true);
            }

            // Set max height
            int MaxWindowHeight = ConsoleWrapper.WindowHeight - 1;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Max height {0}", MaxWindowHeight);

            // Print a space {Column} times until the entire screen is wiped.
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Wipe direction {0}", ToDirection.ToString());
            switch (ToDirection)
            {
                case WipeDirections.Right:
                    {
                        for (int Column = 0; Column <= ConsoleWrapper.WindowWidth; Column++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            for (int Row = 0; Row <= MaxWindowHeight; Row++)
                            {
                                if (ConsoleResizeHandler.WasResized(false))
                                    break;

                                // Do the actual writing
                                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Setting Y position to {0}", Row);
                                ConsoleWrapper.SetCursorPosition(0, Row);
                                ConsoleWrapper.Write(new string(' ', Column));
                                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", Column);
                            }
                            ThreadManager.SleepNoBlock(ScreensaverPackInit.SaversConfig.WipeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }

                        break;
                    }
                case WipeDirections.Left:
                    {
                        for (int Column = ConsoleWrapper.WindowWidth; Column >= 1; Column -= 1)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            for (int Row = 0; Row <= MaxWindowHeight; Row++)
                            {
                                if (ConsoleResizeHandler.WasResized(false))
                                    break;

                                // Do the actual writing
                                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Setting position to {0}", Column - 1, Row);
                                ConsoleWrapper.SetCursorPosition(Column - 1, Row);
                                ConsoleWrapper.Write(new string(' ', ConsoleWrapper.WindowWidth - Column + 1));
                                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", ConsoleWrapper.WindowWidth - Column + 1);
                            }
                            ThreadManager.SleepNoBlock(ScreensaverPackInit.SaversConfig.WipeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }

                        break;
                    }
                case WipeDirections.Top:
                    {
                        for (int Row = MaxWindowHeight; Row >= 0; Row -= 1)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Do the actual writing
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Setting Y position to {0}", Row);
                            ConsoleWrapper.SetCursorPosition(0, Row);
                            ConsoleWrapper.Write(new string(' ', ConsoleWrapper.WindowWidth));
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", ConsoleWrapper.WindowWidth);
                            ThreadManager.SleepNoBlock(ScreensaverPackInit.SaversConfig.WipeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }

                        break;
                    }
                case WipeDirections.Bottom:
                    {
                        for (int Row = 0; Row <= MaxWindowHeight; Row++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Do the actual writing
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", ConsoleWrapper.WindowWidth);
                            ConsoleWrapper.Write(new string(' ', ConsoleWrapper.WindowWidth));
                            ThreadManager.SleepNoBlock(ScreensaverPackInit.SaversConfig.WipeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        ConsoleWrapper.SetCursorPosition(0, 0);
                        break;
                    }
            }

            if (!ConsoleResizeHandler.WasResized(false))
            {
                TimesWiped += 1;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Wiped {0} times out of {1}", TimesWiped, ScreensaverPackInit.SaversConfig.WipeWipesNeededToChangeDirection);

                // Check if the number of times wiped is equal to the number of required times to change wiping direction.
                if (TimesWiped == ScreensaverPackInit.SaversConfig.WipeWipesNeededToChangeDirection)
                {
                    TimesWiped = 0;
                    ToDirection = (WipeDirections)Convert.ToInt32(Enum.Parse(typeof(WipeDirections), RandomDriver.Random(3).ToString()));
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Changed direction to {0}", ToDirection.ToString());
                }
            }
            else
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...");
                ColorTools.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.WipeBackgroundColor));
            }

            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(ScreensaverPackInit.SaversConfig.WipeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Wipe directions
        /// </summary>
        private enum WipeDirections
        {
            /// <summary>
            /// Wipe from right to left
            /// </summary>
            Left,
            /// <summary>
            /// Wipe from left to right
            /// </summary>
            Right,
            /// <summary>
            /// Wipe from bottom to top
            /// </summary>
            Top,
            /// <summary>
            /// Wipe from top to bottom
            /// </summary>
            Bottom
        }

    }
}
