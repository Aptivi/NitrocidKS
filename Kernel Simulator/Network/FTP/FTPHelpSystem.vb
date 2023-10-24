
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Public Module FTPHelpSystem

    'This dictionary is the definitions for commands.
    Public FTPDefinitions As Dictionary(Of String, String)
    Public FTPModDefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Updates the help definition so it reflects the available commands
    ''' </summary>
    Public Sub InitFTPHelp()
        FTPDefinitions = New Dictionary(Of String, String) From {{"connect", DoTranslation("Connects to an FTP server (it must start with ""ftp://"" or ""ftps://"")")},
                                                                 {"cdl", DoTranslation("Changes local directory to download to or upload from")},
                                                                 {"cdr", DoTranslation("Changes remote directory to download from or upload to")},
                                                                 {"cp", DoTranslation("Copies file or directory to another file or directory.")},
                                                                 {"del", DoTranslation("Deletes remote file from server")},
                                                                 {"disconnect", DoTranslation("Disconnects from server")},
                                                                 {"get", DoTranslation("Downloads remote file to local directory using binary or text")},
                                                                 {"exit", DoTranslation("Exits FTP shell and returns to kernel")},
                                                                 {"help", DoTranslation("Shows help screen")},
                                                                 {"lsl", DoTranslation("Lists local directory")},
                                                                 {"lsr", DoTranslation("Lists remote directory")},
                                                                 {"mv", DoTranslation("Moves file or directory to another file or directory. You can also use that to rename files.")},
                                                                 {"perm", DoTranslation("Sets file permissions. This is supported only on FTP servers that run Unix.")},
                                                                 {"quickconnect", DoTranslation("Uses information from Speed Dial to connect to any network quickly")},
                                                                 {"put", DoTranslation("Uploads local file to remote directory using binary or text")},
                                                                 {"pwdl", DoTranslation("Gets current local directory")},
                                                                 {"pwdr", DoTranslation("Gets current remote directory")},
                                                                 {"type", DoTranslation("Sets the type for this session")}}
    End Sub

    ''' <summary>
    ''' Shows the list of commands.
    ''' </summary>
    ''' <param name="command">Specified command</param>
    Public Sub FTPShowHelp(Optional command As String = "")

        If command = "" Then
            If simHelp = False Then
                Write(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In FTPDefinitions.Keys
                    Write("- {0}: ", False, ColTypes.ListEntry, cmd) : Write("{0}", True, ColTypes.ListValue, FTPDefinitions(cmd))
                Next
                Write(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If FTPModDefs.Count = 0 Then Write("- " + DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In FTPModDefs.Keys
                    Write("- {0}: ", False, ColTypes.ListEntry, cmd) : Write("{0}", True, ColTypes.ListValue, FTPModDefs(cmd))
                Next
                Write(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If FTPShellAliases.Count = 0 Then Write("- " + DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In FTPShellAliases.Keys
                    Write("- {0}: ", False, ColTypes.ListEntry, cmd) : Write("{0}", True, ColTypes.ListValue, FTPDefinitions(FTPShellAliases(cmd)))
                Next
            Else
                For Each cmd As String In FTPCommands.Keys
                    Write("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                For Each cmd As String In FTPModDefs.Keys
                    Write("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                Write(String.Join(", ", FTPShellAliases.Keys), True, ColTypes.ListEntry)
            End If
        ElseIf command = "pwdl" Then
            Write(DoTranslation("Usage:") + " currlocaldir or pwdl", True, ColTypes.Neutral)
        ElseIf command = "pwdr" Then
            Write(DoTranslation("Usage:") + " currremotedir or pwdr", True, ColTypes.Neutral)
        ElseIf command = "connect" Then
            Write(DoTranslation("Usage:") + " connect <server>", True, ColTypes.Neutral)
        ElseIf command = "cdl" Then
            Write(DoTranslation("Usage:") + " cdl <directory>", True, ColTypes.Neutral)
        ElseIf command = "cdr" Then
            Write(DoTranslation("Usage:") + " cdr <directory>", True, ColTypes.Neutral)
        ElseIf command = "cp" Then
            Write(DoTranslation("Usage:") + " cp <sourcefileordir> <targetfileordir>", True, ColTypes.Neutral)
        ElseIf command = "del" Then
            Write(DoTranslation("Usage:") + " del <file>", True, ColTypes.Neutral)
        ElseIf command = "disconnect" Then
            Write(DoTranslation("Usage:") + " disconnect", True, ColTypes.Neutral)
        ElseIf command = "get" Then
            Write(DoTranslation("Usage:") + " get <file>", True, ColTypes.Neutral)
        ElseIf command = "exit" Then
            Write(DoTranslation("Usage:") + " exit", True, ColTypes.Neutral)
        ElseIf command = "lsl" Then
            Write(DoTranslation("Usage:") + " lsl [dir]", True, ColTypes.Neutral)
        ElseIf command = "ldr" Then
            Write(DoTranslation("Usage:") + " lsr [dir]", True, ColTypes.Neutral)
        ElseIf command = "mv" Then
            Write(DoTranslation("Usage:") + " mv <sourcefileordir> <targetfileordir>", True, ColTypes.Neutral)
        ElseIf command = "perm" Then
            Write(DoTranslation("Usage:") + " perm <file> <permnumber>", True, ColTypes.Neutral)
        ElseIf command = "type" Then
            Write(DoTranslation("Usage:") + " type <a/b>", True, ColTypes.Neutral)
        ElseIf command = "put" Then
            Write(DoTranslation("Usage:") + " put <file>", True, ColTypes.Neutral)
        ElseIf command = "quickconnect" Then
            Write(DoTranslation("Usage:") + " quickconnect", True, ColTypes.Neutral)
        End If

    End Sub

End Module
