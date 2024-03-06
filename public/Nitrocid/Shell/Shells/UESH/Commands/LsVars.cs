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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Scripting;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lists variables
    /// </summary>
    /// <remarks>
    /// This command lists all the defined UESH variables by either the set or the setrange commands, UESH commands that define and set a variable to a value (choice, ...), a UESH script, a mod, or your system's environment variables.
    /// </remarks>
    class LsVarsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            foreach (string VarName in UESHVariables.Variables.Keys)
            {
                TextWriters.Write($"- {VarName}: ", false, KernelColorType.ListEntry);
                TextWriters.Write(UESHVariables.Variables[VarName], true, KernelColorType.ListValue);
            }
            return 0;
        }

    }
}
