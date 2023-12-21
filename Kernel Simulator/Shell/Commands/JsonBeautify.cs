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

using System.IO;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Beautifiers;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class JsonBeautifyCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string JsonFile = Filesystem.NeutralizePath(ListArgs[0]);
            string JsonOutputFile;
            string BeautifiedJson;

            if (Checking.FileExists(JsonFile))
            {
                // Beautify the JSON and display it on screen
                BeautifiedJson = JsonBeautifier.BeautifyJson(JsonFile);
                TextWriterColor.Write(BeautifiedJson, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));

                // Beautify it to an output file specified (optional)
                if (ListArgs.Count() > 1)
                {
                    JsonOutputFile = Filesystem.NeutralizePath(ListArgs[1]);
                    File.WriteAllText(JsonOutputFile, BeautifiedJson);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("File {0} not found."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), JsonFile);
            }
        }

    }
}