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
using System.Collections;
using System.Collections.Generic;

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
using System.Text;
using KS.Files.Folders;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.DebugWriters;
using KS.TimeDate;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;

namespace KS.Files.Interactive
{
    public class FileManagerCli : BaseInteractiveTui, IInteractiveTui
    {
        internal string firstPanePath = Paths.HomePath;
        internal string secondPanePath = Paths.HomePath;
        internal bool refreshFirstPaneListing = true;
        internal bool refreshSecondPaneListing = true;
        private List<FileSystemInfo> firstPaneListing = [];
        private List<FileSystemInfo> secondPaneListing = [];

        /// <summary>
        /// File manager bindings
        /// </summary>
        public override List<InteractiveTuiBinding> Bindings { get; set; } =
        [
            new("Open", ConsoleKey.Enter, default, (info, _) => Open((FileSystemInfo)info)),
            new("Copy", ConsoleKey.F1, default, (info, _) => CopyFileOrDir((FileSystemInfo)info)),
            new("Move", ConsoleKey.F2, default, (info, _) => MoveFileOrDir((FileSystemInfo)info)),
            new("Delete", ConsoleKey.F3, default, (info, _) => RemoveFileOrDir((FileSystemInfo)info)),
            new("Up", ConsoleKey.F4, default, (_, _) => GoUp()),
            new("Info", ConsoleKey.F5, default, (info, _) => PrintFileSystemInfo((FileSystemInfo)info)),
            new("Go To", ConsoleKey.F6, default, (_, _) => GoTo()),
            new("Copy To", ConsoleKey.F1, ConsoleModifiers.Shift, (info, _) => CopyTo((FileSystemInfo)info)),
            new("Move To", ConsoleKey.F2, ConsoleModifiers.Shift, (info, _) => MoveTo((FileSystemInfo)info)),
            new("Rename", ConsoleKey.F9, default, (info, _) => Rename((FileSystemInfo)info)),
            new("New Folder", ConsoleKey.F10, default, (_, _) => MakeDir())
        ];

        /// <summary>
        /// Always true in the file manager as we want it to behave like Total Commander
        /// </summary>
        public override bool SecondPaneInteractable => true;

        /// <inheritdoc/>
        public override IEnumerable PrimaryDataSource
        {
            get
            {
                try
                {
                    if (refreshFirstPaneListing)
                    {
                        refreshFirstPaneListing = false;
                        firstPaneListing = Listing.CreateList(firstPanePath, true);
                    }
                    return firstPaneListing;
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Failed to get current directory list for the first pane [{0}]: {1}", firstPanePath, ex.Message);
                    DebugWriter.WStkTrc(ex);
                    return new List<FileSystemInfo>();
                }
            }
        }

        /// <inheritdoc/>
        public override IEnumerable SecondaryDataSource
        {
            get
            {
                try
                {
                    if (refreshSecondPaneListing)
                    {
                        refreshSecondPaneListing = false;
                        secondPaneListing = Listing.CreateList(secondPanePath, true);
                    }
                    return secondPaneListing;
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Failed to get current directory list for the second pane [{0}]: {1}", secondPanePath, ex.Message);
                    DebugWriter.WStkTrc(ex);
                    return new List<FileSystemInfo>();
                }
            }
        }

        /// <inheritdoc/>
        public override bool AcceptsEmptyData => true;

        /// <inheritdoc/>
        public override void RenderStatus(object item)
        {
            FileSystemInfo FileInfoCurrentPane = (FileSystemInfo)item;

            // Check to see if we're given the file system info
            if (FileInfoCurrentPane is null)
            {
                InteractiveTuiStatus.Status = Translate.DoTranslation("No info.");
                return;
            }

            // Now, populate the info to the status
            try
            {
                bool infoIsDirectory = Checking.FolderExists(FileInfoCurrentPane.FullName);
                InteractiveTuiStatus.Status = $"[{(infoIsDirectory ? "/" : "*")}] {FileInfoCurrentPane.Name}";
            }
            catch (Exception ex)
            {
                InteractiveTuiStatus.Status = ex.Message;
            }
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item)
        {
            try
            {
                FileSystemInfo file = (FileSystemInfo)item;
                bool isDirectory = Checking.FolderExists(file.FullName);
                return $" [{(isDirectory ? "/" : "*")}] {file.Name}";
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed to get entry: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
                return "";
            }
        }

