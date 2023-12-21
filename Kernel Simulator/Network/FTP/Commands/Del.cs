using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;

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

using KS.Network.FTP.Filesystem;
using KS.Shell.ShellBase.Commands;

namespace KS.Network.FTP.Commands
{
	class FTP_DelCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			if (FTPShellCommon.FtpConnected == true)
			{
				// Print a message
				TextWriterColor.Write(Translate.DoTranslation("Deleting {0}..."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Progress), ListArgs[0]);

				// Make a confirmation message so user will not accidentally delete a file or folder
				TextWriterColor.Write(Translate.DoTranslation("Are you sure you want to delete {0} <y/n>?") + " ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input), ListArgs[0]);
				_ = Convert.ToString(Input.DetectKeypress().KeyChar);
				TextWriterColor.WritePlain("", true);

				try
				{
					FTPFilesystem.FTPDeleteRemote(ListArgs[0]);
				}
				catch (Exception ex)
				{
					TextWriterColor.Write(ex.Message, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
				}
			}
			else
			{
				TextWriterColor.Write(Translate.DoTranslation("You must connect to server with administrative privileges before performing the deletion."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
			}
		}

	}
}