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
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Misc.Text;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using KS.Network.Transfer;

namespace KS.Misc.Games
{
    static class Speller
    {

        public static List<string> Words = [];

        /// <summary>
        /// Initializes the game
        /// </summary>
        public static void InitializeWords()
        {
            var RandomDriver = new Random();
            string RandomWord;
            string SpeltWord;
            TextWriters.Write(Translate.DoTranslation("Press CTRL+C to exit."), true, KernelColorTools.ColTypes.Tip);
            if (Words.Count == 0)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Downloading words...");
                Words.AddRange(NetworkTransfer.DownloadString("https://cdn.jsdelivr.net/gh/sindresorhus/word-list/words.txt").SplitNewLines().ToList());
            }
            while (true)
            {
                RandomWord = Words.ElementAt(RandomDriver.Next(Words.Count));
                DebugWriter.Wdbg(DebugLevel.I, "Word: {0}", RandomWord);
                TextWriters.Write(RandomWord, true, KernelColorTools.ColTypes.Input);
                SpeltWord = Input.ReadLineNoInput('\0');

                if ((SpeltWord ?? "") == (RandomWord ?? ""))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Spelt: {0} = {1}", SpeltWord, RandomWord);
                    TextWriters.Write(Translate.DoTranslation("Spelt perfectly!"), true, KernelColorTools.ColTypes.Success);
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Spelt: {0} != {1}", SpeltWord, RandomWord);
                    TextWriters.Write(Translate.DoTranslation("Spelt incorrectly."), true, KernelColorTools.ColTypes.Warning);
                }
            }
        }

    }
}