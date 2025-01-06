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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files.Editors.TextEdit;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using Textify.General;

namespace Nitrocid.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Replaces a word or phrase with another one in a line using regular expressions
    /// </summary>
    /// <remarks>
    /// You can use this command to replace a word or a complete phrase enclosed in double quotes with another one (enclosed in double quotes again) in a line.
    /// </remarks>
    class ReplaceInlineRegexCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length == 3)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[2]))
                {
                    if (Convert.ToInt32(parameters.ArgumentsList[2]) <= TextEditShellCommon.FileLines.Count)
                    {
                        TextEditTools.ReplaceRegex(parameters.ArgumentsList[0], parameters.ArgumentsList[1], Convert.ToInt32(parameters.ArgumentsList[2]));
                        TextWriters.Write(Translate.DoTranslation("String replaced."), true, KernelColorType.Success);
                        return 0;
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                    }
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, KernelColorType.Error, parameters.ArgumentsList[2]);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [parameters.ArgumentsList[2]]);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                }
            }
            else if (parameters.ArgumentsList.Length > 3)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[2]) & TextTools.IsStringNumeric(parameters.ArgumentsList[3]))
                {
                    if (Convert.ToInt32(parameters.ArgumentsList[2]) <= TextEditShellCommon.FileLines.Count & Convert.ToInt32(parameters.ArgumentsList[3]) <= TextEditShellCommon.FileLines.Count)
                    {
                        int LineNumberStart = Convert.ToInt32(parameters.ArgumentsList[2]);
                        int LineNumberEnd = Convert.ToInt32(parameters.ArgumentsList[3]);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        for (int LineNumber = LineNumberStart; LineNumber <= LineNumberEnd; LineNumber++)
                        {
                            TextEditTools.ReplaceRegex(parameters.ArgumentsList[0], parameters.ArgumentsList[1], LineNumber);
                            TextWriters.Write(Translate.DoTranslation("String replaced in line {0}."), true, KernelColorType.Success, LineNumber);
                        }
                        return 0;
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                    }
                }
            }
            return 0;
        }

    }
}
