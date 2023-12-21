using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Network.HTTP.Commands
{
	class HTTP_GetStringCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			if (HTTPShellCommon.HTTPConnected == true)
			{
				// Print a message
				TextWriterColor.Write(Translate.DoTranslation("Getting {0}..."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Progress), ListArgs[0]);

				try
				{
					var ResponseTask = HTTPTools.HttpGetString(ListArgs[0]);
					ResponseTask.Wait();
					string Response = ResponseTask.Result;
					TextWriterColor.Write(Response, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
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
				TextWriterColor.Write(Translate.DoTranslation("You must connect to server before performing transmission."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
			}
		}

	}
}