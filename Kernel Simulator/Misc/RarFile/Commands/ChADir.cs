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

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
namespace KS.Misc.RarFile.Commands
{
    class RarShell_ChADirCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (!RarTools.ChangeWorkingArchiveDirectory(ListArgs[0]))
            {
                TextWriters.Write(Translate.DoTranslation("Archive directory {0} doesn't exist"), true, KernelColorTools.ColTypes.Error, ListArgs[0]);
            }
        }

    }
}