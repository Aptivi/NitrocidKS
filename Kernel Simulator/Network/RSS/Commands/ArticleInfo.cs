using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Network.RSS.Commands
{
	class RSS_ArticleInfoCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			int ArticleIndex = (int)Math.Round(Convert.ToDouble(ListArgs[0]) - 1d);
			if (ArticleIndex > RSSShellCommon.RSSFeedInstance.FeedArticles.Count - 1)
			{
				TextWriterColor.Write(Translate.DoTranslation("Article number couldn't be bigger than the available articles."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
				DebugWriter.Wdbg(DebugLevel.E, "Tried to access article number {0}, but count is {1}.", ArticleIndex, RSSShellCommon.RSSFeedInstance.FeedArticles.Count - 1);
			}
			else
			{
				var Article = RSSShellCommon.RSSFeedInstance.FeedArticles[ArticleIndex];
				TextWriterColor.Write("- " + Translate.DoTranslation("Title:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
				TextWriterColor.Write(Article.ArticleTitle, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
				TextWriterColor.Write("- " + Translate.DoTranslation("Link:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
				TextWriterColor.Write(Article.ArticleLink, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
				foreach (string Variable in Article.ArticleVariables.Keys)
				{
					if (!(Variable == "title") & !(Variable == "link") & !(Variable == "summary") & !(Variable == "description") & !(Variable == "content"))
					{
						TextWriterColor.Write("- {0}: ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), Variable);
						TextWriterColor.Write(Article.ArticleVariables[Variable].InnerText, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
					}
				}
				TextWriterColor.Write(Kernel.Kernel.NewLine + Article.ArticleDescription, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
			}
		}

	}
}