﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using System.Linq;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Arguments;
using Nitrocid.Shell.Prompts;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.Shells.Admin.Commands;
using Nitrocid.Shell.Shells.Admin.Presets;

namespace Nitrocid.Shell.Shells.Admin
{
    /// <summary>
    /// Common admin shell class
    /// </summary>
    internal class AdminShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// Admin commands
        /// </summary>
        public override List<CommandInfo> Commands => new()
        {
            new CommandInfo("arghelp", /* Localizable */ "Kernel arguments help system",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(false, "argument", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => ArgumentParse.AvailableCMDLineArgs.Keys.ToArray()
                        })
                    })
                ], new ArgHelpCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("bootlog", /* Localizable */ "Prints the boot log",
                [
                    new CommandArgumentInfo()
                ], new BootLogCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("cdbglog", /* Localizable */ "Deletes everything in debug log",
                [
                    new CommandArgumentInfo()
                ], new CdbgLogCommand()),

            new CommandInfo("clearfiredevents", /* Localizable */ "Clears all fired events",
                [
                    new CommandArgumentInfo()
                ], new ClearFiredEventsCommand()),

            new CommandInfo("journal", /* Localizable */ "Gets current kernel journal log",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(false, "sessionNum", new()
                        {
                            IsNumeric = true,
                        }),
                    })
                ], new JournalCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsevents", /* Localizable */ "Lists all fired events",
                [
                    new CommandArgumentInfo()
                ], new LsEventsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsusers", /* Localizable */ "Lists the users",
                [
                    new CommandArgumentInfo(true)
                ], new LsUsersCommand()),

            new CommandInfo("savenotifs", /* Localizable */ "Saves the recent notifications",
                [
                    new CommandArgumentInfo()
                ], new SaveNotifsCommand()),

            new CommandInfo("userflag", /* Localizable */ "Manipulates with the user main flags",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "user"),
                        new CommandArgumentPart(true, "admin/anonymous/disabled"),
                        new CommandArgumentPart(true, "false/true")
                    })
                ], new UserFlagCommand()),

            new CommandInfo("userinfo", /* Localizable */ "Gets the user information",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(false, "user")
                    })
                ], new UserInfoCommand()),

            new CommandInfo("userlang", /* Localizable */ "Changes the preferred user language",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "user"),
                        new CommandArgumentPart(true, "lang/clear")
                    })
                ], new UserLangCommand()),
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new AdminDefaultPreset() },
            { "PowerLine1", new AdminPowerLine1Preset() },
            { "PowerLine2", new AdminPowerLine2Preset() },
            { "PowerLine3", new AdminPowerLine3Preset() },
            { "PowerLineBG1", new AdminPowerLineBG1Preset() },
            { "PowerLineBG2", new AdminPowerLineBG2Preset() },
            { "PowerLineBG3", new AdminPowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new AdminShell();

        public override PromptPresetBase CurrentPreset =>
            PromptPresetManager.GetAllPresetsFromShell(ShellType)[PromptPresetManager.CurrentPresets[ShellType]];

    }
}
