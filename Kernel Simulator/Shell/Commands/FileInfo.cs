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
using KS.Files.LineEndings;
using KS.Files.Querying;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;
using KS.TimeDate;
using Terminaux.Writer.FancyWriters;

namespace KS.Shell.Commands
{
    class FileInfoCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            foreach (string FileName in ListArgs)
            {
                string FilePath = Filesystem.NeutralizePath(FileName);
                DebugWriter.Wdbg(DebugLevel.I, "Neutralized file path: {0} ({1})", FilePath, Checking.FileExists(FilePath));
                SeparatorWriterColor.WriteSeparator(FileName, true);
                if (Checking.FileExists(FilePath))
                {
                    var FileInfo = new FileInfo(FilePath);
                    var Style = LineEndingsTools.GetLineEndingFromFile(FilePath);
                    TextWriters.Write(Translate.DoTranslation("Name: {0}"), true, KernelColorTools.ColTypes.Neutral, FileInfo.Name);
                    TextWriters.Write(Translate.DoTranslation("Full name: {0}"), true, KernelColorTools.ColTypes.Neutral, Filesystem.NeutralizePath(FileInfo.FullName));
                    TextWriters.Write(Translate.DoTranslation("File size: {0}"), true, KernelColorTools.ColTypes.Neutral, FileInfo.Length.FileSizeToString());
                    TextWriters.Write(Translate.DoTranslation("Creation time: {0}"), true, KernelColorTools.ColTypes.Neutral, TimeDateRenderers.Render(FileInfo.CreationTime));
                    TextWriters.Write(Translate.DoTranslation("Last access time: {0}"), true, KernelColorTools.ColTypes.Neutral, TimeDateRenderers.Render(FileInfo.LastAccessTime));
                    TextWriters.Write(Translate.DoTranslation("Last write time: {0}"), true, KernelColorTools.ColTypes.Neutral, TimeDateRenderers.Render(FileInfo.LastWriteTime));
                    TextWriters.Write(Translate.DoTranslation("Attributes: {0}"), true, KernelColorTools.ColTypes.Neutral, FileInfo.Attributes);
                    TextWriters.Write(Translate.DoTranslation("Where to find: {0}"), true, KernelColorTools.ColTypes.Neutral, Filesystem.NeutralizePath(FileInfo.DirectoryName));
                    TextWriters.Write(Translate.DoTranslation("Newline style:") + " {0}", true, KernelColorTools.ColTypes.Neutral, Style.ToString());
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Can't get information about nonexistent file."), true, KernelColorTools.ColTypes.Error);
                }
            }
        }

    }
}
