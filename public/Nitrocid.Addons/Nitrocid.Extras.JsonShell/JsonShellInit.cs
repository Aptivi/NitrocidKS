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

using Nitrocid.Extras.JsonShell.Commands;
using Nitrocid.Extras.JsonShell.Json;
using Nitrocid.Extras.JsonShell.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Modifications;
using Nitrocid.Shell.Prompts;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Nitrocid.Extras.JsonShell
{
    internal class JsonShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("jsondiff", /* Localizable */ "Shows the difference between two JSON files",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "file1"),
                        new CommandArgumentPart(true, "file2"),
                    })
                ], new JsonDiffCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("jsonbeautify", /* Localizable */ "Beautifies the JSON file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "jsonfile"),
                        new CommandArgumentPart(true, "output"),
                    ], true)
                ], new JsonBeautifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("jsonminify", /* Localizable */ "Minifies the JSON file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "jsonfile"),
                        new CommandArgumentPart(true, "output"),
                    ], true)
                ], new JsonMinifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasJsonShell);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        internal static JsonConfig JsonConfig =>
            (JsonConfig)Config.baseConfigurations[nameof(JsonConfig)];

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
        {
            var config = new JsonConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterAddonShell("JsonShell", new JsonShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterAddonShell("JsonShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(JsonConfig));
        }
    }
}
