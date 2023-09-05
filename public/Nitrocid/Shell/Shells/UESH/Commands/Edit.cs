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

using KS.Files;
using KS.Files.Operations;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Opens the text editor shell
    /// </summary>
    /// <remarks>
    /// If you want to edit a text document, this command will let you open the text editor shell to a specified document so you can edit it. Currently, it's on the basic stage, so it doesn't have advanced options yet.
    /// <br></br>
    /// It can also open binary files, but we don't recommend doing that, because it isn't a hex editor yet. Editing a binary file may or may not cause file corruptions. Use hexedit for such tasks.
    /// </remarks>
    class EditCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            string path = Filesystem.NeutralizePath(ListArgsOnly[0]);
            bool forceText = SwitchManager.ContainsSwitch(ListSwitchesOnly, "-text");
            bool forceJson = SwitchManager.ContainsSwitch(ListSwitchesOnly, "-json");
            bool forceHex = SwitchManager.ContainsSwitch(ListSwitchesOnly, "-hex");
            bool forceSql = SwitchManager.ContainsSwitch(ListSwitchesOnly, "-sql");
            Opening.OpenEditor(path, forceText, forceJson, forceHex, forceSql);
            return 0;
        }

    }
}
