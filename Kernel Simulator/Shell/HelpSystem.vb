
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Module HelpSystem

    Sub ShowHelp(Optional ByVal command As String = "")

        If (command = "") Then

            'TODO: Color the help system.
            'TODO: Modification Parser for mods with commands (Help, Commands, and everything).
            If (simHelp = False) Then
                'It is messy. It will actually be converted to dictionary in 0.0.5.
                Wln("- adduser: {0}" + vbNewLine + "- annoying-sound (Alias: beep): {1}" + vbNewLine + "- arginj: {2}" + vbNewLine + "- calc: {3}" + vbNewLine +
                    "- cdir (Alias: currentdir): {4}" + vbNewLine + "- changedir (Aliases: chdir, cd): {5}" + vbNewLine + "- chhostname: {6}" + vbNewLine +
                    "- chmotd: {7}" + vbNewLine + "- choice: {8}" + vbNewLine + "- chpwd: {9}" + vbNewLine + "- chusrname: {10}" + vbNewLine + "- cls: {11}" + vbNewLine +
                    "- debuglog: {12}" + vbNewLine + "- disco: {13}" + vbNewLine + "- echo: {14}" + vbNewLine + "- help: {15}" + vbNewLine + "- hwprobe: {16}" + vbNewLine +
                    "- list (Alias: ls): {17}" + vbNewLine + "- logout: {18}" + vbNewLine + "- lsdrivers: {19}" + vbNewLine + "- lsnet: {20}" + vbNewLine +
                    "- lsnettree: {21}" + vbNewLine + "- md (Alias: mkdir): {22}" + vbNewLine + "- netinfo: {23}" + vbNewLine + "- panicsim: {24}" + vbNewLine +
                    "- perm: {24}" + vbNewLine + "- ping: {25}" + vbNewLine + "- read: {26}" + vbNewLine + "- reboot: {27}" + vbNewLine + "- reloadconfig: {28}" + vbNewLine +
                    "- rd (Alias: rmdir): {29}" + vbNewLine + "- rmuser: {30}" + vbNewLine + "- scical: {31}" + vbNewLine + "- setcolors: {32}" + vbNewLine +
                    "- setthemes: {33}" + vbNewLine + "- showmotd: {34}" + vbNewLine + "- showtd: {35}" + vbNewLine + "- shutdown: {36}" + vbNewLine +
                    "- sysinfo: {37}" + vbNewLine + "- unitconv: {38}" + vbNewLine + "- version: {39}" + vbNewLine + vbNewLine +
                    "* You can use multiple commands at the same time using the colon between commands." + vbNewLine +
                    "* Pre-defined aliases will be removed in 0.0.5.", "neutralText",
                    adduserCmdDesc, annoyingSoundCmdDesc, argInjCmdDesc, calcCmdDesc, currentDirCmdDesc, changeDirCmdDesc, chHostNameCmdDesc, chMotdCmdDesc,
                    choiceCmdDesc, chPwdCmdDesc, chUsrNameCmdDesc, clsCmdDesc, debugLogCmdDesc, discoCmdDesc, echoCmdDesc, helpCmdDesc, hwProbeCmdDesc, listCmdDesc,
                    logoutCmdDesc, lsDriversCmdDesc, lsnetCmdDesc, lsNetByTreeCmdDesc, makeDirectoryCmdDesc, netInfoCmdDesc, permCmdDesc, pingCmdDesc,
                    readCmdDesc, rebootCmdDesc, reloadConfigCmdDesc, rmDirCmdDesc, rmUserCmdDesc, sciCalCmdDesc, setColorsCmdDesc, setThemesCmdDesc, showMotdCmdDesc,
                    showTdCmdDesc, shutdownCmdDesc, sysInfoCmdDesc, unitConvCmdDesc, versionCmdDesc)
            Else
                Wln(String.Join(", ", availableCommands), "neutralText")
            End If

        ElseIf (command.Contains("adduser")) Then

            Wln("Usage: adduser <userName> [password] [confirm]" + vbNewLine +
                "       adduser: to be prompted about new username and password. (deprecated)", "neutralText")

        ElseIf (command.Contains("annoying-sound") Or command.Contains("beep")) Then

            Wln("Usage: annoying-sound/beep <Frequency:Hz> <Time:Seconds>" + vbNewLine +
                "       annoying-sound/beep: to be prompted about beeping. (deprecated)", "neutralText")

        ElseIf (command.Contains("arginj")) Then

            Wln("Usage: arginj [Arguments separated by commas]" + vbNewLine +
                "       arginj: to be prompted about boot arguments. (deprecated)", "neutralText")

        ElseIf (command.Contains("calc")) Then

            Wln("Usage: calc <expression> ...", "neutralText")

        ElseIf (command.Contains("cdir") Or command.Contains("currentdir")) Then

            Wln("Usage: cdir/currentdir: to get current directory", "neutralText")

        ElseIf (command.Contains("changedir") Or command.Contains("chdir") Or command.StartsWith("cd")) Then

            Wln("Usage: chdir/changedir/cd <directory> OR ..", "neutralText")

        ElseIf (command.Contains("chhostname")) Then

            Wln("Usage: chhostname <HostName>" + vbNewLine +
                "       chhostname: to be prompted about changing host name. (deprecated)", "neutralText")

        ElseIf (command.Contains("chmotd")) Then

            Wln("Usage: chmotd <Message>", "neutralText")

        ElseIf (command.Contains("choice")) Then

            Wln("Usage: choice <Question> <sets>" + vbNewLine +
                "       choice: to be prompted about choices. (deprecated)", "neutralText")

        ElseIf (command.Contains("chpwd")) Then

            Wln("Usage: chpwd: to be prompted about changing passwords. (deprecated)", "neutralText")

        ElseIf (command.Contains("chusrname")) Then

            Wln("Usage: chusrname <oldUserName> <newUserName>" + vbNewLine +
                "       chusrname: to be prompted about changing usernames. (deprecated)", "neutralText")

        ElseIf (command.Contains("cls")) Then

            Wln("Usage: cls: to clear screen.", "neutralText")

        ElseIf (command.Contains("debuglog")) Then

            Wln("Usage: debuglog: Shows you debug logs so you can send the log to us.", "neutralText")

        ElseIf (command.Contains("disco")) Then

            Wln("Usage: disco: to get a disco effect on the console. True color support will come with GUI console.", "neutralText")

        ElseIf (command.Contains("echo")) Then

            Wln("Usage: echo <text>" + vbNewLine +
                "       echo: to be prompted about text printing. (deprecated)", "neutralText")

        ElseIf (command.Contains("hwprobe")) Then

            Wln("Usage: hwprobe: Probes hardware (Only works when the hardware is not probed and hwprobe is not executed).", "neutralText")

        ElseIf (command.Contains("ls") Or command.Contains("list")) Then

            If (command = "ls" Or command = "list") Then
                Wln("Usage: ls/list [oneDirectory]" + vbNewLine +
                    "       ls/list: to get current directory.", "neutralText")
            ElseIf (command = "lsdrivers") Then
                Wln("Usage: lsdrivers: Lists probed drivers." + vbNewLine +
                    "       Friends of lsdrivers: sysinfo, version", "neutralText")
            ElseIf (command = "lsnet") Then
                Wln("Usage: lsnet: Lists network information, as well as every computer connected to a network." + vbNewLine +
                    "       Friends of lsnet: lsnettree", "neutralText")
            ElseIf (command = "lsnettree") Then
                Wln("Usage: lsnettree: Lists network information, as well as every computer connected to a network, in a tree form." + vbNewLine +
                    "       Friends of lsnettree: lsnet", "neutralText")
            End If

        ElseIf (command.Contains("logout")) Then

            Wln("Usage: logout: Logs you out of the user." + vbNewLine +
                "       Friends of logout: reboot, shutdown", "neutralText")

        ElseIf (command.Contains("mkdir") Or command.Contains("md")) Then

            Wln("Usage: md/mkdir <anything>", "neutralText")

        ElseIf (command.Contains("netinfo")) Then

            Wln("Usage: netinfo: Get every network information", "neutralText")

        ElseIf (command.Contains("perm")) Then

            Wln("Usage: perm <userName> <Admin/Disabled> <Allow/Disallow>" + vbNewLine +
                "       perm: to be prompted about permission setting. (deprecated)", "neutralText")

        ElseIf (command.Contains("ping")) Then

            Wln("Usage: ping <Address> [repeatTimes]" + vbNewLine +
                "       ping: to get prompted about writing address. (deprecated)", "neutralText")

        ElseIf (command.Contains("rmdir") Or command.Contains("rd")) Then

            Wln("Usage: rd/rmdir <directory>", "neutralText")

        ElseIf (command.Contains("read")) Then

            Wln("Usage: read <file>" + vbNewLine +
                "       read: to get prompted about reading file contents. (deprecated)", "neutralText")

        ElseIf (command.Contains("reboot")) Then

            Wln("Usage: reboot: Restarts your simulated computer." + vbNewLine +
                "       Friends of reboot: shutdown, logout", "neutralText")

        ElseIf (command.Contains("reloadconfig")) Then

            Wln("Usage: reloadconfig: Reloads the configuration that is changed by the end-user or by tool." + vbNewLine +
                "       Colors doesn't require a restart, but most of the settings require you to restart.", "neutralText")

        ElseIf (command.Contains("rmuser")) Then

            Wln("Usage: rmuser <Username>" + vbNewLine +
                "       rmuser: to get prompted about removing usernames.", "neutralText")

        ElseIf (command.Contains("scical")) Then

            Wln("Usage: scical <expression1|pi|e> <+|-|*|/|%> <expression2|pi|e> ..." + vbNewLine +
                "       scical <sqrt|tan|sin|cos> <number>", "neutralText")

        ElseIf (command.Contains("setcolors")) Then

            Wln("Usage: setcolors <inputColor/def> <licenseColor/def> <contKernelErrorColor/def> <uncontKernelErrorColor/def> <hostNameShellColor/def> <userNameShellColor/def> <backgroundColor/def> <neutralTextColor/def>" + vbNewLine +
                "       setcolors: to get prompted about setting colors. (deprecated)" + vbNewLine +
                "       Friends of setcolors: setthemes", "neutralText")

        ElseIf (command.Contains("setthemes")) Then

            Wln("Usage: setthemes <Theme>" + vbNewLine +
                "       setthemes: to get prompted about setting themes. (deprecated)" + vbNewLine +
                "       Friends of setthemes: setcolors", "neutralText")

        ElseIf (command.Contains("showmotd")) Then

            Wln("Usage: showmotd: Shows your current Message Of The Day.", "neutralText")

        ElseIf (command.Contains("showtd")) Then

            Wln("Usage: showtd: Shows the date and time.", "neutralText")

        ElseIf (command.Contains("shutdown")) Then

            Wln("Usage: shutdown: Shuts down your simulated computer." + vbNewLine +
                "       Friends of shutdown: reboot, logout", "neutralText")

        ElseIf (command.Contains("sysinfo")) Then

            Wln("Usage: sysinfo: Shows system information and versions." + vbNewLine +
                "       Friends of sysinfo: lsdrivers, version", "neutralText")

        ElseIf (command.Contains("unitconv")) Then

            Wln("Usage: unitconv <sourceUnit> <targetUnit> <value>" + vbNewLine +
                "Units: B, KB, MB, GB, TB, Bits, Octal, Binary, Decimal, Hexadecimal, mm, cm, m, km, Fahrenheit, Celsius, Kelvin, " +
                "Reaumur, Romer, Delisle, Rankine, j, kj, m/s, km/h, cm/ms, Kilograms, Grams, Tons, Kilotons, Megatons, kn, n, Hz, kHz, MHz, " +
                "GHz, Number (source only), Money (target only), Percent (target only), Centivolts, Volts, Kilovolts, Watts, Kilowatts, " +
                "Milliliters, Liters, Kiloliters, Gallons, Ounces, Feet, Inches, Yards and Miles.", "neutralText")

        ElseIf (command.Contains("version")) Then

            Wln("Usage: version: Shows kernel version." + vbNewLine +
                "       Friends of version: lsdrivers, sysinfo", "neutralText")

        Else

            Wln("No help for {0}", "neutralText", command)

        End If

    End Sub

End Module
