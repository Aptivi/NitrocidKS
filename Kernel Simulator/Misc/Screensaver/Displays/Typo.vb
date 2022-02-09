﻿
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

Imports Microsoft.VisualBasic.VBMath
Imports System.ComponentModel

Namespace Misc.Screensaver.Displays
    Module TypoDisplay

        Public WithEvents Typo As New NamedBackgroundWorker("Typo screensaver thread") With {.WorkerSupportsCancellation = True}

        Sub Typo_DoWork(sender As Object, e As DoWorkEventArgs) Handles Typo.DoWork
            'Variables
            Dim RandomDriver As New Random()
            Dim CpmSpeedMin As Integer = TypoWritingSpeedMin * 5
            Dim CpmSpeedMax As Integer = TypoWritingSpeedMax * 5
            Dim Strikes As New List(Of String) From {"q`12wsa", "r43edfgt5", "u76yhjki8", "p09ol;'[-=]\", "/';. ", "m,lkjn ", "vbhgfc ", "zxdsa "}
            Dim CapStrikes As New List(Of String) From {"Q~!@WSA", "R$#EDFGT%", "U&^YHJKI*", "P)(OL:""{_+}|", "?"":> ", "M<LKJN ", "VBHGFC ", "ZXDSA "}
            Dim CapSymbols As String = "~!@$#%&^*)(:""{_+}|?><"
            Dim CurrentWindowWidth As Integer = Console.WindowWidth
            Dim CurrentWindowHeight As Integer = Console.WindowHeight
            Dim ResizeSyncing As Boolean

            'Preparations
            SetConsoleColor(New Color(TypoTextColor))
            Console.Clear()

            'Screensaver logic
            Do While True
                Console.CursorVisible = False
                If Typo.CancellationPending = True Then
                    HandleSaverCancel()
                    Exit Do
                Else
                    'Prepare display (make a paragraph indentation)
                    Console.WriteLine()
                    Console.Write("    ")
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", Console.CursorLeft, Console.CursorTop)

                    'Get struck character and write it
                    For Each StruckChar As Char In TypoWrite
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Typo.CancellationPending Then Exit For
                        If ResizeSyncing Then Exit For

                        'Calculate needed milliseconds from two WPM speeds (minimum and maximum)
                        Dim SelectedCpm As Integer = RandomDriver.Next(CpmSpeedMin, CpmSpeedMax)
                        Dim WriteMs As Integer = (60 / SelectedCpm) * 1000
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Delay for {0} CPM: {1} ms", SelectedCpm, WriteMs)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Struck character: {0}", StruckChar)

                        'See if the typo is guaranteed
                        Dim Probability As Double = If(TypoMissStrikePossibility >= 80, 80, TypoMissStrikePossibility) / 100
                        Dim TypoGuaranteed As Boolean = Rnd() < Probability
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Probability: {0} Guarantee: {1}", Probability, TypoGuaranteed)
                        If TypoGuaranteed Then
                            'Sometimes, a typo is generated by missing a character.
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Made a typo!")
                            Dim MissProbability As Double = If(TypoMissPossibility >= 10, 10, TypoMissPossibility) / 100
                            Dim MissGuaranteed As Boolean = Rnd() < MissProbability
                            If MissGuaranteed Then
                                'Miss is guaranteed. Simulate the missed character
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Missed a character!")
                                StruckChar = ""
                            Else
                                'Typo is guaranteed. Select a strike string randomly until the struck key is found in between the characters
                                Dim StrikeCharsIndex As Integer
                                Dim StruckFound As Boolean = False
                                Dim CappedStrike As Boolean = False
                                Dim StrikesString As String = ""
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Bruteforcing...")
                                Do Until StruckFound
                                    StrikeCharsIndex = RandomDriver.Next(0, Strikes.Count - 1)
                                    CappedStrike = Char.IsUpper(StruckChar) Or CapSymbols.Contains(StruckChar)
                                    StrikesString = If(CappedStrike, CapStrikes(StrikeCharsIndex), Strikes(StrikeCharsIndex))
                                    StruckFound = Not String.IsNullOrEmpty(StrikesString) AndAlso StrikesString.Contains(StruckChar)
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Strike chars index: {0}", StrikeCharsIndex)
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Capped strike: {0}", CappedStrike)
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Strikes pattern: {0}", StrikesString)
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Found? {0}", StruckFound)
                                Loop
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Found!")

                                'Select a random character that is a typo from the selected strike index
                                Dim RandomStrikeIndex As Integer = RandomDriver.Next(0, StrikesString.Length - 1)
                                Dim MistypedChar As Char = StrikesString(RandomStrikeIndex)
                                If "`-=\][';/.,".Contains(MistypedChar) And CappedStrike Then
                                    'The mistyped character is a symbol and the strike is capped. Select a symbol from CapStrikes.
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Mistyped character is a symbol and the strike is capped.")
                                    MistypedChar = CapStrikes(StrikeCharsIndex)(RandomStrikeIndex)
                                End If
                                StruckChar = MistypedChar
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Struck character: {0}", StruckChar)
                            End If
                        End If

                        'Write the final character to the console and wait
                        If Not StruckChar = vbNullChar Then Console.Write(StruckChar)
                        SleepNoBlock(WriteMs, Typo)
                    Next

                    'Wait until retry
                    Console.WriteLine()
                    If Not ResizeSyncing Then SleepNoBlock(TypoWriteAgainDelay, Typo)

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                End If
                SleepNoBlock(TypoDelay, Typo)
            Loop
        End Sub

        ''' <summary>
        ''' Checks for any screensaver error
        ''' </summary>
        Sub CheckForError(sender As Object, e As RunWorkerCompletedEventArgs) Handles Typo.RunWorkerCompleted
            HandleSaverError(e.Error)
        End Sub

    End Module
End Namespace
