
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

Public Module SFTPHelpSystem

    'This dictionary is the definitions for commands.
    Public SFTPDefinitions As Dictionary(Of String, String)
    Public SFTPModDefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Updates the help definition so it reflects the available commands
    ''' </summary>
    Public Sub InitSFTPHelp()
        SFTPDefinitions = New Dictionary(Of String, String) From {{"pwdl", DoTranslation("Gets current local directory")},
                                                                  {"pwdr", DoTranslation("Gets current remote directory")},
                                                                  {"connect", DoTranslation("Connects to an SFTP server (it must start with ""sftp://"")")},
                                                                  {"cdl", DoTranslation("Changes local directory to download to or upload from")},
                                                                  {"cdr", DoTranslation("Changes remote directory to download from or upload to")},
                                                                  {"del", DoTranslation("Deletes remote file from server")},
                                                                  {"disconnect", DoTranslation("Disconnects from server")},
                                                                  {"get", DoTranslation("Downloads remote file to local directory using binary or text")},
                                                                  {"exit", DoTranslation("Exits SFTP shell and returns to kernel")},
                                                                  {"help", DoTranslation("Shows help screen")},
                                                                  {"lsl", DoTranslation("Lists local directory")},
                                                                  {"lsr", DoTranslation("Lists remote directory")},
                                                                  {"quickconnect", DoTranslation("Uses information from Speed Dial to connect to any network quickly")},
                                                                  {"put", DoTranslation("Uploads local file to remote directory using binary or text")}}
    End Sub

    ''' <summary>
    ''' Shows the list of commands.
    ''' </summary>
    ''' <param name="command">Specified command</param>
    Public Sub SFTPShowHelp(Optional ByVal command As String = "")

        If command = "" Then
            If simHelp = False Then
                Write(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In SFTPDefinitions.Keys
                    Write("- {0}: ", False, ColTypes.ListEntry, cmd) : Write("{0}", True, ColTypes.ListValue, SFTPDefinitions(cmd))
                Next
                Write(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If SFTPModDefs.Count = 0 Then Write("- " + DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In SFTPModDefs.Keys
                    Write("- {0}: ", False, ColTypes.ListEntry, cmd) : Write("{0}", True, ColTypes.ListValue, SFTPModDefs(cmd))
                Next
                Write(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If SFTPShellAliases.Count = 0 Then Write("- " + DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In SFTPShellAliases.Keys
                    Write("- {0}: ", False, ColTypes.ListEntry, cmd) : Write("{0}", True, ColTypes.ListValue, SFTPDefinitions(SFTPShellAliases(cmd)))
                Next
            Else
                For Each cmd As String In SFTPCommands.Keys
                    Write("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                For Each cmd As String In SFTPModDefs.Keys
                    Write("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                Write(String.Join(", ", SFTPShellAliases.Keys), True, ColTypes.ListEntry)
            End If
        ElseIf command = "pwdl" Then
            Write(DoTranslation("Usage:") + " pwdl", True, ColTypes.Neutral)
        ElseIf command = "pwdr" Then
            Write(DoTranslation("Usage:") + " pwdr", True, ColTypes.Neutral)
        ElseIf command = "connect" Then
            Write(DoTranslation("Usage:") + " connect <server>", True, ColTypes.Neutral)
        ElseIf command = "cdl" Then
            Write(DoTranslation("Usage:") + " cdl <directory>", True, ColTypes.Neutral)
        ElseIf command = "cdr" Then
            Write(DoTranslation("Usage:") + " cdr <directory>", True, ColTypes.Neutral)
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
        ElseIf command = "put" Then
            Write(DoTranslation("Usage:") + " put <file>", True, ColTypes.Neutral)
        ElseIf command = "quickconnect" Then
            Write(DoTranslation("Usage:") + " quickconnect", True, ColTypes.Neutral)
        End If

    End Sub

End Module
