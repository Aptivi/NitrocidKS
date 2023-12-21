//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
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
    class FTP_InfoCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (FTPShellCommon.FtpConnected)
            {
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("FTP server information"), true);
                TextWriterColor.Write(Translate.DoTranslation("Server address:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(FTPShellCommon.ClientFTP.Host, false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(Translate.DoTranslation("Server port:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(FTPShellCommon.ClientFTP.Port.ToString(), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(Translate.DoTranslation("Server type:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(FTPShellCommon.ClientFTP.ServerType.ToString(), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(Translate.DoTranslation("Server system type:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(FTPShellCommon.ClientFTP.SystemType, false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(Translate.DoTranslation("Server system:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(FTPShellCommon.ClientFTP.ServerOS.ToString(), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(Translate.DoTranslation("Server encryption mode:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(FTPShellCommon.ClientFTP.Config.EncryptionMode.ToString(), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(Translate.DoTranslation("Server data connection type:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(FTPShellCommon.ClientFTP.Config.DataConnectionType.ToString(), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(Translate.DoTranslation("Server download data type:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(FTPShellCommon.ClientFTP.Config.DownloadDataType.ToString(), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(Translate.DoTranslation("Server upload data type:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(FTPShellCommon.ClientFTP.Config.UploadDataType.ToString(), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("You haven't connected to any server yet"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
            }
        }

    }
}