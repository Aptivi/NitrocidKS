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

Class GetTimeInfoCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
        Dim DateTimeInfo As Date
        Dim UnixEpoch As New Date(1970, 1, 1, 0, 0, 0, 0)
        If Date.TryParse(ListArgs(0), DateTimeInfo) Then
            W("-- " + DoTranslation("Information for") + " {0} --" + vbNewLine, True, ColTypes.Neutral, Render(DateTimeInfo))
            W(DoTranslation("Milliseconds:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Millisecond)
            W(DoTranslation("Seconds:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Second)
            W(DoTranslation("Minutes:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Minute)
            W(DoTranslation("Hours:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Hour)
            W(DoTranslation("Days:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Day)
            W(DoTranslation("Months:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Month)
            W(DoTranslation("Year:") + " {0}" + vbNewLine, True, ColTypes.Neutral, DateTimeInfo.Year)
            W(DoTranslation("Date:") + " {0}", True, ColTypes.Neutral, RenderDate(DateTimeInfo))
            W(DoTranslation("Time:") + " {0}" + vbNewLine, True, ColTypes.Neutral, RenderTime(DateTimeInfo))
            W(DoTranslation("Day of Year:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.DayOfYear)
            W(DoTranslation("Day of Week:") + " {0}" + vbNewLine, True, ColTypes.Neutral, DateTimeInfo.DayOfWeek.ToString)
            W(DoTranslation("Binary:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.ToBinary)
            W(DoTranslation("Local Time:") + " {0}", True, ColTypes.Neutral, Render(DateTimeInfo.ToLocalTime))
            W(DoTranslation("Universal Time:") + " {0}", True, ColTypes.Neutral, Render(DateTimeInfo.ToUniversalTime))
            W(DoTranslation("Unix Time:") + " {0}", True, ColTypes.Neutral, (DateTimeInfo - UnixEpoch).TotalSeconds)
        Else
            W(DoTranslation("Failed to parse date information for") + " {0}. " + DoTranslation("Ensure that the format is correct."), True, ColTypes.Error, ListArgs(0))
        End If
    End Sub

End Class