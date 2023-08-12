
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

Imports KS.TimeDate

Namespace Shell.Commands
    Class GetTimeInfoCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim DateTimeInfo As Date
            If Date.TryParse(ListArgs(0), DateTimeInfo) Then
                TextWriterColor.Write("-- " + DoTranslation("Information for") + " {0} --" + NewLine, True, ColTypes.Neutral, Render(DateTimeInfo))
                TextWriterColor.Write(DoTranslation("Milliseconds:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Millisecond)
                TextWriterColor.Write(DoTranslation("Seconds:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Second)
                TextWriterColor.Write(DoTranslation("Minutes:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Minute)
                TextWriterColor.Write(DoTranslation("Hours:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Hour)
                TextWriterColor.Write(DoTranslation("Days:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Day)
                TextWriterColor.Write(DoTranslation("Months:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Month)
                TextWriterColor.Write(DoTranslation("Year:") + " {0}" + NewLine, True, ColTypes.Neutral, DateTimeInfo.Year)
                TextWriterColor.Write(DoTranslation("Date:") + " {0}", True, ColTypes.Neutral, RenderDate(DateTimeInfo))
                TextWriterColor.Write(DoTranslation("Time:") + " {0}" + NewLine, True, ColTypes.Neutral, RenderTime(DateTimeInfo))
                TextWriterColor.Write(DoTranslation("Day of Year:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.DayOfYear)
                TextWriterColor.Write(DoTranslation("Day of Week:") + " {0}" + NewLine, True, ColTypes.Neutral, DateTimeInfo.DayOfWeek.ToString)
                TextWriterColor.Write(DoTranslation("Binary:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.ToBinary)
                TextWriterColor.Write(DoTranslation("Local Time:") + " {0}", True, ColTypes.Neutral, Render(DateTimeInfo.ToLocalTime))
                TextWriterColor.Write(DoTranslation("Universal Time:") + " {0}", True, ColTypes.Neutral, Render(DateTimeInfo.ToUniversalTime))
                TextWriterColor.Write(DoTranslation("Unix Time:") + " {0}", True, ColTypes.Neutral, DateToUnix(DateTimeInfo))
            Else
                TextWriterColor.Write(DoTranslation("Failed to parse date information for") + " {0}. " + DoTranslation("Ensure that the format is correct."), True, ColTypes.Error, ListArgs(0))
            End If
        End Sub

    End Class
End Namespace