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

using KS.Languages;
using KS.Shell.ShellBase.Shells;

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

namespace KS.Shell.ShellBase.Commands
{
    public class CommandInfo
    {

        /// <summary>
        /// The command
        /// </summary>
        public string Command { get; private set; }
        /// <summary>
        /// The type of command
        /// </summary>
        public ShellType Type { get; private set; }
        /// <summary>
        /// The untranslated help definition of command. Translated by <see cref="GetTranslatedHelpEntry()"/>
        /// </summary>
        public string HelpDefinition { get; set; }
        /// <summary>
        /// Command argument info
        /// </summary>
        public CommandArgumentInfo CommandArgumentInfo { get; private set; }
        /// <summary>
        /// Command base for execution
        /// </summary>
        public CommandExecutor CommandBase { get; private set; }
        /// <summary>
        /// Is the command admin-only?
        /// </summary>
        public bool Strict { get; private set; }
        /// <summary>
        /// Is the command wrappable?
        /// </summary>
        public bool Wrappable { get; private set; }
        /// <summary>
        /// If true, the command can't be run in maintenance mode
        /// </summary>
        public bool NoMaintenance { get; private set; }
        /// <summary>
        /// Is the command obsolete?
        /// </summary>
        public bool Obsolete { get; private set; }
        /// <summary>
        /// Does the command set a UESH $variable?
        /// </summary>
        public bool SettingVariable { get; private set; }

        /// <summary>
        /// Installs a new instance of command info class
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="Type">Shell command type</param>
        /// <param name="HelpDefinition">Command help definition</param>
        /// <param name="CommandArgumentInfo">Command argument info</param>
        /// <param name="CommandBase">Command base for execution</param>
        /// <param name="Strict">Is the command admin-only?</param>
        /// <param name="Wrappable">Is the command wrappable?</param>
        /// <param name="NoMaintenance">If true, the command can't be run in maintenance mode</param>
        /// <param name="Obsolete">Is the command obsolete?</param>
        /// <param name="SettingVariable">Does the command set a UESH $variable?</param>
        public CommandInfo(string Command, ShellType Type, string HelpDefinition, CommandArgumentInfo CommandArgumentInfo, CommandExecutor CommandBase, bool Strict = false, bool Wrappable = false, bool NoMaintenance = false, bool Obsolete = false, bool SettingVariable = false)
        {
            this.Command = Command;
            this.Type = Type;
            this.HelpDefinition = HelpDefinition;
            this.CommandArgumentInfo = CommandArgumentInfo;
            this.CommandBase = CommandBase;
            this.Strict = Strict;
            this.Wrappable = Wrappable;
            this.NoMaintenance = NoMaintenance;
            this.Obsolete = Obsolete;
            this.SettingVariable = SettingVariable;
        }

        /// <summary>
        /// Gets the translated version of help entry (KS built-in commands only)
        /// </summary>
        public string GetTranslatedHelpEntry()
        {
            return Translate.DoTranslation(HelpDefinition);
        }

    }
}