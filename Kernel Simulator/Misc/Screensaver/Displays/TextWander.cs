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
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for TextWander
    /// </summary>
    public static class TextWanderSettings
    {
        private static bool textWanderTrueColor = true;
        private static int textWanderDelay = 1000;
        private static string textWanderWrite = "Nitrocid KS";
        private static int textWanderMinimumRedColorLevel = 0;
        private static int textWanderMinimumGreenColorLevel = 0;
        private static int textWanderMinimumBlueColorLevel = 0;
        private static int textWanderMinimumColorLevel = 0;
        private static int textWanderMaximumRedColorLevel = 255;
        private static int textWanderMaximumGreenColorLevel = 255;
        private static int textWanderMaximumBlueColorLevel = 255;
        private static int textWanderMaximumColorLevel = 255;

        /// <summary>
        /// [TextWander] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool TextWanderTrueColor
        {
            get
            {
                return textWanderTrueColor;
            }
            set
            {
                textWanderTrueColor = value;
            }
        }
        /// <summary>
        /// [TextWander] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int TextWanderDelay
        {
            get
            {
                return textWanderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                textWanderDelay = value;
            }
        }
        /// <summary>
        /// [TextWander] TextWander for Bouncing TextWander. Shorter is better.
        /// </summary>
        public static string TextWanderWrite
        {
            get
            {
                return textWanderWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                textWanderWrite = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum red color level (true color)
        /// </summary>
        public static int TextWanderMinimumRedColorLevel
        {
            get
            {
                return textWanderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textWanderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum green color level (true color)
        /// </summary>
        public static int TextWanderMinimumGreenColorLevel
        {
            get
            {
                return textWanderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textWanderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum blue color level (true color)
        /// </summary>
        public static int TextWanderMinimumBlueColorLevel
        {
            get
            {
                return textWanderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textWanderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextWanderMinimumColorLevel
        {
            get
            {
                return textWanderMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                textWanderMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum red color level (true color)
        /// </summary>
        public static int TextWanderMaximumRedColorLevel
        {
            get
            {
                return textWanderMaximumRedColorLevel;
            }
            set
            {
                if (value <= textWanderMinimumRedColorLevel)
                    value = textWanderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                textWanderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum green color level (true color)
        /// </summary>
        public static int TextWanderMaximumGreenColorLevel
        {
            get
            {
                return textWanderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= textWanderMinimumGreenColorLevel)
                    value = textWanderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                textWanderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum blue color level (true color)
        /// </summary>
        public static int TextWanderMaximumBlueColorLevel
        {
            get
            {
                return textWanderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= textWanderMinimumBlueColorLevel)
                    value = textWanderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                textWanderMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextWanderMaximumColorLevel
        {
            get
            {
                return textWanderMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= textWanderMinimumColorLevel)
                    value = textWanderMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                textWanderMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for TextWander
    /// </summary>
    public class TextWanderDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "TextWander";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            Color color = ChangeTextWanderColor();
            string renderedTextWander = TextWanderSettings.TextWanderWrite;
            int furthestX = ConsoleWrapper.WindowWidth - renderedTextWander.Length;
            int randomPosX = RandomDriver.RandomIdx(furthestX);
            int randomPosY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);

            // Write the text
            TextWriterWhereColor.WriteWhereColor(renderedTextWander, randomPosX, randomPosY, color);

            // Delay
            ThreadManager.SleepNoBlock(TextWanderSettings.TextWanderDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            TextWriterWhereColor.WriteWhereColor(new string(' ', renderedTextWander.Length), randomPosX, randomPosY, color);
        }

        /// <summary>
        /// Changes the color of date and time
        /// </summary>
        public Color ChangeTextWanderColor()
        {
            Color ColorInstance;
            if (TextWanderSettings.TextWanderTrueColor)
            {
                int RedColorNum = RandomDriver.Random(TextWanderSettings.TextWanderMinimumRedColorLevel, TextWanderSettings.TextWanderMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(TextWanderSettings.TextWanderMinimumGreenColorLevel, TextWanderSettings.TextWanderMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(TextWanderSettings.TextWanderMinimumBlueColorLevel, TextWanderSettings.TextWanderMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(TextWanderSettings.TextWanderMinimumColorLevel, TextWanderSettings.TextWanderMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
