
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
Imports System.Runtime.CompilerServices

Module RemoteDebugCmd

    Public ReadOnly DebugCommands As New Dictionary(Of String, CommandInfo) From {{"exit", New CommandInfo("exit", ShellCommandType.RemoteDebugShell, DoTranslation("Disconnects you from the debugger"), False, 0)},
                                                                                  {"help", New CommandInfo("help", ShellCommandType.RemoteDebugShell, DoTranslation("Shows help screen"), False, 0)},
                                                                                  {"register", New CommandInfo("register", ShellCommandType.RemoteDebugShell, DoTranslation("Sets device username"), True, 1)},
                                                                                  {"trace", New CommandInfo("trace", ShellCommandType.RemoteDebugShell, DoTranslation("Shows last stack trace on exception"), True, 1)},
                                                                                  {"username", New CommandInfo("username", ShellCommandType.RemoteDebugShell, DoTranslation("Shows current username in the session"), False, 0)}}
    Public DebugModCmds As New ArrayList

    ''' <summary>
    ''' Client command parsing.
    ''' </summary>
    ''' <param name="CmdString">A specified command. It may contain arguments.</param>
    ''' <param name="SocketStreamWriter">A socket stream writer</param>
    ''' <param name="Address">An IP address</param>
    Sub ParseCmd(CmdString As String, SocketStreamWriter As StreamWriter, Address As String)
        EventManager.RaiseRemoteDebugExecuteCommand(Address, CmdString)
        Dim CmdArgs As List(Of String) = CmdString.Split({" "c}, StringSplitOptions.RemoveEmptyEntries).ToList
        Dim CmdName As String = CmdArgs(0)
        CmdArgs.RemoveAt(0)
        Try
            Select Case CmdName
                Case "trace"
                    'Print stack trace command code
                    If dbgStackTraces.Count <> 0 Then
                        If CmdArgs.Count <> 0 Then
                            Try
                                SocketStreamWriter.WriteLine(dbgStackTraces(CmdArgs(0)))
                            Catch ex As Exception
                                SocketStreamWriter.WriteLine(DoTranslation("Index {0} invalid. There are {1} stack traces. Index is zero-based, so try subtracting by 1."), CmdArgs(0), dbgStackTraces.Count)
                            End Try
                        Else
                            SocketStreamWriter.WriteLine(dbgStackTraces(0))
                        End If
                    Else
                        SocketStreamWriter.WriteLine(DoTranslation("No stack trace"))
                    End If
                Case "username"
                    'Current username
                    SocketStreamWriter.WriteLine(signedinusrnm)
                Case "register"
                    'Register to remote debugger so we can set device name
                    If String.IsNullOrWhiteSpace(GetDeviceProperty(Address, DeviceProperty.Name)) Then
                        SetDeviceProperty(Address, DeviceProperty.Name, CmdArgs(0))
                        dbgConns(dbgConns.ElementAt(DebugDevices.GetIndexOfKey(DebugDevices.GetKeyFromValue(Address))).Key) = CmdArgs(0)
                        SocketStreamWriter.WriteLine(DoTranslation("Hi, {0}!").FormatString(CmdArgs(0)))
                    Else
                        SocketStreamWriter.WriteLine(DoTranslation("You're already registered."))
                    End If
                Case "help"
                    'Help command code
                    If CmdArgs.Count <> 0 Then
                        RDebugShowHelp(CmdArgs(0), SocketStreamWriter)
                    Else
                        RDebugShowHelp("", SocketStreamWriter)
                    End If
                Case "exit"
                    'Exit command code
                    DisconnectDbgDev(Address)
            End Select
        Catch ex As Exception
            SocketStreamWriter.WriteLine(DoTranslation("Error executing remote debug command {0}: {1}"), CmdName, ex.Message)
            EventManager.RaiseRemoteDebugCommandError(Address, CmdString, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Gets a key from a value in the dictionary
    ''' </summary>
    ''' <typeparam name="TKey">Key</typeparam>
    ''' <typeparam name="TValue">Value</typeparam>
    ''' <param name="Dict">Source dictionary</param>
    ''' <param name="Value">Value</param>
    ''' <returns>Key from value</returns>
    <Extension()>
    Friend Function GetKeyFromValue(Of TKey, TValue)(Dict As Dictionary(Of TKey, TValue), Value As TValue) As TKey
        If Dict Is Nothing Then Throw New ArgumentNullException(NameOf(Dict))
        For Each DictKey In Dict.Keys
            If Dict(DictKey).Equals(Value) Then
                Return DictKey
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets an index from a key in the dictionary
    ''' </summary>
    ''' <typeparam name="TKey">Key</typeparam>
    ''' <typeparam name="TValue">Value</typeparam>
    ''' <param name="Dict">Source dictionary</param>
    ''' <param name="Key">Key</param>
    ''' <returns>Index of key</returns>
    <Extension()>
    Friend Function GetIndexOfKey(Of TKey, TValue)(Dict As Dictionary(Of TKey, TValue), Key As TKey) As Integer
        If Dict Is Nothing Then Throw New ArgumentNullException(NameOf(Dict))
        Dim DetectedIndex = 0
        Dim Index = 0, loopTo = Dict.Count - 1

        While Index <= loopTo
            Dim ListEntry As Object = Dict.Keys.ElementAtOrDefault(Index)
            If ListEntry.Equals(Key) Then
                DetectedIndex = Index
            End If

            Index += 1
        End While
        Return DetectedIndex
    End Function
End Module
