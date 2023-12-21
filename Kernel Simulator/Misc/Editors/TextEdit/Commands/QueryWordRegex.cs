using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;

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

using KS.Misc.Reflection;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Misc.Editors.TextEdit.Commands
{
	class TextEdit_QueryWordRegexCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			if ((ListArgs?.Count()) is { } arg1 && arg1 == 2)
			{
				if (StringQuery.IsStringNumeric(ListArgs[1]))
				{
					if (Convert.ToInt32(ListArgs[1]) <= TextEditShellCommon.TextEdit_FileLines.Count)
					{
						var QueriedChars = TextEditTools.TextEdit_QueryWordRegex(ListArgs[0], Convert.ToInt32(ListArgs[1]));
						foreach (int WordIndex in QueriedChars.Keys)
						{
							TextWriterColor.Write("- {0}: ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), WordIndex);
							TextWriterColor.Write("{0} ({1})", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), ListArgs[0], TextEditShellCommon.TextEdit_FileLines[Convert.ToInt32(ListArgs[1])]);
						}
					}
					else
					{
						TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
					}
				}
				else if (ListArgs[1].ToLower() == "all")
				{
					var QueriedWords = TextEditTools.TextEdit_QueryWordRegex(ListArgs[0]);
					foreach (int LineIndex in QueriedWords.Keys)
					{
						foreach (int WordIndex in QueriedWords[LineIndex].Keys)
						{
							TextWriterColor.Write("- {0}:{1}: ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), LineIndex, WordIndex);
							TextWriterColor.Write("{0} ({1})", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), ListArgs[0], TextEditShellCommon.TextEdit_FileLines[LineIndex]);
						}
					}
				}
			}
			else if ((ListArgs?.Count()) is { } arg2 && arg2 > 2)
			{
				if (StringQuery.IsStringNumeric(ListArgs[1]) & StringQuery.IsStringNumeric(ListArgs[2]))
				{
					if (Convert.ToInt32(ListArgs[1]) <= TextEditShellCommon.TextEdit_FileLines.Count & Convert.ToInt32(ListArgs[2]) <= TextEditShellCommon.TextEdit_FileLines.Count)
					{
						int LineNumberStart = Convert.ToInt32(ListArgs[1]);
						int LineNumberEnd = Convert.ToInt32(ListArgs[2]);
						LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
						for (int LineNumber = LineNumberStart, loopTo = LineNumberEnd; LineNumber <= loopTo; LineNumber++)
						{
							var QueriedChars = TextEditTools.TextEdit_QueryWordRegex(ListArgs[0], LineNumber);
							foreach (int WordIndex in QueriedChars.Keys)
							{
								TextWriterColor.Write("- {0}:{1}: ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), LineNumber, WordIndex);
								TextWriterColor.Write("{0} ({1})", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), ListArgs[0], TextEditShellCommon.TextEdit_FileLines[Convert.ToInt32(ListArgs[1])]);
							}
						}
					}
					else
					{
						TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
					}
				}
			}
		}

	}
}
