//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Hardware;
using KS.Kernel;
using KS.Languages;
using KS.Login;
using KS.Misc.Probers;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
using Terminaux.Writer.FancyWriters;

namespace KS.Shell.Commands
{
    class SysInfoCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            bool ShowSystemInfo = default, ShowHardwareInfo = default, ShowUserInfo = default, ShowMessageOfTheDay = default, ShowMal = default;
            if (ListSwitchesOnly.Contains("-s"))
                ShowSystemInfo = true;
            if (ListSwitchesOnly.Contains("-h"))
                ShowHardwareInfo = true;
            if (ListSwitchesOnly.Contains("-u"))
                ShowUserInfo = true;
            if (ListSwitchesOnly.Contains("-m"))
                ShowMessageOfTheDay = true;
            if (ListSwitchesOnly.Contains("-l"))
                ShowMal = true;
            if (ListSwitchesOnly.Contains("-a") | ListSwitchesOnly.Length == 0)
            {
                ShowSystemInfo = true;
                ShowHardwareInfo = true;
                ShowUserInfo = true;
                ShowMessageOfTheDay = true;
                ShowMal = true;
            }

            if (ShowSystemInfo)
            {
                // Kernel section
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Kernel settings"), true);
                TextWriters.Write(Translate.DoTranslation("Kernel Version:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                TextWriters.Write(Kernel.Kernel.KernelVersion, true, KernelColorTools.ColTypes.ListValue);
                TextWriters.Write(Translate.DoTranslation("Debug Mode:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                TextWriters.Write(Flags.DebugMode.ToString(), true, KernelColorTools.ColTypes.ListValue);
                TextWriters.Write(Translate.DoTranslation("Colored Shell:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                TextWriters.Write(Shell.ColoredShell.ToString(), true, KernelColorTools.ColTypes.ListValue);
                TextWriters.Write(Translate.DoTranslation("Arguments on Boot:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                TextWriters.Write(Flags.ArgsOnBoot.ToString(), true, KernelColorTools.ColTypes.ListValue);
                TextWriters.Write(Translate.DoTranslation("Help command simplified:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                TextWriters.Write(Flags.SimHelp.ToString(), true, KernelColorTools.ColTypes.ListValue);
                TextWriters.Write(Translate.DoTranslation("MOTD on Login:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                TextWriters.Write(Flags.ShowMOTD.ToString(), true, KernelColorTools.ColTypes.ListValue);
                TextWriters.Write(Translate.DoTranslation("Time/Date on corner:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                TextWriters.Write(Flags.CornerTimeDate.ToString(), true, KernelColorTools.ColTypes.ListValue);
                TextWriters.Write("", KernelColorTools.ColTypes.Neutral);
            }

            if (ShowHardwareInfo)
            {
                // Hardware section
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Hardware settings"), true);
                HardwareList.ListHardware();
                TextWriters.Write(Translate.DoTranslation("Use \"hwinfo\" for extended information about hardware."), true, KernelColorTools.ColTypes.Tip);
                TextWriters.Write("", KernelColorTools.ColTypes.Neutral);
            }

            if (ShowUserInfo)
            {
                // User section
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("User settings"), true);
                TextWriters.Write(Translate.DoTranslation("Current user name:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                TextWriters.Write(Login.Login.CurrentUser.Username, true, KernelColorTools.ColTypes.ListValue);
                TextWriters.Write(Translate.DoTranslation("Current host name:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                TextWriters.Write(Kernel.Kernel.HostName, true, KernelColorTools.ColTypes.ListValue);
                TextWriters.Write(Translate.DoTranslation("Available usernames:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                TextWriters.Write(string.Join(", ", UserManagement.ListAllUsers()), true, KernelColorTools.ColTypes.ListValue);
                TextWriters.Write("", KernelColorTools.ColTypes.Neutral);
            }

            if (ShowMessageOfTheDay)
            {
                // Show MOTD
                SeparatorWriterColor.WriteSeparator("MOTD", true);
                TextWriters.Write(PlaceParse.ProbePlaces(Kernel.Kernel.MOTDMessage), true, KernelColorTools.ColTypes.Neutral);
            }

            if (ShowMal)
            {
                // Show MAL
                SeparatorWriterColor.WriteSeparator("MAL", true);
                TextWriters.Write(PlaceParse.ProbePlaces(Kernel.Kernel.MAL), true, KernelColorTools.ColTypes.Neutral);
            }
        }

        public override void HelpHelper()
        {
            TextWriters.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, KernelColorTools.ColTypes.Neutral);
            TextWriters.Write("  -s: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Shows the system information"), true, KernelColorTools.ColTypes.ListValue);
            TextWriters.Write("  -h: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Shows the hardware information"), true, KernelColorTools.ColTypes.ListValue);
            TextWriters.Write("  -u: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Shows the user information"), true, KernelColorTools.ColTypes.ListValue);
            TextWriters.Write("  -m: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Shows the message of the day"), true, KernelColorTools.ColTypes.ListValue);
            TextWriters.Write("  -l: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Shows the message of the day after login"), true, KernelColorTools.ColTypes.ListValue);
            TextWriters.Write("  -a: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Shows all information"), true, KernelColorTools.ColTypes.ListValue);
        }

    }
}
