﻿//
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
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Platform;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;

namespace KS.Files.Print
{
    public static class FileInfoPrinter
    {

        /// <summary>
        /// Prints the file information to the console
        /// </summary>
        public static void PrintFileInfo(FileSystemInfo FileInfo)
        {
            PrintFileInfo(FileInfo, Listing.ShowFileDetailsList);
        }

        /// <summary>
        /// Prints the file information to the console
        /// </summary>
        public static void PrintFileInfo(FileSystemInfo FileInfo, bool ShowFileDetails)
        {
            if (Checking.FileExists(FileInfo.FullName))
            {
                if (FileInfo.Attributes == FileAttributes.Hidden & Flags.HiddenFiles | !FileInfo.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    if (PlatformDetector.IsOnWindows() & (!FileInfo.Name.StartsWith(".") | FileInfo.Name.StartsWith(".") & Flags.HiddenFiles) | PlatformDetector.IsOnUnix())
                    {
                        if (FileInfo.Name.EndsWith(".uesh"))
                        {
                            TextWriters.Write("- " + FileInfo.Name, false, KernelColorTools.ColTypes.Stage);
                            if (ShowFileDetails)
                                TextWriters.Write(": ", false, KernelColorTools.ColTypes.Stage);
                        }
                        else
                        {
                            TextWriters.Write("- " + FileInfo.Name, false, KernelColorTools.ColTypes.ListEntry);
                            if (ShowFileDetails)
                                TextWriters.Write(": ", false, KernelColorTools.ColTypes.ListEntry);
                        }
                        if (ShowFileDetails)
                        {
                            TextWriters.Write(Translate.DoTranslation("{0}, Created in {1} {2}, Modified in {3} {4}"), false, KernelColorTools.ColTypes.ListValue, ((FileInfo)FileInfo).Length.FileSizeToString(), FileInfo.CreationTime.ToShortDateString(), FileInfo.CreationTime.ToShortTimeString(), FileInfo.LastWriteTime.ToShortDateString(), FileInfo.LastWriteTime.ToShortTimeString());
                        }
                        TextWriters.Write("", KernelColorTools.ColTypes.Neutral);
                    }
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("File {0} not found"), true, KernelColorTools.ColTypes.Error, FileInfo.FullName);
                DebugWriter.Wdbg(DebugLevel.I, "IO.FileExists = {0}", Checking.FileExists(FileInfo.FullName));
            }
        }

    }
}