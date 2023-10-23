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
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Replaces a word or phrase with another one using regular expressions
    /// </summary>
    /// <remarks>
    /// You can use this command to replace a word or phrase enclosed in double quotes with another one enclosed in double quotes.
    /// </remarks>
    class ReplaceRegexCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            TextEditTools.ReplaceRegex(parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("String replaced."), true, KernelColorType.Success);
            return 0;
        }

    }
}