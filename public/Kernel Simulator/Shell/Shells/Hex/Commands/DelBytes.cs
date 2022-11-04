﻿
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

using Extensification.LongExts;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Editors.HexEdit;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Deletes the bytes
    /// </summary>
    /// <remarks>
    /// You can use this command to remove a extraneous bytes in a specified range. You can use the print command to review the changes.
    /// </remarks>
    class HexEdit_DelBytesCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgsOnly.Length == 1)
            {
                if (StringQuery.IsStringNumeric(ListArgsOnly[0]))
                {
                    if (Convert.ToInt64(ListArgsOnly[0]) <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                    {
                        HexEditTools.HexEdit_DeleteBytes(Convert.ToInt64(ListArgsOnly[0]));
                        TextWriterColor.Write(Translate.DoTranslation("Deleted bytes."), true, ColorTools.ColTypes.Success);
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, ColorTools.ColTypes.Error);
                    }
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Specified Byte number {0} is not a valid number."), true, ColorTools.ColTypes.Error, ListArgsOnly[0]);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly[0]);
                }
            }
            else if (ListArgsOnly.Length > 1)
            {
                if (StringQuery.IsStringNumeric(ListArgsOnly[0]) & StringQuery.IsStringNumeric(ListArgsOnly[1]))
                {
                    if (Convert.ToInt64(ListArgsOnly[0]) <= HexEditShellCommon.HexEdit_FileBytes.LongLength & Convert.ToInt64(ListArgsOnly[1]) <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                    {
                        long ByteNumberStart = Convert.ToInt64(ListArgsOnly[0]);
                        long ByteNumberEnd = Convert.ToInt64(ListArgsOnly[1]);
                        ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
                        HexEditTools.HexEdit_DeleteBytes(ByteNumberStart, ByteNumberEnd);
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, ColorTools.ColTypes.Error);
                    }
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("The byte number is not numeric."), true, ColorTools.ColTypes.Error);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly[1]);
                }
            }
        }

    }
}
