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

Imports KS.Network.RPC

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Remote execution
    ''' </summary>
    ''' <remarks>
    ''' This command can be used to remotely execute KS commands.
    ''' <br></br>
    ''' The user must have at least the administrative privileges before they can run the below commands.
    ''' </remarks>
    Class RexecCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgsOnly.Length = 2 Then
                SendCommand("<Request:Exec>(" + ListArgsOnly(1) + ")", ListArgsOnly(0))
            Else
                SendCommand("<Request:Exec>(" + ListArgsOnly(2) + ")", ListArgsOnly(0), ListArgsOnly(1))
            End If
        End Sub

    End Class
End Namespace
