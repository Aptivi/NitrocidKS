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
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Text;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Misc.Editors.HexEdit.Commands
{
    class HexEdit_DelBytesCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if ((ListArgs?.Count()) is { } arg1 && arg1 == 1)
            {
                if (StringQuery.IsStringNumeric(ListArgs[0]))
                {
                    if (Convert.ToInt64(ListArgs[0]) <= HexEditShellCommon.HexEdit_FileBytes.LongCount())
                    {
                        HexEditTools.HexEdit_DeleteBytes(Convert.ToInt64(ListArgs[0]));
                        TextWriters.Write(Translate.DoTranslation("Deleted bytes."), true, KernelColorTools.ColTypes.Success);
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorTools.ColTypes.Error);
                    }
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Specified Byte number {0} is not a valid number."), true, KernelColorTools.ColTypes.Error, ListArgs[0]);
                    DebugWriter.Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs[0]);
                }
            }
            else if ((ListArgs?.Count()) is { } arg2 && arg2 > 1)
            {
                if (StringQuery.IsStringNumeric(ListArgs[0]) & StringQuery.IsStringNumeric(ListArgs[1]))
                {
                    if (Convert.ToInt64(ListArgs[0]) <= HexEditShellCommon.HexEdit_FileBytes.LongCount() & Convert.ToInt64(ListArgs[1]) <= HexEditShellCommon.HexEdit_FileBytes.LongCount())
                    {
                        long ByteNumberStart = Convert.ToInt64(ListArgs[0]);
                        long ByteNumberEnd = Convert.ToInt64(ListArgs[1]);
                        ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
                        HexEditTools.HexEdit_DeleteBytes(ByteNumberStart, ByteNumberEnd);
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorTools.ColTypes.Error);
                    }
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("The byte number is not numeric."), true, KernelColorTools.ColTypes.Error);
                    DebugWriter.Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs[1]);
                }
            }
        }

    }
}
