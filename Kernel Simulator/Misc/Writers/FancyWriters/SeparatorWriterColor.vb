
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

Imports TermSeparator = Terminaux.Writer.FancyWriters.SeparatorWriterColor

Namespace Misc.Writers.FancyWriters
    Public Module SeparatorWriterColor

        ''' <summary>
        ''' Draw a separator with text
        ''' </summary>
        ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        ''' <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        ''' <param name="Vars">Variables to format the message before it's written.</param>
        Public Sub WriteSeparator(Text As String, PrintSuffix As Boolean, ParamArray Vars() As Object)
            TermSeparator.WriteSeparator(Text, PrintSuffix, Vars)
        End Sub

        ''' <summary>
        ''' Draw a separator with text
        ''' </summary>
        ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        ''' <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        ''' <param name="ColTypes">A type of colors that will be changed.</param>
        ''' <param name="Vars">Variables to format the message before it's written.</param>
        Public Sub WriteSeparator(Text As String, PrintSuffix As Boolean, ColTypes As ColTypes, ParamArray Vars() As Object)
            TermSeparator.WriteSeparatorColor(Text, PrintSuffix, GetConsoleColor(ColTypes), Vars)
        End Sub

        ''' <summary>
        ''' Draw a separator with text
        ''' </summary>
        ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        ''' <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        ''' <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        ''' <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        ''' <param name="Vars">Variables to format the message before it's written.</param>
        Public Sub WriteSeparator(Text As String, PrintSuffix As Boolean, colorTypeForeground As ColTypes, colorTypeBackground As ColTypes, ParamArray Vars() As Object)
            TermSeparator.WriteSeparatorColorBack(Text, PrintSuffix, GetConsoleColor(colorTypeForeground), GetConsoleColor(colorTypeBackground), Vars)
        End Sub

        ''' <summary>
        ''' Draw a separator with text
        ''' </summary>
        ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        ''' <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        ''' <param name="Color">A color that will be changed to.</param>
        ''' <param name="Vars">Variables to format the message before it's written.</param>
        Public Sub WriteSeparator(Text As String, PrintSuffix As Boolean, Color As ConsoleColor, ParamArray Vars() As Object)
            TermSeparator.WriteSeparatorColor(Text, PrintSuffix, Color, Vars)
        End Sub

        ''' <summary>
        ''' Draw a separator with text
        ''' </summary>
        ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        ''' <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
        ''' <param name="BackgroundColor">A background color that will be changed to.</param>
        ''' <param name="Vars">Variables to format the message before it's written.</param>
        Public Sub WriteSeparator(Text As String, PrintSuffix As Boolean, ForegroundColor As ConsoleColor, BackgroundColor As ConsoleColor, ParamArray Vars() As Object)
            TermSeparator.WriteSeparatorColorBack(Text, PrintSuffix, ForegroundColor, BackgroundColor, Vars)
        End Sub

        ''' <summary>
        ''' Draw a separator with text
        ''' </summary>
        ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        ''' <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        ''' <param name="Color">A color that will be changed to.</param>
        ''' <param name="Vars">Variables to format the message before it's written.</param>
        Public Sub WriteSeparator(Text As String, PrintSuffix As Boolean, Color As Color, ParamArray Vars() As Object)
            TermSeparator.WriteSeparatorColor(Text, PrintSuffix, Color, Vars)
        End Sub

        ''' <summary>
        ''' Draw a separator with text
        ''' </summary>
        ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        ''' <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
        ''' <param name="BackgroundColor">A background color that will be changed to.</param>
        ''' <param name="Vars">Variables to format the message before it's written.</param>
        Public Sub WriteSeparator(Text As String, PrintSuffix As Boolean, ForegroundColor As Color, BackgroundColor As Color, ParamArray Vars() As Object)
            TermSeparator.WriteSeparatorColorBack(Text, PrintSuffix, ForegroundColor, BackgroundColor, Vars)
        End Sub

    End Module
End Namespace
