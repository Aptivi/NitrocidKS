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

using KS.Files.Operations.Querying;
using KS.Kernel.Exceptions;
using KS.Languages;
using System.IO;

namespace KS.Files.Instances
{
    /// <summary>
    /// Proxy class for <see cref="FileSystemInfo"/>
    /// </summary>
    public class FileSystemEntry
    {
        private readonly string filePath;
        private readonly string filePathUnneutralized;

        /// <summary>
        /// Does this file or directory entry exist?
        /// </summary>
        public bool Exists =>
            Checking.Exists(filePath);

        /// <summary>
        /// Gets the file entry type
        /// </summary>
        public FileSystemEntryType Type =>
            Checking.FolderExists(filePath) ? FileSystemEntryType.Directory :
            Checking.FileExists(filePath) ? FileSystemEntryType.File :
            FileSystemEntryType.Nonexistent;

        /// <summary>
        /// Gets the original file path that was passed to the constructor
        /// </summary>
        public string OriginalFilePath =>
            filePathUnneutralized;

        /// <summary>
        /// Gets the actual file path processed by <see cref="Filesystem.NeutralizePath(string, bool)"/>
        /// </summary>
        public string FilePath =>
            filePath;

        /// <summary>
        /// Gets the base entry from the unprocessed file path
        /// </summary>
        public FileSystemInfo BaseEntryUnprocessed =>
            Type == FileSystemEntryType.File ? new FileInfo(OriginalFilePath) :
            Type == FileSystemEntryType.Directory ? new DirectoryInfo(OriginalFilePath) :
            throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Can't get base entry out of non-existent file or directory.") + $" {OriginalFilePath}");

        /// <summary>
        /// Gets the base entry from the processed file path
        /// </summary>
        public FileSystemInfo BaseEntry =>
            Type == FileSystemEntryType.File ? new FileInfo(FilePath) :
            Type == FileSystemEntryType.Directory ? new DirectoryInfo(FilePath) :
            throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Can't get base entry out of non-existent file or directory.") + $" {FilePath}");

        /// <summary>
        /// Makes a new instance of this class
        /// </summary>
        /// <param name="filePath">File path (will be neutralized by <see cref="Filesystem.NeutralizePath(string, bool)"/>)</param>
        public FileSystemEntry(string filePath)
        {
            this.filePath = Filesystem.NeutralizePath(filePath);
            filePathUnneutralized = filePath;
        }
    }
}