using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

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

using global::System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using FluentFTP;
using FluentFTP.Helpers;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Themes;
using KS.ConsoleBase.Themes.Studio;
using KS.Files;
using global::KS.Files.PathLookup;
using global::KS.Files.Querying;
using KS.Kernel;
using KS.Languages;
using KS.Login;
using global::KS.Misc.Execution;
using KS.Misc.Platform;
using KS.Misc.Probers;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Writers.FancyWriters;
using global::KS.Modifications;
using global::KS.Scripting;
using KS.Shell;
using global::KS.Shell.Commands;
using KS.Shell.ShellBase;
using global::KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using global::KS.Shell.UnifiedCommands;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Constants;
using Newtonsoft.Json;
using Renci.SshNet;
using Terminaux.Base;
using Terminaux.Colors;

namespace KS.Shell
{
	public static class Shell
	{

		internal static global::KS.Misc.Threading.KernelThread ProcessStartCommandThread = new("Executable Command Thread", false, (_) => global::KS.Misc.Execution.ProcessExecutor.ExecuteProcess());
		internal readonly static global::System.Collections.Generic.Dictionary<global::System.String, global::KS.Shell.ShellBase.Commands.CommandInfo> ModCommands = new();
		internal readonly static global::System.Collections.Generic.List<global::System.String> InjectedCommands = new();

