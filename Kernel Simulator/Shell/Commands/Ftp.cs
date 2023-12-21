using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

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
	class FtpCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			try
			{
				if ((((ListArgs?.Length) is { } arg1 ? arg1 == 0 : null) | ListArgs is null) == true)
				{
					ShellStart.StartShell(ShellType.FTPShell);
				}
				else
				{
					ShellStart.StartShell(ShellType.FTPShell, ListArgs[0]);
				}
			}
			catch (Kernel.Exceptions.FTPShellException ftpex)
			{
				TextWriterColor.Write(ftpex.Message, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
			}
			catch (Exception ex)
			{
				DebugWriter.WStkTrc(ex);
				TextWriterColor.Write(Translate.DoTranslation("Unknown FTP shell error:") + " {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.Message);
			}
		}

	}
}