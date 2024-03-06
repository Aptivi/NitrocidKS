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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Security.Permissions;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Users.Groups;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Removes uninvited or redundant group
    /// </summary>
    /// <remarks>
    /// This command lets you remove the uninvited or redundant group from the group dictionary that is initialized at the start of the kernel.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class RmGroupCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            PermissionsTools.Demand(PermissionTypes.ManageGroups);
            GroupManagement.RemoveGroup(parameters.ArgumentsList[0]);
            if (!GroupManagement.DoesGroupExist(parameters.ArgumentsList[0]))
                TextWriterColor.Write(Translate.DoTranslation("Group {0} removed."), parameters.ArgumentsList[0]);
            return 0;
        }

    }
}
