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
using KS.Files;
using KS.Files.Operations;
using KS.Files.Operations.Querying;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;

namespace KS.Misc.Text.Probers.Motd
{
    /// <summary>
    /// Message of the Day After Login (MAL) parsing module
    /// </summary>
    public static class MalParse
    {
        private static string malMessage;

        /// <summary>
        /// MAL file path
        /// </summary>
        public static string MalFilePath =>
            Config.MainConfig.MalFilePath;

        /// <summary>
        /// Current MAL message
        /// </summary>
        public static string MalMessage
        {
            get => malMessage ?? Translate.DoTranslation("Enjoy your day") + ", <user>!";
            set => malMessage = value ?? Translate.DoTranslation("Enjoy your day") + ", <user>!";
        }

        /// <summary>
        /// Initializes the MAL if the file isn't found.
        /// </summary>
        public static void InitMal()
        {
            if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.MAL)))
                SetMal(Translate.DoTranslation("Enjoy your day") + ", <user>!");
        }

        /// <summary>
        /// Sets the MAL
        /// </summary>
        /// <param name="Message">A message of the day after login</param>
        public static void SetMal(string Message)
        {
            try
            {
                // Get the MOTD file path
                Config.MainConfig.MalFilePath = FilesystemTools.NeutralizePath(MalFilePath);
                DebugWriter.WriteDebug(DebugLevel.I, "Path: {0}", MalFilePath);

                // Set the message
                MalMessage = Message;
                Writing.WriteContentsText(MalFilePath, Message);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.MOTD, Translate.DoTranslation("Error when trying to set MAL: {0}"), ex.Message);
            }
        }

        /// <summary>
        /// Reads the message of the day before/after login
        /// </summary>
        public static void ReadMal()
        {
            try
            {
                // Get the MAL file path
                Config.MainConfig.MalFilePath = FilesystemTools.NeutralizePath(MalFilePath);
                DebugWriter.WriteDebug(DebugLevel.I, "Path: {0}", MalFilePath);

                // Read the message
                InitMal();
                MalMessage = Reading.ReadContentsText(MalFilePath);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.MOTD, Translate.DoTranslation("Error when trying to get MAL: {0}"), ex.Message);
            }
        }

    }
}
