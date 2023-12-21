using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using Newtonsoft.Json;

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

namespace KS.Misc.Editors.JsonShell.Commands
{
	class JsonShell_SaveCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			var TargetFormatting = Formatting.Indented;
			if (ListSwitchesOnly.Length > 0)
			{
				if (ListSwitchesOnly[0] == "-b")
					TargetFormatting = Formatting.Indented;
				if (ListSwitchesOnly[0] == "-m")
					TargetFormatting = Formatting.None;
			}
			JsonTools.JsonShell_SaveFile(false, TargetFormatting);
		}

		public override void HelpHelper()
		{
			TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
			TextWriterColor.Write("  -b: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
			TextWriterColor.Write(Translate.DoTranslation("Saves the JSON file, beautifying it in the process"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
			TextWriterColor.Write("  -m: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
			TextWriterColor.Write(Translate.DoTranslation("Saves the JSON file, minifying it in the process"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
		}

	}
}