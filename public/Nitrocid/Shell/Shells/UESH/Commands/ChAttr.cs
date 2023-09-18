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

using System;
using System.IO;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Files.Attributes;
using KS.Files.Querying;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Users.Permissions;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Changes attributes of file
    /// </summary>
    /// <remarks>
    /// You can use this command to change attributes of a file.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Attribute</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>Normal</term>
    /// <description>The file is a normal file</description>
    /// </item>
    /// <item>
    /// <term>ReadOnly</term>
    /// <description>The file is a read-only file</description>
    /// </item>
    /// <item>
    /// <term>Hidden</term>
    /// <description>The file is a hidden file</description>
    /// </item>
    /// <item>
    /// <term>Archive</term>
    /// <description>The file is an archive. Used for backups.</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class ChAttrCommand : BaseCommand, ICommand
    {

        // Warning: Don't use ListSwitchesOnly to replace ListArgsOnly(1); the removal signs of ChAttr are treated as switches and will cause unexpected behavior if changed.
        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            string NeutralizedFilePath = Filesystem.NeutralizePath(ListArgsOnly[0]);
            PermissionsTools.Demand(PermissionTypes.ManageFilesystem);
            if (Checking.FileExists(NeutralizedFilePath))
            {
                if (ListArgsOnly[2] == "Normal" | ListArgsOnly[2] == "ReadOnly" | ListArgsOnly[2] == "Hidden" | ListArgsOnly[2] == "Archive")
                {
                    if (ListArgsOnly[1] == "add")
                    {
                        FileAttributes Attrib = (FileAttributes)Convert.ToInt32(Enum.Parse(typeof(FileAttributes), ListArgsOnly[2]));
                        if (AttributeManager.TryAddAttributeToFile(NeutralizedFilePath, Attrib))
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Attribute has been added successfully.") + " {0}", ListArgsOnly[2]);
                            return 0;
                        }
                        else
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Failed to add attribute.") + " {0}", ListArgsOnly[2]);
                            return 10000 + (int)KernelExceptionType.Filesystem;
                        }
                    }
                    else if (ListArgsOnly[1] == "rem")
                    {
                        FileAttributes Attrib = (FileAttributes)Convert.ToInt32(Enum.Parse(typeof(FileAttributes), ListArgsOnly[2]));
                        if (AttributeManager.TryRemoveAttributeFromFile(NeutralizedFilePath, Attrib))
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Attribute has been removed successfully.") + " {0}", ListArgsOnly[2]);
                            return 0;
                        }
                        else
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Failed to remove attribute.") + " {0}", ListArgsOnly[2]);
                            return 10000 + (int)KernelExceptionType.Filesystem;
                        }
                    }
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Attribute \"{0}\" is invalid."), true, KernelColorType.Error, ListArgsOnly[2]);
                    return 10000 + (int)KernelExceptionType.Filesystem;
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("File not found."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Filesystem;
            }
            return 0;
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("where <attributes> is one of the following:"));
            TextWriterColor.Write("- Normal: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("The file is a normal file"), true, KernelColorType.ListValue);                   // Normal   = 128
            TextWriterColor.Write("- ReadOnly: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("The file is a read-only file"), true, KernelColorType.ListValue);              // ReadOnly = 1
            TextWriterColor.Write("- Hidden: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("The file is a hidden file"), true, KernelColorType.ListValue);                   // Hidden   = 2
            TextWriterColor.Write("- Archive: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("The file is an archive. Used for backups."), true, KernelColorType.ListValue);  // Archive  = 32
        }

    }
}
