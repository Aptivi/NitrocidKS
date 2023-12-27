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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FluentFTP.Helpers;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.PathLookup;
using KS.Files.Querying;
using KS.Kernel;
using KS.Languages;
using KS.Login;
using KS.Misc.Execution;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Modifications;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.UnifiedCommands;

namespace KS.Shell
{
    /// <summary>
    /// Shell tools
    /// </summary>
    public static class Shell
    {

        internal static KernelThread ProcessStartCommandThread = new("Executable Command Thread", false, (param) => ProcessExecutor.ExecuteProcess((ProcessExecutor.ExecuteProcessThreadParameters)param));
        internal static readonly Dictionary<string, CommandInfo> ModCommands = [];
        internal static readonly List<string> InjectedCommands = [];

        /// <summary>
        /// Whether the shell is colored or not
        /// </summary>
        public static bool ColoredShell = true;
        /// <summary>
        /// Specifies where to lookup for executables in these paths. Same as in PATH implementation.
        /// </summary>
        public static string PathsToLookup = Environment.GetEnvironmentVariable("PATH");
        /// <summary>
        /// Path lookup delimiter, depending on the operating system
        /// </summary>
        public static readonly string PathLookupDelimiter = Convert.ToString(Path.PathSeparator);
        /// <summary>
        /// List of commands
        /// </summary>
        public static readonly Dictionary<string, CommandInfo> Commands = new()
        {
            { "adduser", new CommandInfo("adduser", ShellType.Shell, "Adds users", new CommandArgumentInfo(["<userName> [password] [confirm]"], true, 1), new Commands.AddUserCommand(), true) },
            { "alias", new CommandInfo("alias", ShellType.Shell, "Adds aliases to commands", new CommandArgumentInfo([$"<rem/add> <{string.Join("/", Enum.GetNames(typeof(ShellType)))}> <alias> <cmd>"], true, 3), new Commands.AliasCommand(), true) },
            { "arginj", new CommandInfo("arginj", ShellType.Shell, "Injects arguments to the kernel (reboot required)", new CommandArgumentInfo(["[Arguments separated by spaces]"], true, 1), new Commands.ArgInjCommand(), true) },
            { "beep", new CommandInfo("beep", ShellType.Shell, "Beep in 'n' Hz and time in 'n' milliseconds", new CommandArgumentInfo(["<37-32767 Hz> <milliseconds>"], true, 2), new Commands.BeepCommand()) },
            { "blockdbgdev", new CommandInfo("blockdbgdev", ShellType.Shell, "Block a debug device by IP address", new CommandArgumentInfo(["<ipaddress>"], true, 1), new Commands.BlockDbgDevCommand(), true) },
            { "calc", new CommandInfo("calc", ShellType.Shell, "Calculator to calculate expressions.", new CommandArgumentInfo(["<expression>"], true, 1), new Commands.CalcCommand()) },
            { "calendar", new CommandInfo("calendar", ShellType.Shell, "Calendar, event, and reminder manager", new CommandArgumentInfo(["<show> [year] [month]", "<event> <add> <date> <title>", "<event> <remove> <eventid>", "<event> <list>", "<event> <saveall>", "<reminder> <add> <dateandtime> <title>", "<reminder> <remove> <reminderid>", "<reminder> <list>", "<reminder> <saveall>"], true, 1), new Commands.CalendarCommand()) },
            { "cat", new CommandInfo("cat", ShellType.Shell, "Prints content of file to console", new CommandArgumentInfo(["[-lines|-nolines] <file>"], true, 1), new Commands.CatCommand(), false, true) },
            { "cdbglog", new CommandInfo("cdbglog", ShellType.Shell, "Deletes everything in debug log", new CommandArgumentInfo([], false, 0), new Commands.CdbgLogCommand(), true) },
            { "chattr", new CommandInfo("chattr", ShellType.Shell, "Changes attribute of a file", new CommandArgumentInfo(["<file> +/-<attributes>"], true, 2), new Commands.ChAttrCommand()) },
            { "chdir", new CommandInfo("chdir", ShellType.Shell, "Changes directory", new CommandArgumentInfo(["<directory/..>"], true, 1), new Commands.ChDirCommand()) },
            { "chhostname", new CommandInfo("chhostname", ShellType.Shell, "Changes host name", new CommandArgumentInfo(["<HostName>"], true, 1), new Commands.ChHostNameCommand(), true) },
            { "chlang", new CommandInfo("chlang", ShellType.Shell, "Changes language", new CommandArgumentInfo(["[-alwaystransliterated|-alwaystranslated|-force] <language>"], true, 1), new Commands.ChLangCommand(), true) },
            { "chmal", new CommandInfo("chmal", ShellType.Shell, "Changes MAL, the MOTD After Login", new CommandArgumentInfo(["[Message]"], false, 0), new Commands.ChMalCommand(), true) },
            { "chmotd", new CommandInfo("chmotd", ShellType.Shell, "Changes MOTD, the Message Of The Day", new CommandArgumentInfo(["[Message]"], false, 0), new Commands.ChMotdCommand(), true) },
            { "choice", new CommandInfo("choice", ShellType.Shell, "Makes user choices", new CommandArgumentInfo(["[-o|-t|-m|-a] [-multiple|-single] <$variable> <answers> <input> [answertitle1] [answertitle2] ..."], true, 3), new Commands.ChoiceCommand(), false, false, false, false, true) },
            { "chpwd", new CommandInfo("chpwd", ShellType.Shell, "Changes password for current user", new CommandArgumentInfo(["<Username> <UserPass> <newPass> <confirm>"], true, 4), new Commands.ChPwdCommand(), true) },
            { "chusrname", new CommandInfo("chusrname", ShellType.Shell, "Changes user name", new CommandArgumentInfo(["<oldUserName> <newUserName>"], true, 2), new Commands.ChUsrNameCommand(), true) },
            { "clearfiredevents", new CommandInfo("clearfiredevents", ShellType.Shell, "Clears all fired events", new CommandArgumentInfo([], false, 0), new Commands.ClearFiredEventsCommand()) },
            { "cls", new CommandInfo("cls", ShellType.Shell, "Clears the screen", new CommandArgumentInfo([], false, 0), new Commands.ClsCommand()) },
            { "colorhextorgb", new CommandInfo("colorhextorgb", ShellType.Shell, "Converts the hexadecimal representation of the color to RGB numbers.", new CommandArgumentInfo(["<#RRGGBB>"], true, 1), new Commands.ColorHexToRgbCommand()) },
            { "colorhextorgbks", new CommandInfo("colorhextorgbks", ShellType.Shell, "Converts the hexadecimal representation of the color to RGB numbers in KS format.", new CommandArgumentInfo(["<#RRGGBB>"], true, 1), new Commands.ColorHexToRgbKSCommand()) },
            { "colorrgbtohex", new CommandInfo("colorrgbtohex", ShellType.Shell, "Converts the color RGB numbers to hex.", new CommandArgumentInfo(["<R> <G> <B>"], true, 3), new Commands.ColorRgbToHexCommand()) },
            { "combine", new CommandInfo("combine", ShellType.Shell, "Combines the two text files or more into the output file.", new CommandArgumentInfo(["<output> <input1> <input2> [input3] ..."], true, 3), new Commands.CombineCommand()) },
            { "convertlineendings", new CommandInfo("convertlineendings", ShellType.Shell, "Converts the line endings to format for the current platform or to specified custom format", new CommandArgumentInfo(["<textfile> [-w|-u|-m]"], true, 1), new Commands.ConvertLineEndingsCommand()) },
            { "copy", new CommandInfo("copy", ShellType.Shell, "Creates another copy of a file under different directory or name.", new CommandArgumentInfo(["<source> <target>"], true, 2), new Commands.CopyCommand()) },
            { "dict", new CommandInfo("dict", ShellType.Shell, "The English Dictionary", new CommandArgumentInfo(["<word>"], true, 1), new Commands.DictCommand()) },
            { "dirinfo", new CommandInfo("dirinfo", ShellType.Shell, "Provides information about a directory", new CommandArgumentInfo(["<directory>"], true, 1), new Commands.DirInfoCommand()) },
            { "disconndbgdev", new CommandInfo("disconndbgdev", ShellType.Shell, "Disconnect a debug device", new CommandArgumentInfo(["<ip>"], true, 1), new Commands.DisconnDbgDevCommand(), true) },
            { "dismissnotif", new CommandInfo("dismissnotif", ShellType.Shell, "Dismisses a notification", new CommandArgumentInfo(["<notificationNumber>"], true, 1), new Commands.DismissNotifCommand()) },
            { "dismissnotifs", new CommandInfo("dismissnotifs", ShellType.Shell, "Dismisses all notifications", new CommandArgumentInfo([], false, 0), new Commands.DismissNotifsCommand()) },
            { "echo", new CommandInfo("echo", ShellType.Shell, "Writes text into the console", new CommandArgumentInfo(["[text]"], false, 0), new Commands.EchoCommand()) },
            { "edit", new CommandInfo("edit", ShellType.Shell, "Edits a text file", new CommandArgumentInfo(["<file>"], true, 1), new Commands.EditCommand()) },
            { "fileinfo", new CommandInfo("fileinfo", ShellType.Shell, "Provides information about a file", new CommandArgumentInfo(["<file>"], true, 1), new Commands.FileInfoCommand()) },
            { "find", new CommandInfo("find", ShellType.Shell, "Finds a file in the specified directory or in the current directory", new CommandArgumentInfo(["<file> [directory]"], true, 1), new Commands.FindCommand()) },
            { "firedevents", new CommandInfo("firedevents", ShellType.Shell, "Lists all fired events", new CommandArgumentInfo([], false, 0), new Commands.FiredEventsCommand()) },
            { "ftp", new CommandInfo("ftp", ShellType.Shell, "Use an FTP shell to interact with servers", new CommandArgumentInfo(["[server]"], false, 0), new Commands.FtpCommand()) },
            { "genname", new CommandInfo("genname", ShellType.Shell, "Name and surname generator", new CommandArgumentInfo(["[namescount] [nameprefix] [namesuffix] [surnameprefix] [surnamesuffix]"], false, 0), new Commands.GenNameCommand()) },
            { "gettimeinfo", new CommandInfo("gettimeinfo", ShellType.Shell, "Gets the date and time information", new CommandArgumentInfo(["<date>"], true, 1), new Commands.GetTimeInfoCommand()) },
            { "get", new CommandInfo("get", ShellType.Shell, "Downloads a file to current working directory", new CommandArgumentInfo(["<URL>"], true, 1), new Commands.Get_Command()) },
            { "help", new CommandInfo("help", ShellType.Shell, "Help page", new CommandArgumentInfo(["[command]"], false, 0, KS.Shell.Commands.HelpCommand.ListCmds), new Commands.HelpCommand()) },
            { "hexedit", new CommandInfo("hexedit", ShellType.Shell, "Edits a binary file", new CommandArgumentInfo(["<file>"], true, 1), new Commands.HexEditCommand()) },
            { "http", new CommandInfo("http", ShellType.Shell, "Starts the HTTP shell", new CommandArgumentInfo([], false, 0), new Commands.HttpCommand()) },
            { "hwinfo", new CommandInfo("hwinfo", ShellType.Shell, "Prints hardware information", new CommandArgumentInfo(["<HardwareType>"], true, 1), new Commands.HwInfoCommand(), false, true) },
            { "if", new CommandInfo("if", ShellType.Shell, "Executes commands once the UESH expressions are satisfied", new CommandArgumentInfo(["<uesh-expression> <command>"], true, 2), new Commands.IfCommand()) },
            { "ifm", new CommandInfo("ifm", ShellType.Shell, "Interactive system host file manager", new CommandArgumentInfo(["[firstPanePath] [secondPanePath]"], false, 0), new Commands.IfmCommand()) },
            { "input", new CommandInfo("input", ShellType.Shell, "Allows user to enter input", new CommandArgumentInfo(["<$variable> <question>"], true, 2), new Commands.InputCommand(), false, false, false, false, true) },
            { "jsonbeautify", new CommandInfo("jsonbeautify", ShellType.Shell, "Beautifies the JSON file", new CommandArgumentInfo(["<jsonfile> [output]"], true, 1), new Commands.JsonBeautifyCommand(), false, true) },
            { "jsonminify", new CommandInfo("jsonminify", ShellType.Shell, "Minifies the JSON file", new CommandArgumentInfo(["<jsonfile> [output]"], true, 1), new Commands.JsonMinifyCommand(), false, true) },
            { "jsonshell", new CommandInfo("jsonshell", ShellType.Shell, "Opens the JSON shell", new CommandArgumentInfo(["<jsonfile>"], true, 1), new Commands.JsonShellCommand()) },
            { "keyinfo", new CommandInfo("keyinfo", ShellType.Shell, "Gets key information for a pressed key. Useful for debugging", new CommandArgumentInfo([""], false, 0), new Commands.KeyInfoCommand()) },
            { "langman", new CommandInfo("langman", ShellType.Shell, "Manage your languages", new CommandArgumentInfo(["<reload/load/unload> <customlanguagename>", "<list/reloadall>"], true, 1), new Commands.LangManCommand(), true) },
            { "list", new CommandInfo("list", ShellType.Shell, "List file/folder contents in current folder", new CommandArgumentInfo(["[-showdetails|-suppressmessages] [directory]"], false, 0), new Commands.ListCommand(), false, true) },
            { "lockscreen", new CommandInfo("lockscreen", ShellType.Shell, "Locks your screen with a password", new CommandArgumentInfo([], false, 0), new Commands.LockScreenCommand()) },
            { "logout", new CommandInfo("logout", ShellType.Shell, "Logs you out", new CommandArgumentInfo([], false, 0), new Commands.LogoutCommand(), false, false, true) },
            { "lovehate", new CommandInfo("lovehate", ShellType.Shell, "Respond to love or hate comments.", new CommandArgumentInfo([], false, 0), new Commands.LoveHateCommand()) },
            { "lsdbgdev", new CommandInfo("lsdbgdev", ShellType.Shell, "Lists debugging devices connected", new CommandArgumentInfo([], false, 0), new Commands.LsDbgDevCommand(), true, true) },
            { "lsvars", new CommandInfo("lsvars", ShellType.Shell, "Lists available UESH variables", new CommandArgumentInfo([], false, 0), new Commands.LsVarsCommand(), false, true) },
            { "mail", new CommandInfo("mail", ShellType.Shell, "Opens the mail client", new CommandArgumentInfo(["[emailAddress]"], false, 0), new Commands.MailCommand()) },
            { "md", new CommandInfo("md", ShellType.Shell, "Creates a directory", new CommandArgumentInfo(["<directory>"], true, 1), new Commands.MdCommand()) },
            { "meteor", new CommandInfo("meteor", ShellType.Shell, "You are a spaceship and the meteors are coming to destroy you. Can you save it?", new CommandArgumentInfo([], false, 0), new Commands.MeteorCommand()) },
            { "mkfile", new CommandInfo("mkfile", ShellType.Shell, "Makes a new file", new CommandArgumentInfo(["<file>"], true, 1), new Commands.MkFileCommand()) },
            { "mktheme", new CommandInfo("mktheme", ShellType.Shell, "Makes a new theme", new CommandArgumentInfo(["<themeName>"], true, 1), new Commands.MkThemeCommand()) },
            { "modman", new CommandInfo("modman", ShellType.Shell, "Manage your mods", new CommandArgumentInfo(["<start/stop/info/reload/install/uninstall> <modfilename>", "<list/listparts> [modname]", "<reloadall/stopall/startall>"], true, 1), new Commands.ModManCommand(), true) },
            { "modmanual", new CommandInfo("modmanual", ShellType.Shell, "Mod manual", new CommandArgumentInfo(["[-list] <ManualTitle>"], true, 1), new Commands.ModManualCommand()) },
            { "move", new CommandInfo("move", ShellType.Shell, "Moves a file to another directory", new CommandArgumentInfo(["<source> <target>"], true, 2), new Commands.MoveCommand()) },
            { "netinfo", new CommandInfo("netinfo", ShellType.Shell, "Lists information about all available interfaces", new CommandArgumentInfo([], false, 0), new Commands.NetInfoCommand(), true, true) },
            { "open", new CommandInfo("open", ShellType.Shell, "Opens a URL", new CommandArgumentInfo(["<URL>"], true, 1), new Commands.OpenCommand()) },
            { "perm", new CommandInfo("perm", ShellType.Shell, "Manage permissions for users", new CommandArgumentInfo(["<userName> <Administrator/Disabled/Anonymous> <Allow/Disallow>"], true, 3), new Commands.PermCommand(), true) },
            { "ping", new CommandInfo("ping", ShellType.Shell, "Pings an address", new CommandArgumentInfo(["[times] <Address1> <Address2> ..."], true, 1), new Commands.PingCommand()) },
            { "put", new CommandInfo("put", ShellType.Shell, "Uploads a file to specified website", new CommandArgumentInfo(["<FileName> <URL>"], true, 2), new Commands.PutCommand()) },
            { "rarshell", new CommandInfo("rarshell", ShellType.Shell, "The RAR shell", new CommandArgumentInfo(["<rarfile>"], true, 1), new Commands.RarShellCommand()) },
            { "reboot", new CommandInfo("reboot", ShellType.Shell, "Restarts your computer (WARNING: No syncing, because it is not a final kernel)", new CommandArgumentInfo(["[ip] [port]"], false, 0), new Commands.RebootCommand()) },
            { "reloadconfig", new CommandInfo("reloadconfig", ShellType.Shell, "Reloads configuration file that is edited.", new CommandArgumentInfo([], false, 0), new Commands.ReloadConfigCommand(), true, false, false, false, true) },
            { "reloadsaver", new CommandInfo("reloadsaver", ShellType.Shell, "Reloads screensaver file in KSMods", new CommandArgumentInfo(["<customsaver>"], true, 1), new Commands.ReloadSaverCommand(), true, false, false, false, true) },
            { "retroks", new CommandInfo("retroks", ShellType.Shell, "Retro Kernel Simulator based on 0.0.4.1", new CommandArgumentInfo([], false, 0), new Commands.RetroKSCommand()) },
            { "rexec", new CommandInfo("rexec", ShellType.Shell, "Remotely executes a command to remote PC", new CommandArgumentInfo(["<address> [port] <command>"], true, 2), new Commands.RexecCommand(), true) },
            { "rm", new CommandInfo("rm", ShellType.Shell, "Removes a directory or a file", new CommandArgumentInfo(["<directory/file>"], true, 1), new Commands.RmCommand()) },
            { "rdebug", new CommandInfo("rdebug", ShellType.Shell, "Enables or disables remote debugging.", new CommandArgumentInfo([], false, 0), new Commands.RdebugCommand(), true) },
            { "reportbug", new CommandInfo("reportbug", ShellType.Shell, "A bug reporting prompt.", new CommandArgumentInfo([], false, 0), new Commands.ReportBugCommand()) },
            { "rmuser", new CommandInfo("rmuser", ShellType.Shell, "Removes a user from the list", new CommandArgumentInfo(["<Username>"], true, 1), new Commands.RmUserCommand(), true) },
            { "rss", new CommandInfo("rss", ShellType.Shell, "Opens an RSS shell to read the feeds", new CommandArgumentInfo(["[feedlink]"], false, 0), new Commands.RssCommand()) },
            { "savecurrdir", new CommandInfo("savecurrdir", ShellType.Shell, "Saves the current directory to kernel configuration file", new CommandArgumentInfo([], false, 0), new Commands.SaveCurrDirCommand(), true) },
            { "savescreen", new CommandInfo("savescreen", ShellType.Shell, "Saves your screen from burn outs", new CommandArgumentInfo(["[saver]"], false, 0), new Commands.SaveScreenCommand()) },
            { "search", new CommandInfo("search", ShellType.Shell, "Searches for specified string in the provided file using regular expressions", new CommandArgumentInfo(["<Regexp> <File>"], true, 2), new Commands.SearchCommand()) },
            { "searchword", new CommandInfo("searchword", ShellType.Shell, "Searches for specified string in the provided file", new CommandArgumentInfo(["<StringEnclosedInDoubleQuotes> <File>"], true, 2), new Commands.SearchWordCommand()) },
            { "select", new CommandInfo("select", ShellType.Shell, "Provides a selection choice", new CommandArgumentInfo(["<$variable> <answers> <input> [answertitle1] [answertitle2] ..."], true, 3), new Commands.SelectCommand(), false, false, false, false, true) },
            { "setsaver", new CommandInfo("setsaver", ShellType.Shell, "Sets up kernel screensavers", new CommandArgumentInfo(["<customsaver/builtinsaver>"], true, 1), new Commands.SetSaverCommand(), true) },
            { "setthemes", new CommandInfo("setthemes", ShellType.Shell, "Sets up kernel themes", new CommandArgumentInfo(["<Theme>"], true, 1), new Commands.SetThemesCommand()) },
            { "settings", new CommandInfo("settings", ShellType.Shell, "Changes kernel configuration", new CommandArgumentInfo(["[-saver|-splash]"], false, 0), new Commands.SettingsCommand(), true) },
            { "set", new CommandInfo("set", ShellType.Shell, "Sets a variable to a value in a script", new CommandArgumentInfo(["<$variable> <value>"], true, 2), new Commands.SetCommand(), false, false, false, false, true) },
            { "setrange", new CommandInfo("setrange", ShellType.Shell, "Creates a variable array with the provided values", new CommandArgumentInfo(["<$variablename> <value1> [value2] [value3] ..."], true, 2), new Commands.SetRangeCommand(), false, false, false, false, true) },
            { "sftp", new CommandInfo("sftp", ShellType.Shell, "Lets you use an SSH FTP server", new CommandArgumentInfo(["[server]"], false, 0), new Commands.SftpCommand()) },
            { "shownotifs", new CommandInfo("shownotifs", ShellType.Shell, "Shows all received notifications", new CommandArgumentInfo([], false, 0), new Commands.ShowNotifsCommand()) },
            { "showtd", new CommandInfo("showtd", ShellType.Shell, "Shows date and time", new CommandArgumentInfo([], false, 0), new Commands.ShowTdCommand()) },
            { "showtdzone", new CommandInfo("showtdzone", ShellType.Shell, "Shows date and time in zones", new CommandArgumentInfo(["[-all] <timezone>"], true, 1), new Commands.ShowTdZoneCommand(), false, true) },
            { "shutdown", new CommandInfo("shutdown", ShellType.Shell, "The kernel will be shut down", new CommandArgumentInfo(["[ip] [port]"], false, 0), new Commands.ShutdownCommand()) },
            { "snaker", new CommandInfo("snaker", ShellType.Shell, "The snake game!", new CommandArgumentInfo([], false, 0), new Commands.SnakerCommand()) },
            { "solver", new CommandInfo("solver", ShellType.Shell, "See if you can solve mathematical equations on time", new CommandArgumentInfo([], false, 0), new Commands.SolverCommand()) },
            { "speedpress", new CommandInfo("speedpress", ShellType.Shell, "See if you can press a key on time", new CommandArgumentInfo(["[-e|-m|-h|-v|-c] [timeout]"], false, 0), new Commands.SpeedPressCommand()) },
            { "spellbee", new CommandInfo("spellbee", ShellType.Shell, "See if you can spell words correctly on time", new CommandArgumentInfo([], false, 0), new Commands.SpellBeeCommand()) },
            { "sshell", new CommandInfo("sshell", ShellType.Shell, "Connects to an SSH server.", new CommandArgumentInfo(["<address:port> <username>"], true, 2), new Commands.SshellCommand()) },
            { "sshcmd", new CommandInfo("sshcmd", ShellType.Shell, "Connects to an SSH server to execute a command.", new CommandArgumentInfo(["<address:port> <username> \"<command>\""], true, 3), new Commands.SshcmdCommand()) },
            { "stopwatch", new CommandInfo("stopwatch", ShellType.Shell, "A simple stopwatch", new CommandArgumentInfo([], false, 0), new Commands.StopwatchCommand()) },
            { "sumfile", new CommandInfo("sumfile", ShellType.Shell, "Calculates file sums.", new CommandArgumentInfo(["[-relative] <MD5/SHA1/SHA256/SHA384/SHA512/all> <file> [outputFile]"], true, 2), new Commands.SumFileCommand()) },
            { "sumfiles", new CommandInfo("sumfiles", ShellType.Shell, "Calculates sums of files in specified directory.", new CommandArgumentInfo(["[-relative] <MD5/SHA1/SHA256/SHA384/SHA512/all> <dir> [outputFile]"], true, 2), new Commands.SumFilesCommand()) },
            { "sysinfo", new CommandInfo("sysinfo", ShellType.Shell, "System information", new CommandArgumentInfo(["[-s|-h|-u|-m|-l|-a]"], false, 0), new Commands.SysInfoCommand()) },
            { "testshell", new CommandInfo("testshell", ShellType.Shell, "Opens a test shell", new CommandArgumentInfo([], false, 0), new Commands.TestShellCommand(), true) },
            { "timer", new CommandInfo("timer", ShellType.Shell, "A simple timer", new CommandArgumentInfo([], false, 0), new Commands.TimerCommand()) },
            { "unblockdbgdev", new CommandInfo("unblockdbgdev", ShellType.Shell, "Unblock a debug device by IP address", new CommandArgumentInfo(["<ipaddress>"], true, 1), new Commands.UnblockDbgDevCommand(), true) },
            { "unitconv", new CommandInfo("unitconv", ShellType.Shell, "Unit converter", new CommandArgumentInfo(["<unittype> <quantity> <sourceunit> <targetunit>"], true, 4), new Commands.UnitConvCommand()) },
            { "unzip", new CommandInfo("unzip", ShellType.Shell, "Extracts a ZIP archive", new CommandArgumentInfo(["<zipfile> [path] [-createdir]"], true, 1), new Commands.UnZipCommand()) },
            { "update", new CommandInfo("update", ShellType.Shell, "System update", new CommandArgumentInfo([], false, 0), new Commands.UpdateCommand(), true) },
            { "usermanual", new CommandInfo("usermanual", ShellType.Shell, "Takes you to our GitHub Wiki.", new CommandArgumentInfo(["[-modapi]"], false, 0), new Commands.UserManualCommand()) },
            { "verify", new CommandInfo("verify", ShellType.Shell, "Verifies sanity of the file", new CommandArgumentInfo(["<MD5/SHA1/SHA256/SHA384/SHA512> <calculatedhash> <hashfile/expectedhash> <file>"], true, 4), new Commands.VerifyCommand()) },
            { "weather", new CommandInfo("weather", ShellType.Shell, "Shows weather info for specified city. Uses OpenWeatherMap.", new CommandArgumentInfo(["[-list] <CityID/CityName> [apikey]"], true, 1), new Commands.WeatherCommand(), false, false, false, false, true) },
            { "wrap", new CommandInfo("wrap", ShellType.Shell, "Wraps the console output", new CommandArgumentInfo(["<command>"], true, 1), new Commands.WrapCommand(), false, false, false, false, true) },
            { "zip", new CommandInfo("zip", ShellType.Shell, "Creates a ZIP archive", new CommandArgumentInfo(["<zipfile> <path> [-fast|-nocomp|-nobasedir]"], true, 2), new Commands.ZipCommand()) },
            { "zipshell", new CommandInfo("zipshell", ShellType.Shell, "Opens a ZIP archive", new CommandArgumentInfo(["<zipfile>"], true, 1), new Commands.ZipShellCommand()) }
        };

