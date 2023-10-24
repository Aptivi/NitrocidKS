
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

Imports System.Globalization
Imports System.Reflection

Public Module ToolPrompts

    ''' <summary>
    ''' Key type for settings entry
    ''' </summary>
    Enum SettingsKeyType
        SUnknown
        SBoolean
        SInt
        SString
        SLongString
        SSelection
        SList
        SVariant
    End Enum

    ''' <summary>
    ''' Main page
    ''' </summary>
    Sub OpenMainPage()
        Dim PromptFinished As Boolean
        Dim AnswerString As String
        Dim AnswerInt As Integer
        Dim MaxSections As Integer = 8

        While Not PromptFinished
            Console.Clear()
            'List sections
            Write("*) " + DoTranslation("Welcome to Settings!") + vbNewLine, True, ColTypes.Neutral)
            Write(DoTranslation("Select section:") + vbNewLine, True, ColTypes.Neutral)
            Write("1) " + DoTranslation("General Settings..."), True, ColTypes.Option)
            Write("2) " + DoTranslation("Hardware Settings..."), True, ColTypes.Option)
            Write("3) " + DoTranslation("Login Settings..."), True, ColTypes.Option)
            Write("4) " + DoTranslation("Shell Settings..."), True, ColTypes.Option)
            Write("5) " + DoTranslation("Filesystem Settings..."), True, ColTypes.Option)
            Write("6) " + DoTranslation("Network Settings..."), True, ColTypes.Option)
            Write("7) " + DoTranslation("Screensaver Settings..."), True, ColTypes.Option)
            Write("8) " + DoTranslation("Miscellaneous Settings...") + vbNewLine, True, ColTypes.Option)
            Write("9) " + DoTranslation("Save Settings"), True, ColTypes.Option)
            Write("10) " + DoTranslation("Exit"), True, ColTypes.Option)

            'Prompt user and check for input
            Console.WriteLine()
            Write("> ", False, ColTypes.Input)
            AnswerString = Console.ReadLine
            Wdbg("I", "User answered {0}", AnswerString)
            Console.WriteLine()

            Wdbg("I", "Is the answer numeric? {0}", IsNumeric(AnswerString))
            If Integer.TryParse(AnswerString, AnswerInt) Then
                Wdbg("I", "Succeeded. Checking the answer if it points to the right direction...")
                If AnswerInt >= 1 And AnswerInt <= MaxSections Then
                    Wdbg("I", "Opening section {0}...", AnswerInt)
                    OpenSection(AnswerString)
                ElseIf AnswerInt = MaxSections + 1 Then 'Save Settings
                    Wdbg("I", "Saving settings...")
                    Try
                        CreateConfig()
                        SaveCustomSaverSettings()
                    Catch ex As Exception
                        Write(ex.Message, True, ColTypes.Error)
                        WStkTrc(ex)
                        Console.ReadKey()
                    End Try
                ElseIf AnswerInt = MaxSections + 2 Then 'Exit
                    Wdbg("W", "Exiting...")
                    PromptFinished = True
                Else
                    Wdbg("W", "Option is not valid. Returning...")
                    Write(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerInt)
                    Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            Else
                Wdbg("W", "Answer is not numeric.")
                Write(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                Console.ReadKey()
            End If
        End While
    End Sub

    ''' <summary>
    ''' Open section
    ''' </summary>
    ''' <param name="SectionNum">Section number</param>
    Sub OpenSection(SectionNum As String, ParamArray SectionParameters() As Object)
        'General variables
        Dim MaxOptions As Integer = 0
        Dim SectionFinished As Boolean
        Dim AnswerString As String
        Dim AnswerInt As Integer
        Dim BuiltinSavers As Integer = 17

        'Section-specific variables
        Dim ConfigurableScreensavers As New List(Of String)

        While Not SectionFinished
            Console.Clear()
            'List options
            Select Case SectionNum
                Case "1" 'General
                    MaxOptions = 6
                    Write("*) " + DoTranslation("General Settings...") + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists all general kernel settings, mainly for maintaining the kernel.") + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Prompt for Arguments on Boot") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(argsOnBoot)))
                    Write("2) " + DoTranslation("Maintenance Mode Trigger") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(maintenance)))
                    Write("3) " + DoTranslation("Change Root Password..."), True, ColTypes.Option)
                    Write("4) " + DoTranslation("Check for Updates on Startup") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(CheckUpdateStart)))
                    Write("5) " + DoTranslation("Change Culture when Switching Languages") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(LangChangeCulture)))
                    Write("6) " + DoTranslation("Culture of") + " {0} [{1}]", True, ColTypes.Option, currentLang, CurrentCult.Name)
                Case "1.3" 'Change Root Password...
                    MaxOptions = 2
                    Write("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Change Root Password...") + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lets you manage root password creation.") + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Change Root Password?") + " [{0}]", True, ColTypes.Option, GetConfigValue("setRootPasswd"))
                    Write("2) " + DoTranslation("Set Root Password..."), True, ColTypes.Option)
                Case "2" 'Hardware
                    MaxOptions = 2
                    Write("*) " + DoTranslation("Hardware Settings...") + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section changes hardware probe behavior.") + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Quiet Probe") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(quietProbe)))
                    Write("2) " + DoTranslation("Full Probe") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(FullProbe)))
                Case "3" 'Login
                    MaxOptions = 3
                    Write("*) " + DoTranslation("Login Settings...") + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section represents the login settings. Log out of your account for the changes to take effect.") + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Show MOTD on Log-in") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(showMOTD)))
                    Write("2) " + DoTranslation("Clear Screen on Log-in") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(clsOnLogin)))
                    Write("3) " + DoTranslation("Show available usernames") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ShowAvailableUsers)))
                Case "4" 'Shell
                    MaxOptions = 9
                    Write("*) " + DoTranslation("Shell Settings...") + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists the shell settings.") + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Colored Shell") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ColoredShell)))
                    Write("2) " + DoTranslation("Simplified Help Command") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(simHelp)))
                    Write("3) " + DoTranslation("Current Directory", currentLang) + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(CurrDir)))
                    Write("4) " + DoTranslation("Lookup Directories", currentLang) + " [{0}]", True, ColTypes.Option, PathsToLookup.Split(PathLookupDelimiter).Length)
                    Write("5) " + DoTranslation("Prompt Style", currentLang) + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ShellPromptStyle)))
                    Write("6) " + DoTranslation("FTP Prompt Style", currentLang) + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(FTPShellPromptStyle)))
                    Write("7) " + DoTranslation("Mail Prompt Style", currentLang) + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(MailShellPromptStyle)))
                    Write("8) " + DoTranslation("SFTP Prompt Style", currentLang) + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(SFTPShellPromptStyle)))
                    Write("9) " + DoTranslation("Custom colors...", currentLang), True, ColTypes.Option)
                Case "4.9" 'Custom colors...
                    MaxOptions = 15
                    Write("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("Custom colors...") + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lets you choose what type of color do you want to change.") + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Input color") + " [{0}]", True, ColTypes.Option, InputColor)
                    Write("2) " + DoTranslation("License color") + " [{0}]", True, ColTypes.Option, LicenseColor)
                    Write("3) " + DoTranslation("Continuable kernel error color") + " [{0}]", True, ColTypes.Option, ContKernelErrorColor)
                    Write("4) " + DoTranslation("Uncontinuable kernel error color") + " [{0}]", True, ColTypes.Option, UncontKernelErrorColor)
                    Write("5) " + DoTranslation("Host name color") + " [{0}]", True, ColTypes.Option, HostNameShellColor)
                    Write("6) " + DoTranslation("User name color") + " [{0}]", True, ColTypes.Option, UserNameShellColor)
                    Write("7) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, BackgroundColor)
                    Write("8) " + DoTranslation("Neutral text color") + " [{0}]", True, ColTypes.Option, NeutralTextColor)
                    Write("9) " + DoTranslation("List entry color") + " [{0}]", True, ColTypes.Option, ListEntryColor)
                    Write("10) " + DoTranslation("List value color") + " [{0}]", True, ColTypes.Option, ListValueColor)
                    Write("11) " + DoTranslation("Stage color") + " [{0}]", True, ColTypes.Option, StageColor)
                    Write("12) " + DoTranslation("Error color") + " [{0}]", True, ColTypes.Option, ErrorColor)
                    Write("13) " + DoTranslation("Warning color") + " [{0}]", True, ColTypes.Option, WarningColor)
                    Write("14) " + DoTranslation("Option color") + " [{0}]", True, ColTypes.Option, OptionColor)
                    Write("15) " + DoTranslation("Banner color") + " [{0}]", True, ColTypes.Option, BannerColor)
                Case "5" 'Filesystem
                    MaxOptions = 6
                    Write("*) " + DoTranslation("Filesystem Settings...") + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists the filesystem settings.") + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Filesystem sort mode") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(SortMode)))
                    Write("2) " + DoTranslation("Filesystem sort direction") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(SortDirection)))
                    Write("3) " + DoTranslation("Debug Size Quota in Bytes") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(DebugQuota)))
                    Write("4) " + DoTranslation("Show Hidden Files") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(HiddenFiles)))
                    Write("5) " + DoTranslation("Size parse mode") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(FullParseMode)))
                    Write("6) " + DoTranslation("Show progress on filesystem operations") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ShowFilesystemProgress)))
                Case "6" 'Network
                    MaxOptions = 12
                    Write("*) " + DoTranslation("Network Settings...") + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists the network settings, like the FTP shell, the network-related command settings, and the remote debug settings.") + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Debug Port") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(DebugPort)))
                    Write("2) " + DoTranslation("Download Retry Times") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(DRetries)))
                    Write("3) " + DoTranslation("Upload Retry Times") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(URetries)))
                    Write("4) " + DoTranslation("Show progress bar while downloading or uploading from ""get"" or ""put"" command") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ShowProgress)))
                    Write("5) " + DoTranslation("Log FTP username") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(FTPLoggerUsername)))
                    Write("6) " + DoTranslation("Log FTP IP address") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(FTPLoggerIP)))
                    Write("7) " + DoTranslation("Return only first FTP profile") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(FTPFirstProfileOnly)))
                    Write("8) " + DoTranslation("Show mail message preview") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ShowPreview)))
                    Write("9) " + DoTranslation("Record chat to debug log") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(RecordChatToDebugLog)))
                    Write("10) " + DoTranslation("Show SSH banner") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(SSHBanner)))
                    Write("11) " + DoTranslation("Enable RPC") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(RPCEnabled)))
                    Write("12) " + DoTranslation("RPC Port") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(RPCPort)))
                Case "7" 'Screensaver
                    MaxOptions = BuiltinSavers + 1 'Screensavers + Keys
                    Write("*) " + DoTranslation("Screensaver Settings...") + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists all the screensavers and their available settings.") + vbNewLine, True, ColTypes.Neutral)

                    'Populate kernel screensavers
                    Write("1) ColorMix...", True, ColTypes.Option)
                    Write("2) Matrix...", True, ColTypes.Option)
                    Write("3) GlitterMatrix...", True, ColTypes.Option)
                    Write("4) Disco...", True, ColTypes.Option)
                    Write("5) Lines...", True, ColTypes.Option)
                    Write("6) GlitterColor...", True, ColTypes.Option)
                    Write("7) BouncingText...", True, ColTypes.Option)
                    Write("8) Dissolve...", True, ColTypes.Option)
                    Write("9) BouncingBlock...", True, ColTypes.Option)
                    Write("10) ProgressClock...", True, ColTypes.Option)
                    Write("11) Lighter...", True, ColTypes.Option)
                    Write("12) Fader...", True, ColTypes.Option)
                    Write("13) Typo...", True, ColTypes.Option)
                    Write("14) Wipe...", True, ColTypes.Option)
                    Write("15) HackUserFromAD...", True, ColTypes.Option)
                    Write("16) AptErrorSim...", True, ColTypes.Option)
                    Write("17) Marquee...", True, ColTypes.Option)

                    'Populate custom screensavers
                    For Each CustomSaver As String In CSvrdb.Keys
                        If CSvrdb(CustomSaver).SaverSettings?.Count >= 1 Then
                            ConfigurableScreensavers.Add(CustomSaver)
                            Write("{0}) {1}...", True, ColTypes.Option, MaxOptions, CustomSaver)
                            MaxOptions += 1
                        End If
                    Next

                    'Populate general screensaver settings
                    Write("{0}) " + DoTranslation("Screensaver Timeout in ms") + " [{1}]", True, ColTypes.Option, MaxOptions, GetConfigValue(NameOf(ScrnTimeout)))
                Case "7.1" 'Screensaver > ColorMix
                    MaxOptions = 3
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > ColorMix" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " ColorMix." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ColorMix255Colors)))
                    Write("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ColorMixTrueColor)))
                    Write("3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ColorMixDelay)))
                Case "7.2" 'Screensaver > Matrix
                    MaxOptions = 1
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > Matrix" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " Matrix." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(MatrixDelay)))
                Case "7.3" 'Screensaver > GlitterMatrix
                    MaxOptions = 1
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > GlitterMatrix" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " GlitterMatrix." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(GlitterMatrixDelay)))
                Case "7.4" 'Screensaver > Disco
                    MaxOptions = 4
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > Disco" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " Disco." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(Disco255Colors)))
                    Write("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(DiscoTrueColor)))
                    Write("3) " + DoTranslation("Cycle colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(DiscoCycleColors)))
                    Write("4) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(DiscoDelay)))
                Case "7.5" 'Screensaver > Lines
                    MaxOptions = 3
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > Lines" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " Lines." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(Lines255Colors)))
                    Write("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(LinesTrueColor)))
                    Write("3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(LinesDelay)))
                Case "7.6" 'Screensaver > GlitterColor
                    MaxOptions = 3
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > GlitterColor" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " GlitterColor." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(GlitterColor255Colors)))
                    Write("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(GlitterColorTrueColor)))
                    Write("3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(GlitterColorDelay)))
                Case "7.7" 'Screensaver > BouncingText
                    MaxOptions = 4
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > BouncingText" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " BouncingText." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(BouncingText255Colors)))
                    Write("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(BouncingTextTrueColor)))
                    Write("3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(BouncingTextDelay)))
                    Write("4) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(BouncingTextWrite)))
                Case "7.8" 'Screensaver > Dissolve
                    MaxOptions = 2
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > Dissolve" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " Dissolve." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(Dissolve255Colors)))
                    Write("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(DissolveTrueColor)))
                Case "7.9" 'Screensaver > BouncingBlock
                    MaxOptions = 3
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > BouncingBlock" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " BouncingBlock." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(BouncingBlock255Colors)))
                    Write("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(BouncingBlockTrueColor)))
                    Write("3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(BouncingBlockDelay)))
                Case "7.10" 'Screensaver > ProgressClock
                    MaxOptions = 8
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " ProgressClock." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClock255Colors)))
                    Write("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClockTrueColor)))
                    Write("3) " + DoTranslation("Cycle colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClockCycleColors)))
                    Write("4) " + DoTranslation("Color of Seconds Bar") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClockSecondsProgressColor)))
                    Write("5) " + DoTranslation("Color of Minutes Bar") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClockMinutesProgressColor)))
                    Write("6) " + DoTranslation("Color of Hours Bar") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClockHoursProgressColor)))
                    Write("7) " + DoTranslation("Color of Information") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClockProgressColor)))
                    Write("8) " + DoTranslation("Ticks to change color") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClockCycleColorsTicks)))
                Case "7.11" 'Screensaver > Lighter
                    MaxOptions = 4
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > Lighter" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " Lighter." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(Lighter255Colors)))
                    Write("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(LighterTrueColor)))
                    Write("3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(LighterDelay)))
                    Write("4) " + DoTranslation("Max Positions Count") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(LighterMaxPositions)))
                Case "7.12" 'Screensaver > Fader
                    MaxOptions = 4
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > Fader" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " Fader." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(FaderDelay)))
                    Write("2) " + DoTranslation("Fade Out Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(FaderFadeOutDelay)))
                    Write("3) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(FaderWrite)))
                    Write("4) " + DoTranslation("Max Fade Steps") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(FaderMaxSteps)))
                Case "7.13" 'Screensaver > Typo
                    MaxOptions = 6
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > Typo" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " Typo." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(TypoDelay)))
                    Write("2) " + DoTranslation("Write Again Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(TypoWriteAgainDelay)))
                    Write("3) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(TypoWrite)))
                    Write("4) " + DoTranslation("Minimum writing speed in WPM") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(TypoWritingSpeedMin)))
                    Write("5) " + DoTranslation("Maximum writing speed in WPM") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(TypoWritingSpeedMax)))
                    Write("6) " + DoTranslation("Probability of typo in percent") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(TypoMissStrikePossibility)))
                Case "7.14" 'Screensaver > Wipe
                    MaxOptions = 4
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > Wipe" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " Wipe." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(Wipe255Colors)))
                    Write("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(WipeTrueColor)))
                    Write("3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(WipeDelay)))
                    Write("4) " + DoTranslation("Wipes to change direction") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(WipeWipesNeededToChangeDirection)))
                Case "7.15" 'Screensaver > HackUserFromAD
                    MaxOptions = 1
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > HackUserFromAD" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " HackUserFromAD." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Hacker Mode") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(HackUserFromADHackerMode)))
                Case "7.16" 'Screensaver > AptErrorSim
                    MaxOptions = 1
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > AptErrorSim" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " AptErrorSim." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Hacker Mode") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(AptErrorSimHackerMode)))
                Case "7.17" 'Screensaver > Marquee
                    MaxOptions = 5
                    Write("*) " + DoTranslation("Screensaver Settings...") + " > Marquee" + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("This section lists screensaver settings for") + " Marquee." + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(Marquee255Colors)))
                    Write("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(MarqueeTrueColor)))
                    Write("3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(MarqueeDelay)))
                    Write("4) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(MarqueeWrite)))
                    Write("5) " + DoTranslation("Always centered") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(MarqueeAlwaysCentered)))
                Case "7." + $"{If(SectionParameters.Length <> 0, SectionParameters(0), $"{BuiltinSavers + 1}")}" 'Screensaver > a custom saver
                    Dim SaverIndex As Integer = SectionParameters(0) - BuiltinSavers - 1
                    Dim Configurables As List(Of String) = SectionParameters(1)
                    Dim OptionNumber As Integer = 1
                    If CSvrdb(Configurables(SaverIndex)).SaverSettings IsNot Nothing Then
                        MaxOptions = CSvrdb(Configurables(SaverIndex)).SaverSettings.Count
                        Write("*) " + DoTranslation("Screensaver Settings...") + " > {0}" + vbNewLine, True, ColTypes.Neutral, Configurables(SaverIndex))
                        Write(DoTranslation("This section lists screensaver settings for") + " {0}." + vbNewLine, True, ColTypes.Neutral, Configurables(SaverIndex))
                        For Each Setting As String In CSvrdb(Configurables(SaverIndex)).SaverSettings.Keys
                            Write("{0}) {1} [{2}]", True, ColTypes.Option, OptionNumber, Setting, CSvrdb(Configurables(SaverIndex)).SaverSettings(Setting))
                            OptionNumber += 1
                        Next
                    End If
                Case "8" 'Misc
                    MaxOptions = 7
                    Write("*) " + DoTranslation("Miscellaneous Settings...") + vbNewLine, True, ColTypes.Neutral)
                    Write(DoTranslation("Settings that don't fit in their appropriate sections land here.") + vbNewLine, True, ColTypes.Neutral)
                    Write("1) " + DoTranslation("Show Time/Date on Upper Right Corner") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(CornerTD)))
                    Write("2) " + DoTranslation("Marquee on startup") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(StartScroll)))
                    Write("3) " + DoTranslation("Long Time and Date") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(LongTimeDate)))
                    Write("4) " + DoTranslation("Preferred Unit for Temperature") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(PreferredUnit)))
                    Write("5) " + DoTranslation("Enable text editor autosave") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(TextEdit_AutoSaveFlag)))
                    Write("6) " + DoTranslation("Text editor autosave interval") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(TextEdit_AutoSaveInterval)))
                    Write("7) " + DoTranslation("Wrap list outputs") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(WrapListOutputs)))
                Case Else 'Invalid section
                    Write("*) ???" + vbNewLine, True, ColTypes.Neutral)
                    Write("X) " + DoTranslation("Invalid section entered. Please go back."), True, ColTypes.Error)
            End Select
            Console.WriteLine()
            Write("{0}) " + DoTranslation("Go Back...") + vbNewLine, True, ColTypes.Option, MaxOptions + 1)
            Wdbg("W", "Section {0} has {1} selections.", SectionNum, MaxOptions)

            'Prompt user and check for input
            Write("> ", False, ColTypes.Input)
            AnswerString = Console.ReadLine
            Wdbg("I", "User answered {0}", AnswerString)
            Console.WriteLine()

            Wdbg("I", "Is the answer numeric? {0}", IsNumeric(AnswerString))
            If Integer.TryParse(AnswerString, AnswerInt) Then
                Wdbg("I", "Succeeded. Checking the answer if it points to the right direction...")
                If AnswerInt >= 1 And AnswerInt <= MaxOptions Then
                    If AnswerInt = 3 And SectionNum = "1" Then
                        Wdbg("I", "Tried to open special section. Opening section 1.3...")
                        OpenSection("1.3")
                    ElseIf AnswerInt <> MaxOptions And SectionNum = "1.3" Then
                        Wdbg("I", "Tried to open special section. Opening key {0} in section 1.3...", AnswerString)
                        OpenKey("1.3", AnswerInt)
                    ElseIf AnswerInt = 9 And SectionNum = "4" Then
                        Wdbg("I", "Tried to open subsection. Opening section 4.9...")
                        OpenSection("4.9")
                    ElseIf AnswerInt <> MaxOptions And SectionNum = "4.9" Then
                        Wdbg("I", "Tried to open subsection. Opening key {0} in section 4.9...", AnswerString)
                        OpenKey("4.9", AnswerInt)
                    ElseIf AnswerInt <> MaxOptions And SectionNum = "7" Then
                        Wdbg("I", "Tried to open subsection. Opening section 7.{0}...", AnswerString)
                        Wdbg("I", "Arguments: AnswerInt: {0}, ConfigurableScreensavers: {1}", AnswerInt, ConfigurableScreensavers.Count)
                        OpenSection("7." + AnswerString, AnswerInt, ConfigurableScreensavers)
                    ElseIf AnswerInt = MaxOptions And SectionNum = "7" Then
                        Wdbg("I", "Opening key {0} from section {1} with argument {2}...", AnswerInt, SectionNum)
                        OpenKey(SectionNum, AnswerInt, MaxOptions)
                    Else
                        Wdbg("I", "Opening key {0} from section {1}...", AnswerInt, SectionNum)
                        OpenKey(SectionNum, AnswerInt)
                    End If
                ElseIf AnswerInt = MaxOptions + 1 Then 'Go Back...
                    Wdbg("I", "User requested exit. Returning...")
                    SectionFinished = True
                Else
                    Wdbg("W", "Option is not valid. Returning...")
                    Write(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerInt)
                    Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            Else
                Wdbg("W", "Answer is not numeric.")
                Write(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                Console.ReadKey()
            End If
        End While
    End Sub

    ''' <summary>
    ''' Open a key.
    ''' </summary>
    ''' <param name="Section">Section number</param>
    ''' <param name="KeyNumber">Key number</param>
    Sub OpenKey(Section As String, KeyNumber As Integer, ParamArray KeyParameters() As Object)
        Dim MaxKeyOptions As Integer = 0
        Dim KeyFinished As Boolean
        Dim KeyType As SettingsKeyType = SettingsKeyType.SUnknown
        Dim KeyVar As String = ""
        Dim KeyValue As Object = ""
        Dim VariantValue As Object = ""
        Dim VariantValueFromExternalPrompt As Boolean
        Dim AnswerString As String = ""
        Dim AnswerInt As Integer
        Dim SectionParts() As String = Section.Split(".")
        Dim ListJoinString As String = ""
        Dim TargetList As IEnumerable(Of Object)
        Dim SelectFrom As IEnumerable(Of Object)
        Dim NeutralizePaths As Boolean
        Dim BuiltinSavers As Integer = 17

        While Not KeyFinished
            Console.Clear()
            'List Keys for specified section
            Select Case Section
                Case "1" 'General
                    Select Case KeyNumber
                        Case 1 'Prompt for Arguments on Boot
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(argsOnBoot)
                            Write("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Prompt for Arguments on Boot") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Sets up the kernel so it prompts you for argument on boot."), True, ColTypes.Neutral)
                        Case 2 'Maintenance Mode Trigger
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(maintenance)
                            Write("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Maintenance Mode Trigger") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Triggers maintenance mode. This disables multiple accounts."), True, ColTypes.Neutral)
                        Case 3 'Change Root Password
                            OpenKey(Section, 1.3)
                        Case 4 'Check for Updates on Startup
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(CheckUpdateStart)
                            Write("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Check for Updates on Startup") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Each startup, it will check for updates."), True, ColTypes.Neutral)
                        Case 5 'Change Culture when Switching Languages
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(LangChangeCulture)
                            Write("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Change Culture when Switching Languages") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("When switching languages, change the month names, calendar, etc."), True, ColTypes.Neutral)
                        Case 6 'Culture of current language
                            MaxKeyOptions = 0
                            KeyType = SettingsKeyType.SSelection
                            KeyVar = NameOf(CurrentCult)
                            Write("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Culture of") + " {0}" + vbNewLine, True, ColTypes.Neutral, currentLang)
                            Write(DoTranslation("Which variant of {0} is being used to change the month names, calendar, etc.?"), True, ColTypes.Neutral, currentLang)
                            SelectFrom = GetCulturesFromCurrentLang()
                            If SelectFrom.Count > 0 Then
                                For Each Cult As CultureInfo In SelectFrom
                                    MaxKeyOptions += 1
                                    Write("{0}) {1} ({2})", True, ColTypes.Option, MaxKeyOptions, Cult.Name, Cult.EnglishName)
                                Next
                            Else
                                SelectFrom = New List(Of CultureInfo) From {New CultureInfo("en-US")}
                                MaxKeyOptions = 1
                                Write("1) en-US (English (United States))", True, ColTypes.Option)
                            End If
                        Case Else
                            Write("*) " + DoTranslation("General Settings...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "1.3" 'General -> Change Root Password
                    Select Case KeyNumber
                        Case 1
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(setRootPasswd)
                            Write("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Change Root Password...") + " > " + DoTranslation("Change Root Password?") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("If the kernel is started, it will set root password."), True, ColTypes.Neutral)
                        Case 2
                            Write("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Change Root Password...") + " > " + DoTranslation("Set Root Password...") + vbNewLine, True, ColTypes.Neutral)
                            If GetConfigValue(NameOf(setRootPasswd)) Then
                                KeyType = SettingsKeyType.SString
                                KeyVar = NameOf(RootPasswd)
                                Write(DoTranslation("Write the root password to be set. Don't worry; the password are shown as stars."), True, ColTypes.Neutral)
                            Else
                                Write("X) " + DoTranslation("Enable ""Change Root Password"" to use this option. Please go back."), True, ColTypes.Error)
                            End If
                        Case Else
                            Write("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Change Root Password...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "2" 'Hardware
                    Select Case KeyNumber
                        Case 1 'Quiet Probe
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(quietProbe)
                            Write("*) " + DoTranslation("Hardware Settings...") + " > " + DoTranslation("Quiet Probe") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Keep hardware probing messages silent."), True, ColTypes.Neutral)
                        Case 2 'Full Probe
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FullProbe)
                            Write("*) " + DoTranslation("Hardware Settings...") + " > " + DoTranslation("Full Probe") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("If true, probes all the hardware; else, will only probe the needed hardware."), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Hardware Settings...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "3" 'Login
                    Select Case KeyNumber
                        Case 1 'Show MOTD on Log-in
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(showMOTD)
                            Write("*) " + DoTranslation("Login Settings...") + " > " + DoTranslation("Show MOTD on Log-in") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Show Message of the Day before displaying login screen."), True, ColTypes.Neutral)
                        Case 2 'Clear Screen on Log-in
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(clsOnLogin)
                            Write("*) " + DoTranslation("Login Settings...") + " > " + DoTranslation("Clear Screen on Log-in") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Clear screen before displaying login screen."), True, ColTypes.Neutral)
                        Case 3 'Show Available Usernames
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowAvailableUsers)
                            Write("*) " + DoTranslation("Login Settings...") + " > " + DoTranslation("Show available usernames") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Shows available users if enabled."), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Login Settings...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "4" 'Shell
                    Select Case KeyNumber
                        Case 1 'Colored Shell
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ColoredShell)
                            Write("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("Colored Shell") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Gives the kernel color support"), True, ColTypes.Neutral)
                        Case 2 'Simplified Help Command
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(simHelp)
                            Write("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("Simplified Help Command") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Simplified help command for all the shells"), True, ColTypes.Neutral)
                        Case 3 'Current Directory
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(CurrDir)
                            Write("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("Current Directory") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Sets the shell's current directory. Write an absolute path to any existing directory."), True, ColTypes.Neutral)
                        Case 4 'Lookup Directories
                            KeyType = SettingsKeyType.SList
                            KeyVar = NameOf(PathsToLookup)
                            ListJoinString = PathLookupDelimiter
                            TargetList = GetPathList()
                            NeutralizePaths = True
                            Write("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("Lookup Directories") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Group of paths separated by the colon. It works the same as PATH. Write a full path to a folder or a folder name. When you're finished, write ""q"". Write a minus sign next to the path to remove an existing directory."), True, ColTypes.Neutral)
                        Case 5 'Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ShellPromptStyle)
                            Write("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("Prompt Style") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 6 'FTP Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(FTPShellPromptStyle)
                            Write("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("FTP Prompt Style") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 7 'Mail Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(MailShellPromptStyle)
                            Write("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("Mail Prompt Style") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 8 'SFTP Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(SFTPShellPromptStyle)
                            Write("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("SFTP Prompt Style") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Shell Settings...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "4.9" 'Shell -> Custom colors
                    Select Case KeyNumber
                        Case 1 'Input color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(InputColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(InputColor).Type = ColorType.TrueColor, If(New Color(InputColor).Type = ColorType._255Color, New Color(InputColor).PlainSequence, ConsoleColors.White), New Color(InputColor).R, New Color(InputColor).G, New Color(InputColor).B)
                        Case 2 'License color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(LicenseColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(LicenseColor).Type = ColorType.TrueColor, If(New Color(LicenseColor).Type = ColorType._255Color, New Color(LicenseColor).PlainSequence, ConsoleColors.White), New Color(LicenseColor).R, New Color(LicenseColor).G, New Color(LicenseColor).B)
                        Case 3 'Continuable kernel error color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ContKernelErrorColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ContKernelErrorColor).Type = ColorType.TrueColor, If(New Color(ContKernelErrorColor).Type = ColorType._255Color, New Color(ContKernelErrorColor).PlainSequence, ConsoleColors.White), New Color(ContKernelErrorColor).R, New Color(ContKernelErrorColor).G, New Color(ContKernelErrorColor).B)
                        Case 4 'Uncontinuable kernel error color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(UncontKernelErrorColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(UncontKernelErrorColor).Type = ColorType.TrueColor, If(New Color(UncontKernelErrorColor).Type = ColorType._255Color, New Color(UncontKernelErrorColor).PlainSequence, ConsoleColors.White), New Color(UncontKernelErrorColor).R, New Color(UncontKernelErrorColor).G, New Color(UncontKernelErrorColor).B)
                        Case 5 'Host name color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(HostNameShellColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(HostNameShellColor).Type = ColorType.TrueColor, If(New Color(HostNameShellColor).Type = ColorType._255Color, New Color(HostNameShellColor).PlainSequence, ConsoleColors.White), New Color(HostNameShellColor).R, New Color(HostNameShellColor).G, New Color(HostNameShellColor).B)
                        Case 6 'User name color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(UserNameShellColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(UserNameShellColor).Type = ColorType.TrueColor, If(New Color(UserNameShellColor).Type = ColorType._255Color, New Color(UserNameShellColor).PlainSequence, ConsoleColors.White), New Color(UserNameShellColor).R, New Color(UserNameShellColor).G, New Color(UserNameShellColor).B)
                        Case 7 'Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(BackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(BackgroundColor).Type = ColorType.TrueColor, If(New Color(BackgroundColor).Type = ColorType._255Color, New Color(BackgroundColor).PlainSequence, ConsoleColors.White), New Color(BackgroundColor).R, New Color(BackgroundColor).G, New Color(BackgroundColor).B)
                        Case 8 'Neutral text color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(NeutralTextColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(NeutralTextColor).Type = ColorType.TrueColor, If(New Color(NeutralTextColor).Type = ColorType._255Color, New Color(NeutralTextColor).PlainSequence, ConsoleColors.White), New Color(NeutralTextColor).R, New Color(NeutralTextColor).G, New Color(NeutralTextColor).B)
                        Case 9 'List entry color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ListEntryColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ListEntryColor).Type = ColorType.TrueColor, If(New Color(ListEntryColor).Type = ColorType._255Color, New Color(ListEntryColor).PlainSequence, ConsoleColors.White), New Color(ListEntryColor).R, New Color(ListEntryColor).G, New Color(ListEntryColor).B)
                        Case 10 'List value color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ListValueColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ListValueColor).Type = ColorType.TrueColor, If(New Color(ListValueColor).Type = ColorType._255Color, New Color(ListValueColor).PlainSequence, ConsoleColors.White), New Color(ListValueColor).R, New Color(ListValueColor).G, New Color(ListValueColor).B)
                        Case 11 'Stage color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(StageColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(StageColor).Type = ColorType.TrueColor, If(New Color(StageColor).Type = ColorType._255Color, New Color(StageColor).PlainSequence, ConsoleColors.White), New Color(StageColor).R, New Color(StageColor).G, New Color(StageColor).B)
                        Case 12 'Error color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ErrorColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ErrorColor).Type = ColorType.TrueColor, If(New Color(ErrorColor).Type = ColorType._255Color, New Color(ErrorColor).PlainSequence, ConsoleColors.White), New Color(ErrorColor).R, New Color(ErrorColor).G, New Color(ErrorColor).B)
                        Case 13 'Warning color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(WarningColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(WarningColor).Type = ColorType.TrueColor, If(New Color(WarningColor).Type = ColorType._255Color, New Color(WarningColor).PlainSequence, ConsoleColors.White), New Color(WarningColor).R, New Color(WarningColor).G, New Color(WarningColor).B)
                        Case 14 'Option color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(OptionColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(OptionColor).Type = ColorType.TrueColor, If(New Color(OptionColor).Type = ColorType._255Color, New Color(OptionColor).PlainSequence, ConsoleColors.White), New Color(OptionColor).R, New Color(OptionColor).G, New Color(OptionColor).B)
                        Case 15 'Banner color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(BannerColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(BannerColor).Type = ColorType.TrueColor, If(New Color(BannerColor).Type = ColorType._255Color, New Color(BannerColor).PlainSequence, ConsoleColors.White), New Color(BannerColor).R, New Color(BannerColor).G, New Color(BannerColor).B)
                    End Select
                Case "5" 'Filesystem
                    Select Case KeyNumber
                        Case 1 'Filesystem sort mode
                            MaxKeyOptions = 5
                            KeyType = SettingsKeyType.SSelection
                            KeyVar = NameOf(SortMode)
                            Write("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Filesystem sort mode") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Controls how the files will be sorted.") + vbNewLine, True, ColTypes.Neutral)
                            Write("1) " + DoTranslation("Full name"), True, ColTypes.Option)
                            Write("2) " + DoTranslation("File size"), True, ColTypes.Option)
                            Write("3) " + DoTranslation("Creation time"), True, ColTypes.Option)
                            Write("4) " + DoTranslation("Last write time"), True, ColTypes.Option)
                            Write("5) " + DoTranslation("Last access time"), True, ColTypes.Option)
                        Case 2 'Filesystem sort direction
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SSelection
                            KeyVar = NameOf(SortDirection)
                            Write("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Filesystem sort direction") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Controls the direction of filesystem sorting whether it's ascending or descending.") + vbNewLine, True, ColTypes.Neutral)
                            Write("1) " + DoTranslation("Ascending order"), True, ColTypes.Option)
                            Write("2) " + DoTranslation("Descending order"), True, ColTypes.Option)
                        Case 3 'Debug Size Quota in Bytes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DebugQuota)
                            Write("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Debug Size Quota in Bytes") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Write how many bytes can the debug log store. It must be numeric."), True, ColTypes.Neutral)
                        Case 4 'Show Hidden Files
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(HiddenFiles)
                            Write("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Show Hidden Files") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Shows hidden files."), True, ColTypes.Neutral)
                        Case 5 'Size parse mode
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FullParseMode)
                            Write("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Size parse mode") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("If enabled, the kernel will parse the whole folder for its total size. Else, will only parse the surface."), True, ColTypes.Neutral)
                        Case 6 'Show progress on filesystem operations
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowFilesystemProgress)
                            Write("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Show progress on filesystem operations") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Shows what file is being processed during the filesystem operations"), True, ColTypes.Neutral)
                    End Select
                Case "6" 'Network
                    Select Case KeyNumber
                        Case 1 'Debug Port
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DebugPort)
                            Write("*) " + DoTranslation("Network Settings...") + " > " + DoTranslation("Debug Port") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Write a remote debugger port. It must be numeric, and must not be already used. Otherwise, remote debugger will fail to open the port."), True, ColTypes.Neutral)
                        Case 2 'Download Retry Times
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DRetries)
                            Write("*) " + DoTranslation("Network Settings...") + " > " + DoTranslation("Download Retry Times") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Write how many times the ""get"" command should retry failed downloads. It must be numeric."), True, ColTypes.Neutral)
                        Case 3 'Upload Retry Times
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(URetries)
                            Write("*) " + DoTranslation("Network Settings...") + " > " + DoTranslation("Upload Retry Times") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Write how many times the ""put"" command should retry failed uploads. It must be numeric."), True, ColTypes.Neutral)
                        Case 4 'Show progress bar while downloading or uploading from "get" or "put" command
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowProgress)
                            Write("*) " + DoTranslation("Network Settings...") + " > " + DoTranslation("Show progress bar while downloading or uploading from ""get"" or ""put"" command") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("If true, it makes ""get"" or ""put"" show the progress bar while downloading or uploading."), True, ColTypes.Neutral)
                        Case 5 'Log FTP username
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FTPLoggerUsername)
                            Write("*) " + DoTranslation("Network Settings...") + " > " + DoTranslation("Log FTP username") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Whether or not to log FTP username."), True, ColTypes.Neutral)
                        Case 6 'Log FTP IP address
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FTPLoggerIP)
                            Write("*) " + DoTranslation("Network Settings...") + " > " + DoTranslation("Log FTP IP address") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Whether or not to log FTP IP address."), True, ColTypes.Neutral)
                        Case 7 'Return only first FTP profile
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FTPFirstProfileOnly)
                            Write("*) " + DoTranslation("Network Settings...") + " > " + DoTranslation("Return only first FTP profile") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Pick the first profile only when connecting."), True, ColTypes.Neutral)
                        Case 8 'Show mail message preview
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowPreview)
                            Write("*) " + DoTranslation("Network Settings...") + " > " + DoTranslation("Show mail message preview") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("When listing mail messages, show body preview."), True, ColTypes.Neutral)
                        Case 9 'Record chat to debug log
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(RecordChatToDebugLog)
                            Write("*) " + DoTranslation("Network Settings...") + " > " + DoTranslation("Record chat to debug log") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Records remote debug chat to debug log."), True, ColTypes.Neutral)
                        Case 10 'Show SSH banner
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(SSHBanner)
                            Write("*) " + DoTranslation("Network Settings...") + " > " + DoTranslation("Show SSH banner") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Shows the SSH server banner on connection."), True, ColTypes.Neutral)
                        Case 11 'Enable RPC
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(RPCEnabled)
                            Write("*) " + DoTranslation("Network Settings...") + " > " + DoTranslation("Enable RPC") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Whether or not to enable RPC."), True, ColTypes.Neutral)
                        Case 12 'RPC Port
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RPCPort)
                            Write("*) " + DoTranslation("Network Settings...") + " > " + DoTranslation("RPC Port") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Write an RPC port. It must be numeric, and must not be already used. Otherwise, RPC will fail to open the port."), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Network Settings...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7" 'Screensaver
                    Select Case KeyNumber
                        Case KeyParameters(0) 'Screensaver Timeout in ms
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ScrnTimeout)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > " + DoTranslation("Screensaver Timeout in ms") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Write when to launch screensaver after specified milliseconds. It must be numeric."), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.1" 'ColorMix
                    Select Case KeyNumber
                        Case 1 'ColorMix: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ColorMix255Colors)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates 255 color support for ColorMix."), True, ColTypes.Neutral)
                        Case 2 'ColorMix: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ColorMixTrueColor)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates true color support for ColorMix."), True, ColTypes.Neutral)
                        Case 3 'ColorMix: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixDelay)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > ColorMix > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.2" 'Matrix
                    Select Case KeyNumber
                        Case 1 'Matrix: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MatrixDelay)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Matrix > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Matrix > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.3" 'GlitterMatrix
                    Select Case KeyNumber
                        Case 1 'GlitterMatrix: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterMatrixDelay)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > GlitterMatrix > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > GlitterMatrix > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.4" 'Disco
                    Select Case KeyNumber
                        Case 1 'Disco: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Disco255Colors)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates 255 color support for Disco."), True, ColTypes.Neutral)
                        Case 2 'Disco: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(DiscoTrueColor)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates true color support for Disco."), True, ColTypes.Neutral)
                        Case 3 'Disco: Cycle colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(DiscoCycleColors)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Cycle colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Disco will cycle colors when enabled. Otherwise, select random colors."), True, ColTypes.Neutral)
                        Case 4 'Disco: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoDelay)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Disco >" + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Disco > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.5" 'Lines
                    Select Case KeyNumber
                        Case 1 'Lines: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Lines255Colors)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates 255 color support for Lines."), True, ColTypes.Neutral)
                        Case 2 'Lines: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(LinesTrueColor)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates true color support for Lines."), True, ColTypes.Neutral)
                        Case 3 'Lines: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesDelay)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Lines > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.6" 'GlitterColor
                    Select Case KeyNumber
                        Case 1 'GlitterColor: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(GlitterColor255Colors)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates 255 color support for GlitterColor."), True, ColTypes.Neutral)
                        Case 2 'GlitterColor: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(GlitterColorTrueColor)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates true color support for GlitterColor."), True, ColTypes.Neutral)
                        Case 3 'GlitterColor: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorDelay)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Lines > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.7" 'BouncingText
                    Select Case KeyNumber
                        Case 1 'BouncingText: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BouncingText255Colors)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates 255 color support for BouncingText."), True, ColTypes.Neutral)
                        Case 2 'BouncingText: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BouncingTextTrueColor)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates true color support for BouncingText."), True, ColTypes.Neutral)
                        Case 3 'BouncingText: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextDelay)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'BouncingText: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(BouncingTextWrite)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Text shown") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Write any text you want shown. Shorter is better."), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > BouncingText > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.8" 'Dissolve
                    Select Case KeyNumber
                        Case 1 'Dissolve: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Dissolve255Colors)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Dissolve > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates 255 color support for Dissolve."), True, ColTypes.Neutral)
                        Case 2 'Dissolve: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(DissolveTrueColor)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Dissolve > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates true color support for Dissolve."), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Dissolve > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.9" 'BouncingBlock
                    Select Case KeyNumber
                        Case 1 'BouncingBlock: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BouncingBlock255Colors)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates 255 color support for BouncingBlock."), True, ColTypes.Neutral)
                        Case 2 'BouncingBlock: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BouncingBlockTrueColor)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates true color support for BouncingBlock."), True, ColTypes.Neutral)
                        Case 3 'BouncingBlock: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockDelay)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > BouncingBlock > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.10" 'ProgressClock
                    Select Case KeyNumber
                        Case 1 'ProgressClock: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ProgressClock255Colors)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates 255 color support for ProgressClock."), True, ColTypes.Neutral)
                        Case 2 'ProgressClock: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ProgressClockTrueColor)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates true color support for ProgressClock."), True, ColTypes.Neutral)
                        Case 3 'ProgressClock: Cycle colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ProgressClockCycleColors)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Cycle colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("ProgressClock will select random colors if it's enabled. Otherwise, use colors from config."), True, ColTypes.Neutral)
                        Case 4 'ProgressClock: Color of Seconds Bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockSecondsProgressColor)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Color of Seconds Bar") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("The color of seconds progress bar. It can be 1-16, 1-255, or ""1-255;1-255;1-255""."), True, ColTypes.Neutral)
                        Case 5 'ProgressClock: Color of Minutes Bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockMinutesProgressColor)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Color of Minutes Bar") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("The color of minutes progress bar. It can be 1-16, 1-255, or ""1-255;1-255;1-255""."), True, ColTypes.Neutral)
                        Case 6 'ProgressClock: Color of Hours Bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockHoursProgressColor)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Color of Hours Bar") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("The color of hours progress bar. It can be 1-16, 1-255, or ""1-255;1-255;1-255""."), True, ColTypes.Neutral)
                        Case 7 'ProgressClock: Color of Information
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockProgressColor)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Color of Information") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("The color of date information. It can be 1-16, 1-255, or ""1-255;1-255;1-255""."), True, ColTypes.Neutral)
                        Case 8 'ProgressClock: Ticks to change color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockCycleColorsTicks)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Ticks to change color") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("If color cycling is enabled, how many ticks before changing colors in ProgressClock? 1 tick = 0.5 seconds"), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.11" 'Lighter
                    Select Case KeyNumber
                        Case 1 'Lighter: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Lighter255Colors)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates 255 color support for Lighter."), True, ColTypes.Neutral)
                        Case 2 'Lighter: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(LighterTrueColor)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates true color support for Lighter."), True, ColTypes.Neutral)
                        Case 3 'Lighter: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterDelay)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'Lighter: Max Positions Count
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMaxPositions)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Max Positions Count") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many positions are lit before dimming?"), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Lighter > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.12" 'Fader
                    Select Case KeyNumber
                        Case 1 'Fader: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderDelay)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Fader > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'Fader: Fade Out Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderFadeOutDelay)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Fader > " + DoTranslation("Fade Out Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many milliseconds to wait before fading out text?"), True, ColTypes.Neutral)
                        Case 3 'Fader: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(FaderWrite)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Fader > " + DoTranslation("Text shown") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Write any text you want shown. Shorter is better."), True, ColTypes.Neutral)
                        Case 4 'Fader: Max Fade Steps
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderMaxSteps)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Fader > " + DoTranslation("Max Fade Steps") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many fade steps to do?"), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Fader > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.13" 'Typo
                    Select Case KeyNumber
                        Case 1 'Typo: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoDelay)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Typo > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'Typo: Write Again Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoWriteAgainDelay)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Typo > " + DoTranslation("Write Again Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many milliseconds to wait before writing text again?"), True, ColTypes.Neutral)
                        Case 3 'Typo: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(TypoWrite)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Typo > " + DoTranslation("Text shown") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Write any text you want shown. Longer is better."), True, ColTypes.Neutral)
                        Case 4 'Typo: Minimum writing speed in WPM
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoWritingSpeedMin)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Typo > " + DoTranslation("Minimum writing speed in WPM") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Minimum writing speed in WPM"), True, ColTypes.Neutral)
                        Case 5 'Typo: Maximum writing speed in WPM
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoWritingSpeedMax)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Typo > " + DoTranslation("Maximum writing speed in WPM") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Maximum writing speed in WPM"), True, ColTypes.Neutral)
                        Case 6 'Typo: Probability of typo in percent
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoMissStrikePossibility)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Typo > " + DoTranslation("Probability of typo in percent") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Probability of typo in percent"), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Typo > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.14" 'Wipe
                    Select Case KeyNumber
                        Case 1 'Wipe: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Wipe255Colors)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates 255 color support for Wipe."), True, ColTypes.Neutral)
                        Case 2 'Wipe: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(WipeTrueColor)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates true color support for Wipe."), True, ColTypes.Neutral)
                        Case 3 'Wipe: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeDelay)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'Wipe: Wipes to change direction
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeWipesNeededToChangeDirection)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Wipes to change direction") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("How many wipes to do before changing direction randomly?"), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Wipe > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.15" 'HackUserFromAD
                    Select Case KeyNumber
                        Case 1 'HackUserFromAD: Hacker Mode
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(HackUserFromADHackerMode)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > HackUserFromAD > " + DoTranslation("Hacker Mode") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("If enabled, green console will be enabled.") + " l33t h4x0r!", True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > HackUserFromAD > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.16" 'AptErrorSim
                    Select Case KeyNumber
                        Case 1 'AptErrorSim: Hacker Mode
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(AptErrorSimHackerMode)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > AptErrorSim > " + DoTranslation("Hacker Mode") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("If enabled, green console will be enabled.") + " l33t h4x0r!", True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > AptErrorSim > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.17" 'Marquee
                    Select Case KeyNumber
                        Case 1 'Marquee: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Marquee255Colors)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates 255 color support for Marquee."), True, ColTypes.Neutral)
                        Case 2 'Marquee: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(MarqueeTrueColor)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Activates true color support for Marquee."), True, ColTypes.Neutral)
                        Case 3 'Marquee: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeDelay)
                            Write(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            Write("*) " + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'Marquee: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(MarqueeWrite)
                            Write(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Text shown") + vbNewLine, True, ColTypes.Neutral)
                            Write("*) " + DoTranslation("Write any text you want shown."), True, ColTypes.Neutral)
                        Case 5 'Marquee: Always centered
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(MarqueeAlwaysCentered)
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Always centered") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Whether the text shown on the marquee is always centered."), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Screensaver Settings...") + " > Marquee > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7." + $"{If(SectionParts.Length > 1, SectionParts(1), $"{BuiltinSavers + 1}")}" 'Custom saver
                    Dim SaverIndex As Integer = SectionParts(1) - BuiltinSavers - 1
                    Dim SaverSettings As Dictionary(Of String, Object) = CSvrdb.Values(SaverIndex).SaverSettings
                    Dim KeyIndex As Integer = KeyNumber - 1
                    If KeyIndex <= SaverSettings.Count - 1 Then
                        KeyType = SettingsKeyType.SVariant
                        KeyVar = CSvrdb.Values(SaverIndex).SaverSettings.Keys(KeyIndex)
                        Write("*) " + DoTranslation("Screensaver Settings...") + " > {0} > {1}" + vbNewLine, True, ColTypes.Neutral, CSvrdb.Keys(SaverIndex), SaverSettings.Keys(KeyIndex))
                        Write(DoTranslation("Consult the screensaver manual or source code for information."), True, ColTypes.Neutral)
                    Else
                        Write("*) " + DoTranslation("Screensaver Settings...") + " > {0} > ???" + vbNewLine, True, ColTypes.Neutral, CSvrdb.Keys(SaverIndex))
                        Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End If
                Case "8" 'Misc
                    Select Case KeyNumber
                        Case 1 'Show Time/Date on Upper Right Corner
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(CornerTD)
                            Write("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Show Time/Date on Upper Right Corner") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("The time and date will be shown in the upper right corner of the screen"), True, ColTypes.Neutral)
                        Case 2 'Marquee on startup
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(StartScroll)
                            Write("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Marquee on startup") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Enables eyecandy on startup"), True, ColTypes.Neutral)
                        Case 3 'Long Time and Date
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(LongTimeDate)
                            Write("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Long Time and Date") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("The time and date will be longer, showing full month names, etc."), True, ColTypes.Neutral)
                        Case 4 'Preferred Unit for Temperature
                            MaxKeyOptions = 3
                            KeyType = SettingsKeyType.SSelection
                            KeyVar = NameOf(PreferredUnit)
                            Write("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Preferred Unit for Temperature") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Select your preferred unit for temperature (this only applies to the ""weather"" command)") + vbNewLine, True, ColTypes.Neutral)
                            Write("1) " + DoTranslation("Kelvin"), True, ColTypes.Option)
                            Write("2) " + DoTranslation("Metric (Celsius)"), True, ColTypes.Option)
                            Write("3) " + DoTranslation("Imperial (Fahrenheit)"), True, ColTypes.Option)
                        Case 5 'Enable text editor autosave
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(TextEdit_AutoSaveFlag)
                            Write("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Enable text editor autosave") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Turns on or off the text editor autosave feature."), True, ColTypes.Neutral)
                        Case 6 'Text editor autosave interval
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TextEdit_AutoSaveInterval)
                            Write("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Text editor autosave interval") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("If autosave is enabled, the text file will be saved for each ""n"" seconds."), True, ColTypes.Neutral)
                        Case 7 'Wrap list outputs
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(WrapListOutputs)
                            Write("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Wrap list outputs") + vbNewLine, True, ColTypes.Neutral)
                            Write(DoTranslation("Wraps the list outputs if it seems too long for the current console geometry."), True, ColTypes.Neutral)
                        Case Else
                            Write("*) " + DoTranslation("Miscellaneous Settings...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            Write("X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case Else
                    Write("*) ???" + vbNewLine, True, ColTypes.Neutral)
                    Write("X) " + DoTranslation("Invalid section entered. Please go back."), True, ColTypes.Error)
            End Select

            'If the type is boolean, write the two options
            If KeyType = SettingsKeyType.SBoolean Then
                Console.WriteLine()
                Write("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                Write("2) " + DoTranslation("Disable"), True, ColTypes.Option)
            End If
            Console.WriteLine()

            'Add an option to go back.
            If Not KeyType = SettingsKeyType.SVariant Then Write("{0}) " + DoTranslation("Go Back...") + vbNewLine, True, ColTypes.Option, MaxKeyOptions + 1)

            'Get key value
            If Not KeyType = SettingsKeyType.SUnknown Then KeyValue = GetConfigValue(KeyVar)

            'Print debugging info
            Wdbg("W", "Key {0} in section {1} has {2} selections.", KeyNumber, Section, MaxKeyOptions)
            Wdbg("W", "Target variable: {0}, Key Type: {1}, Key value: {2}, Variant Value: {3}", KeyVar, KeyType, KeyValue, VariantValue)

            'Prompt user
            If KeyNumber = 2 And Section = "1.3" And Not KeyType = SettingsKeyType.SUnknown Then
                Write("> ", False, ColTypes.Input)
                AnswerString = ReadLineNoInput("*")
                Console.WriteLine()
            ElseIf KeyType = SettingsKeyType.SVariant And Not VariantValueFromExternalPrompt Then
                Write("> ", False, ColTypes.Input)
                VariantValue = Console.ReadLine
                If NeutralizePaths Then AnswerString = NeutralizePath(AnswerString)
                Wdbg("I", "User answered {0}", VariantValue)
            ElseIf KeyType = SettingsKeyType.SBoolean Then
                If KeyValue Then
                    AnswerString = "2"
                Else
                    AnswerString = "1"
                End If
            ElseIf Not KeyType = SettingsKeyType.SVariant Then
                If KeyType = SettingsKeyType.SList Then
#Disable Warning BC42104
                    Write("> ", False, ColTypes.Input)
                    Do Until AnswerString = "q"
                        AnswerString = Console.ReadLine
                        If Not AnswerString = "q" Then
                            If NeutralizePaths Then AnswerString = NeutralizePath(AnswerString)
                            TargetList = Enumerable.Append(TargetList, AnswerString)
                            Wdbg("I", "Added answer {0} to list.", AnswerString)
                            Write("> ", False, ColTypes.Input)
                        End If
                    Loop
#Enable Warning BC42104
                Else
                    Write(If(KeyType = SettingsKeyType.SUnknown, "> ", "[{0}] > "), False, ColTypes.Input, KeyValue)
                    If KeyType = SettingsKeyType.SLongString Then
                        AnswerString = ReadLineLong()
                    Else
                        AnswerString = Console.ReadLine
                    End If
                    If NeutralizePaths Then AnswerString = NeutralizePath(AnswerString)
                    Wdbg("I", "User answered {0}", AnswerString)
                End If
            End If

            'Check for input
#Disable Warning BC42104
            Wdbg("I", "Is the answer numeric? {0}", IsNumeric(AnswerString))
            If Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SBoolean Then
                Wdbg("I", "Answer is numeric and key is of the Boolean type.")
                If AnswerInt >= 1 And AnswerInt <= MaxKeyOptions Then
                    Wdbg("I", "Translating {0} to the boolean equivalent...", AnswerInt)
                    KeyFinished = True
                    Select Case AnswerInt
                        Case 1 'True
                            Wdbg("I", "Setting to True...")
                            SetConfigValue(KeyVar, True)
                        Case 2 'False
                            Wdbg("I", "Setting to False...")
                            SetConfigValue(KeyVar, False)
                    End Select
                ElseIf AnswerInt = MaxKeyOptions + 1 Then 'Go Back...
                    Wdbg("I", "User requested exit. Returning...")
                    KeyFinished = True
                Else
                    Wdbg("W", "Option is not valid. Returning...")
                    Write(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerInt)
                    Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            ElseIf (Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SInt) Or
                   (Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SSelection) Then
                Wdbg("I", "Answer is numeric and key is of the {0} type.", KeyType)
                If AnswerInt = MaxKeyOptions + 1 And KeyType = SettingsKeyType.SSelection Then 'Go Back...
                    Wdbg("I", "User requested exit. Returning...")
                    KeyFinished = True
                ElseIf AnswerInt >= 0 And SelectFrom IsNot Nothing Then
                    Wdbg("I", "Setting variable {0} to item index {1}...", KeyVar, AnswerInt - 1)
                    KeyFinished = True
                    SetConfigValue(KeyVar, SelectFrom(AnswerInt - 1))
                ElseIf AnswerInt >= 0 Then
                    Wdbg("I", "Setting variable {0} to {1}...", KeyVar, AnswerInt)
                    KeyFinished = True
                    SetConfigValue(KeyVar, AnswerInt)
                Else
                    Wdbg("W", "Negative values are disallowed.")
                    Write(DoTranslation("The answer may not be negative."), True, ColTypes.Error)
                    Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            ElseIf KeyType = SettingsKeyType.SUnknown Then
                Wdbg("I", "User requested exit. Returning...")
                KeyFinished = True
            ElseIf KeyType = SettingsKeyType.SString Or KeyType = SettingsKeyType.SLongString Then
                Wdbg("I", "Answer is not numeric and key is of the String type. Setting variable...")
                If String.IsNullOrWhiteSpace(AnswerString) Then
                    Wdbg("I", "Answer is nothing. Setting to {0}...", KeyValue)
                    AnswerString = KeyValue
                End If
                KeyFinished = True
                SetConfigValue(KeyVar, AnswerString)
            ElseIf KeyType = SettingsKeyType.SList Then
                Wdbg("I", "Answer is not numeric and key is of the List type. Adding answers to the list...")
                KeyFinished = True
                SetConfigValue(KeyVar, String.Join(ListJoinString, TargetList))
            ElseIf SectionParts.Length > 1 Then
                If Section = "7." + SectionParts(1) And SectionParts(1) > BuiltinSavers And KeyType = SettingsKeyType.SVariant Then
                    Dim SaverIndex As Integer = SectionParts(1) - BuiltinSavers - 1
                    Dim SaverSettings As Dictionary(Of String, Object) = CSvrdb.Values(SaverIndex).SaverSettings
                    SaverSettings(KeyVar) = VariantValue
                    Wdbg("I", "User requested exit. Returning...")
                    KeyFinished = True
                ElseIf KeyType = SettingsKeyType.SVariant Then
                    SetConfigValue(KeyVar, VariantValue)
                    Wdbg("I", "User requested exit. Returning...")
                    KeyFinished = True
                End If
            ElseIf KeyType = SettingsKeyType.SVariant Then
                SetConfigValue(KeyVar, VariantValue)
                Wdbg("I", "User requested exit. Returning...")
                KeyFinished = True
            Else
                Wdbg("W", "Answer is not valid.")
                Write(DoTranslation("The answer is invalid. Check to make sure that the answer is numeric for config entries that need numbers as answers."), True, ColTypes.Error)
                Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                Console.ReadKey()
            End If
#Enable Warning BC42104
        End While
    End Sub

    ''' <summary>
    ''' Sets the value of a variable to the new value dynamically
    ''' </summary>
    ''' <param name="Variable">Variable name. Use operator NameOf to get name.</param>
    ''' <param name="VariableValue">New value of variable</param>
    Public Sub SetConfigValue(Variable As String, VariableValue As Object)
        'Get field for specified variable
        Dim TargetField As FieldInfo = GetField(Variable)

        'Set the variable if found
        If TargetField IsNot Nothing Then
            'The "obj" description says this: "The object whose field value will be set."
            'Apparently, SetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
            'Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
            Wdbg("I", "Got field {0}. Setting to {1}...", TargetField.Name, VariableValue)
            TargetField.SetValue(Variable, VariableValue)
        Else
            'Variable not found on any of the "flag" modules.
            Wdbg("I", "Field {0} not found.", Variable)
            Write(DoTranslation("Variable {0} is not found on any of the modules."), True, ColTypes.Error, Variable)
        End If
    End Sub

    ''' <summary>
    ''' Gets the value of a variable dynamically 
    ''' </summary>
    ''' <param name="Variable">Variable name. Use operator NameOf to get name.</param>
    ''' <returns>Value of a variable</returns>
    Public Function GetConfigValue(Variable As String) As Object
        'Get field for specified variable
        Dim TargetField As FieldInfo = GetField(Variable)

        'Get the variable if found
        If TargetField IsNot Nothing Then
            'The "obj" description says this: "The object whose field value will be returned."
            'Apparently, GetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
            'Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
            Wdbg("I", "Got field {0}.", TargetField.Name)
            Return TargetField.GetValue(Variable)
        Else
            'Variable not found on any of the "flag" modules.
            Wdbg("I", "Field {0} not found.", Variable)
            Write(DoTranslation("Variable {0} is not found on any of the modules."), True, ColTypes.Error, Variable)
            Return Nothing
        End If
    End Function

End Module
