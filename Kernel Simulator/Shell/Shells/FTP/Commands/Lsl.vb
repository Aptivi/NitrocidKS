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

Imports KS.Files.Folders

Namespace Shell.Shells.FTP.Commands
    ''' <summary>
    ''' Lists the contents of the current folder or the folder provided
    ''' </summary>
    ''' <remarks>
    ''' You can see the list of the files and sub-directories contained in the current working directory if no directories are specified, or in the specified directory, if specified.
    ''' <br></br>
    ''' You can also see the list of the files and sub-directories contained in the previous directory of your current position.
    ''' <br></br>
    ''' <list type="table">
    ''' <listheader>
    ''' <term>Switches</term>
    ''' <description>Description</description>
    ''' </listheader>
    ''' <item>
    ''' <term>-showdetails</term>
    ''' <description>Shows the details of the files and folders</description>
    ''' </item>
    ''' <item>
    ''' <term>-suppressmessages</term>
    ''' <description>Suppresses the "unauthorized" messages</description>
    ''' </item>
    ''' </list>
    ''' <br></br>
    ''' </remarks>
    Class FTP_LslCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim ShowFileDetails As Boolean = ListSwitchesOnly.Contains("-showdetails") OrElse ShowFileDetailsList
            Dim SuppressUnauthorizedMessage As Boolean = ListSwitchesOnly.Contains("-suppressmessages") OrElse SuppressUnauthorizedMessages
            If ListArgsOnly?.Length = 0 Or ListArgsOnly Is Nothing Then
                List(FtpCurrentDirectory, ShowFileDetails, SuppressUnauthorizedMessage)
            Else
                For Each Directory As String In ListArgsOnly
                    Dim direct As String = NeutralizePath(Directory)
                    List(direct, ShowFileDetails, SuppressUnauthorizedMessage)
                Next
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            Write("  -showdetails: ", False, ColTypes.ListEntry) : Write(DoTranslation("Shows the file details in the list"), True, ColTypes.ListValue)
            Write("  -suppressmessages: ", False, ColTypes.ListEntry) : Write(DoTranslation("Suppresses the annoying ""permission denied"" messages"), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace
