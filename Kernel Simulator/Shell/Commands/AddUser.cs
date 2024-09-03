﻿//
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
using KS.Login;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
namespace KS.Shell.Commands
{
    class AddUserCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if ((ListArgs?.Length) is { } arg1 && arg1 == 1)
            {
                TextWriters.Write(Translate.DoTranslation("usrmgr: Creating username {0}..."), true, KernelColorTools.ColTypes.Neutral, ListArgs[0]);
                UserManagement.AddUser(ListArgs[0]);
            }
            else if ((ListArgs?.Length) is { } arg2 && arg2 > 2)
            {
                if ((ListArgs[1] ?? "") == (ListArgs[2] ?? ""))
                {
                    TextWriters.Write(Translate.DoTranslation("usrmgr: Creating username {0}..."), true, KernelColorTools.ColTypes.Neutral, ListArgs[0]);
                    UserManagement.AddUser(ListArgs[0], ListArgs[1]);
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Passwords don't match."), true, KernelColorTools.ColTypes.Error);
                }
            }
        }

    }
}