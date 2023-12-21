using System;
using System.Collections.Generic;

// Kernel Simulator  Copyright (C) 2018-2019  Aptivi
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

using System.Text;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Misc.Platform;
using KS.Misc.Probers;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using Microsoft.VisualBasic.CompilerServices;
using Terminaux.Base;

namespace KS.ManPages
{
	public static class PageViewer
	{

		/// <summary>
        /// Manual page information style
        /// </summary>
		public static string ManpageInfoStyle = "";

		/// <summary>
        /// Previews the manual page
        /// </summary>
        /// <param name="ManualTitle">A manual title</param>
		public static void ViewPage(string ManualTitle)
		{
			if (PageManager.Pages.ContainsKey(ManualTitle))
			{
				// Variables
				int InfoPlace;

				// Get the bottom place
				InfoPlace = ConsoleWrapper.WindowHeight - 1;
				DebugWriter.Wdbg(DebugLevel.I, "Bottom info height is {0}", InfoPlace);

				// If there is any To-do, write them to the console
				DebugWriter.Wdbg(DebugLevel.I, "Todo count for \"{0}\": {1}", ManualTitle, PageManager.Pages[ManualTitle].Todos.Count.ToString());
				if (PageManager.Pages[ManualTitle].Todos.Count != 0)
				{
					DebugWriter.Wdbg(DebugLevel.I, "Todos are found in manpage.");
					TextWriterColor.Write(Translate.DoTranslation("This manual page needs work for:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Warning));
					ListWriterColor.WriteList(PageManager.Pages[ManualTitle].Todos, true);
					TextWriterColor.Write(Kernel.Kernel.NewLine + Translate.DoTranslation("Press any key to read the manual page..."), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Warning));
					Input.DetectKeypress();
				}

				// Clear screen for readability
				ConsoleWrapper.Clear();

				// Write the information to the console
				if (!string.IsNullOrWhiteSpace(ManpageInfoStyle))
				{
					TextWriterWhereColor.WriteWhere(PlaceParse.ProbePlaces(ManpageInfoStyle), ConsoleWrapper.CursorLeft, InfoPlace, true, KernelColorTools.BackgroundColor, KernelColorTools.NeutralTextColor, PageManager.Pages[ManualTitle].Title, PageManager.Pages[ManualTitle].Revision);
				}
				else
				{
					TextWriterWhereColor.WriteWhere(" {0} [v{1}] ", ConsoleWrapper.CursorLeft, InfoPlace, true, KernelColorTools.BackgroundColor, KernelColorTools.NeutralTextColor, PageManager.Pages[ManualTitle].Title, PageManager.Pages[ManualTitle].Revision);
				}

				// Disable blinking cursor
				ConsoleWrapper.CursorVisible = false;

				// Split the sentences to parts to deal with sentence lengths that are longer than the console window width
				var IncompleteSentences = new List<string>();
				var IncompleteSentenceBuilder = new StringBuilder();
				int CharactersParsed;
				int EscapeCharacters;
				int EscapeCharactersCountInside;
				var InEsc = default(bool);
				foreach (string line in PageManager.Pages[ManualTitle].Body.ToString().SplitNewLines())
				{
					CharactersParsed = 0;
					EscapeCharacters = 0;
					EscapeCharactersCountInside = 0;

					// Deal with empty lines
					if (string.IsNullOrEmpty(line))
					{
						IncompleteSentences.Add("");
					}

					// Now, enumerate through each character in the string
					foreach (char LineChar in line)
					{
						// If the character is Escape, run through the color change sequence until we reach "m"
						if (LineChar == Color255.GetEsc())
							InEsc = true;
						if (InEsc)
							EscapeCharactersCountInside += 1;
						if (InEsc & (EscapeCharactersCountInside > 19 | Conversions.ToString(LineChar) == "m"))
						{
							EscapeCharacters += 1;
							InEsc = false;
						}

						// Append the character into the incomplete sentence builder.
						IncompleteSentenceBuilder.Append(LineChar);
						CharactersParsed += 1;

						// Check to see if we're at the maximum character number
						if (!InEsc)
						{
							if (IncompleteSentenceBuilder.Length - EscapeCharacters == ConsoleWrapper.WindowWidth - Convert.ToInt32(PlatformDetector.IsOnUnix()) | line.Length == CharactersParsed)
							{
								// We're at the character number of maximum character. Add the sentence to the list for "wrapping" in columns.
								DebugWriter.Wdbg(DebugLevel.I, "Adding {0} to the list... Incomplete sentences: {1}", IncompleteSentenceBuilder.ToString(), IncompleteSentences.Count);
								IncompleteSentences.Add(IncompleteSentenceBuilder.ToString());

								// Clean everything up
								IncompleteSentenceBuilder.Clear();
							}
						}
						else
						{
							EscapeCharacters += 1;
						}
					}
				}

				// Write the body
				foreach (string line in IncompleteSentences)
				{
					// Write the line
					int OldTop = ConsoleWrapper.CursorTop + 1;
					TextWriterColor.Write(line, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
					if (OldTop != ConsoleWrapper.CursorTop)
						ConsoleWrapper.CursorTop = OldTop;

					// Check to see if we're at the end
					if (ConsoleWrapper.CursorTop == InfoPlace - 1)
					{
						var PressedKey = Input.DetectKeypress();
						if (PressedKey.Key == ConsoleKey.Escape)
						{
							ConsoleWrapper.Clear();
							return;
						}
						else
						{
							ConsoleWrapper.Clear();
							if (!string.IsNullOrWhiteSpace(ManpageInfoStyle))
							{
								TextWriterWhereColor.WriteWhere(PlaceParse.ProbePlaces(ManpageInfoStyle), ConsoleWrapper.CursorLeft, InfoPlace, true, KernelColorTools.BackgroundColor, KernelColorTools.NeutralTextColor, PageManager.Pages[ManualTitle].Title, PageManager.Pages[ManualTitle].Revision);
							}
							else
							{
								TextWriterWhereColor.WriteWhere(" {0} (v{1}) ", ConsoleWrapper.CursorLeft, InfoPlace, true, KernelColorTools.BackgroundColor, KernelColorTools.NeutralTextColor, PageManager.Pages[ManualTitle].Title, PageManager.Pages[ManualTitle].Revision);
							}
						}
					}
				}

				// Stop on last page
				DebugWriter.Wdbg(DebugLevel.I, "We're on the last page.");
				Input.DetectKeypress();

				// Clean up
				DebugWriter.Wdbg(DebugLevel.I, "Exiting...");
				ConsoleWrapper.Clear();
			}
			else
			{
				TextWriterColor.Write(Translate.DoTranslation("Manual page {0} not found."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), ManualTitle);
			}
		}

	}
}