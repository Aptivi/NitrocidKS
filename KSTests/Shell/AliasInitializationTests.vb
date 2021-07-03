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

Imports System.IO
Imports KS

<TestClass()> Public Class AliasInitializationTests

    ''' <summary>
    ''' Tests alias initialization
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitAliases()
        Dim PathToTestAliases As String = Path.GetFullPath("TestAliases.json")
        If Not File.Exists(paths("Aliases")) Then File.Copy(PathToTestAliases, paths("Aliases"))
        InitAliases()
        Aliases.ShouldNotBeEmpty
        RemoteDebugAliases.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests alias initialization and saving
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitAndSaveAliases()
        Dim PathToTestAliases As String = Path.GetFullPath("TestAliases.json")
        If Not File.Exists(paths("Aliases")) Then File.Copy(PathToTestAliases, paths("Aliases"))
        InitAliases()
        SaveAliases()
        Aliases.ShouldNotBeEmpty
        RemoteDebugAliases.ShouldNotBeEmpty
    End Sub

End Class