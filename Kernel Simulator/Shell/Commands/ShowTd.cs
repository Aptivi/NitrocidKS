using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
	class ShowTdCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			TimeDate.TimeDate.ShowCurrentTimes();
		}

	}
}