using System.Collections.Generic;
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

using KS.Misc.Configuration;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.TestShell.Commands
{
	class Test_CheckSettingsEntryVarsCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			var Results = SettingsApp.CheckSettingsVariables();
			var NotFound = new List<string>();

			// Go through each and every result
			foreach (string Variable in Results.Keys)
			{
				bool IsFound = Results[Variable];
				if (!IsFound)
				{
					NotFound.Add(Variable);
				}
			}

			// Warn if not found
			if (NotFound.Count > 0)
			{
				TextWriterColor.Write(Translate.DoTranslation("These configuration entries have invalid variables or enumerations and need to be fixed:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Warning));
				ListWriterColor.WriteList(NotFound);
			}
		}

	}
}