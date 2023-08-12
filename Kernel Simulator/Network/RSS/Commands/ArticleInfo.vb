
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Imports KS.Network.RSS.Instance

Namespace Network.RSS.Commands
    Class RSS_ArticleInfoCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim ArticleIndex As Integer = ListArgs(0) - 1
            If ArticleIndex > RSSFeedInstance.FeedArticles.Count - 1 Then
                TextWriterColor.Write(DoTranslation("Article number couldn't be bigger than the available articles."), True, ColTypes.Error)
                Wdbg(DebugLevel.E, "Tried to access article number {0}, but count is {1}.", ArticleIndex, RSSFeedInstance.FeedArticles.Count - 1)
            Else
                Dim Article As RSSArticle = RSSFeedInstance.FeedArticles(ArticleIndex)
                TextWriterColor.Write("- " + DoTranslation("Title:") + " ", False, ColTypes.ListEntry)
                TextWriterColor.Write(Article.ArticleTitle, True, ColTypes.ListValue)
                TextWriterColor.Write("- " + DoTranslation("Link:") + " ", False, ColTypes.ListEntry)
                TextWriterColor.Write(Article.ArticleLink, True, ColTypes.ListValue)
                For Each Variable As String In Article.ArticleVariables.Keys
                    If Not Variable = "title" And Not Variable = "link" And Not Variable = "summary" And Not Variable = "description" And Not Variable = "content" Then
                        TextWriterColor.Write("- {0}: ", False, ColTypes.ListEntry, Variable)
                        TextWriterColor.Write(Article.ArticleVariables(Variable).InnerText, True, ColTypes.ListValue)
                    End If
                Next
                TextWriterColor.Write(NewLine + Article.ArticleDescription, True, ColTypes.Neutral)
            End If
        End Sub

    End Class
End Namespace