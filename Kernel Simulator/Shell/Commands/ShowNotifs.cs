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
using KS.Misc.Notifiers;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class ShowNotifsCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            int Count = 1;
            if (!(Notifications.NotifRecents.Count == 0))
            {
                foreach (Notification Notif in Notifications.NotifRecents)
                {
                    TextWriters.Write($"[{Count}/{Notifications.NotifRecents.Count}] {Notif.Title}: ", false, KernelColorTools.ColTypes.ListEntry);
                    TextWriters.Write(Notif.Desc, false, KernelColorTools.ColTypes.ListValue);
                    if (Notif.Type == Notifications.NotifType.Progress)
                    {
                        TextWriters.Write($" ({Notif.Progress}%)", false, Notif.ProgressFailed ? KernelColorTools.ColTypes.Error : KernelColorTools.ColTypes.Success);
                    }
                    TextWriters.Write("", true, KernelColorTools.ColTypes.Neutral);
                    Count += 1;
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("No recent notifications"), true, KernelColorTools.ColTypes.Neutral);
            }
        }

    }
}