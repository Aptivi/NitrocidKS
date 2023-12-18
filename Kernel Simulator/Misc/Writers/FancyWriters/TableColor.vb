
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

Imports Terminaux.Writer.FancyWriters.Tools
Imports TermTable = Terminaux.Writer.FancyWriters.TableColor

Namespace Misc.Writers.FancyWriters
    Public Module TableColor

        ''' <summary>
        ''' Draw a table with text
        ''' </summary>
        ''' <param name="Headers">Headers to insert to the table.</param>
        ''' <param name="Rows">Rows to insert to the table.</param>
        ''' <param name="Margin">Safe threshold from left</param>
        Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer, Optional SeparateRows As Boolean = True, Optional CellOptions As List(Of CellOptions) = Nothing)
            TermTable.WriteTable(Headers, Rows, Margin, SeparateRows, CellOptions)
        End Sub

        ''' <summary>
        ''' Draw a table with text
        ''' </summary>
        ''' <param name="Color">A color that will be changed to.</param>
        Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer, Color As ConsoleColor, Optional SeparateRows As Boolean = True, Optional CellOptions As List(Of CellOptions) = Nothing)
            TermTable.WriteTable(Headers, Rows, Margin, Color, Color, Color, BackgroundColor, SeparateRows, CellOptions)
        End Sub

        ''' <summary>
        ''' Draw a table with text
        ''' </summary>
        ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
        ''' <param name="BackgroundColor">A background color that will be changed to.</param>
        Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer, ForegroundColor As ConsoleColor, BackgroundColor As ConsoleColor, Optional SeparateRows As Boolean = True, Optional CellOptions As List(Of CellOptions) = Nothing)
            TermTable.WriteTable(Headers, Rows, Margin, ForegroundColor, ForegroundColor, ForegroundColor, BackgroundColor, SeparateRows, CellOptions)
        End Sub

        ''' <summary>
        ''' Draw a table with text
        ''' </summary>
        ''' <param name="Headers">Headers to insert to the table.</param>
        ''' <param name="Rows">Rows to insert to the table.</param>
        ''' <param name="Margin">Safe threshold from left</param>
        ''' <param name="ColTypes">A type of colors that will be changed.</param>
        Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer, ColTypes As ColTypes, Optional SeparateRows As Boolean = True, Optional CellOptions As List(Of CellOptions) = Nothing)
            TermTable.WriteTable(Headers, Rows, Margin, GetConsoleColor(ColTypes), GetConsoleColor(ColTypes), GetConsoleColor(ColTypes), BackgroundColor, SeparateRows, CellOptions)
        End Sub

        ''' <summary>
        ''' Draw a table with text
        ''' </summary>
        ''' <param name="Headers">Headers to insert to the table.</param>
        ''' <param name="Rows">Rows to insert to the table.</param>
        ''' <param name="Margin">Safe threshold from left</param>
        ''' <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        ''' <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer, colorTypeForeground As ColTypes, colorTypeBackground As ColTypes, Optional SeparateRows As Boolean = True, Optional CellOptions As List(Of CellOptions) = Nothing)
            TermTable.WriteTable(Headers, Rows, Margin, GetConsoleColor(colorTypeForeground), GetConsoleColor(colorTypeForeground), GetConsoleColor(colorTypeForeground), GetConsoleColor(colorTypeBackground), SeparateRows, CellOptions)
        End Sub

        ''' <summary>
        ''' Draw a table with text
        ''' </summary>
        ''' <param name="Color">A color that will be changed to.</param>
        Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer, Color As Color, Optional SeparateRows As Boolean = True, Optional CellOptions As List(Of CellOptions) = Nothing)
            TermTable.WriteTable(Headers, Rows, Margin, Color, Color, Color, BackgroundColor, SeparateRows, CellOptions)
        End Sub

        ''' <summary>
        ''' Draw a table with text
        ''' </summary>
        ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
        ''' <param name="BackgroundColor">A background color that will be changed to.</param>
        Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer, ForegroundColor As Color, BackgroundColor As Color, Optional SeparateRows As Boolean = True, Optional CellOptions As List(Of CellOptions) = Nothing)
            TermTable.WriteTable(Headers, Rows, Margin, ForegroundColor, ForegroundColor, ForegroundColor, BackgroundColor, SeparateRows, CellOptions)
        End Sub

    End Module
End Namespace
