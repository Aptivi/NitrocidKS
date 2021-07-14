
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Imports System.CodeDom.Compiler
Imports System.ComponentModel
Imports System.Reflection
Imports System.Threading

Public Module Screensaver

    'Variables
    Public LockMode As Boolean = False
    Public InSaver As Boolean = False
    Public defSaverName As String = "glitterMatrix"
    Public ScrnSvrdb As New Dictionary(Of String, BackgroundWorker) From {{"colorMix", ColorMix}, {"colorMix255", ColorMix255}, {"matrix", Matrix}, {"glitterMatrix", GlitterMatrix}, {"disco", Disco},
                                                                          {"lines", Lines}, {"glitterColor", GlitterColor}, {"aptErrorSim", AptErrorSim}, {"hackUserFromAD", HackUserFromAD},
                                                                          {"glitterColor255", GlitterColor255}, {"disco255", Disco255}, {"lines255", Lines255}}
    Public CSvrdb As New Dictionary(Of String, ICustomSaver)
    Public WithEvents Timeout As New BackgroundWorker
    Friend SaverAutoReset As New AutoResetEvent(False)
    Private execCustomSaver As CompilerResults
    Private DoneFlag As Boolean = False

    'Interface
    Public Interface ICustomSaver
        Sub InitSaver()
        Sub PreDisplay() 'Code before starting screensaver
        Sub PostDisplay() 'Code after stopping screensaver
        Sub ScrnSaver() 'Code during the screensaver
        Property Initialized As Boolean
        Property DelayForEachWrite As Integer
        Property SaverName As String
    End Interface

    Sub HandleTimeout(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Timeout.DoWork
        Dim time As Integer = 3600000
        Dim count As Integer
        Dim oldcursor As Integer = Console.CursorLeft
        While True
            If Not ScrnTimeReached Then
                For count = 0 To time
                    Thread.Sleep(1)
                    If oldcursor <> Console.CursorLeft Then
                        count = 0
                    End If
                    oldcursor = Console.CursorLeft
                Next
                If Not RebootRequested Then
                    Wdbg("W", "Screen time has reached.")
                    ShowSavers(defSaverName)
                End If
            End If
        End While
    End Sub
    Sub ShowSavers(ByVal saver As String)
        Try
            InSaver = True
            ScrnTimeReached = True
            EventManager.RaisePreShowScreensaver()
            Wdbg("I", "Requested screensaver: {0}", saver)
            If ScrnSvrdb.ContainsKey(saver) Then
                ScrnSvrdb(saver).RunWorkerAsync()
                Wdbg("I", "{0} started", saver)
                Console.ReadKey()
                ScrnTimeReached = False
                ScrnSvrdb(saver).CancelAsync()
                SaverAutoReset.WaitOne()
            ElseIf CSvrdb.ContainsKey(saver) Then
                'Only one custom screensaver can be used.
                finalSaver = CSvrdb(saver)
                Custom.WorkerSupportsCancellation = True
                Custom.RunWorkerAsync()
                Wdbg("I", "Custom screensaver {0} started", saver)
                Console.ReadKey()
                ScrnTimeReached = False
                Custom.CancelAsync()
                SaverAutoReset.WaitOne()
            Else
                W(DoTranslation("The requested screensaver {0} is not found.", currentLang), True, ColTypes.Neutral, saver)
                Wdbg("I", "Screensaver {0} not found in the dictionary.", saver)
            End If
            EventManager.RaisePostShowScreensaver()
            InSaver = False
        Catch ex As InvalidOperationException
            W(DoTranslation("Error when trying to start screensaver, because of an invalid operation.", currentLang), True, ColTypes.Neutral)
            WStkTrc(ex)
        End Try
    End Sub

    Sub CompileCustom(ByVal file As String)
        'Initialize path
        Dim modPath As String = paths("Mods")
        file = file.Replace("\", "/").Replace(modPath, "")

        'Start parsing screensaver
        If FileIO.FileSystem.FileExists(modPath + file) Then
            Wdbg("I", "Parsing {0}...", file)
            If Not file.EndsWith("SS.m") Then
                Wdbg("W", "{0} is not a screensaver. A screensaver code should have ""SS.m"" at the end.", file)
            Else
                Wdbg("W", "{0} is a valid screensaver. Generating...", file)
                finalSaver = GenSaver(IO.File.ReadAllText(modPath + file))
                If DoneFlag = True Then
                    Wdbg("I", "{0} compiled correctly. Starting...", file)
                    finalSaver.InitSaver()
                    Dim SaverName As String = finalSaver.SaverName
                    If finalSaver.Initialized = True Then
                        Dim IsFound As Boolean
                        If Not SaverName = "" Then
                            IsFound = CSvrdb.ContainsKey(SaverName)
                        Else
                            IsFound = CSvrdb.ContainsKey(file)
                        End If
                        Wdbg("I", "Is screensaver found? {0}", IsFound)
                        If Not IsFound Then
                            If Not SaverName = "" Then
                                W(DoTranslation("{0} has been initialized properly.", currentLang), True, ColTypes.Neutral, SaverName)
                                Wdbg("I", "{0} ({1}) compiled correctly. Starting...", SaverName, file)
                                CSvrdb.Add(SaverName, finalSaver)
                            Else
                                W(DoTranslation("{0} has been initialized properly.", currentLang), True, ColTypes.Neutral, file)
                                Wdbg("I", "{0} compiled correctly. Starting...", file)
                                CSvrdb.Add(file, finalSaver)
                            End If
                        Else
                            If Not SaverName = "" Then
                                Wdbg("W", "{0} ({1}) already exists. Recompiling...", SaverName, file)
                                CSvrdb.Remove(SaverName)
                                CompileCustom(file)
                                Exit Sub
                            Else
                                Wdbg("W", "{0} already exists. Recompiling...", file)
                                CSvrdb.Remove(file)
                                CompileCustom(file)
                                Exit Sub
                            End If
                        End If
                    Else
                        If Not SaverName = "" Then
                            W(DoTranslation("{0} did not initialize. The screensaver code might have experienced an error while initializing.", currentLang), True, ColTypes.Neutral, SaverName)
                            Wdbg("W", "{0} ({1}) is compiled, but not initialized.", SaverName, file)
                        Else
                            W(DoTranslation("{0} did not initialize. The screensaver code might have experienced an error while initializing.", currentLang), True, ColTypes.Neutral, file)
                            Wdbg("W", "{0} is compiled, but not initialized.", file)
                        End If
                    End If
                End If
            End If
        Else
            W(DoTranslation("Screensaver {0} does not exist.", currentLang), True, ColTypes.Neutral, file)
            Wdbg("E", "The file {0} does not exist for compilation.", file)
        End If
    End Sub

    Sub SetDefaultScreensaver(ByVal saver As String)
        If ScrnSvrdb.ContainsKey(saver) Or CSvrdb.ContainsKey(saver) Then
            Wdbg("I", "{0} is found. Setting it to default...", saver)
            Dim ksconf As New IniFile()
            Dim pathConfig As String = paths("Configuration")
            ksconf.Load(pathConfig)
            ksconf.Sections("Misc").Keys("Screensaver").Value = saver
            ksconf.Save(pathConfig)
            defSaverName = saver
            W(DoTranslation("{0} is set to default screensaver.", currentLang), True, ColTypes.Neutral, saver)
        Else
            Wdbg("W", "{0} is not found.", saver)
            W(DoTranslation("Screensaver {0} not found in database. Check the name and try again.", currentLang), True, ColTypes.Neutral, saver)
        End If
    End Sub

    Function GenSaver(ByVal code As String) As ICustomSaver
        DoneFlag = False
        Using provider As New VBCodeProvider()
            Dim prm As New CompilerParameters With {
                .GenerateExecutable = False,
                .GenerateInMemory = True
            }
            prm.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly.Location)
            prm.ReferencedAssemblies.Add("System.dll")
            prm.ReferencedAssemblies.Add("System.Core.dll")
            prm.ReferencedAssemblies.Add("System.Data.dll")
            prm.ReferencedAssemblies.Add("System.DirectoryServices.dll")
            prm.ReferencedAssemblies.Add("System.Xml.dll")
            prm.ReferencedAssemblies.Add("System.Xml.Linq.dll")
            Wdbg("I", "All referenced assemblies prepared.")
            Dim namespc As String = GetType(ICustomSaver).Namespace
            Dim modCode() As String = New String() {"Imports " & namespc & vbNewLine & code}
            Wdbg("I", "Compiling right now...")
            execCustomSaver = provider.CompileAssemblyFromSource(prm, modCode)
            Wdbg("I", "Compilation results: Errors? {0}, Warnings? {1} | Total: {2}", execCustomSaver.Errors.HasErrors, execCustomSaver.Errors.HasWarnings, execCustomSaver.Errors.Count)
            If execCustomSaver.Errors.HasErrors Then
                W(DoTranslation("Screensaver can't be loaded because of the following: ", currentLang), True, ColTypes.Neutral)
                Wdbg("E", "Errors when compiling:")
                For Each errorName In execCustomSaver.Errors
                    W(errorName.ToString, True, ColTypes.Neutral) : Wdbg("E", errorName.ToString, True)
                Next
                Exit Function
            Else
                DoneFlag = True
            End If
            Wdbg("I", "Creating instance of type...")
            For Each t As Type In execCustomSaver.CompiledAssembly.GetTypes()
                If t.GetInterface(GetType(ICustomSaver).Name) IsNot Nothing Then Return CType(execCustomSaver.CompiledAssembly.CreateInstance(t.Name), ICustomSaver)
            Next
        End Using
    End Function

End Module
