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
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Misc.Editors.TextEdit.Commands
{
    class TextEdit_PrintCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            int LineNumber = 1;
            if ((ListArgs?.Length) is { } arg1 && arg1 > 0)
            {
                if ((ListArgs?.Length) is { } arg2 && arg2 == 1)
                {
                    // We've only provided one line number
                    DebugWriter.Wdbg(DebugLevel.I, "Line number provided: {0}", ListArgs[0]);
                    DebugWriter.Wdbg(DebugLevel.I, "Is it numeric? {0}", StringQuery.IsStringNumeric(ListArgs[0]));
                    if (StringQuery.IsStringNumeric(ListArgs[0]))
                    {
                        LineNumber = Convert.ToInt32(ListArgs[0]);
                        DebugWriter.Wdbg(DebugLevel.I, "File lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
                        if (Convert.ToInt32(ListArgs[0]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                        {
                            string Line = TextEditShellCommon.TextEdit_FileLines[LineNumber - 1];
                            DebugWriter.Wdbg(DebugLevel.I, "Line number: {0} ({1})", LineNumber, Line);
                            TextWriterColor.Write("- {0}: ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), LineNumber);
                            TextWriterColor.Write(Line, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
                        }
                        else
                        {
                            TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ListArgs[0]);
                        DebugWriter.Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs[0]);
                    }
                }
                else
                {
                    // We've provided two line numbers in the range
                    DebugWriter.Wdbg(DebugLevel.I, "Line numbers provided: {0}, {1}", ListArgs[0], ListArgs[1]);
                    DebugWriter.Wdbg(DebugLevel.I, "Is it numeric? {0}", StringQuery.IsStringNumeric(ListArgs[0]), StringQuery.IsStringNumeric(ListArgs[1]));
                    if (StringQuery.IsStringNumeric(ListArgs[0]) & StringQuery.IsStringNumeric(ListArgs[1]))
                    {
                        int LineNumberStart = Convert.ToInt32(ListArgs[0]);
                        int LineNumberEnd = Convert.ToInt32(ListArgs[1]);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        DebugWriter.Wdbg(DebugLevel.I, "File lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
                        if (LineNumberStart <= TextEditShellCommon.TextEdit_FileLines.Count & LineNumberEnd <= TextEditShellCommon.TextEdit_FileLines.Count)
                        {
                            var loopTo = LineNumberEnd;
                            for (LineNumber = LineNumberStart; LineNumber <= loopTo; LineNumber++)
                            {
                                string Line = TextEditShellCommon.TextEdit_FileLines[LineNumber - 1];
                                DebugWriter.Wdbg(DebugLevel.I, "Line number: {0} ({1})", LineNumber, Line);
                                TextWriterColor.Write("- {0}: ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), LineNumber);
                                TextWriterColor.Write(Line, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
                            }
                        }
                        else
                        {
                            TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ListArgs[0]);
                        DebugWriter.Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs[0]);
                    }
                }
            }
            else
            {
                foreach (string Line in TextEditShellCommon.TextEdit_FileLines)
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Line number: {0} ({1})", LineNumber, Line);
                    TextWriterColor.Write("- {0}: ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), LineNumber);
                    TextWriterColor.Write(Line, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
                    LineNumber += 1;
                }
            }
        }

    }
}
