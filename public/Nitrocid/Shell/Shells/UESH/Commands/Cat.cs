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

using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Files.Print;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Prints files to console.
    /// </summary>
    /// <remarks>
    /// This command lets you print the contents of a text file to the console.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-lines</term>
    /// <description>Prints the line numbers alongside the contents</description>
    /// </item>
    /// <item>
    /// <term>-nolines</term>
    /// <description>Prints only the contents</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class CatCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            try
            {
                bool PrintLines = Flags.PrintLineNumbers;
                bool ForcePlain = false;
                if (ListSwitchesOnly.Contains("-lines"))
                    PrintLines = true;
                if (ListSwitchesOnly.Contains("-nolines"))
                    // -lines and -nolines cancel together.
                    PrintLines = false; 
                if (ListSwitchesOnly.Contains("-plain"))
                    ForcePlain = true;
                FileContentPrinter.PrintContents(ListArgsOnly[0], PrintLines, ForcePlain);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(ex.Message, true, KernelColorType.Error);
            }
        }

    }
}
