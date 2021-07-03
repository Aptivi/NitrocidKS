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

Imports KS

<TestClass()> Public Class PlaceholderActionTests

    ''' <summary>
    ''' Tests parsing placeholders
    ''' </summary>
    <TestMethod()> <TestCategory("Action")> Public Sub TestParsePlaceholders()
        Dim UnparsedStrings As New List(Of String)
        CurrentUser = "Test"
        Dim ParsedStrings As New List(Of String) From {
            ProbePlaces("Username is <user>"),
            ProbePlaces("Hostname is <host>"),
            ProbePlaces("Short date is <shortdate>"),
            ProbePlaces("Long date is <longdate>"),
            ProbePlaces("Short time is <shorttime>"),
            ProbePlaces("Long time is <longtime>"),
            ProbePlaces("Date is <date>"),
            ProbePlaces("Time is <time>"),
            ProbePlaces("Timezone is <timezone>"),
            ProbePlaces("Summer timezone is <summertimezone>"),
            ProbePlaces("Operating system is <system>")
        }
        For Each ParsedString As String In ParsedStrings
            If ParsedString.Contains("<") And ParsedString.Contains(">") Then
                UnparsedStrings.Add(ParsedString)
            End If
        Next
        UnparsedStrings.ShouldBeEmpty
    End Sub

End Class