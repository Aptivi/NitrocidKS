
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Imports Terminaux.Colors.Selector

Namespace ConsoleBase
    Public Module ColorWheelOpen

        ''' <summary>
        ''' Initializes color wheel
        ''' </summary>
        Public Function ColorWheel() As String
            Return ColorWheel(False, ConsoleColors.White, 0, 0, 0)
        End Function

        ''' <summary>
        ''' Initializes color wheel
        ''' </summary>
        ''' <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
        Public Function ColorWheel(TrueColor As Boolean) As String
            Return ColorWheel(TrueColor, ConsoleColors.White, 0, 0, 0)
        End Function

        ''' <summary>
        ''' Initializes color wheel
        ''' </summary>
        ''' <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
        ''' <param name="DefaultColor">The default 255-color to use</param>
        Public Function ColorWheel(TrueColor As Boolean, DefaultColor As ConsoleColors) As String
            Return ColorWheel(TrueColor, DefaultColor, 0, 0, 0)
        End Function

        ''' <summary>
        ''' Initializes color wheel
        ''' </summary>
        ''' <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
        ''' <param name="DefaultColorR">The default red color range of 0-255 to use</param>
        ''' <param name="DefaultColorG">The default green color range of 0-255 to use</param>
        ''' <param name="DefaultColorB">The default blue color range of 0-255 to use</param>
        Public Function ColorWheel(TrueColor As Boolean, DefaultColorR As Integer, DefaultColorG As Integer, DefaultColorB As Integer) As String
            Return ColorWheel(TrueColor, ConsoleColors.White, DefaultColorR, DefaultColorG, DefaultColorB)
        End Function

        ''' <summary>
        ''' Initializes color wheel
        ''' </summary>
        ''' <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
        ''' <param name="DefaultColor">The default 255-color to use</param>
        ''' <param name="DefaultColorR">The default red color range of 0-255 to use</param>
        ''' <param name="DefaultColorG">The default green color range of 0-255 to use</param>
        ''' <param name="DefaultColorB">The default blue color range of 0-255 to use</param>
        Public Function ColorWheel(TrueColor As Boolean, DefaultColor As ConsoleColors, DefaultColorR As Integer, DefaultColorG As Integer, DefaultColorB As Integer) As String
            If TrueColor Then
                Return ColorSelector.OpenColorSelector($"{DefaultColorR};{DefaultColorG};{DefaultColorB}").PlainSequence
            Else
                Return ColorSelector.OpenColorSelector(DefaultColor).PlainSequence
            End If
        End Function

    End Module
End Namespace