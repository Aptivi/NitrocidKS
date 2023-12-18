
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

Imports System.IO
Imports System.Reflection
Imports System.Text
Imports KS.Files.Folders
Imports KS.Files.Operations
Imports KS.Files.Querying
Imports KS.TimeDate
Imports Terminaux.Inputs.Interactive
Imports Terminaux.Inputs.Styles.Infobox

Namespace Files.Interactive
    Public Class FileManagerCli
        Inherits BaseInteractiveTui
        Implements IInteractiveTui
        Friend firstPanePath As String = HomePath
        Friend secondPanePath As String = HomePath
        Friend refreshFirstPaneListing As Boolean = True
        Friend refreshSecondPaneListing As Boolean = True
        Private firstPaneListing As New List(Of FileSystemInfo)
        Private secondPaneListing As New List(Of FileSystemInfo)

        ''' <summary>
        ''' File manager bindings
        ''' </summary>
        Public Overrides Property Bindings As New List(Of InteractiveTuiBinding) From
        {
            New InteractiveTuiBinding("Open", ConsoleKey.Enter, Nothing, Sub(info, __) Open(CType(info, FileSystemInfo))),
            New InteractiveTuiBinding("Copy", ConsoleKey.F1, Nothing, Sub(info, __) CopyFileOrDir(CType(info, FileSystemInfo))),
            New InteractiveTuiBinding("Move", ConsoleKey.F2, Nothing, Sub(info, __) MoveFileOrDir(CType(info, FileSystemInfo))),
            New InteractiveTuiBinding("Delete", ConsoleKey.F3, Nothing, Sub(info, __) RemoveFileOrDir(CType(info, FileSystemInfo))),
            New InteractiveTuiBinding("Up", ConsoleKey.F4, Nothing, Sub(__, __) GoUp()),
            New InteractiveTuiBinding("Info", ConsoleKey.F5, Nothing, Sub(info, __) PrintFileSystemInfo(CType(info, FileSystemInfo))),
            New InteractiveTuiBinding("Go To", ConsoleKey.F6, Nothing, Sub(__, __) [GoTo]()),
            New InteractiveTuiBinding("Copy To", ConsoleKey.F1, ConsoleModifiers.Shift, Sub(info, __) CopyTo(CType(info, FileSystemInfo))),
            New InteractiveTuiBinding("Move To", ConsoleKey.F2, ConsoleModifiers.Shift, Sub(info, __) MoveTo(CType(info, FileSystemInfo))),
            New InteractiveTuiBinding("Rename", ConsoleKey.F9, Nothing, Sub(info, __) Rename(CType(info, FileSystemInfo))),
            New InteractiveTuiBinding("New Folder", ConsoleKey.F10, Nothing, Sub(__, __) MakeDir())
        }

        ''' <summary>
        ''' Always true in the file manager as we want it to behave like Total Commander
        ''' </summary>
        Public Overrides ReadOnly Property SecondPaneInteractable As Boolean
            Get
                Return True
            End Get
        End Property

        ''' <inheritdoc/>
        Public Overrides ReadOnly Property PrimaryDataSource As IEnumerable
            Get
                Try
                    If refreshFirstPaneListing Then
                        refreshFirstPaneListing = False
                        firstPaneListing = Listing.CreateList(firstPanePath, True)
                    End If
                    Return firstPaneListing
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Failed to get current directory list for the first pane [{0}]: {1}", firstPanePath, ex.Message)
                    WStkTrc(ex)
                    Return New List(Of FileSystemInfo)()
                End Try
            End Get
        End Property

        ''' <inheritdoc/>
        Public Overrides ReadOnly Property SecondaryDataSource As IEnumerable
            Get
                Try
                    If refreshSecondPaneListing Then
                        refreshSecondPaneListing = False
                        secondPaneListing = Listing.CreateList(secondPanePath, True)
                    End If
                    Return secondPaneListing
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Failed to get current directory list for the second pane [{0}]: {1}", secondPanePath, ex.Message)
                    WStkTrc(ex)
                    Return New List(Of FileSystemInfo)()
                End Try
            End Get
        End Property

        ''' <inheritdoc/>
        Public Overrides ReadOnly Property AcceptsEmptyData As Boolean
            Get
                Return True
            End Get
        End Property

        ''' <inheritdoc/>
        Public Overrides Sub RenderStatus(item As Object)
            Dim FileInfoCurrentPane As FileSystemInfo = CType(item, FileSystemInfo)

            ' Check to see if we're given the file system info
            If FileInfoCurrentPane Is Nothing Then
                InteractiveTuiStatus.Status = DoTranslation("No info.")
                Return
            End If

            ' Now, populate the info to the status
            Try
                Dim infoIsDirectory = FolderExists(FileInfoCurrentPane.FullName)
                InteractiveTuiStatus.Status = $"[{If(infoIsDirectory, "/", "*")}] {FileInfoCurrentPane.Name}"
            Catch ex As Exception
                InteractiveTuiStatus.Status = ex.Message
            End Try
        End Sub

        ''' <inheritdoc/>
        Public Overrides Function GetEntryFromItem(item As Object) As String
            Try
                Dim file As FileSystemInfo = CType(item, FileSystemInfo)
                Dim isDirectory = FolderExists(file.FullName)
                Return $" [{If(isDirectory, "/", "*")}] {file.Name}"
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to get entry: {0}", ex.Message)
                WStkTrc(ex)
                Return ""
            End Try
        End Function

        Private Shared Sub Open(currentFileSystemInfo As FileSystemInfo)
            Try
                ' Don't do anything if we haven't been provided anything.
                If currentFileSystemInfo Is Nothing Then Return

                ' Check for existence
                If Not currentFileSystemInfo.Exists Then Return

                ' Now that the selected file or folder exists, check the type.
                If FolderExists(currentFileSystemInfo.FullName) Then
                    ' We're dealing with a folder. Open it in the selected pane.
                    If InteractiveTuiStatus.CurrentPane = 2 Then
                        CType(Instance, FileManagerCli).secondPanePath = NeutralizePath(currentFileSystemInfo.FullName.ToString() & "/")
                        InteractiveTuiStatus.SecondPaneCurrentSelection = 1
                        CType(Instance, FileManagerCli).refreshSecondPaneListing = True
                    Else
                        CType(Instance, FileManagerCli).firstPanePath = NeutralizePath(currentFileSystemInfo.FullName.ToString() & "/")
                        InteractiveTuiStatus.FirstPaneCurrentSelection = 1
                        CType(Instance, FileManagerCli).refreshFirstPaneListing = True
                    End If
                End If
            Catch ex As Exception
                Dim finalInfoRendered = New StringBuilder()
                finalInfoRendered.AppendLine(DoTranslation("Can't open file or folder") + FormatString(": {0}", ex.Message))
                finalInfoRendered.AppendLine(vbLf & DoTranslation("Press any key to close this window.").ToString())
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString())
            End Try
        End Sub

        Private Shared Sub GoUp()
            If InteractiveTuiStatus.CurrentPane = 2 Then
                CType(Instance, FileManagerCli).secondPanePath = NeutralizePath(CType(Instance, FileManagerCli).secondPanePath & "/..")
                InteractiveTuiStatus.SecondPaneCurrentSelection = 1
                CType(Instance, FileManagerCli).refreshSecondPaneListing = True
            Else
                CType(Instance, FileManagerCli).firstPanePath = NeutralizePath(CType(Instance, FileManagerCli).firstPanePath & "/..")
                InteractiveTuiStatus.FirstPaneCurrentSelection = 1
                CType(Instance, FileManagerCli).refreshFirstPaneListing = True
            End If
        End Sub

        Private Shared Sub PrintFileSystemInfo(currentFileSystemInfo As FileSystemInfo)
            ' Don't do anything if we haven't been provided anything.
            If currentFileSystemInfo Is Nothing Then Return

            ' .NET managed info
            Dim asmName As AssemblyName = Nothing

            ' Render the final information string
            Try
                Dim finalInfoRendered = New StringBuilder()
                Dim fullPath As String = currentFileSystemInfo.FullName
                If FolderExists(fullPath) Then
                    ' The file system info instance points to a folder
                    Dim DirInfo = New DirectoryInfo(fullPath)
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("Name: {0}"), DirInfo.Name))
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("Full name: {0}"), NeutralizePath(DirInfo.FullName)))
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("Size: {0}"), GetAllSizesInFolder(DirInfo)))
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("Creation time: {0}"), Render(DirInfo.CreationTime)))
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("Last access time: {0}"), Render(DirInfo.LastAccessTime)))
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("Last write time: {0}"), Render(DirInfo.LastWriteTime)))
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("Attributes: {0}"), DirInfo.Attributes))

                    ' The file system info instance points to a file
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("Parent directory: {0}"), NeutralizePath(DirInfo.Parent.FullName)))
                Else
                    Dim fileInfo As New FileInfo(fullPath)
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("Name: {0}"), fileInfo.Name))
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("Full name: {0}"), NeutralizePath(fileInfo.FullName)))
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("File size: {0}"), fileInfo.Length))
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("Creation time: {0}"), Render(fileInfo.CreationTime)))
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("Last access time: {0}"), Render(fileInfo.LastAccessTime)))
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("Last write time: {0}"), Render(fileInfo.LastWriteTime)))
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("Attributes: {0}"), fileInfo.Attributes))
                    finalInfoRendered.AppendLine(FormatString(DoTranslation("Where to find: {0}"), NeutralizePath(fileInfo.DirectoryName)))
                End If
                finalInfoRendered.AppendLine(vbLf & DoTranslation("Press any key to close this window.").ToString())

                ' Now, render the info box
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString())
            Catch ex As Exception
                Dim finalInfoRendered = New StringBuilder()
                finalInfoRendered.AppendLine(DoTranslation("Can't get file system info") + FormatString(": {0}", ex.Message))
                finalInfoRendered.AppendLine(vbLf & DoTranslation("Press any key to close this window.").ToString())
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString())
            End Try
        End Sub

        Private Shared Sub CopyFileOrDir(currentFileSystemInfo As FileSystemInfo)
            ' Don't do anything if we haven't been provided anything.
            If currentFileSystemInfo Is Nothing Then Return

            Try
                Dim dest As String = If(InteractiveTuiStatus.CurrentPane = 2, CType(Instance, FileManagerCli).firstPanePath, CType(Instance, FileManagerCli).secondPanePath).ToString() & "/"
                Wdbg(DebugLevel.I, $"Destination is {dest}")
                Copying.CopyFileOrDir(currentFileSystemInfo.FullName, dest)
                If InteractiveTuiStatus.CurrentPane = 2 Then
                    CType(Instance, FileManagerCli).refreshFirstPaneListing = True
                Else
                    CType(Instance, FileManagerCli).refreshSecondPaneListing = True
                End If
            Catch ex As Exception
                Dim finalInfoRendered = New StringBuilder()
                finalInfoRendered.AppendLine(DoTranslation("Can't copy file or directory") + FormatString(": {0}", ex.Message))
                finalInfoRendered.AppendLine(vbLf & DoTranslation("Press any key to close this window.").ToString())
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString())
            End Try
        End Sub

        Private Shared Sub MoveFileOrDir(currentFileSystemInfo As FileSystemInfo)
            ' Don't do anything if we haven't been provided anything.
            If currentFileSystemInfo Is Nothing Then Return

            Try
                Dim dest As String = If(InteractiveTuiStatus.CurrentPane = 2, CType(Instance, FileManagerCli).firstPanePath, CType(Instance, FileManagerCli).secondPanePath).ToString() & "/"
                Wdbg(DebugLevel.I, $"Destination is {dest}")
                Moving.MoveFileOrDir(currentFileSystemInfo.FullName, dest)
                CType(Instance, FileManagerCli).refreshSecondPaneListing = True
                CType(Instance, FileManagerCli).refreshFirstPaneListing = True
            Catch ex As Exception
                Dim finalInfoRendered = New StringBuilder()
                finalInfoRendered.AppendLine(DoTranslation("Can't move file or directory") + FormatString(": {0}", ex.Message))
                finalInfoRendered.AppendLine(vbLf & DoTranslation("Press any key to close this window.").ToString())
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString())
            End Try
        End Sub

        Private Shared Sub RemoveFileOrDir(currentFileSystemInfo As FileSystemInfo)
            ' Don't do anything if we haven't been provided anything.
            If currentFileSystemInfo Is Nothing Then Return

            Try
                If FolderExists(currentFileSystemInfo.FullName) Then
                    RemoveDirectory(currentFileSystemInfo.FullName)
                Else
                    RemoveFile(currentFileSystemInfo.FullName)
                End If
                If InteractiveTuiStatus.CurrentPane = 2 Then
                    CType(Instance, FileManagerCli).refreshSecondPaneListing = True
                Else
                    CType(Instance, FileManagerCli).refreshFirstPaneListing = True
                End If
            Catch ex As Exception
                Dim finalInfoRendered = New StringBuilder()
                finalInfoRendered.AppendLine(DoTranslation("Can't remove file or directory") + FormatString(": {0}", ex.Message))
                finalInfoRendered.AppendLine(vbLf & DoTranslation("Press any key to close this window.").ToString())
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString())
            End Try
        End Sub

        Private Shared Sub [GoTo]()
            ' Now, render the search box
            Dim path As String = InfoBoxInputColor.WriteInfoBoxInput(DoTranslation("Enter a path or a full path to a local folder."))
            path = NeutralizePath(path, CType(Instance, FileManagerCli).firstPanePath)
            If FolderExists(path) Then
                InteractiveTuiStatus.FirstPaneCurrentSelection = 1
                CType(Instance, FileManagerCli).firstPanePath = path
                CType(Instance, FileManagerCli).refreshFirstPaneListing = True
            Else
                InfoBoxColor.WriteInfoBox(DoTranslation("Folder doesn't exist. Make sure that you've written the correct path."))
            End If
        End Sub

        Private Shared Sub CopyTo(currentFileSystemInfo As FileSystemInfo)
            ' Don't do anything if we haven't been provided anything.
            If currentFileSystemInfo Is Nothing Then Return

            Try
                Dim path As String = InfoBoxInputColor.WriteInfoBoxInput(DoTranslation("Enter a path or a full path to a destination folder to copy the selected file to."))
                path = NeutralizePath(path, If(InteractiveTuiStatus.CurrentPane = 2, CType(Instance, FileManagerCli).secondPanePath, CType(Instance, FileManagerCli).firstPanePath)).ToString() & "/"
                Wdbg(DebugLevel.I, $"Destination is {path}")
                If FolderExists(path) Then
                    If Parsing.TryParsePath(path) Then
                        Copying.CopyFileOrDir(currentFileSystemInfo.FullName, path)
                        If InteractiveTuiStatus.CurrentPane = 2 Then
                            CType(Instance, FileManagerCli).refreshFirstPaneListing = True
                        Else
                            CType(Instance, FileManagerCli).refreshSecondPaneListing = True
                        End If
                    Else
                        InfoBoxColor.WriteInfoBox(DoTranslation("Make sure that you've written the correct path."))
                    End If
                Else
                    InfoBoxColor.WriteInfoBox(DoTranslation("File doesn't exist. Make sure that you've written the correct path."))
                End If
            Catch ex As Exception
                Dim finalInfoRendered = New StringBuilder()
                finalInfoRendered.AppendLine(DoTranslation("Can't copy file or directory") + FormatString(": {0}", ex.Message))
                finalInfoRendered.AppendLine(vbLf & DoTranslation("Press any key to close this window.").ToString())
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString())
            End Try
        End Sub

        Private Shared Sub MoveTo(currentFileSystemInfo As FileSystemInfo)
            ' Don't do anything if we haven't been provided anything.
            If currentFileSystemInfo Is Nothing Then Return

            Try
                Dim path As String = InfoBoxInputColor.WriteInfoBoxInput(DoTranslation("Enter a path or a full path to a destination folder to move the selected file to."))
                path = NeutralizePath(path, If(InteractiveTuiStatus.CurrentPane = 2, CType(Instance, FileManagerCli).secondPanePath, CType(Instance, FileManagerCli).firstPanePath)).ToString() & "/"
                Wdbg(DebugLevel.I, $"Destination is {path}")
                If FolderExists(path) Then
                    If Parsing.TryParsePath(path) Then
                        Moving.MoveFileOrDir(currentFileSystemInfo.FullName, path)
                        CType(Instance, FileManagerCli).refreshSecondPaneListing = True
                        CType(Instance, FileManagerCli).refreshFirstPaneListing = True
                    Else
                        InfoBoxColor.WriteInfoBox(DoTranslation("Make sure that you've written the correct path."))
                    End If
                Else
                    InfoBoxColor.WriteInfoBox(DoTranslation("File doesn't exist. Make sure that you've written the correct path."))
                End If
            Catch ex As Exception
                Dim finalInfoRendered = New StringBuilder()
                finalInfoRendered.AppendLine(DoTranslation("Can't move file or directory") + FormatString(": {0}", ex.Message))
                finalInfoRendered.AppendLine(vbLf & DoTranslation("Press any key to close this window.").ToString())
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString())
            End Try
        End Sub

        Private Shared Sub Rename(currentFileSystemInfo As FileSystemInfo)
            ' Don't do anything if we haven't been provided anything.
            If currentFileSystemInfo Is Nothing Then Return

            Try
                Dim filename As String = InfoBoxInputColor.WriteInfoBoxInput(DoTranslation("Enter a new file name."))
                Wdbg(DebugLevel.I, $"New filename is {filename}")
                If Not FileExists(filename) Then
                    If Parsing.TryParseFileName(filename) Then
                        Moving.MoveFileOrDir(currentFileSystemInfo.FullName, Path.GetDirectoryName(currentFileSystemInfo.FullName).ToString() & $"/{filename}")
                        If InteractiveTuiStatus.CurrentPane = 2 Then
                            CType(Instance, FileManagerCli).refreshSecondPaneListing = True
                        Else
                            CType(Instance, FileManagerCli).refreshFirstPaneListing = True
                        End If
                    Else
                        InfoBoxColor.WriteInfoBox(DoTranslation("Make sure that you've written the correct file name."))
                    End If
                Else
                    InfoBoxColor.WriteInfoBox(DoTranslation("File already exists. The name shouldn't be occupied by another file."))
                End If
            Catch ex As Exception
                Dim finalInfoRendered = New StringBuilder()
                finalInfoRendered.AppendLine(DoTranslation("Can't move file or directory") + FormatString(": {0}", ex.Message))
                finalInfoRendered.AppendLine(vbLf & DoTranslation("Press any key to close this window.").ToString())
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString())
            End Try
        End Sub

        Private Shared Sub MakeDir()
            ' Now, render the search box
            Dim path As String = InfoBoxInputColor.WriteInfoBoxInput(DoTranslation("Enter a new directory name."))
            path = NeutralizePath(path, If(InteractiveTuiStatus.CurrentPane = 2, CType(Instance, FileManagerCli).secondPanePath, CType(Instance, FileManagerCli).firstPanePath))
            If Not FolderExists(path) Then
                TryMakeDirectory(path)
                If InteractiveTuiStatus.CurrentPane = 2 Then
                    CType(Instance, FileManagerCli).refreshSecondPaneListing = True
                Else
                    CType(Instance, FileManagerCli).refreshFirstPaneListing = True
                End If
            Else
                InfoBoxColor.WriteInfoBox(DoTranslation("Folder already exists. The name shouldn't be occupied by another folder."))
            End If
        End Sub
    End Class
End Namespace

