using System;
using TermInput = Terminaux.Inputs.Input;


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

using Terminaux.Reader;

namespace KS.ConsoleBase.Inputs
{
	public static class Input
	{

		internal static TermReaderSettings GlobalSettings = new();

		/// <summary>
		/// Current mask character
		/// </summary>
		public static string CurrentMask = Convert.ToString('*');

		/// <summary>
		/// Reads the line from the console
		/// </summary>
		public static string ReadLine()
		{
			return ReadLine("", "", true);
		}

		/// <summary>
		/// Reads the line from the console
		/// </summary>
		/// <param name="UseCtrlCAsInput">Whether to treat CTRL + C as input</param>
		public static string ReadLine(bool UseCtrlCAsInput)
		{
			return ReadLine("", "", UseCtrlCAsInput);
		}

		/// <summary>
		/// Reads the line from the console
		/// </summary>
		/// <param name="InputText">Input text to write</param>
		/// <param name="DefaultValue">Default value</param>
		public static string ReadLine(string InputText, string DefaultValue)
		{
			return ReadLine(InputText, DefaultValue, true);
		}

		/// <summary>
		/// Reads the line from the console
		/// </summary>
		/// <param name="InputText">Input text to write</param>
		/// <param name="DefaultValue">Default value</param>
		/// <param name="UseCtrlCAsInput">Whether to treat CTRL + C as input</param>
		public static string ReadLine(string InputText, string DefaultValue, bool UseCtrlCAsInput)
		{
			return TermInput.ReadLine(InputText, DefaultValue, new TermReaderSettings() { TreatCtrlCAsInput = UseCtrlCAsInput });
		}

		/// <summary>
		/// Reads the next line of characters from the standard input stream without showing input being written by user.
		/// </summary>
		public static string ReadLineNoInput()
		{
			if (!string.IsNullOrEmpty(CurrentMask))
			{
				return ReadLineNoInput(CurrentMask[0]);
			}
			else
			{
				return ReadLineNoInput('\0');
			}
		}

		/// <summary>
		/// Reads the next line of characters from the standard input stream without showing input being written by user.
		/// </summary>
		/// <param name="MaskChar">Specifies the password mask character</param>
		public static string ReadLineNoInput(char MaskChar)
		{
			return TermInput.ReadLineNoInput(MaskChar);
		}

		/// <summary>
		/// Reads the next key from the console input stream with the timeout
		/// </summary>
		/// <param name="Intercept"></param>
		/// <param name="Timeout"></param>
		public static ConsoleKeyInfo ReadKeyTimeout(bool Intercept, TimeSpan Timeout)
		{
			return TermInput.ReadKeyTimeout(Intercept, Timeout);
		}

		/// <summary>
		/// Detects the keypress
		/// </summary>
		public static ConsoleKeyInfo DetectKeypress()
		{
			return TermInput.DetectKeypress();
		}

	}
}
