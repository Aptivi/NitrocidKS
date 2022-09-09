﻿
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Extensification.StringExts;
using KS.Files;
using KS.Kernel.Debugging.RemoteDebug;

namespace KS.Kernel.Debugging
{
    /// <summary>
    /// Debug writing module
    /// </summary>
    public static class DebugWriter
    {

        /// <summary>
        /// Debug stack trace list
        /// </summary>
        public readonly static List<string> DebugStackTraces = new();
        internal static StreamWriter DebugStreamWriter;

        /// <summary>
        /// Outputs the text into the debugger file, and sets the time stamp.
        /// </summary>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger file. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebug(DebugLevel Level, string text, params object[] vars)
        {
            if (Flags.DebugMode)
            {
                // Open debugging stream
                if (DebugStreamWriter is null | DebugStreamWriter?.BaseStream is null)
                    DebugStreamWriter = new StreamWriter(Paths.GetKernelPath(KernelPathType.Debugging), true) { AutoFlush = true };

                // Try to debug...
                try
                {
                    var STrace = new StackTrace(true);
                    string Source = Path.GetFileName(STrace.GetFrame(1).GetFileName());
                    string LineNum = STrace.GetFrame(1).GetFileLineNumber().ToString();
                    string Func = STrace.GetFrame(1).GetMethod().Name;
                    var OffendingIndex = new List<string>();

                    // We could be calling this function by WdbgConditional, so descend a frame
                    if (Func == "WdbgConditional")
                    {
                        Source = Path.GetFileName(STrace.GetFrame(2).GetFileName());
                        LineNum = STrace.GetFrame(2).GetFileLineNumber().ToString();
                        Func = STrace.GetFrame(2).GetMethod().Name;
                    }

                    // Apparently, GetFileName on Mono in Linux doesn't work for MDB files made using pdb2mdb for PDB files that are generated by Visual Studio, so we take the last entry for the backslash to get the source file name.
                    if (KernelPlatform.IsOnUnix())
                    {
                        if (!string.IsNullOrEmpty(Source))
                        {
                            Source = Source.Split('\\')[Source.Split('\\').Length - 1];
                        }
                    }

                    // Check for debug quota
                    if (Flags.CheckDebugQuota)
                        DebugManager.CheckForDebugQuotaExceed();

                    // Check to see if source file name is not empty.
                    if (Source is not null & !(Convert.ToDouble(LineNum) == 0d))
                    {
                        // Debug to file and all connected debug devices (raw mode)
                        DebugStreamWriter.WriteLine($"{TimeDate.TimeDate.KernelDateTime.ToShortDateString()} {TimeDate.TimeDate.KernelDateTime.ToShortTimeString()} [{Level}] ({Func} - {Source}:{LineNum}): {text}", vars);
                        for (int i = 0, loopTo = RemoteDebugger.DebugDevices.Count - 1; i <= loopTo; i++)
                        {
                            try
                            {
                                RemoteDebugger.DebugDevices[i].ClientStreamWriter.WriteLine($"{TimeDate.TimeDate.KernelDateTime.ToShortDateString()} {TimeDate.TimeDate.KernelDateTime.ToShortTimeString()} [{Level}] ({Func} - {Source}:{LineNum}): {text}", vars);
                            }
                            catch (Exception ex)
                            {
                                OffendingIndex.Add(i.ToString());
                                WriteDebugStackTrace(ex);
                            }
                        }
                    }
                    else // Rare case, unless debug symbol is not found on archives.
                    {
                        DebugStreamWriter.WriteLine($"{TimeDate.TimeDate.KernelDateTime.ToShortDateString()} {TimeDate.TimeDate.KernelDateTime.ToShortTimeString()} [{Level}] {text}", vars);
                        for (int i = 0, loopTo1 = RemoteDebugger.DebugDevices.Count - 1; i <= loopTo1; i++)
                        {
                            try
                            {
                                RemoteDebugger.DebugDevices[i].ClientStreamWriter.WriteLine($"{TimeDate.TimeDate.KernelDateTime.ToShortDateString()} {TimeDate.TimeDate.KernelDateTime.ToShortTimeString()} [{Level}] {text}", vars);
                            }
                            catch (Exception ex)
                            {
                                OffendingIndex.Add(i.ToString());
                                WriteDebugStackTrace(ex);
                            }
                        }
                    }

                    // Disconnect offending clients who are disconnected
                    foreach (string si in OffendingIndex)
                    {
                        int i = int.Parse(si);
                        if (i != -1)
                        {
                            RemoteDebugger.DebugDevices[i].ClientSocket.Disconnect(true);
                            Kernel.KernelEventManager.RaiseRemoteDebugConnectionDisconnected(RemoteDebugger.DebugDevices[i].ClientIP);
                            WriteDebug(DebugLevel.W, "Debug device {0} ({1}) disconnected.", RemoteDebugger.DebugDevices[i].ClientName, RemoteDebugger.DebugDevices[i].ClientIP);
                            RemoteDebugger.DebugDevices.RemoveAt(i);
                        }
                    }
                    OffendingIndex.Clear();
                }
                catch (Exception ex)
                {
                    WriteDebug(DebugLevel.F, "Debugger error: {0}", ex.Message);
                    WriteDebugStackTrace(ex);
                }
            }
        }

