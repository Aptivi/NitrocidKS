﻿//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using KS.Languages;
using KS.Misc.Notifications;

namespace Nitrocid.Extras.Calendar.Calendar.Events
{
    /// <summary>
    /// Event information class
    /// </summary>
    public class EventInfo
    {

        private bool EventNotified;
        /// <summary>
        /// Event date
        /// </summary>
        public DateTime EventDate { get; set; }
        /// <summary>
        /// Event title
        /// </summary>
        public string EventTitle { get; set; }

        /// <summary>
        /// Notifies the user about the event
        /// </summary>
        protected internal void NotifyEvent()
        {
            if (!EventNotified)
            {
                var EventNotification = new Notification(EventTitle, Translate.DoTranslation("Now it's an event day!"), NotificationPriority.Medium, NotificationType.Normal);
                NotificationManager.NotifySend(EventNotification);
                EventNotified = true;
            }
        }

    }
}
