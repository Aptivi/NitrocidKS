
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

Module TextWriterWhereColor

    ''' <summary>
    ''' Outputs the text into the terminal prompt with location support, and sets colors as needed.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Left">Column number in console</param>
    ''' <param name="Top">Row number in console</param>
    ''' <param name="Return">Whether or not to return to old position</param>
    ''' <param name="colorType">A type of colors that will be changed.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhere(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal [Return] As Boolean, ByVal colorType As ColTypes, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
            If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out) Then
                If colorType = ColTypes.Neutral Then
                    SetConsoleColor(New Color(NeutralTextColor))
                ElseIf colorType = ColTypes.Input Then
                    SetConsoleColor(New Color(InputColor))
                ElseIf colorType = ColTypes.Continuable Then
                    SetConsoleColor(New Color(ContKernelErrorColor))
                ElseIf colorType = ColTypes.Uncontinuable Then
                    SetConsoleColor(New Color(UncontKernelErrorColor))
                ElseIf colorType = ColTypes.HostName Then
                    SetConsoleColor(New Color(HostNameShellColor))
                ElseIf colorType = ColTypes.UserName Then
                    SetConsoleColor(New Color(UserNameShellColor))
                ElseIf colorType = ColTypes.License Then
                    SetConsoleColor(New Color(LicenseColor))
                ElseIf colorType = ColTypes.Gray Then
                    If New Color(BackgroundColor).IsBright Then
                        SetConsoleColor(New Color(NeutralTextColor))
                    Else
                        SetConsoleColor(New Color(ConsoleColors.Gray))
                    End If
                ElseIf colorType = ColTypes.ListValue Then
                    SetConsoleColor(New Color(ListValueColor))
                ElseIf colorType = ColTypes.ListEntry Then
                    SetConsoleColor(New Color(ListEntryColor))
                ElseIf colorType = ColTypes.Stage Then
                    SetConsoleColor(New Color(StageColor))
                ElseIf colorType = ColTypes.Error Then
                    SetConsoleColor(New Color(ErrorColor))
                ElseIf colorType = ColTypes.Warning Then
                    SetConsoleColor(New Color(WarningColor))
                ElseIf colorType = ColTypes.Option Then
                    SetConsoleColor(New Color(OptionColor))
                ElseIf colorType = ColTypes.Banner Then
                    SetConsoleColor(New Color(BannerColor))
                Else
                    Exit Sub
                End If
                SetConsoleColor(New Color(BackgroundColor), True)
            End If

            'Format the message as necessary
            If Not vars.Length = 0 Then msg = String.Format(msg, vars)

            'Write text in another place. By the way, we check the text for newlines and console width excess
            Dim OldLeft As Integer = Console.CursorLeft
            Dim OldTop As Integer = Console.CursorTop
            Dim Paragraphs() As String = msg.SplitNewLines
            Console.SetCursorPosition(Left, Top)
            For MessageParagraphIndex As Integer = 0 To Paragraphs.Length - 1
                'We can now check to see if we're writing a letter past the console window width
                Dim MessageParagraph As String = Paragraphs(MessageParagraphIndex)
                For Each ParagraphChar As Char In MessageParagraph
                    If Console.CursorLeft = Console.WindowWidth - 1 Then
                        If Console.CursorTop = Console.BufferHeight - 1 Then
                            'We've reached the end of buffer. Write the line to scroll.
                            Console.WriteLine()
                        Else
                            Console.CursorTop += 1
                        End If
                        Console.CursorLeft = Left
                    End If
                    Console.Write(ParagraphChar)
                Next

                'We're starting with the new paragraph, so we increase the Console.CursorTop value by 1.
                If Not MessageParagraphIndex = Paragraphs.Length - 1 Then
                    If Console.CursorTop = Console.BufferHeight - 1 Then
                        'We've reached the end of buffer. Write the line to scroll.
                        Console.WriteLine()
                    Else
                        Console.CursorTop += 1
                    End If
                    Console.CursorLeft = Left
                End If
            Next
            If [Return] Then Console.SetCursorPosition(OldLeft, OldTop)
            If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor = "0;0;0" Then Console.ResetColor()
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with location support, and sets colors as needed.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Left">Column number in console</param>
    ''' <param name="Top">Row number in console</param>
    ''' <param name="Return">Whether or not to return to old position</param>
    ''' <param name="color">A color that will be changed to.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhereC16(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal [Return] As Boolean, ByVal color As ConsoleColor, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Console.BackgroundColor = IIf(IsNumeric(New Color(BackgroundColor).PlainSequence), If(BackgroundColor <= 15, [Enum].Parse(GetType(ConsoleColor), BackgroundColor), ConsoleColor.Black), ConsoleColor.Black)
            Console.ForegroundColor = color

            'Format the message as necessary
            If Not vars.Length = 0 Then msg = String.Format(msg, vars)

            'Write text in another place. By the way, we check the text for newlines and console width excess
            Dim OldLeft As Integer = Console.CursorLeft
            Dim OldTop As Integer = Console.CursorTop
            Dim Paragraphs() As String = msg.SplitNewLines
            Console.SetCursorPosition(Left, Top)
            For MessageParagraphIndex As Integer = 0 To Paragraphs.Length - 1
                'We can now check to see if we're writing a letter past the console window width
                Dim MessageParagraph As String = Paragraphs(MessageParagraphIndex)
                For Each ParagraphChar As Char In MessageParagraph
                    If Console.CursorLeft = Console.WindowWidth - 1 Then
                        If Console.CursorTop = Console.BufferHeight - 1 Then
                            'We've reached the end of buffer. Write the line to scroll.
                            Console.WriteLine()
                        Else
                            Console.CursorTop += 1
                        End If
                        Console.CursorLeft = Left
                    End If
                    Console.Write(ParagraphChar)
                Next

                'We're starting with the new paragraph, so we increase the Console.CursorTop value by 1.
                If Not MessageParagraphIndex = Paragraphs.Length - 1 Then
                    If Console.CursorTop = Console.BufferHeight - 1 Then
                        'We've reached the end of buffer. Write the line to scroll.
                        Console.WriteLine()
                    Else
                        Console.CursorTop += 1
                    End If
                    Console.CursorLeft = Left
                End If
            Next
            If [Return] Then Console.SetCursorPosition(OldLeft, OldTop)
            If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor = "0;0;0" Then Console.ResetColor()
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with location support, and sets colors as needed.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Left">Column number in console</param>
    ''' <param name="Top">Row number in console</param>
    ''' <param name="Return">Whether or not to return to old position</param>
    ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
    ''' <param name="BackgroundColor">A background color that will be changed to.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhereC16(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal [Return] As Boolean, ByVal ForegroundColor As ConsoleColor, ByVal BackgroundColor As ConsoleColor, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Console.BackgroundColor = BackgroundColor
            Console.ForegroundColor = ForegroundColor

            'Format the message as necessary
            If Not vars.Length = 0 Then msg = String.Format(msg, vars)

            'Write text in another place. By the way, we check the text for newlines and console width excess
            Dim OldLeft As Integer = Console.CursorLeft
            Dim OldTop As Integer = Console.CursorTop
            Dim Paragraphs() As String = msg.SplitNewLines
            Console.SetCursorPosition(Left, Top)
            For MessageParagraphIndex As Integer = 0 To Paragraphs.Length - 1
                'We can now check to see if we're writing a letter past the console window width
                Dim MessageParagraph As String = Paragraphs(MessageParagraphIndex)
                For Each ParagraphChar As Char In MessageParagraph
                    If Console.CursorLeft = Console.WindowWidth - 1 Then
                        If Console.CursorTop = Console.BufferHeight - 1 Then
                            'We've reached the end of buffer. Write the line to scroll.
                            Console.WriteLine()
                        Else
                            Console.CursorTop += 1
                        End If
                        Console.CursorLeft = Left
                    End If
                    Console.Write(ParagraphChar)
                Next

                'We're starting with the new paragraph, so we increase the Console.CursorTop value by 1.
                If Not MessageParagraphIndex = Paragraphs.Length - 1 Then
                    If Console.CursorTop = Console.BufferHeight - 1 Then
                        'We've reached the end of buffer. Write the line to scroll.
                        Console.WriteLine()
                    Else
                        Console.CursorTop += 1
                    End If
                    Console.CursorLeft = Left
                End If
            Next
            If [Return] Then Console.SetCursorPosition(OldLeft, OldTop)
            If BackgroundColor = ConsoleColor.Black Then Console.ResetColor()
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with location support, and sets colors as needed.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Left">Column number in console</param>
    ''' <param name="Top">Row number in console</param>
    ''' <param name="Return">Whether or not to return to old position</param>
    ''' <param name="color">A color that will be changed to.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhereC(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal [Return] As Boolean, ByVal color As Color, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out) Then
                SetConsoleColor(color)
                SetConsoleColor(New Color(BackgroundColor), True)
            End If

            'Format the message as necessary
            If Not vars.Length = 0 Then msg = String.Format(msg, vars)

            'Write text in another place. By the way, we check the text for newlines and console width excess
            Dim OldLeft As Integer = Console.CursorLeft
            Dim OldTop As Integer = Console.CursorTop
            Dim Paragraphs() As String = msg.SplitNewLines
            Console.SetCursorPosition(Left, Top)
            For MessageParagraphIndex As Integer = 0 To Paragraphs.Length - 1
                'We can now check to see if we're writing a letter past the console window width
                Dim MessageParagraph As String = Paragraphs(MessageParagraphIndex)
                For Each ParagraphChar As Char In MessageParagraph
                    If Console.CursorLeft = Console.WindowWidth - 1 Then
                        If Console.CursorTop = Console.BufferHeight - 1 Then
                            'We've reached the end of buffer. Write the line to scroll.
                            Console.WriteLine()
                        Else
                            Console.CursorTop += 1
                        End If
                        Console.CursorLeft = Left
                    End If
                    Console.Write(ParagraphChar)
                Next

                'We're starting with the new paragraph, so we increase the Console.CursorTop value by 1.
                If Not MessageParagraphIndex = Paragraphs.Length - 1 Then
                    If Console.CursorTop = Console.BufferHeight - 1 Then
                        'We've reached the end of buffer. Write the line to scroll.
                        Console.WriteLine()
                    Else
                        Console.CursorTop += 1
                    End If
                    Console.CursorLeft = Left
                End If
            Next
            If [Return] Then Console.SetCursorPosition(OldLeft, OldTop)
            If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor = "0;0;0" Then Console.ResetColor()
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with location support, and sets colors as needed.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Left">Column number in console</param>
    ''' <param name="Top">Row number in console</param>
    ''' <param name="Return">Whether or not to return to old position</param>
    ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
    ''' <param name="BackgroundColor">A background color that will be changed to.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhereC(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal [Return] As Boolean, ByVal ForegroundColor As Color, ByVal BackgroundColor As Color, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out) Then
                SetConsoleColor(ForegroundColor)
                SetConsoleColor(BackgroundColor, True)
            End If

            'Format the message as necessary
            If Not vars.Length = 0 Then msg = String.Format(msg, vars)

            'Write text in another place. By the way, we check the text for newlines and console width excess
            Dim OldLeft As Integer = Console.CursorLeft
            Dim OldTop As Integer = Console.CursorTop
            Dim Paragraphs() As String = msg.SplitNewLines
            Console.SetCursorPosition(Left, Top)
            For MessageParagraphIndex As Integer = 0 To Paragraphs.Length - 1
                'We can now check to see if we're writing a letter past the console window width
                Dim MessageParagraph As String = Paragraphs(MessageParagraphIndex)
                For Each ParagraphChar As Char In MessageParagraph
                    If Console.CursorLeft = Console.WindowWidth - 1 Then
                        If Console.CursorTop = Console.BufferHeight - 1 Then
                            'We've reached the end of buffer. Write the line to scroll.
                            Console.WriteLine()
                        Else
                            Console.CursorTop += 1
                        End If
                        Console.CursorLeft = Left
                    End If
                    Console.Write(ParagraphChar)
                Next

                'We're starting with the new paragraph, so we increase the Console.CursorTop value by 1.
                If Not MessageParagraphIndex = Paragraphs.Length - 1 Then
                    If Console.CursorTop = Console.BufferHeight - 1 Then
                        'We've reached the end of buffer. Write the line to scroll.
                        Console.WriteLine()
                    Else
                        Console.CursorTop += 1
                    End If
                    Console.CursorLeft = Left
                End If
            Next
            If [Return] Then Console.SetCursorPosition(OldLeft, OldTop)
            If BackgroundColor.PlainSequence = "0" Or BackgroundColor.PlainSequence = "0;0;0" Then Console.ResetColor()
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

End Module
