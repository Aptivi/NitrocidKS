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

Imports System.IO

Class ChAttrCommand
    Inherits CommandExecutor
    Implements ICommand

    'Warning: Don't use ListSwitchesOnly to replace ListArgs(1); the removal signs of ChAttr are treated as switches and will cause unexpected behavior if changed.
    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
        Dim NeutralizedFilePath As String = NeutralizePath(ListArgs(0))
        If FileExists(NeutralizedFilePath) Then
            If ListArgs(1).EndsWith("Normal") Or ListArgs(1).EndsWith("ReadOnly") Or ListArgs(1).EndsWith("Hidden") Or ListArgs(1).EndsWith("Archive") Then
                If ListArgs(1).StartsWith("+") Then
                    Dim Attrib As FileAttributes = [Enum].Parse(GetType(FileAttributes), ListArgs(1).Remove(0, 1))
                    If AddAttributeToFile(NeutralizedFilePath, Attrib) Then
                        W(DoTranslation("Attribute has been added successfully."), True, ColTypes.Neutral, ListArgs(1))
                    Else
                        W(DoTranslation("Failed to add attribute."), True, ColTypes.Neutral, ListArgs(1))
                    End If
                ElseIf ListArgs(1).StartsWith("-") Then
                    Dim Attrib As FileAttributes = [Enum].Parse(GetType(FileAttributes), ListArgs(1).Remove(0, 1))
                    If RemoveAttributeFromFile(NeutralizedFilePath, Attrib) Then
                        W(DoTranslation("Attribute has been removed successfully."), True, ColTypes.Neutral, ListArgs(1))
                    Else
                        W(DoTranslation("Failed to remove attribute."), True, ColTypes.Neutral, ListArgs(1))
                    End If
                End If
            Else
                W(DoTranslation("Attribute ""{0}"" is invalid."), True, ColTypes.Error, ListArgs(1))
            End If
        Else
            W(DoTranslation("File not found."), True, ColTypes.Error)
        End If
    End Sub

    Public Sub HelpHelper()
        W(DoTranslation("where <attributes> is one of the following:") + vbNewLine, True, ColTypes.Neutral)
        W("- Normal: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is a normal file"), True, ColTypes.ListValue)                   'Normal   = 128
        W("- ReadOnly: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is a read-only file"), True, ColTypes.ListValue)              'ReadOnly = 1
        W("- Hidden: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is a hidden file"), True, ColTypes.ListValue)                   'Hidden   = 2
        W("- Archive: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is an archive. Used for backups."), True, ColTypes.ListValue)  'Archive  = 32
    End Sub

End Class