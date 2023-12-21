//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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

using System.Net.Sockets;
using System.Text;
using System.Threading;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Screensaver;
using KS.Misc.Text;
using KS.Misc.Writers.DebugWriters;

namespace KS.Network.RPC
{
    /// <summary>
    /// RPC commands module
    /// </summary>
    public static class RPCCommands
    {
        private static bool received = false;

        /// <summary>
        /// List of RPC commands.<br/>
        /// <br/>&lt;Request:Shutdown&gt;: Shuts down the remote kernel. Usage: &lt;Request:Shutdown&gt;(IP)
        /// <br/>&lt;Request:Reboot&gt;: Reboots the remote kernel. Usage: &lt;Request:Reboot&gt;(IP)
        /// <br/>&lt;Request:RebootSafe&gt;: Reboots the remote kernel to safe mode. Usage: &lt;Request:RebootSafe&gt;(IP)
        /// <br/>&lt;Request:SaveScr&gt;: Saves the screen remotely. Usage: &lt;Request:SaveScr&gt;(IP)
        /// <br/>&lt;Request:Acknowledge&gt;: Pings the remote kernel silently. Usage: &lt;Request:Acknowledge&gt;(IP)
        /// <br/>&lt;Request:Ping&gt;: Pings the remote kernel with notification. Usage: &lt;Request:Ping&gt;(IP)
        /// </summary>
        private static readonly Dictionary<string, Action<string>> RPCCommandReplyActions = new() { { "Shutdown", (_) => HandleShutdown() }, { "Reboot", (_) => HandleReboot() }, { "RebootSafe", (_) => HandleRebootSafe() }, { "SaveScr", (_) => HandleSaveScr() }, { "Acknowledge", HandleAcknowledge }, { "Ping", HandlePing } };

        /// <summary>
        /// Send an RPC command to another instance of KS using the specified address
        /// </summary>
        /// <param name="Request">A request</param>
        /// <param name="IP">An IP address which the RPC is hosted</param>
        public static void SendCommand(string Request, string IP)
        {
            SendCommand(Request, IP, RemoteProcedure.RPCPort);
        }

        /// <summary>
        /// Send an RPC command to another instance of KS using the specified address
        /// </summary>
        /// <param name="Request">A request</param>
        /// <param name="IP">An IP address which the RPC is hosted</param>
        /// <param name="Port">A port which the RPC is hosted</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SendCommand(string Request, string IP, int Port)
        {
            if (RemoteProcedure.RPCEnabled)
            {
                // Get the command and the argument
                string Cmd = Request.Remove(Request.IndexOf("("));
                DebugWriter.Wdbg(DebugLevel.I, "Command: {0}", Cmd);
                string Arg = Request.Substring(Request.IndexOf("(") + 1);
                DebugWriter.Wdbg(DebugLevel.I, "Prototype Arg: {0}", Arg);
                Arg = Arg.Remove(Arg.Length - 1);
                DebugWriter.Wdbg(DebugLevel.I, "Finished Arg: {0}", Arg);

                // Check the command
                if (RPCCommandReplyActions.Keys.Any(Cmd.Contains))
                {
                    // Check the request type
                    DebugWriter.Wdbg(DebugLevel.I, "Command found.");
                    string RequestType = Cmd.Substring(Cmd.IndexOf(":") + 1, Cmd.IndexOf(">"));
                    byte[] ByteMsg = [];

                    // Populate the byte message to send the confirmation to
                    DebugWriter.Wdbg(DebugLevel.I, "Stream opened for device {0}", Arg);
                    ByteMsg = Encoding.Default.GetBytes($"{RequestType}Confirm, " + Arg + Kernel.Kernel.NewLine.ToString());

                    // Send the response
                    DebugWriter.Wdbg(DebugLevel.I, "Sending response to device...");
                    RemoteProcedure.RPCListen.Send(ByteMsg, ByteMsg.Length, IP, Port);
                    Kernel.Kernel.KernelEventManager.RaiseRPCCommandSent(Cmd, Arg, IP, Port);
                }
                else
                {
                    // Rare case reached. Drop it.
                    DebugWriter.Wdbg(DebugLevel.E, "Malformed request. {0}", Cmd);
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("Trying to send an RPC command while RPC didn't start."));
            }
        }

