using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;

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

using KS.Scripting.Conditions;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Commands
{
	class IfCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			try
			{
				if (UESHConditional.ConditionSatisfied(ListArgsOnly[0]))
				{
					string CommandString = string.Join(" ", ListArgsOnly.Skip(1).ToArray());
					var AltThreads = ShellStart.ShellStack[ShellStart.ShellStack.Count - 1].AltCommandThreads;
					if (AltThreads.Count == 0 || AltThreads[AltThreads.Count - 1].IsAlive)
					{
						var CommandThread = new KernelThread($"Alternative Shell Command Thread", false, (param) => GetCommand.ExecuteCommand((GetCommand.ExecuteCommandThreadParameters)param));
						ShellStart.ShellStack[ShellStart.ShellStack.Count - 1].AltCommandThreads.Add(CommandThread);
					}
					Shell.GetLine(CommandString);
				}
			}
			catch (Exception ex)
			{
				DebugWriter.Wdbg(DebugLevel.E, "Failed to satisfy condition. See above for more information: {0}", ex.Message);
				DebugWriter.WStkTrc(ex);
				TextWriterColor.Write(Translate.DoTranslation("Failed to satisfy condition. More info here:") + " {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.Message);
			}
		}

	}
}