        private static void Open(FileSystemInfo currentFileSystemInfo)
        {
            try
            {
                // Don't do anything if we haven't been provided anything.
                if (currentFileSystemInfo is null)
                    return;

                // Check for existence
                if (!currentFileSystemInfo.Exists)
                    return;

                // Now that the selected file or folder exists, check the type.
                if (Checking.FolderExists(currentFileSystemInfo.FullName))
                {
                    // We're dealing with a folder. Open it in the selected pane.
                    if (InteractiveTuiStatus.CurrentPane == 2)
                    {
                        ((FileManagerCli)Instance).secondPanePath = Filesystem.NeutralizePath(currentFileSystemInfo.FullName.ToString() + "/");
                        InteractiveTuiStatus.SecondPaneCurrentSelection = 1;
                        ((FileManagerCli)Instance).refreshSecondPaneListing = true;
                    }
                    else
                    {
                        ((FileManagerCli)Instance).firstPanePath = Filesystem.NeutralizePath(currentFileSystemInfo.FullName.ToString() + "/");
                        InteractiveTuiStatus.FirstPaneCurrentSelection = 1;
                        ((FileManagerCli)Instance).refreshFirstPaneListing = true;
                    }
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't open file or folder") + ": {0}\n".FormatString(ex.Message));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Press any key to close this window.").ToString());
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString());
            }
        }

        private static void GoUp()
        {
            if (InteractiveTuiStatus.CurrentPane == 2)
            {
                ((FileManagerCli)Instance).secondPanePath = Filesystem.NeutralizePath(((FileManagerCli)Instance).secondPanePath + "/..");
                InteractiveTuiStatus.SecondPaneCurrentSelection = 1;
                ((FileManagerCli)Instance).refreshSecondPaneListing = true;
            }
            else
            {
                ((FileManagerCli)Instance).firstPanePath = Filesystem.NeutralizePath(((FileManagerCli)Instance).firstPanePath + "/..");
                InteractiveTuiStatus.FirstPaneCurrentSelection = 1;
                ((FileManagerCli)Instance).refreshFirstPaneListing = true;
            }
        }

