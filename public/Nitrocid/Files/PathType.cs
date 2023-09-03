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

namespace KS.Files
{
    /// <summary>
    /// Specifies the kernel path type
    /// </summary>
    public enum KernelPathType
    {
        /// <summary>
        /// Mods directory. Usually found in user directory as KSMods.
        /// </summary>
        Mods,
        /// <summary>
        /// Configuration file.
        /// </summary>
        Configuration,
        /// <summary>
        /// Kernel debug log file.
        /// </summary>
        Debugging,
        /// <summary>
        /// Aliases file.
        /// </summary>
        Aliases,
        /// <summary>
        /// Users file.
        /// </summary>
        Users,
        /// <summary>
        /// Speed dial file.
        /// </summary>
        SpeedDial,
        /// <summary>
        /// Debug devices configuration file.
        /// </summary>
        DebugDevNames,
        /// <summary>
        /// MOTD text file.
        /// </summary>
        MOTD,
        /// <summary>
        /// MAL text file.
        /// </summary>
        MAL,
        /// <summary>
        /// Events folder
        /// </summary>
        Events,
        /// <summary>
        /// Reminders folder
        /// </summary>
        Reminders,
        /// <summary>
        /// Custom languages folder
        /// </summary>
        CustomLanguages,
        /// <summary>
        /// Custom splashes folder
        /// </summary>
        CustomSplashes,
        /// <summary>
        /// Journal file
        /// </summary>
        Journalling,
        /// <summary>
        /// Contacts directory
        /// </summary>
        Contacts,
        /// <summary>
        /// Contacts import directory
        /// </summary>
        ContactsImport,
        /// <summary>
        /// Screensaver configuration file.
        /// </summary>
        SaverConfiguration,
        /// <summary>
        /// Splash configuration file.
        /// </summary>
        SplashConfiguration,
        /// <summary>
        /// User groups configuration file.
        /// </summary>
        UserGroups,
        /// <summary>
        /// To-do list configuration file.
        /// </summary>
        ToDoList,
        /// <summary>
        /// Addons folder
        /// </summary>
        Addons
    }
}
