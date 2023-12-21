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
using System.Text.RegularExpressions;
using KS.ConsoleBase.Colors;

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

using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class SearchCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            try
            {
                var Matches = Searching.SearchFileForStringRegexp(ListArgs[1], new Regex(ListArgs[0], RegexOptions.IgnoreCase));
                foreach (string Match in Matches)
                    TextWriterColor.Write(Match, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Error trying to search {0} for {1}", ListArgs[0], ListArgs[1]);
                DebugWriter.WStkTrc(ex);
                TextWriterColor.Write(Translate.DoTranslation("Searching {0} for {1} failed.") + " {2}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ListArgs[0], ListArgs[1], ex.Message);
            }
        }

    }
}