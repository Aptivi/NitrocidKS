
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

Namespace Shell.Commands
    Class ColorHexToRgbCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Hex As String = ListArgsOnly(0)

            'Do the job
            Dim rgb As String() = ConvertFromHexToRGB(Hex).Split(";")
            Write("- " + DoTranslation("Red color level:") + " ", False, ColTypes.ListEntry)
            Write($"{rgb(0)}", True, ColTypes.ListValue)
            Write("- " + DoTranslation("Green color level:") + " ", False, ColTypes.ListEntry)
            Write($"{rgb(1)}", True, ColTypes.ListValue)
            Write("- " + DoTranslation("Blue color level:") + " ", False, ColTypes.ListEntry)
            Write($"{rgb(2)}", True, ColTypes.ListValue)
        End Sub

        Private Shared Function ConvertFromHexToRGB(Hex As String) As String
            If Hex.StartsWith("#") Then
                Dim ColorDecimal As Integer = Convert.ToInt32(Hex.Substring(1), 16)
                Dim R As Integer = CByte((ColorDecimal And &HFF0000) >> &H10)
                Dim G As Integer = CByte((ColorDecimal And &HFF00) >> 8)
                Dim B As Integer = CByte(ColorDecimal And &HFF)
                Wdbg(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B)
                Return $"{R};{G};{B}"
            Else
                Throw New Exception(DoTranslation("Invalid hex color specifier."))
            End If
        End Function

    End Class
End Namespace
