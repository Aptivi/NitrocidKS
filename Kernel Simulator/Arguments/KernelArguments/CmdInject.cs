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

using KS.Arguments.ArgumentBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.Arguments.KernelArguments
{
    class CmdInjectArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgs is not null)
            {
                foreach (string InjectedCommand in ListArgsOnly)
                {
                    Shell.Shell.InjectedCommands.AddRange(InjectedCommand.Split(new[] { " : " }, StringSplitOptions.RemoveEmptyEntries));
                    Flags.CommandFlag = true;
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Available commands: {0}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), string.Join(", ", Shell.Shell.Commands.Keys));
                TextWriterColor.Write(">> ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
                Shell.Shell.InjectedCommands.AddRange(Input.ReadLine().Split(new[] { " : " }, StringSplitOptions.RemoveEmptyEntries));
                if (string.Join(", ", Shell.Shell.InjectedCommands) != "q")
                {
                    Flags.CommandFlag = true;
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Command injection has been cancelled."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
                }
            }
        }

    }
}