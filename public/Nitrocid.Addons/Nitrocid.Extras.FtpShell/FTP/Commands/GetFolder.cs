﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.FtpShell.Tools.Transfer;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Extras.FtpShell.FTP.Commands
{
    /// <summary>
    /// Downloads a folder from the current working directory
    /// </summary>
    /// <remarks>
    /// Downloads the binary or text folder and saves it to the current working local directory for you to use the downloaded folder that is provided in the FTP server.
    /// </remarks>
    class GetFolderCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string RemoteFolder = parameters.ArgumentsList[0];
            string LocalFolder = parameters.ArgumentsList.Length > 1 ? parameters.ArgumentsList[1] : "";
            TextWriters.Write(Translate.DoTranslation("Downloading folder {0}..."), true, KernelColorType.Progress, RemoteFolder);
            bool Result = !string.IsNullOrWhiteSpace(LocalFolder) ? FTPTransfer.FTPGetFolder(RemoteFolder, LocalFolder) : FTPTransfer.FTPGetFolder(RemoteFolder);
            if (Result)
            {
                TextWriterRaw.Write();
                TextWriters.Write(Translate.DoTranslation("Downloaded folder {0}."), true, KernelColorType.Success, RemoteFolder);
                return 0;
            }
            else
            {
                TextWriterRaw.Write();
                TextWriters.Write(Translate.DoTranslation("Download failed for folder {0}."), true, KernelColorType.Error, RemoteFolder);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.FTPNetwork);
            }
        }

    }
}
