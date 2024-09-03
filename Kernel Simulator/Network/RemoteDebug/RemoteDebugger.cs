﻿//
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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Notifiers;
using KS.Misc.Probers;
using KS.Misc.Threading;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Aliases;
using KS.TimeDate;

namespace KS.Network.RemoteDebug
{
    public static class RemoteDebugger
    {

        public static int DebugPort = 3014;
        public static Socket RDebugClient;
        public static TcpListener DebugTCP;
        public static List<RemoteDebugDevice> DebugDevices = [];
        public static KernelThread RDebugThread = new("Remote Debug Thread", true, StartRDebugger);
        public static List<string> RDebugBlocked = []; // Blocked IP addresses
        public static bool RDebugStopping;
        public static bool RDebugAutoStart = true;
        public static string RDebugMessageFormat = "";
        internal static bool RDebugFailed;
        internal static Exception RDebugFailedReason;
        private static readonly string RDebugVersion = "0.7.0";
        private static bool RDebugBail;

        /// <summary>
        /// Whether to start or stop the remote debugger
        /// </summary>
        public static void StartRDebugThread()
        {
            if (Flags.DebugMode)
            {
                if (!RDebugThread.IsAlive)
                {
                    RDebugThread.Start();
                    while (!RDebugBail)
                    {
                    }
                    RDebugBail = false;
                }
            }
        }

        /// <summary>
        /// Whether to start or stop the remote debugger
        /// </summary>
        public static void StopRDebugThread()
        {
            if (Flags.DebugMode)
            {
                if (RDebugThread.IsAlive)
                {
                    RDebugStopping = true;
                    RDebugThread.Stop();
                }
            }
        }

        /// <summary>
        /// Thread to accept connections after the listener starts
        /// </summary>
        public static void StartRDebugger()
        {
            // Listen to a current IP address
            try
            {
                DebugTCP = new TcpListener(new IPAddress([(byte)0, (byte)0, (byte)0, (byte)0]), DebugPort);
                DebugTCP.Start();
            }
            catch (SocketException sex)
            {
                RDebugFailed = true;
                RDebugFailedReason = sex;
                DebugWriter.WStkTrc(sex);
            }

            // Start the listening thread
            var RStream = new KernelThread("Remote Debug Listener Thread", false, ReadAndBroadcastAsync);
            RStream.Start();
            RDebugBail = true;

            // Run forever! Until the remote debugger is stopping.
            while (!RDebugStopping)
            {
                try
                {
                    Thread.Sleep(1);

                    // Variables
                    NetworkStream RDebugStream;
                    StreamWriter RDebugSWriter;
                    Socket RDebugClient;
                    string RDebugIP;
                    string RDebugEndpoint;
                    string RDebugName;
                    RemoteDebugDevice RDebugInstance;

                    // Check for pending connections
                    if (DebugTCP.Pending())
                    {
                        // Populate the device variables with the information
                        RDebugClient = DebugTCP.AcceptSocket();
                        RDebugStream = new NetworkStream(RDebugClient);

                        // Add the device to JSON
                        RDebugEndpoint = RDebugClient.RemoteEndPoint.ToString();
                        RDebugIP = RDebugEndpoint.Remove(RDebugClient.RemoteEndPoint.ToString().IndexOf(":"));
                        RemoteDebugTools.AddDeviceToJson(RDebugIP, false);

                        // Get the remaining properties
                        RDebugName = Convert.ToString(RemoteDebugTools.GetDeviceProperty(RDebugIP, RemoteDebugTools.DeviceProperty.Name));
                        RDebugInstance = new RemoteDebugDevice(RDebugClient, RDebugStream, RDebugIP, RDebugName);
                        RDebugSWriter = RDebugInstance.ClientStreamWriter;

                        // Check the name
                        if (string.IsNullOrEmpty(RDebugName))
                        {
                            DebugWriter.Wdbg(DebugLevel.W, "Debug device {0} has no name. Prompting for name...", RDebugIP);
                        }

                        // Check to see if the device is blocked
                        if (RDebugBlocked.Contains(RDebugIP))
                        {
                            // Blocked! Disconnect it.
                            DebugWriter.Wdbg(DebugLevel.W, "Debug device {0} ({1}) tried to join remote debug, but blocked.", RDebugName, RDebugIP);
                            RDebugClient.Disconnect(true);
                        }
                        else
                        {
                            // Not blocked yet. Add the connection.
                            DebugDevices.Add(RDebugInstance);
                            RDebugSWriter.WriteLine(Translate.DoTranslation(">> Remote Debug and Chat: version") + " {0}", RDebugVersion);
                            RDebugSWriter.WriteLine(Translate.DoTranslation(">> Your address is {0}."), RDebugIP);
                            if (string.IsNullOrEmpty(RDebugName))
                            {
                                RDebugSWriter.WriteLine(Translate.DoTranslation(">> Welcome! This is your first time entering remote debug and chat. Use \"/register <name>\" to register.") + " ", RDebugName);
                            }
                            else
                            {
                                RDebugSWriter.WriteLine(Translate.DoTranslation(">> Your name is {0}."), RDebugName);
                            }

                            // Acknowledge the debugger
                            DebugWriter.Wdbg(DebugLevel.I, "Debug device \"{0}\" ({1}) connected.", RDebugName, RDebugIP);
                            RDebugSWriter.Flush();
                            Kernel.Kernel.KernelEventManager.RaiseRemoteDebugConnectionAccepted(RDebugIP);
                        }
                    }
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    if (Flags.NotifyOnRemoteDebugConnectionError)
                    {
                        var RemoteDebugError = new Notification(Translate.DoTranslation("Remote debugger connection error"), ex.Message, Notifications.NotifPriority.Medium, Notifications.NotifType.Normal);
                        Notifications.NotifySend(RemoteDebugError);
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("Remote debugger connection error") + ": {0}", true, KernelColorTools.ColTypes.Error, ex.Message);
                    }
                    DebugWriter.WStkTrc(ex);
                }
            }

