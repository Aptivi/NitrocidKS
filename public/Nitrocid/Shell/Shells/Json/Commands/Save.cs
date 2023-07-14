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

using KS.Misc.Editors.JsonShell;
using KS.Shell.ShellBase.Commands;
using Newtonsoft.Json;

namespace KS.Shell.Shells.Json.Commands
{
    /// <summary>
    /// Saves changes to a JSON file
    /// </summary>
    /// <remarks>
    /// If you're done with the JSON file, you can save it to the current JSON file. You can optionally beautify or minify the JSON file using the below switches:
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-b</term>
    /// <description>Beautifies the JSON file while saving</description>
    /// </item>
    /// <item>
    /// <term>-m</term>
    /// <description>Minifies the JSON file while saving</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class JsonShell_SaveCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var TargetFormatting = Formatting.Indented;
            if (ListSwitchesOnly.Length > 0)
            {
                if (ListSwitchesOnly[0] == "-b")
                    TargetFormatting = Formatting.Indented;
                if (ListSwitchesOnly[0] == "-m")
                    TargetFormatting = Formatting.None;
            }
            JsonTools.JsonShell_SaveFile(false, TargetFormatting);
        }

    }
}
