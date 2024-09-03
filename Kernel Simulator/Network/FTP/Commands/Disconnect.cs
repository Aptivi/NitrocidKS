﻿//
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

using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
namespace KS.Network.FTP.Commands
{
    class FTP_DisconnectCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (FTPShellCommon.FtpConnected == true)
            {
                // Set a connected flag to False
                FTPShellCommon.FtpConnected = false;
                FTPShellCommon.ClientFTP.Config.DisconnectWithQuit = ListSwitchesOnly.Contains("-f");
                FTPShellCommon.ClientFTP.Disconnect();
                TextWriters.Write(Translate.DoTranslation("Disconnected from {0}"), true, KernelColorTools.ColTypes.Success, FTPShellCommon.FtpSite);

                // Clean up everything
                FTPShellCommon.FtpSite = "";
                FTPShellCommon.FtpCurrentRemoteDir = "";
                FTPShellCommon.FtpUser = "";
                FTPShellCommon.FtpPass = "";
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("You haven't connected to any server yet"), true, KernelColorTools.ColTypes.Error);
            }
        }

        public override void HelpHelper()
        {
            TextWriters.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, KernelColorTools.ColTypes.Neutral);
            TextWriters.Write("  -f: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Disconnects from server disgracefully"), true, KernelColorTools.ColTypes.ListValue);
        }

    }
}