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
using KS.Misc.Writers.ConsoleWriters;

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

using KS.Network.RemoteDebug;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class BlockDbgDevCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (!RemoteDebugger.RDebugBlocked.Contains(ListArgs[0]))
            {
                if (RemoteDebugTools.TryAddToBlockList(ListArgs[0]))
                {
                    TextWriterColor.Write(Translate.DoTranslation("{0} can't join remote debug now."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), ListArgs[0]);
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Failed to block {0}."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), ListArgs[0]);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("{0} is already blocked."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), ListArgs[0]);
            }
        }

    }
}