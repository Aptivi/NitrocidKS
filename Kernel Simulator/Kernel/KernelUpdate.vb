
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

Imports Newtonsoft.Json.Linq
Imports SemanVer.Instance

Namespace Kernel
    Public Class KernelUpdate

        ''' <summary>
        ''' Updated kernel version
        ''' </summary>
        Public ReadOnly Property UpdateVersion As SemVer
        ''' <summary>
        ''' Update file URL
        ''' </summary>
        Public ReadOnly Property UpdateURL As Uri
        ''' <summary>
        ''' Is the kernel up to date?
        ''' </summary>
        Public ReadOnly Property Updated As Boolean

        ''' <summary>
        ''' Installs a new instance of class KernelUpdate
        ''' </summary>
        ''' <param name="UpdateToken">The kernel update token</param>
        Protected Friend Sub New(UpdateToken As JToken)
            'Sort the versions (We sometimes release servicing versions of earlier series, like 0.0.8.x, and the GitHub API sorts the releases based
            'on the date of the release, so we retry sorting them, this time, by version, so we get the list in the below format.)
            '
            ' Before:
            '     [ 0.0.21.5, 0.0.19.5, 0.0.18.5, 0.0.17.7, 0.0.16.13, 0.0.12.8, 0.0.8.12, 0.0.21.4, 0.0.21.3, 0.0.20.6, 0.0.21.2, ... ]
            ' After:
            '     [ 0.0.21.5, 0.0.21.4, 0.0.21.3, 0.0.21.2, 0.0.21.1,  0.0.21.0, 0.0.20.6, 0.0.20.5, 0.0.20.4, 0.0.20.3, 0.0.20.2, ... ]
            '
            'After we do this, Kernel Simulator should recognize newer servicing versions based on the current series (i.e. KS 0.0.21.3 didn't notify
            'the user that 0.0.21.4 was available due to 0.0.8.12 and versions that came after coming as first according to the API until 0.0.21.5
            'arrived)
            Dim SortedVersions As New List(Of KernelUpdateInfo)
            For Each KernelUpdate As JToken In UpdateToken
                Dim tagName As String = KernelUpdate.SelectToken("tag_name").ToString()
                tagName = If(tagName.StartsWith("v"), tagName.Substring(1), tagName)
                Dim KernelUpdateVer As SemVer = Nothing
                If tagName.Split(".").Length > 3 Then
                    KernelUpdateVer = SemVer.ParseWithRev(tagName)
                Else
                    KernelUpdateVer = SemVer.Parse(tagName)
                End If
                Dim KernelUpdateURL = ""
                For Each asset In KernelUpdate.SelectToken("assets")
                    Dim url = CStr(asset("browser_download_url"))
#If NETCOREAPP Then
                    If url.EndsWith("-bin-dotnet.zip") OrElse url.EndsWith("-bin-dotnet.rar") Then
#Else
                    If url.EndsWith("-bin.zip") OrElse url.EndsWith("-bin.rar") Then
#End If
                        KernelUpdateURL = url
                        Exit For
                    End If
                Next
                Dim KernelUpdateInfo As New KernelUpdateInfo(KernelUpdateVer, KernelUpdateURL)
                SortedVersions.Add(KernelUpdateInfo)
            Next
            SortedVersions = SortedVersions.OrderByDescending(Function(x) x.UpdateVersion).ToList

            'Get the latest version found
            Dim CurrentVer As SemVer = SemVer.ParseWithRev(KernelVersion)
            Dim UpdateVer As SemVer = SortedVersions(0).UpdateVersion
            Dim UpdateURI As Uri = SortedVersions(0).UpdateURL
            Wdbg(DebugLevel.I, "Update version: {0}", UpdateVer.ToString)
            Wdbg(DebugLevel.I, "Update URL: {0}", UpdateURI.ToString)

            'Install the values
            UpdateVersion = UpdateVer
            UpdateURL = UpdateURI

            'If the updated version is lower or equal to the current version, consider the kernel up-to-date.
            Updated = UpdateVersion <= CurrentVer
        End Sub

    End Class
End Namespace
