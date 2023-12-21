using KS.ConsoleBase.Colors;
using KS.Files;

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

using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Commands
{
	class JsonShellCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			ListArgs[0] = Filesystem.NeutralizePath(ListArgs[0]);
			DebugWriter.Wdbg(DebugLevel.I, "File path is {0} and .Exists is {0}", ListArgs[0], Checking.FileExists(ListArgs[0]));
			if (Checking.FileExists(ListArgs[0]))
			{
				ShellStart.StartShell(ShellType.JsonShell, ListArgs[0]);
			}
			else
			{
				TextWriterColor.Write(Translate.DoTranslation("File doesn't exist."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
			}
		}

	}
}