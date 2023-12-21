using System;

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

using System.IO;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Encryption;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
	class VerifyCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			try
			{
				string HashFile = Filesystem.NeutralizePath(ListArgs[2]);
				if (Checking.FileExists(HashFile))
				{
					if (HashVerifier.VerifyHashFromHashesFile(ListArgs[3], (Encryption.Algorithms)Convert.ToInt32(Enum.Parse(typeof(Encryption.Algorithms), ListArgs[0])), ListArgs[2], ListArgs[1]))
					{
						TextWriterColor.Write(Translate.DoTranslation("Hashes match."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
					}
					else
					{
						TextWriterColor.Write(Translate.DoTranslation("Hashes don't match."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Warning));
					}
				}
				else if (HashVerifier.VerifyHashFromHash(ListArgs[3], (Encryption.Algorithms)Convert.ToInt32(Enum.Parse(typeof(Encryption.Algorithms), ListArgs[0])), ListArgs[2], ListArgs[1]))
				{
					TextWriterColor.Write(Translate.DoTranslation("Hashes match."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
				}
				else
				{
					TextWriterColor.Write(Translate.DoTranslation("Hashes don't match."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Warning));
				}
			}
			catch (Kernel.Exceptions.InvalidHashAlgorithmException ihae)
			{
				DebugWriter.WStkTrc(ihae);
				TextWriterColor.Write(Translate.DoTranslation("Invalid encryption algorithm."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
			}
			catch (Kernel.Exceptions.InvalidHashException ihe)
			{
				DebugWriter.WStkTrc(ihe);
				TextWriterColor.Write(Translate.DoTranslation("Hashes are malformed."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
			}
			catch (FileNotFoundException fnfe)
			{
				DebugWriter.WStkTrc(fnfe);
				TextWriterColor.Write(Translate.DoTranslation("{0} is not found."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ListArgs[3]);
			}
		}

	}
}