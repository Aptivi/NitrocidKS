﻿
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Drivers;
using KS.Files;
using KS.Files.Folders;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Probers.Regexp;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using System.Collections.Generic;
using System.Linq;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Finds a file in the specified directory or in the current directory using regular expressions
    /// </summary>
    /// <remarks>
    /// If you are looking for a file and you can't remember where, using this command will help you find it.
    /// </remarks>
    class FindRegCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string RegexToMatch = ListArgsOnly[0];
            string DirectoryToSearch = CurrentDirectory.CurrentDir;
            bool isRecursive = ListSwitchesOnly.Contains("-recursive");
            if (ListArgsOnly.Length > 1)
                DirectoryToSearch = Filesystem.NeutralizePath(ListArgsOnly[1]);

            // Print the results if found
            var AllFileEntries = Listing.GetFilesystemEntriesRegex(DirectoryToSearch, RegexToMatch, isRecursive);
            ListWriterColor.WriteList(AllFileEntries, true);
        }

    }
}
