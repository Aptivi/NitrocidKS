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
using KS.Files;
using KS.Languages;
using KS.Misc.Probers;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
namespace KS.Shell.Commands
{
    class ChMalCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if ((ListArgs?.Length) is { } arg1 && arg1 > 0)
            {
                if (string.IsNullOrEmpty(StringArgs))
                {
                    TextWriters.Write(Translate.DoTranslation("Blank MAL After Login."), true, KernelColorTools.ColTypes.Error);
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Changing MAL..."), true, KernelColorTools.ColTypes.Neutral);
                    MOTDParse.SetMOTD(StringArgs, MOTDParse.MessageType.MAL);
                }
            }
            else
            {
                ShellStart.StartShell(ShellType.TextShell, Paths.GetKernelPath(KernelPathType.MAL));
                TextWriters.Write(Translate.DoTranslation("Changing MAL..."), true, KernelColorTools.ColTypes.Neutral);
                MOTDParse.ReadMOTD(MOTDParse.MessageType.MAL);
            }
        }

    }
}