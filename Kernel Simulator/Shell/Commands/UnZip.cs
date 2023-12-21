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
using System.IO.Compression;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Folders;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class UnZipCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if ((ListArgs?.Length) is { } arg1 && arg1 == 1)
            {
                string ZipArchiveName = Filesystem.NeutralizePath(ListArgs[0]);
                ZipFile.ExtractToDirectory(ZipArchiveName, CurrentDirectory.CurrentDir);
            }
            else if ((ListArgs?.Length) is { } arg2 && arg2 > 1)
            {
                string ZipArchiveName = Filesystem.NeutralizePath(ListArgs[0]);
                string Destination = !(ListArgs[1] == "-createdir") ? Filesystem.NeutralizePath(ListArgs[1]) : "";
                if (ListArgs?.Contains("-createdir") == true)
                {
                    Destination = $"{(!(ListArgs[1] == "-createdir") ? Filesystem.NeutralizePath(ListArgs[1]) : "")}/{(!(ListArgs[1] == "-createdir") ? Path.GetFileNameWithoutExtension(ZipArchiveName) : Filesystem.NeutralizePath(Path.GetFileNameWithoutExtension(ZipArchiveName)))}";
                    if (Convert.ToString(Destination[0]) == "/")
                        Destination = Destination.Substring(1);
                }
                ZipFile.ExtractToDirectory(ZipArchiveName, Destination);
            }
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
            TextWriterColor.Write("  -createdir: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Creates a directory that contains the contents of the ZIP file"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
        }

    }
}