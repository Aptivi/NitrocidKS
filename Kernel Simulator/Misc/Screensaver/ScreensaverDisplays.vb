
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Imports System.ComponentModel

Public Module ScreensaverDisplays

    Public WithEvents ColorMix As New BackgroundWorker With {.WorkerSupportsCancellation = True}
    Public WithEvents Matrix As New BackgroundWorker With {.WorkerSupportsCancellation = True}
    Public WithEvents Disco As New BackgroundWorker With {.WorkerSupportsCancellation = True}
    Public WithEvents Lines As New BackgroundWorker With {.WorkerSupportsCancellation = True}
    Public WithEvents GlitterMatrix As New BackgroundWorker With {.WorkerSupportsCancellation = True}
    Public WithEvents GlitterColor As New BackgroundWorker With {.WorkerSupportsCancellation = True}
    Public WithEvents AptErrorSim As New BackgroundWorker With {.WorkerSupportsCancellation = True}
    Public WithEvents HackUserFromAD As New BackgroundWorker With {.WorkerSupportsCancellation = True}
    Public WithEvents ColorMix255 As New BackgroundWorker With {.WorkerSupportsCancellation = True}
    Public WithEvents GlitterColor255 As New BackgroundWorker With {.WorkerSupportsCancellation = True}
    Public WithEvents Disco255 As New BackgroundWorker With {.WorkerSupportsCancellation = True}
    Public WithEvents Lines255 As New BackgroundWorker With {.WorkerSupportsCancellation = True}
    Public WithEvents Custom As New BackgroundWorker With {.WorkerSupportsCancellation = True}
    Public finalSaver As ICustomSaver
    Public colors() As ConsoleColor = CType([Enum].GetValues(GetType(ConsoleColor)), ConsoleColor())        '15 Console Colors
    Public colors255() As ConsoleColors = CType([Enum].GetValues(GetType(ConsoleColors)), ConsoleColors())  '255 Console Colors

    Sub Custom_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Custom.DoWork
        'To Screensaver Developers: ONLY put the effect code in your scrnSaver() sub.
        '                           Set colors, write welcome message, etc. with the exception of infinite loop and the effect code in preDisplay() sub
        '                           Recommended: Turn off console cursor, and clear the screen in preDisplay() sub.
        '                           Substitute: TextWriterColor.W() with System.Console.WriteLine() or System.Console.Write().
        Console.CursorVisible = False
        finalSaver.PreDisplay()
        Do While True
            If Not finalSaver.DelayForEachWrite = Nothing Then
                SleepNoBlock(finalSaver.DelayForEachWrite, Custom)
            End If
            If Custom.CancellationPending = True Then
                Wdbg("W", "Cancellation requested. Showing ending...")
                finalSaver.PostDisplay()
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                Load()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Custom screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                finalSaver.ScrnSaver()
            End If
        Loop
    End Sub

    Sub ColorMix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles ColorMix.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()
        Console.CursorVisible = False
        Dim colorrand As New Random()
        Do While True
            SleepNoBlock(1, ColorMix)
            If ColorMix.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                Load()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Mix Colors screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                Console.BackgroundColor = CType(colorrand.Next(1, 16), ConsoleColor) : Console.Write(" ")
            End If
        Loop
    End Sub

    Sub Matrix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Matrix.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.Green
        Console.Clear()
        Console.CursorVisible = False
        Dim random As New Random()
        Do While True
            If Matrix.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                Load()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Matrix screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                SleepNoBlock(1, Matrix)
                Console.Write(CStr(random.Next(2)))
            End If
        Loop
    End Sub

    Sub Disco_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Disco.DoWork
        Console.CursorVisible = False
        Dim random As New Random()
        Do While True
            SleepNoBlock(100, Disco)
            If Disco.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                Load()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Disco screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else

                Console.BackgroundColor = colors(random.Next(colors.Length - 1))
                Console.Clear()
            End If
        Loop
    End Sub

    Sub Lines_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Lines.DoWork
        Console.CursorVisible = False
        Dim random As New Random()
        Wdbg("I", "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            SleepNoBlock(500, Lines)
            If Lines.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                Load()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Lines screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                Console.Clear()
                Console.BackgroundColor = ConsoleColor.Black
                Console.ForegroundColor = colors(random.Next(colors.Length - 1))
                Dim Line As String = ""
                Dim Top As Integer = New Random().Next(Console.WindowHeight)
                For i As Integer = 1 To Console.WindowWidth
                    Line += "-"
                Next
                Console.SetCursorPosition(0, Top)
                Console.WriteLine(Line)
            End If
        Loop
    End Sub

    Sub GlitterMatrix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles GlitterMatrix.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.Green
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Wdbg("I", "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            If GlitterMatrix.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                Load()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Glitter Matrix screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                SleepNoBlock(1, GlitterMatrix)
                Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                Console.SetCursorPosition(Left, Top)
                Console.Write(CStr(RandomDriver.Next(2)))
            End If
        Loop
    End Sub

    Sub AptErrorSim_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles AptErrorSim.DoWork
        Console.CursorVisible = False
        Do While True
            SleepNoBlock(100, AptErrorSim)
IFCANCEL:
            If AptErrorSim.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                Load()
                Console.CursorVisible = True
                Wdbg("I", "All clean. apt Error Simulator screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                Console.Clear()
                Console.Write("{0}@{1}:{2}$ ", HName, signedinusrnm, CurrDir)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(3000, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                WriteSlowly("sudo apt -y dist-upgrade", False, 100)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(200, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine()
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(50, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.Write("Reading package lists... ")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(50, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Done")
                Console.WriteLine("Building dependency tree")
                Console.Write("Reading state information... ")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(210, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Done")
                Console.Write("Calculating upgrade...")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(80, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Done")
                Console.WriteLine("The following packages will be upgraded:")
                Console.WriteLine("  libnvpair1linux libuutil1linux libzfs2linux libzpool2linux zfs-initramfs zfs-zed zfsutils-linux")
                Console.WriteLine("7 upgraded, 0 newly installed, 0 to remove and 0 not upgraded")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(300, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Need to get 1,510 kB of archives.")
                Console.WriteLine("After this operation, 155 kB of additional disk space will be used.")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(600, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.Write("0% [Connecting to archive.ubuntu.com]")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(10000, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine(vbNewLine + "Err:1 http://archive.ubuntu.com/ubuntu focal/main amd64 zfs-initramfs amd64 0.8.2-3ubuntu1")
                Console.WriteLine("Err:2 http://archive.ubuntu.com/ubuntu focal/main amd64 zfs-zed amd64 0.8.2-3ubuntu1")
                Console.WriteLine("Err:3 http://archive.ubuntu.com/ubuntu focal/main amd64 zfsutils-linux amd64 0.8.2-3ubuntu1")
                Console.WriteLine("Err:4 http://archive.ubuntu.com/ubuntu focal/main amd64 libnvpair1linux amd64 0.8.2-3ubuntu1")
                Console.WriteLine("Err:5 http://archive.ubuntu.com/ubuntu focal/main amd64 libuutil1linux amd64 0.8.2-3ubuntu1")
                Console.WriteLine("Err:6 http://archive.ubuntu.com/ubuntu focal/main amd64 libzfs2linux amd64 0.8.2-3ubuntu1")
                Console.WriteLine("Err:7 http://archive.ubuntu.com/ubuntu focal/main amd64 libzpool2linux amd64 0.8.2-3ubuntu1")
                Console.WriteLine("  Connection timed out [IP: 91.189.88.31 80]")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(100, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("E: Failed to fetch http://archive.ubuntu.com/ubuntu/pool/main/z/zfs-initramfs/zfs-initramfs_0.8.2-3ubuntu1_amd64.deb  Connection timed out [IP: 91.189.88.31 80]")
                Console.WriteLine("E: Failed to fetch http://archive.ubuntu.com/ubuntu/pool/main/z/zfs-zed/zfs-zed_0.8.2-3ubuntu1_amd64.deb  Connection timed out [IP: 91.189.88.31 80]")
                Console.WriteLine("E: Failed to fetch http://archive.ubuntu.com/ubuntu/pool/main/z/zfsutils-linux/zfsutils-linux_0.8.2-3ubuntu1_amd64.deb  Connection timed out [IP: 91.189.88.31 80]")
                Console.WriteLine("E: Failed to fetch http://archive.ubuntu.com/ubuntu/pool/main/libn/libnvpair1linux/libnvpair1linux_0.8.2-3ubuntu1_amd64.deb  Connection timed out [IP: 91.189.88.31 80]")
                Console.WriteLine("E: Failed to fetch http://archive.ubuntu.com/ubuntu/pool/main/libu/libuutil1linux/libuutil1linux_0.8.2-3ubuntu1_amd64.deb  Connection timed out [IP: 91.189.88.31 80]")
                Console.WriteLine("E: Failed to fetch http://archive.ubuntu.com/ubuntu/pool/main/libz/libzfs2linux/libzfs2linux_0.8.2-3ubuntu1_amd64.deb  Connection timed out [IP: 91.189.88.31 80]")
                Console.WriteLine("E: Failed to fetch http://archive.ubuntu.com/ubuntu/pool/main/libz/libzpool2linux/libzpool2linux_0.8.2-3ubuntu1_amd64.deb  Connection timed out [IP: 91.189.88.31 80]")
                Console.WriteLine("E: Unable to fetch some archives, maybe run apt-get update or try with --fix-missing?")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(100, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.Write("{0}@{1}:{2}$ ", HName, signedinusrnm, CurrDir)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(5000, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
            End If
        Loop
    End Sub

    Sub HackUserFromAD_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles HackUserFromAD.DoWork
        Do While True
            SleepNoBlock(1000, HackUserFromAD)
IFCANCEL:
            If HackUserFromAD.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                Load()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Hacking Simulator for Active Domain users screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                Console.Clear()
                Console.BackgroundColor = ConsoleColor.Black
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine("Microsoft Windows [Version 10.0.18362.449]" + vbNewLine +
                                  "(c) 2019 Microsoft Corporation. All rights reserved." + vbNewLine)
                Console.Write(CurrDir + ">")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(3000, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                WriteSlowly("net user /domain", False, 100)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(200, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine()
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(50, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("The request will be processed at a domain controller for domain Community.Workspace." + vbNewLine)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(2000, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("User accounts for \\LOGON-HANDLER" + vbNewLine + vbNewLine +
                                  "-------------------------------------------------------------------------------" + vbNewLine +
                                  "Administrator            CommAdmin                EnterpriseManager" + vbNewLine +
                                  "Guest                    UserEmployees            Work" + vbNewLine +
                                  "The command completed successfully." + vbNewLine + vbNewLine)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.Write(CurrDir + ">")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(1000, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                WriteSlowly("net user EnterpriseManager /domain", False, 125)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(325, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine()
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(50, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("The request will be processed at a domain controller for domain Community.Workspace." + vbNewLine)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(2000, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("User name                    EnterpriseManager" + vbNewLine +
                                  "Full Name                    Enterprise Manager" + vbNewLine +
                                  "Comment                      Only usable by IT experts" + vbNewLine +
                                  "User's comment" + vbNewLine +
                                  "Country/region code          000 (System Default)" + vbNewLine +
                                  "Account active               Yes" + vbNewLine +
                                  "Account expires              Never" + vbNewLine + vbNewLine +
                                 $"Password last set            {FormatDateTime(KernelDateTime, DateFormat.ShortDate)} {FormatDateTime(KernelDateTime, DateFormat.LongTime)}" + vbNewLine +
                                  "Password expires             Never" + vbNewLine +
                                 $"Password changeable          ‎{FormatDateTime(KernelDateTime, DateFormat.ShortDate)} {FormatDateTime(KernelDateTime, DateFormat.LongTime)}" + vbNewLine +
                                  "Password required            No" + vbNewLine +
                                  "User may change password     Yes" + vbNewLine +
                                  "Workstations allowed         All" + vbNewLine + vbNewLine +
                                  "Logon script" + vbNewLine +
                                  "User profile" + vbNewLine +
                                  "Home directory               Z:\ENT\Private\EM" + vbNewLine +
                                  "Last logon                   ‎2/27/‎2018 5:05:41 PM" + vbNewLine + vbNewLine +
                                  "Logon hours allowed          All" + vbNewLine + vbNewLine +
                                  "Local Group Memberships      *None" + vbNewLine +
                                  "Global Group memberships     *Administrators" + vbNewLine +
                                  "The command completed successfully." + vbNewLine + vbNewLine)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.Write(CurrDir + ">")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(1000, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                WriteSlowly("ntlm.py dump --user=EnterpriseManager --domain=Community.Workspace", False, 85)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(130, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Dumping NTLM Hash...")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(4000, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Dump completed, and saved in ./Hash.txt")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.Write(CurrDir + ">")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(1000, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                WriteSlowly("ntlm.py decrypt Hash.txt", False, 160)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(215, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Trying to decrypt...")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(2150, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Decryption complete. Plain-text password retrieved in ./Pass.txt")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.Write(CurrDir + ">")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(1000, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                WriteSlowly("start.py --user=EnterpriseManager --pass=`write Pass.txt` ""start""", False, 130)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(115, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Microsoft Windows [Version 10.0.18362.449]" + vbNewLine +
                                  "(c) 2019 Microsoft Corporation. All rights reserved." + vbNewLine)
                Console.Write("Z:\ENT\Private\EM>")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(3000, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                WriteSlowly("shutdown /s /fw /t 00 /m \\LOGON-HANDLER", False, 130)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(115, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("LOGON-HANDLER: System shutdown is initiated.")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(115, HackUserFromAD)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.Write("Z:\ENT\Private\EM>")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(5000, HackUserFromAD)
            End If
        Loop
    End Sub

    Sub GlitterColor_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles GlitterColor.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Wdbg("I", "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            If GlitterColor.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                Load()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Glitter Color screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                SleepNoBlock(1, GlitterColor)
                Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                Console.SetCursorPosition(Left, Top)
                Console.BackgroundColor = colors(RandomDriver.Next(colors.Length - 1))
                Console.Write(" ")
            End If
        Loop
    End Sub

    Sub ColorMix255_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles ColorMix255.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Dim RandomDriver As New Random
        Console.Clear()
        Console.CursorVisible = False
        Do While True
            SleepNoBlock(1, ColorMix255)
            If ColorMix255.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                Load()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Mix 255 Colors screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                Dim esc As Char = GetEsc()
                Dim ColorNum As Integer = RandomDriver.Next(255)
                Console.Write(esc + "[48;5;" + CStr(ColorNum) + "m ")
            End If
        Loop
    End Sub

    Sub GlitterColor255_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles GlitterColor255.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Wdbg("I", "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            If GlitterColor255.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                Load()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Glitter 255 Colors screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                SleepNoBlock(1, GlitterColor255)
                Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                Console.SetCursorPosition(Left, Top)
                Dim esc As Char = GetEsc()
                Dim ColorNum As Integer = RandomDriver.Next(255)
                Console.Write(esc + "[48;5;" + CStr(ColorNum) + "m ")
            End If
        Loop
    End Sub

    Sub Disco255_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Disco255.DoWork
        Console.CursorVisible = False
        Dim random As New Random
        Do While True
            SleepNoBlock(100, Disco255)
            If Disco255.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                Load()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Disco 255 screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                Dim esc As Char = GetEsc()
                Dim color As Integer = random.Next(255)
                Console.Write(esc + "[48;5;" + CStr(color) + "m")
                Console.Clear()
            End If
        Loop
    End Sub

    Sub Lines255_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Lines255.DoWork
        Console.CursorVisible = False
        Dim random As New Random
        Wdbg("I", "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            SleepNoBlock(500, Lines255)
            If Lines255.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                Load()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Lines 255 screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                Dim esc As Char = GetEsc()
                Console.BackgroundColor = ConsoleColor.Black
                Console.Clear()
                Dim color As Integer = random.Next(255)
                Console.Write(esc + "[38;5;" + CStr(color) + "m")
                Dim Line As String = ""
                Dim Top As Integer = New Random().Next(Console.WindowHeight)
                For i As Integer = 1 To Console.WindowWidth
                    Line += "-"
                Next
                Console.SetCursorPosition(0, Top)
                Console.WriteLine(Line)
            End If
        Loop
    End Sub

End Module
