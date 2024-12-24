﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using System;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files.Editors.HexEdit;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using Nitrocid.Shell.ShellBase.Commands;
using Textify.General;

namespace Nitrocid.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Replaces a byte with another one
    /// </summary>
    /// <remarks>
    /// You can use this command to replace a byte with another one.
    /// </remarks>
    class ReplaceCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var FileBytes = HexEditShellCommon.FileBytes ??
                throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("Hex file is not open yet."));
            if (parameters.ArgumentsList.Length == 2)
            {
                byte ByteFrom = Convert.ToByte(parameters.ArgumentsList[0], 16);
                byte ByteWith = Convert.ToByte(parameters.ArgumentsList[1], 16);
                HexEditTools.Replace(ByteFrom, ByteWith);
                TextWriters.Write(Translate.DoTranslation("Byte replaced."), true, KernelColorType.Success);
                return 0;
            }
            else if (parameters.ArgumentsList.Length == 3)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[2]))
                {
                    if (Convert.ToInt64(parameters.ArgumentsList[2]) <= FileBytes.LongLength)
                    {
                        byte ByteFrom = Convert.ToByte(parameters.ArgumentsList[0], 16);
                        byte ByteWith = Convert.ToByte(parameters.ArgumentsList[1], 16);
                        HexEditTools.Replace(ByteFrom, ByteWith, Convert.ToInt64(parameters.ArgumentsList[2]));
                        TextWriters.Write(Translate.DoTranslation("Byte replaced."), true, KernelColorType.Success);
                        return 0;
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
                    }
                }
            }
            else if (parameters.ArgumentsList.Length > 3)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[2]) & TextTools.IsStringNumeric(parameters.ArgumentsList[3]))
                {
                    if (Convert.ToInt64(parameters.ArgumentsList[2]) <= FileBytes.LongLength & Convert.ToInt64(parameters.ArgumentsList[3]) <= FileBytes.LongLength)
                    {
                        byte ByteFrom = Convert.ToByte(parameters.ArgumentsList[0], 16);
                        byte ByteWith = Convert.ToByte(parameters.ArgumentsList[1], 16);
                        long ByteNumberStart = Convert.ToInt64(parameters.ArgumentsList[2]);
                        long ByteNumberEnd = Convert.ToInt64(parameters.ArgumentsList[3]);
                        ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
                        HexEditTools.Replace(ByteFrom, ByteWith, ByteNumberStart, ByteNumberEnd);
                        TextWriters.Write(Translate.DoTranslation("Byte replaced."), true, KernelColorType.Success);
                        return 0;
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
                    }
                }
            }
            return 0;
        }

    }
}
