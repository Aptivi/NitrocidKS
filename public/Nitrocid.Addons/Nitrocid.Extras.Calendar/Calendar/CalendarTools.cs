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

using System.Globalization;

namespace Nitrocid.Extras.Calendar.Calendar
{
    /// <summary>
    /// Calendar tools
    /// </summary>
    public static class CalendarTools
    {
        /// <summary>
        /// Gets the culture from the culture type
        /// </summary>
        /// <param name="calendar">Calendar type</param>
        public static CultureInfo GetCultureFromCalendar(CalendarTypes calendar)
        {
            switch (calendar)
            {
                case CalendarTypes.Gregorian:
                    return new CultureInfo("en-US");
                case CalendarTypes.Hijri:
                    var Cult = new CultureInfo("ar");
                    Cult.DateTimeFormat.Calendar = new HijriCalendar();
                    return Cult;
                case CalendarTypes.Persian:
                    return new CultureInfo("fa");
                case CalendarTypes.SaudiHijri:
                    return new CultureInfo("ar-SA");
                case CalendarTypes.ThaiBuddhist:
                    return new CultureInfo("th-TH");
                case CalendarTypes.Chinese:
                    return new CultureInfo("zh-CN");
                case CalendarTypes.Japanese:
                    return new CultureInfo("ja-JP");
                default:
                    return new CultureInfo("en-US");
            }
        }
    }
}