		/// <summary>
        /// Whether the shell is colored or not
        /// </summary>
		public static global::System.Boolean ColoredShell = true;
		/// <summary>
        /// Specifies where to lookup for executables in these paths. Same as in PATH implementation.
        /// </summary>
		public static global::System.String PathsToLookup = global::System.Environment.GetEnvironmentVariable("PATH");
		/// <summary>
        /// Path lookup delimiter, depending on the operating system
        /// </summary>
		public readonly static global::System.String PathLookupDelimiter = Conversions.ToString(global::System.IO.Path.PathSeparator);
		/// <summary>
        /// List of commands
        /// </summary>
		public readonly static global::System.Collections.Generic.Dictionary<global::System.String, global::KS.Shell.ShellBase.Commands.CommandInfo> Commands = new() { { "adduser", new global::KS.Shell.ShellBase.Commands.CommandInfo("adduser", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Adds users", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<userName> [password] [confirm]" }, true, 1), new global::KS.Shell.Commands.AddUserCommand(), true) }, { "alias", new global::KS.Shell.ShellBase.Commands.CommandInfo("alias", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Adds aliases to commands", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { $"<rem/add> <{global::System.String.Join("/", global::System.Enum.GetNames(typeof(global::KS.Shell.ShellBase.Shells.ShellType)))}> <alias> <cmd>" }, true, 3), new global::KS.Shell.Commands.AliasCommand(), true) }, { "arginj", new global::KS.Shell.ShellBase.Commands.CommandInfo("arginj", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Injects arguments to the kernel (reboot required)", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[Arguments separated by spaces]" }, true, 1), new global::KS.Shell.Commands.ArgInjCommand(), true) }, { "beep", new global::KS.Shell.ShellBase.Commands.CommandInfo("beep", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Beep in 'n' Hz and time in 'n' milliseconds", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<37-32767 Hz> <milliseconds>" }, true, 2), new global::KS.Shell.Commands.BeepCommand()) }, { "blockdbgdev", new global::KS.Shell.ShellBase.Commands.CommandInfo("blockdbgdev", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Block a debug device by IP address", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<ipaddress>" }, true, 1), new global::KS.Shell.Commands.BlockDbgDevCommand(), true) }, { "calc", new global::KS.Shell.ShellBase.Commands.CommandInfo("calc", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Calculator to calculate expressions.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<expression>" }, true, 1), new global::KS.Shell.Commands.CalcCommand()) }, { "calendar", new global::KS.Shell.ShellBase.Commands.CommandInfo("calendar", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Calendar, event, and reminder manager", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<show> [year] [month]", "<event> <add> <date> <title>", "<event> <remove> <eventid>", "<event> <list>", "<event> <saveall>", "<reminder> <add> <dateandtime> <title>", "<reminder> <remove> <reminderid>", "<reminder> <list>", "<reminder> <saveall>" }, true, 1), new global::KS.Shell.Commands.CalendarCommand()) }, { "cat", new global::KS.Shell.ShellBase.Commands.CommandInfo("cat", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Prints content of file to console", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[-lines|-nolines] <file>" }, true, 1), new global::KS.Shell.Commands.CatCommand(), false, true) }, { "cdbglog", new global::KS.Shell.ShellBase.Commands.CommandInfo("cdbglog", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Deletes everything in debug log", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.CdbgLogCommand(), true) }, { "chattr", new global::KS.Shell.ShellBase.Commands.CommandInfo("chattr", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Changes attribute of a file", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<file> +/-<attributes>" }, true, 2), new global::KS.Shell.Commands.ChAttrCommand()) }, { "chdir", new global::KS.Shell.ShellBase.Commands.CommandInfo("chdir", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Changes directory", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<directory/..>" }, true, 1), new global::KS.Shell.Commands.ChDirCommand()) }, { "chhostname", new global::KS.Shell.ShellBase.Commands.CommandInfo("chhostname", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Changes host name", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<HostName>" }, true, 1), new global::KS.Shell.Commands.ChHostNameCommand(), true) }, { "chlang", new global::KS.Shell.ShellBase.Commands.CommandInfo("chlang", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Changes language", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[-alwaystransliterated|-alwaystranslated|-force] <language>" }, true, 1), new global::KS.Shell.Commands.ChLangCommand(), true) }, { "chmal", new global::KS.Shell.ShellBase.Commands.CommandInfo("chmal", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Changes MAL, the MOTD After Login", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[Message]" }, false, 0), new global::KS.Shell.Commands.ChMalCommand(), true) }, { "chmotd", new global::KS.Shell.ShellBase.Commands.CommandInfo("chmotd", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Changes MOTD, the Message Of The Day", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[Message]" }, false, 0), new global::KS.Shell.Commands.ChMotdCommand(), true) }, { "choice", new global::KS.Shell.ShellBase.Commands.CommandInfo("choice", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Makes user choices", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[-o|-t|-m|-a] [-multiple|-single] <$variable> <answers> <input> [answertitle1] [answertitle2] ..." }, true, 3), new global::KS.Shell.Commands.ChoiceCommand(), false, false, false, false, true) }, { "chpwd", new global::KS.Shell.ShellBase.Commands.CommandInfo("chpwd", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Changes password for current user", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<Username> <UserPass> <newPass> <confirm>" }, true, 4), new global::KS.Shell.Commands.ChPwdCommand(), true) }, { "chusrname", new global::KS.Shell.ShellBase.Commands.CommandInfo("chusrname", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Changes user name", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<oldUserName> <newUserName>" }, true, 2), new global::KS.Shell.Commands.ChUsrNameCommand(), true) }, { "clearfiredevents", new global::KS.Shell.ShellBase.Commands.CommandInfo("clearfiredevents", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Clears all fired events", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.ClearFiredEventsCommand()) }, { "cls", new global::KS.Shell.ShellBase.Commands.CommandInfo("cls", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Clears the screen", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.ClsCommand()) }, { "colorhextorgb", new global::KS.Shell.ShellBase.Commands.CommandInfo("colorhextorgb", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Converts the hexadecimal representation of the color to RGB numbers.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<#RRGGBB>" }, true, 1), new global::KS.Shell.Commands.ColorHexToRgbCommand()) }, { "colorhextorgbks", new global::KS.Shell.ShellBase.Commands.CommandInfo("colorhextorgbks", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Converts the hexadecimal representation of the color to RGB numbers in KS format.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<#RRGGBB>" }, true, 1), new global::KS.Shell.Commands.ColorHexToRgbKSCommand()) }, { "colorrgbtohex", new global::KS.Shell.ShellBase.Commands.CommandInfo("colorrgbtohex", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Converts the color RGB numbers to hex.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<R> <G> <B>" }, true, 3), new global::KS.Shell.Commands.ColorRgbToHexCommand()) }, { "combine", new global::KS.Shell.ShellBase.Commands.CommandInfo("combine", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Combines the two text files or more into the output file.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<output> <input1> <input2> [input3] ..." }, true, 3), new global::KS.Shell.Commands.CombineCommand()) }, { "convertlineendings", new global::KS.Shell.ShellBase.Commands.CommandInfo("convertlineendings", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Converts the line endings to format for the current platform or to specified custom format", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<textfile> [-w|-u|-m]" }, true, 1), new global::KS.Shell.Commands.ConvertLineEndingsCommand()) }, { "copy", new global::KS.Shell.ShellBase.Commands.CommandInfo("copy", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Creates another copy of a file under different directory or name.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<source> <target>" }, true, 2), new global::KS.Shell.Commands.CopyCommand()) }, { "dict", new global::KS.Shell.ShellBase.Commands.CommandInfo("dict", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "The English Dictionary", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<word>" }, true, 1), new global::KS.Shell.Commands.DictCommand()) }, { "dirinfo", new global::KS.Shell.ShellBase.Commands.CommandInfo("dirinfo", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Provides information about a directory", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<directory>" }, true, 1), new global::KS.Shell.Commands.DirInfoCommand()) }, { "disconndbgdev", new global::KS.Shell.ShellBase.Commands.CommandInfo("disconndbgdev", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Disconnect a debug device", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<ip>" }, true, 1), new global::KS.Shell.Commands.DisconnDbgDevCommand(), true) }, { "dismissnotif", new global::KS.Shell.ShellBase.Commands.CommandInfo("dismissnotif", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Dismisses a notification", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<notificationNumber>" }, true, 1), new global::KS.Shell.Commands.DismissNotifCommand()) }, { "dismissnotifs", new global::KS.Shell.ShellBase.Commands.CommandInfo("dismissnotifs", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Dismisses all notifications", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.DismissNotifsCommand()) }, { "echo", new global::KS.Shell.ShellBase.Commands.CommandInfo("echo", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Writes text into the console", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[text]" }, false, 0), new global::KS.Shell.Commands.EchoCommand()) }, { "edit", new global::KS.Shell.ShellBase.Commands.CommandInfo("edit", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Edits a text file", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<file>" }, true, 1), new global::KS.Shell.Commands.EditCommand()) }, { "fileinfo", new global::KS.Shell.ShellBase.Commands.CommandInfo("fileinfo", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Provides information about a file", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<file>" }, true, 1), new global::KS.Shell.Commands.FileInfoCommand()) }, { "find", new global::KS.Shell.ShellBase.Commands.CommandInfo("find", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Finds a file in the specified directory or in the current directory", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<file> [directory]" }, true, 1), new global::KS.Shell.Commands.FindCommand()) }, { "firedevents", new global::KS.Shell.ShellBase.Commands.CommandInfo("firedevents", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Lists all fired events", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.FiredEventsCommand()) }, { "ftp", new global::KS.Shell.ShellBase.Commands.CommandInfo("ftp", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Use an FTP shell to interact with servers", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[server]" }, false, 0), new global::KS.Shell.Commands.FtpCommand()) }, { "genname", new global::KS.Shell.ShellBase.Commands.CommandInfo("genname", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Name and surname generator", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[namescount] [nameprefix] [namesuffix] [surnameprefix] [surnamesuffix]" }, false, 0), new global::KS.Shell.Commands.GenNameCommand()) }, { "gettimeinfo", new global::KS.Shell.ShellBase.Commands.CommandInfo("gettimeinfo", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Gets the date and time information", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<date>" }, true, 1), new global::KS.Shell.Commands.GetTimeInfoCommand()) }, { "get", new global::KS.Shell.ShellBase.Commands.CommandInfo("get", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Downloads a file to current working directory", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<URL>" }, true, 1), new global::KS.Shell.Commands.Get_Command()) }, { "help", new global::KS.Shell.ShellBase.Commands.CommandInfo("help", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Help page", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[command]" }, false, 0, global::KS.Shell.Commands.HelpCommand.ListCmds), new global::KS.Shell.Commands.HelpCommand()) }, { "hexedit", new global::KS.Shell.ShellBase.Commands.CommandInfo("hexedit", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Edits a binary file", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<file>" }, true, 1), new global::KS.Shell.Commands.HexEditCommand()) }, { "http", new global::KS.Shell.ShellBase.Commands.CommandInfo("http", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Starts the HTTP shell", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.HttpCommand()) }, { "hwinfo", new global::KS.Shell.ShellBase.Commands.CommandInfo("hwinfo", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Prints hardware information", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<HardwareType>" }, true, 1), new global::KS.Shell.Commands.HwInfoCommand(), false, true) }, { "if", new global::KS.Shell.ShellBase.Commands.CommandInfo("if", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Executes commands once the UESH expressions are satisfied", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<uesh-expression> <command>" }, true, 2), new global::KS.Shell.Commands.IfCommand()) }, { "ifm", new global::KS.Shell.ShellBase.Commands.CommandInfo("ifm", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Interactive system host file manager", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[firstPanePath] [secondPanePath]" }, false, 0), new global::KS.Shell.Commands.IfmCommand()) }, { "input", new global::KS.Shell.ShellBase.Commands.CommandInfo("input", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Allows user to enter input", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<$variable> <question>" }, true, 2), new global::KS.Shell.Commands.InputCommand(), false, false, false, false, true) }, { "jsonbeautify", new global::KS.Shell.ShellBase.Commands.CommandInfo("jsonbeautify", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Beautifies the JSON file", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<jsonfile> [output]" }, true, 1), new global::KS.Shell.Commands.JsonBeautifyCommand(), false, true) }, { "jsonminify", new global::KS.Shell.ShellBase.Commands.CommandInfo("jsonminify", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Minifies the JSON file", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<jsonfile> [output]" }, true, 1), new global::KS.Shell.Commands.JsonMinifyCommand(), false, true) }, { "jsonshell", new global::KS.Shell.ShellBase.Commands.CommandInfo("jsonshell", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Opens the JSON shell", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<jsonfile>" }, true, 1), new global::KS.Shell.Commands.JsonShellCommand()) }, { "keyinfo", new global::KS.Shell.ShellBase.Commands.CommandInfo("keyinfo", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Gets key information for a pressed key. Useful for debugging", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "" }, false, 0), new global::KS.Shell.Commands.KeyInfoCommand()) }, { "langman", new global::KS.Shell.ShellBase.Commands.CommandInfo("langman", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Manage your languages", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<reload/load/unload> <customlanguagename>", "<list/reloadall>" }, true, 1), new global::KS.Shell.Commands.LangManCommand(), true) }, { "list", new global::KS.Shell.ShellBase.Commands.CommandInfo("list", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "List file/folder contents in current folder", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[-showdetails|-suppressmessages] [directory]" }, false, 0), new global::KS.Shell.Commands.ListCommand(), false, true) }, { "lockscreen", new global::KS.Shell.ShellBase.Commands.CommandInfo("lockscreen", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Locks your screen with a password", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.LockScreenCommand()) }, { "logout", new global::KS.Shell.ShellBase.Commands.CommandInfo("logout", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Logs you out", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.LogoutCommand(), false, false, true) }, { "lovehate", new global::KS.Shell.ShellBase.Commands.CommandInfo("lovehate", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Respond to love or hate comments.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.LoveHateCommand()) }, { "lsdbgdev", new global::KS.Shell.ShellBase.Commands.CommandInfo("lsdbgdev", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Lists debugging devices connected", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.LsDbgDevCommand(), true, true) }, { "lsvars", new global::KS.Shell.ShellBase.Commands.CommandInfo("lsvars", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Lists available UESH variables", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.LsVarsCommand(), false, true) }, { "mail", new global::KS.Shell.ShellBase.Commands.CommandInfo("mail", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Opens the mail client", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[emailAddress]" }, false, 0), new global::KS.Shell.Commands.MailCommand()) }, { "md", new global::KS.Shell.ShellBase.Commands.CommandInfo("md", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Creates a directory", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<directory>" }, true, 1), new global::KS.Shell.Commands.MdCommand()) }, { "meteor", new global::KS.Shell.ShellBase.Commands.CommandInfo("meteor", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "You are a spaceship and the meteors are coming to destroy you. Can you save it?", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.MeteorCommand()) }, { "mkfile", new global::KS.Shell.ShellBase.Commands.CommandInfo("mkfile", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Makes a new file", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<file>" }, true, 1), new global::KS.Shell.Commands.MkFileCommand()) }, { "mktheme", new global::KS.Shell.ShellBase.Commands.CommandInfo("mktheme", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Makes a new theme", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<themeName>" }, true, 1), new global::KS.Shell.Commands.MkThemeCommand()) }, { "modman", new global::KS.Shell.ShellBase.Commands.CommandInfo("modman", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Manage your mods", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<start/stop/info/reload/install/uninstall> <modfilename>", "<list/listparts> [modname]", "<reloadall/stopall/startall>" }, true, 1), new global::KS.Shell.Commands.ModManCommand(), true) }, { "modmanual", new global::KS.Shell.ShellBase.Commands.CommandInfo("modmanual", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Mod manual", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[-list] <ManualTitle>" }, true, 1), new global::KS.Shell.Commands.ModManualCommand()) }, { "move", new global::KS.Shell.ShellBase.Commands.CommandInfo("move", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Moves a file to another directory", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<source> <target>" }, true, 2), new global::KS.Shell.Commands.MoveCommand()) }, { "netinfo", new global::KS.Shell.ShellBase.Commands.CommandInfo("netinfo", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Lists information about all available interfaces", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.NetInfoCommand(), true, true) }, { "open", new global::KS.Shell.ShellBase.Commands.CommandInfo("open", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Opens a URL", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<URL>" }, true, 1), new global::KS.Shell.Commands.OpenCommand()) }, { "perm", new global::KS.Shell.ShellBase.Commands.CommandInfo("perm", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Manage permissions for users", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<userName> <Administrator/Disabled/Anonymous> <Allow/Disallow>" }, true, 3), new global::KS.Shell.Commands.PermCommand(), true) }, { "ping", new global::KS.Shell.ShellBase.Commands.CommandInfo("ping", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Pings an address", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[times] <Address1> <Address2> ..." }, true, 1), new global::KS.Shell.Commands.PingCommand()) }, { "put", new global::KS.Shell.ShellBase.Commands.CommandInfo("put", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Uploads a file to specified website", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<FileName> <URL>" }, true, 2), new global::KS.Shell.Commands.PutCommand()) }, { "rarshell", new global::KS.Shell.ShellBase.Commands.CommandInfo("rarshell", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "The RAR shell", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<rarfile>" }, true, 1), new global::KS.Shell.Commands.RarShellCommand()) }, { "reboot", new global::KS.Shell.ShellBase.Commands.CommandInfo("reboot", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Restarts your computer (WARNING: No syncing, because it is not a final kernel)", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[ip] [port]" }, false, 0), new global::KS.Shell.Commands.RebootCommand()) }, { "reloadconfig", new global::KS.Shell.ShellBase.Commands.CommandInfo("reloadconfig", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Reloads configuration file that is edited.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.ReloadConfigCommand(), true, false, false, false, true) }, { "reloadsaver", new global::KS.Shell.ShellBase.Commands.CommandInfo("reloadsaver", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Reloads screensaver file in KSMods", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<customsaver>" }, true, 1), new global::KS.Shell.Commands.ReloadSaverCommand(), true, false, false, false, true) }, { "retroks", new global::KS.Shell.ShellBase.Commands.CommandInfo("retroks", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Retro Kernel Simulator based on 0.0.4.1", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.RetroKSCommand()) }, { "rexec", new global::KS.Shell.ShellBase.Commands.CommandInfo("rexec", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Remotely executes a command to remote PC", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<address> [port] <command>" }, true, 2), new global::KS.Shell.Commands.RexecCommand(), true) }, { "rm", new global::KS.Shell.ShellBase.Commands.CommandInfo("rm", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Removes a directory or a file", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<directory/file>" }, true, 1), new global::KS.Shell.Commands.RmCommand()) }, { "rdebug", new global::KS.Shell.ShellBase.Commands.CommandInfo("rdebug", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Enables or disables remote debugging.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.RdebugCommand(), true) }, { "reportbug", new global::KS.Shell.ShellBase.Commands.CommandInfo("reportbug", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "A bug reporting prompt.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.ReportBugCommand()) }, { "rmuser", new global::KS.Shell.ShellBase.Commands.CommandInfo("rmuser", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Removes a user from the list", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<Username>" }, true, 1), new global::KS.Shell.Commands.RmUserCommand(), true) }, { "rss", new global::KS.Shell.ShellBase.Commands.CommandInfo("rss", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Opens an RSS shell to read the feeds", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[feedlink]" }, false, 0), new global::KS.Shell.Commands.RssCommand()) }, { "savecurrdir", new global::KS.Shell.ShellBase.Commands.CommandInfo("savecurrdir", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Saves the current directory to kernel configuration file", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.SaveCurrDirCommand(), true) }, { "savescreen", new global::KS.Shell.ShellBase.Commands.CommandInfo("savescreen", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Saves your screen from burn outs", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[saver]" }, false, 0), new global::KS.Shell.Commands.SaveScreenCommand()) }, { "search", new global::KS.Shell.ShellBase.Commands.CommandInfo("search", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Searches for specified string in the provided file using regular expressions", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<Regexp> <File>" }, true, 2), new global::KS.Shell.Commands.SearchCommand()) }, { "searchword", new global::KS.Shell.ShellBase.Commands.CommandInfo("searchword", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Searches for specified string in the provided file", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<StringEnclosedInDoubleQuotes> <File>" }, true, 2), new global::KS.Shell.Commands.SearchWordCommand()) }, { "select", new global::KS.Shell.ShellBase.Commands.CommandInfo("select", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Provides a selection choice", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<$variable> <answers> <input> [answertitle1] [answertitle2] ..." }, true, 3), new global::KS.Shell.Commands.SelectCommand(), false, false, false, false, true) }, { "setsaver", new global::KS.Shell.ShellBase.Commands.CommandInfo("setsaver", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Sets up kernel screensavers", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<customsaver/builtinsaver>" }, true, 1), new global::KS.Shell.Commands.SetSaverCommand(), true) }, { "setthemes", new global::KS.Shell.ShellBase.Commands.CommandInfo("setthemes", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Sets up kernel themes", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<Theme>" }, true, 1), new global::KS.Shell.Commands.SetThemesCommand()) }, { "settings", new global::KS.Shell.ShellBase.Commands.CommandInfo("settings", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Changes kernel configuration", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[-saver|-splash]" }, false, 0), new global::KS.Shell.Commands.SettingsCommand(), true) }, { "set", new global::KS.Shell.ShellBase.Commands.CommandInfo("set", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Sets a variable to a value in a script", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<$variable> <value>" }, true, 2), new global::KS.Shell.Commands.SetCommand(), false, false, false, false, true) }, { "setrange", new global::KS.Shell.ShellBase.Commands.CommandInfo("setrange", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Creates a variable array with the provided values", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<$variablename> <value1> [value2] [value3] ..." }, true, 2), new global::KS.Shell.Commands.SetRangeCommand(), false, false, false, false, true) }, { "sftp", new global::KS.Shell.ShellBase.Commands.CommandInfo("sftp", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Lets you use an SSH FTP server", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[server]" }, false, 0), new global::KS.Shell.Commands.SftpCommand()) }, { "shownotifs", new global::KS.Shell.ShellBase.Commands.CommandInfo("shownotifs", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Shows all received notifications", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.ShowNotifsCommand()) }, { "showtd", new global::KS.Shell.ShellBase.Commands.CommandInfo("showtd", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Shows date and time", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.ShowTdCommand()) }, { "showtdzone", new global::KS.Shell.ShellBase.Commands.CommandInfo("showtdzone", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Shows date and time in zones", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[-all] <timezone>" }, true, 1), new global::KS.Shell.Commands.ShowTdZoneCommand(), false, true) }, { "shutdown", new global::KS.Shell.ShellBase.Commands.CommandInfo("shutdown", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "The kernel will be shut down", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[ip] [port]" }, false, 0), new global::KS.Shell.Commands.ShutdownCommand()) }, { "snaker", new global::KS.Shell.ShellBase.Commands.CommandInfo("snaker", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "The snake game!", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.SnakerCommand()) }, { "solver", new global::KS.Shell.ShellBase.Commands.CommandInfo("solver", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "See if you can solve mathematical equations on time", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.SolverCommand()) }, { "speedpress", new global::KS.Shell.ShellBase.Commands.CommandInfo("speedpress", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "See if you can press a key on time", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[-e|-m|-h|-v|-c] [timeout]" }, false, 0), new global::KS.Shell.Commands.SpeedPressCommand()) }, { "spellbee", new global::KS.Shell.ShellBase.Commands.CommandInfo("spellbee", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "See if you can spell words correctly on time", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.SpellBeeCommand()) }, { "sshell", new global::KS.Shell.ShellBase.Commands.CommandInfo("sshell", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Connects to an SSH server.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<address:port> <username>" }, true, 2), new global::KS.Shell.Commands.SshellCommand()) }, { "sshcmd", new global::KS.Shell.ShellBase.Commands.CommandInfo("sshcmd", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Connects to an SSH server to execute a command.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<address:port> <username> \"<command>\"" }, true, 3), new global::KS.Shell.Commands.SshcmdCommand()) }, { "stopwatch", new global::KS.Shell.ShellBase.Commands.CommandInfo("stopwatch", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "A simple stopwatch", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.StopwatchCommand()) }, { "sumfile", new global::KS.Shell.ShellBase.Commands.CommandInfo("sumfile", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Calculates file sums.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[-relative] <MD5/SHA1/SHA256/SHA384/SHA512/all> <file> [outputFile]" }, true, 2), new global::KS.Shell.Commands.SumFileCommand()) }, { "sumfiles", new global::KS.Shell.ShellBase.Commands.CommandInfo("sumfiles", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Calculates sums of files in specified directory.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[-relative] <MD5/SHA1/SHA256/SHA384/SHA512/all> <dir> [outputFile]" }, true, 2), new global::KS.Shell.Commands.SumFilesCommand()) }, { "sysinfo", new global::KS.Shell.ShellBase.Commands.CommandInfo("sysinfo", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "System information", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[-s|-h|-u|-m|-l|-a]" }, false, 0), new global::KS.Shell.Commands.SysInfoCommand()) }, { "testshell", new global::KS.Shell.ShellBase.Commands.CommandInfo("testshell", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Opens a test shell", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.TestShellCommand(), true) }, { "timer", new global::KS.Shell.ShellBase.Commands.CommandInfo("timer", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "A simple timer", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.TimerCommand()) }, { "unblockdbgdev", new global::KS.Shell.ShellBase.Commands.CommandInfo("unblockdbgdev", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Unblock a debug device by IP address", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<ipaddress>" }, true, 1), new global::KS.Shell.Commands.UnblockDbgDevCommand(), true) }, { "unitconv", new global::KS.Shell.ShellBase.Commands.CommandInfo("unitconv", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Unit converter", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<unittype> <quantity> <sourceunit> <targetunit>" }, true, 4), new global::KS.Shell.Commands.UnitConvCommand()) }, { "unzip", new global::KS.Shell.ShellBase.Commands.CommandInfo("unzip", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Extracts a ZIP archive", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<zipfile> [path] [-createdir]" }, true, 1), new global::KS.Shell.Commands.UnZipCommand()) }, { "update", new global::KS.Shell.ShellBase.Commands.CommandInfo("update", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "System update", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.UpdateCommand(), true) }, { "usermanual", new global::KS.Shell.ShellBase.Commands.CommandInfo("usermanual", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Takes you to our GitHub Wiki.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[-modapi]" }, false, 0), new global::KS.Shell.Commands.UserManualCommand()) }, { "verify", new global::KS.Shell.ShellBase.Commands.CommandInfo("verify", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Verifies sanity of the file", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<MD5/SHA1/SHA256/SHA384/SHA512> <calculatedhash> <hashfile/expectedhash> <file>" }, true, 4), new global::KS.Shell.Commands.VerifyCommand()) }, { "weather", new global::KS.Shell.ShellBase.Commands.CommandInfo("weather", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Shows weather info for specified city. Uses OpenWeatherMap.", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "[-list] <CityID/CityName> [apikey]" }, true, 1), new global::KS.Shell.Commands.WeatherCommand(), false, false, false, false, true) }, { "wrap", new global::KS.Shell.ShellBase.Commands.CommandInfo("wrap", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Wraps the console output", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<command>" }, true, 1), new global::KS.Shell.Commands.WrapCommand(), false, false, false, false, true) }, { "zip", new global::KS.Shell.ShellBase.Commands.CommandInfo("zip", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Creates a ZIP archive", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<zipfile> <path> [-fast|-nocomp|-nobasedir]" }, true, 2), new global::KS.Shell.Commands.ZipCommand()) }, { "zipshell", new global::KS.Shell.ShellBase.Commands.CommandInfo("zipshell", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Opens a ZIP archive", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(new[] { "<zipfile>" }, true, 1), new global::KS.Shell.Commands.ZipShellCommand()) } };
		/// <summary>
        /// List of unified commands
        /// </summary>
		public readonly static global::System.Collections.Generic.Dictionary<global::System.String, global::KS.Shell.ShellBase.Commands.CommandInfo> UnifiedCommandDict = new() { { "presets", new global::KS.Shell.ShellBase.Commands.CommandInfo("presets", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Opens the shell preset library", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.UnifiedCommands.PresetsUnifiedCommand()) }, { "exit", new global::KS.Shell.ShellBase.Commands.CommandInfo("exit", global::KS.Shell.ShellBase.Shells.ShellType.Shell, "Exits the shell if running on subshell", new global::KS.Shell.ShellBase.Commands.CommandArgumentInfo(global::System.Array.Empty<string>(), false, 0), new global::KS.Shell.Commands.ExitCommand()) } };

		/// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="IsInvokedByKernelArgument">Indicates whether it was invoked by kernel argument parse (for internal use only)</param>
        /// <param name="OutputPath">Optional (non-)neutralized output path</param>
        /// <param name="ShellType">Shell type</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
		public static void GetLine(global::System.String FullCommand, global::System.Boolean IsInvokedByKernelArgument = false, global::System.String OutputPath = "", global::KS.Shell.ShellBase.Shells.ShellType ShellType = global::KS.Shell.ShellBase.Shells.ShellType.Shell)
		{
			// Check for sanity
			if (global::System.String.IsNullOrEmpty(FullCommand))
				FullCommand = "";

			// Variables
			var OutputTextWriter = default(global::System.IO.StreamWriter);
			global::System.IO.FileStream OutputStream;

			// Check for a type of command
			global::System.String[] SplitCommands = FullCommand.Split(new[] { " : " }, global::System.StringSplitOptions.RemoveEmptyEntries);
			foreach (global::System.String Command in SplitCommands)
			{
				global::System.Boolean Done = false;

				// Check to see if the command is a comment
				if ((global::System.Boolean)(((string.IsNullOrEmpty(Command) | (Command?.StartsWithAnyOf(new[] { " ", "#" })))) == false))
				{
					global::System.String[] Parts = Command.SplitEncloseDoubleQuotes();

					// Iterate through mod commands
					global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.I, "Mod commands probing started with {0} from {1}", Command, FullCommand);
					if (global::KS.Modifications.ModManager.ListModCommands(ShellType).ContainsKey(Parts[0]))
					{
						Done = true;
						global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.I, "Mod command: {0}", Parts[0]);
						global::KS.Modifications.ModExecutor.ExecuteModCommand(Command);
					}

					// Iterate through alias commands
					global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.I, "Aliases probing started with {0} from {1}", Command, FullCommand);
					if (global::KS.Shell.ShellBase.Aliases.AliasManager.GetAliasesListFromType(ShellType).ContainsKey(Parts[0]))
					{
						Done = true;
						global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.I, "Alias: {0}", Parts[0]);
						global::KS.Shell.ShellBase.Aliases.AliasExecutor.ExecuteAlias(Command, ShellType);
					}

					// Execute the built-in command
					if (((Done) == (false)))
					{
						global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.I, "Executing built-in command");

						// If requested command has output redirection sign after arguments, remove it from final command string and set output to that file
						global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.I, "Does the command contain the redirection sign \">>>\" or \">>\"? {0} and {1}", (global::System.Object)Command.Contains(">>>"), (global::System.Object)Command.Contains(">>"));
						if (Command.Contains(">>>"))
						{
							global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.I, "Output redirection found with append.");
							global::System.String OutputFileName = Command.Substring((Command.LastIndexOf(">")) + (2));
							global::KS.Kernel.Kernel.DefConsoleOut = global::System.Console.Out;
							OutputStream = new global::System.IO.FileStream(global::KS.Files.Filesystem.NeutralizePath(OutputFileName), global::System.IO.FileMode.Append, global::System.IO.FileAccess.Write);
							OutputTextWriter = new global::System.IO.StreamWriter(OutputStream) { AutoFlush = true };
							global::System.Console.SetOut(OutputTextWriter);
							Command = Command.Replace((" >>> ") + (OutputFileName), "");
						}
						else if (Command.Contains(">>"))
						{
							global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.I, "Output redirection found with overwrite.");
							global::System.String OutputFileName = Command.Substring((Command.LastIndexOf(">")) + (2));
							global::KS.Kernel.Kernel.DefConsoleOut = global::System.Console.Out;
							OutputStream = new global::System.IO.FileStream(global::KS.Files.Filesystem.NeutralizePath(OutputFileName), global::System.IO.FileMode.OpenOrCreate, global::System.IO.FileAccess.Write);
							OutputTextWriter = new global::System.IO.StreamWriter(OutputStream) { AutoFlush = true };
							global::System.Console.SetOut(OutputTextWriter);
							Command = Command.Replace((" >> ") + (OutputFileName), "");
						}

						// Checks to see if the user provided optional path
						if (!(global::System.String.IsNullOrWhiteSpace(OutputPath)))
						{
							global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.I, "Optional output redirection found using OutputPath ({0}).", OutputPath);
							global::KS.Kernel.Kernel.DefConsoleOut = global::System.Console.Out;
							OutputStream = new global::System.IO.FileStream(global::KS.Files.Filesystem.NeutralizePath(OutputPath), global::System.IO.FileMode.OpenOrCreate, global::System.IO.FileAccess.Write);
							OutputTextWriter = new global::System.IO.StreamWriter(OutputStream) { AutoFlush = true };
							global::System.Console.SetOut(OutputTextWriter);
						}

						// Reads command written by user
						do
						{
							try
							{
								global::System.String EntireCommand = Command;
								if (!(((string.IsNullOrEmpty(Command) | ((Command.StartsWithAnyOf(new[] { " ", "#" })) == (true))))))
								{
									global::KS.ConsoleBase.ConsoleExtensions.SetTitle($"{global::KS.Kernel.Kernel.ConsoleTitle} - {Command}");

									// Parse script command (if any)
									var scriptArgs = Command.Split(new[] { ".uesh " }, global::System.StringSplitOptions.RemoveEmptyEntries).ToList();
									scriptArgs.RemoveAt(0);

									// Get the index of the first space
									global::System.Int32 indexCmd = Command.IndexOf(" ");
									global::System.String cmdArgs = Command; // Command with args
									global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.I, "Prototype indexCmd and Command: {0}, {1}", (global::System.Object)indexCmd, Command);
									if (((indexCmd) == (-(1))))
										indexCmd = Command.Length;
									Command = Command.Substring(0, indexCmd);
									global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.I, "Finished indexCmd and Command: {0}, {1}", (global::System.Object)indexCmd, Command);

									// Scan PATH for file existence and set file name as needed
									global::System.String TargetFile = "";
									global::System.String TargetFileName = "";
									global::KS.Files.PathLookup.PathLookupTools.FileExistsInPath(Command, ref TargetFile);
									if (global::System.String.IsNullOrEmpty(TargetFile))
										TargetFile = global::KS.Files.Filesystem.NeutralizePath(Command);
									if (global::KS.Files.Querying.Parsing.TryParsePath(TargetFile))
										TargetFileName = global::System.IO.Path.GetFileName(TargetFile);

									// Check to see if a user is able to execute a command
									var Commands = global::KS.Shell.ShellBase.Commands.GetCommand.GetCommands(ShellType);
									if (Commands.ContainsKey(Command))
									{
										if ((ShellType == global::KS.Shell.ShellBase.Shells.ShellType.Shell))
										{
											if ((((global::KS.Login.PermissionManagement.HasPermission(global::KS.Login.Login.CurrentUser.Username, global::KS.Login.PermissionManagement.PermissionType.Administrator)) == (false)) & (Commands[Command].Strict)))
											{
												global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", Command);
												global::KS.Misc.Writers.ConsoleWriters.TextWriterColor.Write(global::KS.Languages.Translate.DoTranslation("You don't have permission to use {0}"), true, color: global::KS.ConsoleBase.Colors.KernelColorTools.GetConsoleColor(global::KS.ConsoleBase.Colors.KernelColorTools.ColTypes.Error), Command);
												break;
											}
										}

										if ((((global::KS.Kernel.Flags.Maintenance) == (true)) & (Commands[Command].NoMaintenance)))
										{
											global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.W, "Cmd exec {0} failed: In maintenance mode. {0} is in NoMaintenanceCmds", Command);
											global::KS.Misc.Writers.ConsoleWriters.TextWriterColor.Write(global::KS.Languages.Translate.DoTranslation("Shell message: The requested command {0} is not allowed to run in maintenance mode."), true, color: global::KS.ConsoleBase.Colors.KernelColorTools.GetConsoleColor(global::KS.ConsoleBase.Colors.KernelColorTools.ColTypes.Error), Command);
										}
										else if (((IsInvokedByKernelArgument) & (((((Command.StartsWith("logout")) | (Command.StartsWith("shutdown"))) | (Command.StartsWith("reboot")))))))
										{
											global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.W, "Cmd exec {0} failed: cmd is one of \"logout\" or \"shutdown\" or \"reboot\"", Command);
											global::KS.Misc.Writers.ConsoleWriters.TextWriterColor.Write(global::KS.Languages.Translate.DoTranslation("Shell message: Command {0} is not allowed to run on log in."), true, color: global::KS.ConsoleBase.Colors.KernelColorTools.GetConsoleColor(global::KS.ConsoleBase.Colors.KernelColorTools.ColTypes.Error), Command);
										}
										else
										{
											global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.I, "Cmd exec {0} succeeded. Running with {1}", Command, cmdArgs);
											var Params = new global::KS.Shell.ShellBase.Commands.GetCommand.ExecuteCommandThreadParameters(EntireCommand, ShellType, (global::System.IO.StreamWriter)null);

											// Since we're probably trying to run a command using the alternative command threads, if the main shell command thread
											// is running, use that to execute the command. This ensures that commands like "wrap" that also execute commands from the
											// shell can do their job.
											var ShellInstance = global::KS.Shell.ShellBase.Shells.ShellStart.ShellStack[(global::KS.Shell.ShellBase.Shells.ShellStart.ShellStack.Count) - (1)];
											var StartCommandThread = ShellInstance.ShellCommandThread;
											global::System.Boolean CommandThreadValid = true;
											if (StartCommandThread.IsAlive)
											{
												if (((ShellInstance.AltCommandThreads.Count) > (0)))
												{
													StartCommandThread = ShellInstance.AltCommandThreads[(ShellInstance.AltCommandThreads.Count) - (1)];
												}
												else
												{
													global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.W, "Cmd exec {0} failed: Alt command threads are not there.");
													CommandThreadValid = false;
												}
											}
											if (CommandThreadValid)
											{
												StartCommandThread.Start(Params);
												StartCommandThread.Wait();
												StartCommandThread.Stop();
											}
										}
									}
									else if (((global::KS.Files.Querying.Parsing.TryParsePath(TargetFile)) & (ShellType == global::KS.Shell.ShellBase.Shells.ShellType.Shell)))
									{
										// If we're in the UESH shell, parse the script file or executable file
										if (((global::KS.Files.Querying.Checking.FileExists(TargetFile)) & (!(TargetFile.EndsWith(".uesh")))))
										{
											global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.I, "Cmd exec {0} succeeded because file is found.", Command);
											try
											{
												// Create a new instance of process
												if (global::KS.Files.Querying.Parsing.TryParsePath(TargetFile))
												{
													cmdArgs = cmdArgs.Replace(TargetFileName, "");
													cmdArgs.Trim();
													global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.I, "Command: {0}, Arguments: {1}", TargetFile, cmdArgs);
													var Params = new global::KS.Misc.Execution.ProcessExecutor.ExecuteProcessThreadParameters(TargetFile, cmdArgs);
													global::KS.Shell.Shell.ProcessStartCommandThread.Start(Params);
													global::KS.Shell.Shell.ProcessStartCommandThread.Wait();
													global::KS.Shell.Shell.ProcessStartCommandThread.Stop();
												}
											}
											catch (global::System.Exception ex)
											{
												global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.E, "Failed to start process: {0}", ex.Message);
												global::KS.Misc.Writers.ConsoleWriters.TextWriterColor.Write(global::KS.Languages.Translate.DoTranslation("Failed to start \"{0}\": {1}"), true, color: global::KS.ConsoleBase.Colors.KernelColorTools.GetConsoleColor(global::KS.ConsoleBase.Colors.KernelColorTools.ColTypes.Error), Command, ex.Message);
												global::KS.Misc.Writers.DebugWriters.DebugWriter.WStkTrc(ex);
											}
										}
										else if (((global::KS.Files.Querying.Checking.FileExists(TargetFile)) & (TargetFile.EndsWith(".uesh"))))
										{
											try
											{
												global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.I, "Cmd exec {0} succeeded because it's a UESH script.", Command);
												global::KS.Scripting.UESHParse.Execute(TargetFile, scriptArgs.Join(" "));
											}
											catch (global::System.Exception ex)
											{
												global::KS.Misc.Writers.ConsoleWriters.TextWriterColor.Write(global::KS.Languages.Translate.DoTranslation("Error trying to execute script: {0}"), true, color: global::KS.ConsoleBase.Colors.KernelColorTools.GetConsoleColor(global::KS.ConsoleBase.Colors.KernelColorTools.ColTypes.Error), ex.Message);
												global::KS.Misc.Writers.DebugWriters.DebugWriter.WStkTrc(ex);
											}
										}
										else
										{
											global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.W, "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", Command, (global::System.Object)indexCmd);
											global::KS.Misc.Writers.ConsoleWriters.TextWriterColor.Write(global::KS.Languages.Translate.DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), true, color: global::KS.ConsoleBase.Colors.KernelColorTools.GetConsoleColor(global::KS.ConsoleBase.Colors.KernelColorTools.ColTypes.Error), Command);
										}
									}
									else
									{
										global::KS.Misc.Writers.DebugWriters.DebugWriter.Wdbg(global::KS.Misc.Writers.DebugWriters.DebugLevel.W, "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", Command, (global::System.Object)indexCmd);
										global::KS.Misc.Writers.ConsoleWriters.TextWriterColor.Write(global::KS.Languages.Translate.DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), true, color: global::KS.ConsoleBase.Colors.KernelColorTools.GetConsoleColor(global::KS.ConsoleBase.Colors.KernelColorTools.ColTypes.Error), Command);
									}
								}
							}
							catch (global::System.Exception ex)
							{
								global::KS.Misc.Writers.DebugWriters.DebugWriter.WStkTrc(ex);
								global::KS.Misc.Writers.ConsoleWriters.TextWriterColor.Write(((global::KS.Languages.Translate.DoTranslation("Error trying to execute command.")) + (global::KS.Kernel.Kernel.NewLine)) + (global::KS.Languages.Translate.DoTranslation("Error {0}: {1}")), true, global::KS.ConsoleBase.Colors.KernelColorTools.GetConsoleColor(global::KS.ConsoleBase.Colors.KernelColorTools.ColTypes.Error), ex.GetType().FullName, ex.Message);
							}
						}
						while (false);
					}
				}
			}
			global::KS.ConsoleBase.ConsoleExtensions.SetTitle(global::KS.Kernel.Kernel.ConsoleTitle);

			// Restore console output to its original state if any
			/* TODO ERROR: Skipped WarningDirectiveTrivia
			#Disable Warning BC42104
			*/
			if (global::KS.Kernel.Kernel.DefConsoleOut is not null)
			{
				global::System.Console.SetOut(global::KS.Kernel.Kernel.DefConsoleOut);
				OutputTextWriter?.Close();
			}
			/* TODO ERROR: Skipped WarningDirectiveTrivia
			#Enable Warning BC42104
			*/
		}

	}
}