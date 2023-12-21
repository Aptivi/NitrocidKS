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

using System.IO;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Folders;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class ChDirCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            try
            {
                CurrentDirectory.SetCurrDir(ListArgs[0]);
            }
            catch (System.Security.SecurityException sex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Security error: {0} ({1})", sex.Message, sex.PermissionType);
                TextWriterColor.Write(Translate.DoTranslation("You are unauthorized to set current directory to {0}: {1}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ListArgs[0], sex.Message);
                DebugWriter.WStkTrc(sex);
            }
            catch (PathTooLongException ptlex)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Directory length: {0}", Filesystem.NeutralizePath(ListArgs[0]).Length);
                TextWriterColor.Write(Translate.DoTranslation("The path you've specified is too long."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                DebugWriter.WStkTrc(ptlex);
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(Translate.DoTranslation("Changing directory has failed: {0}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.Message);
                DebugWriter.WStkTrc(ex);
            }
        }

    }
}