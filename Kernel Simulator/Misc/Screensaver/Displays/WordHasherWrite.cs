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
using System.Text;
using Textify.Figlet;
using Terminaux.Colors;
using Textify.Data.Analysis.Words;
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
    /// Settings for WordHasherWrite
    /// </summary>
    public static class WordHasherWriteSettings
    {
        private static bool wordHasherWriteTrueColor = true;
        private static int wordHasherWriteDelay = 1000;
        private static int wordHasherWriteMinimumRedColorLevel = 0;
        private static int wordHasherWriteMinimumGreenColorLevel = 0;
        private static int wordHasherWriteMinimumBlueColorLevel = 0;
        private static int wordHasherWriteMinimumColorLevel = 0;
        private static int wordHasherWriteMaximumRedColorLevel = 255;
        private static int wordHasherWriteMaximumGreenColorLevel = 255;
        private static int wordHasherWriteMaximumBlueColorLevel = 255;
        private static int wordHasherWriteMaximumColorLevel = 255;

        /// <summary>
        /// [WordHasherWrite] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool WordHasherWriteTrueColor
        {
            get
            {
                return wordHasherWriteTrueColor;
            }
            set
            {
                wordHasherWriteTrueColor = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int WordHasherWriteDelay
        {
            get
            {
                return wordHasherWriteDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                wordHasherWriteDelay = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum red color level (true color)
        /// </summary>
        public static int WordHasherWriteMinimumRedColorLevel
        {
            get
            {
                return wordHasherWriteMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherWriteMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum green color level (true color)
        /// </summary>
        public static int WordHasherWriteMinimumGreenColorLevel
        {
            get
            {
                return wordHasherWriteMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherWriteMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum blue color level (true color)
        /// </summary>
        public static int WordHasherWriteMinimumBlueColorLevel
        {
            get
            {
                return wordHasherWriteMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherWriteMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int WordHasherWriteMinimumColorLevel
        {
            get
            {
                return wordHasherWriteMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                wordHasherWriteMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum red color level (true color)
        /// </summary>
        public static int WordHasherWriteMaximumRedColorLevel
        {
            get
            {
                return wordHasherWriteMaximumRedColorLevel;
            }
            set
            {
                if (value <= wordHasherWriteMinimumRedColorLevel)
                    value = wordHasherWriteMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherWriteMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum green color level (true color)
        /// </summary>
        public static int WordHasherWriteMaximumGreenColorLevel
        {
            get
            {
                return wordHasherWriteMaximumGreenColorLevel;
            }
            set
            {
                if (value <= wordHasherWriteMinimumGreenColorLevel)
                    value = wordHasherWriteMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherWriteMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum blue color level (true color)
        /// </summary>
        public static int WordHasherWriteMaximumBlueColorLevel
        {
            get
            {
                return wordHasherWriteMaximumBlueColorLevel;
            }
            set
            {
                if (value <= wordHasherWriteMinimumBlueColorLevel)
                    value = wordHasherWriteMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherWriteMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int WordHasherWriteMaximumColorLevel
        {
            get
            {
                return wordHasherWriteMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= wordHasherWriteMinimumColorLevel)
                    value = wordHasherWriteMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                wordHasherWriteMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for WordHasherWrite
    /// </summary>
    public class WordHasherWriteDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "WordHasherWrite";

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
            var hasherColor = ChangeWordHasherWriteColor();

            // Write word and hash
            string word = WordManager.GetRandomWord();
            var figFont = FigletTools.GetFigletFont("small");
            StringBuilder builtWord = new();
            for (int i = 0; i < word.Length; i++)
            {
                builtWord.Append(word[i]);
                string finalWord = builtWord.ToString();
                string wordHash = Encryption.Encryption.GetEncryptedString(finalWord, Encryption.Encryption.Algorithms.SHA256);
                int figHeight = FigletTools.GetFigletHeight(finalWord, figFont) / 2;
                int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                int hashY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
                ThreadManager.SleepNoBlock(250, ScreensaverDisplayer.ScreensaverDisplayerThread);
                ConsoleWrapper.Clear();
                CenteredFigletTextColor.WriteCenteredFigletColor(consoleY, figFont, finalWord, hasherColor);
                TextWriterWhereColor.WriteWhereColor(wordHash, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordHash.Length / 2d), hashY, hasherColor);
            }

            // Delay
            ThreadManager.SleepNoBlock(WordHasherWriteSettings.WordHasherWriteDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Destruct word
            for (int i = builtWord.Length - 1; i >= 0; i--)
            {
                builtWord.Remove(i, 1);
                if (builtWord.Length == 0)
                    break;
                string finalWord = builtWord.ToString();
                string wordHash = Encryption.Encryption.GetEncryptedString(finalWord, Encryption.Encryption.Algorithms.SHA256);
                int figHeight = FigletTools.GetFigletHeight(finalWord, figFont) / 2;
                int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                int hashY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
                ThreadManager.SleepNoBlock(250, ScreensaverDisplayer.ScreensaverDisplayerThread);
                ConsoleWrapper.Clear();
                CenteredFigletTextColor.WriteCenteredFigletColor(consoleY, figFont, finalWord, hasherColor);
                TextWriterWhereColor.WriteWhereColor(wordHash, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordHash.Length / 2d), hashY, hasherColor);
            }
        }

        /// <summary>
        /// Changes the color of word and its hash
        /// </summary>
        private Color ChangeWordHasherWriteColor()
        {
            Color ColorInstance;
            if (WordHasherWriteSettings.WordHasherWriteTrueColor)
            {
                int RedColorNum = RandomDriver.Random(WordHasherWriteSettings.WordHasherWriteMinimumRedColorLevel, WordHasherWriteSettings.WordHasherWriteMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(WordHasherWriteSettings.WordHasherWriteMinimumGreenColorLevel, WordHasherWriteSettings.WordHasherWriteMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(WordHasherWriteSettings.WordHasherWriteMinimumBlueColorLevel, WordHasherWriteSettings.WordHasherWriteMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(WordHasherWriteSettings.WordHasherWriteMinimumColorLevel, WordHasherWriteSettings.WordHasherWriteMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
