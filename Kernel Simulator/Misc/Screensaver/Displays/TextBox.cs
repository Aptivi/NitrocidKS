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

using Terminaux.Inputs.Styles.Infobox;
using KS.Misc.Reflection;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for TextBox
    /// </summary>
    public static class TextBox
    {
        private static bool textBoxTrueColor = true;
        private static int textBoxDelay = 1000;
        private static string textBoxWrite = "Nitrocid KS";
        private static bool textBoxRainbowMode;
        private static int textBoxMinimumRedColorLevel = 0;
        private static int textBoxMinimumGreenColorLevel = 0;
        private static int textBoxMinimumBlueColorLevel = 0;
        private static int textBoxMinimumColorLevel = 0;
        private static int textBoxMaximumRedColorLevel = 255;
        private static int textBoxMaximumGreenColorLevel = 255;
        private static int textBoxMaximumBlueColorLevel = 255;
        private static int textBoxMaximumColorLevel = 255;

        /// <summary>
        /// [TextBox] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool TextBoxTrueColor
        {
            get
            {
                return textBoxTrueColor;
            }
            set
            {
                textBoxTrueColor = value;
            }
        }
        /// <summary>
        /// [TextBox] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int TextBoxDelay
        {
            get
            {
                return textBoxDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                textBoxDelay = value;
            }
        }
        /// <summary>
        /// [TextBox] TextBox for Bouncing TextBox. Shorter is better.
        /// </summary>
        public static string TextBoxWrite
        {
            get
            {
                return textBoxWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                textBoxWrite = value;
            }
        }
        /// <summary>
        /// [TextBox] Enables the rainbow colors mode
        /// </summary>
        public static bool TextBoxRainbowMode
        {
            get
            {
                return textBoxRainbowMode;
            }
            set
            {
                textBoxRainbowMode = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum red color level (true color)
        /// </summary>
        public static int TextBoxMinimumRedColorLevel
        {
            get
            {
                return textBoxMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textBoxMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum green color level (true color)
        /// </summary>
        public static int TextBoxMinimumGreenColorLevel
        {
            get
            {
                return textBoxMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textBoxMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum blue color level (true color)
        /// </summary>
        public static int TextBoxMinimumBlueColorLevel
        {
            get
            {
                return textBoxMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textBoxMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextBoxMinimumColorLevel
        {
            get
            {
                return textBoxMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                textBoxMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum red color level (true color)
        /// </summary>
        public static int TextBoxMaximumRedColorLevel
        {
            get
            {
                return textBoxMaximumRedColorLevel;
            }
            set
            {
                if (value <= textBoxMinimumRedColorLevel)
                    value = textBoxMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                textBoxMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum green color level (true color)
        /// </summary>
        public static int TextBoxMaximumGreenColorLevel
        {
            get
            {
                return textBoxMaximumGreenColorLevel;
            }
            set
            {
                if (value <= textBoxMinimumGreenColorLevel)
                    value = textBoxMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                textBoxMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum blue color level (true color)
        /// </summary>
        public static int TextBoxMaximumBlueColorLevel
        {
            get
            {
                return textBoxMaximumBlueColorLevel;
            }
            set
            {
                if (value <= textBoxMinimumBlueColorLevel)
                    value = textBoxMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                textBoxMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextBoxMaximumColorLevel
        {
            get
            {
                return textBoxMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= textBoxMinimumColorLevel)
                    value = textBoxMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                textBoxMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for TextBox
    /// </summary>
    public class TextBoxDisplay : BaseScreensaver, IScreensaver
    {

        private int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "TextBox";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            currentHueAngle = 0;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            Color color = ChangeTextBoxColor();
            string renderedTextBox = TextBox.TextBoxWrite;

            // Write the text
            if (TextBox.TextBoxRainbowMode)
            {
                color = new($"hsl:{currentHueAngle};100;50");
                currentHueAngle++;
                if (currentHueAngle > 360)
                    currentHueAngle = 0;
            }
            InfoBoxColor.WriteInfoBoxColor(renderedTextBox, false, color);

            // Delay
            int delay = TextBox.TextBoxRainbowMode ? 16 : TextBox.TextBoxDelay;
            ThreadManager.SleepNoBlock(delay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Changes the color of date and time
        /// </summary>
        private Color ChangeTextBoxColor()
        {
            Color ColorInstance;
            if (TextBox.TextBoxTrueColor)
            {
                int RedColorNum = RandomDriver.Random(TextBox.TextBoxMinimumRedColorLevel, TextBox.TextBoxMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(TextBox.TextBoxMinimumGreenColorLevel, TextBox.TextBoxMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(TextBox.TextBoxMinimumBlueColorLevel, TextBox.TextBoxMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(TextBox.TextBoxMinimumColorLevel, TextBox.TextBoxMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
