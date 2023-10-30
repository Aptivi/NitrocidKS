﻿//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using FluentFTP;
using KS.Network.Base.Connections;
using KS.Network.Base.SpeedDial;
using KS.Network.FTP;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using System;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can interact with the File Transfer Protocol (FTP) shell to connect to a server and transfer files
    /// </summary>
    /// <remarks>
    /// You can use the FTP shell to connect to your FTP server or the public FTP servers to interact with the files found in the server.
    /// <br></br>
    /// You can download files to your computer, upload files to the server, manage files by renaming, deleting, etc., and so on.
    /// </remarks>
    class FtpCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            NetworkConnectionTools.OpenConnectionForShell(ShellType.FTPShell, FTPTools.TryToConnect, EstablishFtpConnection, parameters.ArgumentsText);
            return 0;
        }

        private NetworkConnection EstablishFtpConnection(string address, SpeedDialEntry connection)
        {
            var encMode = Enum.Parse<FtpEncryptionMode>(connection.Options[1].ToString());
            return FTPTools.PromptForPassword(null, connection.Options[0].ToString(), address, connection.Port, connection.Options.Length > 1 ? encMode : FtpEncryptionMode.None);
        }
    }
}
