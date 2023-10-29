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

Namespace Network.HTTP.Commands
    Class HTTP_GetStringCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If HTTPConnected = True Then
                'Print a message
                TextWriterColor.Write(DoTranslation("Getting {0}..."), True, ColTypes.Progress, ListArgs(0))

                Try
                    Dim ResponseTask As Task(Of String) = HttpGetString(ListArgs(0))
                    ResponseTask.Wait()
                    Dim Response As String = ResponseTask.Result
                    TextWriterColor.Write(Response, True, ColTypes.Neutral)
                Catch aex As AggregateException
                    TextWriterColor.Write(aex.Message + ":", True, ColTypes.Error)
                    For Each InnerException As Exception In aex.InnerExceptions
                        TextWriterColor.Write("- " + InnerException.Message, True, ColTypes.Error)
                        If InnerException.InnerException IsNot Nothing Then
                            TextWriterColor.Write("- " + InnerException.InnerException.Message, True, ColTypes.Error)
                        End If
                    Next
                Catch ex As Exception
                    TextWriterColor.Write(ex.Message, True, ColTypes.Error)
                End Try
            Else
                TextWriterColor.Write(DoTranslation("You must connect to server before performing transmission."), True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace