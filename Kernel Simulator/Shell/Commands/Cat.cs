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
using KS.Files.Print;
using KS.Kernel;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class CatCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            try
            {
                bool PrintLines = Flags.PrintLineNumbers;
                if (ListSwitchesOnly.Contains("-lines"))
                    PrintLines = true;
                if (ListSwitchesOnly.Contains("-nolines"))
                    PrintLines = false; // -lines and -nolines cancel together.
                FileContentPrinter.PrintContents(ListArgs[0], PrintLines);
            }
            catch (Exception ex)
            {
                DebugWriter.WStkTrc(ex);
                TextWriters.Write(ex.Message, true, KernelColorTools.ColTypes.Error);
            }
        }

        public override void HelpHelper()
        {
            TextWriters.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, KernelColorTools.ColTypes.Neutral);
            TextWriters.Write("  -lines: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Prints the line numbers that follow the line being printed"), true, KernelColorTools.ColTypes.ListValue);
            TextWriters.Write("  -nolines: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Prevents printing the line numbers"), true, KernelColorTools.ColTypes.ListValue);
        }

    }
}