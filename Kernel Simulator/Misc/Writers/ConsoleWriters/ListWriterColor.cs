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
using System.Collections;
using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;
using Terminaux.Colors;
namespace KS.Misc.Writers.ConsoleWriters
{
    public static class ListWriterColor
    {
        #region Dictionary
        /// <summary>
        /// Outputs the list entries into the terminal prompt. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List)
        {
            WriteList(List, Flags.WrapListOutputs);
        }

        /// <summary>
        /// Outputs the list entries into the terminal prompt, and wraps output if needed.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, bool Wrap)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Variables
                    var LinesMade = default(int);
                    int OldTop;

                    // Try to write list to console
                    OldTop = ConsoleWrapper.CursorTop;
                    foreach (TKey ListEntry in List.Keys)
                    {
                        var Values = new List<object>();
                        if (List[ListEntry] as IEnumerable is not null & List[ListEntry] as string is null)
                        {
                            foreach (var Value in (IEnumerable)List[ListEntry])
                                Values.Add(Value);
                            TextWriterColor.Write("- {0}: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), ListEntry);
                            TextWriterColor.Write("{0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), string.Join(", ", Values));
                        }
                        else
                        {
                            TextWriterColor.Write("- {0}: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), ListEntry);
                            TextWriterColor.Write("{0}", true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), List[ListEntry]);
                        }
                        if (Wrap)
                        {
                            LinesMade += ConsoleWrapper.CursorTop - OldTop;
                            OldTop = ConsoleWrapper.CursorTop;
                            if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                            {
                                if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                    break;
                                LinesMade = 0;
                            }
                        }
                        else if (Console.KeyAvailable)
                        {
                            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                break;
                        }
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, ConsoleColor ListKeyColor, ConsoleColor ListValueColor)
        {
            WriteList(List, ListKeyColor, ListValueColor, Flags.WrapListOutputs);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, ConsoleColor ListKeyColor, ConsoleColor ListValueColor, bool Wrap)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Variables
                    var LinesMade = default(int);
                    int OldTop;

                    // Try to write list to console
                    OldTop = ConsoleWrapper.CursorTop;
                    foreach (TKey ListEntry in List.Keys)
                    {
                        var Values = new List<object>();
                        if (List[ListEntry] as IEnumerable is not null & List[ListEntry] as string is null)
                        {
                            foreach (var Value in (IEnumerable)List[ListEntry])
                                Values.Add(Value);
                            TextWriterColor.Write("- {0}: ", false, ListKeyColor, ListEntry);
                            TextWriterColor.Write("{0}", true, color: ListValueColor, string.Join(", ", Values));
                        }
                        else
                        {
                            TextWriterColor.Write("- {0}: ", false, ListKeyColor, ListEntry);
                            TextWriterColor.Write("{0}", true, ListValueColor, List[ListEntry]);
                        }
                        if (Wrap)
                        {
                            LinesMade += ConsoleWrapper.CursorTop - OldTop;
                            OldTop = ConsoleWrapper.CursorTop;
                            if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                            {
                                if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                    break;
                                LinesMade = 0;
                            }
                        }
                        else if (Console.KeyAvailable)
                        {
                            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                break;
                        }
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, Color ListKeyColor, Color ListValueColor)
        {
            WriteList(List, ListKeyColor, ListValueColor, Flags.WrapListOutputs);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, Color ListKeyColor, Color ListValueColor, bool Wrap)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Variables
                    var LinesMade = default(int);
                    int OldTop;

                    // Try to write list to console
                    OldTop = ConsoleWrapper.CursorTop;
                    foreach (TKey ListEntry in List.Keys)
                    {
                        var Values = new List<object>();
                        if (List[ListEntry] as IEnumerable is not null & List[ListEntry] as string is null)
                        {
                            foreach (var Value in (IEnumerable)List[ListEntry])
                                Values.Add(Value);
                            TextWriterColor.Write("- {0}: ", false, ListKeyColor, ListEntry);
                            TextWriterColor.Write("{0}", true, ListValueColor, string.Join(", ", Values));
                        }
                        else
                        {
                            TextWriterColor.Write("- {0}: ", false, ListKeyColor, ListEntry);
                            TextWriterColor.Write("{0}", true, ListValueColor, List[ListEntry]);
                        }
                        if (Wrap)
                        {
                            LinesMade += ConsoleWrapper.CursorTop - OldTop;
                            OldTop = ConsoleWrapper.CursorTop;
                            if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                            {
                                if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                    break;
                                LinesMade = 0;
                            }
                        }
                        else if (Console.KeyAvailable)
                        {
                            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                break;
                        }
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }
        #endregion
        #region Enumerables
        /// <summary>
        /// Outputs the list entries into the terminal prompt. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        public static void WriteList<T>(IEnumerable<T> List)
        {
            WriteList(List, Flags.WrapListOutputs);
        }

        /// <summary>
        /// Outputs the list entries into the terminal prompt, and wraps output if needed.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<T>(IEnumerable<T> List, bool Wrap)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Variables
                    var LinesMade = default(int);
                    int OldTop;
                    int EntryNumber = 1;

                    // Try to write list to console
                    OldTop = ConsoleWrapper.CursorTop;
                    foreach (T ListEntry in List)
                    {
                        var Values = new List<object>();
                        if (ListEntry as IEnumerable is not null & ListEntry as string is null)
                        {
                            foreach (var Value in (IEnumerable)ListEntry)
                                Values.Add(Value);
                            TextWriterColor.Write("- [{0}] ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), EntryNumber);
                            TextWriterColor.Write("{0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), string.Join(", ", Values));
                        }
                        else
                        {
                            TextWriterColor.Write("- [{0}] ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), EntryNumber);
                            TextWriterColor.Write("{0}", true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), ListEntry);
                        }
                        EntryNumber += 1;
                        if (Wrap)
                        {
                            LinesMade += ConsoleWrapper.CursorTop - OldTop;
                            OldTop = ConsoleWrapper.CursorTop;
                            if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                            {
                                if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                    break;
                                LinesMade = 0;
                            }
                        }
                        else if (Console.KeyAvailable)
                        {
                            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                break;
                        }
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<T>(IEnumerable<T> List, ConsoleColor ListKeyColor, ConsoleColor ListValueColor)
        {
            WriteList(List, ListKeyColor, ListValueColor, Flags.WrapListOutputs);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<T>(IEnumerable<T> List, ConsoleColor ListKeyColor, ConsoleColor ListValueColor, bool Wrap)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Variables
                    var LinesMade = default(int);
                    int OldTop;
                    int EntryNumber = 1;

                    // Try to write list to console
                    OldTop = ConsoleWrapper.CursorTop;
                    foreach (T ListEntry in List)
                    {
                        var Values = new List<object>();
                        if (ListEntry as IEnumerable is not null & ListEntry as string is null)
                        {
                            foreach (var Value in (IEnumerable)ListEntry)
                                Values.Add(Value);
                            TextWriterColor.Write("- [{0}] ", false, color: ListKeyColor, EntryNumber);
                            TextWriterColor.Write("{0}", true, color: ListValueColor, string.Join(", ", Values));
                        }
                        else
                        {
                            TextWriterColor.Write("- [{0}] ", false, color: ListKeyColor, EntryNumber);
                            TextWriterColor.Write("{0}", true, ListValueColor, ListEntry);
                        }
                        EntryNumber += 1;
                        if (Wrap)
                        {
                            LinesMade += ConsoleWrapper.CursorTop - OldTop;
                            OldTop = ConsoleWrapper.CursorTop;
                            if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                            {
                                if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                    break;
                                LinesMade = 0;
                            }
                        }
                        else if (Console.KeyAvailable)
                        {
                            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                break;
                        }
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<T>(IEnumerable<T> List, Color ListKeyColor, Color ListValueColor)
        {
            WriteList(List, ListKeyColor, ListValueColor, Flags.WrapListOutputs);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<T>(IEnumerable<T> List, Color ListKeyColor, Color ListValueColor, bool Wrap)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Variables
                    var LinesMade = default(int);
                    int OldTop;
                    int EntryNumber = 1;

                    // Try to write list to console
                    OldTop = ConsoleWrapper.CursorTop;
                    foreach (T ListEntry in List)
                    {
                        var Values = new List<object>();
                        if (ListEntry as IEnumerable is not null & ListEntry as string is null)
                        {
                            foreach (var Value in (IEnumerable)ListEntry)
                                Values.Add(Value);
                            TextWriterColor.Write("- {0}: ", false, ListKeyColor, EntryNumber);
                            TextWriterColor.Write("{0}", true, ListValueColor, string.Join(", ", Values));
                        }
                        else
                        {
                            TextWriterColor.Write("- {0}: ", false, ListKeyColor, EntryNumber);
                            TextWriterColor.Write("{0}", true, ListValueColor, ListEntry);
                        }
                        EntryNumber += 1;
                        if (Wrap)
                        {
                            LinesMade += ConsoleWrapper.CursorTop - OldTop;
                            OldTop = ConsoleWrapper.CursorTop;
                            if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                            {
                                if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                    break;
                                LinesMade = 0;
                            }
                        }
                        else if (Console.KeyAvailable)
                        {
                            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                break;
                        }
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }
        #endregion
    }
}