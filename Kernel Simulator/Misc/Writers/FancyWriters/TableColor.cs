using System;
using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using Terminaux.Colors;
using TermTable = Terminaux.Writer.FancyWriters.TableColor;

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

using Terminaux.Writer.FancyWriters.Tools;

namespace KS.Misc.Writers.FancyWriters
{
	public static class TableColor
	{

		/// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Safe threshold from left</param>
		public static void WriteTable(string[] Headers, string[,] Rows, int Margin, bool SeparateRows = true, List<CellOptions> CellOptions = null)
		{
			TermTable.WriteTable(Headers, Rows, Margin, SeparateRows, CellOptions);
		}

		/// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Color">A color that will be changed to.</param>
		public static void WriteTable(string[] Headers, string[,] Rows, int Margin, ConsoleColor Color, bool SeparateRows = true, List<CellOptions> CellOptions = null)
		{
			TermTable.WriteTable(Headers, Rows, Margin, (Color)Color, (Color)Color, (Color)Color, KernelColorTools.BackgroundColor, SeparateRows, CellOptions);
		}

		/// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
		public static void WriteTable(string[] Headers, string[,] Rows, int Margin, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor, bool SeparateRows = true, List<CellOptions> CellOptions = null)
		{
			TermTable.WriteTable(Headers, Rows, Margin, (Color)ForegroundColor, (Color)ForegroundColor, (Color)ForegroundColor, (Color)BackgroundColor, SeparateRows, CellOptions);
		}

		/// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Safe threshold from left</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
		public static void WriteTable(string[] Headers, string[,] Rows, int Margin, KernelColorTools.ColTypes ColTypes, bool SeparateRows = true, List<CellOptions> CellOptions = null)
		{
			TermTable.WriteTable(Headers, Rows, Margin, KernelColorTools.GetConsoleColor(ColTypes), KernelColorTools.GetConsoleColor(ColTypes), KernelColorTools.GetConsoleColor(ColTypes), KernelColorTools.BackgroundColor, SeparateRows, CellOptions);
		}

		/// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Safe threshold from left</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
		public static void WriteTable(string[] Headers, string[,] Rows, int Margin, KernelColorTools.ColTypes colorTypeForeground, KernelColorTools.ColTypes colorTypeBackground, bool SeparateRows = true, List<CellOptions> CellOptions = null)
		{
			TermTable.WriteTable(Headers, Rows, Margin, KernelColorTools.GetConsoleColor(colorTypeForeground), KernelColorTools.GetConsoleColor(colorTypeForeground), KernelColorTools.GetConsoleColor(colorTypeForeground), KernelColorTools.GetConsoleColor(colorTypeBackground), SeparateRows, CellOptions);
		}

		/// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Color">A color that will be changed to.</param>
		public static void WriteTable(string[] Headers, string[,] Rows, int Margin, Color Color, bool SeparateRows = true, List<CellOptions> CellOptions = null)
		{
			TermTable.WriteTable(Headers, Rows, Margin, Color, Color, Color, KernelColorTools.BackgroundColor, SeparateRows, CellOptions);
		}

		/// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
		public static void WriteTable(string[] Headers, string[,] Rows, int Margin, Color ForegroundColor, Color BackgroundColor, bool SeparateRows = true, List<CellOptions> CellOptions = null)
		{
			TermTable.WriteTable(Headers, Rows, Margin, ForegroundColor, ForegroundColor, ForegroundColor, BackgroundColor, SeparateRows, CellOptions);
		}

	}
}