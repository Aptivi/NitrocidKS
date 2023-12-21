using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

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

namespace KS.Misc.RarFile.Commands
{
	class RarShell_GetCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			string Where = "";
			var Absolute = default(bool);
			if ((ListArgs?.Length) is { } arg1 && arg1 > 1)
			{
				if (!(ListArgs[1] == "-absolute"))
					Where = Filesystem.NeutralizePath(ListArgs[1]);
				if (ListArgs?.Contains("-absolute") == true)
				{
					Absolute = true;
				}
			}
			RarTools.ExtractRarFileEntry(ListArgs[0], Where, Absolute);
		}

		public override void HelpHelper()
		{
			TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
			TextWriterColor.Write("  -absolute: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
			TextWriterColor.Write(Translate.DoTranslation("Indicates that the target path is absolute"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
		}

	}
}