using System;
using System.Collections.Generic;

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

using System.IO;
using KS.Misc.Editors.TextEdit.Commands;
using KS.Misc.Threading;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Misc.Editors.TextEdit
{
	public static class TextEditShellCommon
	{

		// Variables
		public static readonly Dictionary<string, CommandInfo> TextEdit_Commands = new() { { "addline", new CommandInfo("addline", ShellType.TextShell, "Adds a new line with text at the end of the file", new CommandArgumentInfo(["<text>"], true, 1), new TextEdit_AddLineCommand()) }, { "addlines", new CommandInfo("addlines", ShellType.TextShell, "Adds the new lines at the end of the file", new CommandArgumentInfo([], false, 0), new TextEdit_AddLinesCommand()) }, { "clear", new CommandInfo("clear", ShellType.TextShell, "Clears the text file", new CommandArgumentInfo([], false, 0), new TextEdit_ClearCommand()) }, { "delcharnum", new CommandInfo("delcharnum", ShellType.TextShell, "Deletes a character from character number in specified line", new CommandArgumentInfo(["<charnumber> <linenumber>"], true, 2), new TextEdit_DelCharNumCommand()) }, { "delline", new CommandInfo("delline", ShellType.TextShell, "Removes the specified line number", new CommandArgumentInfo(["<linenumber> [linenumber2]"], true, 1), new TextEdit_DelLineCommand()) }, { "delword", new CommandInfo("delword", ShellType.TextShell, "Deletes a word or phrase from line number", new CommandArgumentInfo(["\"<word/phrase>\" <linenumber> [linenumber2]"], true, 2), new TextEdit_DelWordCommand()) }, { "editline", new CommandInfo("editline", ShellType.TextShell, "Edits the specified line", new CommandArgumentInfo(["<linenumber>"], true, 1), new TextEdit_EditLineCommand()) }, { "exitnosave", new CommandInfo("exitnosave", ShellType.TextShell, "Exits the text editor", new CommandArgumentInfo([], false, 0), new TextEdit_ExitNoSaveCommand()) }, { "help", new CommandInfo("help", ShellType.TextShell, "Lists available commands", new CommandArgumentInfo(["[command]"], false, 0), new TextEdit_HelpCommand()) }, { "print", new CommandInfo("print", ShellType.TextShell, "Prints the contents of the file with line numbers to the console", new CommandArgumentInfo(["[linenumber] [linenumber2]"], false, 0), new TextEdit_PrintCommand()) }, { "querychar", new CommandInfo("querychar", ShellType.TextShell, "Queries a character in a specified line or all lines", new CommandArgumentInfo(["<char> <linenumber/all> [linenumber2]"], true, 2), new TextEdit_QueryCharCommand()) }, { "queryword", new CommandInfo("queryword", ShellType.TextShell, "Queries a word in a specified line or all lines", new CommandArgumentInfo(["\"<word/phrase>\" <linenumber/all> [linenumber2]"], true, 2), new TextEdit_QueryWordCommand()) }, { "querywordregex", new CommandInfo("querywordregex", ShellType.TextShell, "Queries a word in a specified line or all lines using regular expressions", new CommandArgumentInfo(["\"<regex>\" <linenumber/all> [linenumber2]"], true, 2), new TextEdit_QueryWordRegexCommand()) }, { "replace", new CommandInfo("replace", ShellType.TextShell, "Replaces a word or phrase with another one", new CommandArgumentInfo(["\"<word/phrase>\" \"<word/phrase>\""], true, 2), new TextEdit_ReplaceCommand()) }, { "replaceinline", new CommandInfo("replaceinline", ShellType.TextShell, "Replaces a word or phrase with another one in a line", new CommandArgumentInfo(["\"<word/phrase>\" \"<word/phrase>\" <linenumber> [linenumber2]"], true, 3), new TextEdit_ReplaceInlineCommand()) }, { "replaceregex", new CommandInfo("replaceregex", ShellType.TextShell, "Replaces a word or phrase with another one using regular expressions", new CommandArgumentInfo(["\"<regex>\" \"<word/phrase>\""], true, 2), new TextEdit_ReplaceRegexCommand()) }, { "replaceinlineregex", new CommandInfo("replaceinlineregex", ShellType.TextShell, "Replaces a word or phrase with another one in a line using regular expressions", new CommandArgumentInfo(["\"<regex>\" \"<word/phrase>\" <linenumber> [linenumber2]"], true, 3), new TextEdit_ReplaceInlineRegexCommand()) }, { "save", new CommandInfo("save", ShellType.TextShell, "Saves the file", new CommandArgumentInfo([], false, 0), new TextEdit_SaveCommand()) } };
		public static List<string> TextEdit_FileLines;
		public static FileStream TextEdit_FileStream;
		public static KernelThread TextEdit_AutoSave = new("Text Edit Autosave Thread", false, TextEditTools.TextEdit_HandleAutoSaveTextFile);
		public static bool TextEdit_AutoSaveFlag = true;
		public static int TextEdit_AutoSaveInterval = 60;
		internal static readonly Dictionary<string, CommandInfo> TextEdit_ModCommands = [];
		internal static List<string> TextEdit_FileLinesOrig;

	}
}