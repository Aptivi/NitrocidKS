﻿
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

Class TextEdit_QueryWordCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String) Implements ICommand.Execute
        If IsNumeric(ListArgs(1)) Then
            If CInt(ListArgs(1)) <= TextEdit_FileLines.Count Then
                Dim QueriedChars As Dictionary(Of Integer, String) = TextEdit_QueryWord(ListArgs(0), ListArgs(1))
                For Each WordIndex As Integer In QueriedChars.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, WordIndex)
                    W("{0} ({1})", True, ColTypes.ListValue, ListArgs(0), TextEdit_FileLines(ListArgs(1)))
                Next
            Else
                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
            End If
        ElseIf ListArgs(1).ToLower = "all" Then
            Dim QueriedWords As Dictionary(Of Integer, Dictionary(Of Integer, String)) = TextEdit_QueryWord(ListArgs(0))
            For Each LineIndex As Integer In QueriedWords.Keys
                For Each WordIndex As Integer In QueriedWords(LineIndex).Keys
                    W("- {0}:{1}: ", False, ColTypes.ListEntry, LineIndex, WordIndex)
                    W("{0} ({1})", True, ColTypes.ListValue, ListArgs(0), TextEdit_FileLines(LineIndex))
                Next
            Next
        End If
    End Sub

End Class