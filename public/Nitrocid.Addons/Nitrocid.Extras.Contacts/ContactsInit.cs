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

using KS.Kernel.Debugging;
using KS.Kernel.Extensions;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Nitrocid.Extras.Contacts.Contacts;
using Nitrocid.Extras.Contacts.Contacts.Commands;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.Contacts
{
    internal class ContactsInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "contacts",
                new CommandInfo("contacts", ShellType.Shell, /* Localizable */ "Manages your contacts",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ContactsCommand())
            },
        };

        string IAddon.AddonName => "Extras - Contacts";

        AddonType IAddon.AddonType => AddonType.Optional;

        void IAddon.FinalizeAddon()
        { }

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());

        void IAddon.StopAddon()
        {
            // Unload all contacts
            ContactsManager.RemoveContacts(false);
            DebugWriter.WriteDebug(DebugLevel.I, "Unloaded all contacts");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());
        }
    }
}
