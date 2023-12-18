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

Imports System.Text
Imports KS.Network.HTTP

Namespace Shell.Prompts.Presets.HTTP
    Public Class HTTPDefaultPreset
        Inherits PromptPresetBase
        Implements IPromptPreset

        Public Overrides ReadOnly Property PresetName As String = "Default" Implements IPromptPreset.PresetName

        Public Overrides ReadOnly Property PresetPrompt As String Implements IPromptPreset.PresetPrompt
            Get
                Return PresetPromptBuilder()
            End Get
        End Property

        Public Overrides ReadOnly Property PresetShellType As ShellType = ShellType.HTTPShell Implements IPromptPreset.PresetShellType

        Friend Overrides Function PresetPromptBuilder() As String Implements IPromptPreset.PresetPromptBuilder
            'Build the preset
            Dim PresetStringBuilder As New StringBuilder

            If HTTPConnected Then
                'Opening
                PresetStringBuilder.Append(GetGray().VTSequenceForeground)
                PresetStringBuilder.Append("[")

                'HTTP site
                PresetStringBuilder.Append(HostNameShellColor.VTSequenceForeground)
                PresetStringBuilder.AppendFormat("{0}", HTTPSite)

                'Closing
                PresetStringBuilder.Append(GetGray().VTSequenceForeground)
                PresetStringBuilder.Append("] > ")
            Else
                'Closing
                PresetStringBuilder.Append(GetGray().VTSequenceForeground)
                PresetStringBuilder.Append("> ")
            End If

            'Present final string
            Return PresetStringBuilder.ToString()
        End Function

    End Class
End Namespace