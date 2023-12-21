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

using System.Collections.Generic;
using FluentFTP.Helpers;
using KS.ConsoleBase.Colors;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;

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

namespace KS.Misc.RarFile.Commands
{
    class RarShell_ListCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            List<RarArchiveEntry> Entries;
            if ((ListArgs?.Length) is { } arg1 && arg1 > 0)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Listing entries with {0} as target directory", ListArgs[0]);
                Entries = RarTools.ListRarEntries(ListArgs[0]);
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.I, "Listing entries with current directory as target directory");
                Entries = RarTools.ListRarEntries(RarShellCommon.RarShell_CurrentArchiveDirectory);
            }
            foreach (RarArchiveEntry Entry in Entries)
            {
                TextWriterColor.Write("- {0}: ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), Entry.Key);
                if (!Entry.IsDirectory) // Entry is a file
                {
                    TextWriterColor.Write("{0} ({1})", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), Entry.CompressedSize.FileSizeToString(), Entry.Size.FileSizeToString());
                }
                else
                {
                    TextWriterColor.WritePlain("", true);
                }
            }
        }

    }
}