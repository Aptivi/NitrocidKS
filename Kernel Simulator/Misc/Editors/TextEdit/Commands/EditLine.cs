//
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

using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Misc.Reflection;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Misc.Editors.TextEdit.Commands
{
    class TextEdit_EditLineCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (StringQuery.IsStringNumeric(ListArgsOnly[0]))
            {
                if (Convert.ToInt32(ListArgsOnly[0]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                {
                    string OriginalLine = TextEditShellCommon.TextEdit_FileLines[(int)Math.Round(Convert.ToDouble(ListArgsOnly[0]) - 1d)];
                    TextWriters.Write(">> ", false, KernelColorTools.ColTypes.Input);
                    string EditedLine = Input.ReadLine("", OriginalLine, false);
                    TextEditShellCommon.TextEdit_FileLines[(int)Math.Round(Convert.ToDouble(ListArgsOnly[0]) - 1d)] = EditedLine;
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorTools.ColTypes.Error);
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, KernelColorTools.ColTypes.Error, ListArgsOnly[0]);
                DebugWriter.Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs[0]);
            }
        }

    }
}