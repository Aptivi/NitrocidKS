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
Imports Extensification.IntegerExts
Imports KS.Misc.Editors.TextEdit

Namespace Shell.Shells.Text.Commands
    ''' <summary>
    ''' Queries a word in a specified line or all lines
    ''' </summary>
    ''' <remarks>
    ''' You can use this command to query a word and get its number from the specified line or all lines.
    ''' </remarks>
    Class TextEdit_QueryWordCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgsOnly.Length = 2 Then
                If IsStringNumeric(ListArgsOnly(1)) Then
                    If CInt(ListArgsOnly(1)) <= TextEdit_FileLines.Count Then
                        Dim QueriedChars As Dictionary(Of Integer, String) = TextEdit_QueryWord(ListArgsOnly(0), ListArgsOnly(1))
                        For Each WordIndex As Integer In QueriedChars.Keys
                            Write("- {0}: ", False, ColTypes.ListEntry, WordIndex)
                            Write("{0} ({1})", True, ColTypes.ListValue, ListArgsOnly(0), TextEdit_FileLines(ListArgsOnly(1)))
                        Next
                    Else
                        Write(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                    End If
                ElseIf ListArgsOnly(1).ToLower = "all" Then
                    Dim QueriedWords As Dictionary(Of Integer, Dictionary(Of Integer, String)) = TextEdit_QueryWord(ListArgsOnly(0))
                    For Each LineIndex As Integer In QueriedWords.Keys
                        For Each WordIndex As Integer In QueriedWords(LineIndex).Keys
                            Write("- {0}:{1}: ", False, ColTypes.ListEntry, LineIndex, WordIndex)
                            Write("{0} ({1})", True, ColTypes.ListValue, ListArgsOnly(0), TextEdit_FileLines(LineIndex))
                        Next
                    Next
                End If
            ElseIf ListArgsOnly.Length > 2 Then
                If IsStringNumeric(ListArgsOnly(1)) And IsStringNumeric(ListArgsOnly(2)) Then
                    If CInt(ListArgsOnly(1)) <= TextEdit_FileLines.Count And CInt(ListArgsOnly(2)) <= TextEdit_FileLines.Count Then
                        Dim LineNumberStart As Integer = ListArgsOnly(1)
                        Dim LineNumberEnd As Integer = ListArgsOnly(2)
                        LineNumberStart.SwapIfSourceLarger(LineNumberEnd)
                        For LineNumber = LineNumberStart To LineNumberEnd
                            Dim QueriedChars As Dictionary(Of Integer, String) = TextEdit_QueryWord(ListArgsOnly(0), LineNumber)
                            For Each WordIndex As Integer In QueriedChars.Keys
                                Write("- {0}:{1}: ", False, ColTypes.ListEntry, LineNumber, WordIndex)
                                Write("{0} ({1})", True, ColTypes.ListValue, ListArgsOnly(0), TextEdit_FileLines(ListArgsOnly(1)))
                            Next
                        Next
                    Else
                        Write(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                    End If
                End If
            End If
        End Sub

    End Class
End Namespace
