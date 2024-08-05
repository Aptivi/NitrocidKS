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
using System.Diagnostics;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Execution;
using KS.Misc.Platform;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;
namespace KS.Network.RSS.Commands
{
    class RSS_ReadCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            int ArticleIndex = (int)Math.Round(Convert.ToDouble(ListArgs[0]) - 1d);
            if (ArticleIndex > RSSShellCommon.RSSFeedInstance.FeedArticles.Count - 1)
            {
                TextWriters.Write(Translate.DoTranslation("Article number couldn't be bigger than the available articles."), true, KernelColorTools.ColTypes.Error);
                DebugWriter.Wdbg(DebugLevel.E, "Tried to access article number {0}, but count is {1}.", ArticleIndex, RSSShellCommon.RSSFeedInstance.FeedArticles.Count - 1);
            }
            else if (!string.IsNullOrWhiteSpace(RSSShellCommon.RSSFeedInstance.FeedArticles[ArticleIndex].ArticleLink))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Opening web browser to {0}...", RSSShellCommon.RSSFeedInstance.FeedArticles[ArticleIndex].ArticleLink);
                if (PlatformDetector.IsOnWindows())
                    ProcessExecutor.ExecuteProcess("cmd.exe", $"/c \"start {RSSShellCommon.RSSFeedInstance.FeedArticles[ArticleIndex].ArticleLink}\"");
                else if (PlatformDetector.IsOnMacOS())
                    ProcessExecutor.ExecuteProcess("open", RSSShellCommon.RSSFeedInstance.FeedArticles[ArticleIndex].ArticleLink);
                else
                    ProcessExecutor.ExecuteProcess("xdg-open", RSSShellCommon.RSSFeedInstance.FeedArticles[ArticleIndex].ArticleLink);
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Article doesn't have a link!"), true, KernelColorTools.ColTypes.Error);
                DebugWriter.Wdbg(DebugLevel.E, "Tried to open a web browser to link of article number {0}, but it's empty. \"{1}\"", ArticleIndex, RSSShellCommon.RSSFeedInstance.FeedArticles[ArticleIndex].ArticleLink);
            }
        }

    }
}
