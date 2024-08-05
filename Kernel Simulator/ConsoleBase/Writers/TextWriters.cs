//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Terminaux.Writer.ConsoleWriters;
using System;
using System.Collections.Generic;
using System.Threading;
using Terminaux.Base;
using KS.ConsoleBase.Colors;
using static KS.ConsoleBase.Colors.KernelColorTools;
using KS.Misc.Writers.DebugWriters;
using KS.Languages;
using KS.Kernel;

namespace KS.ConsoleBase.Writers
{
    /// <summary>
    /// Text writer wrapper for writing with ColTypes (<see cref="Terminaux.Writer.ConsoleWriters"/>)
    /// </summary>
    public static class TextWriters
    {
        internal static object WriteLock = new();

        /// <summary>
        /// Outputs a list entry and value into the terminal prompt.
        /// </summary>
        /// <param name="entry">A list entry that will be listed to the terminal prompt.</param>
        /// <param name="value">A list value that will be listed to the terminal prompt.</param>
        /// <param name="indent">Indentation level</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteListEntry(string entry, string value, ColTypes ListKeyColor, ColTypes ListValueColor, int indent = 0) =>
            ListEntryWriterColor.WriteListEntry(entry, value, KernelColorTools.GetConsoleColor(ListKeyColor), KernelColorTools.GetConsoleColor(ListValueColor), indent);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, ColTypes ListKeyColor, ColTypes ListValueColor) =>
           ListWriterColor.WriteList(List, KernelColorTools.GetConsoleColor(ListKeyColor), KernelColorTools.GetConsoleColor(ListValueColor), Flags.WrapListOutputs);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, ColTypes ListKeyColor, ColTypes ListValueColor, bool Wrap) =>
            ListWriterColor.WriteList(List, KernelColorTools.GetConsoleColor(ListKeyColor), KernelColorTools.GetConsoleColor(ListValueColor), Wrap);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<T>(IEnumerable<T> List, ColTypes ListKeyColor, ColTypes ListValueColor) =>
            ListWriterColor.WriteList(List, KernelColorTools.GetConsoleColor(ListKeyColor), KernelColorTools.GetConsoleColor(ListValueColor), Flags.WrapListOutputs);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<T>(IEnumerable<T> List, ColTypes ListKeyColor, ColTypes ListValueColor, bool Wrap) =>
            ListWriterColor.WriteList(List, KernelColorTools.GetConsoleColor(ListKeyColor), KernelColorTools.GetConsoleColor(ListValueColor), Wrap);

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, ColTypes colorType, params object[] vars) =>
            Write(Text, true, false, colorType, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ColTypes colorType, params object[] vars) =>
            Write(Text, Line, false, colorType, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, ColTypes colorType, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    KernelColorTools.SetConsoleColorDry(colorType, Highlight, false);
                    KernelColorTools.SetConsoleColorDry(KernelColorTools.BackgroundColor, !Highlight, false);

                    // Write the text to console
                    if (Highlight)
                    {
                        TextWriterRaw.WritePlain(Text, false, vars);
                        KernelColorTools.SetConsoleColorDry(colorType);
                        KernelColorTools.SetConsoleColorDry(KernelColorTools.BackgroundColor, true);
                        TextWriterRaw.WritePlain("", Line);
                    }
                    else
                    {
                        TextWriterRaw.WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WStkTrc(ex);
                    DebugWriter.Wdbg(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, ColTypes colorTypeForeground, ColTypes colorTypeBackground, params object[] vars) =>
            Write(Text, true, false, colorTypeForeground, colorTypeBackground, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ColTypes colorTypeForeground, ColTypes colorTypeBackground, params object[] vars) =>
            Write(Text, Line, false, colorTypeForeground, colorTypeBackground, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, ColTypes colorTypeForeground, ColTypes colorTypeBackground, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    KernelColorTools.SetConsoleColorDry(colorTypeForeground, Highlight, false);
                    KernelColorTools.SetConsoleColorDry(colorTypeBackground, !Highlight, false);

                    // Write the text to console
                    if (Highlight)
                    {
                        TextWriterRaw.WritePlain(Text, false, vars);
                        KernelColorTools.SetConsoleColorDry(colorTypeForeground);
                        KernelColorTools.SetConsoleColorDry(colorTypeBackground, true);
                        TextWriterRaw.WritePlain("", Line);
                    }
                    else
                    {
                        TextWriterRaw.WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WStkTrc(ex);
                    DebugWriter.Wdbg(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, ColTypes colorType, params object[] vars) =>
            WriteWhere(msg, Left, Top, false, 0, colorType, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, bool Return, ColTypes colorType, params object[] vars) =>
            WriteWhere(msg, Left, Top, Return, 0, colorType, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, bool Return, int RightMargin, ColTypes colorType, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    ConsoleWrapper.Write(TextWriterWhereColor.RenderWhereColorBack(msg, Left, Top, Return, RightMargin, KernelColorTools.GetConsoleColor(colorType), KernelColorTools.BackgroundColor, vars));
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WStkTrc(ex);
                    DebugWriter.Wdbg(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, ColTypes colorTypeForeground, ColTypes colorTypeBackground, params object[] vars) =>
            WriteWhere(msg, Left, Top, false, 0, colorTypeForeground, colorTypeBackground, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, bool Return, ColTypes colorTypeForeground, ColTypes colorTypeBackground, params object[] vars) =>
            WriteWhere(msg, Left, Top, Return, 0, colorTypeForeground, colorTypeBackground, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, bool Return, int RightMargin, ColTypes colorTypeForeground, ColTypes colorTypeBackground, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    ConsoleWrapper.Write(TextWriterWhereColor.RenderWhereColorBack(msg, Left, Top, Return, RightMargin, KernelColorTools.GetConsoleColor(colorTypeForeground), KernelColorTools.GetConsoleColor(colorTypeBackground), vars));
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WStkTrc(ex);
                    DebugWriter.Wdbg(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }
    }
}
