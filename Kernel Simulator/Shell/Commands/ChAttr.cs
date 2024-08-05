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
using System.IO;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Attributes;

using KS.Files.Querying;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class ChAttrCommand : CommandExecutor, ICommand
    {

        // Warning: Don't use ListSwitchesOnly to replace ListArgs(1); the removal signs of ChAttr are treated as switches and will cause unexpected behavior if changed.
        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string NeutralizedFilePath = Filesystem.NeutralizePath(ListArgs[0]);
            if (Checking.FileExists(NeutralizedFilePath))
            {
                if (ListArgs[1].EndsWith("Normal") | ListArgs[1].EndsWith("ReadOnly") | ListArgs[1].EndsWith("Hidden") | ListArgs[1].EndsWith("Archive"))
                {
                    if (ListArgs[1].StartsWith("+"))
                    {
                        FileAttributes Attrib = (FileAttributes)Convert.ToInt32(Enum.Parse(typeof(FileAttributes), ListArgs[1].Remove(0, 1)));
                        if (AttributeManager.TryAddAttributeToFile(NeutralizedFilePath, Attrib))
                        {
                            TextWriters.Write(Translate.DoTranslation("Attribute has been added successfully."), true, KernelColorTools.ColTypes.Neutral, ListArgs[1]);
                        }
                        else
                        {
                            TextWriters.Write(Translate.DoTranslation("Failed to add attribute."), true, KernelColorTools.ColTypes.Neutral, ListArgs[1]);
                        }
                    }
                    else if (ListArgs[1].StartsWith("-"))
                    {
                        FileAttributes Attrib = (FileAttributes)Convert.ToInt32(Enum.Parse(typeof(FileAttributes), ListArgs[1].Remove(0, 1)));
                        if (AttributeManager.TryRemoveAttributeFromFile(NeutralizedFilePath, Attrib))
                        {
                            TextWriters.Write(Translate.DoTranslation("Attribute has been removed successfully."), true, KernelColorTools.ColTypes.Neutral, ListArgs[1]);
                        }
                        else
                        {
                            TextWriters.Write(Translate.DoTranslation("Failed to remove attribute."), true, KernelColorTools.ColTypes.Neutral, ListArgs[1]);
                        }
                    }
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Attribute \"{0}\" is invalid."), true, KernelColorTools.ColTypes.Error, ListArgs[1]);
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("File not found."), true, KernelColorTools.ColTypes.Error);
            }
        }

        public override void HelpHelper()
        {
            TextWriters.Write(Translate.DoTranslation("where <attributes> is one of the following:"), true, KernelColorTools.ColTypes.Neutral);
            TextWriters.Write("- Normal: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("The file is a normal file"), true, KernelColorTools.ColTypes.ListValue);                   // Normal   = 128
            TextWriters.Write("- ReadOnly: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("The file is a read-only file"), true, KernelColorTools.ColTypes.ListValue);                // ReadOnly = 1
            TextWriters.Write("- Hidden: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("The file is a hidden file"), true, KernelColorTools.ColTypes.ListValue);                   // Hidden   = 2
            TextWriters.Write("- Archive: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("The file is an archive. Used for backups."), true, KernelColorTools.ColTypes.ListValue);   // Archive  = 32
        }

    }
}
