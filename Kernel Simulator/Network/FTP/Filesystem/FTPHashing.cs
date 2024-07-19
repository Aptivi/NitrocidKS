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
using FluentFTP;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;
namespace KS.Network.FTP.Filesystem
{
    public static class FTPHashing
    {

        /// <summary>
        /// Gets a hash for file
        /// </summary>
        /// <param name="File">A file to be hashed</param>
        /// <param name="HashAlgorithm">A hash algorithm supported by the FTP server</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static FtpHash FTPGetHash(string File, FtpHashAlgorithm HashAlgorithm)
        {
            if (FTPShellCommon.FtpConnected == true)
            {
                if (!string.IsNullOrEmpty(File))
                {
                    if (FTPShellCommon.ClientFTP.FileExists(File))
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Hashing {0} using {1}...", File, HashAlgorithm.ToString());
                        return FTPShellCommon.ClientFTP.GetChecksum(File, HashAlgorithm);
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.E, "{0} is not found.", File);
                        throw new Kernel.Exceptions.FTPFilesystemException(Translate.DoTranslation("{0} is not found in the server."), File);
                    }
                }
                else
                {
                    throw new ArgumentNullException(File, Translate.DoTranslation("Enter a remote file to be hashed."));
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("You must connect to a server before performing this operation."));
            }
        }

        /// <summary>
        /// Gets a hash for files in a directory
        /// </summary>
        /// <param name="Directory">A directory for its contents to be hashed</param>
        /// <param name="HashAlgorithm">A hash algorithm supported by the FTP server</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static Dictionary<string, FtpHash> FTPGetHashes(string Directory, FtpHashAlgorithm HashAlgorithm)
        {
            return FTPGetHashes(Directory, HashAlgorithm, FTPShellCommon.FtpRecursiveHashing);
        }

        /// <summary>
        /// Gets a hash for files in a directory
        /// </summary>
        /// <param name="Directory">A directory for its contents to be hashed</param>
        /// <param name="HashAlgorithm">A hash algorithm supported by the FTP server</param>
        /// <param name="Recurse">Whether to hash the files within the subdirectories too.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static Dictionary<string, FtpHash> FTPGetHashes(string Directory, FtpHashAlgorithm HashAlgorithm, bool Recurse)
        {
            if (FTPShellCommon.FtpConnected == true)
            {
                if (!string.IsNullOrEmpty(Directory))
                {
                    if (FTPShellCommon.ClientFTP.DirectoryExists(Directory))
                    {
                        var Hashes = new Dictionary<string, FtpHash>();
                        FtpListItem[] Items;
                        if (Recurse)
                        {
                            Items = FTPShellCommon.ClientFTP.GetListing(Directory, FtpListOption.Recursive);
                        }
                        else
                        {
                            Items = FTPShellCommon.ClientFTP.GetListing(Directory);
                        }
                        foreach (FtpListItem Item in Items)
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Hashing {0} using {1}...", Item.FullName, HashAlgorithm.ToString());
                            Hashes.Add(Item.FullName, FTPGetHash(Item.FullName, HashAlgorithm));
                        }
                        return Hashes;
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.E, "{0} is not found.", Directory);
                        throw new Kernel.Exceptions.FTPFilesystemException(Translate.DoTranslation("{0} is not found in the server."), Directory);
                    }
                }
                else
                {
                    throw new ArgumentNullException(Directory, Translate.DoTranslation("Enter a remote directory."));
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("You must connect to a server before performing this operation."));
            }
        }

    }
}