﻿
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

using KS.Drivers;

namespace KS.Files.Querying
{
    /// <summary>
    /// File parsing module
    /// </summary>
    public static class Parsing
    {

        /// <summary>
        /// Gets all the invalid path characters
        /// </summary>
        public static char[] GetInvalidPathChars() =>
            DriverHandler.CurrentFilesystemDriver.GetInvalidPathChars();

        /// <summary>
        /// Tries to parse the path (For file names and only names, use <see cref="TryParseFileName(string)"/> instead.)
        /// </summary>
        /// <param name="Path">The path to be parsed</param>
        /// <returns>True if successful; false if unsuccessful</returns>
        public static bool TryParsePath(string Path) =>
            DriverHandler.CurrentFilesystemDriver.TryParsePath(Path);

        /// <summary>
        /// Tries to parse the file name (For full paths, use <see cref="TryParsePath(string)"/> instead.)
        /// </summary>
        /// <param name="Name">The file name to be parsed</param>
        /// <returns>True if successful; false if unsuccessful</returns>
        public static bool TryParseFileName(string Name) =>
            DriverHandler.CurrentFilesystemDriver.TryParseFileName(Name);

        /// <summary>
        /// Is the file a binary file?
        /// </summary>
        /// <param name="Path">Path to file</param>
        public static bool IsBinaryFile(string Path) => 
            DriverHandler.CurrentFilesystemDriver.IsBinaryFile(Path);

        /// <summary>
        /// Is the file a JSON file?
        /// </summary>
        /// <param name="Path">Path to file</param>
        public static bool IsJson(string Path) =>
            DriverHandler.CurrentFilesystemDriver.IsJson(Path);

    }
}
