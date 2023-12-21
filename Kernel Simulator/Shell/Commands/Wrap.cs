using System.Collections;

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

using System.IO;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Languages;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Commands
{
	class WrapCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			string CommandToBeWrapped = ListArgs[0].Split(' ')[0];
			if (Shell.Commands.ContainsKey(CommandToBeWrapped))
			{
				if (Shell.Commands[CommandToBeWrapped].Wrappable)
				{
					string WrapOutputPath = Paths.TempPath + "/wrapoutput.txt";
					var AltThreads = ShellStart.ShellStack[ShellStart.ShellStack.Count - 1].AltCommandThreads;
					if (AltThreads.Count == 0 || AltThreads[AltThreads.Count - 1].IsAlive)
					{
						var WrappedCommand = new KernelThread($"Wrapped Shell Command Thread", false, (_) => GetCommand.ExecuteCommand());
						ShellStart.ShellStack[ShellStart.ShellStack.Count - 1].AltCommandThreads.Add(WrappedCommand);
					}
					Shell.GetLine(ListArgs[0], false, WrapOutputPath);
					var WrapOutputStream = new StreamReader(WrapOutputPath);
					string WrapOutput = WrapOutputStream.ReadToEnd();
					TextWriterWrappedColor.WriteWrapped(WrapOutput, false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
					if (!WrapOutput.EndsWith(Kernel.Kernel.NewLine))
						TextWriterColor.WritePlain("", true);
					WrapOutputStream.Close();
					File.Delete(WrapOutputPath);
				}
				else
				{
					var WrappableCmds = new ArrayList();
					foreach (CommandInfo CommandInfo in Shell.Commands.Values)
					{
						if (CommandInfo.Wrappable)
							WrappableCmds.Add(CommandInfo.Command);
					}
					TextWriterColor.Write(Translate.DoTranslation("The command is not wrappable. These commands are wrappable:") + " {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), string.Join(", ", WrappableCmds.ToArray()));
				}
			}
			else
			{
				TextWriterColor.Write(Translate.DoTranslation("The wrappable command is not found."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
			}
		}

		public override void HelpHelper()
		{
			// Get wrappable commands
			var WrappableCmds = new ArrayList();
			foreach (CommandInfo CommandInfo in Shell.Commands.Values)
			{
				if (CommandInfo.Wrappable)
					WrappableCmds.Add(CommandInfo.Command);
			}

			// Print them along with help description
			TextWriterColor.Write(Translate.DoTranslation("Wrappable commands:") + " {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), string.Join(", ", WrappableCmds.ToArray()));
		}

	}
}