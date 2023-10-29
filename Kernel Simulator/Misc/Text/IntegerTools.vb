
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

Imports System.Runtime.CompilerServices

Public Module IntegerTools
    ''' <summary>
    ''' Swaps the two numbers if the source is larger than the target
    ''' </summary>
    ''' <param name="SourceNumber">Number</param>
    ''' <param name="TargetNumber">Number</param>
    <Extension()>
    Public Sub SwapIfSourceLarger(ByRef SourceNumber As Integer, ByRef TargetNumber As Integer)
        Dim Source = SourceNumber
        Dim Target = TargetNumber
        If SourceNumber > TargetNumber Then
            SourceNumber = Target
            TargetNumber = Source
        End If
    End Sub

    ''' <summary>
    ''' Swaps the two numbers if the source is larger than the target
    ''' </summary>
    ''' <param name="SourceNumber">Number</param>
    ''' <param name="TargetNumber">Number</param>
    <Extension()>
    Public Sub SwapIfSourceLarger(ByRef SourceNumber As Long, ByRef TargetNumber As Long)
        Dim Source = SourceNumber
        Dim Target = TargetNumber
        If SourceNumber > TargetNumber Then
            SourceNumber = Target
            TargetNumber = Source
        End If
    End Sub
End Module
