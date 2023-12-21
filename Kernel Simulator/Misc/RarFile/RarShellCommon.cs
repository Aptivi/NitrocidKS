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
using KS.Misc.RarFile.Commands;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

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

using SharpCompress.Archives.Rar;

namespace KS.Misc.RarFile
{
    static class RarShellCommon
    {

        // Variables
        public static readonly Dictionary<string, CommandInfo> RarShell_Commands = new() { { "cdir", new CommandInfo("cdir", ShellType.RARShell, "Gets current local directory", new CommandArgumentInfo([], false, 0), new RarShell_CDirCommand()) }, { "chdir", new CommandInfo("chdir", ShellType.RARShell, "Changes directory", new CommandArgumentInfo(["<directory>"], true, 1), new RarShell_ChDirCommand()) }, { "chadir", new CommandInfo("chadir", ShellType.RARShell, "Changes archive directory", new CommandArgumentInfo(["<archivedirectory>"], true, 1), new RarShell_ChADirCommand()) }, { "get", new CommandInfo("get", ShellType.RARShell, "Extracts a file to a specified directory or a current directory", new CommandArgumentInfo(["<entry> [where] [-absolute]"], true, 1), new RarShell_GetCommand(), false, false, false, false, false) }, { "help", new CommandInfo("help", ShellType.RARShell, "Lists available commands", new CommandArgumentInfo(["[command]"], false, 0), new RarShell_HelpCommand()) }, { "list", new CommandInfo("list", ShellType.RARShell, "Lists all files inside the archive", new CommandArgumentInfo(["[directory]"], false, 0), new RarShell_ListCommand()) } };
        internal static readonly Dictionary<string, CommandInfo> RarShell_ModCommands = [];
        public static FileStream RarShell_FileStream;
        public static RarArchive RarShell_RarArchive;
        public static string RarShell_CurrentDirectory;
        public static string RarShell_CurrentArchiveDirectory;

    }
}