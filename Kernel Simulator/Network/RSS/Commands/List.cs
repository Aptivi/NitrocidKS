﻿//
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

using KS.ConsoleBase.Colors;
using KS.Misc.Text;
using KS.ConsoleBase.Writers;
using KS.Network.RSS.Instance;
using KS.Shell.ShellBase.Commands;

namespace KS.Network.RSS.Commands
{
    class RSS_ListCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            foreach (RSSArticle Article in RSSShellCommon.RSSFeedInstance.FeedArticles)
            {
                TextWriters.Write("- {0}: ", false, KernelColorTools.ColTypes.ListEntry, Article.ArticleTitle);
                TextWriters.Write(Article.ArticleLink, true, KernelColorTools.ColTypes.ListValue);
                TextWriters.Write("    {0}", true, KernelColorTools.ColTypes.Neutral, TextTools.SplitNewLines(Article.ArticleDescription)[0].Truncate(200));
            }
        }

    }
}
