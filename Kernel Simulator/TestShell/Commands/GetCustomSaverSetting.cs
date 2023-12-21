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

using KS.Misc.Screensaver.Customized;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.TestShell.Commands
{
	class Test_GetCustomSaverSettingCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			if (CustomSaverTools.CustomSavers.ContainsKey(ListArgs[0]))
			{
				TextWriterColor.Write("- {0} -> {1}: ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), ListArgs[0], ListArgs[1]);
				TextWriterColor.Write(Convert.ToString(CustomSaverTools.GetCustomSaverSettings(ListArgs[0], ListArgs[1])), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
			}
			else
			{
				TextWriterColor.Write(Translate.DoTranslation("Screensaver {0} not found."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ListArgs[0]);
			}
		}

	}
}
