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
using System.IO;
using KS.Files.Folders;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Network.Base;
using KS.Network.Mail;
using KS.Shell.Shells.FTP;
using KS.Shell.Shells.Mail;
using KS.Shell.Shells.SFTP;
using KS.Users;
using KS.Kernel.Events;
using KS.ConsoleBase.Colors;
using System.Collections.Generic;
using KS.Kernel.Power;
using KS.Kernel.Time;
using KS.Kernel.Time.Renderers;
using KS.Shell.ShellBase.Scripting;
using Terminaux.Colors;
using KS.Kernel;
using KS.Files.Operations.Querying;

namespace KS.Misc.Text.Probers.Placeholder
{
    /// <summary>
    /// Placeholder parsing module
    /// </summary>
    public static class PlaceParse
    {
        private readonly static Dictionary<string, Func<string>> customPlaceholders = new();
        private readonly static Dictionary<string, Func<string>> placeholders = new()
        {
            { "<user>",                             () => UserManagement.CurrentUser.Username },
            { "<ftpuser>",                          () => FTPShellCommon.FtpUser },
            { "<ftpaddr>",                          () => FTPShellCommon.FtpSite },
            { "<currentftpdirectory>",              () => FTPShellCommon.FtpCurrentRemoteDir },
            { "<currentftplocaldirectory>",         () => FTPShellCommon.FtpCurrentDirectory },
            { "<currentftplocaldirectoryname>",     () => !string.IsNullOrEmpty(FTPShellCommon.FtpCurrentDirectory) ? new DirectoryInfo(FTPShellCommon.FtpCurrentDirectory).Name : ""},
            { "<sftpuser>",                         () => SFTPShellCommon.SFTPUser },
            { "<sftpaddr>",                         () => SFTPShellCommon.SFTPSite },
            { "<currentsftpdirectory>",             () => SFTPShellCommon.SFTPCurrentRemoteDir },
            { "<currentsftplocaldirectory>",        () => SFTPShellCommon.SFTPCurrDirect },
            { "<currentsftplocaldirectoryname>",    () => !string.IsNullOrEmpty(SFTPShellCommon.SFTPCurrDirect) ? new DirectoryInfo(SFTPShellCommon.SFTPCurrDirect).Name : ""},
            { "<mailuser>",                         () => MailLogin.Mail_Authentication.UserName },
            { "<mailaddr>",                         () => MailLogin.Mail_Authentication.Domain },
            { "<currentmaildirectory>",             () => MailShellCommon.IMAP_CurrentDirectory },
            { "<host>",                             () => NetworkTools.HostName },
            { "<currentdirectory>",                 () => CurrentDirectory.CurrentDir },
            { "<currentdirectoryname>",             () => !string.IsNullOrEmpty(CurrentDirectory.CurrentDir) ? new DirectoryInfo(CurrentDirectory.CurrentDir).Name : ""},
            { "<shortdate>",                        () => TimeDateRenderers.RenderDate(FormatType.Short) },
            { "<longdate>",                         () => TimeDateRenderers.RenderDate(FormatType.Long) },
            { "<shorttime>",                        () => TimeDateRenderers.RenderTime(FormatType.Short) },
            { "<longtime>",                         () => TimeDateRenderers.RenderTime(FormatType.Long) },
            { "<date>",                                   TimeDateRenderers.RenderDate },
            { "<time>",                                   TimeDateRenderers.RenderTime },
            { "<timezone>",                         () => TimeZoneInfo.Local.StandardName },
            { "<summertimezone>",                   () => TimeZoneInfo.Local.DaylightName },
            { "<system>",                                 Environment.OSVersion.ToString },
            { "<newline>",                          () => CharManager.NewLine },
            { "<dollar>",                                 UserManagement.GetUserDollarSign },
            { "<randomfile>",                             Getting.GetRandomFileName },
            { "<randomfolder>",                           Getting.GetRandomFolderName },
            { "<rid>",                                    KernelPlatform.GetCurrentRid },
            { "<ridgeneric>",                             KernelPlatform.GetCurrentGenericRid },
            { "<termemu>",                                KernelPlatform.GetTerminalEmulator },
            { "<termtype>",                               KernelPlatform.GetTerminalType },
            { "<f:reset>",                          () => KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground },
            { "<b:reset>",                          () => KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground },
            { "<uptime>",                           () => PowerManager.KernelUptime }
        };

