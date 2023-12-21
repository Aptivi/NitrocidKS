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
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Misc.Editors.HexEdit.Commands
{
	class HexEdit_ReplaceCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			if ((ListArgs?.Count()) is { } arg1 && arg1 == 2)
			{
				byte ByteFrom = Convert.ToByte(ListArgs[0], 16);
				byte ByteWith = Convert.ToByte(ListArgs[1], 16);
				HexEditTools.HexEdit_Replace(ByteFrom, ByteWith);
				TextWriterColor.Write(Translate.DoTranslation("Byte replaced."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Success));
			}
			else if ((ListArgs?.Count()) is { } arg2 && arg2 == 3)
			{
				if (StringQuery.IsStringNumeric(ListArgs[2]))
				{
					if (Conversions.ToLong(ListArgs[2]) <= HexEditShellCommon.HexEdit_FileBytes.LongCount())
					{
						byte ByteFrom = Convert.ToByte(ListArgs[0], 16);
						byte ByteWith = Convert.ToByte(ListArgs[1], 16);
						HexEditTools.HexEdit_Replace(ByteFrom, ByteWith, Conversions.ToLong(ListArgs[2]));
						TextWriterColor.Write(Translate.DoTranslation("Byte replaced."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Success));
					}
					else
					{
						TextWriterColor.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
					}
				}
			}
			else if ((ListArgs?.Count()) is { } arg3 && arg3 > 3)
			{
				if (StringQuery.IsStringNumeric(ListArgs[2]) & StringQuery.IsStringNumeric(ListArgs[3]))
				{
					if (Conversions.ToLong(ListArgs[2]) <= HexEditShellCommon.HexEdit_FileBytes.LongCount() & Conversions.ToLong(ListArgs[3]) <= HexEditShellCommon.HexEdit_FileBytes.LongCount())
					{
						byte ByteFrom = Convert.ToByte(ListArgs[0], 16);
						byte ByteWith = Convert.ToByte(ListArgs[1], 16);
						long ByteNumberStart = Conversions.ToLong(ListArgs[2]);
						long ByteNumberEnd = Conversions.ToLong(ListArgs[3]);
						ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
						HexEditTools.HexEdit_Replace(ByteFrom, ByteWith, ByteNumberStart, ByteNumberEnd);
						TextWriterColor.Write(Translate.DoTranslation("Byte replaced."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Success));
					}
					else
					{
						TextWriterColor.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
					}
				}
			}
		}

	}
}