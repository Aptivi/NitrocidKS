
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

Imports KS.Network.FTP.Transfer

Namespace Network.FTP.Commands
    Class FTP_GetFolderCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim RemoteFolder As String = ListArgs(0)
            Dim LocalFolder As String = If(ListArgs.Count > 1, ListArgs(1), "")
            Write(DoTranslation("Downloading folder {0}..."), True, color:=GetConsoleColor(ColTypes.Progress), RemoteFolder)
            Dim Result As Boolean = If(Not String.IsNullOrWhiteSpace(LocalFolder), FTPGetFolder(RemoteFolder, LocalFolder), FTPGetFolder(RemoteFolder))
            If Result Then
                WritePlain("", True)
                Write(DoTranslation("Downloaded folder {0}."), True, color:=GetConsoleColor(ColTypes.Success), RemoteFolder)
            Else
                WritePlain("", True)
                Write(DoTranslation("Download failed for folder {0}."), True, color:=GetConsoleColor(ColTypes.Error), RemoteFolder)
            End If
        End Sub

    End Class
End Namespace
