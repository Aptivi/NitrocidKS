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

using KS.Kernel.Configuration;
using KS.Kernel.Configuration.Instances;
using KS.Kernel.Configuration.Settings;
using Newtonsoft.Json;

namespace Nitrocid.Extras.Timers.Settings
{
    /// <summary>
    /// Configuration instance for timers
    /// </summary>
    public class TimersConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries =>
            ConfigTools.GetSettingsEntries(Resources.AddonResources.TimersSettings);

        /// <summary>
        /// If enabled, will use figlet for timer. Please note that it needs a big console screen in order to render the time properly with Figlet enabled.
        /// </summary>
        public bool EnableFigletTimer { get; set; }
        /// <summary>
        /// Write a figlet font that is supported by the Figletize library. Consult the library documentation for more information
        /// </summary>
        public string TimerFigletFont { get; set; } = "small";
    }
}
