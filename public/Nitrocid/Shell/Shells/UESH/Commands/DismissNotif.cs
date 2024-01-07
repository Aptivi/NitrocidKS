﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Notifications;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Dismisses a specific notification
    /// </summary>
    /// <remarks>
    /// Takes a notification number to dismiss a specified notification, if you're done with it, or you don't want clutter on your recents.
    /// <br></br>
    /// To show available notifications, consult shownotifs command.
    /// </remarks>
    class DismissNotifCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            int NotifIndex = (int)Math.Round(Convert.ToDouble(parameters.ArgumentsList[0]) - 1d);
            if (NotificationManager.NotifDismiss(NotifIndex))
            {
                TextWriterColor.Write(Translate.DoTranslation("Notification dismissed successfully."));
                return 0;
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Error trying to dismiss notification."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.NotificationManagement;
            }
        }

    }
}
