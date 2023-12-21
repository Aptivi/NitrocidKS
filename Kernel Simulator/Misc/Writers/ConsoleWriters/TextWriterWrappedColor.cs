using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel;
using KS.Languages;

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

using KS.Misc.Reflection;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;
using Terminaux.Colors;

namespace KS.Misc.Writers.ConsoleWriters
{
	public static class TextWriterWrappedColor
	{

		/// <summary>
		/// Outputs the text into the terminal prompt, wraps the long terminal output if needed.
		/// </summary>
		/// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
		/// <param name="Line">Whether to print a new line or not</param>
		/// <param name="vars">Variables to format the message before it's written.</param>
		public static void WriteWrappedPlain(string Text, bool Line, params object[] vars)
		{
			lock (TextWriterColor.WriteLock)
			{
				var LinesMade = default(int);
				int OldTop;
				try
				{
					// Format string as needed
					if (!(vars.Length == 0))
						Text = StringManipulate.FormatString(Text, vars);

					OldTop = ConsoleWrapper.CursorTop;
					foreach (char TextChar in Text.ToString().ToCharArray())
					{
						TextWriterColor.WritePlain(Convert.ToString(TextChar), false);
						LinesMade += ConsoleWrapper.CursorTop - OldTop;
						OldTop = ConsoleWrapper.CursorTop;
						if (LinesMade == ConsoleWrapper.WindowHeight - 1)
						{
							if (Input.DetectKeypress().Key == ConsoleKey.Escape)
								break;
							LinesMade = 0;
						}
					}
					if (Line)
						TextWriterColor.WritePlain("", true);
				}
				catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
				{
					DebugWriter.WStkTrc(ex);
					KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
				}
			}
		}

		/// <summary>
		/// Outputs the text into the terminal prompt, wraps the long terminal output if needed, and sets colors as needed.
		/// </summary>
		/// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
		/// <param name="Line">Whether to print a new line or not</param>
		/// <param name="colorType">A type of colors that will be changed.</param>
		/// <param name="vars">Variables to format the message before it's written.</param>
		public static void WriteWrapped(string Text, bool Line, KernelColorTools.ColTypes colorType, params object[] vars)
		{
			lock (TextWriterColor.WriteLock)
			{
				try
				{
					// Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
					KernelColorTools.SetConsoleColor(colorType);

					// Write wrapped output
					WriteWrappedPlain(Text, Line, vars);
				}
				catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
				{
					DebugWriter.WStkTrc(ex);
					KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
				}
			}
		}

		/// <summary>
		/// Outputs the text into the terminal prompt, wraps the long terminal output if needed, and sets colors as needed.
		/// </summary>
		/// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
		/// <param name="Line">Whether to print a new line or not</param>
		/// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
		/// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
		/// <param name="vars">Variables to format the message before it's written.</param>
		public static void WriteWrapped(string Text, bool Line, KernelColorTools.ColTypes colorTypeForeground, KernelColorTools.ColTypes colorTypeBackground, params object[] vars)
		{
			lock (TextWriterColor.WriteLock)
			{
				try
				{
					// Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
					KernelColorTools.SetConsoleColor(colorTypeForeground);
					KernelColorTools.SetConsoleColor(colorTypeBackground, true);

					// Write wrapped output
					WriteWrappedPlain(Text, Line, vars);
				}
				catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
				{
					DebugWriter.WStkTrc(ex);
					KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
				}
			}
		}

		/// <summary>
		/// Outputs the text into the terminal prompt with custom color support and wraps the long terminal output if needed.
		/// </summary>
		/// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
		/// <param name="Line">Whether to print a new line or not</param>
		/// <param name="color">A color that will be changed to.</param>
		/// <param name="vars">Variables to format the message before it's written.</param>
		public static void WriteWrapped(string Text, bool Line, ConsoleColor color, params object[] vars)
		{
			lock (TextWriterColor.WriteLock)
			{
				try
				{
					// Try to write to console
					Console.BackgroundColor = (ConsoleColor)Convert.ToInt32(StringQuery.IsStringNumeric(KernelColorTools.BackgroundColor.PlainSequence) && Convert.ToDouble(KernelColorTools.BackgroundColor.PlainSequence) <= 15d ? Enum.Parse(typeof(ConsoleColor), KernelColorTools.BackgroundColor.PlainSequence) : ConsoleColor.Black);
					Console.ForegroundColor = color;

					// Write wrapped output
					WriteWrappedPlain(Text, Line, vars);
				}
				catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
				{
					DebugWriter.WStkTrc(ex);
					KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
				}
			}
		}

		/// <summary>
		/// Outputs the text into the terminal prompt with custom color support and wraps the long terminal output if needed.
		/// </summary>
		/// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
		/// <param name="Line">Whether to print a new line or not</param>
		/// <param name="ForegroundColor">A foreground color that will be changed to.</param>
		/// <param name="BackgroundColor">A background color that will be changed to.</param>
		/// <param name="vars">Variables to format the message before it's written.</param>
		public static void WriteWrapped(string Text, bool Line, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor, params object[] vars)
		{
			lock (TextWriterColor.WriteLock)
			{
				try
				{
					// Try to write to console
					Console.BackgroundColor = BackgroundColor;
					Console.ForegroundColor = ForegroundColor;

					// Write wrapped output
					WriteWrappedPlain(Text, Line, vars);
				}
				catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
				{
					DebugWriter.WStkTrc(ex);
					KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
				}
			}
		}

		/// <summary>
		/// Outputs the text into the terminal prompt with custom color support and wraps the long terminal output if needed.
		/// </summary>
		/// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
		/// <param name="Line">Whether to print a new line or not</param>
		/// <param name="color">A color that will be changed to.</param>
		/// <param name="vars">Variables to format the message before it's written.</param>
		public static void WriteWrapped(string Text, bool Line, Color color, params object[] vars)
		{
			lock (TextWriterColor.WriteLock)
			{
				try
				{
					// Try to write to console
					if (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out))
					{
						KernelColorTools.SetConsoleColor(color);
						KernelColorTools.SetConsoleColor(KernelColorTools.BackgroundColor, true);
					}

					// Write wrapped output
					WriteWrappedPlain(Text, Line, vars);
				}
				catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
				{
					DebugWriter.WStkTrc(ex);
					KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
				}
			}
		}

		/// <summary>
		/// Outputs the text into the terminal prompt with custom color support and wraps the long terminal output if needed.
		/// </summary>
		/// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
		/// <param name="Line">Whether to print a new line or not</param>
		/// <param name="ForegroundColor">A foreground color that will be changed to.</param>
		/// <param name="BackgroundColor">A background color that will be changed to.</param>
		/// <param name="vars">Variables to format the message before it's written.</param>
		public static void WriteWrapped(string Text, bool Line, Color ForegroundColor, Color BackgroundColor, params object[] vars)
		{
			lock (TextWriterColor.WriteLock)
			{
				try
				{
					// Try to write to console
					if (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out))
					{
						KernelColorTools.SetConsoleColor(ForegroundColor);
						KernelColorTools.SetConsoleColor(BackgroundColor, true);
					}

					// Write wrapped output
					WriteWrappedPlain(Text, Line, vars);
				}
				catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
				{
					DebugWriter.WStkTrc(ex);
					KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
				}
			}
		}

	}
}