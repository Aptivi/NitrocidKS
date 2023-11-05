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

using KS.Files.Editors.JsonShell;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Switches;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace KS.Shell.Shells.Json.Commands
{
    /// <summary>
    /// Adds a new array
    /// </summary>
    /// <remarks>
    /// You can use this command to add an array to the end of the parent property. Note that the parent property must exist.
    /// </remarks>
    class AddArrayCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string parent = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-parentProperty");
            JsonTools.AddNewArray(parent, parameters.ArgumentsList[0], JArray.Parse("[ \"" + string.Join("\", \"", parameters.ArgumentsList.Skip(1).ToArray()) + "\" ]"));
            return 0;
        }
    }
}