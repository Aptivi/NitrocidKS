﻿
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

Imports StringMath

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Calculates expressions.
    ''' </summary>
    ''' <remarks>
    ''' This command lets you calculate expressions and return the results.
    ''' </remarks>
    Class CalcCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Try
                Dim Res As String = SMath.Evaluate(StringArgs)
                Wdbg(DebugLevel.I, "Res = {0}", Res)
                Write(StringArgs + " = " + Res, True, ColTypes.Neutral)
            Catch ex As Exception
                Wdbg(DebugLevel.I, "Error trying to calculate expression {0}: {1}", StringArgs, ex.Message)
                WStkTrc(ex)
                Write(DoTranslation("Error in calculation.") + " {0}", True, ColTypes.Error, ex.Message)
            End Try
        End Sub

    End Class
End Namespace
