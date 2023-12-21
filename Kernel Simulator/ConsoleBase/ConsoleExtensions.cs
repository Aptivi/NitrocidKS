using Terminaux.Base;

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

using TermConsoleExtensions = Terminaux.Base.ConsoleExtensions;

namespace KS.ConsoleBase
{
	public static class ConsoleExtensions
	{

		/// <summary>
		/// Clears the console buffer, but keeps the current cursor position
		/// </summary>
		public static void ClearKeepPosition()
		{
			TermConsoleExtensions.ClearKeepPosition();
		}

		/// <summary>
		/// Clears the line to the right
		/// </summary>
		public static void ClearLineToRight()
		{
			TermConsoleExtensions.ClearLineToRight();
		}

		/// <summary>
		/// Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
		/// </summary>
		/// <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
		/// <param name="MaximumNumber">The maximum number.</param>
		/// <param name="WidthOffset">The console window width offset. It's usually a multiple of 2.</param>
		/// <returns>How many times to repeat the character</returns>
		public static int PercentRepeat(int CurrentNumber, int MaximumNumber, int WidthOffset)
		{
			return TermConsoleExtensions.PercentRepeat(CurrentNumber, MaximumNumber, WidthOffset);
		}

		/// <summary>
		/// Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
		/// </summary>
		/// <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
		/// <param name="MaximumNumber">The maximum number.</param>
		/// <param name="TargetWidth">The target width</param>
		/// <returns>How many times to repeat the character</returns>
		public static int PercentRepeatTargeted(int CurrentNumber, int MaximumNumber, int TargetWidth)
		{
			return TermConsoleExtensions.PercentRepeatTargeted(CurrentNumber, MaximumNumber, TargetWidth);
		}

		/// <summary>
		/// Filters the VT sequences that matches the regex
		/// </summary>
		/// <param name="Text">The text that contains the VT sequences</param>
		/// <returns>The text that doesn't contain the VT sequences</returns>
		public static string FilterVTSequences(string Text)
		{
			return TermConsoleExtensions.FilterVTSequences(Text);
		}

		/// <summary>
		/// Get the filtered cursor positions (by filtered means filtered from the VT escape sequences that matches the regex in the routine)
		/// </summary>
		/// <param name="Text">The text that contains the VT sequences</param>
		/// <param name="Left">The filtered left position</param>
		/// <param name="Top">The filtered top position</param>
		public static void GetFilteredPositions(string Text, ref int Left, ref int Top, params object[] Vars)
		{
			var pos = TermConsoleExtensions.GetFilteredPositions(Text, false, Vars);
			Left = pos.Item1;
			Top = pos.Item2;
		}

		/// <summary>
		/// Polls $TERM_PROGRAM to get terminal emulator
		/// </summary>
		public static string GetTerminalEmulator()
		{
			return ConsolePlatform.GetTerminalEmulator();
		}

		/// <summary>
		/// Polls $TERM to get terminal type (vt100, dumb, ...)
		/// </summary>
		public static string GetTerminalType()
		{
			return ConsolePlatform.GetTerminalType();
		}

		public static void SetTitle(string Text)
		{
			TermConsoleExtensions.SetTitle(Text);
		}

	}
}