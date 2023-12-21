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

using System.Collections.Generic;

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

using KS.Arguments;
using KS.Arguments.ArgumentBase;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class ArgInjCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var FinalArgs = new List<string>();
            foreach (string arg in ListArgs)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Parsing argument {0}...", arg);
                if (ArgumentParse.AvailableArgs.ContainsKey(arg))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Adding argument {0}...", arg);
                    FinalArgs.Add(arg);
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Argument {0} not found.", arg);
                    TextWriterColor.Write(Translate.DoTranslation("Argument {0} not found to inject."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Warning), arg);
                }
            }
            if (FinalArgs.Count == 0)
            {
                TextWriterColor.Write(Translate.DoTranslation("No arguments specified. Hint: Specify multiple arguments separated by spaces"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
            }
            else
            {
                ArgumentPrompt.EnteredArguments = new List<string>(FinalArgs);
                Flags.ArgsInjected = true;
                TextWriterColor.Write(Translate.DoTranslation("Injected arguments, {0}, will be scheduled to run at next reboot."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), string.Join(", ", ArgumentPrompt.EnteredArguments));
            }
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("where arguments will be {0}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), string.Join(", ", ArgumentParse.AvailableArgs.Keys));
        }

    }
}