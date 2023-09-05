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

using FluentFTP.Helpers;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using Nitrocid.Extras.Notes.Management;
using System;

namespace Nitrocid.Extras.Notes.Commands
{
    internal class RemoveNote : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            if (ListArgsOnly[0].IsNumeric())
                NoteManagement.RemoveNote(Convert.ToInt32(ListArgsOnly[0]) - 1);
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("The note ID must be a note number"), true, KernelColorType.Error);
                return 7;
            }
            return 0;
        }

    }
}
