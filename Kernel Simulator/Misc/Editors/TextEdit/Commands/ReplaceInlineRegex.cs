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
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Misc.Editors.TextEdit.Commands
{
	class TextEdit_ReplaceInlineRegexCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			if ((ListArgs?.Count()) is { } arg1 && arg1 == 3)
			{
				if (StringQuery.IsStringNumeric(ListArgs[2]))
				{
					if (Conversions.ToInteger(ListArgs[2]) <= TextEditShellCommon.TextEdit_FileLines.Count)
					{
						TextEditTools.TextEdit_ReplaceRegex(ListArgs[0], ListArgs[1], Conversions.ToInteger(ListArgs[2]));
						TextWriterColor.Write(Translate.DoTranslation("String replaced."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Success));
					}
					else
					{
						TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
					}
				}
				else
				{
					TextWriterColor.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ListArgs[2]);
					DebugWriter.Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs[2]);
				}
			}
			else if ((ListArgs?.Count()) is { } arg2 && arg2 > 3)
			{
				if (StringQuery.IsStringNumeric(ListArgs[2]) & StringQuery.IsStringNumeric(ListArgs[3]))
				{
					if (Conversions.ToInteger(ListArgs[2]) <= TextEditShellCommon.TextEdit_FileLines.Count & Conversions.ToInteger(ListArgs[3]) <= TextEditShellCommon.TextEdit_FileLines.Count)
					{
						int LineNumberStart = Conversions.ToInteger(ListArgs[2]);
						int LineNumberEnd = Conversions.ToInteger(ListArgs[3]);
						LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
						for (int LineNumber = LineNumberStart, loopTo = LineNumberEnd; LineNumber <= loopTo; LineNumber++)
						{
							TextEditTools.TextEdit_ReplaceRegex(ListArgs[0], ListArgs[1], LineNumber);
							TextWriterColor.Write(Translate.DoTranslation("String replaced in line {0}."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Success), LineNumber);
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