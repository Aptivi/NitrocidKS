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

Module BeatFaderDisplay

    Public WithEvents BeatFader As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of FaderBack
    ''' </summary>
    Sub BeatFader_DoWork(sender As Object, e As DoWorkEventArgs) Handles BeatFader.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Try
            Do While True
                If BeatFader.CancellationPending = True Then
                    Wdbg(DebugLevel.W, "Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    SetInputColor()
                    LoadBack()
                    Console.CursorVisible = True
                    Wdbg(DebugLevel.I, "All clean. Beat Fader screensaver stopped.")
                    SaverAutoReset.Set()
                    Exit Do
                Else
                    Dim BeatInterval As Integer = 60000 / BeatFaderDelay
                    Dim BeatIntervalStep As Integer = BeatInterval / BeatFaderMaxSteps
                    WdbgConditional(ScreensaverDebug, "I", "Beat interval from {0} BPM: {1}", BeatFaderDelay, BeatInterval)
                    WdbgConditional(ScreensaverDebug, "I", "Beat steps: {0} ms", BeatFaderDelay, BeatIntervalStep)
                    SleepNoBlock(BeatIntervalStep, BeatFader)

                    'If we're cycling colors, set them. Else, use the user-provided color
                    Dim RedColorNum, GreenColorNum, BlueColorNum As Integer
                    Dim SelectedColorType As ColorType = ColorType._255Color
                    If BeatFaderCycleColors Then
                        'We're cycling. Select the color mode, starting from true color
                        WdbgConditional(ScreensaverDebug, "I", "Cycling colors...")
                        If BeatFaderTrueColor Then
                            RedColorNum = RandomDriver.Next(255)
                            GreenColorNum = RandomDriver.Next(255)
                            BlueColorNum = RandomDriver.Next(255)
                            SelectedColorType = ColorType.TrueColor
                        ElseIf BeatFader255Colors Then
                            Dim ConsoleColor As New ConsoleColorsInfo(RandomDriver.Next(255))
                            RedColorNum = ConsoleColor.R
                            GreenColorNum = ConsoleColor.G
                            BlueColorNum = ConsoleColor.B
                        Else
                            Dim ConsoleColor As New ConsoleColorsInfo(RandomDriver.Next(15))
                            RedColorNum = ConsoleColor.R
                            GreenColorNum = ConsoleColor.G
                            BlueColorNum = ConsoleColor.B
                        End If
                        WdbgConditional(ScreensaverDebug, "I", "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                    Else
                        'We're not cycling. Parse the color and then select the color mode, starting from true color
                        WdbgConditional(ScreensaverDebug, "I", "Parsing colors... {0}", BeatFaderBeatColor)
                        Dim UserColor As New Color(BeatFaderBeatColor)
                        If UserColor.Type = ColorType.TrueColor Then
                            RedColorNum = UserColor.R
                            GreenColorNum = UserColor.G
                            BlueColorNum = UserColor.B
                            SelectedColorType = ColorType.TrueColor
                        ElseIf UserColor.Type = ColorType._255Color Then
                            Dim ConsoleColor As New ConsoleColorsInfo(UserColor.PlainSequence)
                            RedColorNum = ConsoleColor.R
                            GreenColorNum = ConsoleColor.G
                            BlueColorNum = ConsoleColor.B
                        End If
                        WdbgConditional(ScreensaverDebug, "I", "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                    End If

                    'Set thresholds
                    Dim ThresholdRed As Double = RedColorNum / BeatFaderMaxSteps
                    Dim ThresholdGreen As Double = GreenColorNum / BeatFaderMaxSteps
                    Dim ThresholdBlue As Double = BlueColorNum / BeatFaderMaxSteps
                    WdbgConditional(ScreensaverDebug, "I", "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue)

                    'Fade out
                    For CurrentStep As Integer = 1 To BeatFaderMaxSteps
                        If BeatFader.CancellationPending Then Exit For
                        WdbgConditional(ScreensaverDebug, "I", "Step {0}/{1} each {2} ms", CurrentStep, BeatFaderMaxSteps, BeatIntervalStep)
                        SleepNoBlock(BeatIntervalStep, FaderBack)
                        Dim CurrentColorRedOut As Integer = RedColorNum - ThresholdRed * CurrentStep
                        Dim CurrentColorGreenOut As Integer = GreenColorNum - ThresholdGreen * CurrentStep
                        Dim CurrentColorBlueOut As Integer = BlueColorNum - ThresholdBlue * CurrentStep
                        WdbgConditional(ScreensaverDebug, "I", "Color out (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        SetConsoleColor(New Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}"), True)
                        Console.Clear()
                    Next
                End If
            Loop
        Catch ex As Exception
            Wdbg(DebugLevel.W, "Screensaver experienced an error: {0}. Cleaning everything up...", ex.Message)
            WStkTrc(ex)
            e.Cancel = True
            SetInputColor()
            LoadBack()
            Console.CursorVisible = True
            Wdbg(DebugLevel.I, "All clean. Beat Fader screensaver stopped.")
            W(DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), True, ColTypes.Error, ex.Message)
            SaverAutoReset.Set()
        End Try
    End Sub

End Module
