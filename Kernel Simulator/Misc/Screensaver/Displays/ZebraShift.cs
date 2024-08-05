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

using Terminaux.Writer.ConsoleWriters;
using KS.Misc.Reflection;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for ZebraShift
    /// </summary>
    public static class ZebraShiftSettings
    {
        private static bool zebraShiftTrueColor = true;
        private static int zebraShiftDelay = 25;
        private static int zebraShiftMinimumRedColorLevel = 0;
        private static int zebraShiftMinimumGreenColorLevel = 0;
        private static int zebraShiftMinimumBlueColorLevel = 0;
        private static int zebraShiftMinimumColorLevel = 0;
        private static int zebraShiftMaximumRedColorLevel = 255;
        private static int zebraShiftMaximumGreenColorLevel = 255;
        private static int zebraShiftMaximumBlueColorLevel = 255;
        private static int zebraShiftMaximumColorLevel = 255;

        /// <summary>
        /// [ZebraShift] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool ZebraShiftTrueColor
        {
            get
            {
                return zebraShiftTrueColor;
            }
            set
            {
                zebraShiftTrueColor = value;
            }
        }
        /// <summary>
        /// [ZebraShift] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int ZebraShiftDelay
        {
            get
            {
                return zebraShiftDelay;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                zebraShiftDelay = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum red color level (true color)
        /// </summary>
        public static int ZebraShiftMinimumRedColorLevel
        {
            get
            {
                return zebraShiftMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                zebraShiftMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum green color level (true color)
        /// </summary>
        public static int ZebraShiftMinimumGreenColorLevel
        {
            get
            {
                return zebraShiftMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                zebraShiftMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum blue color level (true color)
        /// </summary>
        public static int ZebraShiftMinimumBlueColorLevel
        {
            get
            {
                return zebraShiftMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                zebraShiftMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ZebraShiftMinimumColorLevel
        {
            get
            {
                return zebraShiftMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                zebraShiftMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum red color level (true color)
        /// </summary>
        public static int ZebraShiftMaximumRedColorLevel
        {
            get
            {
                return zebraShiftMaximumRedColorLevel;
            }
            set
            {
                if (value <= zebraShiftMinimumRedColorLevel)
                    value = zebraShiftMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                zebraShiftMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum green color level (true color)
        /// </summary>
        public static int ZebraShiftMaximumGreenColorLevel
        {
            get
            {
                return zebraShiftMaximumGreenColorLevel;
            }
            set
            {
                if (value <= zebraShiftMinimumGreenColorLevel)
                    value = zebraShiftMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                zebraShiftMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum blue color level (true color)
        /// </summary>
        public static int ZebraShiftMaximumBlueColorLevel
        {
            get
            {
                return zebraShiftMaximumBlueColorLevel;
            }
            set
            {
                if (value <= zebraShiftMinimumBlueColorLevel)
                    value = zebraShiftMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                zebraShiftMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ZebraShiftMaximumColorLevel
        {
            get
            {
                return zebraShiftMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= zebraShiftMinimumColorLevel)
                    value = zebraShiftMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                zebraShiftMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for ZebraShift
    /// </summary>
    public class ZebraShiftDisplay : BaseScreensaver, IScreensaver
    {

        private static Color firstLineColor = ConsoleColors.White;
        private static Color secondLineColor = ConsoleColors.Black;
        private static bool inverse = false;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "ZebraShift";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();

            // Select two colors for two lines
            if (ZebraShiftSettings.ZebraShiftTrueColor)
            {
                int firstGroupRedColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumRedColorLevel, ZebraShiftSettings.ZebraShiftMaximumRedColorLevel);
                int firstGroupGreenColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumGreenColorLevel, ZebraShiftSettings.ZebraShiftMaximumGreenColorLevel);
                int firstGroupBlueColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumBlueColorLevel, ZebraShiftSettings.ZebraShiftMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color for first group (R;G;B: {0};{1};{2})", firstGroupRedColorNum, firstGroupGreenColorNum, firstGroupBlueColorNum);
                int secondGroupRedColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumRedColorLevel, ZebraShiftSettings.ZebraShiftMaximumRedColorLevel);
                int secondGroupGreenColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumGreenColorLevel, ZebraShiftSettings.ZebraShiftMaximumGreenColorLevel);
                int secondGroupBlueColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumBlueColorLevel, ZebraShiftSettings.ZebraShiftMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color for second group (R;G;B: {0};{1};{2})", secondGroupRedColorNum, secondGroupGreenColorNum, secondGroupBlueColorNum);
                var firstGroupColorStorage = new Color(firstGroupRedColorNum, firstGroupGreenColorNum, firstGroupBlueColorNum);
                var secondGroupColorStorage = new Color(secondGroupRedColorNum, secondGroupGreenColorNum, secondGroupBlueColorNum);
                firstLineColor = firstGroupColorStorage;
                secondLineColor = secondGroupColorStorage;
            }
            else
            {
                int firstGroupColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumColorLevel, ZebraShiftSettings.ZebraShiftMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color for first group ({0})", firstGroupColorNum);
                int secondGroupColorNum = RandomDriver.Random(ZebraShiftSettings.ZebraShiftMinimumColorLevel, ZebraShiftSettings.ZebraShiftMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color for second group ({0})", secondGroupColorNum);
                firstLineColor = firstGroupColorNum;
                secondLineColor = secondGroupColorNum;
            }
            inverse = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Prepare the string buffer
            var zebraBuffer = new StringBuilder();

            // Now, make the two zebra lines
            int width = ConsoleWrapper.WindowWidth;
            int height = ConsoleWrapper.WindowHeight;
            var finalFirstColor = inverse ? secondLineColor : firstLineColor;
            var finalSecondColor = inverse ? firstLineColor : secondLineColor;
            for (int x = 0; x < width; x++)
            {
                // Select a color
                bool useOther = x % 2 == 0;
                var color = useOther ? finalSecondColor : finalFirstColor;

                // Fill the line vertically
                zebraBuffer.Append(color.VTSequenceBackground);
                for (int y = 0; y < height; y++)
                {
                    zebraBuffer.Append(
                        CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1) +
                        " "
                    );
                }
            }
            inverse = !inverse;

            // Write the shift buffer
            TextWriterRaw.WritePlain(zebraBuffer.ToString(), false);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(ZebraShiftSettings.ZebraShiftDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
