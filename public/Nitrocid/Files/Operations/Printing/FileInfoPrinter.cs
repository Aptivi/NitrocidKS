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

using KS.Drivers;
using KS.Files.Folders;
using KS.Files.Instances;

namespace KS.Files.Operations.Printing
{
    /// <summary>
    /// File information printing module
    /// </summary>
    public static class FileInfoPrinter
    {

        /// <summary>
        /// Prints the file information to the console
        /// </summary>
        public static void PrintFileInfo(FileSystemEntry FileInfo) =>
            DriverHandler.CurrentFilesystemDriverLocal.PrintFileInfo(FileInfo, Listing.ShowFileDetailsList);

        /// <summary>
        /// Prints the file information to the console
        /// </summary>
        public static void PrintFileInfo(FileSystemEntry FileInfo, bool ShowFileDetails) =>
            DriverHandler.CurrentFilesystemDriverLocal.PrintFileInfo(FileInfo, ShowFileDetails);

    }
}