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

Imports KS.Misc.Reflection
Imports KS.Misc.Editors.HexEdit

Namespace Shell.Shells.Hex.Commands
    ''' <summary>
    ''' Removes the specified byte number
    ''' </summary>
    ''' <remarks>
    ''' You can use this command to remove a specified byte by number. You can use the print command to take a look at the unneeded byte and its number.
    ''' </remarks>
    Class HexEdit_DelByteCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If IsStringNumeric(ListArgs(0)) Then
                If CInt(ListArgs(0)) <= HexEdit_FileBytes.LongLength Then
                    HexEdit_DeleteByte(ListArgs(0))
                    Write(DoTranslation("Byte deleted."), True, ColTypes.Success)
                Else
                    Write(DoTranslation("The specified byte number may not be larger than the file size."), True, ColTypes.Error)
                End If
            Else
                Write(DoTranslation("The byte number is not numeric."), True, ColTypes.Error)
                Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs(0))
            End If
        End Sub

    End Class
End Namespace
