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

Imports KS.Files.Querying
Imports System.IO

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Changes attributes of file
    ''' </summary>
    ''' <remarks>
    ''' You can use this command to change attributes of a file.
    ''' <br></br>
    ''' <list type="table">
    ''' <listheader>
    ''' <term>Attribute</term>
    ''' <description>Description</description>
    ''' </listheader>
    ''' <item>
    ''' <term>Normal</term>
    ''' <description>The file is a normal file</description>
    ''' </item>
    ''' <item>
    ''' <term>ReadOnly</term>
    ''' <description>The file is a read-only file</description>
    ''' </item>
    ''' <item>
    ''' <term>Hidden</term>
    ''' <description>The file is a hidden file</description>
    ''' </item>
    ''' <item>
    ''' <term>Archive</term>
    ''' <description>The file is an archive. Used for backups.</description>
    ''' </item>
    ''' </list>
    ''' <br></br>
    ''' </remarks>
    Class ChAttrCommand
        Inherits CommandExecutor
        Implements ICommand

        'Warning: Don't use ListSwitchesOnly to replace ListArgsOnly(1); the removal signs of ChAttr are treated as switches and will cause unexpected behavior if changed.
        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim NeutralizedFilePath As String = NeutralizePath(ListArgsOnly(0))
            If FileExists(NeutralizedFilePath) Then
                If ListArgsOnly(1).EndsWith("Normal") Or ListArgsOnly(1).EndsWith("ReadOnly") Or ListArgsOnly(1).EndsWith("Hidden") Or ListArgsOnly(1).EndsWith("Archive") Then
                    If ListArgsOnly(1).StartsWith("+") Then
                        Dim Attrib As FileAttributes = [Enum].Parse(GetType(FileAttributes), ListArgsOnly(1).Remove(0, 1))
                        If TryAddAttributeToFile(NeutralizedFilePath, Attrib) Then
                            Write(DoTranslation("Attribute has been added successfully."), True, ColTypes.Neutral, ListArgsOnly(1))
                        Else
                            Write(DoTranslation("Failed to add attribute."), True, ColTypes.Neutral, ListArgsOnly(1))
                        End If
                    ElseIf ListArgsOnly(1).StartsWith("-") Then
                        Dim Attrib As FileAttributes = [Enum].Parse(GetType(FileAttributes), ListArgsOnly(1).Remove(0, 1))
                        If TryRemoveAttributeFromFile(NeutralizedFilePath, Attrib) Then
                            Write(DoTranslation("Attribute has been removed successfully."), True, ColTypes.Neutral, ListArgsOnly(1))
                        Else
                            Write(DoTranslation("Failed to remove attribute."), True, ColTypes.Neutral, ListArgsOnly(1))
                        End If
                    End If
                Else
                    Write(DoTranslation("Attribute ""{0}"" is invalid."), True, ColTypes.Error, ListArgsOnly(1))
                End If
            Else
                Write(DoTranslation("File not found."), True, ColTypes.Error)
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("where <attributes> is one of the following:"), True, ColTypes.Neutral)
            Write("- Normal: ", False, ColTypes.ListEntry) : Write(DoTranslation("The file is a normal file"), True, ColTypes.ListValue)                   'Normal   = 128
            Write("- ReadOnly: ", False, ColTypes.ListEntry) : Write(DoTranslation("The file is a read-only file"), True, ColTypes.ListValue)              'ReadOnly = 1
            Write("- Hidden: ", False, ColTypes.ListEntry) : Write(DoTranslation("The file is a hidden file"), True, ColTypes.ListValue)                   'Hidden   = 2
            Write("- Archive: ", False, ColTypes.ListEntry) : Write(DoTranslation("The file is an archive. Used for backups."), True, ColTypes.ListValue)  'Archive  = 32
        End Sub

    End Class
End Namespace
