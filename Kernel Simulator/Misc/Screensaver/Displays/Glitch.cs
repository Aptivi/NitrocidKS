﻿//
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
using System.Collections;
using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using Terminaux.Base;
using Terminaux.Colors;
namespace KS.Misc.Screensaver.Displays
{
    public static class GlitchSettings
    {

        private static int _GlitchDelay = 10;
        private static int _GlitchDensity = 40;

        /// <summary>
        /// [Glitch] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int GlitchDelay
        {
            get
            {
                return _GlitchDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _GlitchDelay = value;
            }
        }
        /// <summary>
        /// [Glitch] The Glitch density in percent
        /// </summary>
        public static int GlitchDensity
        {
            get
            {
                return _GlitchDensity;
            }
            set
            {
                if (value < 0)
                    value = 40;
                if (value > 100)
                    value = 40;
                _GlitchDensity = value;
            }
        }

    }

    public class GlitchDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;
        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;

        public override string ScreensaverName { get; set; } = "Glitch";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();
        }

        public override void ScreensaverLogic()
        {
            // Select random positions to generate the glitch
            double GlitchDense = (GlitchSettings.GlitchDensity > 100 ? 100 : GlitchSettings.GlitchDensity) / 100d;
            int AmountOfBlocks = ConsoleWrapper.WindowWidth * ConsoleWrapper.WindowHeight;
            int BlocksToCover = (int)Math.Round(AmountOfBlocks * GlitchDense);
            var CoveredBlocks = new ArrayList();
            while (!(CoveredBlocks.Count == BlocksToCover | ResizeSyncing))
            {
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    int CoverX = RandomDriver.Next(ConsoleWrapper.WindowWidth);
                    int CoverY = RandomDriver.Next(ConsoleWrapper.WindowHeight);
                    ConsoleWrapper.SetCursorPosition(CoverX, CoverY);

                    // Select random glitch type
                    GlitchType GlitchType = (GlitchType)Convert.ToInt32(Enum.Parse(typeof(GlitchType), RandomDriver.Next(5).ToString()));

                    // Select random letter
                    bool LetterCapitalized = Convert.ToBoolean(RandomDriver.Next(2));
                    int LetterRangeStart = LetterCapitalized ? 65 : 97;
                    int LetterRangeEnd = LetterCapitalized ? 90 : 122;
                    char Letter = Convert.ToChar(RandomDriver.Next(LetterRangeStart, LetterRangeEnd + 1));

                    // Select random symbol
                    bool UseExtendedAscii = Convert.ToBoolean(RandomDriver.Next(2));
                    int SymbolRangeStart = UseExtendedAscii ? 128 : 33;
                    int SymbolRangeEnd = UseExtendedAscii ? 256 : 64;
                    char Symbol = Convert.ToChar(RandomDriver.Next(SymbolRangeStart, SymbolRangeEnd + 1));

                    // Select red, green, or blue background and foreground
                    GlitchColorType GlitchBlockColorType = (GlitchColorType)Convert.ToInt32(Enum.Parse(typeof(GlitchColorType), RandomDriver.Next(3).ToString()));
                    GlitchColorType GlitchLetterColorType = (GlitchColorType)Convert.ToInt32(Enum.Parse(typeof(GlitchColorType), RandomDriver.Next(3).ToString()));
                    bool ColorLetter = Convert.ToBoolean(RandomDriver.Next(2));
                    int ColorBlockNumber = RandomDriver.Next(0, 256);
                    int ColorLetterNumber = RandomDriver.Next(0, 256);
                    var ColorBlockInstance = Color.Empty;
                    var ColorLetterInstance = Color.Empty;

                    // ...    for the block
                    switch (GlitchBlockColorType)
                    {
                        case GlitchColorType.Red:
                            {
                                ColorBlockInstance = new Color(ColorBlockNumber, 0, 0);
                                break;
                            }
                        case GlitchColorType.Green:
                            {
                                ColorBlockInstance = new Color(0, ColorBlockNumber, 0);
                                break;
                            }
                        case GlitchColorType.Blue:
                            {
                                ColorBlockInstance = new Color(0, 0, ColorBlockNumber);
                                break;
                            }
                    }

                    // ...and for the letter
                    switch (GlitchLetterColorType)
                    {
                        case GlitchColorType.Red:
                            {
                                ColorLetterInstance = new Color(ColorLetterNumber, 0, 0);
                                break;
                            }
                        case GlitchColorType.Green:
                            {
                                ColorLetterInstance = new Color(0, ColorLetterNumber, 0);
                                break;
                            }
                        case GlitchColorType.Blue:
                            {
                                ColorLetterInstance = new Color(0, 0, ColorLetterNumber);
                                break;
                            }
                    }

                    // Now, print based on the glitch type
                    switch (GlitchType)
                    {
                        case Displays.GlitchType.RandomLetter:
                            {
                                if (ColorLetter)
                                    KernelColorTools.SetConsoleColor(ColorLetterInstance);
                                else
                                    Console.ForegroundColor = ConsoleColor.White;
                                TextWriterColor.WritePlain(Convert.ToString(Letter), false);
                                break;
                            }
                        case Displays.GlitchType.RandomSymbol:
                            {
                                if (ColorLetter)
                                    KernelColorTools.SetConsoleColor(ColorLetterInstance);
                                else
                                    Console.ForegroundColor = ConsoleColor.White;
                                TextWriterColor.WritePlain(Convert.ToString(Symbol), false);
                                break;
                            }
                        case Displays.GlitchType.RedGreenBlueColor:
                            {
                                KernelColorTools.SetConsoleColor(ColorBlockInstance, true);
                                TextWriterColor.WritePlain(" ", false);
                                break;
                            }
                        case Displays.GlitchType.RedGreenBlueColorWithRandomLetter:
                            {
                                if (ColorLetter)
                                    KernelColorTools.SetConsoleColor(ColorLetterInstance);
                                else
                                    Console.ForegroundColor = ConsoleColor.White;
                                KernelColorTools.SetConsoleColor(ColorBlockInstance, true);
                                TextWriterColor.WritePlain(Convert.ToString(Letter), false);
                                break;
                            }
                        case Displays.GlitchType.RedGreenBlueColorWithRandomSymbol:
                            {
                                if (ColorLetter)
                                    KernelColorTools.SetConsoleColor(ColorLetterInstance);
                                else
                                    Console.ForegroundColor = ConsoleColor.White;
                                KernelColorTools.SetConsoleColor(ColorBlockInstance, true);
                                TextWriterColor.WritePlain(Convert.ToString(Symbol), false);
                                break;
                            }
                    }
                    if (!CoveredBlocks.Contains(CoverX.ToString() + ", " + CoverY.ToString()))
                        CoveredBlocks.Add(CoverX.ToString() + ", " + CoverY.ToString());
                }
                else
                {
                    // We're resizing.
                    ConsoleWrapper.CursorVisible = false;
                    break;
                }
                ThreadManager.SleepNoBlock(GlitchSettings.GlitchDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
        }

    }

    enum GlitchType
    {
        /// <summary>
        /// A block with either red, green, or blue as color, and can be darkened
        /// </summary>
        RedGreenBlueColor,
        /// <summary>
        /// A block with either red, green, or blue as color, and can be darkened and feature a random letter printed in white
        /// </summary>
        RedGreenBlueColorWithRandomLetter,
        /// <summary>
        /// A block with either red, green, or blue as color, and can be darkened and feature a random symbol printed in white
        /// </summary>
        RedGreenBlueColorWithRandomSymbol,
        /// <summary>
        /// A random letter printed in white, red, green, or blue, and can be darkened
        /// </summary>
        RandomLetter,
        /// <summary>
        /// A random symbol printed in white, red, green, or blue, and can be darkened
        /// </summary>
        RandomSymbol
    }

    enum GlitchColorType
    {
        /// <summary>
        /// The red color
        /// </summary>
        Red,
        /// <summary>
        /// The green color
        /// </summary>
        Green,
        /// <summary>
        /// The blue color
        /// </summary>
        Blue
    }
}