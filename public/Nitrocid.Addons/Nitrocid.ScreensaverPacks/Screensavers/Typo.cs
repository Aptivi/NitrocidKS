﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Nitrocid.Kernel.Configuration;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Typo
    /// </summary>
    public class TypoDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Typo";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.SetConsoleColor(new Color(ScreensaverPackInit.SaversConfig.TypoTextColor));
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int CpmSpeedMin = ScreensaverPackInit.SaversConfig.TypoWritingSpeedMin * 5;
            int CpmSpeedMax = ScreensaverPackInit.SaversConfig.TypoWritingSpeedMax * 5;
            var Strikes = new List<string>() { "q`12wsa", "r43edfgt5", "u76yhjki8", @"p09ol;'[-=]\", "/';. ", "m,lkjn ", "vbhgfc ", "zxdsa " };
            var CapStrikes = new List<string>() { "Q~!@WSA", "R$#EDFGT%", "U&^YHJKI*", "P)(OL:\"{_+}|", "?\":> ", "M<LKJN ", "VBHGFC ", "ZXDSA " };
            string CapSymbols = "~!@$#%&^*)(:\"{_+}|?><";

            ConsoleWrapper.CursorVisible = false;

            // Prepare display (make a paragraph indentation)
            ConsoleWrapper.WriteLine();
            ConsoleWrapper.Write("    ");
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);

            // Get struck character and write it
            var StrikeCharsIndex = 0;
            foreach (char StruckChar in ScreensaverPackInit.SaversConfig.TypoWrite)
            {
                char StruckCharAssigned = StruckChar;

                // Check to see if we can go ahead
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                // Calculate needed milliseconds from two WPM speeds (minimum and maximum)
                int SelectedCpm = RandomDriver.Random(CpmSpeedMin, CpmSpeedMax);
                int WriteMs = (int)Math.Round(60d / SelectedCpm * 1000d);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Delay for {0} CPM: {1} ms", SelectedCpm, WriteMs);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Struck character: {0}", StruckCharAssigned);

                // See if the typo is guaranteed
                double Probability = (ScreensaverPackInit.SaversConfig.TypoMissStrikePossibility >= 80 ? 80 : ScreensaverPackInit.SaversConfig.TypoMissStrikePossibility) / 100d;
                bool TypoGuaranteed = RandomDriver.RandomChance(Probability);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Probability: {0} Guarantee: {1}", Probability, TypoGuaranteed);
                if (TypoGuaranteed)
                {
                    // Sometimes, a typo is generated by missing a character.
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Made a typo!");
                    double MissProbability = (ScreensaverPackInit.SaversConfig.TypoMissPossibility >= 10 ? 10 : ScreensaverPackInit.SaversConfig.TypoMissPossibility) / 100d;
                    bool MissGuaranteed = RandomDriver.RandomChance(MissProbability);
                    if (MissGuaranteed)
                    {
                        // Miss is guaranteed. Simulate the missed character
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Missed a character!");
                        StruckCharAssigned = Convert.ToChar("\0");
                    }
                    // Typo is guaranteed. Select a strike string randomly until the struck key is found in between the characters
                    else
                    {
                        bool StruckFound = false;
                        bool CappedStrike = false;
                        string StrikesString = "";
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Bruteforcing...");
                        while (!StruckFound)
                        {
                            StrikeCharsIndex = RandomDriver.RandomIdx(0, Strikes.Count);
                            CappedStrike = char.IsUpper(StruckCharAssigned) | CapSymbols.Contains(StruckCharAssigned);
                            StrikesString = CappedStrike ? CapStrikes[StrikeCharsIndex] : Strikes[StrikeCharsIndex];
                            StruckFound = !string.IsNullOrEmpty(StrikesString) && StrikesString.Contains(StruckCharAssigned);
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Strike chars index: {0}", StrikeCharsIndex);
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Capped strike: {0}", CappedStrike);
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Strikes pattern: {0}", StrikesString);
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Found? {0}", StruckFound);
                        }
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Found!");

                        // Select a random character that is a typo from the selected strike index
                        int RandomStrikeIndex = RandomDriver.RandomIdx(0, StrikesString.Length);
                        char MistypedChar = StrikesString[RandomStrikeIndex];
                        if (@"`-=\][';/.,".Contains(MistypedChar) & CappedStrike)
                        {
                            // The mistyped character is a symbol and the strike is capped. Select a symbol from CapStrikes.
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Mistyped character is a symbol and the strike is capped.");
                            MistypedChar = CapStrikes[StrikeCharsIndex][RandomStrikeIndex];
                        }
                        StruckCharAssigned = MistypedChar;
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Struck character: {0}", StruckCharAssigned);
                    }
                }

                // Write the final character to the console and wait
                if (StruckCharAssigned != Convert.ToChar(0))
                    ConsoleWrapper.Write(StruckCharAssigned);
                ScreensaverManager.Delay(WriteMs);
            }

            // Wait until retry
            ConsoleWrapper.WriteLine();
            if (!ConsoleResizeHandler.WasResized(false))
                ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.TypoWriteAgainDelay);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.TypoDelay);
        }

    }
}
