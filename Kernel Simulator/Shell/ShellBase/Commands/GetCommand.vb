
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

Imports System.IO
Imports System.Threading
Imports KS.Misc.Editors.JsonShell
Imports KS.Misc.Editors.TextEdit
Imports KS.Misc.Editors.HexEdit
Imports KS.Misc.Writers.MiscWriters
Imports KS.Misc.RarFile
Imports KS.Misc.ZipFile
Imports KS.Shell.Shells.FTP
Imports KS.Shell.Shells.HTTP
Imports KS.Shell.Shells.Mail
Imports KS.Network.RemoteDebug
Imports KS.Shell.Shells.RSS
Imports KS.Shell.Shells.SFTP
Imports KS.Shell.Shells.Test

Namespace Shell.ShellBase.Commands
    Public Module GetCommand

        ''' <summary>
        ''' Thread parameters for ExecuteCommand()
        ''' </summary>
        Friend Class ExecuteCommandThreadParameters
            ''' <summary>
            ''' The requested command with arguments
            ''' </summary>
            Friend RequestedCommand As String
            ''' <summary>
            ''' The shell type
            ''' </summary>
            Friend ShellType As ShellType
            ''' <summary>
            ''' The debug device stream writer
            ''' </summary>
            Friend DebugDeviceSocket As StreamWriter

            Friend Sub New(RequestedCommand As String, ShellType As ShellType, DebugDeviceSocket As StreamWriter)
                Me.RequestedCommand = RequestedCommand
                Me.ShellType = ShellType
                Me.DebugDeviceSocket = DebugDeviceSocket
            End Sub
        End Class

        ''' <summary>
        ''' Executes a command
        ''' </summary>
        ''' <param name="ThreadParams">Thread parameters for ExecuteCommand.</param>
        Friend Sub ExecuteCommand(ThreadParams As ExecuteCommandThreadParameters)
            Dim RequestedCommand As String = ThreadParams.RequestedCommand
            Dim ShellType As ShellType = ThreadParams.ShellType
            Dim DebugDeviceSocket As StreamWriter = ThreadParams.DebugDeviceSocket
            Try
                'Variables
                Dim ArgumentInfo As New ProvidedCommandArgumentsInfo(RequestedCommand, ShellType)
                Dim Command As String = ArgumentInfo.Command
                Dim FullArgs() As String = ArgumentInfo.FullArgumentsList
                Dim Args() As String = ArgumentInfo.ArgumentsList
                Dim Switches() As String = ArgumentInfo.SwitchesList
                Dim StrArgs As String = ArgumentInfo.ArgumentsText
                Dim RequiredArgumentsProvided As Boolean = ArgumentInfo.RequiredArgumentsProvided
                Dim TargetCommands As Dictionary(Of String, CommandInfo) = Shell.Commands

                'Set TargetCommands according to the shell type
                TargetCommands = GetCommands(ShellType)

                'Check to see if a requested command is obsolete
                If TargetCommands(Command).Flags.HasFlag(CommandFlags.Obsolete) Then
                    Wdbg(DebugLevel.I, "The command requested {0} is obsolete", Command)
                    DecisiveWrite(ShellType, DebugDeviceSocket, DoTranslation("This command is obsolete and will be removed in a future release."), True, ColTypes.Neutral)
                End If

                'If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                If TargetCommands(Command).CommandArgumentInfo IsNot Nothing Then
                    Dim ArgInfo As CommandArgumentInfo = TargetCommands(Command).CommandArgumentInfo
                    If (ArgInfo.ArgumentsRequired And RequiredArgumentsProvided) Or Not ArgInfo.ArgumentsRequired Then
                        Dim CommandBase As CommandExecutor = TargetCommands(Command).CommandBase
                        CommandBase.Execute(StrArgs, FullArgs, Args, Switches)
                    Else
                        Wdbg(DebugLevel.W, "User hasn't provided enough arguments for {0}", Command)
                        DecisiveWrite(ShellType, DebugDeviceSocket, DoTranslation("There was not enough arguments. See below for usage:"), True, ColTypes.Neutral)
                        ShowHelp(Command, ShellType)
                    End If
                Else
                    Dim CommandBase As CommandExecutor = TargetCommands(Command).CommandBase
                    CommandBase.Execute(StrArgs, FullArgs, Args, Switches)
                End If
            Catch taex As ThreadInterruptedException
                CancelRequested = False
                Exit Sub
            Catch ex As Exception
                KernelEventManager.RaiseCommandError(RequestedCommand, ex)
                WStkTrc(ex)
                DecisiveWrite(ShellType, DebugDeviceSocket, DoTranslation("Error trying to execute command") + " {2}." + NewLine + DoTranslation("Error {0}: {1}"), True, ColTypes.Error, ex.GetType.FullName, ex.Message, RequestedCommand)
            End Try
        End Sub

        ''' <summary>
        ''' Gets the command dictionary according to the shell type
        ''' </summary>
        ''' <param name="ShellType">The shell type</param>
        Public Function GetCommands(ShellType As ShellType) As Dictionary(Of String, CommandInfo)
            'Individual shells
            Dim FinalCommands As Dictionary(Of String, CommandInfo)
            Select Case ShellType
                Case ShellType.FTPShell
                    FinalCommands = New Dictionary(Of String, CommandInfo)(FTPCommands)
                Case ShellType.MailShell
                    FinalCommands = New Dictionary(Of String, CommandInfo)(MailCommands)
                Case ShellType.RemoteDebugShell
                    FinalCommands = New Dictionary(Of String, CommandInfo)(DebugCommands)
                Case ShellType.RSSShell
                    FinalCommands = New Dictionary(Of String, CommandInfo)(RSSCommands)
                Case ShellType.SFTPShell
                    FinalCommands = New Dictionary(Of String, CommandInfo)(SFTPCommands)
                Case ShellType.TestShell
                    FinalCommands = New Dictionary(Of String, CommandInfo)(Test_Commands)
                Case ShellType.TextShell
                    FinalCommands = New Dictionary(Of String, CommandInfo)(TextEdit_Commands)
                Case ShellType.ZIPShell
                    FinalCommands = New Dictionary(Of String, CommandInfo)(ZipShell_Commands)
                Case ShellType.JsonShell
                    FinalCommands = New Dictionary(Of String, CommandInfo)(JsonShell_Commands)
                Case ShellType.HTTPShell
                    FinalCommands = New Dictionary(Of String, CommandInfo)(HTTPCommands)
                Case ShellType.HexShell
                    FinalCommands = New Dictionary(Of String, CommandInfo)(HexEdit_Commands)
                Case ShellType.RARShell
                    FinalCommands = New Dictionary(Of String, CommandInfo)(RarShell_Commands)
                Case Else
                    FinalCommands = New Dictionary(Of String, CommandInfo)(Shell.Commands)
            End Select

            'Unified commands
            For Each UnifiedCommand As String In UnifiedCommandDict.Keys
                FinalCommands.AddOrModify(UnifiedCommand, UnifiedCommandDict(UnifiedCommand))
            Next

            Return FinalCommands
        End Function

    End Module
End Namespace
