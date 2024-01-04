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

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Extras.UnitConv.Commands;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using UnitsNet;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Modifications;

namespace Nitrocid.Extras.UnitConv
{
    internal class UnitConvInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "listunits",
                new CommandInfo("listunits", /* Localizable */ "Lists all available units",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "type", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => Quantity.Infos.Select((src) => src.Name).ToArray()
                            }),
                        })
                    ], new ListUnitsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "unitconv",
                new CommandInfo("unitconv", /* Localizable */ "Unit converter",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "unittype", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => Quantity.Infos.Select((src) => src.Name).ToArray()
                            }),
                            new CommandArgumentPart(true, "quantity", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                            new CommandArgumentPart(true, "sourceunit"),
                            new CommandArgumentPart(true, "targetunit"),
                        ],
                        [
                            new SwitchInfo("tui", /* Localizable */ "Use the TUI version of the unit converter", new SwitchOptions()
                            {
                                OptionalizeLastRequiredArguments = 4,
                                AcceptsValues = false
                            })
                        ])
                    ], new UnitConvCommand())
            },
        };

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasUnitConv);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands.Values]);

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Keys]);

        void IAddon.FinalizeAddon()
        { }
    }
}
