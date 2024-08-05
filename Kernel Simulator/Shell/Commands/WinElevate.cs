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
using KS.Kernel;
using KS.Languages;
using KS.Login;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
using SpecProbe.Software.Platform;

namespace KS.Shell.Commands
{
    class WinElevateCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (PlatformHelper.IsOnWindows())
            {
                bool isAdmin = WindowsUserTools.IsAdministrator();
                if (isAdmin)
                    TextWriters.Write(Translate.DoTranslation("You're already running the elevated Nitrocid session!"), true, KernelColorTools.ColTypes.Neutral);
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Elevating your Nitrocid session..."), true, KernelColorTools.ColTypes.Neutral);
                    Flags.rebootingElevated = true;
                    KernelTools.PowerManage(PowerMode.Shutdown);
                }
            }
            else
                TextWriters.Write(Translate.DoTranslation("This command is unavailable for your platform."), true, KernelColorTools.ColTypes.Neutral);
        }

    }
}
