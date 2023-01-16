﻿
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.Base.Transfer;

namespace KS.Misc.Games
{
    static class Hangman
    {

        public static List<string> Words = new();

        /// <summary>
        /// Initializes the game
        /// </summary>
        public static void InitializeHangman()
        {
            string RandomWord;
            bool hung = false;
            int currentAttempt = 0;
            List<char> gotChars = new();
            List<char> wrongChars = new();

            // Download the words if not done
            if (Words.Count == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Downloading words...");
                Words.AddRange(NetworkTransfer.DownloadString("https://cdn.jsdelivr.net/gh/sindresorhus/word-list/words.txt").SplitNewLines().ToList());
            }

            // Now, select a random word
            RandomWord = Words[RandomDriver.RandomIdx(Words.Count)];

            while (!hung)
            {
                ConsoleWrapper.CursorVisible = false;
                ConsoleWrapper.Clear();

                // Draw how many underscores to place
                var underscoresRender = new StringBuilder();
                for (int i = 0; i < RandomWord.Length; i++)
                    underscoresRender.Append(gotChars.Contains(RandomWord[i]) ? $"{RandomWord[i]} " : "_ ");
                int underscoresPosX = (ConsoleWrapper.WindowWidth / 2) - (underscoresRender.Length / 2);
                int underscoresPosY = ConsoleWrapper.WindowHeight - 2;
                Color underscoresSeq = new(RandomDriver.Random(255), RandomDriver.Random(255), RandomDriver.Random(255));
                TextWriterWhereColor.WriteWhere(underscoresRender.ToString(), underscoresPosX, underscoresPosY, underscoresSeq);

                // Draw the wrong characters written
                string wrongRender = string.Join(" ", wrongChars);
                int wrongPosX = (ConsoleWrapper.WindowWidth / 2) - (wrongRender.Length / 2);
                int wrongPosY = ConsoleWrapper.WindowHeight - 4;
                Color wrongSeq = new(RandomDriver.Random(255), RandomDriver.Random(255), RandomDriver.Random(255));
                TextWriterWhereColor.WriteWhere(wrongRender.ToString(), wrongPosX, wrongPosY, wrongSeq);

                // Draw the hanger
                string hangerSprite =
                    @" /---------\" + "\n" +
                    @" |         |" + "\n" +
                    @" |         |" + "\n" +
                    @" |          " + "\n" +
                    @" |          " + "\n" +
                    @" |          " + "\n" +
                    @" |          " + "\n" +
                    @" |          " + "\n" +
                    @"---         " + "\n"
                ;
                int hangerPosX = 3;
                int hangerPosY = 2;
                Color hangerSeq = new(RandomDriver.Random(255), RandomDriver.Random(255), RandomDriver.Random(255));
                TextWriterWhereColor.WriteWhere(hangerSprite, hangerPosX, hangerPosY, hangerSeq);

                // Now, draw the hung man based on attempts
                if (currentAttempt >= 1)
                {
                    // Draw the head
                    int headPosX = 14;
                    int headPosY = 5;
                    TextWriterWhereColor.WriteWhere("*", headPosX, headPosY, hangerSeq);
                }
                if (currentAttempt >= 2)
                {
                    // Draw the body
                    int bodyPosX = 14;
                    int bodyPosY = 6;
                    TextWriterWhereColor.WriteWhere("|", bodyPosX, bodyPosY, hangerSeq);
                    TextWriterWhereColor.WriteWhere("|", bodyPosX, bodyPosY + 1, hangerSeq);
                }
                if (currentAttempt >= 3)
                {
                    // Draw the first hand
                    int handPosX = 13;
                    int handPosY = 6;
                    TextWriterWhereColor.WriteWhere("-", handPosX, handPosY, hangerSeq);
                }
                if (currentAttempt >= 4)
                {
                    // Draw the second hand
                    int handPosX = 15;
                    int handPosY = 6;
                    TextWriterWhereColor.WriteWhere("-", handPosX, handPosY, hangerSeq);
                }
                if (currentAttempt >= 5)
                {
                    // Draw the first leg
                    int legPosX = 13;
                    int legPosY = 8;
                    TextWriterWhereColor.WriteWhere("/", legPosX, legPosY, hangerSeq);
                }
                if (currentAttempt >= 6)
                {
                    // Draw the second leg
                    int legPosX = 15;
                    int legPosY = 8;
                    TextWriterWhereColor.WriteWhere("\\", legPosX, legPosY, hangerSeq);
                    hung = true;
                }

                // Check to see if we won the game
                bool won = false;
                List<char> queried = new();
                foreach (char ch in RandomWord)
                {
                    if (gotChars.Contains(ch))
                        queried.Add(ch);
                    if (string.Join("", queried) == RandomWord)
                        won = true;
                }

                // See if the man is completely hung
                if (hung)
                {
                    // Lost the game
                    string hungStr = Translate.DoTranslation("You're hung!");
                    int hungPosX = (ConsoleWrapper.WindowWidth / 2) - (hungStr.Length / 2);
                    int hungPosY = ConsoleWrapper.WindowHeight - 6;
                    TextWriterWhereColor.WriteWhere(hungStr, hungPosX, hungPosY, hangerSeq);
                    Thread.Sleep(5000);
                }
                else if (won)
                {
                    // Won the game
                    hung = true;
                    string wonStr = Translate.DoTranslation("You win!");
                    int wonPosX = (ConsoleWrapper.WindowWidth / 2) - (wonStr.Length / 2);
                    int wonPosY = ConsoleWrapper.WindowHeight - 6;
                    TextWriterWhereColor.WriteWhere(wonStr, wonPosX, wonPosY, hangerSeq);
                    Thread.Sleep(5000);
                }
                else
                {
                    // Wait for character
                    var input = Input.DetectKeypress();
                    var inputChar = input.KeyChar;
                    if (RandomWord.Contains(inputChar))
                    {
                        // Right character! Add it to the right characters list
                        if (!gotChars.Contains(inputChar))
                            gotChars.Add(inputChar);
                    }
                    else if (input.Key == ConsoleKey.Escape)
                    {
                        // Escaping.
                        hung = true;
                    }
                    else
                    {
                        // Wrong character!
                        if (!wrongChars.Contains(inputChar))
                        {
                            currentAttempt++;
                            wrongChars.Add(inputChar);
                        }
                    }
                }
            }

            // Clear after ourselves
            ConsoleWrapper.CursorVisible = true;
            ConsoleWrapper.Clear();
        }

    }
}
