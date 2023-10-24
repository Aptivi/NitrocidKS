
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

Public Module HelpSystem

    'This dictionary is the definitions for commands.
    Public definitions As Dictionary(Of String, String)
    Public moddefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Updates the shell help dictionary to reflect the available commands
    ''' </summary>
    Public Sub InitHelp()
        definitions = New Dictionary(Of String, String) From {{"adduser", DoTranslation("Adds users")},
                                                              {"alias", DoTranslation("Adds aliases to commands")},
                                                              {"arginj", DoTranslation("Injects arguments to the kernel (reboot required)")},
                                                              {"beep", DoTranslation("Beep in 'n' Hz and time in 'n' milliseconds")},
                                                              {"blockdbgdev", DoTranslation("Block a debug device by IP address")},
                                                              {"calc", DoTranslation("Calculator to calculate expressions.")},
                                                              {"cat", DoTranslation("Prints content of file to console")},
                                                              {"cdbglog", DoTranslation("Deletes everything in debug log")},
                                                              {"chattr", DoTranslation("Changes attribute of a file")},
                                                              {"chdir", DoTranslation("Changes directory")},
                                                              {"chhostname", DoTranslation("Changes host name")},
                                                              {"chlang", DoTranslation("Changes language")},
                                                              {"chmal", DoTranslation("Changes MAL, the MOTD After Login")},
                                                              {"chmotd", DoTranslation("Changes MOTD, the Message Of The Day")},
                                                              {"choice", DoTranslation("Makes user choices")},
                                                              {"chpwd", DoTranslation("Changes password for current user")},
                                                              {"chusrname", DoTranslation("Changes user name")},
                                                              {"cls", DoTranslation("Clears the screen")},
                                                              {"copy", DoTranslation("Creates another copy of a file under different directory or name.")},
                                                              {"dirinfo", DoTranslation("Provides information about a directory")},
                                                              {"disconndbgdev", DoTranslation("Disconnect a debug device")},
                                                              {"dismissnotif", DoTranslation("Dismisses a notification")},
                                                              {"echo", DoTranslation("Writes text into the console")},
                                                              {"edit", DoTranslation("Edits a text file")},
                                                              {"fileinfo", DoTranslation("Provides information about a file")},
                                                              {"firedevents", DoTranslation("Lists all fired events")},
                                                              {"ftp", DoTranslation("Use an FTP shell to interact with servers")},
                                                              {"gettimeinfo", DoTranslation("Gets the date and time information")},
                                                              {"get", DoTranslation("Downloads a file to current working directory")},
                                                              {"help", DoTranslation("Help page")},
                                                              {"hwinfo", DoTranslation("Prints hardware information")},
                                                              {"input", DoTranslation("Allows user to enter input")},
                                                              {"list", DoTranslation("List file/folder contents in current folder")},
                                                              {"lockscreen", DoTranslation("Locks your screen with a password")},
                                                              {"logout", DoTranslation("Logs you out")},
                                                              {"loteresp", DoTranslation("Respond to love or hate comments.")},
                                                              {"lsdbgdev", DoTranslation("Lists debugging devices connected")},
                                                              {"lsmail", DoTranslation("Lists all mails in the specific user.")},
                                                              {"mathbee", DoTranslation("See if you can solve mathematical equations on time")},
                                                              {"md", DoTranslation("Creates a directory")},
                                                              {"mkfile", DoTranslation("Makes a new file")},
                                                              {"mktheme", DoTranslation("Makes a new theme")},
                                                              {"modinfo", DoTranslation("Gets mod information")},
                                                              {"move", DoTranslation("Moves a file to another directory")},
                                                              {"netinfo", DoTranslation("Lists information about all available interfaces")},
                                                              {"perm", DoTranslation("Manage permissions for users")},
                                                              {"ping", DoTranslation("Pings an address")},
                                                              {"put", DoTranslation("Uploads a file to specified website")},
                                                              {"reboot", DoTranslation("Restarts your computer (WARNING: No syncing, because it is not a final kernel)")},
                                                              {"reloadconfig", DoTranslation("Reloads configuration file that is edited.")},
                                                              {"reloadmods", DoTranslation("Reloads mods.")},
                                                              {"reloadsaver", DoTranslation("Reloads screensaver file in KSMods")},
                                                              {"rexec", DoTranslation("Remotely executes a command to remote PC")},
                                                              {"rm", DoTranslation("Removes a directory or a file")},
                                                              {"rdebug", DoTranslation("Enables or disables remote debugging.")},
                                                              {"reportbug", DoTranslation("A bug reporting prompt.")},
                                                              {"rmuser", DoTranslation("Removes a user from the list")},
                                                              {"rss", DoTranslation("Opens an RSS shell to read the feeds")},
                                                              {"savecurrdir", DoTranslation("Saves the current directory to kernel configuration file")},
                                                              {"savescreen", DoTranslation("Saves your screen from burn outs")},
                                                              {"search", DoTranslation("Searches for specified string in the provided file using regular expressions")},
                                                              {"searchword", DoTranslation("Searches for specified string in the provided file")},
                                                              {"setsaver", DoTranslation("Sets up kernel screensavers")},
                                                              {"setthemes", DoTranslation("Sets up kernel themes")},
                                                              {"settings", DoTranslation("Changes kernel configuration")},
                                                              {"set", DoTranslation("Sets a variable to a value in a script")},
                                                              {"sftp", DoTranslation("Lets you use an SSH FTP server")},
                                                              {"shownotifs", DoTranslation("Shows all received notifications")},
                                                              {"showtd", DoTranslation("Shows date and time")},
                                                              {"showtdzone", DoTranslation("Shows date and time in zones")},
                                                              {"shutdown", DoTranslation("The kernel will be shut down")},
                                                              {"speedpress", DoTranslation("See if you can press a key on time")},
                                                              {"spellbee", DoTranslation("See if you can spell words correctly on time")},
                                                              {"sshell", DoTranslation("Connects to an SSH server.")},
                                                              {"sshcmd", DoTranslation("Connects to an SSH server to execute a command.")},
                                                              {"sumfile", DoTranslation("Calculates file sums.")},
                                                              {"sumfiles", DoTranslation("Calculates sums of files in specified directory.")},
                                                              {"sysinfo", DoTranslation("System information")},
                                                              {"unblockdbgdev", DoTranslation("Unblock a debug device by IP address")},
                                                              {"unzip", DoTranslation("Extracts a ZIP archive")},
                                                              {"update", DoTranslation("System update")},
                                                              {"usermanual", DoTranslation("Takes you to our GitHub Wiki.")},
                                                              {"verify", DoTranslation("Verifies sanity of the file")},
                                                              {"weather", DoTranslation("Shows weather info for specified city. Uses OpenWeatherMap.")},
                                                              {"wrap", DoTranslation("Wraps the console output")},
                                                              {"zip", DoTranslation("Creates a ZIP archive")},
                                                              {"zipshell", DoTranslation("Opens a ZIP archive")}}
    End Sub

    ''' <summary>
    ''' Shows the help of a command, or command list if nothing is specified
    ''' </summary>
    ''' <param name="command">A specified command</param>
    Public Sub ShowHelp(Optional ByVal command As String = "")

        Dim ScreensaverFiles As New List(Of String)
        ScreensaverFiles.AddRange(Directory.GetFiles(paths("Mods"), "*.ss.vb", SearchOption.TopDirectoryOnly).Select(Function(x) Path.GetFileName(x)))
        ScreensaverFiles.AddRange(Directory.GetFiles(paths("Mods"), "*.ss.cs", SearchOption.TopDirectoryOnly).Select(Function(x) Path.GetFileName(x)))
        If command = "" Then

            If simHelp = False Then
                Write(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In definitions.Keys
                    If (Not Commands(cmd).Strict) Or (Commands(cmd).Strict And adminList(signedinusrnm)) Then
                        Write("- {0}: ", False, ColTypes.ListEntry, cmd) : Write("{0}", True, ColTypes.ListValue, definitions(cmd))
                    End If
                Next
                Write(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If moddefs.Count = 0 Then Write("- " + DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In moddefs.Keys
                    Write("- {0}: ", False, ColTypes.ListEntry, cmd) : Write("{0}", True, ColTypes.ListValue, moddefs(cmd))
                Next
                Write(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If Aliases.Count = 0 Then Write("- " + DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In Aliases.Keys
                    Write("- {0}: ", False, ColTypes.ListEntry, cmd) : Write("{0}", True, ColTypes.ListValue, definitions(Aliases(cmd)))
                Next
                Write(vbNewLine + DoTranslation("* You can use multiple commands using the colon between commands."), True, ColTypes.Neutral)
            Else
                For Each cmd As String In Commands.Keys
                    If (Not Commands(cmd).Strict) Or (Commands(cmd).Strict And adminList(signedinusrnm)) Then
                        Write("{0}, ", False, ColTypes.ListEntry, cmd)
                    End If
                Next
                For Each cmd As String In moddefs.Keys
                    Write("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                Write(String.Join(", ", Aliases.Keys), True, ColTypes.ListEntry)
            End If

        ElseIf command = "adduser" Then

            Write(DoTranslation("Usage:") + " adduser <userName> [password] [confirm]", True, ColTypes.Neutral)

        ElseIf command = "alias" Then

            Write(DoTranslation("Usage:") + " alias <rem/add> <{0}> <alias> <cmd>", True, ColTypes.Neutral, String.Join("/", [Enum].GetNames(GetType(AliasType))))

        ElseIf command = "arginj" Then

            Write(DoTranslation("Usage:") + " arginj [Arguments separated by spaces]" + vbNewLine +
              "       " + DoTranslation("where arguments will be {0}"), True, ColTypes.Neutral, String.Join(", ", AvailableArgs))

        ElseIf command = "beep" Then

            Write(DoTranslation("Usage:") + " beep <37-32767 Hz> <milliseconds>", True, ColTypes.Neutral)

        ElseIf command = "blockdbgdev" Then

            Write(DoTranslation("Usage:") + " blockdbgdev <ipaddress>", True, ColTypes.Neutral)

        ElseIf command = "cat" Then

            Write(DoTranslation("Usage:") + " cat <file>", True, ColTypes.Neutral)

        ElseIf command = "calc" Then

            Write(DoTranslation("Usage:") + " calc <expression>", True, ColTypes.Neutral)

        ElseIf command = "cdbglog" Then

            Write(DoTranslation("Usage:") + " cdbglog: " + DoTranslation("Deletes everything in debug log"), True, ColTypes.Neutral)

        ElseIf command = "chattr" Then

            Write(DoTranslation("Usage:") + " chattr <file> +/-<attributes>", True, ColTypes.Neutral)
            Write(DoTranslation("where <attributes> is one of the following:") + vbNewLine, True, ColTypes.Neutral)
            Write("- Normal: ", False, ColTypes.ListEntry) : Write(DoTranslation("The file is a normal file"), True, ColTypes.ListValue)                   'Normal   = 128
            Write("- ReadOnly: ", False, ColTypes.ListEntry) : Write(DoTranslation("The file is a read-only file"), True, ColTypes.ListValue)              'ReadOnly = 1
            Write("- Hidden: ", False, ColTypes.ListEntry) : Write(DoTranslation("The file is a hidden file"), True, ColTypes.ListValue)                   'Hidden   = 2
            Write("- Archive: ", False, ColTypes.ListEntry) : Write(DoTranslation("The file is an archive. Used for backups."), True, ColTypes.ListValue)  'Archive  = 32

        ElseIf command = "chdir" Then

            Write(DoTranslation("Usage:") + " chdir <directory/..>", True, ColTypes.Neutral)

        ElseIf command = "chhostname" Then

            Write(DoTranslation("Usage:") + " chhostname <HostName>", True, ColTypes.Neutral)

        ElseIf command = "chlang" Then

            Write(DoTranslation("Usage:") + " chlang <language>" + vbNewLine +
              "<language>: " + String.Join("/", Languages.Keys), True, ColTypes.Neutral)

        ElseIf command = "chmotd" Then

            Write(DoTranslation("Usage:") + " chmotd [Message]", True, ColTypes.Neutral)

        ElseIf command = "chmal" Then

            Write(DoTranslation("Usage:") + " chmal [Message]", True, ColTypes.Neutral)

        ElseIf command = "choice" Then

            Write(DoTranslation("Usage:") + " choice [-o|-t|-m] <$variable> <answers> <input>" + vbNewLine +
              "       " + DoTranslation("where <$variable> is any variable that will be used to store response") + vbNewLine +
              "       " + DoTranslation("where <answers> are one-lettered answers of the question separated in slashes"), True, ColTypes.Neutral)

        ElseIf command = "chpwd" Then

            Write(DoTranslation("Usage:") + " chpwd <Username> <UserPass> <newPass> <confirm>", True, ColTypes.Neutral)

        ElseIf command = "chusrname" Then

            Write(DoTranslation("Usage:") + " chusrname <oldUserName> <newUserName>", True, ColTypes.Neutral)

        ElseIf command = "cls" Then

            Write(DoTranslation("Usage:") + " cls: " + DoTranslation("to clear screen."), True, ColTypes.Neutral)

        ElseIf command = "copy" Then

            Write(DoTranslation("Usage:") + " copy <source> <target>: " + DoTranslation("To copy files to another directory or different name"), True, ColTypes.Neutral)

        ElseIf command = "dirinfo" Then

            Write(DoTranslation("Usage:") + " dirinfo <directory>", True, ColTypes.Neutral)

        ElseIf command = "dismissnotif" Then

            Write(DoTranslation("Usage:") + " dismissnotif <notificationNumber>: " + DoTranslation("Dismisses a notification"), True, ColTypes.Neutral)

        ElseIf command = "disconndbgdev" Then

            Write(DoTranslation("Usage:") + " disconndbgdev <ip>", True, ColTypes.Neutral)

        ElseIf command = "echo" Then

            Write(DoTranslation("Usage:") + " echo <text>", True, ColTypes.Neutral)

        ElseIf command = "edit" Then

            Write(DoTranslation("Usage:") + " edit <file>", True, ColTypes.Neutral)

        ElseIf command = "fileinfo" Then

            Write(DoTranslation("Usage:") + " fileinfo <file>", True, ColTypes.Neutral)

        ElseIf command = "firedevents" Then

            Write(DoTranslation("Usage:") + " firedevents: " + DoTranslation("Lists all fired events"), True, ColTypes.Neutral)

        ElseIf command = "ftp" Then

            Write(DoTranslation("Usage:") + " ftp [server]: " + DoTranslation("Initializes the FTP shell."), True, ColTypes.Neutral)

        ElseIf command = "gettimeinfo" Then

            Write(DoTranslation("Usage:") + " gettimeinfo <date>", True, ColTypes.Neutral)

        ElseIf command = "get" Then

            Write(DoTranslation("Usage:") + " get <URL> [username]", True, ColTypes.Neutral)

        ElseIf command = "hwinfo" Then

            Write(DoTranslation("Usage:") + " hwinfo <HardwareType>: " + DoTranslation("Prints hardware information") + vbNewLine +
              "       " + DoTranslation("where HardwareType will be") + " HDD, LogicalParts, CPU, GPU, Sound, Network, System, Machine, BIOS, RAM, all.", True, ColTypes.Neutral)

        ElseIf command = "input" Then

            Write(DoTranslation("Usage:") + " input <$variable> <question>", True, ColTypes.Neutral)

        ElseIf command = "list" Then

            Write(DoTranslation("Usage:") + " list [oneDirectory]" + vbNewLine +
              "       list: " + DoTranslation("to get current directory."), True, ColTypes.Neutral)

        ElseIf command = "loteresp" Then

            Write(DoTranslation("Usage:") + " loteresp: " + DoTranslation("Respond to love or hate comments."), True, ColTypes.Neutral)

        ElseIf command = "lsdbgdev" Then

            Write(DoTranslation("Usage:") + " lsdbgdev" + vbNewLine +
              "       lsdbgdev: " + DoTranslation("Lists all connected debugging devices."), True, ColTypes.Neutral)

        ElseIf command = "lsmail" Then

            Write(DoTranslation("Usage:") + " lsmail [emailAddress]", True, ColTypes.Neutral)

        ElseIf command = "lockscreen" Then

            Write(DoTranslation("Usage:") + " lockscreen: " + DoTranslation("Locks your screen with the password.") + vbNewLine +
              "       " + DoTranslation("Friends of") + " lockscreen: savescreen", True, ColTypes.Neutral)

        ElseIf command = "logout" Then

            Write(DoTranslation("Usage:") + " logout: " + DoTranslation("Logs you out of the user.") + vbNewLine +
              "       " + DoTranslation("Friends of") + " logout: reboot, shutdown", True, ColTypes.Neutral)

        ElseIf command = "mathbee" Then

            Write(DoTranslation("Usage:") + " mathbee: " + DoTranslation("See if you can solve mathematical equations on time"), True, ColTypes.Neutral)

        ElseIf command = "md" Then

            Write(DoTranslation("Usage:") + " md <anything>", True, ColTypes.Neutral)

        ElseIf command = "mkfile" Then

            Write(DoTranslation("Usage:") + " mkfile <anything>", True, ColTypes.Neutral)

        ElseIf command = "mktheme" Then

            Write(DoTranslation("Usage:") + " mktheme <themeName>", True, ColTypes.Neutral)

        ElseIf command = "modinfo" Then

            Write(DoTranslation("Usage:") + " modinfo <mod>", True, ColTypes.Neutral)

        ElseIf command = "move" Then

            Write(DoTranslation("Usage:") + " move <source> <target>: " + DoTranslation("To move files to another directory"), True, ColTypes.Neutral)

        ElseIf command = "netinfo" Then

            Write(DoTranslation("Usage:") + " netinfo: " + DoTranslation("Get every network information"), True, ColTypes.Neutral)

        ElseIf command = "perm" Then

            Write(DoTranslation("Usage:") + " perm <userName> <Administrator/Disabled/Anonymous> <Allow/Disallow>", True, ColTypes.Neutral)

        ElseIf command = "ping" Then

            Write(DoTranslation("Usage:") + " ping [times] <Address1> <Address2> ...", True, ColTypes.Neutral)

        ElseIf command = "put" Then

            Write(DoTranslation("Usage:") + " put <FileName> <URL> [username]", True, ColTypes.Neutral)

        ElseIf command = "rm" Then

            Write(DoTranslation("Usage:") + " rm <directory/file>", True, ColTypes.Neutral)

        ElseIf command = "rdebug" Then

            Write(DoTranslation("Usage:") + " rdebug: " + DoTranslation("Enables or disables remote debugging."), True, ColTypes.Neutral)

        ElseIf command = "reboot" Then

            Write(DoTranslation("Usage:") + " reboot [ip] [port]: " + DoTranslation("Restarts your simulated computer.") + vbNewLine +
              "       " + DoTranslation("Friends of") + " reboot: shutdown, logout", True, ColTypes.Neutral)

        ElseIf command = "reloadmods" Then

            Write(DoTranslation("Usage:") + " reloadmods: " + DoTranslation("Reloads modifications."), True, ColTypes.Neutral)

        ElseIf command = "reloadconfig" Then

            Write(DoTranslation("Usage:") + " reloadconfig: " + DoTranslation("Reloads the configuration that is changed by the end-user or by tool.") + vbNewLine +
              "       " + DoTranslation("Colors doesn't require a restart, but most of the settings require you to restart."), True, ColTypes.Neutral)

        ElseIf command = "reloadsaver" Then

            Write(DoTranslation("Usage:") + " reloadsaver <customsaver>" + vbNewLine +
              "       " + DoTranslation("where customsaver will be") + " {0}", True, ColTypes.Neutral, String.Join(", ", ScreensaverFiles))

        ElseIf command = "reportbug" Then

            Write(DoTranslation("Usage:") + " reportbug", True, ColTypes.Neutral)

        ElseIf command = "rexec" Then

            Write(DoTranslation("Usage:") + " rexec <address> [port] <command>: " + DoTranslation("Remotely executes a command to remote PC"), True, ColTypes.Neutral)

        ElseIf command = "rmuser" Then

            Write(DoTranslation("Usage:") + " rmuser <Username>", True, ColTypes.Neutral)

        ElseIf command = "rss" Then

            Write(DoTranslation("Usage:") + " rss [feedlink]", True, ColTypes.Neutral)

        ElseIf command = "savecurrdir" Then

            Write(DoTranslation("Usage:") + " savecurrdir", True, ColTypes.Neutral)

        ElseIf command = "savescreen" Then

            Write(DoTranslation("Usage:") + " savescreen [saver]: " + DoTranslation("shows you a selected screensaver, while protecting your screen from burn outs.") + vbNewLine +
              "       " + DoTranslation("Friends of") + " savescreen: lockscreen", True, ColTypes.Neutral)

        ElseIf command = "search" Then

            Write(DoTranslation("Usage:") + " search <Regexp> <File>", True, ColTypes.Neutral)

        ElseIf command = "searchword" Then

            Write(DoTranslation("Usage:") + " search <StringEnclosedInDoubleQuotes> <File>", True, ColTypes.Neutral)

        ElseIf command = "setsaver" Then

            Write(DoTranslation("Usage:") + " setsaver <customsaver/{0}>", True, ColTypes.Neutral, String.Join("/", ScrnSvrdb.Keys))
            If CSvrdb.Count > 0 Then
                Write("       " + DoTranslation("where customsaver will be") + " {0}", True, ColTypes.Neutral, String.Join(", ", CSvrdb.Keys))
            End If

        ElseIf command = "setthemes" Then

            Write(DoTranslation("Usage:") + " setthemes <Theme>" + vbNewLine +
              "       " + "<Theme>: ThemeName.json, " + String.Join(", ", colorTemplates.Keys), True, ColTypes.Neutral)

        ElseIf command = "settings" Then

            Write(DoTranslation("Usage:") + " settings", True, ColTypes.Neutral)

        ElseIf command = "set" Then

            Write(DoTranslation("Usage:") + " set <$variable> <value>", True, ColTypes.Neutral)

        ElseIf command = "sftp" Then

            Write(DoTranslation("Usage:") + " sftp [server]: " + DoTranslation("Initializes the SFTP shell."), True, ColTypes.Neutral)

        ElseIf command = "shownotifs" Then

            Write(DoTranslation("Usage:") + " shownotifs: " + DoTranslation("Shows all received notifications"), True, ColTypes.Neutral)

        ElseIf command = "showtd" Then

            Write(DoTranslation("Usage:") + " showtd: " + DoTranslation("Shows the date and time."), True, ColTypes.Neutral)

        ElseIf command = "showtdzone" Then

            Write(DoTranslation("Usage:") + " showtdzone <timezone/all>: " + DoTranslation("Shows the date and time in zones."), True, ColTypes.Neutral)

        ElseIf command = "shutdown" Then

            Write(DoTranslation("Usage:") + " shutdown [ip] [port]: " + DoTranslation("Shuts down your simulated computer.") + vbNewLine +
              "       " + DoTranslation("Friends of") + " shutdown: reboot, logout", True, ColTypes.Neutral)

        ElseIf command = "speedpress" Then

            Write(DoTranslation("Usage:") + " speedpress <e/m/h>: " + DoTranslation("See if you can press a key on time"), True, ColTypes.Neutral)

        ElseIf command = "spellbee" Then

            Write(DoTranslation("Usage:") + " spellbee: " + DoTranslation("See if you can spell words correctly on time"), True, ColTypes.Neutral)

        ElseIf command = "sshell" Then

            Write(DoTranslation("Usage:") + " sshell <address:port> <username>: " + DoTranslation("Connects to an SSH server."), True, ColTypes.Neutral)

        ElseIf command = "sshcmd" Then

            Write(DoTranslation("Usage:") + " sshcmd <address:port> <username> ""<command>"": " + DoTranslation("Connects to an SSH server to execute a command."), True, ColTypes.Neutral)

        ElseIf command = "sumfile" Then

            Write(DoTranslation("Usage:") + " sumfile <MD5/SHA1/SHA256/SHA512/all> <file>: " + DoTranslation("Calculates file sums."), True, ColTypes.Neutral)

        ElseIf command = "sumfiles" Then

            Write(DoTranslation("Usage:") + " sumfiles <MD5/SHA1/SHA256/SHA512/all> <dir> [outputFile]", True, ColTypes.Neutral)

        ElseIf command = "sysinfo" Then

            Write(DoTranslation("Usage:") + " sysinfo: " + DoTranslation("Shows system information and versions."), True, ColTypes.Neutral)

        ElseIf command = "unblockdbgdev" Then

            Write(DoTranslation("Usage:") + " unblockdbgdev <ipaddress>", True, ColTypes.Neutral)

        ElseIf command = "unzip" Then

            Write(DoTranslation("Usage:") + " unzip <zipfile> [path] [-createdir]", True, ColTypes.Neutral)

        ElseIf command = "update" Then

            Write(DoTranslation("Usage:") + " update: " + DoTranslation("System update"), True, ColTypes.Neutral)

        ElseIf command = "usermanual" Then

            Write(DoTranslation("Usage:") + " usermanual: " + DoTranslation("Takes you to our GitHub Wiki."), True, ColTypes.Neutral)

        ElseIf command = "verify" Then

            Write(DoTranslation("Usage:") + " verify <MD5/SHA1/SHA256/SHA512> <calculatedhash> <hashfile/expectedhash> <file>", True, ColTypes.Neutral)

        ElseIf command = "weather" Then

            Write(DoTranslation("Usage:") + " weather <CityID/CityName/listcities>: " + DoTranslation("Shows weather info for specified city. Uses OpenWeatherMap.") + vbNewLine +
                                                                                    DoTranslation("You can always consult http://bulk.openweathermap.org/sample/city.list.json.gz for the list of cities with their IDs.") + " " + DoTranslation("Or, pass ""listcities"" to this command."), True, ColTypes.Neutral)

        ElseIf command = "wrap" Then

            Dim WrappableCmds As New ArrayList
            For Each CommandInfo As CommandInfo In Commands.Values
                If CommandInfo.Wrappable Then WrappableCmds.Add(CommandInfo.Command)
            Next
            Write(DoTranslation("Usage:") + " wrap <command>", True, ColTypes.Neutral)
            Write("       " + DoTranslation("Wrappable commands:") + " {0}", True, ColTypes.Neutral, String.Join(", ", WrappableCmds.ToArray))

        ElseIf command = "zip" Then

            Write(DoTranslation("Usage:") + " zip <zipfile> <path> [-fast/-nocomp] [-nobasedir]", True, ColTypes.Neutral)

        ElseIf command = "zipshell" Then

            Write(DoTranslation("Usage:") + " zipshell <zipfile>", True, ColTypes.Neutral)

        Else

            Write(DoTranslation("No help for command ""{0}""."), True, ColTypes.Error, command)

        End If

    End Sub

End Module
