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
using System;

namespace KS.Misc.Editors.HexEdit.Commands
{
	class HexEdit_PrintCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			long ByteNumber;
			if ((ListArgs?.Length) is { } arg1 && arg1 > 0)
			{
				if ((ListArgs?.Length) is { } arg2 && arg2 == 1)
				{
					// We've only provided one range
					DebugWriter.Wdbg(DebugLevel.I, "Byte number provided: {0}", ListArgs[0]);
					DebugWriter.Wdbg(DebugLevel.I, "Is it numeric? {0}", StringQuery.IsStringNumeric(ListArgs[0]));
					if (StringQuery.IsStringNumeric(ListArgs[0]))
					{
						ByteNumber = Convert.ToInt64(ListArgs[0]);
						HexEditTools.HexEdit_DisplayHex(ByteNumber);
					}
					else
					{
						TextWriterColor.Write(Translate.DoTranslation("The byte number is not numeric."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
						DebugWriter.Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs[0]);
					}
				}
				else
				{
					// We've provided two Byte numbers in the range
					DebugWriter.Wdbg(DebugLevel.I, "Byte numbers provided: {0}, {1}", ListArgs[0], ListArgs[1]);
					DebugWriter.Wdbg(DebugLevel.I, "Is it numeric? {0}", StringQuery.IsStringNumeric(ListArgs[0]), StringQuery.IsStringNumeric(ListArgs[1]));
					if (StringQuery.IsStringNumeric(ListArgs[0]) & StringQuery.IsStringNumeric(ListArgs[1]))
					{
						long ByteNumberStart = Convert.ToInt64(ListArgs[0]);
						long ByteNumberEnd = Convert.ToInt64(ListArgs[1]);
						ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
						HexEditTools.HexEdit_DisplayHex(ByteNumberStart, ByteNumberEnd);
					}
					else
					{
						TextWriterColor.Write(Translate.DoTranslation("The byte number is not numeric."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
						DebugWriter.Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs[0]);
					}
				}
			}
			else
			{
				HexEditTools.HexEdit_DisplayHex();
			}
		}

	}
}
