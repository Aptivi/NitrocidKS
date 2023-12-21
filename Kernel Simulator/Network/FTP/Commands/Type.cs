using FluentFTP;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
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

namespace KS.Network.FTP.Commands
{
	class FTP_TypeCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			if (ListArgs[0].ToLower() == "a")
			{
				FTPShellCommon.ClientFTP.Config.DownloadDataType = FtpDataType.ASCII;
				FTPShellCommon.ClientFTP.Config.ListingDataType = FtpDataType.ASCII;
				FTPShellCommon.ClientFTP.Config.UploadDataType = FtpDataType.ASCII;
				TextWriterColor.Write(Translate.DoTranslation("Data type set to ASCII!"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Success));
				TextWriterColor.Write(Translate.DoTranslation("Beware that most files won't download or upload properly using this mode, so we highly recommend using the Binary mode on most situations."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Warning));
			}
			else if (ListArgs[0].ToLower() == "b")
			{
				FTPShellCommon.ClientFTP.Config.DownloadDataType = FtpDataType.Binary;
				FTPShellCommon.ClientFTP.Config.ListingDataType = FtpDataType.Binary;
				FTPShellCommon.ClientFTP.Config.UploadDataType = FtpDataType.Binary;
				TextWriterColor.Write(Translate.DoTranslation("Data type set to Binary!"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Success));
			}
			else
			{
				TextWriterColor.Write(Translate.DoTranslation("Invalid data type."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
			}
		}

	}
}