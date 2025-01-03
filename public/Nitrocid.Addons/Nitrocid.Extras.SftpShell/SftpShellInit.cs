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

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Extras.SftpShell.Commands;
using Nitrocid.Extras.SftpShell.Settings;
using Nitrocid.Extras.SftpShell.SFTP;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using System.Linq;

namespace Nitrocid.Extras.SftpShell
{
    internal class SftpShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("sftp", /* Localizable */ "Lets you use an SSH FTP server",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(false, "server"),
                    })
                ], new SftpCommandExec()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasSftpShell);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        internal static SftpConfig SftpConfig =>
            (SftpConfig)Config.baseConfigurations[nameof(SftpConfig)];

        void IAddon.FinalizeAddon()
        {
            var config = new SftpConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterAddonShell("SFTPShell", new SFTPShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterAddonShell("SFTPShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(SftpConfig));
        }
    }
}
