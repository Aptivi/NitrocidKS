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

Public Module RemoteProcedure

    Public RPCListen As UdpClient
    Public RPCPort As Integer = 12345
    Public RPCEnabled As Boolean = True
    Friend RPCThread As New Thread(AddressOf ReceiveCommand) With {.IsBackground = True, .Name = "RPC Thread"}

    ''' <summary>
    ''' Whether the RPC started
    ''' </summary>
    Public ReadOnly Property RPCStarted As Boolean
        Get
            Return RPCThread.IsAlive
        End Get
    End Property

    ''' <summary>
    ''' Starts the RPC listener
    ''' </summary>
    Sub StartRPC()
        If RPCEnabled Then
            Try
                Wdbg(DebugLevel.I, "RPC: Starting...")
                If Not RPCStarted Then
                    RPCListen = New UdpClient(RPCPort) With {.EnableBroadcast = True}
                    Wdbg(DebugLevel.I, "RPC: Listener started")
                    RPCThread.Start()
                    Wdbg(DebugLevel.I, "RPC: Thread started")
                    Write(DoTranslation("RPC listening on all addresses using port {0}."), True, ColTypes.Neutral, RPCPort)
                Else
                    Throw New ThreadStateException(DoTranslation("Trying to start RPC while it's already started."))
                End If
            Catch ex As ThreadStateException
                Write(DoTranslation("RPC is already running."), True, ColTypes.Error)
                WStkTrc(ex)
            End Try
        Else
            Write(DoTranslation("Not starting RPC because it's disabled."), True, ColTypes.Neutral)
        End If
    End Sub

    ''' <summary>
    ''' Stops the RPC listener
    ''' </summary>
    Sub StopRPC()
        If RPCStarted Then
            RPCThread.Abort()
            RPCListen?.Close()
            RPCListen = Nothing
            RPCThread = New Thread(AddressOf ReceiveCommand) With {.IsBackground = True, .Name = "RPC Thread"}
            Wdbg(DebugLevel.I, "RPC stopped.")
        Else
            Wdbg(DebugLevel.E, "RPC hasn't started yet!")
        End If
    End Sub

End Module
