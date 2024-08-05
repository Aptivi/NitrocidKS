﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Linq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
namespace KS.Misc.Editors.JsonShell.Commands
{
    class JsonShell_PrintCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgs is not null && ((ListArgs?.Count()) is { } arg1 ? arg1 > 0 : (bool?)null).GetValueOrDefault())
            {
                TextWriters.Write(JsonTools.JsonShell_SerializeToString(StringArgs), true, KernelColorTools.ColTypes.Neutral);
            }
            else
            {
                TextWriters.Write(JsonTools.JsonShell_SerializeToString("$"), true, KernelColorTools.ColTypes.Neutral);
            }
        }

    }
}