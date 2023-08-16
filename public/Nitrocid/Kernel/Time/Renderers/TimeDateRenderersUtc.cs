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

using System;
using System.Globalization;
using KS.Languages;

namespace KS.Kernel.Time.Renderers
{
    /// <summary>
    /// Time and date rendering module (UTC)
    /// </summary>
    public static class TimeDateRenderersUtc
    {

        /// <summary>
        /// Renders the current time based on kernel config (long or short) and current culture
        /// </summary>
        /// <returns>A long or short time</returns>
        public static string RenderTimeUtc() =>
            Flags.LongTimeDate
            ? TimeDateTools.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.LongTimePattern, CultureManager.CurrentCult)
            : TimeDateTools.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);

        /// <summary>
        /// Renders the current time based on kernel config (long or short) and current culture
        /// </summary>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time</returns>
        public static string RenderTimeUtc(FormatType FormatType) =>
            FormatType == FormatType.Long
            ? TimeDateTools.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.LongTimePattern, CultureManager.CurrentCult)
            : TimeDateTools.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);

        /// <summary>
        /// Renders the current time based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <returns>A time</returns>
        public static string RenderTimeUtc(CultureInfo Cult) =>
            Flags.LongTimeDate
            ? TimeDateTools.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.LongTimePattern, Cult)
            : TimeDateTools.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the current time based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time</returns>
        public static string RenderTimeUtc(CultureInfo Cult, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? TimeDateTools.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.LongTimePattern, Cult)
            : TimeDateTools.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the time based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <returns>A long or short time</returns>
        public static string RenderTimeUtc(DateTime DT) =>
            Flags.LongTimeDate
            ? DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.LongTimePattern, CultureManager.CurrentCult)
            : DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);

        /// <summary>
        /// Renders the time based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time</returns>
        public static string RenderTimeUtc(DateTime DT, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.LongTimePattern, CultureManager.CurrentCult)
            : DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);

        /// <summary>
        /// Renders the time based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <param name="Cult">A culture</param>
        /// <returns>A time</returns>
        public static string RenderTimeUtc(DateTime DT, CultureInfo Cult) =>
            Flags.LongTimeDate
            ? DT.ToUniversalTime().ToString(Cult.DateTimeFormat.LongTimePattern, Cult)
            : DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the time based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time</param>
        /// <param name="Cult">A culture</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time</returns>
        public static string RenderTimeUtc(DateTime DT, CultureInfo Cult, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? DT.ToUniversalTime().ToString(Cult.DateTimeFormat.LongTimePattern, Cult)
            : DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the current date based on kernel config (long or short) and current culture
        /// </summary>
        /// <returns>A long or short date</returns>
        public static string RenderDateUtc() =>
            Flags.LongTimeDate
            ? TimeDateTools.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.LongDatePattern, CultureManager.CurrentCult)
            : TimeDateTools.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult);

        /// <summary>
        /// Renders the current date based on kernel config (long or short) and current culture
        /// </summary>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short date</returns>
        public static string RenderDateUtc(FormatType FormatType) =>
            FormatType == FormatType.Long
            ? TimeDateTools.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.LongDatePattern, CultureManager.CurrentCult)
            : TimeDateTools.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult);

        /// <summary>
        /// Renders the current date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <returns>A date</returns>
        public static string RenderDateUtc(CultureInfo Cult) =>
            Flags.LongTimeDate
            ? TimeDateTools.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.LongDatePattern, Cult)
            : TimeDateTools.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);

        /// <summary>
        /// Renders the current date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A date</returns>
        public static string RenderDateUtc(CultureInfo Cult, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? TimeDateTools.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.LongDatePattern, Cult)
            : TimeDateTools.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);

        /// <summary>
        /// Renders the date based on specified date using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <returns>A long or short date</returns>
        public static string RenderDateUtc(DateTime DT) =>
            Flags.LongTimeDate
            ? DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.LongDatePattern, CultureManager.CurrentCult)
            : DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult);

        /// <summary>
        /// Renders the date based on specified date using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short date</returns>
        public static string RenderDateUtc(DateTime DT, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.LongDatePattern, CultureManager.CurrentCult)
            : DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult);

        /// <summary>
        /// Renders the date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <param name="Cult">A culture</param>
        /// <returns>A date</returns>
        public static string RenderDateUtc(DateTime DT, CultureInfo Cult) =>
            Flags.LongTimeDate
            ? DT.ToUniversalTime().ToString(Cult.DateTimeFormat.LongDatePattern, Cult)
            : DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);

        /// <summary>
        /// Renders the date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified date</param>
        /// <param name="Cult">A culture</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A date</returns>
        public static string RenderDateUtc(DateTime DT, CultureInfo Cult, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? DT.ToUniversalTime().ToString(Cult.DateTimeFormat.LongDatePattern, Cult)
            : DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortDatePattern, Cult);

        /// <summary>
        /// Renders the current time and date based on kernel config (long or short) and current culture
        /// </summary>
        /// <returns>A long or short time and date</returns>
        public static string RenderUtc() =>
            Flags.LongTimeDate
            ? TimeDateTools.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCult)
            : TimeDateTools.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult) + " - " + TimeDateTools.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);

        /// <summary>
        /// Renders the current time and date based on kernel config (long or short) and current culture
        /// </summary>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time and date</returns>
        public static string RenderUtc(FormatType FormatType) =>
            FormatType == FormatType.Long
            ? TimeDateTools.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCult)
            : TimeDateTools.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult) + " - " + TimeDateTools.KernelDateTimeUtc.ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);

        /// <summary>
        /// Renders the current time and date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <returns>A time and date</returns>
        public static string RenderUtc(CultureInfo Cult) =>
            Flags.LongTimeDate
            ? TimeDateTools.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult)
            : TimeDateTools.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + TimeDateTools.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the current time based on specified custom format
        /// </summary>
        /// <param name="CustomFormat">A custom format for rendering the time</param>
        /// <returns>A time</returns>
        public static string RenderUtc(string CustomFormat) =>
            TimeDateTools.KernelDateTimeUtc.ToString(CustomFormat, CultureManager.CurrentCult);

        /// <summary>
        /// Renders the current time and date based on specified culture
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time and date</returns>
        public static string RenderUtc(CultureInfo Cult, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? TimeDateTools.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult)
            : TimeDateTools.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + TimeDateTools.KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the current time based on specified culture and custom format
        /// </summary>
        /// <param name="Cult">A culture.</param>
        /// <param name="CustomFormat">A custom format for rendering the time</param>
        /// <returns>A time</returns>
        public static string RenderUtc(CultureInfo Cult, string CustomFormat) =>
            TimeDateTools.KernelDateTimeUtc.ToString(CustomFormat, Cult);

        /// <summary>
        /// Renders the time and date based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <returns>A long or short time and date</returns>
        public static string RenderUtc(DateTime DT) =>
            Flags.LongTimeDate
            ? DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCult)
            : DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult) + " - " + DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);

        /// <summary>
        /// Renders the time and date based on specified time using the kernel config (long or short) and current culture
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A long or short time and date</returns>
        public static string RenderUtc(DateTime DT, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.FullDateTimePattern, CultureManager.CurrentCult)
            : DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortDatePattern, CultureManager.CurrentCult) + " - " + DT.ToUniversalTime().ToString(CultureManager.CurrentCult.DateTimeFormat.ShortTimePattern, CultureManager.CurrentCult);

        /// <summary>
        /// Renders the time and date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="Cult">A culture</param>
        /// <returns>A time and date</returns>
        public static string RenderUtc(DateTime DT, CultureInfo Cult) =>
            Flags.LongTimeDate
            ? DT.ToUniversalTime().ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult)
            : DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the current time based on current culture using the custom format
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="CustomFormat">A custom format for rendering the time</param>
        /// <returns>A time</returns>
        public static string RenderUtc(DateTime DT, string CustomFormat) =>
            DT.ToUniversalTime().ToString(CustomFormat, CultureManager.CurrentCult);

        /// <summary>
        /// Renders the time and date based on specified date and culture using the kernel config (long or short)
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="Cult">A culture</param>
        /// <param name="FormatType">Date/time format type</param>
        /// <returns>A time and date</returns>
        public static string RenderUtc(DateTime DT, CultureInfo Cult, FormatType FormatType) =>
            FormatType == FormatType.Long
            ? DT.ToUniversalTime().ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult)
            : DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + DT.ToUniversalTime().ToString(Cult.DateTimeFormat.ShortTimePattern, Cult);

        /// <summary>
        /// Renders the current time based on specified date and culture using the custom format
        /// </summary>
        /// <param name="DT">Specified time and date</param>
        /// <param name="Cult">A culture</param>
        /// <param name="CustomFormat">A custom format for rendering the time</param>
        /// <returns>A time</returns>
        public static string RenderUtc(DateTime DT, CultureInfo Cult, string CustomFormat) =>
            DT.ToUniversalTime().ToString(CustomFormat, Cult);

    }
}
