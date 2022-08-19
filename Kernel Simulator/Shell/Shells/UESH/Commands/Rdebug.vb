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

Imports KS.Network.RemoteDebug

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Enables remote debug
    ''' </summary>
    ''' <remarks>
    ''' If the kernel is on the debugging mode, you can use this command to turn on/off the functionality. If the remote debug is on, it will turn it off, and it will do inverse.
    ''' <br></br>
    ''' The remote debug will listen on a port that is unused as specified in the kernel settings, Debug Port.
    ''' <br></br>
    ''' The user must have at least the administrative privileges before they can run the below commands.
    ''' </remarks>
    Class RdebugCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If DebugMode Then
                If RDebugThread.IsAlive Then
                    StopRDebugThread()
                Else
                    StartRDebugThread()
                End If
            Else
                Write(DoTranslation("Debugging not enabled."), True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace
