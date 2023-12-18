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

Imports KS.Files.Querying
Imports System.IO
Imports KS.Modifications

Namespace Shell.Commands
    Class ModManCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If Not SafeMode Then
                Dim CommandMode As String = ListArgsOnly(0).ToLower
                Dim TargetMod As String = ""
                Dim TargetModPath As String = ""
                Dim ModListTerm As String = ""

                'These command modes require two arguments to be passed, so re-check here and there. Optional arguments also lie there.
                Select Case CommandMode
                    Case "start", "stop", "info", "reload", "install", "uninstall"
                        If ListArgsOnly.Length > 1 Then
                            TargetMod = ListArgsOnly(1)
                            TargetModPath = NeutralizePath(TargetMod, GetKernelPath(KernelPathType.Mods))
                            If Not (TryParsePath(TargetModPath) AndAlso FileExists(TargetModPath)) Then
                                Write(DoTranslation("Mod not found or file has invalid characters."), True, GetConsoleColor(ColTypes.Error))
                                Exit Sub
                            End If
                        Else
                            Write(DoTranslation("Mod file is not specified."), True, GetConsoleColor(ColTypes.Error))
                            Exit Sub
                        End If
                    Case "list", "listparts"
                        If ListArgsOnly.Length > 1 Then
                            ModListTerm = ListArgsOnly(1)
                        End If
                End Select

                'Now, the actual logic
                Select Case CommandMode
                    Case "start"
                        Write(DoTranslation("Starting mod") + " {0}...", True, color:=GetConsoleColor(ColTypes.Neutral), Path.GetFileNameWithoutExtension(TargetMod))
                        StartMod(Path.GetFileName(TargetModPath))
                    Case "stop"
                        StopMod(Path.GetFileName(TargetModPath))
                    Case "info"
                        For Each script As String In Mods.Keys
                            If Mods(script).ModFilePath = TargetModPath Then
                                WriteSeparator(script, True)
                                Write("- " + DoTranslation("Mod name:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModName, True, GetConsoleColor(ColTypes.ListValue))
                                Write("- " + DoTranslation("Mod file name:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModFileName, True, GetConsoleColor(ColTypes.ListValue))
                                Write("- " + DoTranslation("Mod file path:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModFilePath, True, GetConsoleColor(ColTypes.ListValue))
                                Write("- " + DoTranslation("Mod version:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModVersion, True, GetConsoleColor(ColTypes.ListValue))
                                Write("- " + DoTranslation("Mod parts:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModParts.Count.ToString, True, GetConsoleColor(ColTypes.ListValue))
                                For Each ModPart As String In Mods(script).ModParts.Keys
                                    WriteSeparator("-- {0}", False, ModPart)
                                    Write("- " + DoTranslation("Part version:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModParts(ModPart).PartScript.Version, True, GetConsoleColor(ColTypes.ListValue))
                                    Write("- " + DoTranslation("Part file name:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModParts(ModPart).PartFileName, True, GetConsoleColor(ColTypes.ListValue))
                                    Write("- " + DoTranslation("Part file path:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModParts(ModPart).PartFilePath, True, GetConsoleColor(ColTypes.ListValue))
                                    If Mods(script).ModParts(ModPart).PartScript.Commands IsNot Nothing Then
                                        For Each ModCommand As String In Mods(script).ModParts(ModPart).PartScript.Commands.Keys
                                            WriteSeparator("--- {0}", False, ModCommand)
                                            Write("- " + DoTranslation("Command name:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(ModCommand, True, GetConsoleColor(ColTypes.ListValue))
                                            Write("- " + DoTranslation("Command definition:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModParts(ModPart).PartScript.Commands(ModCommand).HelpDefinition, True, GetConsoleColor(ColTypes.ListValue))
                                            Write("- " + DoTranslation("Command type:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModParts(ModPart).PartScript.Commands(ModCommand).Type.ToString, True, GetConsoleColor(ColTypes.ListValue))
                                            Write("- " + DoTranslation("Strict command?") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModParts(ModPart).PartScript.Commands(ModCommand).Strict.ToString, True, GetConsoleColor(ColTypes.ListValue))
                                            Write("- " + DoTranslation("Wrappable command?") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModParts(ModPart).PartScript.Commands(ModCommand).Wrappable.ToString, True, GetConsoleColor(ColTypes.ListValue))
                                            Write("- " + DoTranslation("Setting shell variable?") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModParts(ModPart).PartScript.Commands(ModCommand).SettingVariable.ToString, True, GetConsoleColor(ColTypes.ListValue))
                                            Write("- " + DoTranslation("Can not run in maintenance mode?") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModParts(ModPart).PartScript.Commands(ModCommand).NoMaintenance.ToString, True, GetConsoleColor(ColTypes.ListValue))
                                            Write("- " + DoTranslation("Obsolete?") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModParts(ModPart).PartScript.Commands(ModCommand).Obsolete.ToString, True, GetConsoleColor(ColTypes.ListValue))
                                            If Mods(script).ModParts(ModPart).PartScript.Commands(ModCommand).CommandArgumentInfo IsNot Nothing Then
                                                For Each Usage As String In Mods(script).ModParts(ModPart).PartScript.Commands(ModCommand).CommandArgumentInfo.HelpUsages
                                                    Write("- " + DoTranslation("Command usage:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Usage, True, GetConsoleColor(ColTypes.ListValue))
                                                Next
                                                Write("- " + DoTranslation("Arguments required?") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModParts(ModPart).PartScript.Commands(ModCommand).CommandArgumentInfo.ArgumentsRequired.ToString, True, GetConsoleColor(ColTypes.ListValue))
                                                Write("- " + DoTranslation("Minimum count of required arguments:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods(script).ModParts(ModPart).PartScript.Commands(ModCommand).CommandArgumentInfo.MinimumArguments.ToString, True, GetConsoleColor(ColTypes.ListValue))
                                            End If
                                        Next
                                    End If
                                Next
                            End If
                        Next
                    Case "reload"
                        ReloadMod(Path.GetFileName(TargetModPath))
                    Case "install"
                        InstallMod(TargetMod)
                    Case "uninstall"
                        UninstallMod(TargetMod)
                    Case "list"
                        For Each [Mod] As String In ListMods(ModListTerm).Keys
                            WriteSeparator([Mod], True)
                            Write("- " + DoTranslation("Mod name:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods([Mod]).ModName, True, GetConsoleColor(ColTypes.ListValue))
                            Write("- " + DoTranslation("Mod file name:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods([Mod]).ModFileName, True, GetConsoleColor(ColTypes.ListValue))
                            Write("- " + DoTranslation("Mod file path:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods([Mod]).ModFilePath, True, GetConsoleColor(ColTypes.ListValue))
                            Write("- " + DoTranslation("Mod version:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods([Mod]).ModVersion, True, GetConsoleColor(ColTypes.ListValue))
                            Write("- " + DoTranslation("Mod parts:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods([Mod]).ModParts.Count.ToString, True, GetConsoleColor(ColTypes.ListValue))
                        Next
                    Case "listparts"
                        Dim ModList As Dictionary(Of String, ModInfo) = ListMods(ModListTerm)
                        For Each [Mod] As String In ModList.Keys
                            For Each Part As String In ModList([Mod]).ModParts.Keys
                                WriteSeparator($"{[Mod]} > {Part}", True)
                                Write("- " + DoTranslation("Mod part name:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(ModList([Mod]).ModParts(Part).PartName, True, GetConsoleColor(ColTypes.ListValue))
                                Write("- " + DoTranslation("Mod part file name:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods([Mod]).ModParts(Part).PartFileName, True, GetConsoleColor(ColTypes.ListValue))
                                Write("- " + DoTranslation("Mod part file path:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Mods([Mod]).ModParts(Part).PartFilePath, True, GetConsoleColor(ColTypes.ListValue))
                            Next
                        Next
                    Case "reloadall"
                        ReloadMods()
                    Case "stopall"
                        StopMods()
                    Case "startall"
                        StartMods()
                    Case Else
                        Write(DoTranslation("Invalid command {0}. Check the usage below:"), True, color:=GetConsoleColor(ColTypes.Error), CommandMode)
                        ShowHelp("modman")
                End Select
            Else
                Write(DoTranslation("Mod management is disabled in safe mode."), True, GetConsoleColor(ColTypes.Error))
            End If
        End Sub

    End Class
End Namespace