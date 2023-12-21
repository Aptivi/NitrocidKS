using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.Misc.Writers.FancyWriters;
using KS.Misc.Writers.FancyWriters.Tools;
using KS.Shell.ShellBase.Commands;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.TestShell.Commands
{
	class Test_PrintFigletFCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			var Parts = new List<string>(ListArgs);
			KernelColorTools.ColTypes Color = (KernelColorTools.ColTypes)Conversions.ToInteger(ListArgs[0]);
			var FigletFont = FigletTools.GetFigletFont(ListArgs[1]);
			object[] Vars = ListArgs[2].Split(';');
			string Text = ListArgs[3];
			FigletColor.WriteFiglet(Text, FigletFont, Color, Vars);
		}

	}
}