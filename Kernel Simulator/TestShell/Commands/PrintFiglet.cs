using KS.ConsoleBase.Colors;
using KS.Misc.Writers.FancyWriters;
using KS.Misc.Writers.FancyWriters.Tools;
using KS.Shell.ShellBase.Commands;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.TestShell.Commands
{
	class Test_PrintFigletCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			KernelColorTools.ColTypes Color = (KernelColorTools.ColTypes)Conversions.ToInteger(ListArgs[0]);
			var FigletFont = FigletTools.GetFigletFont(ListArgs[1]);
			string Text = ListArgs[2];
			FigletColor.WriteFiglet(Text, FigletFont, Color);
		}

	}
}