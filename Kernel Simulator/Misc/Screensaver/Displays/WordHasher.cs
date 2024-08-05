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
using Textify.Figlet;
using Terminaux.Colors;
using Textify.Data.Analysis.Words;
using KS.Misc.Screensaver;
using Terminaux.Writer.ConsoleWriters;
using KS.Languages;
using Terminaux.Writer.FancyWriters;
using KS.Misc.Threading;
using KS.Misc.Reflection;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for WordHasher
    /// </summary>
    public static class WordHasherSettings
    {
        private static bool wordHasherTrueColor = true;
        private static int wordHasherDelay = 1000;
        private static int wordHasherMinimumRedColorLevel = 0;
        private static int wordHasherMinimumGreenColorLevel = 0;
        private static int wordHasherMinimumBlueColorLevel = 0;
        private static int wordHasherMinimumColorLevel = 0;
        private static int wordHasherMaximumRedColorLevel = 255;
        private static int wordHasherMaximumGreenColorLevel = 255;
        private static int wordHasherMaximumBlueColorLevel = 255;
        private static int wordHasherMaximumColorLevel = 255;

        /// <summary>
        /// [WordHasher] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool WordHasherTrueColor
        {
            get
            {
                return wordHasherTrueColor;
            }
            set
            {
                wordHasherTrueColor = value;
            }
        }
        /// <summary>
        /// [WordHasher] How many milliseconds to wait before making the next ?
        /// </summary>
        public static int WordHasherDelay
        {
            get
            {
                return wordHasherDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                wordHasherDelay = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum red color level (true color)
        /// </summary>
        public static int WordHasherMinimumRedColorLevel
        {
            get
            {
                return wordHasherMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum green color level (true color)
        /// </summary>
        public static int WordHasherMinimumGreenColorLevel
        {
            get
            {
                return wordHasherMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum blue color level (true color)
        /// </summary>
        public static int WordHasherMinimumBlueColorLevel
        {
            get
            {
                return wordHasherMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int WordHasherMinimumColorLevel
        {
            get
            {
                return wordHasherMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                wordHasherMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum red color level (true color)
        /// </summary>
        public static int WordHasherMaximumRedColorLevel
        {
            get
            {
                return wordHasherMaximumRedColorLevel;
            }
            set
            {
                if (value <= wordHasherMinimumRedColorLevel)
                    value = wordHasherMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum green color level (true color)
        /// </summary>
        public static int WordHasherMaximumGreenColorLevel
        {
            get
            {
                return wordHasherMaximumGreenColorLevel;
            }
            set
            {
                if (value <= wordHasherMinimumGreenColorLevel)
                    value = wordHasherMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum blue color level (true color)
        /// </summary>
        public static int WordHasherMaximumBlueColorLevel
        {
            get
            {
                return wordHasherMaximumBlueColorLevel;
            }
            set
            {
                if (value <= wordHasherMinimumBlueColorLevel)
                    value = wordHasherMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int WordHasherMaximumColorLevel
        {
            get
            {
                return wordHasherMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= wordHasherMinimumColorLevel)
                    value = wordHasherMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                wordHasherMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for WordHasher
    /// </summary>
    public class WordHasherDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "WordHasher";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();

            // Write loading
            string word = Translate.DoTranslation("Loading...");
            string wordHash = Encryption.Encryption.GetEncryptedString(word, Encryption.Encryption.Algorithms.SHA256);
            var figFont = FigletTools.GetFigletFont("small");
            int figHeight = FigletTools.GetFigletHeight(word, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            int hashY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
            CenteredFigletTextColor.WriteCenteredFigletColor(consoleY, figFont, word, ConsoleColors.Green);
            TextWriterWhereColor.WriteWhereColor(wordHash, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordHash.Length / 2d), hashY, ConsoleColors.Green);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Change color
            var hasherColor = ChangeWordHasherColor();

            // Write word and hash
            string word = WordManager.GetRandomWord();
            string wordHash = Encryption.Encryption.GetEncryptedString(word, Encryption.Encryption.Algorithms.SHA256);
            var figFont = FigletTools.GetFigletFont("small");
            int figHeight = FigletTools.GetFigletHeight(word, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            int hashY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
            ConsoleWrapper.Clear();
            CenteredFigletTextColor.WriteCenteredFigletColor(consoleY, figFont, word, hasherColor);
            TextWriterWhereColor.WriteWhereColor(wordHash, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordHash.Length / 2d), hashY, hasherColor);

            // Delay
            ThreadManager.SleepNoBlock(WordHasherSettings.WordHasherDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Changes the color of word and its hash
        /// </summary>
        private Color ChangeWordHasherColor()
        {
            Color ColorInstance;
            if (WordHasherSettings.WordHasherTrueColor)
            {
                int RedColorNum = RandomDriver.Random(WordHasherSettings.WordHasherMinimumRedColorLevel, WordHasherSettings.WordHasherMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(WordHasherSettings.WordHasherMinimumGreenColorLevel, WordHasherSettings.WordHasherMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(WordHasherSettings.WordHasherMinimumBlueColorLevel, WordHasherSettings.WordHasherMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(WordHasherSettings.WordHasherMinimumColorLevel, WordHasherSettings.WordHasherMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
