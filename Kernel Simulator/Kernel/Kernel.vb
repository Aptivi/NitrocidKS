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

Imports KS.Arguments.ArgumentBase
Imports KS.Files.Querying
Imports KS.Hardware
Imports KS.Kernel.Exceptions
Imports KS.Kernel.Configuration
Imports KS.Misc.Reflection
Imports KS.Misc.Splash
Imports KS.Misc.Writers.MiscWriters
Imports KS.Misc.Probers.Motd
Imports KS.Modifications
Imports KS.Network.RemoteDebug
Imports KS.Network.RPC
Imports KS.TimeDate
Imports ReadLineReboot
Imports System.IO
Imports System.Reflection.Assembly
Imports System.Threading
Imports KS.Misc.Notifications

#if SPECIFIERREL
Imports KS.Network
Imports KS.Network.Transfer
Imports KS.Kernel.Updates
#endif

Namespace Kernel
    Public Module Kernel

        'Variables
        Public MOTDMessage, MAL As String
        Public HostName As String = "kernel"
        Public ReadOnly KernelVersion As String = GetExecutingAssembly().GetName().Version.ToString()
        Public ReadOnly NewLine As String = Environment.NewLine
        Public ReadOnly KernelEventManager As New Events.Events
        Public ReadOnly ExecutableDir As String = Environment.CurrentDirectory
        Friend StageTimer As New Stopwatch
        Friend DefConsoleOut As TextWriter

        '#ifdef'd variables ... Framework monikers
#if NETCOREAPP
        Public Const KernelSimulatorMoniker As String = ".NET CoreCLR"
#else
        Public Const KernelSimulatorMoniker As String = ".NET Framework"
#EndIf

        'Release specifiers (SPECIFIER: REL, RC, or DEV | MILESTONESPECIFIER: ALPHA, BETA, DELTA, GAMMA, NONE | None satisfied: Unsupported Release)
#If SPECIFIERRELThen Then
        Public ReadOnly ConsoleTitle As String = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker}"
#elif SPECIFIERRC
        Public ReadOnly ConsoleTitle As String = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker} - Release Candidate"
#elif SPECIFIERDEV
#If MILESTONESPECIFIER = "ALPHA" Then
        Public ReadOnly ConsoleTitle As String = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker} - Developer Preview - Milestone 1"
#elif MILESTONESPECIFIER = "BETA" Then
        Public ReadOnly ConsoleTitle As String = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker} - Developer Preview - Beta 1"
#elif MILESTONESPECIFIER = "DELTA" Then
        Public ReadOnly ConsoleTitle As String = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker} - Developer Preview - Delta 1"
#elif MILESTONESPECIFIER = "GAMMA" Then
        Public ReadOnly ConsoleTitle As String = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker} - Developer Preview - Gamma 1"
#else
        Public ReadOnly ConsoleTitle As String = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker} - Developer Preview"
#endif
#Else
        Public ReadOnly ConsoleTitle As String = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker} - Unsupported Release"
