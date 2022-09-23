﻿
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

using System;
using System.Collections.Generic;
using System.Net.Http;
using KS.Misc.Threading;
using KS.Network.RSS;
using KS.Network.RSS.Instance;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.RSS.Commands;

namespace KS.Shell.Shells.RSS
{
    /// <summary>
    /// Common RSS shell module
    /// </summary>
    public static class RSSShellCommon
    {

        /// <summary>
        /// RSS commands
        /// </summary>
        public readonly static Dictionary<string, CommandInfo> RSSCommands = new()
        {
            { "articleinfo", new CommandInfo("articleinfo", ShellType.RSSShell, "Gets the article info", new CommandArgumentInfo(new[] { "<feednum>" }, true, 1), new RSS_ArticleInfoCommand()) },
            { "bookmark", new CommandInfo("bookmark", ShellType.RSSShell, "Bookmarks the feed", new CommandArgumentInfo(), new RSS_BookmarkCommand()) },
            { "chfeed", new CommandInfo("chfeed", ShellType.RSSShell, "Changes the feed link", new CommandArgumentInfo(new[] { "[-bookmark] <feedurl/bookmarknumber>" }, true, 1), new RSS_ChFeedCommand()) },
            { "feedinfo", new CommandInfo("feedinfo", ShellType.RSSShell, "Gets the feed info", new CommandArgumentInfo(), new RSS_FeedInfoCommand()) },
            { "list", new CommandInfo("list", ShellType.RSSShell, "Lists all feeds", new CommandArgumentInfo(), new RSS_ListCommand()) },
            { "listbookmark", new CommandInfo("listbookmark", ShellType.RSSShell, "Lists all bookmarked feeds", new CommandArgumentInfo(), new RSS_ListBookmarkCommand()) },
            { "read", new CommandInfo("read", ShellType.RSSShell, "Reads a feed in a web browser", new CommandArgumentInfo(new[] { "<feednum>" }, true, 1), new RSS_ReadCommand()) },
            { "search", new CommandInfo("search", ShellType.RSSShell, "Searches the feed for a phrase in title and/or description", new CommandArgumentInfo(new[] { "[-t|-d|-a|-cs] <phrase>" }, true, 1), new RSS_SearchCommand()) },
            { "selfeed", new CommandInfo("selfeed", ShellType.RSSShell, "Selects the feed from the existing feed list from online sources", new CommandArgumentInfo(), new RSS_SelFeedCommand()) },
            { "unbookmark", new CommandInfo("unbookmark", ShellType.RSSShell, "Removes the feed bookmark", new CommandArgumentInfo(), new RSS_UnbookmarkCommand()) }
        };
        /// <summary>
        /// RSS feed instance
        /// </summary>
        public static RSSFeed RSSFeedInstance;
        /// <summary>
        /// RSS feed URL prompt style
        /// </summary>
        public static string RSSFeedUrlPromptStyle = "";
        /// <summary>
        /// RSS fetch timeout in milliseconds
        /// </summary>
        public static int RSSFetchTimeout = 60000;
        /// <summary>
        /// Whether to refresh RSS feeds or not
        /// </summary>
        public static bool RSSRefreshFeeds = true;
        /// <summary>
        /// RSS refresh interval in milliseconds
        /// </summary>
        public static int RSSRefreshInterval = 60000;
        /// <summary>
        /// Whether to keep the connection alive or not
        /// </summary>
        public static bool RSSKeepAlive;
        internal static KernelThread RSSRefresher = new("RSS Feed Refresher", false, RSSTools.RefreshFeeds);
        internal static HttpClient RSSRefresherClient = new() { Timeout = TimeSpan.FromMilliseconds(RSSFetchTimeout) };
        internal static string RSSFeedLink;
        internal readonly static Dictionary<string, CommandInfo> RSSModCommands = new();

    }
}
