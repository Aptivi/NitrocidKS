using System.Linq;
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

using KS.Network.FTP.Transfer;
using KS.Shell.ShellBase.Commands;

namespace KS.Network.FTP.Commands
{
	class FTP_PutCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			string LocalFile = ListArgs[0];
			string RemoteFile = ListArgs.Count() > 1 ? ListArgs[1] : "";
			TextWriterColor.Write(Translate.DoTranslation("Uploading file {0}..."), false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Progress), ListArgs[0]);
			bool Result = !string.IsNullOrWhiteSpace(LocalFile) ? FTPTransfer.FTPUploadFile(RemoteFile, LocalFile) : FTPTransfer.FTPUploadFile(RemoteFile);
			if (Result)
			{
				TextWriterColor.WritePlain("", true);
				TextWriterColor.Write(Translate.DoTranslation("Uploaded file {0}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Success), LocalFile);
			}
			else
			{
				TextWriterColor.WritePlain("", true);
				TextWriterColor.Write(Translate.DoTranslation("Failed to upload {0}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), LocalFile);
			}
		}

	}
}