            try
            {
                RStream.Wait();
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.I, $"Failed to wait: {ex.Message}");
            }
            finally
            {
                RDebugStopping = false;
                DebugTCP.Stop();
                DebugDevices.Clear();
            }
        }

        /// <summary>
        /// Thread to listen to messages and post them to the debugger
        /// </summary>
        public static void ReadAndBroadcastAsync()
        {
            while (!RDebugStopping)
            {
                for (int DeviceIndex = 0; DeviceIndex <= DebugDevices.Count - 1; DeviceIndex++)
                {
                    var device = DebugDevices[DeviceIndex];
                    try
                    {
                        Thread.Sleep(1);

                        // Variables
                        var MessageBuffer = new byte[65537];
                        var SocketStream = new NetworkStream(device.ClientSocket);
                        var SocketStreamWriter = device.ClientStreamWriter;
                        string SocketIP = device.ClientIP;
                        string SocketName = device.ClientName;

                        // Read a message from the stream
                        if (!SocketStream.DataAvailable)
                            if (device.ClientSocket.Connected)
                                continue;
                            else
                                break;
                        SocketStream.Read(MessageBuffer, 0, 65536);
                        string Message = System.Text.Encoding.Default.GetString(MessageBuffer);

                        // Make some fixups regarding newlines, which means remove all instances of vbCr (Mac OS 9 newlines) and vbLf (Linux newlines).
                        // Windows hosts are affected, too, because it uses vbCrLf, which means (vbCr + vbLf)
                        Message = Message.Replace("\r", "\0");
                        Message = Message.Replace("\n", "\0");

                        // Don't post message if it starts with a null character. On Unix, the nullchar detection always returns false even if it seems
                        // that the message starts with the actual character, not the null character, so detect nullchar by getting the first character
                        // from the message and comparing it to the null char ASCII number, which is 0.
                        if (!(Convert.ToInt32(Message[0]) == 0))
                        {
                            // Fix the value of the message
                            Message = Message.Replace("\0", "");

                            // Now, check the message
                            if (Message.StartsWith("/"))
                            {
                                // Message is a command
                                string FullCommand = Message.Replace("/", "").Replace("\0", "");
                                string Command = FullCommand.Split(' ')[0];
                                if (RemoteDebugCmd.DebugCommands.ContainsKey(Command))
                                {
                                    // Parsing starts here.
                                    RemoteDebugCmd.ParseCmd(FullCommand, SocketStreamWriter, SocketIP);
                                }
                                else if (AliasManager.RemoteDebugAliases.ContainsKey(Command))
                                {
                                    // Alias parsing starts here.
                                    AliasExecutor.ExecuteRDAlias(FullCommand, SocketStreamWriter, SocketIP);
                                }
                                else
                                {
                                    SocketStreamWriter.WriteLine(Translate.DoTranslation("Command {0} not found. Use \"/help\" to see the list."), Command);
                                }
                            }
                            // Check to see if the unnamed stranger is trying to send a message
                            else if (!string.IsNullOrEmpty(SocketName))
                            {
                                // Check the message format
                                if (string.IsNullOrWhiteSpace(RDebugMessageFormat))
                                {
                                    RDebugMessageFormat = "{0}> {1}";
                                }

                                // Decide if we're recording the chat to the debug log
                                if (Flags.RecordChatToDebugLog)
                                {
                                    DebugWriter.Wdbg(DebugLevel.I, PlaceParse.ProbePlaces(RDebugMessageFormat), SocketName, Message);
                                }
                                else
                                {
                                    DebugWriter.WdbgDevicesOnly(DebugLevel.I, PlaceParse.ProbePlaces(RDebugMessageFormat), SocketName, Message);
                                }

                                // Add the message to the chat history
                                RemoteDebugTools.SetDeviceProperty(SocketIP, RemoteDebugTools.DeviceProperty.ChatHistory, "[" + TimeDateRenderers.Render() + "] " + Message);
                            }
                        }
                    }
                    catch (IOException ioex) when (ioex.Message.Contains("non-connected"))
                    {
                        // HACK: Ugly workaround, but we have to search the message for "non-connected" to get the specific error message that we
                        // need to react appropriately. Removing the device from the debug devices list will allow the kernel to continue working
                        // without crashing just because of this exception. We had to search the above word in this phrase:
                        // 
                        // System.IO.IOException: The operation is not allowed on non-connected sockets.
                        // ^^^^^^^^^^^^^
                        // 
                        // Though, we wish to have a better workaround to detect this specific error message on .NET Framework 4.8.
                        DebugDevices.RemoveAt(DeviceIndex);
                    }
                    catch (Exception ex)
                    {
                        SocketException SE = (SocketException)ex.InnerException;
                        if (SE is not null)
                        {
                            if (!(SE.SocketErrorCode == SocketError.TimedOut) & !(SE.SocketErrorCode == SocketError.WouldBlock))
                            {
                                if (DebugDevices.Count > DeviceIndex)
                                {
                                    string SocketIP = device?.ClientIP;
                                    DebugWriter.Wdbg(DebugLevel.E, "Error from host {0}: {1}", SocketIP, SE.SocketErrorCode.ToString());
                                    DebugWriter.WStkTrc(ex);
                                }
                                else
                                {
                                    DebugWriter.Wdbg(DebugLevel.E, "Error from unknown host: {0}", SE.SocketErrorCode.ToString());
                                    DebugWriter.WStkTrc(ex);
                                }
                            }
                        }
                        else
                        {
                            DebugWriter.Wdbg(DebugLevel.E, "Unknown error of remote debug: {0}: {1}", ex.GetType().FullName, ex.Message);
                            DebugWriter.WStkTrc(ex);
                        }
                    }
                }
            }
        }

    }
}