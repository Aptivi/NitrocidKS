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

Imports System.ComponentModel

Module LinesDisplay

    Public WithEvents Lines As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of Lines
    ''' </summary>
    Sub Lines_DoWork(sender As Object, e As DoWorkEventArgs) Handles Lines.DoWork
        Console.CursorVisible = False
        Dim random As New Random()
        Wdbg("I", "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Try
            Do While True
                SleepNoBlock(LinesDelay, Lines)
                If Lines.CancellationPending = True Then
                    Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    LoadBack()
                    Console.CursorVisible = True
                    Wdbg("I", "All clean. Lines screensaver stopped.")
                    SaverAutoReset.Set()
                    Exit Do
                Else
                    Dim esc As Char = GetEsc()
                    If LinesTrueColor Then
                        Console.BackgroundColor = ConsoleColor.Black
                        Console.Clear()
                        Dim RedColorNum As Integer = random.Next(255)
                        Dim GreenColorNum As Integer = random.Next(255)
                        Dim BlueColorNum As Integer = random.Next(255)
                        Dim ColorStorage As New RGB(RedColorNum, GreenColorNum, BlueColorNum)
                        Console.Write(esc + "[38;2;" + ColorStorage.ToString + "m")
                    ElseIf Lines255Colors Then
                        Console.BackgroundColor = ConsoleColor.Black
                        Console.Clear()
                        Dim color As Integer = random.Next(255)
                        Console.Write(esc + "[38;5;" + CStr(color) + "m")
                    Else
                        Console.Clear()
                        Console.BackgroundColor = ConsoleColor.Black
                        Console.ForegroundColor = colors(random.Next(colors.Length - 1))
                    End If
                    Dim Line As String = ""
                    Dim Top As Integer = New Random().Next(Console.WindowHeight)
                    For i As Integer = 1 To Console.WindowWidth
                        Line += "-"
                    Next
                    Console.SetCursorPosition(0, Top)
                    Console.WriteLine(Line)
                End If
            Loop
        Catch ex As Exception
            Wdbg("W", "Screensaver experienced an error: {0}. Cleaning everything up...", ex.Message)
            WStkTrc(ex)
            e.Cancel = True
            LoadBack()
            Console.CursorVisible = True
            Wdbg("I", "All clean. Lines screensaver stopped.")
            Write(DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), True, ColTypes.Error, ex.Message)
            SaverAutoReset.Set()
        End Try
    End Sub

End Module
