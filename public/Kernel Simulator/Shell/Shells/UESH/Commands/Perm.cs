﻿
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using System;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Users.Permissions;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Manages permissions for your user or other users
    /// </summary>
    /// <remarks>
    /// If you want to manage permissions for your user or other users, or if you want to prevent the user from being logged on, use this command.
    /// <br></br>
    /// This command lets you manage permissions whether the administrative privileges are on or off, or if the user is disabled or not.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class PermCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string target = ListArgsOnly[0];
            string mode = ListArgsOnly[1];
            string perm = ListArgsOnly[2];

            if (!Enum.TryParse(typeof(PermissionTypes), perm, out object permission))
                // Permission not found
                throw new KernelException(KernelExceptionType.PermissionManagement, Translate.DoTranslation("No such permission"));

            if (mode == "allow")
                // Granting permission.
                PermissionsTools.GrantPermission(target, (PermissionTypes)permission);
            else if (mode == "revoke")
                // Revoking permission.
                PermissionsTools.RevokePermission(target, (PermissionTypes)permission);
            else
                // No mode
                throw new KernelException(KernelExceptionType.PermissionManagement, Translate.DoTranslation("No such permission mode"));
        }

    }
}
