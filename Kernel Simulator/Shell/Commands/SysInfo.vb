
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

Imports KS.Hardware

Namespace Shell.Commands
    Class SysInfoCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim ShowSystemInfo, ShowHardwareInfo, ShowUserInfo, ShowMessageOfTheDay, ShowMal As Boolean
            If ListSwitchesOnly.Contains("-s") Then ShowSystemInfo = True
            If ListSwitchesOnly.Contains("-h") Then ShowHardwareInfo = True
            If ListSwitchesOnly.Contains("-u") Then ShowUserInfo = True
            If ListSwitchesOnly.Contains("-m") Then ShowMessageOfTheDay = True
            If ListSwitchesOnly.Contains("-l") Then ShowMal = True
            If ListSwitchesOnly.Contains("-a") Or ListSwitchesOnly.Length = 0 Then
                ShowSystemInfo = True
                ShowHardwareInfo = True
                ShowUserInfo = True
                ShowMessageOfTheDay = True
                ShowMal = True
            End If

            If ShowSystemInfo Then
                'Kernel section
                WriteSeparator(DoTranslation("Kernel settings"), True)
                Write(DoTranslation("Kernel Version:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(KernelVersion, True, GetConsoleColor(ColTypes.ListValue))
                Write(DoTranslation("Debug Mode:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(DebugMode.ToString, True, GetConsoleColor(ColTypes.ListValue))
                Write(DoTranslation("Colored Shell:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(ColoredShell.ToString, True, GetConsoleColor(ColTypes.ListValue))
                Write(DoTranslation("Arguments on Boot:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(ArgsOnBoot.ToString, True, GetConsoleColor(ColTypes.ListValue))
                Write(DoTranslation("Help command simplified:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(SimHelp.ToString, True, GetConsoleColor(ColTypes.ListValue))
                Write(DoTranslation("MOTD on Login:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(ShowMOTD.ToString, True, GetConsoleColor(ColTypes.ListValue))
                Write(DoTranslation("Time/Date on corner:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(CornerTimeDate.ToString, True, GetConsoleColor(ColTypes.ListValue))
                WritePlain("", True)
            End If

            If ShowHardwareInfo Then
                'Hardware section
                WriteSeparator(DoTranslation("Hardware settings"), True)
                ListHardware()
                Write(DoTranslation("Use ""hwinfo"" for extended information about hardware."), True, ColTypes.Tip)
                WritePlain("", True)
            End If

            If ShowUserInfo Then
                'User section
                WriteSeparator(DoTranslation("User settings"), True)
                Write(DoTranslation("Current user name:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(CurrentUser.Username, True, GetConsoleColor(ColTypes.ListValue))
                Write(DoTranslation("Current host name:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(HostName, True, GetConsoleColor(ColTypes.ListValue))
                Write(DoTranslation("Available usernames:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(String.Join(", ", ListAllUsers), True, GetConsoleColor(ColTypes.ListValue))
                WritePlain("", True)
            End If

            If ShowMessageOfTheDay Then
                'Show MOTD
                WriteSeparator("MOTD", True)
                Write(ProbePlaces(MOTDMessage), True, GetConsoleColor(ColTypes.Neutral))
            End If

            If ShowMal Then
                'Show MAL
                WriteSeparator("MAL", True)
                Write(ProbePlaces(MAL), True, GetConsoleColor(ColTypes.Neutral))
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, GetConsoleColor(ColTypes.Neutral))
            Write("  -s: ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(DoTranslation("Shows the system information"), True, GetConsoleColor(ColTypes.ListValue))
            Write("  -h: ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(DoTranslation("Shows the hardware information"), True, GetConsoleColor(ColTypes.ListValue))
            Write("  -u: ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(DoTranslation("Shows the user information"), True, GetConsoleColor(ColTypes.ListValue))
            Write("  -m: ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(DoTranslation("Shows the message of the day"), True, GetConsoleColor(ColTypes.ListValue))
            Write("  -l: ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(DoTranslation("Shows the message of the day after login"), True, GetConsoleColor(ColTypes.ListValue))
            Write("  -a: ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(DoTranslation("Shows all information"), True, GetConsoleColor(ColTypes.ListValue))
        End Sub

    End Class
End Namespace