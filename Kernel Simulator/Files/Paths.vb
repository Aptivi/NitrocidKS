
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

        'Variables
        Friend KernelPaths As New Dictionary(Of String, String)
        Friend pathsInitialized As Boolean = False

        ''' <summary>
        ''' Initializes the paths
        ''' </summary>
        Sub InitPaths()
            If pathsInitialized Then Return
            KernelPaths.Add("Mods", HomePath + "/KSMods/")
            KernelPaths.Add("Configuration", HomePath + "/KernelConfig.json")
            KernelPaths.Add("Debugging", HomePath + "/kernelDbg.log")
            KernelPaths.Add("Aliases", HomePath + "/Aliases.json")
            KernelPaths.Add("Users", HomePath + "/Users.json")
            KernelPaths.Add("FTPSpeedDial", HomePath + "/FTP_SpeedDial.json")
            KernelPaths.Add("SFTPSpeedDial", HomePath + "/SFTP_SpeedDial.json")
            KernelPaths.Add("DebugDevNames", HomePath + "/DebugDeviceNames.json")
            KernelPaths.Add("MOTD", HomePath + "/MOTD.txt")
            KernelPaths.Add("MAL", HomePath + "/MAL.txt")
            KernelPaths.Add("CustomSaverSettings", HomePath + "/CustomSaverSettings.json")
            KernelPaths.Add("Events", HomePath + "/KSEvents/")
            KernelPaths.Add("Reminders", HomePath + "/KSReminders/")
            KernelPaths.Add("CustomLanguages", HomePath + "/KSLanguages/")
            pathsInitialized = True
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