        private static void PrintFileSystemInfo(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            // Render the final information string
            try
            {
                var finalInfoRendered = new StringBuilder();
                string fullPath = currentFileSystemInfo.FullName;
                if (Checking.FolderExists(fullPath))
                {
                    // The file system info instance points to a folder
                    var DirInfo = new DirectoryInfo(fullPath);
                    finalInfoRendered.AppendLine(Translate.DoTranslation("Name: {0}").FormatString(DirInfo.Name));
                    finalInfoRendered.AppendLine(Translate.DoTranslation("Full name: {0}").FormatString(Filesystem.NeutralizePath(DirInfo.FullName)));
                    finalInfoRendered.AppendLine(Translate.DoTranslation("Size: {0}").FormatString(SizeGetter.GetAllSizesInFolder(DirInfo)));
                    finalInfoRendered.AppendLine(Translate.DoTranslation("Creation time: {0}").FormatString(TimeDateRenderers.Render(DirInfo.CreationTime)));
                    finalInfoRendered.AppendLine(Translate.DoTranslation("Last access time: {0}").FormatString(TimeDateRenderers.Render(DirInfo.LastAccessTime)));
                    finalInfoRendered.AppendLine(Translate.DoTranslation("Last write time: {0}").FormatString(TimeDateRenderers.Render(DirInfo.LastWriteTime)));
                    finalInfoRendered.AppendLine(Translate.DoTranslation("Attributes: {0}").FormatString(DirInfo.Attributes));

                    // The file system info instance points to a file
                    finalInfoRendered.AppendLine(Translate.DoTranslation("Parent directory: {0}").FormatString(Filesystem.NeutralizePath(DirInfo.Parent.FullName)));
                }
                else
                {
                    var fileInfo = new FileInfo(fullPath);
                    finalInfoRendered.AppendLine(Translate.DoTranslation("Name: {0}").FormatString(fileInfo.Name));
                    finalInfoRendered.AppendLine(Translate.DoTranslation("Full name: {0}").FormatString(Filesystem.NeutralizePath(fileInfo.FullName)));
                    finalInfoRendered.AppendLine(Translate.DoTranslation("File size: {0}").FormatString(fileInfo.Length));
                    finalInfoRendered.AppendLine(Translate.DoTranslation("Creation time: {0}").FormatString(TimeDateRenderers.Render(fileInfo.CreationTime)));
                    finalInfoRendered.AppendLine(Translate.DoTranslation("Last access time: {0}").FormatString(TimeDateRenderers.Render(fileInfo.LastAccessTime)));
                    finalInfoRendered.AppendLine(Translate.DoTranslation("Last write time: {0}").FormatString(TimeDateRenderers.Render(fileInfo.LastWriteTime)));
                    finalInfoRendered.AppendLine(Translate.DoTranslation("Attributes: {0}").FormatString(fileInfo.Attributes));
                    finalInfoRendered.AppendLine(Translate.DoTranslation("Where to find: {0}").FormatString(Filesystem.NeutralizePath(fileInfo.DirectoryName)));
                }
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window.").ToString());

                // Now, render the info box
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString());
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't get file system info") + ": {0}".FormatString(ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window.").ToString());
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString());
            }
        }

        private static void CopyFileOrDir(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            try
            {
                string dest = (InteractiveTuiStatus.CurrentPane == 2 ? ((FileManagerCli)Instance).firstPanePath : ((FileManagerCli)Instance).secondPanePath).ToString() + "/";
                DebugWriter.Wdbg(DebugLevel.I, $"Destination is {dest}");
                Copying.CopyFileOrDir(currentFileSystemInfo.FullName, dest);
                if (InteractiveTuiStatus.CurrentPane == 2)
                {
                    ((FileManagerCli)Instance).refreshFirstPaneListing = true;
                }
                else
                {
                    ((FileManagerCli)Instance).refreshSecondPaneListing = true;
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't copy file or directory") + ": {0}".FormatString(ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window.").ToString());
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString());
            }
        }

        private static void MoveFileOrDir(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            try
            {
                string dest = (InteractiveTuiStatus.CurrentPane == 2 ? ((FileManagerCli)Instance).firstPanePath : ((FileManagerCli)Instance).secondPanePath).ToString() + "/";
                DebugWriter.Wdbg(DebugLevel.I, $"Destination is {dest}");
                Moving.MoveFileOrDir(currentFileSystemInfo.FullName, dest);
                ((FileManagerCli)Instance).refreshSecondPaneListing = true;
                ((FileManagerCli)Instance).refreshFirstPaneListing = true;
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + ": {0}".FormatString(ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window.").ToString());
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString());
            }
        }

        private static void RemoveFileOrDir(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            try
            {
                if (Checking.FolderExists(currentFileSystemInfo.FullName))
                {
                    Removing.RemoveDirectory(currentFileSystemInfo.FullName);
                }
                else
                {
                    Removing.RemoveFile(currentFileSystemInfo.FullName);
                }
                if (InteractiveTuiStatus.CurrentPane == 2)
                {
                    ((FileManagerCli)Instance).refreshSecondPaneListing = true;
                }
                else
                {
                    ((FileManagerCli)Instance).refreshFirstPaneListing = true;
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't remove file or directory") + ": {0}".FormatString(ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window.").ToString());
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString());
            }
        }

        private static void GoTo()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a path or a full path to a local folder."));
            path = Filesystem.NeutralizePath(path, ((FileManagerCli)Instance).firstPanePath);
            if (Checking.FolderExists(path))
            {
                InteractiveTuiStatus.FirstPaneCurrentSelection = 1;
                ((FileManagerCli)Instance).firstPanePath = path;
                ((FileManagerCli)Instance).refreshFirstPaneListing = true;
            }
            else
            {
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Folder doesn't exist. Make sure that you've written the correct path."));
            }
        }

        private static void CopyTo(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            try
            {
                string path = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a path or a full path to a destination folder to copy the selected file to."));
                path = Filesystem.NeutralizePath(path, InteractiveTuiStatus.CurrentPane == 2 ? ((FileManagerCli)Instance).secondPanePath : ((FileManagerCli)Instance).firstPanePath).ToString() + "/";
                DebugWriter.Wdbg(DebugLevel.I, $"Destination is {path}");
                if (Checking.FolderExists(path))
                {
                    if (Parsing.TryParsePath(path))
                    {
                        Copying.CopyFileOrDir(currentFileSystemInfo.FullName, path);
                        if (InteractiveTuiStatus.CurrentPane == 2)
                        {
                            ((FileManagerCli)Instance).refreshFirstPaneListing = true;
                        }
                        else
                        {
                            ((FileManagerCli)Instance).refreshSecondPaneListing = true;
                        }
                    }
                    else
                    {
                        InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Make sure that you've written the correct path."));
                    }
                }
                else
                {
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path."));
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't copy file or directory") + ": {0}".FormatString(ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window.").ToString());
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString());
            }
        }

        private static void MoveTo(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            try
            {
                string path = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a path or a full path to a destination folder to move the selected file to."));
                path = Filesystem.NeutralizePath(path, InteractiveTuiStatus.CurrentPane == 2 ? ((FileManagerCli)Instance).secondPanePath : ((FileManagerCli)Instance).firstPanePath).ToString() + "/";
                DebugWriter.Wdbg(DebugLevel.I, $"Destination is {path}");
                if (Checking.FolderExists(path))
                {
                    if (Parsing.TryParsePath(path))
                    {
                        Moving.MoveFileOrDir(currentFileSystemInfo.FullName, path);
                        ((FileManagerCli)Instance).refreshSecondPaneListing = true;
                        ((FileManagerCli)Instance).refreshFirstPaneListing = true;
                    }
                    else
                    {
                        InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Make sure that you've written the correct path."));
                    }
                }
                else
                {
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path."));
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + ": {0}".FormatString(ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window.").ToString());
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString());
            }
        }

        private static void Rename(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            try
            {
                string filename = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a new file name."));
                DebugWriter.Wdbg(DebugLevel.I, $"New filename is {filename}");
                if (!Checking.FileExists(filename))
                {
                    if (Parsing.TryParseFileName(filename))
                    {
                        Moving.MoveFileOrDir(currentFileSystemInfo.FullName, Path.GetDirectoryName(currentFileSystemInfo.FullName).ToString() + $"/{filename}");
                        if (InteractiveTuiStatus.CurrentPane == 2)
                        {
                            ((FileManagerCli)Instance).refreshSecondPaneListing = true;
                        }
                        else
                        {
                            ((FileManagerCli)Instance).refreshFirstPaneListing = true;
                        }
                    }
                    else
                    {
                        InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Make sure that you've written the correct file name."));
                    }
                }
                else
                {
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("File already exists. The name shouldn't be occupied by another file."));
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + ": {0}".FormatString(ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window.").ToString());
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString());
            }
        }

        private static void MakeDir()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a new directory name."));
            path = Filesystem.NeutralizePath(path, InteractiveTuiStatus.CurrentPane == 2 ? ((FileManagerCli)Instance).secondPanePath : ((FileManagerCli)Instance).firstPanePath);
            if (!Checking.FolderExists(path))
            {
                Making.TryMakeDirectory(path);
                if (InteractiveTuiStatus.CurrentPane == 2)
                {
                    ((FileManagerCli)Instance).refreshSecondPaneListing = true;
                }
                else
                {
                    ((FileManagerCli)Instance).refreshFirstPaneListing = true;
                }
            }
            else
            {
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Folder already exists. The name shouldn't be occupied by another folder."));
            }
        }
    }
}