        /// <summary>
        /// Thread to listen to commands.
        /// </summary>
        public static void ReceiveCommand()
        {
            var RemoteEndpoint = new IPEndPoint(IPAddress.Any, RemoteProcedure.RPCPort);
            while (!Flags.RebootRequested)
            {
                try
                {
                    var receiveResult = RemoteProcedure.RPCListen.BeginReceive(new AsyncCallback(AcknowledgeMessage), null);
                    while (!received)
                    {
                        SpinWait.SpinUntil(new Func<bool>(() => received || Flags.RebootRequested));
                        if (Flags.RebootRequested)
                            break;
                    }
                }
                catch (Exception ex)
                {
                    SocketException SE = (SocketException)ex.InnerException;
                    if (SE is not null)
                    {
                        if (SE.SocketErrorCode != SocketError.TimedOut)
                        {
                            DebugWriter.Wdbg(DebugLevel.E, "Error from host: {0}", SE.SocketErrorCode.ToString());
                            DebugWriter.WStkTrc(ex);
                        }
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.E, "Fatal error: {0}", ex.Message);
                        DebugWriter.WStkTrc(ex);
                        Kernel.Kernel.KernelEventManager.RaiseRPCCommandError("", ex, RemoteEndpoint.Address.ToString(), RemoteEndpoint.Port);
                    }
                }
                received = false;
            }
        }

        private static void AcknowledgeMessage(IAsyncResult asyncResult)
        {
            received = true;

            // Invoke the action based on message
            Action<string> replyAction = null;
            try
            {
                if (RemoteProcedure.RPCListen is null || RemoteProcedure.RPCListen.Client is null)
                    return;
                if (Flags.RebootRequested)
                    return;
                if (RemoteProcedure.RPCListen.Available == 0)
                    return;
                var endpoint = new IPEndPoint(IPAddress.Any, RemoteProcedure.RPCPort);
                byte[] MessageBuffer = RemoteProcedure.RPCListen.EndReceive(asyncResult, ref endpoint);
                string Message = Encoding.Default.GetString(MessageBuffer);

                // Get the command and the argument
                string Cmd = Message.Remove(Message.IndexOf(","));
                DebugWriter.Wdbg(DebugLevel.I, "Command: {0}", Cmd);
                string Arg = Message.Substring(Message.IndexOf(",") + 2).Replace(Environment.NewLine, "");
                DebugWriter.Wdbg(DebugLevel.I, "Final Arg: {0}", Arg);

                // If the message is not empty, parse it
                if (!string.IsNullOrEmpty(Message))
                {
                    DebugWriter.Wdbg((DebugLevel)Convert.ToInt32("RPC: Received message {0}"), Message);
                    Kernel.Kernel.KernelEventManager.RaiseRPCCommandReceived(Message, endpoint.Address.ToString(), endpoint.Port);

                    if (RPCCommandReplyActions.TryGetValue(Cmd, out replyAction))
                    {
                        replyAction.Invoke(Arg);
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Not found. Message was {0}", Message);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed to acknowledge message: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
            }
            received = false;
        }

        private static void HandleShutdown()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Shutdown confirmed from remote access.");
            KernelTools.RPCPowerListener.Start(PowerMode.Shutdown);
        }

        private static void HandleReboot()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Reboot confirmed from remote access.");
            KernelTools.RPCPowerListener.Start(PowerMode.Reboot);
        }

        private static void HandleRebootSafe()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Reboot to safe mode confirmed from remote access.");
            KernelTools.RPCPowerListener.Start(PowerMode.RebootSafe);
        }

        private static void HandleSaveScr()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Save screen confirmed from remote access.");
            Screensaver.ShowSavers(Screensaver.DefSaverName);
            while (Screensaver.InSaver)
                Thread.Sleep(1);
        }

        private static void HandleAcknowledge(string value)
        {
            string IPAddr = value.Replace("AckConfirm, ", "").Replace(Kernel.Kernel.NewLine, "");
            DebugWriter.Wdbg(DebugLevel.I, "{0} says \"Hello.\"", IPAddr);
        }

        private static void HandlePing(string value)
        {
            string IPAddr = value.Replace("PingConfirm, ", "").Replace(Kernel.Kernel.NewLine, "");
            DebugWriter.Wdbg(DebugLevel.I, "{0} pinged this device!", IPAddr);
            Notifications.NotifySend(new Notification(Translate.DoTranslation("Ping!"), Translate.DoTranslation("{0} pinged you.").FormatString(IPAddr), Notifications.NotifPriority.Low, Notifications.NotifType.Normal));
        }
    }
}