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

Imports System.IO
Imports System.IO.Compression
Imports System.Threading

Module ZipShell

    'Variables
    Public ZipShell_Exiting As Boolean
    Public ReadOnly ZipShell_Commands As New Dictionary(Of String, CommandInfo) From {{"cdir", New CommandInfo("cdir", ShellCommandType.ZIPShell, "Gets current local directory", "", False, 0, New ZipShell_CDirCommand)},
                                                                                      {"chdir", New CommandInfo("chdir", ShellCommandType.ZIPShell, "Changes directory", "<directory>", True, 1, New ZipShell_ChDirCommand)},
                                                                                      {"chadir", New CommandInfo("chadir", ShellCommandType.ZIPShell, "Changes archive directory", "<archivedirectory>", True, 1, New ZipShell_ChADirCommand)},
                                                                                      {"exit", New CommandInfo("exit", ShellCommandType.ZIPShell, "Exits the ZIP shell", "", False, 0, New ZipShell_ExitCommand)},
                                                                                      {"get", New CommandInfo("get", ShellCommandType.ZIPShell, "Extracts a file to a specified directory or a current directory", "<entry> [where] [-absolute]", True, 1, New ZipShell_GetCommand)},
                                                                                      {"help", New CommandInfo("help", ShellCommandType.ZIPShell, "Lists available commands", "[command]", False, 0, New ZipShell_HelpCommand)},
                                                                                      {"list", New CommandInfo("list", ShellCommandType.ZIPShell, "Lists all files inside the archive", "[directory]", False, 0, New ZipShell_ListCommand)}}
    Public ZipShell_ModCommands As New ArrayList
    Public ZipShell_FileStream As FileStream
    Public ZipShell_ZipArchive As ZipArchive
    Public ZipShell_CurrentDirectory As String
    Public ZipShell_CurrentArchiveDirectory As String
    Public ZipShell_PromptStyle As String = ""

    ''' <summary>
    ''' Initializes the ZIP shell
    ''' </summary>
    ''' <param name="ZipFile">A ZIP file. We recommend you to use <see cref="NeutralizePath(String, Boolean)"></see> to neutralize path.</param>
    Public Sub InitializeZipShell(ZipFile As String)
        'Add handler for ZIP shell
        AddHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
        ZipShell_CurrentDirectory = CurrDir

        While Not ZipShell_Exiting
            'Open file if not open
            If ZipShell_FileStream Is Nothing Then ZipShell_FileStream = New FileStream(ZipFile, FileMode.Open)
            If ZipShell_ZipArchive Is Nothing Then ZipShell_ZipArchive = New ZipArchive(ZipShell_FileStream, ZipArchiveMode.Update, False)

            'Prepare for prompt
            If DefConsoleOut IsNot Nothing Then
                Console.SetOut(DefConsoleOut)
            End If
            Wdbg(DebugLevel.I, "ZipShell_PromptStyle = {0}", ZipShell_PromptStyle)
            If ZipShell_PromptStyle = "" Then
                W("[", False, ColTypes.Gray) : W("{0}@{1}", False, ColTypes.UserName, ZipShell_CurrentArchiveDirectory, Path.GetFileName(ZipFile)) : W("] > ", False, ColTypes.Gray)
            Else
                Dim ParsedPromptStyle As String = ProbePlaces(ZipShell_PromptStyle)
                ParsedPromptStyle.ConvertVTSequences
                W(ParsedPromptStyle, False, ColTypes.Gray)
            End If
            SetInputColor()

            'Prompt for command
            EventManager.RaiseZipShellInitialized()
            Dim WrittenCommand As String = Console.ReadLine

            'Check to see if the command doesn't start with spaces or if the command is nothing
            Wdbg(DebugLevel.I, "Starts with spaces: {0}, Is Nothing: {1}, Is Blank {2}", WrittenCommand?.StartsWith(" "), WrittenCommand Is Nothing, WrittenCommand = "")
            If Not (WrittenCommand = Nothing Or WrittenCommand?.StartsWithAnyOf({" ", "#"}) = True) Then
                Dim Command As String = WrittenCommand.SplitEncloseDoubleQuotes(" ")(0)
                Wdbg(DebugLevel.I, "Checking command {0} for existence.", Command)
                If ZipShell_Commands.ContainsKey(Command) Then
                    Wdbg(DebugLevel.I, "Command {0} found in the list of {1} commands.", Command, ZipShell_Commands.Count)
                    Dim Params As New ExecuteCommandThreadParameters(WrittenCommand, ShellCommandType.ZIPShell, Nothing)
                    ZipShell_CommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "ZIP Shell Command Thread"}
                    EventManager.RaiseZipPreExecuteCommand(WrittenCommand)
                    Wdbg(DebugLevel.I, "Made new thread. Starting with argument {0}...", WrittenCommand)
                    ZipShell_CommandThread.Start(Params)
                    ZipShell_CommandThread.Join()
                    EventManager.RaiseZipPostExecuteCommand(WrittenCommand)
                ElseIf ZipShell_ModCommands.Contains(Command) Then
                    Wdbg(DebugLevel.I, "Mod command {0} executing...", Command)
                    ExecuteModCommand(WrittenCommand)
                ElseIf ZIPShellAliases.Keys.Contains(Command) Then
                    Wdbg(DebugLevel.I, "ZIP shell alias command found.")
                    WrittenCommand = WrittenCommand.Replace($"""{Command}""", Command)
                    ExecuteZIPAlias(WrittenCommand)
                Else
                    W(DoTranslation("The specified ZIP shell command is not found."), True, ColTypes.Error)
                    Wdbg(DebugLevel.E, "Command {0} not found in the list of {1} commands.", Command, ZipShell_Commands.Count)
                End If
            End If

            'This is to fix race condition between shell initialization and starting the event handler thread
            If WrittenCommand Is Nothing Then
                Thread.Sleep(30)
            End If
        End While

        'Close file stream
        ZipShell_ZipArchive.Dispose()
        ZipShell_CurrentDirectory = ""
        ZipShell_CurrentArchiveDirectory = ""
        ZipShell_ZipArchive = Nothing
        ZipShell_FileStream = Nothing

        'Remove handler for ZIP shell
        AddHandler Console.CancelKeyPress, AddressOf CancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
        ZipShell_Exiting = False
    End Sub

    ''' <summary>
    ''' Executes the ZIP shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteZIPAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, ZIPShellAliases(FirstWordCmd))
        Wdbg(DebugLevel.I, "Actual command: {0}", actualCmd)
        Dim Params As New ExecuteCommandThreadParameters(actualCmd, ShellCommandType.ZIPShell, Nothing)
        ZipShell_CommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "ZIP Shell Command Thread"}
        ZipShell_CommandThread.Start(Params)
        ZipShell_CommandThread.Join()
    End Sub

End Module
