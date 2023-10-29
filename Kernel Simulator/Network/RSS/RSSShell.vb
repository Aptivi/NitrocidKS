﻿
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Imports System.Threading

Public Module RSSShell

    'Variables
    Public RSSExiting As Boolean
    Public ReadOnly RSSCommands As New Dictionary(Of String, CommandInfo) From {{"articleinfo", New CommandInfo("articleinfo", ShellCommandType.RSSShell, DoTranslation("Gets the article info"), True, 1)},
                                                                                {"chfeed", New CommandInfo("chfeed", ShellCommandType.RSSShell, DoTranslation("Changes the feed link"), True, 1)},
                                                                                {"exit", New CommandInfo("exit", ShellCommandType.RSSShell, DoTranslation("Exits RSS shell and returns to kernel"), False, 0)},
                                                                                {"feedinfo", New CommandInfo("feedinfo", ShellCommandType.RSSShell, DoTranslation("Gets the feed info"), False, 0)},
                                                                                {"help", New CommandInfo("help", ShellCommandType.RSSShell, DoTranslation("Shows help screen"), False, 0)},
                                                                                {"list", New CommandInfo("list", ShellCommandType.RSSShell, DoTranslation("Lists all feeds"), False, 0)},
                                                                                {"read", New CommandInfo("read", ShellCommandType.RSSShell, DoTranslation("Reads a feed in a web browser"), True, 1)}}
    Public RSSModCommands As New ArrayList
    Public RSSFeedInstance As RSSFeed
    Friend RSSFeedLink As String

    ''' <summary>
    ''' Opens an RSS shell to read feeds
    ''' </summary>
    ''' <param name="FeedUrl">A link to an RSS feed</param>
    Public Sub InitiateRSSShell(Optional FeedUrl As String = "")
        'Add handler for RSS shell
        AddHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
        Dim OldRSSFeedLink As String = ""
        RSSFeedLink = FeedUrl

        While Not RSSExiting
Begin:
            If String.IsNullOrWhiteSpace(RSSFeedLink) Then
                Do While String.IsNullOrWhiteSpace(RSSFeedLink)
                    Try
                        Write(DoTranslation("Enter an RSS feed URL:") + " ", False, ColTypes.Input)
                        RSSFeedLink = Console.ReadLine
                        RSSFeedInstance = New RSSFeed(RSSFeedLink, RSSFeedType.Infer)
                        RSSFeedLink = RSSFeedInstance.FeedUrl
                        OldRSSFeedLink = RSSFeedLink
                    Catch ex As Exception
                        Wdbg("E", "Failed to parse RSS feed URL {0}: {1}", FeedUrl, ex.Message)
                        WStkTrc(ex)
                        Write(DoTranslation("Failed to parse feed URL:") + " {0}", True, ColTypes.Error, ex.Message)
                        RSSFeedLink = ""
                    End Try
                Loop
            Else
                'Make a new RSS feed instance
                Try
                    If OldRSSFeedLink <> RSSFeedLink Then
                        RSSFeedInstance = New RSSFeed(RSSFeedLink, RSSFeedType.Infer)
                        RSSFeedLink = RSSFeedInstance.FeedUrl
                    End If
                    OldRSSFeedLink = RSSFeedLink
                Catch ex As Exception
                    Wdbg("E", "Failed to parse RSS feed URL {0}: {1}", RSSFeedLink, ex.Message)
                    WStkTrc(ex)
                    Write(DoTranslation("Failed to parse feed URL:") + " {0}", True, ColTypes.Error, ex.Message)
                    RSSFeedLink = ""
                    GoTo Begin
                End Try

                'Prepare for prompt
                If DefConsoleOut IsNot Nothing Then
                    Console.SetOut(DefConsoleOut)
                End If
                Write("[", False, ColTypes.Gray) : Write("{0}", False, ColTypes.UserName, New Uri(RSSFeedLink).Host) : Write("] > ", False, ColTypes.Gray) : Write("", False, ColTypes.Input)

                'Prompt for command
                EventManager.RaiseRSSShellInitialized(RSSFeedLink)
                Dim WrittenCommand As String = Console.ReadLine

                'Check to see if the command doesn't start with spaces or if the command is nothing
                Try
                    Wdbg("I", "Starts with spaces: {0}, Is Nothing: {1}, Is Blank {2}", WrittenCommand.StartsWith(" "), WrittenCommand Is Nothing, WrittenCommand = "")
                    If Not (WrittenCommand = Nothing Or WrittenCommand?.StartsWithAnyOf({" ", "#"}) = True) Then
                        Dim Command As String = WrittenCommand.SplitEncloseDoubleQuotes()(0)
                        Wdbg("I", "Checking command {0} for existence.", Command)
                        If RSSCommands.ContainsKey(Command) Then
                            Wdbg("I", "Command {0} found in the list of {1} commands.", Command, RSSCommands.Count)
                            RSSCommandThread = New Thread(AddressOf RSSParseCommand) With {.Name = "RSS Shell Command Thread"}
                            EventManager.RaiseRSSPreExecuteCommand(RSSFeedLink, WrittenCommand)
                            Wdbg("I", "Made new thread. Starting with argument {0}...", WrittenCommand)
                            RSSCommandThread.Start(WrittenCommand)
                            RSSCommandThread.Join()
                            EventManager.RaiseRSSPostExecuteCommand(RSSFeedLink, WrittenCommand)
                        ElseIf RSSModCommands.Contains(Command) Then
                            Wdbg("I", "Mod command {0} executing...", Command)
                            ExecuteModCommand(WrittenCommand)
                        ElseIf RSSShellAliases.Keys.Contains(Command) Then
                            Wdbg("I", "RSS shell alias command found.")
                            WrittenCommand = WrittenCommand.Replace($"""{Command}""", Command)
                            ExecuteRSSAlias(WrittenCommand)
                        Else
                            Write(DoTranslation("The specified RSS shell command is not found."), True, ColTypes.Error)
                            Wdbg("E", "Command {0} not found in the list of {1} commands.", WrittenCommand.Split(" ")(0), RSSCommands.Count)
                        End If
                    End If
                Catch ex As Exception
                    Wdbg("E", "Unknown RSS shell error: {0}", ex.Message)
                    WStkTrc(ex)
                    Write(DoTranslation("Unknown shell error:") + " {0}", True, ColTypes.Error, ex.Message)
                End Try

                'This is to fix race condition between shell initialization and starting the event handler thread
                If WrittenCommand Is Nothing Then
                    Thread.Sleep(30)
                End If
            End If
        End While

        'Remove handler for RSS shell
        AddHandler Console.CancelKeyPress, AddressOf CancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
        RSSExiting = False
    End Sub

    ''' <summary>
    ''' Executes the RSS shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteRSSAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes()(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, RSSShellAliases(FirstWordCmd))
        Wdbg("I", "Actual command: {0}", actualCmd)
        RSSCommandThread = New Thread(AddressOf RSSParseCommand) With {.Name = "RSS Shell Command Thread"}
        RSSCommandThread.Start(actualCmd)
        RSSCommandThread.Join()
    End Sub

End Module
