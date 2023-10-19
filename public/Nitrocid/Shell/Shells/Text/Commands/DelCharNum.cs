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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files.Editors.TextEdit;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Deletes a character from character number in specified line.
    /// </summary>
    /// <remarks>
    /// You can use this command to delete a character using a character number in a specified line. You can revise the print command output, but it will only tell you the line number and not the character number. To solve the problem, use the querychar command.
    /// </remarks>
    class TextEdit_DelCharNumCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (TextTools.IsStringNumeric(parameters.ArgumentsList[1]) & TextTools.IsStringNumeric(parameters.ArgumentsList[0]))
            {
                if (Convert.ToInt32(parameters.ArgumentsList[1]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                {
                    TextEditTools.TextEdit_DeleteChar(Convert.ToInt32(parameters.ArgumentsList[0]), Convert.ToInt32(parameters.ArgumentsList[1]));
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Character deleted."), true, KernelColorType.Success);
                    return 0;
                }
                else
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                    return 10000 + (int)KernelExceptionType.TextEditor;
                }
            }
            else
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("One or both of the numbers are not numeric."), true, KernelColorType.Error);
                DebugWriter.WriteDebug(DebugLevel.E, "{0} and {1} are not numeric values.", parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
                return 10000 + (int)KernelExceptionType.TextEditor;
            }
        }

    }
}