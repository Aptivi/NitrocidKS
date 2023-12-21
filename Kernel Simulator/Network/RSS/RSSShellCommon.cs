using System;
using System.Collections.Generic;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Net.Http;
using KS.Misc.Threading;
using KS.Network.RSS.Commands;
using KS.Network.RSS.Instance;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Network.RSS
{
	public static class RSSShellCommon
	{

		// Variables
		public readonly static Dictionary<string, CommandInfo> RSSCommands = new() { { "articleinfo", new CommandInfo("articleinfo", ShellType.RSSShell, "Gets the article info", new CommandArgumentInfo(new[] { "<feednum>" }, true, 1), new RSS_ArticleInfoCommand()) }, { "bookmark", new CommandInfo("bookmark", ShellType.RSSShell, "Bookmarks the feed", new CommandArgumentInfo(Array.Empty<string>(), false, 0), new RSS_BookmarkCommand()) }, { "chfeed", new CommandInfo("chfeed", ShellType.RSSShell, "Changes the feed link", new CommandArgumentInfo(new[] { "[-bookmark] <feedurl/bookmarknumber>" }, true, 1), new RSS_ChFeedCommand()) }, { "feedinfo", new CommandInfo("feedinfo", ShellType.RSSShell, "Gets the feed info", new CommandArgumentInfo(Array.Empty<string>(), false, 0), new RSS_FeedInfoCommand()) }, { "help", new CommandInfo("help", ShellType.RSSShell, "Shows help screen", new CommandArgumentInfo(Array.Empty<string>(), false, 0), new RSS_HelpCommand()) }, { "list", new CommandInfo("list", ShellType.RSSShell, "Lists all feeds", new CommandArgumentInfo(Array.Empty<string>(), false, 0), new RSS_ListCommand()) }, { "listbookmark", new CommandInfo("listbookmark", ShellType.RSSShell, "Lists all bookmarked feeds", new CommandArgumentInfo(Array.Empty<string>(), false, 0), new RSS_ListBookmarkCommand()) }, { "read", new CommandInfo("read", ShellType.RSSShell, "Reads a feed in a web browser", new CommandArgumentInfo(new[] { "<feednum>" }, true, 1), new RSS_ReadCommand()) }, { "selfeed", new CommandInfo("selfeed", ShellType.RSSShell, "Selects the feed from the existing feed list from online sources", new CommandArgumentInfo(Array.Empty<string>(), false, 0), new RSS_SelFeedCommand()) }, { "unbookmark", new CommandInfo("unbookmark", ShellType.RSSShell, "Removes the feed bookmark", new CommandArgumentInfo(Array.Empty<string>(), false, 0), new RSS_UnbookmarkCommand()) } };
		public static RSSFeed RSSFeedInstance;
		public static string RSSFeedUrlPromptStyle = "";
		public static int RSSFetchTimeout = 60000;
		public static bool RSSRefreshFeeds = true;
		public static int RSSRefreshInterval = 60000;
		public static bool RSSKeepAlive;
		internal static KernelThread RSSRefresher = new("RSS Feed Refresher", false, RSSTools.RefreshFeeds);
		internal static HttpClient RSSRefresherClient = new() { Timeout = TimeSpan.FromMilliseconds(RSSFetchTimeout) };
		internal static string RSSFeedLink;
		internal readonly static Dictionary<string, CommandInfo> RSSModCommands = new();

	}
}