        /// <summary>
        /// Conditionally outputs the text into the debugger file, and sets the time stamp.
        /// </summary>
        /// <param name="Condition">The condition that must be satisfied</param>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger file. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebugConditional(ref bool Condition, DebugLevel Level, string text, params object[] vars)
        {
            if (Condition)
                WriteDebug(Level, text, vars);
        }

        /// <summary>
        /// Outputs the text into the debugger devices, and sets the time stamp. Note that it doesn't print where did the debugger debug in source files.
        /// </summary>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger devices. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebugDevicesOnly(DebugLevel Level, string text, params object[] vars)
        {
            if (Flags.DebugMode)
            {
                var OffendingIndex = new List<string>();

                // For contributors who are testing new code: Define ENABLEIMMEDIATEWINDOWDEBUG for immediate debugging (Immediate Window)
                for (int i = 0, loopTo = RemoteDebugger.DebugDevices.Count - 1; i <= loopTo; i++)
                {
                    try
                    {
                        RemoteDebugger.DebugDevices[i].ClientStreamWriter.WriteLine($"{TimeDate.TimeDate.KernelDateTime.ToShortDateString()} {TimeDate.TimeDate.KernelDateTime.ToShortTimeString()} [{Level}] {text}", vars);
                    }
                    catch (Exception ex)
                    {
                        OffendingIndex.Add(i.ToString());
                        WriteDebugStackTrace(ex);
                    }
                }

                // Disconnect offending clients who are disconnected
                foreach (string si in OffendingIndex)
                {
                    int i = int.Parse(si);
                    if (i != -1)
                    {
                        RemoteDebugger.DebugDevices[i].ClientSocket.Disconnect(true);
                        Kernel.KernelEventManager.RaiseRemoteDebugConnectionDisconnected(RemoteDebugger.DebugDevices[i].ClientIP);
                        WriteDebug(DebugLevel.W, "Debug device {0} ({1}) disconnected.", RemoteDebugger.DebugDevices[i].ClientName, RemoteDebugger.DebugDevices[i].ClientIP);
                        RemoteDebugger.DebugDevices.RemoveAt(i);
                    }
                }
                OffendingIndex.Clear();
            }
        }

        /// <summary>
        /// Conditionally writes the exception's stack trace to the debugger
        /// </summary>
        /// <param name="Condition">The condition that must be satisfied</param>
        /// <param name="Ex">An exception</param>
        public static void WriteDebugStackTraceConditional(ref bool Condition, Exception Ex)
        {
            if (Condition)
                WriteDebugStackTrace(Ex);
        }

        /// <summary>
        /// Writes the exception's stack trace to the debugger
        /// </summary>
        /// <param name="Ex">An exception</param>
        public static void WriteDebugStackTrace(Exception Ex)
        {
            if (Flags.DebugMode)
            {
                // These two NewLines are padding for accurate stack tracing.
                var Inner = Ex.InnerException;
                int InnerNumber = 1;
                var NewStackTraces = new List<string>() { $"{Kernel.NewLine}{Ex.ToString().Substring(0, Ex.ToString().IndexOf(":"))}: {Ex.Message}{Kernel.NewLine}{Ex.StackTrace}{Kernel.NewLine}" };

                // Get all the inner exceptions
                while (Inner is not null)
                {
                    NewStackTraces.Add($"[{InnerNumber}] {Inner.ToString().Substring(0, Inner.ToString().IndexOf(":"))}: {Inner.Message}{Kernel.NewLine}{Inner.StackTrace}{Kernel.NewLine}");
                    InnerNumber += 1;
                    Inner = Inner.InnerException;
                }

                // Print stack trace to debugger
                var StkTrcs = new List<string>();
                for (int i = 0, loopTo = NewStackTraces.Count - 1; i <= loopTo; i++)
                    StkTrcs.AddRange(NewStackTraces[i].SplitNewLines());
                for (int i = 0, loopTo1 = StkTrcs.Count - 1; i <= loopTo1; i++)
                    WriteDebug(DebugLevel.E, StkTrcs[i]);
                DebugStackTraces.AddRange(NewStackTraces);
            }
        }

    }
}
