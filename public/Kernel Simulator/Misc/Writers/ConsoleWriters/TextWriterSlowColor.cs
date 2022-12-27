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
using System.Threading;
using ColorSeq;
using KS.Kernel.Debugging;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Drivers;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Writers.ConsoleWriters
{
    /// <summary>
    /// Console text writer with color support (slow write)
    /// </summary>
    public static class TextWriterSlowColor
    {

        /// <summary>
        /// Outputs the text into the terminal prompt slowly with color support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteSlowly(string msg, bool Line, double MsEachLetter, KernelColorType colorType, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    ColorTools.SetConsoleColor(colorType);

                    // Write text slowly
                    DriverHandler.CurrentConsoleDriver.WriteSlowlyPlain(msg, Line, MsEachLetter, vars);
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt slowly with color support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteSlowly(string msg, bool Line, double MsEachLetter, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    ColorTools.SetConsoleColor(colorTypeForeground);
                    ColorTools.SetConsoleColor(colorTypeBackground, true);

                    // Write text slowly
                    DriverHandler.CurrentConsoleDriver.WriteSlowlyPlain(msg, Line, MsEachLetter, vars);
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt slowly with color support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteSlowly(string msg, bool Line, double MsEachLetter, ConsoleColors color, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    ColorTools.SetConsoleColor(new Color(Convert.ToInt32(color)));
                    ColorTools.SetConsoleColor(KernelColorType.Background, true);

                    // Write text slowly
                    DriverHandler.CurrentConsoleDriver.WriteSlowlyPlain(msg, Line, MsEachLetter, vars);
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt slowly with color support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteSlowly(string msg, bool Line, double MsEachLetter, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    ColorTools.SetConsoleColor(new Color(Convert.ToInt32(ForegroundColor)));
                    ColorTools.SetConsoleColor(new Color(Convert.ToInt32(BackgroundColor)));

                    // Write text slowly
                    DriverHandler.CurrentConsoleDriver.WriteSlowlyPlain(msg, Line, MsEachLetter, vars);
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt slowly with color support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteSlowly(string msg, bool Line, double MsEachLetter, Color color, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    ColorTools.SetConsoleColor(color);
                    ColorTools.SetConsoleColor(KernelColorType.Background, true);

                    // Write text slowly
                    DriverHandler.CurrentConsoleDriver.WriteSlowlyPlain(msg, Line, MsEachLetter, vars);
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt slowly with color support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteSlowly(string msg, bool Line, double MsEachLetter, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    ColorTools.SetConsoleColor(ForegroundColor);
                    ColorTools.SetConsoleColor(BackgroundColor, true);

                    // Write text slowly
                    DriverHandler.CurrentConsoleDriver.WriteSlowlyPlain(msg, Line, MsEachLetter, vars);
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

    }
}
