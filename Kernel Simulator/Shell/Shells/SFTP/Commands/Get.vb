﻿
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports KS.Network.SFTP.Transfer

Namespace Shell.Shells.SFTP.Commands
    ''' <summary>
    ''' Downloads a file from the current working directory
    ''' </summary>
    ''' <remarks>
    ''' Downloads the binary or text file and saves it to the current working local directory for you to use the downloaded file that is provided in the SFTP server.
    ''' </remarks>
    Class SFTP_GetCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Write(DoTranslation("Downloading file {0}..."), False, ColTypes.Progress, ListArgsOnly(0))
            If SFTPGetFile(ListArgsOnly(0)) Then
                Console.WriteLine()
                Write(DoTranslation("Downloaded file {0}."), True, ColTypes.Success, ListArgsOnly(0))
            Else
                Console.WriteLine()
                Write(DoTranslation("Download failed for file {0}."), True, ColTypes.Error, ListArgsOnly(0))
            End If
        End Sub

    End Class
End Namespace