#endif

        ''' <summary>
        ''' Entry point
        ''' </summary>
        Sub Main(Args() As String)
            'Set main thread name
            Thread.CurrentThread.Name = "Main Kernel Thread"

            'This is a kernel entry point
            While Not KernelShutdown
                Try
                    'A title
                    SetTitle(ConsoleTitle)

                    'Initial ReadLine settings
                    ReadLine.CtrlCEnabled = True
                    InputHistoryEnabled = True
                    ReadLine.PrewriteDefaultValue = True
                    ReadLine.AutoCompletionEnabled = True

                    'Check for terminal
                    CheckConsole()

                    'Initialize crucial things
                    If SafeMode Then ReadFailsafeConfig()
                    If Not IsOnUnix() Then Color255.Initialize255()
                    AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf LoadFromAssemblySearchPaths

                    'Check for pre-boot arguments
                    ParseArguments(Args.ToList, ArgumentType.PreBootCommandLineArgs)

                    'Download debug symbols if not found (loads automatically, useful for debugging problems and stack traces)
                    'TODO: Move this to a separate function
#if SPECIFIERREL
                    If Not NetworkAvailable Then
                        NotifySend(New Notification(DoTranslation("No network while downloading debug data"),
                                                    DoTranslation("Check your internet connection and try again."),
                                                    NotifPriority.Medium, NotifType.Normal))
                    End If
                    If NetworkAvailable Then
                        'Check to see if we're running from Ubuntu PPA
                        Dim PPASpotted As Boolean = ExecPath.StartsWith("/usr/lib/ks")
                        If ExecPath.StartsWith("/usr/lib/ks") Then
                            ReportProgress(DoTranslation("Use apt to update Kernel Simulator."), 10, ColTypes.Error)
                        End If

                        'Download debug symbols
                        If Not FileExists(GetExecutingAssembly.Location.Replace(".exe", ".pdb")) And Not PPASpotted Then
                            Try
#if NETCOREAPP
                                DownloadFile($"https://github.com/Aptivi/Kernel-Simulator/releases/download/v{KernelVersion}-beta/{KernelVersion}-dotnet.pdb", GetExecutingAssembly.Location.Replace(".exe", ".pdb"))
#else
                                DownloadFile($"https://github.com/Aptivi/Kernel-Simulator/releases/download/v{KernelVersion}-beta/{KernelVersion}.pdb", GetExecutingAssembly.Location.Replace(".exe", ".pdb"))
#endif
                            Catch ex As Exception
                                NotifySend(New Notification(DoTranslation("Error downloading debug data"),
                                                            DoTranslation("There is an error while downloading debug data. Check your internet connection."),
                                                            NotifPriority.Medium, NotifType.Normal))
                            End Try
                        End If
                    End If
#endif

                        'Check for console size
                        If CheckingForConsoleSize Then
                        'Check for the minimum console window requirements (80x24)
                        Do While Console.WindowWidth < 80 Or Console.WindowHeight < 24
                            Write(DoTranslation("Your console is too small to run properly:") + " {0}x{1}", True, ColTypes.Warning, Console.WindowWidth, Console.WindowHeight)
                            Write(DoTranslation("To have a better experience, resize your console window while still being on this screen. Press any key to continue..."), True, ColTypes.Warning)
                            Console.ReadKey(True)
                        Loop
                    Else
                        Write(DoTranslation("Looks like you're bypassing the console size detection. Things may not work properly on small screens.") + NewLine +
                              DoTranslation("To have a better experience, resize your console window while still being on this screen. Press any key to continue..."), True, ColTypes.Warning)
                        Console.ReadKey(True)
                        CheckingForConsoleSize = True
                    End If

                    'Initialize everything
                    StageTimer.Start()
                    InitEverything(Args)
                    CheckErrored()

                    'Stage 1: Initialize the system
                    ReportNewStage(1, DoTranslation("- Stage 1: System initialization"))
                    If RDebugAutoStart And DebugMode Then
                        ReportProgress(DoTranslation("Starting the remote debugger..."), 3, ColTypes.Neutral)
                        StartRDebugThread()
                        If Not RDebugFailed Then
                            ReportProgress(DoTranslation("Debug listening on all addresses using port {0}.").FormatString(DebugPort), 5, ColTypes.Neutral)
                        Else
                            ReportProgress(DoTranslation("Remote debug failed to start: {0}").FormatString(RDebugFailedReason.Message), 5, ColTypes.Error)
                        End If
                    End If
                    ReportProgress(DoTranslation("Starting RPC..."), 3, ColTypes.Neutral)
                    WrapperStartRPC()

                    'If the two files are not found, create two MOTD files with current config.
                    If Not FileExists(GetKernelPath(KernelPathType.MOTD)) Then
                        SetMotd(DoTranslation("Welcome to Kernel!"))
                        ReportProgress(DoTranslation("Generated default MOTD."), 3, ColTypes.Neutral)
                    End If
                    If Not FileExists(GetKernelPath(KernelPathType.MAL)) Then
                        SetMal(DoTranslation("Logged in successfully as <user>"))
                        ReportProgress(DoTranslation("Generated default MAL."), 3, ColTypes.Neutral)
                    End If

                    'Check for kernel updates
#if SPECIFIERREL
                    If CheckUpdateStart Then
                        CheckKernelUpdates()
                    End If
#endif

                    'Phase 2: Probe hardware
                    ReportNewStage(2, DoTranslation("- Stage 2: Hardware detection"))
                    If Not QuietHardwareProbe Then ReportProgress(DoTranslation("hwprobe: Your hardware will be probed. Please wait..."), 15, ColTypes.Progress)
                    StartProbing()
                    If Not EnableSplash And Not QuietKernel Then ListHardware()
                    CheckErrored()

                    'Phase 3: Parse Mods and Screensavers
                    ReportNewStage(3, DoTranslation("- Stage 3: Mods and screensavers detection"))
                    Wdbg(DebugLevel.I, "Safe mode flag is set to {0}", SafeMode)
                    If Not SafeMode Then
                        If StartKernelMods Then StartMods()
                    Else
                        ReportProgress(DoTranslation("Running in safe mode. Skipping stage..."), 0, ColTypes.Neutral)
                    End If
                    KernelEventManager.RaiseStartKernel()

                    'Phase 4: Log-in
                    ReportNewStage(4, DoTranslation("- Stage 4: Log in"))
                    InitializeSystemAccount()
                    ReportProgress(DoTranslation("System account initialized"), 5, ColTypes.Neutral)
                    InitializeUsers()
                    ReportProgress(DoTranslation("Users initialized"), 5, ColTypes.Neutral)
                    LoadPermissions()
                    ReportProgress(DoTranslation("Permissions loaded"), 5, ColTypes.Neutral)

                    'Reset console state and stop stage timer
                    ReportNewStage(5, "")

                    'Show the closing screen
                    ReportProgress(DoTranslation("Welcome!"), 100, ColTypes.Success)
                    CloseSplash()

                    'Show current time
                    If ShowCurrentTimeBeforeLogin Then ShowCurrentTimes()

                    'Notify user of errors if appropriate
                    If NotifyKernelError Then
                        NotifyKernelError = False
                        NotifySend(New Notification(DoTranslation("Previous boot failed"),
                                                    LastKernelErrorException.Message,
                                                    NotifPriority.High, NotifType.Normal))
                    End If

                    'Show license if new style used
                    If NewWelcomeStyle Or EnableSplash Then
                        Console.WriteLine()
                        WriteSeparator(DoTranslation("License information"), True, ColTypes.Stage)
                        WriteLicense(False)
                    End If

                    'Initialize login prompt
                    If Not Maintenance Then
                        LoginPrompt()
                    Else
                        ReadMotd()
                        ReadMal()
                        Write(DoTranslation("Enter the admin password for maintenance."), True, ColTypes.Neutral)
                        If Users.ContainsKey("root") Then
                            Wdbg(DebugLevel.I, "Root account found. Prompting for password...")
                            ShowPasswordPrompt("root")
                        Else
                            'Some malicious mod removed the root account, or rare situation happened and it was gone.
                            Wdbg(DebugLevel.W, "Root account not found for maintenance. Initializing it...")
                            InitializeSystemAccount()
                            ShowPasswordPrompt("root")
                        End If
                    End If

                    'Clear all active threads as we're rebooting
                    StopAllThreads()
                Catch icde As InsaneConsoleDetectedException
                    Console.WriteLine(icde.Message)
                    Console.WriteLine(icde.InsanityReason)
                    KernelShutdown = True
                Catch kee As KernelErrorException
                    WStkTrc(kee)
                    KernelErrored = False
                    RebootRequested = False
                    LogoutRequested = False
                    SafeMode = False
                Catch ex As Exception
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.U, True, 5, DoTranslation("Kernel Error while booting: {0}"), ex, ex.Message)
                End Try
            End While

            'Clear the console and reset the colors
            Console.ResetColor()
            Console.Clear()

            'If "No APM" is enabled, simply print the text
            If SimulateNoAPM Then
                Console.WriteLine(DoTranslation("It's now safe to turn off your computer."))
                Console.ReadKey(True)
            End If
        End Sub

        ''' <summary>
        ''' Check to see if KernelError has been called
        ''' </summary>
        Sub CheckErrored()
            If KernelErrored Then Throw New KernelErrorException(DoTranslation("Kernel Error while booting: {0}"), LastKernelErrorException, LastKernelErrorException.Message)
        End Sub

    End Module
End Namespace
