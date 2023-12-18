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

Imports KS.Misc.Screensaver.Customized

Namespace TestShell.Commands
    Class Test_SetCustomSaverSettingCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If CustomSavers.ContainsKey(ListArgs(0)) Then
                If SetCustomSaverSettings(ListArgs(0), ListArgs(1), ListArgs(2)) Then
                    Write(DoTranslation("Settings set successfully for screensaver") + " {0}.", True, color:=GetConsoleColor(ColTypes.Neutral), ListArgs(0))
                Else
                    Write(DoTranslation("Failed to set a setting for screensaver") + " {0}.", True, color:=GetConsoleColor(ColTypes.Error), ListArgs(0))
                End If
            Else
                Write(DoTranslation("Screensaver {0} not found."), True, color:=GetConsoleColor(ColTypes.Error), ListArgs(0))
            End If
        End Sub

    End Class
End Namespace