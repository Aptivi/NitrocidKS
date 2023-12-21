using System;

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

using TermSelectionStyle = Terminaux.Inputs.Styles.Selection.SelectionStyle;

namespace KS.ConsoleBase.Inputs.Styles
{
	public static class SelectionStyle
	{

		/// <summary>
		/// Prompts user for selection
		/// </summary>
		/// <param name="Question">A question</param>
		/// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
		public static int PromptSelection(string Question, string AnswersStr)
		{
			return PromptSelection(Question, AnswersStr, []);
		}

		/// <summary>
		/// Prompts user for Selection
		/// </summary>
		/// <param name="Question">A question</param>
		/// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
		/// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
		public static int PromptSelection(string Question, string AnswersStr, string[] AnswersTitles)
		{
			return TermSelectionStyle.PromptSelection(Question, AnswersStr, AnswersTitles);
		}

	}
}