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
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Misc.Editors.HexEdit.Commands
{
	class HexEdit_DelByteCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			if (StringQuery.IsStringNumeric(ListArgs[0]))
			{
				if (Convert.ToInt32(ListArgs[0]) <= HexEditShellCommon.HexEdit_FileBytes.LongCount())
				{
					HexEditTools.HexEdit_DeleteByte(Convert.ToInt64(ListArgs[0]));
					TextWriterColor.Write(Translate.DoTranslation("Byte deleted."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Success));
				}
				else
				{
					TextWriterColor.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
				}
			}
			else
			{
				TextWriterColor.Write(Translate.DoTranslation("The byte number is not numeric."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
				DebugWriter.Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs[0]);
			}
		}

	}
}
