using KS.ConsoleBase.Colors;
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

using KS.Network.FTP;
using KS.Shell.ShellBase.Commands;

namespace KS.Network.SFTP.Commands
{
	class SFTP_DisconnectCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			if (SFTPShellCommon.SFTPConnected)
			{
				// Set a connected flag to False
				SFTPShellCommon.SFTPConnected = false;
				SFTPShellCommon.ClientSFTP.Disconnect();
				TextWriterColor.Write(Translate.DoTranslation("Disconnected from {0}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), FTPShellCommon.FtpSite);

				// Clean up everything
				SFTPShellCommon.SFTPSite = "";
				SFTPShellCommon.SFTPCurrentRemoteDir = "";
				SFTPShellCommon.SFTPUser = "";
				SFTPShellCommon.SFTPPass = "";
			}
			else
			{
				TextWriterColor.Write(Translate.DoTranslation("You haven't connected to any server yet"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
			}
		}

	}
}