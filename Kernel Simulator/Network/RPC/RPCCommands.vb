
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

Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports KS.Misc.Notifications
Imports KS.Misc.Screensaver
Imports Renci.SshNet.Security

Namespace Network.RPC
    ''' <summary>
    ''' RPC commands module
    ''' </summary>
    Public Module RPCCommands
        Private received As Boolean = False

        ''' <summary>
        ''' List of RPC commands.<br/>
        ''' <br/>&lt;Request:Shutdown&gt;: Shuts down the remote kernel. Usage: &lt;Request:Shutdown&gt;(IP)
        ''' <br/>&lt;Request:Reboot&gt;: Reboots the remote kernel. Usage: &lt;Request:Reboot&gt;(IP)
        ''' <br/>&lt;Request:RebootSafe&gt;: Reboots the remote kernel to safe mode. Usage: &lt;Request:RebootSafe&gt;(IP)
        ''' <br/>&lt;Request:SaveScr&gt;: Saves the screen remotely. Usage: &lt;Request:SaveScr&gt;(IP)
        ''' <br/>&lt;Request:Acknowledge&gt;: Pings the remote kernel silently. Usage: &lt;Request:Acknowledge&gt;(IP)
        ''' <br/>&lt;Request:Ping&gt;: Pings the remote kernel with notification. Usage: &lt;Request:Ping&gt;(IP)
        ''' </summary>
        Private ReadOnly RPCCommandReplyActions As New Dictionary(Of String, Action(Of String))() From
        {
            {"Shutdown", AddressOf HandleShutdown},
            {"Reboot", AddressOf HandleReboot},
            {"RebootSafe", AddressOf HandleRebootSafe},
            {"SaveScr", AddressOf HandleSaveScr},
            {"Acknowledge", AddressOf HandleAcknowledge},
            {"Ping", AddressOf HandlePing}
        }

        ''' <summary>
        ''' Send an RPC command to another instance of KS using the specified address
        ''' </summary>
        ''' <paramname="Request">A request</param>
        ''' <paramname="IP">An IP address which the RPC is hosted</param>
        Public Sub SendCommand(Request As String, IP As String)
            SendCommand(Request, IP, RemoteProcedure.RPCPort)
        End Sub

        ''' <summary>
        ''' Send an RPC command to another instance of KS using the specified address
        ''' </summary>
        ''' <paramname="Request">A request</param>
        ''' <paramname="IP">An IP address which the RPC is hosted</param>
        ''' <paramname="Port">A port which the RPC is hosted</param>
        ''' <exceptioncref="InvalidOperationException"></exception>
        Public Sub SendCommand(Request As String, IP As String, Port As Integer)
            If RPCEnabled Then
                ' Get the command and the argument
                Dim Cmd = Request.Remove(Request.IndexOf("("))
                Wdbg(DebugLevel.I, "Command: {0}", Cmd)
                Dim Arg As String = Request.Substring(Request.IndexOf("(") + 1)
                Wdbg(DebugLevel.I, "Prototype Arg: {0}", Arg)
                Arg = Arg.Remove(Arg.Length - 1)
                Wdbg(DebugLevel.I, "Finished Arg: {0}", Arg)

                ' Check the command
                If RPCCommandReplyActions.Keys.Any(AddressOf Cmd.Contains) Then
                    ' Check the request type
                    Wdbg(DebugLevel.I, "Command found.")
                    Dim RequestType = Cmd.Substring(Cmd.IndexOf(":") + 1, Cmd.IndexOf(">"))
                    Dim ByteMsg = Array.Empty(Of Byte)()

                    ' Populate the byte message to send the confirmation to
                    Wdbg(DebugLevel.I, "Stream opened for device {0}", Arg)
                    ByteMsg = Encoding.Default.GetBytes($"{RequestType}Confirm, " & Arg & NewLine.ToString())

                    ' Send the response
                    Wdbg(DebugLevel.I, "Sending response to device...")
                    RPCListen.Send(ByteMsg, ByteMsg.Length, IP, Port)
                    KernelEventManager.RaiseRPCCommandSent(Cmd, Arg, IP, Port)
                Else
                    ' Rare case reached. Drop it.
                    Wdbg(DebugLevel.E, "Malformed request. {0}", Cmd)
                End If
            Else
                Throw New InvalidOperationException(DoTranslation("Trying to send an RPC command while RPC didn't start."))
            End If
        End Sub

        ''' <summary>
        ''' Thread to listen to commands.
        ''' </summary>
        Public Sub ReceiveCommand()
            Dim RemoteEndpoint = New IPEndPoint(IPAddress.Any, RPCPort)
            While Not RebootRequested
                Try
                    Dim receiveResult = RPCListen.BeginReceive(New AsyncCallback(AddressOf AcknowledgeMessage), Nothing)
                    While Not received
                        SpinWait.SpinUntil(New Func(Of Boolean)(Function() received OrElse RebootRequested))
                        If RebootRequested Then Exit While
                    End While
                Catch ex As Exception
                    Dim SE = CType(ex.InnerException, SocketException)
                    If SE IsNot Nothing Then
                        If SE.SocketErrorCode <> SocketError.TimedOut Then
                            Wdbg(DebugLevel.E, "Error from host: {0}", SE.SocketErrorCode.ToString())
                            WStkTrc(ex)
                        End If
                    Else
                        Wdbg(DebugLevel.E, "Fatal error: {0}", ex.Message)
                        WStkTrc(ex)
                        KernelEventManager.RaiseRPCCommandError("", ex, RemoteEndpoint.Address.ToString(), RemoteEndpoint.Port)
                    End If
                End Try
                received = False
            End While
        End Sub

        Private Sub AcknowledgeMessage(asyncResult As IAsyncResult)
            received = True

            ' Invoke the action based on message
            Dim replyAction As Action(Of String) = Nothing
            Try
                If RPCListen Is Nothing OrElse RPCListen.Client Is Nothing Then Return
                If RebootRequested Then Return
                If RPCListen.Available = 0 Then Return
                Dim endpoint = New IPEndPoint(IPAddress.Any, RPCPort)
                Dim MessageBuffer As Byte() = RPCListen.EndReceive(asyncResult, endpoint)
                Dim Message = Encoding.Default.GetString(MessageBuffer)

                ' Get the command and the argument
                Dim Cmd = Message.Remove(Message.IndexOf(","))
                Wdbg(DebugLevel.I, "Command: {0}", Cmd)
                Dim Arg = Message.Substring((Message.IndexOf(",") + 2)).Replace(Environment.NewLine, "")
                Wdbg(DebugLevel.I, "Final Arg: {0}", Arg)

                ' If the message is not empty, parse it
                If Not String.IsNullOrEmpty(Message) Then
                    Wdbg("RPC: Received message {0}", Message)
                    KernelEventManager.RaiseRPCCommandReceived(Message, endpoint.Address.ToString, endpoint.Port)

                    If RPCCommandReplyActions.TryGetValue(Cmd, replyAction) Then
                        replyAction.Invoke(Arg)
                    Else
                        Wdbg(DebugLevel.W, "Not found. Message was {0}", Message)
                    End If
                End If
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to acknowledge message: {0}", ex.Message)
                WStkTrc(ex)
            End Try
            received = False
        End Sub

        Private Sub HandleShutdown()
            Wdbg(DebugLevel.I, "Shutdown confirmed from remote access.")
            RPCPowerListener.Start(PowerMode.Shutdown)
        End Sub

        Private Sub HandleReboot()
            Wdbg(DebugLevel.I, "Reboot confirmed from remote access.")
            RPCPowerListener.Start(PowerMode.Reboot)
        End Sub

        Private Sub HandleRebootSafe()
            Wdbg(DebugLevel.I, "Reboot to safe mode confirmed from remote access.")
            RPCPowerListener.Start(PowerMode.RebootSafe)
        End Sub

        Private Sub HandleSaveScr()
            Wdbg(DebugLevel.I, "Save screen confirmed from remote access.")
            ShowSavers(DefSaverName)
            While InSaver
                Thread.Sleep(1)
            End While
        End Sub

        Private Sub HandleExec(value As String)
            Dim Command = value.Replace("ExecConfirm, ", "").Replace(NewLine, "")
            If LoggedIn Then
                Wdbg(DebugLevel.I, "Exec confirmed from remote access.")
                WritePlain("", True)
                GetLine(Command)
            Else
                Wdbg(DebugLevel.W, "Tried to exec from remote access while not logged in. Dropping packet...")
            End If
        End Sub

        Private Sub HandleAcknowledge(value As String)
            Dim IPAddr = value.Replace("AckConfirm, ", "").Replace(NewLine, "")
            Wdbg(DebugLevel.I, "{0} says ""Hello.""", IPAddr)
        End Sub

        Private Sub HandlePing(value As String)
            Dim IPAddr = value.Replace("PingConfirm, ", "").Replace(NewLine, "")
            Wdbg(DebugLevel.I, "{0} pinged this device!", IPAddr)
            NotifySend(New Notification(DoTranslation("Ping!"), FormatString(DoTranslation("{0} pinged you."), IPAddr), NotifPriority.Low, NotifType.Normal))
        End Sub
    End Module
End Namespace
