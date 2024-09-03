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

using System;
using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Calendar.Events;
using KS.Misc.Calendar.Reminders;
using KS.ConsoleBase.Writers;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Writer.FancyWriters.Tools;

namespace KS.Misc.Calendar
{
    public static class CalendarPrint
    {

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        public static void PrintCalendar()
        {
            PrintCalendar(DateTime.Today.Year, DateTime.Today.Month);
        }

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        public static void PrintCalendar(int Year, int Month)
        {
            string[] CalendarDays = CultureManager.CurrentCult.DateTimeFormat.DayNames;
            string[] CalendarMonths = CultureManager.CurrentCult.DateTimeFormat.MonthNames;
            var CalendarData = new string[6, CalendarDays.Length];
            _ = new DateTime(Year, Month, 1, CultureManager.CurrentCult.Calendar);
            var DateTo = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month), CultureManager.CurrentCult.Calendar);
            int CurrentWeek = 1;
            string CalendarTitle = CalendarMonths[Month - 1] + " " + Year;
            var CalendarCellOptions = new List<CellOptions>();

            // Populate the calendar data
            TextWriters.WriteWhere(CalendarTitle, (int)Math.Round((ConsoleWrapper.WindowWidth - CalendarTitle.Length) / 2d), ConsoleWrapper.CursorTop, true, KernelColorTools.ColTypes.Neutral);
            TextWriters.Write("", KernelColorTools.ColTypes.Neutral);
            for (int CurrentDay = 1, loopTo = DateTo.Day; CurrentDay <= loopTo; CurrentDay++)
            {
                var CurrentDate = new DateTime(Year, Month, CurrentDay, CultureManager.CurrentCult.DateTimeFormat.Calendar);
                if (CurrentDate.DayOfWeek == 0)
                    CurrentWeek += 1;
                int CurrentWeekIndex = CurrentWeek - 1;
                string CurrentDayMark = $" {CurrentDay} ";
                bool ReminderMarked = false;
                bool EventMarked = false;
                bool IsWeekend = CurrentDate.DayOfWeek == DayOfWeek.Friday | CurrentDate.DayOfWeek == DayOfWeek.Saturday;

                // Dim out the weekends
                if (IsWeekend)
                {
                    var WeekendOptions = new CellOptions((int)CurrentDate.DayOfWeek + 1, CurrentWeek)
                    {
                        ColoredCell = true,
                        CellColor = new Color(128, 128, 128),
                        CellBackgroundColor = KernelColorTools.BackgroundColor
                    };
                    CalendarCellOptions.Add(WeekendOptions);
                }

                // Know where and how to put the day number
                foreach (ReminderInfo Reminder in ReminderManager.Reminders)
                {
                    if (Reminder.ReminderDate.Date == CurrentDate & !ReminderMarked)
                    {
                        CurrentDayMark = $"({CurrentDay})";
                        ReminderMarked = true;
                    }
                }
                foreach (EventInfo EventInstance in EventManager.CalendarEvents)
                {
                    if (EventInstance.EventDate == CurrentDate & !EventMarked)
                    {
                        var EventCell = new CellOptions((int)CurrentDate.DayOfWeek + 1, CurrentWeek)
                        {
                            ColoredCell = true,
                            CellColor = KernelColorTools.StageColor,
                            CellBackgroundColor = KernelColorTools.BackgroundColor
                        };
                        CalendarCellOptions.Add(EventCell);
                        EventMarked = true;
                    }
                }
                CalendarData[CurrentWeekIndex, (int)CurrentDate.DayOfWeek] = CurrentDayMark;
            }
            TextFancyWriters.WriteTable(CalendarDays, CalendarData, 2, KernelColorTools.ColTypes.Neutral, KernelColorTools.ColTypes.Neutral, KernelColorTools.ColTypes.Neutral, KernelColorTools.ColTypes.Neutral, true, CalendarCellOptions);
        }

    }
}