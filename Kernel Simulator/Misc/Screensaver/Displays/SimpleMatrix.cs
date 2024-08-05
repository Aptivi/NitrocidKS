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
    /// Settings for SimpleMatrix
    /// </summary>
    public static class SimpleMatrixSettings
    {
        private static int simpleMatrixDelay = 1;

        /// <summary>
        /// [SimpleMatrix] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SimpleMatrixDelay
        {
            get
            {
                return simpleMatrixDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                simpleMatrixDelay = value;
            }
        }
    }

    /// <summary>
    /// Display code for SimpleMatrix
    /// </summary>
    public class SimpleMatrixDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "SimpleMatrix";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            ColorTools.SetConsoleColor(ConsoleColors.Green);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            if (!ConsoleResizeHandler.WasResized(false))
            {
                ConsoleWrapper.Write(RandomDriver.Random(1).ToString());
            }
            else
            {
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...");
                ConsoleWrapper.Clear();
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(SimpleMatrixSettings.SimpleMatrixDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
