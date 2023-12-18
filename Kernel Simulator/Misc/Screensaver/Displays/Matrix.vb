
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

Namespace Misc.Screensaver.Displays
    Public Module MatrixSettings

        Private _matrixDelay As Integer = 1

        ''' <summary>
        ''' [Matrix] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property MatrixDelay As Integer
            Get
                Return _matrixDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 1
                _matrixDelay = value
            End Set
        End Property

    End Module

    Public Class MatrixDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        Public Overrides Property ScreensaverName As String = "Matrix" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = ConsoleWrapper.WindowWidth
            CurrentWindowHeight = ConsoleWrapper.WindowHeight
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.Green
            ConsoleWrapper.Clear()
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            ConsoleWrapper.CursorVisible = False
            If CurrentWindowHeight <> ConsoleWrapper.WindowHeight Or CurrentWindowWidth <> ConsoleWrapper.WindowWidth Then ResizeSyncing = True
            If Not ResizeSyncing Then
                WritePlain(CStr(RandomDriver.Next(2)), False)
            Else
                WdbgConditional(ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...")
                ConsoleWrapper.Clear()
            End If

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = ConsoleWrapper.WindowWidth
            CurrentWindowHeight = ConsoleWrapper.WindowHeight
            SleepNoBlock(MatrixDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace
