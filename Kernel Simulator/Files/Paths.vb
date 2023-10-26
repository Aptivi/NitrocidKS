
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

Namespace Files
    Public Module Paths

        ''' <summary>
        ''' Platform-dependent home path
        ''' </summary>
        Public ReadOnly Property HomePath As String
            Get
                If IsOnUnix() Then
                    Return Environment.GetEnvironmentVariable("HOME")
                Else
                    Return Environment.GetEnvironmentVariable("USERPROFILE").Replace("\", "/")
                End If
            End Get
        End Property

        ''' <summary>
        ''' Platform-dependent temp path
        ''' </summary>
        Public ReadOnly Property TempPath As String
            Get
                If IsOnUnix() Then
                    Return "/tmp"
                Else
                    Return Environment.GetEnvironmentVariable("TEMP").Replace("\", "/")
                End If
            End Get
        End Property

        ''' <summary>
        ''' Retro Kernel Simulator download path
        ''' </summary>
        Public ReadOnly Property RetroKSDownloadPath As String
            Get
#If NETCOREAPP Then
                If IsOnUnix() Then
                    Return Environment.GetEnvironmentVariable("HOME") + "/.config/retroks/exec/coreclr"
                Else
                    Return (Environment.GetEnvironmentVariable("LOCALAPPDATA") + "/RetroKS/exec/coreclr").Replace("\", "/")
                End If
#Else
                If IsOnUnix() Then
                    Return Environment.GetEnvironmentVariable("HOME") + "/.config/retroks/exec/fx"
                Else
                    Return (Environment.GetEnvironmentVariable("LOCALAPPDATA") + "/RetroKS/exec/fx").Replace("\", "/")
                End If
#End If

            End Get
        End Property

        'Variables
        Friend KernelPaths As New Dictionary(Of String, String)

        ''' <summary>
        ''' Initializes the paths
        ''' </summary>
        Sub InitPaths()
            If Not KernelPaths.ContainsKey("Mods") Then KernelPaths.Add("Mods", HomePath + "/KSMods/")
            If Not KernelPaths.ContainsKey("Configuration") Then KernelPaths.Add("Configuration", HomePath + "/KernelConfig.json")
            If Not KernelPaths.ContainsKey("Debugging") Then KernelPaths.Add("Debugging", HomePath + "/kernelDbg.log")
            If Not KernelPaths.ContainsKey("Aliases") Then KernelPaths.Add("Aliases", HomePath + "/Aliases.json")
            If Not KernelPaths.ContainsKey("Users") Then KernelPaths.Add("Users", HomePath + "/Users.json")
            If Not KernelPaths.ContainsKey("FTPSpeedDial") Then KernelPaths.Add("FTPSpeedDial", HomePath + "/FTP_SpeedDial.json")
            If Not KernelPaths.ContainsKey("SFTPSpeedDial") Then KernelPaths.Add("SFTPSpeedDial", HomePath + "/SFTP_SpeedDial.json")
            If Not KernelPaths.ContainsKey("DebugDevNames") Then KernelPaths.Add("DebugDevNames", HomePath + "/DebugDeviceNames.json")
            If Not KernelPaths.ContainsKey("MOTD") Then KernelPaths.Add("MOTD", HomePath + "/MOTD.txt")
            If Not KernelPaths.ContainsKey("MAL") Then KernelPaths.Add("MAL", HomePath + "/MAL.txt")
            If Not KernelPaths.ContainsKey("CustomSaverSettings") Then KernelPaths.Add("CustomSaverSettings", HomePath + "/CustomSaverSettings.json")
            If Not KernelPaths.ContainsKey("Events") Then KernelPaths.Add("Events", HomePath + "/KSEvents/")
            If Not KernelPaths.ContainsKey("Reminders") Then KernelPaths.Add("Reminders", HomePath + "/KSReminders/")
            If Not KernelPaths.ContainsKey("CustomLanguages") Then KernelPaths.Add("CustomLanguages", HomePath + "/KSLanguages/")
            If Not KernelPaths.ContainsKey("CustomSplashes") Then KernelPaths.Add("CustomSplashes", HomePath + "/KSSplashes/")
        End Sub

        ''' <summary>
        ''' Gets the neutralized kernel path
        ''' </summary>
        ''' <param name="PathType">Kernel path type</param>
        ''' <returns>A kernel path</returns>
        ''' <exception cref="Exceptions.InvalidKernelPathException"></exception>
        Public Function GetKernelPath(PathType As KernelPathType) As String
            If [Enum].IsDefined(GetType(KernelPathType), PathType) Then
                Return NeutralizePath(KernelPaths(PathType.ToString))
            Else
                Throw New Exceptions.InvalidKernelPathException(DoTranslation("Invalid kernel path type."))
            End If
        End Function

    End Module
End Namespace
