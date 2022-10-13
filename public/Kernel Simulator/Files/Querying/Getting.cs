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

using System.IO;

namespace KS.Files.Querying
{
    /// <summary>
    /// Module related to getting information
    /// </summary>
    public static class Getting
    {
        /// <summary>
        /// Gets the file name with the file number suffix applied
        /// </summary>
        /// <param name="path">Path to the directory that the generated numbered file name will situate</param>
        /// <param name="fileName">The file name with an extension</param>
        /// <returns>Numbered file name with the file number suffix applied in this format: [filename]-[number].[ext]</returns>
        public static string GetNumberedFileName(string path, string fileName)
        {
            Filesystem.ThrowOnInvalidPath(path);
            Filesystem.ThrowOnInvalidPath(fileName);
            path = Filesystem.NeutralizePath(path);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string fileNameExtension = Path.GetExtension(fileName);
            int fileNumber = 0;

            // Check if the target file exists
            bool fileNameValid = false;
            do
            {
                string fileNameGenerated = Filesystem.NeutralizePath($"{fileNameWithoutExtension}-{fileNumber}{fileNameExtension}", path);
                if (!Checking.FileExists(fileNameGenerated))
                    return fileNameGenerated;
                fileNumber++;
            }
            while (!fileNameValid);
            return $"{fileNameWithoutExtension}-{fileNumber}{fileNameExtension}";
        }
    }
}
