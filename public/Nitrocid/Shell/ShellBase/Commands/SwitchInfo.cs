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

using KS.Languages;

namespace KS.Shell.ShellBase.Commands
{
    /// <summary>
    /// Information for command switches
    /// </summary>
    public class SwitchInfo
    {

        /// <summary>
        /// The switch name
        /// </summary>
        public string SwitchName { get; private set; }
        /// <summary>
        /// The untranslated help definition of this switch. Translated by <see cref="GetTranslatedHelpEntry()"/>
        /// </summary>
        public string HelpDefinition { get; set; }
        /// <summary>
        /// Is the switch required?
        /// </summary>
        public bool IsRequired { get; private set; }
        /// <summary>
        /// Does the switch require arguments?
        /// </summary>
        public bool ArgumentsRequired { get; private set; }

        /// <summary>
        /// Installs a new instance of switch info class
        /// </summary>
        /// <param name="Switch">Switch</param>
        /// <param name="HelpDefinition">Switch help definition</param>
        /// <param name="IsRequired">Is the switch required?</param>
        /// <param name="ArgumentsRequired">Whether the switch requires a value to be set</param>
        public SwitchInfo(string Switch, string HelpDefinition, bool IsRequired = false, bool ArgumentsRequired = false)
        {
            SwitchName = Switch;
            this.HelpDefinition = HelpDefinition;
            this.IsRequired = IsRequired;
            this.ArgumentsRequired = ArgumentsRequired;
        }

        /// <summary>
        /// Gets the translated version of help entry (KS built-in switches only)
        /// </summary>
        public string GetTranslatedHelpEntry() =>
            Translate.DoTranslation(HelpDefinition);
    }
}
