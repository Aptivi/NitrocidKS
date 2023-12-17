
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Namespace Network.RSS.Commands
    Class RSS_FeedInfoCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Write("- " + DoTranslation("Title:") + " ", False, GetConsoleColor(ColTypes.ListEntry))
            Write(RSSFeedInstance.FeedTitle, True, GetConsoleColor(ColTypes.ListValue))
            Write("- " + DoTranslation("Link:") + " ", False, GetConsoleColor(ColTypes.ListEntry))
            Write(RSSFeedInstance.FeedUrl, True, GetConsoleColor(ColTypes.ListValue))
            Write("- " + DoTranslation("Description:") + " ", False, GetConsoleColor(ColTypes.ListEntry))
            Write(RSSFeedInstance.FeedDescription, True, GetConsoleColor(ColTypes.ListValue))
            Write("- " + DoTranslation("Feed type:") + " ", False, GetConsoleColor(ColTypes.ListEntry))
            Write(RSSFeedInstance.FeedType.ToString, True, GetConsoleColor(ColTypes.ListValue))
            Write("- " + DoTranslation("Number of articles:") + " ", False, GetConsoleColor(ColTypes.ListEntry))
            Write(RSSFeedInstance.FeedArticles.Count.ToString, True, GetConsoleColor(ColTypes.ListValue))
        End Sub

    End Class
End Namespace