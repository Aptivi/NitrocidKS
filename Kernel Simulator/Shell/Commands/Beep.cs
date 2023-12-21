using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Platform;

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
using KS.Shell.ShellBase.Commands;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Shell.Commands
{
	class BeepCommand : CommandExecutor, ICommand
	{

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "There is already a platform check in the command logic.")]
		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			if (StringQuery.IsStringNumeric(ListArgs[0]) & Conversions.ToInteger(ListArgs[0]) >= 37 & Conversions.ToInteger(ListArgs[0]) <= 32767)
			{
				if (StringQuery.IsStringNumeric(ListArgs[1]))
				{
					if (PlatformDetector.IsOnWindows())
					{
						Console.Beep(Conversions.ToInteger(ListArgs[0]), Conversions.ToInteger(ListArgs[1]));
					}
					else
					{
						Console.Beep();
					}
				}
				else
				{
					TextWriterColor.Write(Translate.DoTranslation("Time must be numeric."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
				}
			}
			else
			{
				TextWriterColor.Write(Translate.DoTranslation("Frequency must be numeric. If it's numeric, ensure that it is >= 37 and <= 32767."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
			}
		}

	}
}