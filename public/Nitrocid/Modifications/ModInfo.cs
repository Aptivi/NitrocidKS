﻿//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using System.Diagnostics;
using KS.Kernel.Exceptions;
using KS.Languages;

namespace KS.Modifications
{
    /// <summary>
    /// Mod information
    /// </summary>
    [DebuggerDisplay("Mod name = {ModName}")]
    public class ModInfo
    {

        /// <summary>
        /// The mod name. If no name is specified, or if it only consists of whitespaces (space), the file name is taken.
        /// </summary>
        public string ModName { get; private set; }
        /// <summary>
        /// The mod file name
        /// </summary>
        public string ModFileName { get; private set; }
        /// <summary>
        /// The mod file path
        /// </summary>
        public string ModFilePath { get; private set; }
        /// <summary>
        /// The mod parts and their scripts
        /// </summary>
        internal Dictionary<string, ModPartInfo> ModParts { get; set; }
        /// <summary>
        /// The mod version. We recommend using <seealso href="https://semver.org/">Semantic Versioning</seealso> scheme.
        /// </summary>
        public string ModVersion { get; private set; }
        /// <summary>
        /// The localization information containing the mod's strings
        /// </summary>
        public Dictionary<string, string[]> ModStrings { get; private set; } = new();

        /// <summary>
        /// Creates new mod info instance
        /// </summary>
        internal ModInfo(string ModName, string ModFileName, string ModFilePath, Dictionary<string, ModPartInfo> ModParts, string ModVersion, Dictionary<string, string[]> ModStrings)
        {
            // Validate values. Check to see if the name is null. If so, it will take the mod file name.
            if (string.IsNullOrWhiteSpace(ModName))
            {
                ModName = ModFileName;
            }

            // Check to see if the mod parts is null or zero. If so, throw exception.
            if (ModParts is null || ModParts.Count == 0)
            {
                throw new KernelException(KernelExceptionType.ModNoParts, Translate.DoTranslation("There are no parts in mod."));
            }

            // Install values to new instance
            this.ModName = ModName;
            this.ModFileName = ModFileName;
            this.ModFilePath = ModFilePath;
            this.ModParts = ModParts;
            this.ModVersion = ModVersion;
            this.ModStrings = ModStrings;
        }

    }
}
