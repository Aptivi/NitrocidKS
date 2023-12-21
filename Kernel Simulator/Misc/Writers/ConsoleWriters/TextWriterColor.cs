using System;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Colors;

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

using TermWriter = Terminaux.Writer.ConsoleWriters.TextWriterColor;

namespace KS.Misc.Writers.ConsoleWriters
{
	public static class TextWriterColor
	{

		internal static object WriteLock = new();

		/// <summary>
		/// Outputs the text into the terminal prompt without colors
		/// </summary>
		/// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
		/// <param name="Line">Whether to print a new line or not</param>
		/// <param name="vars">Variables to format the message before it's written.</param>
		public static void WritePlain(string Text, bool Line, params object[] vars)
		{
			TermWriter.WritePlain(Text, Line, vars);
		}

		/// <summary>
		/// Outputs the text into the terminal prompt, and sets colors as needed.
		/// </summary>
		/// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
		/// <param name="Line">Whether to print a new line or not</param>
		/// <param name="colorType">A type of colors that will be changed.</param>
		/// <param name="vars">Variables to format the message before it's written.</param>
		public static void Write(string Text, bool Line, KernelColorTools.ColTypes colorType, params object[] vars)
		{
			lock (WriteLock)
			{
				try
				{
					// Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
					KernelColorTools.SetConsoleColor(colorType);

					// Write the text to console
					WritePlain(Text, Line, vars);
				}
				catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
				{
					DebugWriter.WStkTrc(ex);
					KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
				}
			}
		}

		/// <summary>
		/// Outputs the text into the terminal prompt, and sets colors as needed.
		/// </summary>
		/// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
		/// <param name="Line">Whether to print a new line or not</param>
		/// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
		/// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
		/// <param name="vars">Variables to format the message before it's written.</param>
		public static void Write(string Text, bool Line, KernelColorTools.ColTypes colorTypeForeground, KernelColorTools.ColTypes colorTypeBackground, params object[] vars)
		{
			lock (WriteLock)
			{
				try
				{
					// Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
					KernelColorTools.SetConsoleColor(colorTypeForeground);
					KernelColorTools.SetConsoleColor(colorTypeBackground, true);

					// Write the text to console
					WritePlain(Text, Line, vars);
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
		/// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
		/// <param name="Line">Whether to print a new line or not</param>
		/// <param name="color">A color that will be changed to.</param>
		/// <param name="vars">Variables to format the message before it's written.</param>
		public static void Write(string Text, bool Line, ConsoleColor color, params object[] vars)
		{
			lock (WriteLock)
			{
				try
				{
					// Try to write to console
					var result = ConsoleColor.Black;
					Console.BackgroundColor = Enum.TryParse(KernelColorTools.BackgroundColor.PlainSequence, out result) ? result : ConsoleColor.Black;
					Console.ForegroundColor = color;

					// Write the text to console
					WritePlain(Text, Line, vars);
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
		/// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
		/// <param name="Line">Whether to print a new line or not</param>
		/// <param name="ForegroundColor">A foreground color that will be changed to.</param>
		/// <param name="BackgroundColor">A background color that will be changed to.</param>
		/// <param name="vars">Variables to format the message before it's written.</param>
		public static void Write(string Text, bool Line, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor, params object[] vars)
		{
			lock (WriteLock)
			{
				try
				{
					// Try to write to console
					Console.BackgroundColor = BackgroundColor;
					Console.ForegroundColor = ForegroundColor;

					// Write the text to console
					WritePlain(Text, Line, vars);
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
		/// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
		/// <param name="Line">Whether to print a new line or not</param>
		/// <param name="color">A color that will be changed to.</param>
		/// <param name="vars">Variables to format the message before it's written.</param>
		public static void Write(string Text, bool Line, Color color, params object[] vars)
		{
			lock (WriteLock)
			{
				try
				{
					// Try to write to console
					if (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out))
					{
						KernelColorTools.SetConsoleColor(color);
						KernelColorTools.SetConsoleColor(KernelColorTools.BackgroundColor, true);
					}

					// Write the text to console
					WritePlain(Text, Line, vars);
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
		/// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
		/// <param name="Line">Whether to print a new line or not</param>
		/// <param name="ForegroundColor">A foreground color that will be changed to.</param>
		/// <param name="BackgroundColor">A background color that will be changed to.</param>
		/// <param name="vars">Variables to format the message before it's written.</param>
		public static void Write(string Text, bool Line, Color ForegroundColor, Color BackgroundColor, params object[] vars)
		{
			lock (WriteLock)
			{
				try
				{
					// Try to write to console
					if (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out))
					{
						KernelColorTools.SetConsoleColor(ForegroundColor);
						KernelColorTools.SetConsoleColor(BackgroundColor, true);
					}

					// Write the text to console
					WritePlain(Text, Line, vars);
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