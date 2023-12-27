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
using System.Data;
using System.Linq;


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

using KS.Files.Folders;
using KS.Misc.Text;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.ShellBase.Commands
{
    public static class CommandAutoComplete
    {

        public static string[] GetSuggestions(string text, int index, char[] delims, ShellType type)
        {
            var ShellCommands = GetCommand.GetCommands(type);
            if (ShellStart.ShellStack.Count > 0)
            {
                if (string.IsNullOrEmpty(text))
                {
                    return [.. ShellCommands.Keys];
                }
                else if (text.Contains(" "))
                {
                    // We're providing completion for argument.
                    string CommandName = Convert.ToString(text.SplitEncloseDoubleQuotes()[Convert.ToInt32(" ")][0]);
                    string[] FileFolderList = Listing.CreateList(CurrentDirectory.CurrentDir, true).Select(x => x.Name).ToArray();
                    if (ShellCommands.ContainsKey(CommandName))
                    {
                        // We have the command. Check its entry for argument info
                        var CommandArgumentInfo = ShellCommands[CommandName].CommandArgumentInfo;
                        if (CommandArgumentInfo is not null)
                        {
                            // There are arguments! Now, check to see if it has the accessible auto completer
                            var AutoCompleter = CommandArgumentInfo.AutoCompleter;
                            if (AutoCompleter is not null)
                            {
                                // We have the delegate! Invoke it.
                                return AutoCompleter.Invoke();
                            }
                            else
                            {
                                // No delegate. Return file list
                                return FileFolderList;
                            }
                        }
                        else
                        {
                            // No arguments. Return file list
                            return FileFolderList;
                        }
                    }
                    return FileFolderList;
                }
                else
                {
                    // We're providing completion for command.
                    return ShellCommands.Keys.Where(x => x.StartsWith(text)).ToArray();
                }
            }
            else
            {
                return null;
            }
        }

    }
}
