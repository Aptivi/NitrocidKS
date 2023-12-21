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

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Misc.Reflection;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Misc.Editors.HexEdit.Commands
{
    class HexEdit_QueryByteCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if ((ListArgs?.Count()) is { } arg1 && arg1 == 1)
            {
                byte ByteContent = Convert.ToByte(ListArgs[0], 16);
                HexEditTools.HexEdit_QueryByteAndDisplay(ByteContent);
            }
            else if ((ListArgs?.Count()) is { } arg2 && arg2 == 2)
            {
                if (StringQuery.IsStringNumeric(ListArgs[1]))
                {
                    if (Convert.ToInt64(ListArgs[1]) <= HexEditShellCommon.HexEdit_FileBytes.LongCount())
                    {
                        byte ByteContent = Convert.ToByte(ListArgs[0], 16);
                        HexEditTools.HexEdit_QueryByteAndDisplay(ByteContent, Convert.ToInt64(ListArgs[1]));
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                    }
                }
            }
            else if ((ListArgs?.Count()) is { } arg3 && arg3 > 2)
            {
                if (StringQuery.IsStringNumeric(ListArgs[1]) & StringQuery.IsStringNumeric(ListArgs[2]))
                {
                    if (Convert.ToInt64(ListArgs[1]) <= HexEditShellCommon.HexEdit_FileBytes.LongCount() & Convert.ToInt64(ListArgs[2]) <= HexEditShellCommon.HexEdit_FileBytes.LongCount())
                    {
                        byte ByteContent = Convert.ToByte(ListArgs[0], 16);
                        long ByteNumberStart = Convert.ToInt64(ListArgs[1]);
                        long ByteNumberEnd = Convert.ToInt64(ListArgs[2]);
                        ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
                        HexEditTools.HexEdit_QueryByteAndDisplay(ByteContent, ByteNumberStart, ByteNumberEnd);
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                    }
                }
            }
        }

    }
}