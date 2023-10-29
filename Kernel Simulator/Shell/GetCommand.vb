
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
Imports System.IO.Compression
Imports System.Net.NetworkInformation
Imports System.Text
Imports System.Threading

Public Module GetCommand

    Public StartCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "Shell Command Thread"}

    ''' <summary>
    ''' Executes a command
    ''' </summary>
    ''' <param name="requestedCommand">A command. It may contain arguments</param>
    Public Sub ExecuteCommand(requestedCommand As String)
        'Variables
        Dim Command As String
        Dim RequiredArgumentsProvided As Boolean = True

        '1. Get the index of the first space (Used for step 3)
        Dim index As Integer = requestedCommand.IndexOf(" ")
        If index = -1 Then index = requestedCommand.Length
        Wdbg("I", "Index: {0}", index)

        '2. Split the requested command string into words
        Dim words() As String = requestedCommand.Split({" "c})
        For i As Integer = 0 To words.Length - 1
            Wdbg("I", "Word {0}: {1}", i + 1, words(i))
        Next
        Command = words(0)

        '3. Get the string of arguments
        Dim strArgs As String = requestedCommand.Substring(index)
        Wdbg("I", "Prototype strArgs: {0}", strArgs)
        If Not index = requestedCommand.Length Then strArgs = strArgs.Substring(1)
        Wdbg("I", "Finished strArgs: {0}", strArgs)

        '4. Split the arguments with enclosed quotes and set the required boolean variable
        Dim eqargs() As String = strArgs.SplitEncloseDoubleQuotes()
        If eqargs IsNot Nothing Then
            RequiredArgumentsProvided = eqargs?.Length >= Commands(Command).MinimumArguments
        ElseIf Commands(Command).ArgumentsRequired And eqargs Is Nothing Then
            RequiredArgumentsProvided = False
        End If

        '4a. Debug: get all arguments from eqargs()
        If eqargs IsNot Nothing Then Wdbg("I", "Arguments parsed from eqargs(): " + String.Join(", ", eqargs))

        '5. Check to see if a requested command is obsolete
        If Commands(Command).Obsolete Then
            Wdbg("I", "The command requested {0} is obsolete", Command)
            Write(DoTranslation("This command is obsolete and will be removed in a future release."), True, ColTypes.Neutral)
        End If

        '6. Execute a command
        Try
            Select Case Command
                Case "help"

                    If requestedCommand = "help" Then
                        ShowHelp()
                    Else
                        If eqargs IsNot Nothing Then
                            ShowHelp(eqargs(0))
                        Else
                            ShowHelp()
                        End If
                    End If

                Case "adduser"

                    If RequiredArgumentsProvided Then
                        If eqargs?.Length = 1 Then
                            Write(DoTranslation("usrmgr: Creating username {0}..."), True, ColTypes.Neutral, eqargs(0))
                            AddUser(eqargs(0))
                        ElseIf eqargs?.Length > 2 Then
                            If eqargs(1) = eqargs(2) Then
                                Write(DoTranslation("usrmgr: Creating username {0}..."), True, ColTypes.Neutral, eqargs(0))
                                AddUser(eqargs(0), eqargs(1))
                            Else
                                Write(DoTranslation("Passwords don't match."), True, ColTypes.Error)
                            End If
                        End If
                    End If

                Case "alias"

                    If RequiredArgumentsProvided Then
                        If eqargs?.Length > 3 Then
                            If eqargs(0) = "add" And [Enum].IsDefined(GetType(AliasType), eqargs(1)) Then
                                ManageAlias(eqargs(0), Val([Enum].Parse(GetType(AliasType), eqargs(1))), eqargs(2), eqargs(3))
                            Else
                                Write(DoTranslation("Invalid type {0}."), True, ColTypes.Error, eqargs(1))
                            End If
                        ElseIf eqargs?.Length = 3 Then
                            If eqargs(0) = "rem" And [Enum].IsDefined(GetType(AliasType), eqargs(1)) Then
                                ManageAlias(eqargs(0), Val([Enum].Parse(GetType(AliasType), eqargs(1))), eqargs(2))
                            Else
                                Write(DoTranslation("Invalid type {0}."), True, ColTypes.Error, eqargs(1))
                            End If
                        End If
                    End If

                Case "arginj"

                    'Argument Injection
                    If RequiredArgumentsProvided Then
                        Dim FinalArgs As New List(Of String)
                        For Each arg As String In eqargs
                            Wdbg("I", "Parsing argument {0}...", arg)
                            If AvailableArgs.Contains(arg) Then
                                Wdbg("I", "Adding argument {0}...", arg)
                                FinalArgs.Add(arg)
                            Else
                                Wdbg("W", "Argument {0} not found.", arg)
                                Write(DoTranslation("Argument {0} not found to inject."), True, ColTypes.Warning, arg)
                            End If
                        Next
                        If FinalArgs.Count = 0 Then
                            Write(DoTranslation("No arguments specified. Hint: Specify multiple arguments separated by spaces"), True, ColTypes.Error)
                        Else
                            EnteredArguments = New List(Of String)(FinalArgs)
                            argsInjected = True
                            Write(DoTranslation("Injected arguments, {0}, will be scheduled to run at next reboot."), True, ColTypes.Neutral, String.Join(", ", EnteredArguments))
                        End If
                    End If

                Case "beep"

                    If RequiredArgumentsProvided Then
                        If eqargs(0).IsNumeric And CInt(eqargs(0)) >= 37 And CInt(eqargs(0)) <= 32767 Then 'Frequency must be numeric, and must be >= 37 and <= 32767
                            If eqargs(1).IsNumeric Then 'Time must be numeric
                                Console.Beep(eqargs(0), eqargs(1))
                            Else
                                Write(DoTranslation("Time must be numeric."), True, ColTypes.Error)
                            End If
                        Else
                            Write(DoTranslation("Frequency must be numeric. If it's numeric, ensure that it is >= 37 and <= 32767."), True, ColTypes.Error)
                        End If
                    End If

                Case "blockdbgdev"

                    If RequiredArgumentsProvided Then
                        If Not RDebugBlocked.Contains(eqargs(0)) Then
                            If AddToBlockList(eqargs(0)) Then
                                Write(DoTranslation("{0} can't join remote debug now."), True, ColTypes.Neutral, eqargs(0))
                            Else
                                Write(DoTranslation("Failed to block {0}."), True, ColTypes.Neutral, eqargs(0))
                            End If
                        Else
                            Write(DoTranslation("{0} is already blocked."), True, ColTypes.Neutral, eqargs(0))
                        End If
                    End If

                Case "cat"

                    If RequiredArgumentsProvided Then
                        Try
                            ReadContents(eqargs(0))
                        Catch ex As Exception
                            WStkTrc(ex)
                            Write(ex.Message, True, ColTypes.Error)
                        End Try
                    End If

                Case "calc"

                    If RequiredArgumentsProvided Then
                        Try
                            Dim Res As String = Evaluate(strArgs)
                            Wdbg("I", "Res = {0}", Res)
                            If Res = "" Then 'If there is an error in calculation
                                Write(DoTranslation("Error in calculation."), True, ColTypes.Error)
                            Else 'Calculation done
                                Write(strArgs + " = " + Res, True, ColTypes.Neutral)
                            End If
                        Catch ex As Exception
                            WStkTrc(ex)
                            Write(DoTranslation("Error in calculation."), True, ColTypes.Error)
                        End Try
                    End If

                Case "cdbglog"

                    If DebugMode Then
                        Try
                            dbgWriter.Close()
                            dbgWriter = New StreamWriter(paths("Debugging")) With {.AutoFlush = True}
                            Write(DoTranslation("Debug log removed. All connected debugging devices may still view messages."), True, ColTypes.Neutral)
                        Catch ex As Exception
                            Write(DoTranslation("Debug log removal failed: {0}"), True, ColTypes.Error, ex.Message)
                            WStkTrc(ex)
                        End Try
                    Else
                        Write(DoTranslation("You must turn on debug mode before you can clear debug log."), True, ColTypes.Neutral)
                    End If

                Case "chattr"

                    If RequiredArgumentsProvided Then
                        Dim NeutralizedFilePath As String = NeutralizePath(eqargs(0))
                        If File.Exists(NeutralizedFilePath) Then
                            If eqargs(1).EndsWith("Normal") Or eqargs(1).EndsWith("ReadOnly") Or eqargs(1).EndsWith("Hidden") Or eqargs(1).EndsWith("Archive") Then
                                If eqargs(1).StartsWith("+") Then
                                    Dim Attrib As FileAttributes = [Enum].Parse(GetType(FileAttributes), eqargs(1).Remove(0, 1))
                                    If AddAttributeToFile(NeutralizedFilePath, Attrib) Then
                                        Write(DoTranslation("Attribute has been added successfully."), True, ColTypes.Neutral, eqargs(1))
                                    Else
                                        Write(DoTranslation("Failed to add attribute."), True, ColTypes.Neutral, eqargs(1))
                                    End If
                                ElseIf eqargs(1).StartsWith("-") Then
                                    Dim Attrib As FileAttributes = [Enum].Parse(GetType(FileAttributes), eqargs(1).Remove(0, 1))
                                    If RemoveAttributeFromFile(NeutralizedFilePath, Attrib) Then
                                        Write(DoTranslation("Attribute has been removed successfully."), True, ColTypes.Neutral, eqargs(1))
                                    Else
                                        Write(DoTranslation("Failed to remove attribute."), True, ColTypes.Neutral, eqargs(1))
                                    End If
                                End If
                            Else
                                Write(DoTranslation("Attribute ""{0}"" is invalid."), True, ColTypes.Error, eqargs(1))
                            End If
                        Else
                            Write(DoTranslation("File not found."), True, ColTypes.Error)
                        End If
                    End If

                Case "chdir"

                    If RequiredArgumentsProvided Then
                        Try
                            SetCurrDir(eqargs(0))
                        Catch sex As Security.SecurityException
                            Wdbg("E", "Security error: {0} ({1})", sex.Message, sex.PermissionType)
                            Write(DoTranslation("You are unauthorized to set current directory to {0}: {1}"), True, ColTypes.Error, eqargs(0), sex.Message)
                            WStkTrc(sex)
                        Catch ptlex As PathTooLongException
                            Wdbg("I", "Directory length: {0}", NeutralizePath(eqargs(0)).Length)
                            Write(DoTranslation("The path you've specified is too long."), True, ColTypes.Error)
                            WStkTrc(ptlex)
                        Catch ex As Exception
                            Write(DoTranslation("Changing directory has failed: {0}"), True, ColTypes.Error, ex.Message)
                            WStkTrc(ex)
                        End Try
                    End If

                Case "chhostname"

                    If RequiredArgumentsProvided Then
                        If eqargs(0) = "" Then
                            Write(DoTranslation("Blank host name."), True, ColTypes.Error)
                        ElseIf eqargs(0).IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
                            Write(DoTranslation("Special characters are not allowed."), True, ColTypes.Error)
                        Else
                            Write(DoTranslation("Changing from: {0} to {1}..."), True, ColTypes.Neutral, HName, eqargs(0))
                            ChangeHostname(eqargs(0))
                        End If
                    End If

                Case "chlang"

                    If RequiredArgumentsProvided Then
                        PromptForSetLang(eqargs(0))
                    End If

                Case "chmotd"

                    If eqargs?.Length > 0 Then
                        If strArgs = "" Then
                            Write(DoTranslation("Blank message of the day."), True, ColTypes.Error)
                        Else
                            Write(DoTranslation("Changing MOTD..."), True, ColTypes.Neutral)
                            SetMOTD(strArgs, MessageType.MOTD)
                        End If
                    Else
                        InitializeTextShell(paths("MOTD"))
                        Write(DoTranslation("Changing MOTD..."), True, ColTypes.Neutral)
                        ReadMOTDFromFile(MessageType.MOTD)
                    End If

                Case "chmal"

                    If eqargs?.Length > 0 Then
                        If strArgs = "" Then
                            Write(DoTranslation("Blank MAL After Login."), True, ColTypes.Error)
                        Else
                            Write(DoTranslation("Changing MAL..."), True, ColTypes.Neutral)
                            SetMOTD(strArgs, MessageType.MAL)
                        End If
                    Else
                        InitializeTextShell(paths("MAL"))
                        Write(DoTranslation("Changing MAL..."), True, ColTypes.Neutral)
                        ReadMOTDFromFile(MessageType.MAL)
                    End If

                Case "choice"

                    If RequiredArgumentsProvided Then
                        If eqargs(0).StartsWith("-") Then
                            Dim OutputType As ChoiceOutputType
                            If eqargs(0) = "-o" Or eqargs(0) = "-t" Or eqargs(0) = "-m" Then
                                If eqargs(0) = "-o" Then OutputType = ChoiceOutputType.OneLine
                                If eqargs(0) = "-t" Then OutputType = ChoiceOutputType.TwoLines
                                If eqargs(0) = "-m" Then OutputType = ChoiceOutputType.Modern
                                PromptChoice(eqargs(3), eqargs(1), eqargs(2), OutputType)
                            Else
                                RequiredArgumentsProvided = False
                            End If
                        Else
                            PromptChoice(eqargs(2), eqargs(0), eqargs(1))
                        End If
                    End If

                Case "chpwd"

                    If RequiredArgumentsProvided Then
                        Try
                            If InStr(eqargs(3), " ") > 0 Then
                                Write(DoTranslation("Spaces are not allowed."), True, ColTypes.Error)
                            ElseIf eqargs(3) = eqargs(2) Then
                                ChangePassword(eqargs(0), eqargs(1), eqargs(2))
                            ElseIf eqargs(3) <> eqargs(2) Then
                                Write(DoTranslation("Passwords doesn't match."), True, ColTypes.Error)
                            End If
                        Catch ex As Exception
                            Write(DoTranslation("Failed to change password of username: {0}"), True, ColTypes.Error, ex.Message)
                            WStkTrc(ex)
                        End Try
                    End If

                Case "chusrname"

                    If RequiredArgumentsProvided Then
                        ChangeUsername(eqargs(0), eqargs(1))
                        Write(DoTranslation("Username has been changed to {0}!"), True, ColTypes.Neutral, eqargs(1))
                        If eqargs(0) = signedinusrnm Then
                            LogoutRequested = True
                        End If
                    End If

                Case "cls"

                    Console.Clear()

                Case "copy"

                    If RequiredArgumentsProvided Then
                        CopyFileOrDir(eqargs(0), eqargs(1))
                    End If

                Case "dirinfo"

                    If RequiredArgumentsProvided Then
                        For Each Dir As String In eqargs
                            Dim DirectoryPath As String = NeutralizePath(Dir)
                            Wdbg("I", "Neutralized directory path: {0} ({1})", DirectoryPath, Directory.Exists(DirectoryPath))
                            WriteSeparator(Dir, True, ColTypes.Stage)
                            If Directory.Exists(DirectoryPath) Then
                                Dim DirInfo As New DirectoryInfo(DirectoryPath)
                                Write(DoTranslation("Name: {0}"), True, ColTypes.Neutral, DirInfo.Name)
                                Write(DoTranslation("Full name: {0}"), True, ColTypes.Neutral, NeutralizePath(DirInfo.FullName))
                                Write(DoTranslation("Size: {0}"), True, ColTypes.Neutral, GetAllSizesInFolder(DirInfo).FileSizeToString)
                                Write(DoTranslation("Creation time: {0}"), True, ColTypes.Neutral, Render(DirInfo.CreationTime))
                                Write(DoTranslation("Last access time: {0}"), True, ColTypes.Neutral, Render(DirInfo.LastAccessTime))
                                Write(DoTranslation("Last write time: {0}"), True, ColTypes.Neutral, Render(DirInfo.LastWriteTime))
                                Write(DoTranslation("Attributes: {0}"), True, ColTypes.Neutral, DirInfo.Attributes)
                                Write(DoTranslation("Parent directory: {0}"), True, ColTypes.Neutral, NeutralizePath(DirInfo.Parent.FullName))
                            Else
                                Write(DoTranslation("Can't get information about nonexistent directory."), True, ColTypes.Error)
                            End If
                        Next
                    End If

                Case "disconndbgdev"

                    If RequiredArgumentsProvided Then
                        DisconnectDbgDev(eqargs(0))
                        Write(DoTranslation("Device {0} disconnected."), True, ColTypes.Neutral, eqargs(0))
                    End If

                Case "dismissnotif"

                    If RequiredArgumentsProvided Then
                        Dim NotifIndex As Integer = eqargs(0) - 1
                        If NotifDismiss(NotifIndex) Then
                            Write(DoTranslation("Notification dismissed successfully."), True, ColTypes.Neutral)
                        Else
                            Write(DoTranslation("Error trying to dismiss notification."), True, ColTypes.Error)
                        End If
                    End If

                Case "echo"

                    Write(ProbePlaces(strArgs), True, ColTypes.Neutral)

                Case "edit"

                    If RequiredArgumentsProvided Then
                        eqargs(0) = NeutralizePath(eqargs(0))
                        Wdbg("I", "File path is {0} and .Exists is {0}", eqargs(0), File.Exists(eqargs(0)))
                        If File.Exists(eqargs(0)) Then
                            InitializeTextShell(eqargs(0))
                        Else
                            Write(DoTranslation("File doesn't exist."), True, ColTypes.Error)
                        End If
                    End If

                Case "fileinfo"

                    If RequiredArgumentsProvided Then
                        For Each FileName As String In eqargs
                            Dim FilePath As String = NeutralizePath(FileName)
                            Wdbg("I", "Neutralized file path: {0} ({1})", FilePath, File.Exists(FilePath))
                            WriteSeparator(FileName, True, ColTypes.Stage)
                            If File.Exists(FilePath) Then
                                Dim FileInfo As New FileInfo(FilePath)
                                Write(DoTranslation("Name: {0}"), True, ColTypes.Neutral, FileInfo.Name)
                                Write(DoTranslation("Full name: {0}"), True, ColTypes.Neutral, NeutralizePath(FileInfo.FullName))
                                Write(DoTranslation("File size: {0}"), True, ColTypes.Neutral, FileInfo.Length.FileSizeToString)
                                Write(DoTranslation("Creation time: {0}"), True, ColTypes.Neutral, Render(FileInfo.CreationTime))
                                Write(DoTranslation("Last access time: {0}"), True, ColTypes.Neutral, Render(FileInfo.LastAccessTime))
                                Write(DoTranslation("Last write time: {0}"), True, ColTypes.Neutral, Render(FileInfo.LastWriteTime))
                                Write(DoTranslation("Attributes: {0}"), True, ColTypes.Neutral, FileInfo.Attributes)
                                Write(DoTranslation("Where to find: {0}"), True, ColTypes.Neutral, NeutralizePath(FileInfo.DirectoryName))
                            Else
                                Write(DoTranslation("Can't get information about nonexistent file."), True, ColTypes.Error)
                            End If
                        Next
                    End If

                Case "firedevents"

                    WriteList(EventManager.FiredEvents)

                Case "ftp"

                    Try
                        If eqargs?.Length = 0 Or eqargs Is Nothing Then
                            InitiateShell()
                        Else
                            InitiateShell(True, eqargs(0))
                        End If
                    Catch ftpex As Exceptions.FTPShellException
                        Write(ftpex.Message, True, ColTypes.Error)
                    Catch ex As Exception
                        WStkTrc(ex)
                        Write(DoTranslation("Unknown FTP shell error:") + " {0}", True, ColTypes.Error, ex.Message)
                    End Try

                Case "gettimeinfo"

                    If RequiredArgumentsProvided Then
                        Dim DateTimeInfo As Date
                        Dim UnixEpoch As New Date(1970, 1, 1, 0, 0, 0, 0)
                        If Date.TryParse(eqargs(0), DateTimeInfo) Then
                            Write("-- " + DoTranslation("Information for") + " {0} --" + vbNewLine, True, ColTypes.Neutral, Render(DateTimeInfo))
                            Write(DoTranslation("Milliseconds:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Millisecond)
                            Write(DoTranslation("Seconds:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Second)
                            Write(DoTranslation("Minutes:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Minute)
                            Write(DoTranslation("Hours:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Hour)
                            Write(DoTranslation("Days:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Day)
                            Write(DoTranslation("Months:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Month)
                            Write(DoTranslation("Year:") + " {0}" + vbNewLine, True, ColTypes.Neutral, DateTimeInfo.Year)
                            Write(DoTranslation("Date:") + " {0}", True, ColTypes.Neutral, RenderDate(DateTimeInfo))
                            Write(DoTranslation("Time:") + " {0}" + vbNewLine, True, ColTypes.Neutral, RenderTime(DateTimeInfo))
                            Write(DoTranslation("Day of Year:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.DayOfYear)
                            Write(DoTranslation("Day of Week:") + " {0}" + vbNewLine, True, ColTypes.Neutral, DateTimeInfo.DayOfWeek.ToString)
                            Write(DoTranslation("Binary:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.ToBinary)
                            Write(DoTranslation("Local Time:") + " {0}", True, ColTypes.Neutral, Render(DateTimeInfo.ToLocalTime))
                            Write(DoTranslation("Universal Time:") + " {0}", True, ColTypes.Neutral, Render(DateTimeInfo.ToUniversalTime))
                            Write(DoTranslation("Unix Time:") + " {0}", True, ColTypes.Neutral, (DateTimeInfo - UnixEpoch).TotalSeconds)
                        Else
                            Write(DoTranslation("Failed to parse date information for") + " {0}. " + DoTranslation("Ensure that the format is correct."), True, ColTypes.Error, eqargs(0))
                        End If
                    End If

                Case "get"
                    If RequiredArgumentsProvided Then
                        Dim RetryCount As Integer = 1
                        Dim URL As String = eqargs(0)
                        Wdbg("I", "URL: {0}", URL)
                        While Not RetryCount > DRetries
                            Try
                                If Not (URL.StartsWith("ftp://") Or URL.StartsWith("ftps://") Or URL.StartsWith("ftpes://")) Then
                                    If Not URL.StartsWith(" ") Then
                                        Dim Credentials As NetworkCredential = Nothing
                                        If eqargs.Count > 1 Then 'Username specified
                                            Credentials = New NetworkCredential With {
                                                .UserName = eqargs(1)
                                            }
                                            Write(DoTranslation("Enter password: "), False, ColTypes.Input)
                                            Credentials.Password = ReadLineNoInput("*")
                                            Console.WriteLine()
                                        End If
                                        Write(DoTranslation("Downloading from {0}..."), True, ColTypes.Neutral, URL)
                                        If DownloadFile(eqargs(0), Credentials) Then
                                            Write(DoTranslation("Download has completed."), True, ColTypes.Neutral)
                                        End If
                                    Else
                                        Write(DoTranslation("Specify the address"), True, ColTypes.Error)
                                    End If
                                Else
                                    Write(DoTranslation("Please use ""ftp"" if you are going to download files from the FTP server."), True, ColTypes.Error)
                                End If
                                Exit Sub
                            Catch ex As Exception
                                DFinish = False
                                Write(DoTranslation("Download failed in try {0}: {1}"), True, ColTypes.Error, RetryCount, ex.Message)
                                RetryCount += 1
                                Wdbg("I", "Try count: {0}", RetryCount)
                                WStkTrc(ex)
                            End Try
                        End While
                    End If
                Case "hwinfo"

                    If RequiredArgumentsProvided Then
                        ListHardware(eqargs(0))
                    End If

                Case "input"

                    If RequiredArgumentsProvided Then
                        PromptInput(strArgs.Replace(eqargs(0) + " ", ""), eqargs(0))
                    End If

                Case "lockscreen"

                    LockScreen()

                Case "list"

                    'Lists folders and files
                    If eqargs?.Length = 0 Or eqargs Is Nothing Then
                        List(CurrDir)
                    Else
                        For Each Directory As String In eqargs
                            Dim direct As String = NeutralizePath(Directory)
                            List(direct)
                        Next
                    End If

                Case "loteresp"

                    InitializeLoteresp()

                Case "lsmail"

                    If KeepAlive Then
                        OpenMailShell(Mail_Authentication.Domain)
                    Else
                        If eqargs?.Length = 0 Or eqargs Is Nothing Then
                            PromptUser()
                        ElseIf Not eqargs(0) = "" Then
                            PromptPassword(eqargs(0))
                        End If
                    End If

                Case "logout"

                    'Logs out of the user
                    LogoutRequested = True

                Case "lsdbgdev"

                    For Each DebugDevice As String In DebugDevices.Values
                        Write($"- {DebugDevice}", True, ColTypes.ListEntry)
                    Next

                Case "mathbee"

                    InitializeSolver()

                Case "md"

                    If RequiredArgumentsProvided Then
                        'Create directory
                        MakeDirectory(eqargs(0))
                    End If

                Case "mkfile"

                    If RequiredArgumentsProvided Then
                        MakeFile(eqargs(0))
                    End If

                Case "mktheme"

                    If RequiredArgumentsProvided Then
                        StartThemeStudio(eqargs(0))
                    End If

                Case "modinfo"

                    If RequiredArgumentsProvided Then
                        If scripts.Keys.Contains(eqargs(0)) Then
                            WriteSeparator(eqargs(0), True, ColTypes.Stage)
                            Write("- " + DoTranslation("Mod parts:") + " ", False, ColTypes.ListEntry) : Write(scripts(eqargs(0)).Count, True, ColTypes.ListValue)
                            For Each ModPart As String In scripts(eqargs(0)).Keys
                                WriteSeparator("-- {0}", True, ColTypes.Stage, ModPart)
                                Write("- " + DoTranslation("Mod version:") + " ", False, ColTypes.ListEntry) : Write(scripts(eqargs(0))(ModPart).Version, True, ColTypes.ListValue)
                                If scripts(eqargs(0))(ModPart).Commands IsNot Nothing Then
                                    For Each ModCommand As String In scripts(eqargs(0))(ModPart).Commands.Keys
                                        WriteSeparator("--- {0}", False, ColTypes.Stage, ModCommand)
                                        Write("- " + DoTranslation("Command name:") + " ", False, ColTypes.ListEntry) : Write(ModCommand, True, ColTypes.ListValue)
                                        Write("- " + DoTranslation("Command definition:") + " ", False, ColTypes.ListEntry) : Write(scripts(eqargs(0))(ModPart).Commands(ModCommand).HelpDefinition, True, ColTypes.ListValue)
                                        Write("- " + DoTranslation("Command type:") + " ", False, ColTypes.ListEntry) : Write(scripts(eqargs(0))(ModPart).Commands(ModCommand).Type, True, ColTypes.ListValue)
                                        Write("- " + DoTranslation("Strict command?") + " ", False, ColTypes.ListEntry) : Write(scripts(eqargs(0))(ModPart).Commands(ModCommand).Strict, True, ColTypes.ListValue)
                                    Next
                                End If
                            Next
                        Else
                            Write(DoTranslation("Mod {0} not found."), True, ColTypes.Error, eqargs(0))
                        End If
                    End If

                Case "move"

                    If RequiredArgumentsProvided Then
                        MoveFileOrDir(eqargs(0), eqargs(1))
                    End If

                Case "netinfo"

                    PrintAdapterProperties()

                Case "perm"

                    If RequiredArgumentsProvided Then
                        Permission([Enum].Parse(GetType(PermissionType), eqargs(1)), eqargs(0), [Enum].Parse(GetType(PermissionManagementMode), eqargs(2)))
                    End If

                Case "ping"

                    If RequiredArgumentsProvided Then
                        'If the pinged address is actually a number of times
                        Dim PingTimes As Integer = 4
                        Dim StepsToSkip As Integer = 0
                        If IsNumeric(eqargs(0)) Then
                            Wdbg("I", "eqargs(0) is numeric. Assuming number of times: {0}", eqargs(0))
                            PingTimes = eqargs(0)
                            StepsToSkip = 1
                        End If
                        For Each PingedAddress As String In eqargs.Skip(StepsToSkip)
                            If PingedAddress <> "" Then
                                WriteSeparator(PingedAddress, True, ColTypes.Stage)
                                For CurrentTime As Integer = 1 To PingTimes
                                    Try
                                        Dim PingReplied As PingReply = PingAddress(PingedAddress)
                                        If PingReplied.Status = IPStatus.Success Then
                                            Write("[{1}] " + DoTranslation("Ping succeeded in {0} ms."), True, ColTypes.Neutral, PingReplied.RoundtripTime, CurrentTime)
                                        Else
                                            Write("[{2}] " + DoTranslation("Failed to ping {0}: {1}"), True, ColTypes.Error, PingedAddress, PingReplied.Status, CurrentTime)
                                        End If
                                    Catch ex As Exception
                                        Write("[{2}] " + DoTranslation("Failed to ping {0}: {1}"), True, ColTypes.Error, PingedAddress, ex.Message, CurrentTime)
                                        WStkTrc(ex)
                                    End Try
                                Next
                            Else
                                Write(DoTranslation("Address may not be empty."), True, ColTypes.Error)
                            End If
                        Next
                    End If

                Case "put"
                    If RequiredArgumentsProvided Then
                        Dim RetryCount As Integer = 1
                        Dim FileName As String = NeutralizePath(eqargs(0))
                        Dim URL As String = eqargs(1)
                        Wdbg("I", "URL: {0}", URL)
                        While Not RetryCount > URetries
                            Try
                                If Not (URL.StartsWith("ftp://") Or URL.StartsWith("ftps://") Or URL.StartsWith("ftpes://")) Then
                                    If Not URL.StartsWith(" ") Then
                                        Dim Credentials As NetworkCredential = Nothing
                                        If eqargs.Count > 2 Then 'Username specified
                                            Credentials = New NetworkCredential With {
                                                .UserName = eqargs(2)
                                            }
                                            Write(DoTranslation("Enter password: "), False, ColTypes.Input)
                                            Credentials.Password = ReadLineNoInput("*")
                                            Console.WriteLine()
                                        End If
                                        Write(DoTranslation("Uploading {0} to {1}..."), True, ColTypes.Neutral, FileName, URL)
                                        If UploadFile(FileName, URL, Credentials) Then
                                            Write(DoTranslation("Upload has completed."), True, ColTypes.Neutral)
                                        End If
                                    Else
                                        Write(DoTranslation("Specify the address"), True, ColTypes.Error)
                                    End If
                                Else
                                    Write(DoTranslation("Please use ""ftp"" if you are going to upload files to the FTP server."), True, ColTypes.Error)
                                End If
                                Exit Sub
                            Catch ex As Exception
                                UFinish = False
                                Write(DoTranslation("Upload failed in try {0}: {1}"), True, ColTypes.Error, RetryCount, ex.Message)
                                RetryCount += 1
                                Wdbg("I", "Try count: {0}", RetryCount)
                                WStkTrc(ex)
                            End Try
                        End While
                    End If
                Case "reloadconfig"

                    'Reload configuration
                    ReloadConfig()
                    Write(DoTranslation("Configuration reloaded. You might need to reboot the kernel for some changes to take effect."), True, ColTypes.Neutral)

                Case "reboot"

                    'Reboot the simulated system
                    If Not eqargs?.Length = 0 Then
                        If eqargs(0) = "safe" Then
                            PowerManage("rebootsafe")
                        ElseIf eqargs(0) <> "" Then
                            If eqargs?.Length > 1 Then
                                PowerManage("remoterestart", eqargs(0), eqargs(1))
                            Else
                                PowerManage("remoterestart", eqargs(0))
                            End If
                        Else
                            PowerManage("reboot")
                        End If
                    Else
                        PowerManage("reboot")
                    End If

                Case "reloadmods"

                    'Reload mods
                    If Not SafeMode Then
                        ReloadMods()
                        Write(DoTranslation("Mods reloaded."), True, ColTypes.Neutral)
                    Else
                        Write(DoTranslation("Reloading not allowed in safe mode."), True, ColTypes.Error)
                    End If

                Case "reloadsaver"

                    If RequiredArgumentsProvided Then
                        If Not SafeMode Then
                            CompileCustom(eqargs(0))
                        Else
                            Write(DoTranslation("Reloading not allowed in safe mode."), True, ColTypes.Error)
                        End If
                    End If

                Case "reportbug"

                    PromptForBug()

                Case "rexec"

                    If RequiredArgumentsProvided Then
                        If eqargs?.Length = 2 Then
                            SendCommand("<Request:Exec>(" + eqargs(1) + ")", eqargs(0))
                        Else
                            SendCommand("<Request:Exec>(" + eqargs(2) + ")", eqargs(0), eqargs(1))
                        End If
                    End If

                Case "rdebug"

                    If DebugMode Then
                        If RDebugThread.IsAlive Then
                            StartRDebugThread(False)
                        Else
                            StartRDebugThread(True)
                        End If
                    Else
                        Write(DoTranslation("Debugging not enabled."), True, ColTypes.Error)
                    End If

                Case "rm"

                    If RequiredArgumentsProvided Then
                        For Each Path As String In eqargs
                            Dim NeutPath As String = NeutralizePath(Path)
                            If File.Exists(NeutPath) Then
                                Wdbg("I", "{0} is a file. Removing...", Path)
                                RemoveFile(Path)
                            ElseIf Directory.Exists(NeutPath) Then
                                Wdbg("I", "{0} is a folder. Removing...", Path)
                                RemoveDirectory(Path)
                            Else
                                Wdbg("W", "Trying to remove {0} which is not found.", Path)
                                Write(DoTranslation("Can't remove {0} because it doesn't exist."), True, ColTypes.Error, Path)
                            End If
                        Next
                    End If

                Case "rmuser"

                    If RequiredArgumentsProvided Then
                        RemoveUser(eqargs(0))
                        Write(DoTranslation("User {0} removed."), True, ColTypes.Neutral, eqargs(0))
                    End If

                Case "rss"

                    If Not eqargs?.Length = 0 Then
                        InitiateRSSShell(eqargs(0))
                    Else
                        InitiateRSSShell()
                    End If

                Case "savescreen"

                    If Not eqargs?.Length = 0 Then
                        ShowSavers(eqargs(0))
                    Else
                        ShowSavers(defSaverName)
                    End If

                Case "search"

                    If RequiredArgumentsProvided Then
                        Try
                            Dim Matches As List(Of String) = SearchFileForStringRegexp(eqargs(1), New RegularExpressions.Regex(eqargs(0), RegularExpressions.RegexOptions.IgnoreCase))
                            For Each Match As String In Matches
                                Write(Match, True, ColTypes.Neutral)
                            Next
                        Catch ex As Exception
                            Wdbg("E", "Error trying to search {0} for {1}", eqargs(0), eqargs(1))
                            WStkTrc(ex)
                            Write(DoTranslation("Searching {0} for {1} failed.") + " {2}", True, ColTypes.Error, eqargs(0), eqargs(1), ex.Message)
                        End Try
                    End If

                Case "searchword"

                    If RequiredArgumentsProvided Then
                        Try
                            Dim Matches As List(Of String) = SearchFileForString(eqargs(1), eqargs(0))
                            For Each Match As String In Matches
                                Write(Match, True, ColTypes.Neutral)
                            Next
                        Catch ex As Exception
                            Wdbg("E", "Error trying to search {0} for {1}", eqargs(0), eqargs(1))
                            WStkTrc(ex)
                            Write(DoTranslation("Searching {0} for {1} failed.") + " {2}", True, ColTypes.Error, eqargs(0), eqargs(1), ex.Message)
                        End Try
                    End If

                Case "savecurrdir"

                    SaveCurrDir()

                Case "setsaver"

                    Dim modPath As String = paths("Mods")
                    If RequiredArgumentsProvided Then
                        If ScrnSvrdb.ContainsKey(strArgs) Or CSvrdb.ContainsKey(strArgs) Then
                            SetDefaultScreensaver(strArgs)
                            Write(DoTranslation("{0} is set to default screensaver."), True, ColTypes.Neutral, strArgs)
                        Else
                            If File.Exists($"{modPath}{strArgs}") And Not SafeMode Then
                                SetDefaultScreensaver(strArgs)
                                Write(DoTranslation("{0} is set to default screensaver."), True, ColTypes.Neutral, strArgs)
                            Else
                                Write(DoTranslation("Screensaver {0} not found."), True, ColTypes.Error, strArgs)
                            End If
                        End If
                    End If

                Case "setthemes"

                    If RequiredArgumentsProvided Then
                        If ColoredShell = True Then
                            Dim ThemePath As String = NeutralizePath(eqargs(0))
                            If File.Exists(ThemePath) Then
                                ApplyThemeFromFile(ThemePath)
                            Else
                                ApplyThemeFromResources(eqargs(0))
                            End If
                        Else
                            Write(DoTranslation("Colors are not available. Turn on colored shell in the kernel config."), True, ColTypes.Neutral)
                        End If
                    End If

                Case "settings"

                    OpenMainPage()

                Case "set"

                    If RequiredArgumentsProvided Then
                        SetVariable(eqargs(0), eqargs(1))
                    End If

                Case "sftp"

                    Try
                        If eqargs?.Length = 0 Or eqargs Is Nothing Then
                            SFTPInitiateShell()
                        Else
                            SFTPInitiateShell(True, eqargs(0))
                        End If
                    Catch sftpex As Exceptions.SFTPShellException
                        Write(sftpex.Message, True, ColTypes.Error)
                    Catch ex As Exception
                        WStkTrc(ex)
                        Write(DoTranslation("Unknown SFTP shell error:") + " {0}", True, ColTypes.Error, ex.Message)
                    End Try

                Case "shownotifs"

                    Dim Count As Integer = 1
                    If Not NotifRecents.Count = 0 Then
                        For Each Notif As Notification In NotifRecents
                            Write($"{Count}: {Notif.Title}" + vbNewLine +
                              $"{Count}: {Notif.Desc}" + vbNewLine +
                              $"{Count}: {Notif.Priority}", True, ColTypes.Neutral)
                            Count += 1
                        Next
                    Else
                        Write(DoTranslation("No recent notifications"), True, ColTypes.Neutral)
                    End If

                Case "showtd"

                    ShowCurrentTimes()

                Case "showtdzone"

                    If RequiredArgumentsProvided Then
                        InitTimesInZones()
                        Dim DoneFlag As Boolean = False
                        If zoneTimes.Keys.Contains(strArgs) Then
                            DoneFlag = True
                            ShowTimesInZones(strArgs)
                        End If
                        If DoneFlag = False Then
                            If eqargs(0) = "all" Then
                                ShowTimesInZones()
                            Else
                                Write(DoTranslation("Timezone is specified incorrectly."), True, ColTypes.Error)
                            End If
                        End If
                    End If

                Case "shutdown"

                    'Shuts down the simulated system
                    If Not eqargs?.Length = 0 Then
                        If eqargs?.Length = 1 Then
                            PowerManage("remoteshutdown", eqargs(0))
                        Else
                            PowerManage("remoteshutdown", eqargs(0), eqargs(1))
                        End If
                    Else
                        PowerManage("shutdown")
                    End If

                Case "speedpress"

                    If RequiredArgumentsProvided Then
                        If eqargs(0) = "e" Or eqargs(0) = "m" Or eqargs(0) = "h" Then
                            InitializeSpeedPress(eqargs(0))
                        Else
                            Write(DoTranslation("Invalid difficulty") + " {0}", True, ColTypes.Error, eqargs(0))
                        End If
                    End If

                Case "spellbee"

                    InitializeWords()

                Case "sshell"

                    If RequiredArgumentsProvided Then
                        Dim AddressDelimiter() As String = eqargs(0).Split(":")
                        Dim Address As String = AddressDelimiter(0)
                        If AddressDelimiter.Length > 1 Then
                            Dim Port As Integer = AddressDelimiter(1)
                            InitializeSSH(Address, Port, eqargs(1), ConnectionType.Shell)
                        Else
                            InitializeSSH(Address, 22, eqargs(1), ConnectionType.Shell)
                        End If
                    End If

                Case "sshcmd"

                    If RequiredArgumentsProvided Then
                        Dim AddressDelimiter() As String = eqargs(0).Split(":")
                        Dim Address As String = AddressDelimiter(0)
                        If AddressDelimiter.Length > 1 Then
                            Dim Port As Integer = AddressDelimiter(1)
                            InitializeSSH(Address, Port, eqargs(1), ConnectionType.Command, eqargs(2))
                        Else
                            InitializeSSH(Address, 22, eqargs(1), ConnectionType.Command, eqargs(2))
                        End If
                    End If

                Case "sumfile"

                    If RequiredArgumentsProvided Then
                        Dim file As String = NeutralizePath(eqargs(1))
                        If IO.File.Exists(file) Then
                            If eqargs(0) = "SHA512" Then
                                Dim spent As New Stopwatch
                                spent.Start() 'Time when you're on a breakpoint is counted
                                Write(GetEncryptedFile(file, Algorithms.SHA512), True, ColTypes.Neutral)
                                Write(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                spent.Stop()
                            ElseIf eqargs(0) = "SHA256" Then
                                Dim spent As New Stopwatch
                                spent.Start() 'Time when you're on a breakpoint is counted
                                Write(GetEncryptedFile(file, Algorithms.SHA256), True, ColTypes.Neutral)
                                Write(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                spent.Stop()
                            ElseIf eqargs(0) = "SHA1" Then
                                Dim spent As New Stopwatch
                                spent.Start() 'Time when you're on a breakpoint is counted
                                Write(GetEncryptedFile(file, Algorithms.SHA1), True, ColTypes.Neutral)
                                Write(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                spent.Stop()
                            ElseIf eqargs(0) = "MD5" Then
                                Dim spent As New Stopwatch
                                spent.Start() 'Time when you're on a breakpoint is counted
                                Write(GetEncryptedFile(file, Algorithms.MD5), True, ColTypes.Neutral)
                                Write(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                spent.Stop()
                            ElseIf eqargs(0) = "all" Then
                                For Each Algorithm As String In [Enum].GetNames(GetType(Algorithms))
                                    Dim AlgorithmEnum As Algorithms = [Enum].Parse(GetType(Algorithms), Algorithm)
                                    Dim spent As New Stopwatch
                                    spent.Start() 'Time when you're on a breakpoint is counted
                                    Write("{0} ({1})", True, ColTypes.Neutral, GetEncryptedFile(file, AlgorithmEnum), AlgorithmEnum)
                                    Write(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                    spent.Stop()
                                Next
                            Else
                                Write(DoTranslation("Invalid encryption algorithm."), True, ColTypes.Error)
                            End If
                        Else
                            Write(DoTranslation("{0} is not found."), True, ColTypes.Error, file)
                        End If
                    End If

                Case "sumfiles"

                    If RequiredArgumentsProvided Then
                        Dim folder As String = NeutralizePath(eqargs(1))
                        Dim out As String = ""
                        Dim FileBuilder As New StringBuilder
                        If Not eqargs.Length < 3 Then
                            out = NeutralizePath(eqargs(2))
                        End If
                        If Directory.Exists(folder) Then
                            For Each file As String In Directory.EnumerateFiles(folder, "*", IO.SearchOption.TopDirectoryOnly)
                                file = NeutralizePath(file)
                                WriteSeparator(file, True, ColTypes.Stage)
                                If eqargs(0) = "SHA512" Then
                                    Dim spent As New Stopwatch
                                    spent.Start() 'Time when you're on a breakpoint is counted
                                    Dim encrypted As String = GetEncryptedFile(file, Algorithms.SHA512)
                                    Write(encrypted, True, ColTypes.Neutral)
                                    Write(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                    FileBuilder.AppendLine($"- {file}: {encrypted} ({eqargs(0)})")
                                    spent.Stop()
                                ElseIf eqargs(0) = "SHA256" Then
                                    Dim spent As New Stopwatch
                                    spent.Start() 'Time when you're on a breakpoint is counted
                                    Dim encrypted As String = GetEncryptedFile(file, Algorithms.SHA256)
                                    Write(encrypted, True, ColTypes.Neutral)
                                    Write(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                    FileBuilder.AppendLine($"- {file}: {encrypted} ({eqargs(0)})")
                                    spent.Stop()
                                ElseIf eqargs(0) = "SHA1" Then
                                    Dim spent As New Stopwatch
                                    spent.Start() 'Time when you're on a breakpoint is counted
                                    Dim encrypted As String = GetEncryptedFile(file, Algorithms.SHA1)
                                    Write(encrypted, True, ColTypes.Neutral)
                                    Write(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                    FileBuilder.AppendLine($"- {file}: {encrypted} ({eqargs(0)})")
                                    spent.Stop()
                                ElseIf eqargs(0) = "MD5" Then
                                    Dim spent As New Stopwatch
                                    spent.Start() 'Time when you're on a breakpoint is counted
                                    Dim encrypted As String = GetEncryptedFile(file, Algorithms.MD5)
                                    Write(encrypted, True, ColTypes.Neutral)
                                    Write(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                    FileBuilder.AppendLine($"- {file}: {encrypted} ({eqargs(0)})")
                                    spent.Stop()
                                ElseIf eqargs(0) = "all" Then
                                    For Each Algorithm As String In [Enum].GetNames(GetType(Algorithms))
                                        Dim AlgorithmEnum As Algorithms = [Enum].Parse(GetType(Algorithms), Algorithm)
                                        Dim spent As New Stopwatch
                                        spent.Start() 'Time when you're on a breakpoint is counted
                                        Dim encrypted As String = GetEncryptedFile(file, AlgorithmEnum)
                                        Write("{0} ({1})", True, ColTypes.Neutral, encrypted, AlgorithmEnum)
                                        Write(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                                        FileBuilder.AppendLine($"- {file}: {encrypted} ({AlgorithmEnum})")
                                        spent.Stop()
                                    Next
                                Else
                                    Write(DoTranslation("Invalid encryption algorithm."), True, ColTypes.Error)
                                    Exit For
                                End If
                                Console.WriteLine()
                            Next
                            If Not out = "" Then
                                Dim FStream As New StreamWriter(out)
                                FStream.Write(FileBuilder.ToString)
                                FStream.Flush()
                            End If
                        Else
                            Write(DoTranslation("{0} is not found."), True, ColTypes.Error, folder)
                        End If
                    End If

                Case "sysinfo"

                    'Shows system information
                    Write(DoTranslation("[ Kernel settings (Running on {0}) ]"), True, ColTypes.ListEntry, Environment.OSVersion.ToString)

                    'Kernel section
                    Write(vbNewLine + DoTranslation("Kernel Version:") + " {0}" + vbNewLine +
                                  DoTranslation("Debug Mode:") + " {1}" + vbNewLine +
                                  DoTranslation("Colored Shell:") + " {2}" + vbNewLine +
                                  DoTranslation("Arguments on Boot:") + " {3}" + vbNewLine +
                                  DoTranslation("Help command simplified:") + " {4}" + vbNewLine +
                                  DoTranslation("MOTD on Login:") + " {5}" + vbNewLine +
                                  DoTranslation("Time/Date on corner:") + " {6}" + vbNewLine, True, ColTypes.Neutral, KernelVersion, DebugMode.ToString, ColoredShell.ToString, argsOnBoot.ToString, simHelp.ToString, showMOTD.ToString, CornerTD.ToString)

                    'Hardware section
                    Write(DoTranslation("[ Hardware settings ]{0}"), True, ColTypes.ListEntry, vbNewLine)
                    ListHardware()
                    Write(DoTranslation("Use ""hwinfo"" for extended information about hardware."), True, ColTypes.Neutral)

                    'User section
                    Write(DoTranslation("{0}[ User settings ]"), True, ColTypes.ListEntry, vbNewLine)
                    Write(vbNewLine + DoTranslation("Current user name:") + " {0}" + vbNewLine +
                                  DoTranslation("Current host name:") + " {1}" + vbNewLine +
                                  DoTranslation("Available usernames:") + " {2}", True, ColTypes.Neutral, signedinusrnm, HName, String.Join(", ", userword.Keys))

                    'Messages Section
                    Write(vbNewLine + "[ MOTD ]", True, ColTypes.ListEntry)
                    Write(vbNewLine + ProbePlaces(MOTDMessage), True, ColTypes.Neutral)
                    Write(vbNewLine + "[ MAL ]", True, ColTypes.ListEntry)
                    Write(vbNewLine + ProbePlaces(MAL), True, ColTypes.Neutral)

                Case "unblockdbgdev"

                    If RequiredArgumentsProvided Then
                        If RDebugBlocked.Contains(eqargs(0)) Then
                            If RemoveFromBlockList(eqargs(0)) Then
                                Write(DoTranslation("{0} can now join remote debug again."), True, ColTypes.Neutral, eqargs(0))
                            Else
                                Write(DoTranslation("Failed to unblock {0}."), True, ColTypes.Neutral, eqargs(0))
                            End If
                        Else
                            Write(DoTranslation("{0} is not blocked yet."), True, ColTypes.Neutral, eqargs(0))
                        End If
                    End If

                Case "unzip"

                    If RequiredArgumentsProvided And eqargs?.Length = 1 Then
                        Dim ZipArchiveName As String = NeutralizePath(eqargs(0))
                        ZipFile.ExtractToDirectory(ZipArchiveName, CurrDir)
                    ElseIf RequiredArgumentsProvided And eqargs?.Length > 1 Then
                        Dim ZipArchiveName As String = NeutralizePath(eqargs(0))
                        Dim Destination As String = If(Not eqargs(1) = "-createdir", NeutralizePath(eqargs(1)), "")
                        If eqargs?.Contains("-createdir") Then
                            Destination = $"{If(Not eqargs(1) = "-createdir", NeutralizePath(eqargs(1)), "")}/{If(Not eqargs(1) = "-createdir", Path.GetFileNameWithoutExtension(ZipArchiveName), NeutralizePath(Path.GetFileNameWithoutExtension(ZipArchiveName)))}"
                            If Destination(0) = "/" Then Destination = Destination.Substring(1)
                        End If
                        ZipFile.ExtractToDirectory(ZipArchiveName, Destination)
                    End If

                Case "update"

#If SPECIFIER = "REL" Then
                    CheckKernelUpdates()
#Else
                    Write(DoTranslation("Checking for updates is disabled because you're running a development version."), True, ColTypes.Error)
#End If

                Case "usermanual"

                    Process.Start("https://github.com/Aptivi/NitrocidKS/wiki")

                Case "verify"

                    If RequiredArgumentsProvided Then
                        Try
                            Dim HashFile As String = NeutralizePath(eqargs(2))
                            If File.Exists(HashFile) Then
                                If VerifyHashFromHashesFile(eqargs(3), [Enum].Parse(GetType(Algorithms), eqargs(0)), eqargs(2), eqargs(1)) Then
                                    Write(DoTranslation("Hashes match."), True, ColTypes.Neutral)
                                Else
                                    Write(DoTranslation("Hashes don't match."), True, ColTypes.Warning)
                                End If
                            Else
                                If VerifyHashFromHash(eqargs(3), [Enum].Parse(GetType(Algorithms), eqargs(0)), eqargs(2), eqargs(1)) Then
                                    Write(DoTranslation("Hashes match."), True, ColTypes.Neutral)
                                Else
                                    Write(DoTranslation("Hashes don't match."), True, ColTypes.Warning)
                                End If
                            End If
                        Catch ihae As Exceptions.InvalidHashAlgorithmException
                            WStkTrc(ihae)
                            Write(DoTranslation("Invalid encryption algorithm."), True, ColTypes.Error)
                        Catch ihe As Exceptions.InvalidHashException
                            WStkTrc(ihe)
                            Write(DoTranslation("Hashes are malformed."), True, ColTypes.Error)
                        Catch fnfe As FileNotFoundException
                            WStkTrc(fnfe)
                            Write(DoTranslation("{0} is not found."), True, ColTypes.Error, eqargs(3))
                        End Try
                    End If

                Case "weather"

                    If RequiredArgumentsProvided Then
                        If eqargs(0) = "listcities" Then
                            Dim Cities As Dictionary(Of Long, String) = ListAllCities()
                            WriteList(Cities)
                        Else
                            Dim APIKey As String
                            Write(DoTranslation("You can get your own API key at https://home.openweathermap.org/api_keys."), True, ColTypes.Neutral)
                            Write(DoTranslation("Enter your API key:") + " ", False, ColTypes.Input)
                            APIKey = ReadLineNoInput("*")
                            Console.WriteLine()
                            PrintWeatherInfo(eqargs(0), APIKey)
                        End If
                    End If

                Case "wrap"

                    If RequiredArgumentsProvided Then
                        Dim CommandToBeWrapped As String = eqargs(0).Split(" ")(0)
                        If Commands.ContainsKey(CommandToBeWrapped) Then
                            If Commands(CommandToBeWrapped).Wrappable Then
                                Dim WrapOutputPath As String = paths("Temp") + "/wrapoutput.txt"
                                GetLine(False, eqargs(0), False, WrapOutputPath)
                                Dim WrapOutputStream As New StreamReader(WrapOutputPath)
                                Dim WrapOutput As String = WrapOutputStream.ReadToEnd
                                WriteWrapped(WrapOutput, False, ColTypes.Neutral)
                                If Not WrapOutput.EndsWith(vbNewLine) Then Console.WriteLine()
                                WrapOutputStream.Close()
                                File.Delete(WrapOutputPath)
                            Else
                                Dim WrappableCmds As New ArrayList
                                For Each CommandInfo As CommandInfo In Commands.Values
                                    If CommandInfo.Wrappable Then WrappableCmds.Add(CommandInfo.Command)
                                Next
                                Write(DoTranslation("The command is not wrappable. These commands are wrappable:") + " {0}", True, ColTypes.Error, String.Join(", ", WrappableCmds.ToArray))
                            End If
                        Else
                            Write(DoTranslation("The wrappable command is not found."), True, ColTypes.Error)
                        End If
                    End If

                Case "zip"

                    If RequiredArgumentsProvided Then
                        Dim ZipArchiveName As String = NeutralizePath(eqargs(0))
                        Dim Destination As String = NeutralizePath(eqargs(1))
                        Dim ZipCompression As CompressionLevel = CompressionLevel.Optimal
                        Dim ZipBaseDir As Boolean = True
                        If eqargs?.Contains("-fast") Then
                            ZipCompression = CompressionLevel.Fastest
                        ElseIf eqargs?.Contains("-nocomp") Then
                            ZipCompression = CompressionLevel.NoCompression
                        End If
                        If eqargs?.Contains("-nobasedir") Then
                            ZipBaseDir = False
                        End If
                        ZipFile.CreateFromDirectory(Destination, ZipArchiveName, ZipCompression, ZipBaseDir)
                    End If

                Case "zipshell"

                    If RequiredArgumentsProvided Then
                        eqargs(0) = NeutralizePath(eqargs(0))
                        Wdbg("I", "File path is {0} and .Exists is {0}", eqargs(0), File.Exists(eqargs(0)))
                        If File.Exists(eqargs(0)) Then
                            InitializeZipShell(eqargs(0))
                        Else
                            Write(DoTranslation("File doesn't exist."), True, ColTypes.Error)
                        End If
                    End If

            End Select

            'If not enough arguments, throw exception
            If Commands(Command).ArgumentsRequired And Not RequiredArgumentsProvided Then
                Wdbg("W", "User hasn't provided enough arguments for {0}", Command)
                Write(DoTranslation("There was not enough arguments. See below for usage:"), True, ColTypes.Neutral)
                ShowHelp(Command)
            End If
        Catch taex As ThreadAbortException
            CancelRequested = False
            Exit Sub
        Catch ex As Exception
            EventManager.RaiseCommandError(requestedCommand, ex)
            If DebugMode = True Then
                Write(DoTranslation("Error trying to execute command") + " {3}." + vbNewLine + DoTranslation("Error {0}: {1}") + vbNewLine + "{2}", True, ColTypes.Error,
                  ex.GetType.FullName, ex.Message, ex.StackTrace, Command)
                WStkTrc(ex)
            Else
                Write(DoTranslation("Error trying to execute command") + " {2}." + vbNewLine + DoTranslation("Error {0}: {1}"), True, ColTypes.Error, ex.GetType.FullName, ex.Message, Command)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Cancels any running command
    ''' </summary>
    ''' <param name="sender">Sender object</param>
    ''' <param name="e">Arguments of event</param>
    Sub CancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            CancelRequested = True
            Console.WriteLine()
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            StartCommandThread.Abort()
        End If
    End Sub

End Module
