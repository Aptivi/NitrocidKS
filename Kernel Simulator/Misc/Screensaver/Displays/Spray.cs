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
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Spray
    /// </summary>
    public static class SpraySettings
    {
        private static int sprayDelay = 10;

        /// <summary>
        /// [Spray] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SprayDelay
        {
            get
            {
                return sprayDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                sprayDelay = value;
            }
        }
    }

    /// <summary>
    /// Display code for Spray
    /// </summary>
    public class SprayDisplay : BaseScreensaver, IScreensaver
    {

        private bool moveUp = false;
        private int offsetY = 0;
        private readonly List<Tuple<int, int>> Stars = [];

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Spray";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            Stars.Clear();
            offsetY = 0;
            ColorTools.LoadBackDry(0);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            moveUp = RandomDriver.RandomBoolean();
            ConsoleWrapper.CursorVisible = false;

            // Move the stars left
            for (int Star = 0; Star <= Stars.Count - 1; Star++)
            {
                int newStarX = Stars[Star].Item1 - 1;
                int newStarY = Stars[Star].Item2;
                Stars[Star] = new Tuple<int, int>(newStarX, newStarY);
            }

            // If any star is out of X range, delete it
            for (int StarIndex = Stars.Count - 1; StarIndex >= 0; StarIndex -= 1)
            {
                var Star = Stars[StarIndex];
                if (Star.Item1 < 0)
                {
                    // The star went beyond. Remove it.
                    Stars.RemoveAt(StarIndex);
                }
            }

            // Add new star
            int StarX = ConsoleWrapper.WindowWidth - 1;
            int StarY = (ConsoleWrapper.WindowHeight / 2) - offsetY;
            if (moveUp)
            {
                offsetY++;
                if ((ConsoleWrapper.WindowHeight / 2) - offsetY < 0)
                    offsetY--;
            }
            else
            {
                offsetY--;
                if ((ConsoleWrapper.WindowHeight / 2) - offsetY >= ConsoleWrapper.WindowHeight)
                    offsetY++;
            }
            Stars.Add(new Tuple<int, int>(StarX, StarY));

            // Draw stars
            var starsBuffer = new StringBuilder();
            for (int StarIndex = Stars.Count - 1; StarIndex >= 0; StarIndex -= 1)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                var Star = Stars[StarIndex];
                char StarSymbol = '*';
                starsBuffer.Append(TextWriterWhereColor.RenderWhere(Convert.ToString(StarSymbol), Star.Item1, Star.Item2, false, ConsoleColors.White, ConsoleColors.Black));
            }
            if (ConsoleResizeHandler.WasResized(false))
            {
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...");
                Stars.Clear();
                offsetY = 0;
            }
            else
                TextWriterRaw.WritePlain(starsBuffer.ToString(), false);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(SpraySettings.SprayDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            ColorTools.LoadBackDry(0);
        }

    }
}