        /// <summary>
        /// List of unified commands
        /// </summary>
        public static readonly Dictionary<string, CommandInfo> UnifiedCommandDict = new()
        {
            { "presets", new CommandInfo("presets", ShellType.Shell, "Opens the shell preset library", new CommandArgumentInfo([], false, 0), new PresetsUnifiedCommand()) },
            { "exit", new CommandInfo("exit", ShellType.Shell, "Exits the shell if running on subshell", new CommandArgumentInfo([], false, 0), new ExitCommand()) }
        };

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="IsInvokedByKernelArgument">Indicates whether it was invoked by kernel argument parse (for internal use only)</param>
        /// <param name="OutputPath">Optional (non-)neutralized output path</param>
        /// <param name="ShellType">Shell type</param>
        /// <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand, bool IsInvokedByKernelArgument = false, string OutputPath = "", ShellType ShellType = ShellType.Shell)
        {
            // Check for sanity
            if (string.IsNullOrEmpty(FullCommand))
                FullCommand = "";

            // Variables
            var OutputTextWriter = default(StreamWriter);
            FileStream OutputStream;

            // Check for a type of command
            string[] SplitCommands = FullCommand.Split(new[] { " : " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string Command in SplitCommands)
            {
                bool Done = false;
                string finalCommand = Command;

                // Check to see if the command is a comment
                if ((string.IsNullOrEmpty(finalCommand) | (finalCommand?.StartsWithAnyOf([" ", "#"]))) == false)
                {
                    string[] Parts = finalCommand.SplitEncloseDoubleQuotes();

                    // Iterate through mod commands
                    DebugWriter.Wdbg(DebugLevel.I, "Mod commands probing started with {0} from {1}", finalCommand, FullCommand);
                    if (ModManager.ListModCommands(ShellType).ContainsKey(Parts[0]))
                    {
                        Done = true;
                        DebugWriter.Wdbg(DebugLevel.I, "Mod command: {0}", Parts[0]);
                        ModExecutor.ExecuteModCommand(finalCommand);
                    }

                    // Iterate through alias commands
                    DebugWriter.Wdbg(DebugLevel.I, "Aliases probing started with {0} from {1}", finalCommand, FullCommand);
                    if (AliasManager.GetAliasesListFromType(ShellType).ContainsKey(Parts[0]))
                    {
                        Done = true;
                        DebugWriter.Wdbg(DebugLevel.I, "Alias: {0}", Parts[0]);
                        AliasExecutor.ExecuteAlias(finalCommand, ShellType);
                    }

                    // Execute the built-in command
                    if (!Done)
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Executing built-in command");

                        // If requested command has output redirection sign after arguments, remove it from final command string and set output to that file
                        DebugWriter.Wdbg(DebugLevel.I, "Does the command contain the redirection sign \">>>\" or \">>\"? {0} and {1}", Command.Contains(">>>"), Command.Contains(">>"));
                        if (finalCommand.Contains(">>>"))
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Output redirection found with append.");
                            string OutputFileName = finalCommand.Substring(finalCommand.LastIndexOf(">") + 2);
                            Kernel.Kernel.DefConsoleOut = Console.Out;
                            OutputStream = new FileStream(Filesystem.NeutralizePath(OutputFileName), FileMode.Append, FileAccess.Write);
                            OutputTextWriter = new StreamWriter(OutputStream) { AutoFlush = true };
                            Console.SetOut(OutputTextWriter);
                            finalCommand = finalCommand.Replace(" >>> " + OutputFileName, "");
                        }
                        else if (finalCommand.Contains(">>"))
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Output redirection found with overwrite.");
                            string OutputFileName = finalCommand.Substring(finalCommand.LastIndexOf(">") + 2);
                            Kernel.Kernel.DefConsoleOut = Console.Out;
                            OutputStream = new FileStream(Filesystem.NeutralizePath(OutputFileName), FileMode.OpenOrCreate, FileAccess.Write);
                            OutputTextWriter = new StreamWriter(OutputStream) { AutoFlush = true };
                            Console.SetOut(OutputTextWriter);
                            finalCommand = finalCommand.Replace(" >> " + OutputFileName, "");
                        }

                        // Checks to see if the user provided optional path
                        if (!string.IsNullOrWhiteSpace(OutputPath))
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Optional output redirection found using OutputPath ({0}).", OutputPath);
                            Kernel.Kernel.DefConsoleOut = Console.Out;
                            OutputStream = new FileStream(Filesystem.NeutralizePath(OutputPath), FileMode.OpenOrCreate, FileAccess.Write);
                            OutputTextWriter = new StreamWriter(OutputStream) { AutoFlush = true };
                            Console.SetOut(OutputTextWriter);
                        }

                        // Reads command written by user
                        try
                        {
                            string EntireCommand = finalCommand;
                            if (!(string.IsNullOrEmpty(finalCommand) | (finalCommand.StartsWithAnyOf([" ", "#"]) == true)))
                            {
                                ConsoleExtensions.SetTitle($"{Kernel.Kernel.ConsoleTitle} - {finalCommand}");

                                // Parse script command (if any)
                                var scriptArgs = finalCommand.Split(new[] { ".uesh " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                scriptArgs.RemoveAt(0);

                                // Get the index of the first space
                                int indexCmd = finalCommand.IndexOf(" ");
                                string cmdArgs = finalCommand; // Command with args
                                DebugWriter.Wdbg(DebugLevel.I, "Prototype indexCmd and finalCommand: {0}, {1}", indexCmd, finalCommand);
                                if (indexCmd == (-1))
                                    indexCmd = finalCommand.Length;
                                finalCommand = finalCommand.Substring(0, indexCmd);
                                DebugWriter.Wdbg(DebugLevel.I, "Finished indexCmd and finalCommand: {0}, {1}", indexCmd, finalCommand);

                                // Scan PATH for file existence and set file name as needed
                                string TargetFile = "";
                                string TargetFileName = "";
                                PathLookupTools.FileExistsInPath(finalCommand, ref TargetFile);
                                if (string.IsNullOrEmpty(TargetFile))
                                    TargetFile = Filesystem.NeutralizePath(finalCommand);
                                if (Parsing.TryParsePath(TargetFile))
                                    TargetFileName = Path.GetFileName(TargetFile);

                                // Check to see if a user is able to execute a command
                                var Commands = GetCommand.GetCommands(ShellType);
                                if (Commands.ContainsKey(finalCommand))
                                {
                                    if (ShellType == ShellType.Shell)
                                    {
                                        if ((PermissionManagement.HasPermission(Login.Login.CurrentUser.Username, PermissionManagement.PermissionType.Administrator) == false) & (Commands[finalCommand].Strict))
                                        {
                                            DebugWriter.Wdbg(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", Command);
                                            TextWriterColor.Write(Translate.DoTranslation("You don't have permission to use {0}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), Command);
                                            break;
                                        }
                                    }

                                    if ((Flags.Maintenance == true) & (Commands[finalCommand].NoMaintenance))
                                    {
                                        DebugWriter.Wdbg(DebugLevel.W, "Cmd exec {0} failed: In maintenance mode. {0} is in NoMaintenanceCmds", finalCommand);
                                        TextWriterColor.Write(Translate.DoTranslation("Shell message: The requested command {0} is not allowed to run in maintenance mode."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), finalCommand);
                                    }
                                    else if ((IsInvokedByKernelArgument) & ((finalCommand.StartsWith("logout")) | (finalCommand.StartsWith("shutdown")) | (finalCommand.StartsWith("reboot"))))
                                    {
                                        DebugWriter.Wdbg(DebugLevel.W, "Cmd exec {0} failed: cmd is one of \"logout\" or \"shutdown\" or \"reboot\"", finalCommand);
                                        TextWriterColor.Write(Translate.DoTranslation("Shell message: Command {0} is not allowed to run on log in."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), finalCommand);
                                    }
                                    else
                                    {
                                        DebugWriter.Wdbg(DebugLevel.I, "Cmd exec {0} succeeded. Running with {1}", finalCommand, cmdArgs);
                                        var Params = new GetCommand.ExecuteCommandThreadParameters(EntireCommand, ShellType, null);

                                        // Since we're probably trying to run a command using the alternative command threads, if the main shell command thread
                                        // is running, use that to execute the command. This ensures that commands like "wrap" that also execute commands from the
                                        // shell can do their job.
                                        var ShellInstance = ShellStart.ShellStack[ShellStart.ShellStack.Count - 1];
                                        var StartCommandThread = ShellInstance.ShellCommandThread;
                                        bool CommandThreadValid = true;
                                        if (StartCommandThread.IsAlive)
                                        {
                                            if (ShellInstance.AltCommandThreads.Count > 0)
                                            {
                                                StartCommandThread = ShellInstance.AltCommandThreads[ShellInstance.AltCommandThreads.Count - 1];
                                            }
                                            else
                                            {
                                                DebugWriter.Wdbg(DebugLevel.W, "Cmd exec {0} failed: Alt command threads are not there.");
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
                                else if ((Parsing.TryParsePath(TargetFile)) & (ShellType == ShellType.Shell))
                                {
                                    // If we're in the UESH shell, parse the script file or executable file
                                    if ((Checking.FileExists(TargetFile)) & (!TargetFile.EndsWith(".uesh")))
                                    {
                                        DebugWriter.Wdbg(DebugLevel.I, "Cmd exec {0} succeeded because file is found.", finalCommand);
                                        try
                                        {
                                            // Create a new instance of process
                                            if (Parsing.TryParsePath(TargetFile))
                                            {
                                                cmdArgs = cmdArgs.Replace(TargetFileName, "");
                                                cmdArgs.Trim();
                                                DebugWriter.Wdbg(DebugLevel.I, "Command: {0}, Arguments: {1}", TargetFile, cmdArgs);
                                                var Params = new ProcessExecutor.ExecuteProcessThreadParameters(TargetFile, cmdArgs);
                                                ProcessStartCommandThread.Start(Params);
                                                ProcessStartCommandThread.Wait();
                                                ProcessStartCommandThread.Stop();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            DebugWriter.Wdbg(DebugLevel.E, "Failed to start process: {0}", ex.Message);
                                            TextWriterColor.Write(Translate.DoTranslation("Failed to start \"{0}\": {1}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), finalCommand, ex.Message);
                                            DebugWriter.WStkTrc(ex);
                                        }
                                    }
                                    else if ((Checking.FileExists(TargetFile)) & (TargetFile.EndsWith(".uesh")))
                                    {
                                        try
                                        {
                                            DebugWriter.Wdbg(DebugLevel.I, "Cmd exec {0} succeeded because it's a UESH script.", finalCommand);
                                            Scripting.UESHParse.Execute(TargetFile, scriptArgs.Join(" "));
                                        }
                                        catch (Exception ex)
                                        {
                                            TextWriterColor.Write(Translate.DoTranslation("Error trying to execute script: {0}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.Message);
                                            DebugWriter.WStkTrc(ex);
                                        }
                                    }
                                    else
                                    {
                                        DebugWriter.Wdbg(DebugLevel.W, "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", finalCommand, indexCmd);
                                        TextWriterColor.Write(Translate.DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), finalCommand);
                                    }
                                }
                                else
                                {
                                    DebugWriter.Wdbg(DebugLevel.W, "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", finalCommand, indexCmd);
                                    TextWriterColor.Write(Translate.DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), finalCommand);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            DebugWriter.WStkTrc(ex);
                            TextWriterColor.Write(Translate.DoTranslation("Error trying to execute command.") + Kernel.Kernel.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.GetType().FullName, ex.Message);
                        }
                    }
                }
            }
            ConsoleExtensions.SetTitle(Kernel.Kernel.ConsoleTitle);

            // Restore console output to its original state if any
            if (Kernel.Kernel.DefConsoleOut is not null)
            {
                Console.SetOut(Kernel.Kernel.DefConsoleOut);
                OutputTextWriter?.Close();
            }
        }

    }
}
