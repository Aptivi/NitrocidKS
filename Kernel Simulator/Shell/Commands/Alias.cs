using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;

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

using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Shell.Commands
{
	class AliasCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			if ((ListArgs?.Length) is { } arg1 && arg1 > 3)
			{
				if (ListArgs[0] == "add" & Enum.IsDefined(typeof(ShellType), ListArgs[1]))
				{
					AliasManager.ManageAlias(ListArgs[0], (ShellType)Conversions.ToInteger(Enum.Parse(typeof(ShellType), ListArgs[1])), ListArgs[2], ListArgs[3]);
				}
				else
				{
					TextWriterColor.Write(Translate.DoTranslation("Invalid type {0}."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ListArgs[1]);
				}
			}
			else if ((ListArgs?.Length) is { } arg2 && arg2 == 3)
			{
				if (ListArgs[0] == "rem" & Enum.IsDefined(typeof(ShellType), ListArgs[1]))
				{
					AliasManager.ManageAlias(ListArgs[0], (ShellType)Conversions.ToInteger(Enum.Parse(typeof(ShellType), ListArgs[1])), ListArgs[2]);
				}
				else
				{
					TextWriterColor.Write(Translate.DoTranslation("Invalid type {0}."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ListArgs[1]);
				}
			}
		}

	}
}