        /// <summary>
        /// Probes the placeholders found in string
        /// </summary>
        /// <param name="text">Specified string</param>
        /// <param name="ThrowIfFailure">Throw if the placeholder parsing fails</param>
        /// <returns>A string that has the parsed placeholders</returns>
        public static string ProbePlaces(string text, bool ThrowIfFailure = false)
        {
            EventsManager.FireEvent(EventType.PlaceholderParsing, text);
            try
            {
                // Parse the text for the following placeholders:
                DebugWriter.WriteDebug(DebugLevel.I, "Parsing text for placeholders...");

                // -> Common placeholders
                foreach (string placeholder in placeholders.Keys)
                {
                    if (text.Contains(placeholder))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "{0} placeholder found.", placeholder);
                        var action = GetPlaceholderAction(placeholder);
                        string result = action();
                        DebugWriter.WriteDebug(DebugLevel.I, "Result: {0}", result);
                        text = text.Replace(placeholder, result);
                    }
                }

                // -> Custom placeholders
                foreach (string placeholder in customPlaceholders.Keys)
                {
                    if (text.Contains(placeholder))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "{0} custom placeholder found.", placeholder);
                        var action = GetPlaceholderAction(placeholder);
                        string result = action();
                        DebugWriter.WriteDebug(DebugLevel.I, "Result: {0}", result);
                        text = text.Replace(placeholder, result);
                    }
                }

                // -> Foreground color placeholder
                if (text.Contains("<f:"))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Foreground color placeholder found.");
                    while (text.Contains("<f:"))
                    {
                        int StartForegroundIndex = text.IndexOf("<f:");
                        int EndForegroundIndex = text[StartForegroundIndex..].IndexOf(">");
                        string SequenceSubstring = text.Substring(StartForegroundIndex, EndForegroundIndex + 1);
                        string PlainSequence = SequenceSubstring[3..^1];
                        string VTSequence = new Color(PlainSequence).VTSequenceForeground;
                        text = text.Replace(SequenceSubstring, VTSequence);
                    }
                }

                // -> Background color placeholder
                if (text.Contains("<b:"))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Background color placeholder found.");
                    while (text.Contains("<b:"))
                    {
                        int StartBackgroundIndex = text.IndexOf("<b:");
                        int EndBackgroundIndex = text[StartBackgroundIndex..].IndexOf(">");
                        string SequenceSubstring = text.Substring(StartBackgroundIndex, EndBackgroundIndex + 1);
                        string PlainSequence = SequenceSubstring[3..^1];
                        string VTSequence = new Color(PlainSequence).VTSequenceBackground;
                        text = text.Replace(SequenceSubstring, VTSequence);
                    }
                }

                // -> UESH variable placeholder
                if (text.Contains("<$"))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "UESH variable placeholder found.");
                    while (text.Contains("<$"))
                    {
                        int StartShellVariableIndex = text.IndexOf("<$");
                        int EndShellVariableIndex = text[StartShellVariableIndex..].IndexOf(">");
                        string ShellVariableSubstring = text.Substring(StartShellVariableIndex, EndShellVariableIndex + 1);
                        string PlainShellVariable = ShellVariableSubstring[1..^1];
                        text = text.Replace(ShellVariableSubstring, UESHVariables.GetVariable(PlainShellVariable));
                    }
                }

                // If successful, raise the parsed event
                EventsManager.FireEvent(EventType.PlaceholderParsed, text);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse placeholder {0}: {1}", text, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                EventsManager.FireEvent(EventType.PlaceholderParseError, text, ex);
                if (ThrowIfFailure)
                    throw new KernelException(KernelExceptionType.InvalidPlaceholder, Translate.DoTranslation("Error trying to parse placeholders. {0}"), ex.Message);
            }
            return text;
        }

        /// <summary>
        /// Registers a custom placeholder
        /// </summary>
        /// <param name="placeholder">Custom placeholder to register</param>
        /// <param name="placeholderAction">Action associated with the placeholder</param>
        public static void RegisterCustomPlaceholder(string placeholder, Func<string> placeholderAction)
        {
            // Sanity checks
            if (string.IsNullOrEmpty(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, Translate.DoTranslation("Placeholder may not be null"));
            if (placeholderAction is null)
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, Translate.DoTranslation("Placeholder action may not be null"));
            if (!placeholder.StartsWith('<') || !placeholder.EndsWith('>'))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, Translate.DoTranslation("Placeholder must satisfy this format") + ": <place>");

            // Try to register
            if (!customPlaceholders.ContainsKey(placeholder))
                customPlaceholders.Add(placeholder, placeholderAction);
        }

        /// <summary>
        /// Unregisters a custom placeholder
        /// </summary>
        /// <param name="placeholder">Custom placeholder to unregister</param>
        public static void UnregisterCustomPlaceholder(string placeholder)
        {
            // Sanity checks
            if (string.IsNullOrEmpty(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, Translate.DoTranslation("Placeholder may not be null"));
            if (!IsPlaceholderRegistered(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, Translate.DoTranslation("Placeholder action may not be null"));
            if (!placeholder.StartsWith('<') || !placeholder.EndsWith('>'))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, Translate.DoTranslation("Placeholder must satisfy this format") + ": <place>");

            // Try to unregister
            customPlaceholders.Remove(placeholder);
        }

        /// <summary>
        /// Checks to see whether the placeholder is built in
        /// </summary>
        /// <param name="placeholder">Placeholder to query</param>
        /// <returns>True if the placeholder is in the list of built-in placeholders</returns>
        /// <exception cref="KernelException"></exception>
        public static bool IsPlaceholderBuiltin(string placeholder)
        {
            // Sanity checks
            if (string.IsNullOrEmpty(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, Translate.DoTranslation("Placeholder may not be null"));
            if (!placeholder.StartsWith('<') || !placeholder.EndsWith('>'))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, Translate.DoTranslation("Placeholder must satisfy this format") + ": <place>");

            // Check to see if we have this placeholder
            return placeholders.ContainsKey(placeholder);
        }

        /// <summary>
        /// Checks to see whether the placeholder is registered
        /// </summary>
        /// <param name="placeholder">Placeholder to query</param>
        /// <returns>True if the placeholder is in the list of placeholders</returns>
        /// <exception cref="KernelException"></exception>
        public static bool IsPlaceholderRegistered(string placeholder)
        {
            // Sanity checks
            if (string.IsNullOrEmpty(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, Translate.DoTranslation("Placeholder may not be null"));
            if (!placeholder.StartsWith('<') || !placeholder.EndsWith('>'))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, Translate.DoTranslation("Placeholder must satisfy this format") + ": <place>");

            // Check to see if we have this placeholder
            return IsPlaceholderBuiltin(placeholder) || customPlaceholders.ContainsKey(placeholder);
        }

        /// <summary>
        /// Gets a placeholder action from the placeholder name
        /// </summary>
        /// <param name="placeholder">Placeholder to query</param>
        public static Func<string> GetPlaceholderAction(string placeholder)
        {
            // Sanity checks
            if (string.IsNullOrEmpty(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, Translate.DoTranslation("Placeholder may not be null"));
            if (!IsPlaceholderRegistered(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, Translate.DoTranslation("Placeholder action may not be null"));
            if (!placeholder.StartsWith('<') || !placeholder.EndsWith('>'))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, Translate.DoTranslation("Placeholder must satisfy this format") + ": <place>");

            // Try to register
            return
                IsPlaceholderBuiltin(placeholder) ?
                placeholders[placeholder] :
                customPlaceholders[placeholder];
        }
    }
}