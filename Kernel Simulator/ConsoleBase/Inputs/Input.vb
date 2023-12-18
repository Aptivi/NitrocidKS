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

Imports Terminaux.Reader
Imports TermInput = Terminaux.Inputs.Input

Namespace ConsoleBase.Inputs
    Public Module Input

        Friend GlobalSettings As New TermReaderSettings

        ''' <summary>
        ''' Current mask character
        ''' </summary>
        Public CurrentMask As String = "*"c

        ''' <summary>
        ''' Reads the line from the console
        ''' </summary>
        Public Function ReadLine() As String
            Return ReadLine("", "", True)
        End Function

        ''' <summary>
        ''' Reads the line from the console
        ''' </summary>
        ''' <param name="UseCtrlCAsInput">Whether to treat CTRL + C as input</param>
        Public Function ReadLine(UseCtrlCAsInput As Boolean) As String
            Return ReadLine("", "", UseCtrlCAsInput)
        End Function

        ''' <summary>
        ''' Reads the line from the console
        ''' </summary>
        ''' <param name="InputText">Input text to write</param>
        ''' <param name="DefaultValue">Default value</param>
        Public Function ReadLine(InputText As String, DefaultValue As String) As String
            Return ReadLine(InputText, DefaultValue, True)
        End Function

        ''' <summary>
        ''' Reads the line from the console
        ''' </summary>
        ''' <param name="InputText">Input text to write</param>
        ''' <param name="DefaultValue">Default value</param>
        ''' <param name="UseCtrlCAsInput">Whether to treat CTRL + C as input</param>
        Public Function ReadLine(InputText As String, DefaultValue As String, UseCtrlCAsInput As Boolean) As String
            Return TermInput.ReadLine(InputText, DefaultValue, New TermReaderSettings() With {.TreatCtrlCAsInput = UseCtrlCAsInput})
        End Function

        ''' <summary>
        ''' Reads the next line of characters from the standard input stream without showing input being written by user.
        ''' </summary>
        Public Function ReadLineNoInput() As String
            If Not String.IsNullOrEmpty(CurrentMask) Then
                Return ReadLineNoInput(CurrentMask(0))
            Else
                Return ReadLineNoInput("")
            End If
        End Function

        ''' <summary>
        ''' Reads the next line of characters from the standard input stream without showing input being written by user.
        ''' </summary>
        ''' <param name="MaskChar">Specifies the password mask character</param>
        Public Function ReadLineNoInput(MaskChar As Char) As String
            Return TermInput.ReadLineNoInput(MaskChar)
        End Function

        ''' <summary>
        ''' Reads the next key from the console input stream with the timeout
        ''' </summary>
        ''' <param name="Intercept"></param>
        ''' <param name="Timeout"></param>
        Public Function ReadKeyTimeout(Intercept As Boolean, Timeout As TimeSpan) As ConsoleKeyInfo
            Return TermInput.ReadKeyTimeout(Intercept, Timeout)
        End Function

        ''' <summary>
        ''' Detects the keypress
        ''' </summary>
        Public Function DetectKeypress() As ConsoleKeyInfo
            Return TermInput.DetectKeypress()
        End Function

    End Module
End Namespace