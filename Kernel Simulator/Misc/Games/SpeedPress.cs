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
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.ConsoleBase.Writers;
namespace KS.Misc.Games
{
    public static class SpeedPress
    {

        public static SpeedPressDifficulty SpeedPressCurrentDifficulty = SpeedPressDifficulty.Medium;
        public static int SpeedPressTimeout = 3000;

        /// <summary>
        /// SpeedPress difficulty
        /// </summary>
        public enum SpeedPressDifficulty
        {
            /// <summary>
            /// Easy difficulty (timeout for three seconds)
            /// </summary>
            Easy,
            /// <summary>
            /// Medium difficulty (timeout for a second and a half)
            /// </summary>
            Medium,
            /// <summary>
            /// Hard difficulty (timeout for a half second)
            /// </summary>
            Hard,
            /// <summary>
            /// Very hard difficulty (timeout for a quarter of a second)
            /// </summary>
            VeryHard,
            /// <summary>
            /// Custom difficulty (custom timeout according to either a switch or the kernel settings)
            /// </summary>
            Custom
        }

        /// <summary>
        /// Initializes the SpeedPress game
        /// </summary>
        /// <param name="Difficulty">The difficulty of the game</param>
        public static void InitializeSpeedPress(SpeedPressDifficulty Difficulty, int CustomTimeout = 0)
        {
            var SpeedTimeout = default(int);
            char SelectedChar;
            var WrittenChar = default(ConsoleKeyInfo);
            var RandomEngine = new Random();

            // Change timeout based on difficulty
            switch (Difficulty)
            {
                case SpeedPressDifficulty.Easy:
                    {
                        SpeedTimeout = 3000;
                        break;
                    }
                case SpeedPressDifficulty.Medium:
                    {
                        SpeedTimeout = 1500;
                        break;
                    }
                case SpeedPressDifficulty.Hard:
                    {
                        SpeedTimeout = 500;
                        break;
                    }
                case SpeedPressDifficulty.VeryHard:
                    {
                        SpeedTimeout = 250;
                        break;
                    }
                case SpeedPressDifficulty.Custom:
                    {
                        if (CustomTimeout > 0)
                        {
                            SpeedTimeout = Math.Abs(CustomTimeout);
                        }
                        else if (SpeedPressTimeout > 0)
                        {
                            SpeedTimeout = Math.Abs(SpeedPressTimeout);
                        }
                        else
                        {
                            SpeedTimeout = 1500;
                        }

                        break;
                    }
            }

            // Enter the loop until the user presses ESC
            TextWriters.Write(Translate.DoTranslation("Press ESC to exit.") + Kernel.Kernel.NewLine, true, KernelColorTools.ColTypes.Tip);
            while (!(WrittenChar.Key == ConsoleKey.Escape) | !(WrittenChar.Modifiers == ConsoleModifiers.Control) & WrittenChar.Key == ConsoleKey.C)
            {
                // Select a random character
                SelectedChar = Convert.ToChar(RandomEngine.Next(97, 122));

                // Prompt user for character
                try
                {
                    TextWriters.Write(Translate.DoTranslation("Current character:") + " {0}", true, KernelColorTools.ColTypes.Neutral, SelectedChar);
                    TextWriters.Write("> ", false, KernelColorTools.ColTypes.Input);
                    WrittenChar = Input.ReadKeyTimeout(false, TimeSpan.FromMilliseconds(SpeedTimeout));
                    TextWriters.Write("", KernelColorTools.ColTypes.Neutral);

                    // Check to see if the user has pressed the correct character
                    if (WrittenChar.KeyChar == SelectedChar)
                    {
                        TextWriters.Write(Translate.DoTranslation("You've pressed the right character!"), true, KernelColorTools.ColTypes.Success);
                    }
                    else if (!(WrittenChar.Key == ConsoleKey.Escape) | !(WrittenChar.Modifiers == ConsoleModifiers.Control) & WrittenChar.Key == ConsoleKey.C)
                    {
                        TextWriters.Write(Translate.DoTranslation("You've pressed the wrong character."), true, KernelColorTools.ColTypes.Warning);
                    }
                }
                catch (Kernel.Exceptions.ConsoleReadTimeoutException)
                {
                    TextWriters.Write("", KernelColorTools.ColTypes.Neutral);
                    TextWriters.Write(Translate.DoTranslation("Character not pressed on time."), true, KernelColorTools.ColTypes.Warning);
                }
            }
        }

    }
}