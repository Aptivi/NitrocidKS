using KS.ConsoleBase.Colors;
using KS.Misc.Writers.FancyWriters;
using KS.Misc.Writers.FancyWriters.Tools;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.TestShell.Commands
{
	class Test_PrintFigletCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			KernelColorTools.ColTypes Color = (KernelColorTools.ColTypes)Convert.ToInt32(ListArgs[0]);
			var FigletFont = FigletTools.GetFigletFont(ListArgs[1]);
			string Text = ListArgs[2];
			FigletColor.WriteFiglet(Text, FigletFont, Color);
		}

	}
}
