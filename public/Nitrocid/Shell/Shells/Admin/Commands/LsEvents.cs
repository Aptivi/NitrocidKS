﻿
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Events;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Admin.Commands
{
    /// <summary>
    /// Shows the list of fired events
    /// </summary>
    /// <remarks>
    /// It shows you a detailed list of fired events with the arguments passed to each of them, if any.
    /// </remarks>
    class LsEventsCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            ListWriterColor.WriteList(EventsManager.ListAllFiredEvents());
            return 0;
        }

        public override int ExecuteDumb(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            var events = EventsManager.ListAllFiredEvents();
            foreach (string @event in events.Keys)
                TextWriterColor.Write(@event);
            return 0;
        }

    }
}
