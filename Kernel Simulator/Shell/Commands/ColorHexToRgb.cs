using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;

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

namespace KS.Shell.Commands
{
	class ColorHexToRgbCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			string Hex = ListArgsOnly[0];

			// Do the job
			string[] rgb = ConvertFromHexToRGB(Hex).Split(';');
			TextWriterColor.Write("- " + Translate.DoTranslation("Red color level:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
			TextWriterColor.Write($"{rgb[0]}", true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
			TextWriterColor.Write("- " + Translate.DoTranslation("Green color level:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
			TextWriterColor.Write($"{rgb[1]}", true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
			TextWriterColor.Write("- " + Translate.DoTranslation("Blue color level:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
			TextWriterColor.Write($"{rgb[2]}", true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
		}

		private static string ConvertFromHexToRGB(string Hex)
		{
			if (Hex.StartsWith("#"))
			{
				int ColorDecimal = Convert.ToInt32(Hex.Substring(1), 16);
				int R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
				int G = (byte)((ColorDecimal & 0xFF00) >> 8);
				int B = (byte)(ColorDecimal & 0xFF);
				DebugWriter.Wdbg(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
				return $"{R};{G};{B}";
			}
			else
			{
				throw new Exception(Translate.DoTranslation("Invalid hex color specifier."));
			}
		}

	}
}