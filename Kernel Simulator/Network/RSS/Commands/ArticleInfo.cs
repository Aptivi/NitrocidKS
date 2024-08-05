//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.ConsoleBase.Writers;
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
                TextWriters.Write(Translate.DoTranslation("Article number couldn't be bigger than the available articles."), true, KernelColorTools.ColTypes.Error);
                DebugWriter.Wdbg(DebugLevel.E, "Tried to access article number {0}, but count is {1}.", ArticleIndex, RSSShellCommon.RSSFeedInstance.FeedArticles.Count - 1);
            }
            else
            {
                var Article = RSSShellCommon.RSSFeedInstance.FeedArticles[ArticleIndex];
                TextWriters.Write("- " + Translate.DoTranslation("Title:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                TextWriters.Write(Article.ArticleTitle, true, KernelColorTools.ColTypes.ListValue);
                TextWriters.Write("- " + Translate.DoTranslation("Link:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                TextWriters.Write(Article.ArticleLink, true, KernelColorTools.ColTypes.ListValue);
                foreach (string Variable in Article.ArticleVariables.Keys)
                {
                    if (!(Variable == "title") & !(Variable == "link") & !(Variable == "summary") & !(Variable == "description") & !(Variable == "content"))
                    {
                        TextWriters.Write("- {0}: ", false, KernelColorTools.ColTypes.ListEntry, Variable);
                        TextWriters.Write(Article.ArticleVariables[Variable].InnerText, true, KernelColorTools.ColTypes.ListValue);
                    }
                }
                TextWriters.Write(Kernel.Kernel.NewLine + Article.ArticleDescription, true, KernelColorTools.ColTypes.Neutral);
            }
        }

    }
}