//
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
using KS.Files;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Probers
{
    public static class MOTDParse
    {

        // Variables
        public static string MOTDFilePath = Paths.GetKernelPath(KernelPathType.MOTD);
        public static string MALFilePath = Paths.GetKernelPath(KernelPathType.MAL);

        /// <summary>
        /// Types of message
        /// </summary>
        public enum MessageType : int
        {
            /// <summary>
            /// MOTD (Message of the Day) message
            /// </summary>
            MOTD = 1,
            /// <summary>
            /// MAL (MOTD After Login) message
            /// </summary>
            MAL
        }

        /// <summary>
        /// Sets the Message of the Day or MAL
        /// </summary>
        /// <param name="Message">A message of the day before/after login</param>
        /// <param name="MType">Message type</param>
        public static void SetMOTD(string Message, MessageType MType)
        {
            try
            {
                System.IO.StreamWriter MOTDStreamW;

                // Get the MOTD and MAL file path
                MOTDFilePath = Filesystem.NeutralizePath(MOTDFilePath);
                MALFilePath = Filesystem.NeutralizePath(MALFilePath);
                DebugWriter.Wdbg(DebugLevel.I, "Paths: {0}, {1}", MOTDFilePath, MALFilePath);
                DebugWriter.Wdbg(DebugLevel.I, "Message type: {0}", MType);

                // Set the message according to message type
                if (MType == MessageType.MOTD)
                {
                    MOTDStreamW = new System.IO.StreamWriter(MOTDFilePath) { AutoFlush = true };
                    DebugWriter.Wdbg(DebugLevel.I, "Opened stream to MOTD path");
                    MOTDStreamW.WriteLine(Message);
                    Kernel.Kernel.MOTDMessage = Message;
                }
                else if (MType == MessageType.MAL)
                {
                    MOTDStreamW = new System.IO.StreamWriter(MALFilePath) { AutoFlush = true };
                    DebugWriter.Wdbg(DebugLevel.I, "Opened stream to MAL path");
                    MOTDStreamW.Write(Message);
                    Kernel.Kernel.MAL = Message;
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.W, "MOTD/MAL is valid, but the message type is not valid. Assuming MOTD...");
                    MOTDStreamW = new System.IO.StreamWriter(MOTDFilePath) { AutoFlush = true };
                    DebugWriter.Wdbg(DebugLevel.I, "Opened stream to MOTD path");
                    MOTDStreamW.WriteLine(Message);
                    Kernel.Kernel.MOTDMessage = Message;
                }

                // Close the message stream
                MOTDStreamW.Close();
                DebugWriter.Wdbg(DebugLevel.I, "Stream closed");
            }
            catch (Exception ex)
            {
                DebugWriter.WStkTrc(ex);
                throw new MOTDException(Translate.DoTranslation("Error when trying to set MOTD/MAL: {0}"), ex.Message);
            }
        }

        /// <summary>
        /// Reads the message of the day before/after login
        /// </summary>
        /// <param name="MType">Message type</param>
        public static void ReadMOTD(MessageType MType)
        {
            try
            {
                System.IO.StreamReader MOTDStreamR;
                var MOTDBuilder = new System.Text.StringBuilder();

                // Get the MOTD and MAL file path
                MOTDFilePath = Filesystem.NeutralizePath(MOTDFilePath);
                MALFilePath = Filesystem.NeutralizePath(MALFilePath);
                DebugWriter.Wdbg(DebugLevel.I, "Paths: {0}, {1}", MOTDFilePath, MALFilePath);
                DebugWriter.Wdbg(DebugLevel.I, "Message type: {0}", MType);

                // Read the message according to message type
                if (MType == MessageType.MOTD)
                {
                    MOTDStreamR = new System.IO.StreamReader(MOTDFilePath);
                    DebugWriter.Wdbg(DebugLevel.I, "Opened stream to MOTD path");
                    MOTDBuilder.Append(MOTDStreamR.ReadToEnd());
                    Kernel.Kernel.MOTDMessage = MOTDBuilder.ToString();
                    MOTDStreamR.Close();
                    DebugWriter.Wdbg(DebugLevel.I, "Stream closed");
                }
                else if (MType == MessageType.MAL)
                {
                    MOTDStreamR = new System.IO.StreamReader(MALFilePath);
                    DebugWriter.Wdbg(DebugLevel.I, "Opened stream to MAL path");
                    MOTDBuilder.Append(MOTDStreamR.ReadToEnd());
                    Kernel.Kernel.MAL = MOTDBuilder.ToString();
                    MOTDStreamR.Close();
                    DebugWriter.Wdbg(DebugLevel.I, "Stream closed");
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.W, "MOTD/MAL is valid, but the message type is not valid.");
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WStkTrc(ex);
                throw new MOTDException(Translate.DoTranslation("Error when trying to get MOTD/MAL: {0}"), ex.Message);
            }
        }
    }
}
