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

using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for NumberScatter
    /// </summary>
    public static class NumberScatterSettings
    {

        /// <summary>
        /// [NumberScatter] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int NumberScatterDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.NumberScatterDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                ScreensaverPackInit.SaversConfig.NumberScatterDelay = value;
            }
        }
        /// <summary>
        /// [NumberScatter] Screensaver background color
        /// </summary>
        public static string NumberScatterBackgroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.NumberScatterBackgroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.NumberScatterBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [NumberScatter] Screensaver foreground color
        /// </summary>
        public static string NumberScatterForegroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.NumberScatterForegroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.NumberScatterForegroundColor = new Color(value).PlainSequence;
            }
        }

    }

    /// <summary>
    /// Display code for NumberScatter
    /// </summary>
    public class NumberScatterDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "NumberScatter";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.SetConsoleColor(new Color(NumberScatterSettings.NumberScatterForegroundColor));
            ColorTools.LoadBackDry(new Color(NumberScatterSettings.NumberScatterBackgroundColor));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            ConsoleWrapper.SetCursorPosition(Left, Top);
            if (!ConsoleResizeHandler.WasResized(false))
            {
                ConsoleWrapper.Write(RandomDriver.Random(9).ToString());
            }
            else
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "Color-syncing. Clearing...");
                ConsoleWrapper.Clear();
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(NumberScatterSettings.NumberScatterDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
