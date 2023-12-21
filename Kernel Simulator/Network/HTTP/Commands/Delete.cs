using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Network.HTTP.Commands
{
	class HTTP_DeleteCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			if (HTTPShellCommon.HTTPConnected == true)
			{
				// Print a message
				TextWriterColor.Write(Translate.DoTranslation("Deleting {0}..."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Progress), ListArgs[0]);

				// Make a confirmation message so user will not accidentally delete a file or folder
				TextWriterColor.Write(Translate.DoTranslation("Are you sure you want to delete {0} <y/n>?") + " ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input), ListArgs[0]);
				_ = Convert.ToString(Input.DetectKeypress().KeyChar);
				TextWriterColor.WritePlain("", true);

				try
				{
					var DeleteTask = HTTPTools.HttpDelete(ListArgs[0]);
					DeleteTask.Wait();
				}
				catch (AggregateException aex)
				{
					TextWriterColor.Write(aex.Message + ":", true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
					foreach (Exception InnerException in aex.InnerExceptions)
					{
						TextWriterColor.Write("- " + InnerException.Message, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
						if (InnerException.InnerException is not null)
						{
							TextWriterColor.Write("- " + InnerException.InnerException.Message, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
						}
					}
				}
				catch (Exception ex)
				{
					TextWriterColor.Write(ex.Message, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
				}
			}
			else
			{
				TextWriterColor.Write(Translate.DoTranslation("You must connect to server with administrative privileges before performing the deletion."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
			}
		}

	}
}