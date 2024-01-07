﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Files.Editors.TextEdit;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using System.Linq;
using Textify.General;

namespace Nitrocid.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Queries a character in a specified line or all lines
    /// </summary>
    /// <remarks>
    /// You can use this command to query a character and get its number from the specified line or all lines. This is useful for some commands like delcharnum.
    /// </remarks>
    class QueryCharCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length == 2)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[1]))
                {
                    if (Convert.ToInt32(parameters.ArgumentsList[1]) <= TextEditShellCommon.FileLines.Count)
                    {
                        int LineIndex = Convert.ToInt32(parameters.ArgumentsList[1]);
                        var QueriedChars = TextEditTools.QueryChar(Convert.ToChar(parameters.ArgumentsList[0]), LineIndex);
                        TextWriters.Write("- {0}: ", false, KernelColorType.ListEntry, LineIndex);

                        // Process the output
                        string text = TextEditShellCommon.FileLines[LineIndex - 1];
                        for (int charIndex = 0; charIndex < text.Length; charIndex++)
                        {
                            char Character = text[charIndex];
                            TextWriters.Write($"{(QueriedChars.Contains(charIndex) ? KernelColorTools.GetColor(KernelColorType.Success).VTSequenceForeground : "")}{Character}", false, KernelColorType.ListValue);
                        }
                        TextWriterColor.Write();
                        return 0;
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                        return 10000 + (int)KernelExceptionType.TextEditor;
                    }
                }
                else if (parameters.ArgumentsList[1].ToLower() == "all")
                {
                    var QueriedChars = TextEditTools.QueryChar(Convert.ToChar(parameters.ArgumentsList[0]));
                    foreach (var QueriedChar in QueriedChars)
                    {
                        int LineIndex = QueriedChar.Item1;
                        TextWriters.Write("- {0}: ", false, KernelColorType.ListEntry, LineIndex + 1);

                        // Process the output
                        string text = TextEditShellCommon.FileLines[LineIndex];
                        var queried = QueriedChar.Item2;
                        for (int charIndex = 0; charIndex < text.Length; charIndex++)
                        {
                            char Character = text[charIndex];
                            TextWriters.Write($"{(queried.Contains(charIndex) ? KernelColorTools.GetColor(KernelColorType.Success).VTSequenceForeground : "")}{Character}", false, KernelColorType.ListValue);
                        }
                        TextWriterColor.Write();
                    }
                    return 0;
                }
            }
            else if (parameters.ArgumentsList.Length > 2)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[1]) & TextTools.IsStringNumeric(parameters.ArgumentsList[2]))
                {
                    if (Convert.ToInt32(parameters.ArgumentsList[1]) <= TextEditShellCommon.FileLines.Count & Convert.ToInt32(parameters.ArgumentsList[2]) <= TextEditShellCommon.FileLines.Count)
                    {
                        int LineNumberStart = Convert.ToInt32(parameters.ArgumentsList[1]);
                        int LineNumberEnd = Convert.ToInt32(parameters.ArgumentsList[2]);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        for (int LineNumber = LineNumberStart; LineNumber <= LineNumberEnd; LineNumber++)
                        {
                            var QueriedChars = TextEditTools.QueryChar(Convert.ToChar(parameters.ArgumentsList[0]), LineNumber);
                            int LineIndex = LineNumber - 1;
                            TextWriters.Write("- {0}: ", false, KernelColorType.ListEntry, LineNumber);

                            // Process the output
                            string text = TextEditShellCommon.FileLines[LineIndex];
                            for (int charIndex = 0; charIndex < text.Length; charIndex++)
                            {
                                char Character = text[charIndex];
                                TextWriters.Write($"{(QueriedChars.Contains(charIndex) ? KernelColorTools.GetColor(KernelColorType.Success).VTSequenceForeground : "")}{Character}", false, KernelColorType.ListValue);
                            }
                            TextWriterColor.Write();
                        }
                        return 0;
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                        return 10000 + (int)KernelExceptionType.TextEditor;
                    }
                }
            }
            return 0;
        }

    }
}
