﻿
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
using System.Linq;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.ConsoleBase.Inputs.Styles
{
    /// <summary>
    /// Selection style for input module
    /// </summary>
    public static class SelectionStyle
    {

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        public static int PromptSelection(string Question, string AnswersStr) => PromptSelection(Question, AnswersStr, Array.Empty<string>());

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        public static int PromptSelection(string Question, string AnswersStr, string[] AnswersTitles)
        {
            int HighlightedAnswer = 1;
            while (true)
            {
                // Variables
                var answers = AnswersStr.Split('/');
                ConsoleKeyInfo Answer;
                ConsoleWrapper.Clear();

                // Check to see if the answer titles are the same
                if (answers.Length != AnswersTitles.Length)
                    Array.Resize(ref AnswersTitles, answers.Length);

                // Ask a question
                TextWriterColor.Write(Question + Kernel.Kernel.NewLine, true, ColorTools.ColTypes.Question);

                // Make pages based on console window height
                int pages = answers.Length / (ConsoleWrapper.WindowHeight - ConsoleWrapper.CursorTop);
                int answersPerPage = answers.Length / pages;
                int displayAnswersPerPage = answersPerPage - ConsoleWrapper.CursorTop;

                // The reason for subtracting the highlighted answer by one is that because while the highlighted answer number is one-based, the indexes are zero-based,
                // causing confusion. Pages, again, are one-based. Highlighting the last option causes us to go to the next page. This is intentional.
                int currentPage = (HighlightedAnswer - 1) / displayAnswersPerPage;
                int startIndex = displayAnswersPerPage * currentPage;
                int endIndex = displayAnswersPerPage * (currentPage + 1);

                // Populate the answers
                for (int AnswerIndex = startIndex; AnswerIndex <= endIndex && AnswerIndex <= answers.Length - 1; AnswerIndex++)
                {
                    string AnswerInstance = answers[AnswerIndex];
                    string AnswerTitle = AnswersTitles[AnswerIndex] ?? "";

                    // Just like we told you previously in above few lines, the last option highlight to go to the next page is intentional. So, change the last option
                    // string so it says "Highlight this entry to go to the next page."
                    string AnswerOption = $" {AnswerInstance}) {AnswerTitle}";
                    int AnswerTitleLeft = answers.Max(x => $" {x}) ".Length);
                    if (AnswerTitleLeft < ConsoleWrapper.WindowWidth)
                    {
                        int blankRepeats = AnswerTitleLeft - $" {AnswerInstance}) ".Length;
                        AnswerOption = $" {AnswerInstance}) " + " ".Repeat(blankRepeats) + $"{AnswerTitle}";
                    }
                    var AnswerColor = AnswerIndex + 1 == HighlightedAnswer ? ColorTools.ColTypes.SelectedOption : ColorTools.ColTypes.Option;
                    TextWriterColor.Write(AnswerIndex == endIndex ? " " + Translate.DoTranslation("Highlight this entry to go to the next page.") : AnswerOption, true, AnswerColor);
                }

                // Wait for an answer
                Answer = ConsoleWrapper.ReadKey(true);

                // Check the answer
                switch (Answer.Key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            HighlightedAnswer -= 1;
                            if (HighlightedAnswer == 0)
                                HighlightedAnswer = answers.Length;

                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            if (HighlightedAnswer == answers.Length)
                                HighlightedAnswer = 0;
                            HighlightedAnswer += 1;
                            break;
                        }
                    case ConsoleKey.PageUp:
                        {
                            HighlightedAnswer = 1;
                            break;
                        }
                    case ConsoleKey.PageDown:
                        {
                            HighlightedAnswer = answers.Length;
                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            TextWriterColor.Write();
                            return HighlightedAnswer;
                        }
                    case ConsoleKey.Escape:
                        {
                            TextWriterColor.Write();
                            return -1;
                        }
                }
            }
        }

    }
}
