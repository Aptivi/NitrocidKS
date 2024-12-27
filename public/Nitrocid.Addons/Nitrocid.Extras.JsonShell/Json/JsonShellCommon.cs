﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nitrocid.Extras.JsonShell.Tools;
using Nitrocid.Kernel.Threading;

namespace Nitrocid.Extras.JsonShell.Json
{
    /// <summary>
    /// Common JSON shell module
    /// </summary>
    public static class JsonShellCommon
    {

        internal static JToken FileTokenOrig = JToken.Parse("{}");
        internal static FileStream? FileStream;
        internal static KernelThread AutoSave = new("JSON Shell Autosave Thread", false, JsonTools.HandleAutoSaveJsonFile);
        internal static int autoSaveInterval = 60;

        /// <summary>
        /// JSON shell file token
        /// </summary>
        public static JToken FileToken { get; set; } = JToken.Parse("{}");
        /// <summary>
        /// JSON formatting
        /// </summary>
        public static Formatting Formatting =>
            (Formatting)JsonShellInit.JsonConfig.JsonShellFormatting;
        /// <summary>
        /// Auto save flag
        /// </summary>
        public static bool AutoSaveFlag =>
            JsonShellInit.JsonConfig.JsonEditAutoSaveFlag;
        /// <summary>
        /// Auto save interval in seconds
        /// </summary>
        public static int AutoSaveInterval =>
            JsonShellInit.JsonConfig.JsonEditAutoSaveInterval;

    }
}
