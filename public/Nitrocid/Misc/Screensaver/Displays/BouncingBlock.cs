﻿
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for BouncingBlock
    /// </summary>
    public static class BouncingBlockSettings
    {

        /// <summary>
        /// [BouncingBlock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool BouncingBlockTrueColor
        {
            get
            {
                return Config.SaverConfig.BouncingBlockTrueColor;
            }
            set
            {
                Config.SaverConfig.BouncingBlockTrueColor = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BouncingBlockDelay
        {
            get
            {
                return Config.SaverConfig.BouncingBlockDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                Config.SaverConfig.BouncingBlockDelay = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] Screensaver background color
        /// </summary>
        public static string BouncingBlockBackgroundColor
        {
            get
            {
                return Config.SaverConfig.BouncingBlockBackgroundColor;
            }
            set
            {
                Config.SaverConfig.BouncingBlockBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingBlock] Screensaver foreground color
        /// </summary>
        public static string BouncingBlockForegroundColor
        {
            get
            {
                return Config.SaverConfig.BouncingBlockForegroundColor;
            }
            set
            {
                Config.SaverConfig.BouncingBlockForegroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum red color level (true color)
        /// </summary>
        public static int BouncingBlockMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.BouncingBlockMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BouncingBlockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum green color level (true color)
        /// </summary>
        public static int BouncingBlockMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.BouncingBlockMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BouncingBlockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum blue color level (true color)
        /// </summary>
        public static int BouncingBlockMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.BouncingBlockMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BouncingBlockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BouncingBlockMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.BouncingBlockMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.BouncingBlockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum red color level (true color)
        /// </summary>
        public static int BouncingBlockMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.BouncingBlockMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BouncingBlockMinimumRedColorLevel)
                    value = Config.SaverConfig.BouncingBlockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BouncingBlockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum green color level (true color)
        /// </summary>
        public static int BouncingBlockMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.BouncingBlockMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BouncingBlockMinimumGreenColorLevel)
                    value = Config.SaverConfig.BouncingBlockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BouncingBlockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum blue color level (true color)
        /// </summary>
        public static int BouncingBlockMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.BouncingBlockMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BouncingBlockMinimumBlueColorLevel)
                    value = Config.SaverConfig.BouncingBlockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BouncingBlockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BouncingBlockMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.BouncingBlockMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.BouncingBlockMinimumColorLevel)
                    value = Config.SaverConfig.BouncingBlockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.BouncingBlockMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for BouncingBlock
    /// </summary>
    public class BouncingBlockDisplay : BaseScreensaver, IScreensaver
    {

        private string Direction = "BottomRight";
        private int RowBlock, ColumnBlock;
        private Color blockColor = null;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BouncingBlock";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RowBlock = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            ColumnBlock = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            KernelColorTools.SetConsoleColor(new Color(BouncingBlockSettings.BouncingBlockForegroundColor));
            KernelColorTools.LoadBack(new Color(BouncingBlockSettings.BouncingBlockBackgroundColor), true);
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Row block: {0} | Column block: {1}", RowBlock, ColumnBlock);

            // Initialize the color
            blockColor ??= GetRandomBlockColor();

            // Render a block
            if (!ConsoleResizeListener.WasResized(false))
            {
                TextWriterWhereColor.WriteWhere(" ", ColumnBlock, RowBlock, true, Color.Empty, blockColor);
            }
            else
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.W, "We're resize-syncing! Setting RowBlock and ColumnBlock to its original position...");
                RowBlock = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
                ColumnBlock = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
                ConsoleResizeListener.WasResized();
                ThreadManager.SleepNoBlock(BouncingBlockSettings.BouncingBlockDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                return;
            }

            if (RowBlock == ConsoleWrapper.WindowHeight - 2)
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "We're on the bottom.");
                Direction = Direction.Replace("Bottom", "Top");
                blockColor = GetRandomBlockColor();
            }
            else if (RowBlock == 1)
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "We're on the top.");
                Direction = Direction.Replace("Top", "Bottom");
                blockColor = GetRandomBlockColor();
            }

            if (ColumnBlock == ConsoleWrapper.WindowWidth - 1)
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "We're on the right.");
                Direction = Direction.Replace("Right", "Left");
                blockColor = GetRandomBlockColor();
            }
            else if (ColumnBlock == 1)
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "We're on the left.");
                Direction = Direction.Replace("Left", "Right");
                blockColor = GetRandomBlockColor();
            }

            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Block is facing {0}.", Direction);
            if (Direction == "BottomRight")
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Increasing row and column block position");
                RowBlock += 1;
                ColumnBlock += 1;
            }
            else if (Direction == "BottomLeft")
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Increasing row and decreasing column block position");
                RowBlock += 1;
                ColumnBlock -= 1;
            }
            else if (Direction == "TopRight")
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Decreasing row and increasing column block position");
                RowBlock -= 1;
                ColumnBlock += 1;
            }
            else if (Direction == "TopLeft")
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Decreasing row and column block position");
                RowBlock -= 1;
                ColumnBlock -= 1;
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(BouncingBlockSettings.BouncingBlockDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        private static Color GetRandomBlockColor()
        {
            if (BouncingBlockSettings.BouncingBlockTrueColor)
            {
                int RedColorNum = RandomDriver.Random(BouncingBlockSettings.BouncingBlockMinimumRedColorLevel, BouncingBlockSettings.BouncingBlockMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(BouncingBlockSettings.BouncingBlockMinimumGreenColorLevel, BouncingBlockSettings.BouncingBlockMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(BouncingBlockSettings.BouncingBlockMinimumBlueColorLevel, BouncingBlockSettings.BouncingBlockMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                return new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(BouncingBlockSettings.BouncingBlockMinimumColorLevel, BouncingBlockSettings.BouncingBlockMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                return new Color(ColorNum);
            }
        }

    }
}
