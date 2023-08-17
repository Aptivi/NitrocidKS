﻿
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using KS.Drivers;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Kernel.Debugging.Trace;
using KS.Kernel.Time;
using KS.Misc.Text;

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
        /// <summary>
        /// Censor private information that may be printed to the debug logs.
        /// </summary>
        public static bool DebugCensorPrivateInfo => Config.MainConfig.DebugCensorPrivateInfo;
        internal static string DebugPath = "";
        internal static StreamWriter DebugStreamWriter;
        internal static object WriteLock = new();

        /// <summary>
        /// Outputs the text into the debugger file, and sets the time stamp. Censors all secure arguments if <see cref="DebugCensorPrivateInfo"/> is on.
        /// </summary>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger file. Supports {0}, {1}, ...</param>
        /// <param name="SecureVarIndexes">Secure variable indexes to modify <paramref name="vars"/> to censor them when <see cref="DebugCensorPrivateInfo"/> is on</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebugPrivacy(DebugLevel Level, string text, int[] SecureVarIndexes, params object[] vars)
        {
            // First, iterate through all the provided secure indexes to convert these to censored strings
            foreach (int SecureVarIndex in SecureVarIndexes)
            {
                // Check the index value
                if (SecureVarIndex < 0)
                    continue;
                if (SecureVarIndex >= vars.Length)
                    continue;

                // Censor all the secure vars found
                if (DebugCensorPrivateInfo)
                    vars[SecureVarIndex] = "[removed for privacy]";
            }

            // Then, go ahead and write the message
            WriteDebug(Level, text, vars);
        }

        /// <summary>
        /// Outputs the text into the debugger file, and sets the time stamp.
        /// </summary>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger file. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebug(DebugLevel Level, string text, params object[] vars)
        {
            lock (WriteLock)
            {
                if (Flags.DebugMode)
                {
                    // Open debugging stream
                    string debugFilePath = DebugPath;
                    if (DebugStreamWriter is null | DebugStreamWriter?.BaseStream is null)
                        DebugStreamWriter = new StreamWriter(debugFilePath, true) { AutoFlush = true };

                    // Try to debug...
                    try
                    {
                        var STrace = new DebugStackFrame();
                        StringBuilder message = new();

                        // We could be calling this function by WriteDebugConditional or WriteDebugPrivacy, so descend a frame
                        if (STrace.RoutineName == nameof(WriteDebugConditional))
                            STrace = new DebugStackFrame(3);
                        if (STrace.RoutineName == nameof(WriteDebugPrivacy))
                            STrace = new DebugStackFrame(2);

                        // Remove the \r line endings from the text, since the debug file needs to have its line endings in the
                        // UNIX format anyways.
                        text = text.Replace(char.ToString((char)13), "");

                        // Handle the new lines
                        string[] texts = text.Split("\n");
                        foreach (string splitText in texts)
                        {
                            // Check to see if source file name is not empty.
                            if (STrace.RoutineFileName is not null & !(STrace.RoutineLineNumber == 0))
                                // Show stack information
                                message.Append($"{TimeDateTools.KernelDateTime.ToShortDateString()} {TimeDateTools.KernelDateTime.ToShortTimeString()} [{Level}] ({STrace.RoutineName} - {STrace.RoutineFileName}:{STrace.RoutineLineNumber}): {splitText}\n");
                            else
                                // Rare case, unless debug symbol is not found on archives.
                                message.Append($"{TimeDateTools.KernelDateTime.ToShortDateString()} {TimeDateTools.KernelDateTime.ToShortTimeString()} [{Level}] {splitText}\n");
                        }

                        // Debug to file and all connected debug devices (raw mode). The reason for the \r\n is that because
                        // Nitrocid on the Linux host tends to use \n only for new lines, and Windows considers \r\n as the
                        // new line. This causes the staircase effect on text written to the remote debugger, which messes up
                        // the output on Windows.
                        //
                        // However, we don't want to append the Windows new line character to the debug file, because we need
                        // it to have consistent line endings across platforms, like if you try to print the output of a text
                        // file that only has \n at the end of each line, we would inadvertently place the \r\n in each debug
                        // line, causing the file to have mixed line endings.
                        if (vars.Length > 0)
                            DriverHandler.CurrentDebugLoggerDriverLocal.Write(message.ToString(), vars);
                        else
                            DriverHandler.CurrentDebugLoggerDriverLocal.Write(message.ToString());
                        message.Replace("\n", "\r\n");
                        for (int i = 0; i <= RemoteDebugger.DebugDevices.Count - 1; i++)
                        {
                            try
                            {
                                if (vars.Length > 0)
                                    RemoteDebugger.DebugDevices[i].ClientStreamWriter.Write(message.ToString(), vars);
                                else
                                    RemoteDebugger.DebugDevices[i].ClientStreamWriter.Write(message);
                            }
                            catch (Exception ex)
                            {
                                RemoteDebugTools.DisconnectDependingOnException(ex, i);
                                if (i > 0)
                                    i--;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteDebug(DebugLevel.F, "Debugger error: {0}", ex.Message);
                        WriteDebugStackTrace(ex);
                    }
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
        public static void WriteDebugConditional(bool Condition, DebugLevel Level, string text, params object[] vars)
        {
            lock (WriteLock)
            {
                if (Condition)
                    WriteDebug(Level, text, vars);
            }
        }

        /// <summary>
        /// Outputs the text into the debugger devices, and sets the time stamp. Note that it doesn't print where did the debugger debug in source files.
        /// </summary>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger devices. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebugDevicesOnly(DebugLevel Level, string text, params object[] vars)
        {
            lock (WriteLock)
            {
                if (Flags.DebugMode)
                {
                    for (int i = 0; i <= RemoteDebugger.DebugDevices.Count - 1; i++)
                    {
                        try
                        {
                            RemoteDebugger.DebugDevices[i].ClientStreamWriter.Write($"{TimeDateTools.KernelDateTime.ToShortDateString()} {TimeDateTools.KernelDateTime.ToShortTimeString()} [{Level}] {text}\r\n", vars);
                        }
                        catch (Exception ex)
                        {
                            RemoteDebugTools.DisconnectDependingOnException(ex, i);
                            if (i > 0)
                                i--;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Conditionally writes the exception's stack trace to the debugger
        /// </summary>
        /// <param name="Condition">The condition that must be satisfied</param>
        /// <param name="Ex">An exception</param>
        public static void WriteDebugStackTraceConditional(bool Condition, Exception Ex)
        {
            lock (WriteLock)
            {
                if (Condition)
                    WriteDebugStackTrace(Ex);
            }
        }

        /// <summary>
        /// Writes the exception's stack trace to the debugger
        /// </summary>
        /// <param name="Ex">An exception</param>
        public static void WriteDebugStackTrace(Exception Ex)
        {
            lock (WriteLock)
            {
                if (Flags.DebugMode)
                {
                    // These two NewLines are padding for accurate stack tracing.
                    var Inner = Ex.InnerException;
                    int InnerNumber = 1;
                    var NewStackTraces = new List<string>() { $"{CharManager.NewLine}{Ex.ToString()[..Ex.ToString().IndexOf(":")]}: {Ex.Message}{CharManager.NewLine}{Ex.StackTrace}{CharManager.NewLine}" };

                    // Get all the inner exceptions
                    while (Inner is not null)
                    {
                        NewStackTraces.Add($"[{InnerNumber}] {Inner.ToString()[..Inner.ToString().IndexOf(":")]}: {Inner.Message}{CharManager.NewLine}{Inner.StackTrace}{CharManager.NewLine}");
                        InnerNumber += 1;
                        Inner = Inner.InnerException;
                    }

                    // Print stack trace to debugger
                    var StkTrcs = new List<string>();
                    for (int i = 0; i <= NewStackTraces.Count - 1; i++)
                        StkTrcs.AddRange(NewStackTraces[i].SplitNewLines());
                    for (int i = 0; i <= StkTrcs.Count - 1; i++)
                        WriteDebug(DebugLevel.T, StkTrcs[i]);
                    DebugStackTraces.AddRange(NewStackTraces);
                }
            }
        }

    }
}
