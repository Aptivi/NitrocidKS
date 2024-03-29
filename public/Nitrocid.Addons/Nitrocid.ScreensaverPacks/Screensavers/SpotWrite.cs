﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Nitrocid.Files.Operations;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Textify.General;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for SpotWrite
    /// </summary>
    public static class SpotWriteSettings
    {

        /// <summary>
        /// [SpotWrite] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SpotWriteDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SpotWriteDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                ScreensaverPackInit.SaversConfig.SpotWriteDelay = value;
            }
        }
        /// <summary>
        /// [SpotWrite] Text for SpotWrite. Longer is better.
        /// </summary>
        public static string SpotWriteWrite
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SpotWriteWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                ScreensaverPackInit.SaversConfig.SpotWriteWrite = value;
            }
        }
        /// <summary>
        /// [SpotWrite] How many milliseconds to wait before writing the text in the new screen again?
        /// </summary>
        public static int SpotWriteNewScreenDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SpotWriteNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                ScreensaverPackInit.SaversConfig.SpotWriteNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [SpotWrite] Text color
        /// </summary>
        public static string SpotWriteTextColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SpotWriteTextColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.SpotWriteTextColor = new Color(value).PlainSequence;
            }
        }

    }

    /// <summary>
    /// Display code for SpotWrite
    /// </summary>
    public class SpotWriteDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "SpotWrite";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.SetConsoleColor(new Color(SpotWriteSettings.SpotWriteTextColor));
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            string TypeWrite = SpotWriteSettings.SpotWriteWrite;
            ConsoleWrapper.CursorVisible = false;

            // SpotWrite can also deal with files written on the field that is used for storing text, so check to see if the path exists.
            DebugWriter.WriteDebug(DebugLevel.I, "Checking \"{0}\" to see if it's a file path", SpotWriteSettings.SpotWriteWrite);
            if (Parsing.TryParsePath(SpotWriteSettings.SpotWriteWrite) && Checking.FileExists(SpotWriteSettings.SpotWriteWrite))
            {
                // File found! Now, write the contents of it to the local variable that stores the actual written text.
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Opening file {0} to write...", SpotWriteSettings.SpotWriteWrite);
                TypeWrite = Reading.ReadContentsText(SpotWriteSettings.SpotWriteWrite);
            }

            // For each line, write four spaces, and extra two spaces if paragraph starts.
            foreach (string Paragraph in TypeWrite.SplitNewLines())
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "New paragraph: {0}", Paragraph);

                // Split the paragraph into sentences that have the length of maximum characters that can be printed in various terminal
                // sizes.
                var IncompleteSentences = TextTools.GetWrappedSentences(Paragraph, ConsoleWrapper.WindowWidth - 2, 4);

                // Prepare display (make a paragraph indentation)
                if (ConsoleWrapper.CursorTop != ConsoleWrapper.WindowHeight - 2)
                {
                    ConsoleWrapper.SetCursorPosition(0, ConsoleWrapper.CursorTop + 1);
                    ConsoleWrapper.Write("    ");
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
                }

                // Get struck character and write it
                for (int SentenceIndex = 0; SentenceIndex <= IncompleteSentences.Length - 1; SentenceIndex++)
                {
                    string Sentence = IncompleteSentences[SentenceIndex];
                    if (ConsoleResizeHandler.WasResized(false))
                        break;
                    foreach (char StruckChar in Sentence)
                    {
                        if (ConsoleResizeHandler.WasResized(false))
                            break;

                        // If we're at the end of the page, clear the screen
                        if (ConsoleWrapper.CursorTop == ConsoleWrapper.WindowHeight - 2)
                        {
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "We're at the end of the page! {0} = {1}", ConsoleWrapper.CursorTop, ConsoleWrapper.WindowHeight - 2);
                            ThreadManager.SleepNoBlock(SpotWriteSettings.SpotWriteNewScreenDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                            ConsoleWrapper.Clear();
                            ConsoleWrapper.WriteLine();
                            if (SentenceIndex == 0)
                            {
                                ConsoleWrapper.Write("    ");
                            }
                            else
                            {
                                ConsoleWrapper.Write(" ");
                            }
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
                        }

                        // Write the final character to the console and wait
                        ConsoleWrapper.Write(Convert.ToString(CharManager.GetEsc()) + "[1K" + Convert.ToString(StruckChar) + Convert.ToString(CharManager.GetEsc()) + "[K");
                        ThreadManager.SleepNoBlock(SpotWriteSettings.SpotWriteDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                    }
                    ConsoleWrapper.Write(Convert.ToString(CharManager.GetEsc()) + "[1K");
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(SpotWriteSettings.SpotWriteDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
