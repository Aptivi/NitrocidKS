
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

Namespace Shell.Commands
    Class KeyInfoCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Write(DoTranslation("Enter a key or a combination of keys to display its information."), True, GetConsoleColor(ColTypes.Neutral))
            Dim KeyPress As ConsoleKeyInfo = DetectKeypress()

            'Pressed key
            Write("- " + DoTranslation("Pressed key") + ": ", False, GetConsoleColor(ColTypes.ListEntry))
            Write(KeyPress.Key.ToString, True, GetConsoleColor(ColTypes.ListValue))

            'If the pressed key is a control key, don't write the actual key char so as not to corrupt the output
            If Not Char.IsControl(KeyPress.KeyChar) Then
                Write("- " + DoTranslation("Pressed key character") + ": ", False, GetConsoleColor(ColTypes.ListEntry))
                Write(KeyPress.KeyChar, True, GetConsoleColor(ColTypes.ListValue))
            End If

            'Pressed key character code
            Write("- " + DoTranslation("Pressed key character code") + ": ", False, GetConsoleColor(ColTypes.ListEntry))
            Write($"0x{Convert.ToInt32(KeyPress.KeyChar):X2} [{Convert.ToInt32(KeyPress.KeyChar)}]", True, GetConsoleColor(ColTypes.ListValue))

            'Pressed modifiers
            Write("- " + DoTranslation("Pressed modifiers") + ": ", False, GetConsoleColor(ColTypes.ListEntry))
            Write(KeyPress.Modifiers.ToString, True, GetConsoleColor(ColTypes.ListValue))

            'Keyboard shortcut
            Write("- " + DoTranslation("Keyboard shortcut") + ": ", False, GetConsoleColor(ColTypes.ListEntry))
            Write($"{String.Join(" +", KeyPress.Modifiers.ToString.Split(", "))} + {KeyPress.Key}", True, GetConsoleColor(ColTypes.ListValue))
        End Sub

    End Class
End Namespace