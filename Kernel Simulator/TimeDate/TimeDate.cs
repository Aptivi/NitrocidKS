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
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Screensaver;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;

namespace KS.TimeDate
{
    public static class TimeDate
    {

        // Variables
        public static DateTime KernelDateTime = new();
        public static DateTime KernelDateTimeUtc = new();
        internal static KernelThread TimeDateChange = new("Time/date updater thread", true, TimeDateChange_DoWork);

        /// <summary>
        /// Specifies the time/date format type.
        /// </summary>
        public enum FormatType
        {
            /// <summary>
            /// Long time/date format
            /// </summary>
            Long,
            /// <summary>
            /// Short time/date format
            /// </summary>
            Short
        }

        /// <summary>
        /// Updates the time and date. Also updates the time and date corner if it was enabled in kernel configuration.
        /// </summary>
        public static void TimeDateChange_DoWork()
        {
            try
            {
                int oldWid = default, oldTop = default;
                while (true)
                {
                    string TimeString = $"{TimeDateRenderers.RenderDate()} - {TimeDateRenderers.RenderTime()}";
                    KernelDateTime = DateTime.Now;
                    KernelDateTimeUtc = DateTime.UtcNow;
                    if (Flags.CornerTimeDate == true & !Screensaver.InSaver)
                    {
                        oldWid = ConsoleWrapper.WindowWidth - TimeString.Length - 1;
                        oldTop = Console.WindowTop;
                        TextWriters.WriteWhere(TimeString, ConsoleWrapper.WindowWidth - TimeString.Length - 1, Console.WindowTop, true, KernelColorTools.ColTypes.Neutral);
                    }
                    Thread.Sleep(1000);
                    if (oldWid != 0)
                        TextWriters.WriteWhere(" ".Repeat(TimeString.Length), oldWid, oldTop, true, KernelColorTools.ColTypes.Neutral);
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.Wdbg(DebugLevel.W, "Aborting time/date change thread.");
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Fatal error in time/date changer: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
            }
        }

        /// <summary>
        /// Updates the KernelDateTime so it reflects the current time, and runs the updater.
        /// </summary>
        public static void InitTimeDate()
        {
            if (!TimeDateChange.IsAlive)
            {
                KernelDateTime = DateTime.Now;
                KernelDateTimeUtc = DateTime.UtcNow;
                TimeDateChange.Start();
            }
        }

        /// <summary>
        /// Shows current time, date, and timezone.
        /// </summary>
        public static void ShowCurrentTimes()
        {
            TextWriters.Write("datetime: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Current time is {0}"), true, KernelColorTools.ColTypes.ListValue, TimeDateRenderers.RenderTime());
            TextWriters.Write("datetime: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Today is {0}"), true, KernelColorTools.ColTypes.ListValue, TimeDateRenderers.RenderDate());
            TextWriters.Write("datetime: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Time and date in UTC: {0}"), true, KernelColorTools.ColTypes.ListValue, TimeDateRenderersUtc.RenderUtc());
            TextWriters.Write("datetime: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Time Zone:") + " {0} ({1})", true, KernelColorTools.ColTypes.ListValue, TimeZoneInfo.Local.StandardName, TimeZoneInfo.Local.GetUtcOffset(KernelDateTime).ToString((TimeZoneInfo.Local.GetUtcOffset(KernelDateTime) < TimeSpan.Zero ? @"\-" : @"\+") + @"hh\:mm\:ss"));
        }

        /// <summary>
        /// Gets the remaining time from now
        /// </summary>
        /// <param name="Milliseconds">The milliseconds interval</param>
        public static string GetRemainingTimeFromNow(int Milliseconds)
        {
            var ThisMoment = KernelDateTime;
            var RemainingTime = ThisMoment.AddMilliseconds(Milliseconds) - ThisMoment;
            string RemainingTimeString = RemainingTime.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult);
            return RemainingTimeString;
        }

    }
}