﻿
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

Public Module PlatformDetector

    ''' <summary>
    ''' Is this system a Windows system?
    ''' </summary>
    Public Function IsOnWindows() As Boolean
        Return Environment.OSVersion.Platform = PlatformID.Win32NT
    End Function

    ''' <summary>
    ''' Is this system a Unix system? True for macOS, too!
    ''' </summary>
    Public Function IsOnUnix() As Boolean
        Return Environment.OSVersion.Platform = PlatformID.Unix
    End Function

    ''' <summary>
    ''' Is this system a macOS system?
    ''' </summary>
    Public Function IsOnMacOS() As Boolean
        If IsOnUnix() Then
            Dim UnameS As New Process
            Dim UnameSInfo As New ProcessStartInfo With {.FileName = "/usr/bin/uname", .Arguments = "-s",
                                                         .CreateNoWindow = True,
                                                         .UseShellExecute = False,
                                                         .WindowStyle = ProcessWindowStyle.Hidden,
                                                         .RedirectStandardOutput = True}
            UnameS.StartInfo = UnameSInfo
            UnameS.Start()
            UnameS.WaitForExit()
            Dim System As String = UnameS.StandardOutput.ReadToEnd
            Return System.Contains("Darwin")
        Else
            Return False
        End If
    End Function

End Module
