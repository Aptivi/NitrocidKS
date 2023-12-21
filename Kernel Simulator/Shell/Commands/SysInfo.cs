using System;
using System.Linq;
using KS.ConsoleBase.Colors;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Hardware;
using KS.Kernel;
using KS.Languages;
using KS.Login;
using KS.Misc.Probers;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Shell.ShellBase.Commands;

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
				TextWriterColor.Write(Translate.DoTranslation("Kernel Version:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
				TextWriterColor.Write(Kernel.Kernel.KernelVersion, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
				TextWriterColor.Write(Translate.DoTranslation("Debug Mode:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
				TextWriterColor.Write(Flags.DebugMode.ToString(), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
				TextWriterColor.Write(Translate.DoTranslation("Colored Shell:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
				TextWriterColor.Write(Shell.ColoredShell.ToString(), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
				TextWriterColor.Write(Translate.DoTranslation("Arguments on Boot:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
				TextWriterColor.Write(Flags.ArgsOnBoot.ToString(), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
				TextWriterColor.Write(Translate.DoTranslation("Help command simplified:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
				TextWriterColor.Write(Flags.SimHelp.ToString(), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
				TextWriterColor.Write(Translate.DoTranslation("MOTD on Login:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
				TextWriterColor.Write(Flags.ShowMOTD.ToString(), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
				TextWriterColor.Write(Translate.DoTranslation("Time/Date on corner:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
				TextWriterColor.Write(Flags.CornerTimeDate.ToString(), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
				TextWriterColor.WritePlain("", true);
			}

			if (ShowHardwareInfo)
			{
				// Hardware section
				SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Hardware settings"), true);
				HardwareList.ListHardware();
				TextWriterColor.Write(Translate.DoTranslation("Use \"hwinfo\" for extended information about hardware."), true, KernelColorTools.ColTypes.Tip);
				TextWriterColor.WritePlain("", true);
			}

			if (ShowUserInfo)
			{
				// User section
				SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("User settings"), true);
				TextWriterColor.Write(Translate.DoTranslation("Current user name:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
				TextWriterColor.Write(Login.Login.CurrentUser.Username, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
				TextWriterColor.Write(Translate.DoTranslation("Current host name:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
				TextWriterColor.Write(Kernel.Kernel.HostName, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
				TextWriterColor.Write(Translate.DoTranslation("Available usernames:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
				TextWriterColor.Write(string.Join(", ", UserManagement.ListAllUsers()), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
				TextWriterColor.WritePlain("", true);
			}

			if (ShowMessageOfTheDay)
			{
				// Show MOTD
				SeparatorWriterColor.WriteSeparator("MOTD", true);
				TextWriterColor.Write(PlaceParse.ProbePlaces(Kernel.Kernel.MOTDMessage), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
			}

			if (ShowMal)
			{
				// Show MAL
				SeparatorWriterColor.WriteSeparator("MAL", true);
				TextWriterColor.Write(PlaceParse.ProbePlaces(Kernel.Kernel.MAL), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
			}
		}

		public override void HelpHelper()
		{
			TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
			TextWriterColor.Write("  -s: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
			TextWriterColor.Write(Translate.DoTranslation("Shows the system information"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
			TextWriterColor.Write("  -h: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
			TextWriterColor.Write(Translate.DoTranslation("Shows the hardware information"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
			TextWriterColor.Write("  -u: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
			TextWriterColor.Write(Translate.DoTranslation("Shows the user information"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
			TextWriterColor.Write("  -m: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
			TextWriterColor.Write(Translate.DoTranslation("Shows the message of the day"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
			TextWriterColor.Write("  -l: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
			TextWriterColor.Write(Translate.DoTranslation("Shows the message of the day after login"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
			TextWriterColor.Write("  -a: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
			TextWriterColor.Write(Translate.DoTranslation("Shows all information"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
		}

	}
}