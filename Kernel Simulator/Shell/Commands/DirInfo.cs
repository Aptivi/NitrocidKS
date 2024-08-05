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

using System.IO;
using FluentFTP.Helpers;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Querying;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;
using KS.TimeDate;
using Terminaux.Writer.FancyWriters;

namespace KS.Shell.Commands
{
    class DirInfoCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            foreach (string Dir in ListArgs)
            {
                string DirectoryPath = Filesystem.NeutralizePath(Dir);
                DebugWriter.Wdbg(DebugLevel.I, "Neutralized directory path: {0} ({1})", DirectoryPath, Checking.FolderExists(DirectoryPath));
                SeparatorWriterColor.WriteSeparator(Dir, true);
                if (Checking.FolderExists(DirectoryPath))
                {
                    var DirInfo = new DirectoryInfo(DirectoryPath);
                    TextWriters.Write(Translate.DoTranslation("Name: {0}"), true, KernelColorTools.ColTypes.Neutral, DirInfo.Name);
                    TextWriters.Write(Translate.DoTranslation("Full name: {0}"), true, KernelColorTools.ColTypes.Neutral, Filesystem.NeutralizePath(DirInfo.FullName));
                    TextWriters.Write(Translate.DoTranslation("Size: {0}"), true, KernelColorTools.ColTypes.Neutral, SizeGetter.GetAllSizesInFolder(DirInfo).FileSizeToString());
                    TextWriters.Write(Translate.DoTranslation("Creation time: {0}"), true, KernelColorTools.ColTypes.Neutral, TimeDateRenderers.Render(DirInfo.CreationTime));
                    TextWriters.Write(Translate.DoTranslation("Last access time: {0}"), true, KernelColorTools.ColTypes.Neutral, TimeDateRenderers.Render(DirInfo.LastAccessTime));
                    TextWriters.Write(Translate.DoTranslation("Last write time: {0}"), true, KernelColorTools.ColTypes.Neutral, TimeDateRenderers.Render(DirInfo.LastWriteTime));
                    TextWriters.Write(Translate.DoTranslation("Attributes: {0}"), true, KernelColorTools.ColTypes.Neutral, DirInfo.Attributes);
                    TextWriters.Write(Translate.DoTranslation("Parent directory: {0}"), true, KernelColorTools.ColTypes.Neutral, Filesystem.NeutralizePath(DirInfo.Parent.FullName));
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Can't get information about nonexistent directory."), true, KernelColorTools.ColTypes.Error);
                }
            }
        }

    }
}
