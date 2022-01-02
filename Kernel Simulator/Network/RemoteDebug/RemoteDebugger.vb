﻿
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

Imports System.Net.Sockets
Imports System.Threading
Imports System.IO

Public Module RemoteDebugger

    Public DebugPort As Integer = 3014
    Public RDebugClient As Socket
    Public DebugTCP As TcpListener
    Public DebugDevices As New List(Of RemoteDebugDevice)
    Public RDebugThread As New Thread(AddressOf StartRDebugger) With {.IsBackground = True, .Name = "Remote Debug Thread"}
    Public RDebugBlocked As New List(Of String) 'Blocked IP addresses
    Public RDebugStopping As Boolean
    Public RDebugAutoStart As Boolean = True
    Public RDebugMessageFormat As String = ""
    Friend RDebugFailed As Boolean
    Friend RDebugFailedReason As Exception
    Private ReadOnly RDebugVersion As String = "0.7.0"
    Private RDebugBail As Boolean

    ''' <summary>
    ''' Whether to start or stop the remote debugger
    ''' </summary>
    Public Sub StartRDebugThread()
        If DebugMode Then
            If Not RDebugThread.IsAlive Then
                RDebugThread.Start()
                While Not RDebugBail
                End While
                RDebugBail = False
            End If
        End If
    End Sub

    ''' <summary>
    ''' Whether to start or stop the remote debugger
    ''' </summary>
    Public Sub StopRDebugThread()
        If DebugMode Then
            If RDebugThread.IsAlive Then
                RDebugStopping = True
                RDebugThread = New Thread(AddressOf StartRDebugger) With {.IsBackground = True, .Name = "Remote Debug Thread"}
            End If
        End If
    End Sub

    ''' <summary>
    ''' Thread to accept connections after the listener starts
    ''' </summary>
    Sub StartRDebugger()
        'Listen to a current IP address
        Try
            DebugTCP = New TcpListener(New IPAddress({0, 0, 0, 0}), DebugPort)
            DebugTCP.Start()
        Catch sex As SocketException
            RDebugFailed = True
            RDebugFailedReason = sex
            WStkTrc(sex)
        End Try

        'Start the listening thread
        Dim RStream As New Thread(AddressOf ReadAndBroadcastAsync) With {.Name = "Remote Debug Listener Thread"}
        RStream.Start()
        RDebugBail = True

        'Run forever! Until the remote debugger is stopping.
        While Not RDebugStopping
            Thread.Sleep(1)
            Try
                'Variables
                Dim RDebugStream As NetworkStream
                Dim RDebugSWriter As StreamWriter
                Dim RDebugClient As Socket
                Dim RDebugIP As String
                Dim RDebugEndpoint As String
                Dim RDebugName As String
                Dim RDebugInstance As RemoteDebugDevice

                'Check for pending connections
                If DebugTCP.Pending Then
                    'Populate the device variables with the information
                    RDebugClient = DebugTCP.AcceptSocket
                    RDebugStream = New NetworkStream(RDebugClient)

                    'Add the device to JSON
                    RDebugEndpoint = RDebugClient.RemoteEndPoint.ToString
                    RDebugIP = RDebugEndpoint.Remove(RDebugClient.RemoteEndPoint.ToString.IndexOf(":"))
                    AddDeviceToJson(RDebugIP, False)

                    'Get the remaining properties
                    RDebugName = GetDeviceProperty(RDebugIP, DeviceProperty.Name)
                    RDebugInstance = New RemoteDebugDevice(RDebugClient, RDebugStream, RDebugIP, RDebugName)
                    RDebugSWriter = RDebugInstance.ClientStreamWriter

                    'Check the name
                    If String.IsNullOrEmpty(RDebugName) Then
                        Wdbg(DebugLevel.W, "Debug device {0} has no name. Prompting for name...", RDebugIP)
                    End If

                    'Check to see if the device is blocked
                    If RDebugBlocked.Contains(RDebugIP) Then
                        'Blocked! Disconnect it.
                        Wdbg(DebugLevel.W, "Debug device {0} ({1}) tried to join remote debug, but blocked.", RDebugName, RDebugIP)
                        RDebugClient.Disconnect(True)
                    Else
                        'Not blocked yet. Add the connection.
                        DebugDevices.Add(RDebugInstance)
                        RDebugSWriter.WriteLine(DoTranslation(">> Remote Debug and Chat: version") + " {0}", RDebugVersion)
                        RDebugSWriter.WriteLine(DoTranslation(">> Your address is {0}."), RDebugIP)
                        If String.IsNullOrEmpty(RDebugName) Then
                            RDebugSWriter.WriteLine(DoTranslation(">> Welcome! This is your first time entering remote debug and chat. Use ""/register <name>"" to register.") + " ", RDebugName)
                        Else
                            RDebugSWriter.WriteLine(DoTranslation(">> Your name is {0}."), RDebugName)
                        End If

                        'Acknowledge the debugger
                        Wdbg(DebugLevel.I, "Debug device ""{0}"" ({1}) connected.", RDebugName, RDebugIP)
                        RDebugSWriter.Flush()
                        KernelEventManager.RaiseRemoteDebugConnectionAccepted(RDebugIP)
                    End If
                End If
            Catch ae As ThreadAbortException
                Exit While
            Catch ex As Exception
                If NotifyOnRemoteDebugConnectionError Then
                    Dim RemoteDebugError As New Notification(DoTranslation("Remote debugger connection error"), ex.Message, NotifPriority.Medium, NotifType.Normal)
                    NotifySend(RemoteDebugError)
                Else
                    Write(DoTranslation("Remote debugger connection error") + ": {0}", True, ColTypes.Error, ex.Message)
                End If
                WStkTrc(ex)
            End Try
        End While

        RDebugStopping = False
        DebugTCP.Stop()
        DebugDevices.Clear()
        Thread.CurrentThread.Abort()
    End Sub

    ''' <summary>
    ''' Thread to listen to messages and post them to the debugger
    ''' </summary>
    Sub ReadAndBroadcastAsync()
        Dim DeviceIndex As Integer = 0
        While True
            Thread.Sleep(1)
            If DeviceIndex > DebugDevices.Count - 1 Then
                'We've reached all the devices!
                DeviceIndex = 0
            Else
                'Variables
                Dim MessageBuffer(65536) As Byte
                Dim SocketStream As New NetworkStream(DebugDevices(DeviceIndex).ClientSocket)
                Dim SocketStreamWriter As StreamWriter = DebugDevices(DeviceIndex).ClientStreamWriter
                Dim SocketIP As String = DebugDevices(DeviceIndex).ClientIP
                Dim SocketName As String = DebugDevices(DeviceIndex).ClientName

                'Set the timeout of ten milliseconds to ensure that no device "take turns in messaging"
                SocketStream.ReadTimeout = 10

                'Increment.
                DeviceIndex += 1
                Try
                    'Read a message from the stream
                    SocketStream.Read(MessageBuffer, 0, 65536)
                    Dim Message As String = Text.Encoding.Default.GetString(MessageBuffer)

                    'Make some fixups regarding newlines, which means remove all instances of vbCr (Mac OS 9 newlines) and vbLf (Linux newlines).
                    'Windows hosts are affected, too, because it uses vbCrLf, which means (vbCr + vbLf)
                    Message = Message.Replace(vbCr, vbNullChar)
                    Message = Message.Replace(vbLf, vbNullChar)

                    'Don't post message if it starts with a null character.
                    If Not Message.StartsWith(vbNullChar) Then
                        'Fix the value of the message
                        Message = Message.Replace(vbNullChar, "")

                        'Now, check the message
                        If Message.StartsWith("/") Then
                            'Message is a command
                            Dim FullCommand As String = Message.Replace("/", "").Replace(vbNullChar, "")
                            Dim Command As String = FullCommand.Split(" ")(0)
                            If DebugCommands.ContainsKey(Command) Then
                                'Parsing starts here.
                                ParseCmd(FullCommand, SocketStreamWriter, SocketIP)
                            ElseIf RemoteDebugAliases.Keys.Contains(Command) Then
                                'Alias parsing starts here.
                                ExecuteRDAlias(FullCommand, SocketStreamWriter, SocketIP)
                            Else
                                SocketStreamWriter.WriteLine(DoTranslation("Command {0} not found. Use ""/help"" to see the list."), Command)
                            End If
                        Else
                            'Check to see if the unnamed stranger is trying to send a message
                            If Not String.IsNullOrEmpty(SocketName) Then
                                'Check the message format
                                If String.IsNullOrWhiteSpace(RDebugMessageFormat) Then
                                    RDebugMessageFormat = "{0}> {1}"
                                End If

                                'Decide if we're recording the chat to the debug log
                                If RecordChatToDebugLog Then
                                    Wdbg(DebugLevel.I, ProbePlaces(RDebugMessageFormat), SocketName, Message)
                                Else
                                    WdbgDevicesOnly(DebugLevel.I, ProbePlaces(RDebugMessageFormat), SocketName, Message)
                                End If

                                'Add the message to the chat history
                                SetDeviceProperty(SocketIP, DeviceProperty.ChatHistory, "[" + Render() + "] " + Message)
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Dim SE As SocketException = CType(ex.InnerException, SocketException)
                    If SE IsNot Nothing Then
                        If Not SE.SocketErrorCode = SocketError.TimedOut And Not SE.SocketErrorCode = SocketError.WouldBlock Then
                            Wdbg(DebugLevel.E, "Error from host {0}: {1}", SocketIP, SE.SocketErrorCode.ToString)
                            WStkTrc(ex)
                        End If
                    Else
                        Wdbg(DebugLevel.E, "Unknown error of remote debug: {0}", ex.Message)
                        WStkTrc(ex)
                    End If
                End Try
            End If
        End While
    End Sub

End Module
