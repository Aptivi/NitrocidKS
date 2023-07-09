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

using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using KS.Files;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.Shells.Archive;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;

namespace KS.Misc.Archive
{
    /// <summary>
    /// Archive shell tools
    /// </summary>
    public static class ArchiveTools
    {

        /// <summary>
        /// Lists all entries according to the target directory or the current directory
        /// </summary>
        /// <param name="Target">Target directory in an archive</param>
        public static List<IArchiveEntry> ListArchiveEntries(string Target)
        {
            if (string.IsNullOrWhiteSpace(Target))
                Target = ArchiveShellCommon.ArchiveShell_CurrentArchiveDirectory;
            var Entries = new List<IArchiveEntry>();
            foreach (IArchiveEntry ArchiveEntry in ArchiveShellCommon.ArchiveShell_Archive?.Entries)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Parsing entry {0}...", ArchiveEntry.Key);
                if (Target is not null)
                {
                    if (ArchiveEntry.Key.StartsWith(Target))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Entry {0} found in target {1}. Adding...", ArchiveEntry.Key, Target);
                        Entries.Add(ArchiveEntry);
                    }
                }
                else if (Target is null)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Adding entry {0}...", ArchiveEntry.Key);
                    Entries.Add(ArchiveEntry);
                }
            }
            DebugWriter.WriteDebug(DebugLevel.I, "Entries: {0}", Entries.Count);
            return Entries;
        }

        /// <summary>
        /// Extracts an entry to a target directory
        /// </summary>
        /// <param name="Target">Target file in an archive</param>
        /// <param name="Where">Where in the local filesystem to extract?</param>
        /// <param name="FullTargetPath">Whether to use the full target path</param>
        public static bool ExtractFileEntry(string Target, string Where, bool FullTargetPath = false)
        {
            if (string.IsNullOrWhiteSpace(Target))
                throw new KernelException(KernelExceptionType.Archive, Translate.DoTranslation("Can't extract nothing."));
            if (string.IsNullOrWhiteSpace(Where))
                Where = ArchiveShellCommon.ArchiveShell_CurrentDirectory;

            // Define absolute target
            string AbsoluteTarget = ArchiveShellCommon.ArchiveShell_CurrentArchiveDirectory + "/" + Target;
            if (AbsoluteTarget.StartsWith("/"))
                AbsoluteTarget = AbsoluteTarget[1..];
            DebugWriter.WriteDebug(DebugLevel.I, "Target: {0}, AbsoluteTarget: {1}", Target, AbsoluteTarget);

            // Define local destination while getting an entry from target
            string LocalDestination = Where + "/";
            var ArchiveEntry = ArchiveShellCommon.ArchiveShell_Archive.Entries.Where(x => x.Key == AbsoluteTarget).ToArray()[0];
            if (FullTargetPath)
            {
                LocalDestination += ArchiveEntry.Key;
            }
            DebugWriter.WriteDebug(DebugLevel.I, "Where: {0}", LocalDestination);

            // Try to extract file
            Directory.CreateDirectory(LocalDestination);
            Making.MakeFile(LocalDestination + ArchiveEntry.Key);
            ArchiveShellCommon.ArchiveShell_FileStream.Seek(0L, SeekOrigin.Begin);
            var ArchiveReader = ReaderFactory.Open(ArchiveShellCommon.ArchiveShell_FileStream);
            while (ArchiveReader.MoveToNextEntry())
            {
                if (ArchiveReader.Entry.Key == ArchiveEntry.Key & !ArchiveReader.Entry.IsDirectory)
                {
                    ArchiveReader.WriteEntryToFile(LocalDestination + ArchiveEntry.Key);
                }
            }
            return true;
        }

        /// <summary>
        /// Packs a local file to the archive
        /// </summary>
        /// <param name="Target">Target local file</param>
        /// <param name="Where">Where in the archive to extract?</param>
        public static bool PackFile(string Target, string Where)
        {
            if (string.IsNullOrWhiteSpace(Target))
                throw new KernelException(KernelExceptionType.Archive, Translate.DoTranslation("Can't pack nothing."));
            if (string.IsNullOrWhiteSpace(Where))
                Where = ArchiveShellCommon.ArchiveShell_CurrentDirectory;
            if (ArchiveShellCommon.ArchiveShell_Archive is not IWritableArchive)
                throw new KernelException(KernelExceptionType.Archive, Translate.DoTranslation("Archive is not writable because type is") + " {0}.", ArchiveShellCommon.ArchiveShell_Archive.Type);

            // Define absolute archive target
            string ArchiveTarget = ArchiveShellCommon.ArchiveShell_CurrentArchiveDirectory + "/" + Target;
            if (ArchiveTarget.StartsWith("/"))
                ArchiveTarget = ArchiveTarget[1..];
            DebugWriter.WriteDebug(DebugLevel.I, "Where: {0}, ArchiveTarget: {1}", Where, ArchiveTarget);

            // Select compression type
            CompressionType compression = CompressionType.None;
            switch (ArchiveShellCommon.ArchiveShell_Archive.Type)
            {
                case ArchiveType.Zip:
                    compression = CompressionType.Deflate;
                    break;
            }

            // Define local destination while getting an entry from target
            Target = Filesystem.NeutralizePath(Target, Where);
            DebugWriter.WriteDebug(DebugLevel.I, "Where: {0}", Target);
            ((IWritableArchive)ArchiveShellCommon.ArchiveShell_Archive).AddEntry(ArchiveTarget, Target);
            ((IWritableArchive)ArchiveShellCommon.ArchiveShell_Archive).SaveTo(ArchiveShellCommon.ArchiveShell_FileStream, new WriterOptions(compression));
            return true;
        }

        /// <summary>
        /// Changes the working archive directory
        /// </summary>
        /// <param name="Target">Target directory</param>
        public static bool ChangeWorkingArchiveDirectory(string Target)
        {
            if (string.IsNullOrWhiteSpace(Target))
                Target = ArchiveShellCommon.ArchiveShell_CurrentArchiveDirectory;

            // Check to see if we're going back
            if (Target.Contains(".."))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Target contains going back. Counting...");
                var CADSplit = ArchiveShellCommon.ArchiveShell_CurrentArchiveDirectory.Split('/').ToList();
                var TargetSplit = Target.Split('/').ToList();
                var CADBackSteps = 0;

                // Add back steps if target is ".."
                DebugWriter.WriteDebug(DebugLevel.I, "Target length: {0}", TargetSplit.Count);
                for (int i = 0; i <= TargetSplit.Count - 1; i++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Target part {0}: {1}", i, TargetSplit[i]);
                    if (TargetSplit[i] == "..")
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Target is going back. Adding step...");
                        CADBackSteps += 1;
                        TargetSplit[i] = "";
                        DebugWriter.WriteDebug(DebugLevel.I, "Steps: {0}", CADBackSteps);
                    }
                }

                // Remove empty strings
                TargetSplit.RemoveAll(x => string.IsNullOrEmpty(x));
                DebugWriter.WriteDebug(DebugLevel.I, "Target length: {0}", TargetSplit.Count);

                // Remove every last entry that goes back
                DebugWriter.WriteDebug(DebugLevel.I, "Old CADSplit length: {0}", CADSplit.Count);
                for (int Steps = CADBackSteps; Steps >= 1; Steps -= 1)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Current step: {0}", Steps);
                    DebugWriter.WriteDebug(DebugLevel.I, "Removing index {0} from CADSplit...", CADSplit.Count - Steps);
                    CADSplit.RemoveAt(CADSplit.Count - Steps);
                    DebugWriter.WriteDebug(DebugLevel.I, "New CADSplit length: {0}", CADSplit.Count);
                }

                // Set current archive directory and target
                ArchiveShellCommon.ArchiveShell_CurrentArchiveDirectory = string.Join("/", CADSplit);
                DebugWriter.WriteDebug(DebugLevel.I, "Setting CAD to {0}...", ArchiveShellCommon.ArchiveShell_CurrentArchiveDirectory);
                Target = string.Join("/", TargetSplit);
                DebugWriter.WriteDebug(DebugLevel.I, "Setting target to {0}...", Target);
            }

            // Prepare the target
            Target = ArchiveShellCommon.ArchiveShell_CurrentArchiveDirectory + "/" + Target;
            if (Target.StartsWith("/"))
                Target = Target[1..];
            DebugWriter.WriteDebug(DebugLevel.I, "Setting target to {0}...", Target);

            // Enumerate entries
            foreach (IArchiveEntry Entry in ListArchiveEntries(Target))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Entry: {0}", Entry.Key);
                if (Entry.Key.StartsWith(Target))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "{0} found ({1}). Changing...", Target, Entry.Key);
                    ArchiveShellCommon.ArchiveShell_CurrentArchiveDirectory = Entry.Key[..^1];
                    DebugWriter.WriteDebug(DebugLevel.I, "Setting CAD to {0}...", ArchiveShellCommon.ArchiveShell_CurrentArchiveDirectory);
                    return true;
                }
            }

            // Assume that we didn't find anything.
            DebugWriter.WriteDebug(DebugLevel.E, "{0} not found.", Target);
            return false;
        }

        /// <summary>
        /// Changes the working local directory
        /// </summary>
        /// <param name="Target">Target directory</param>
        public static bool ChangeWorkingArchiveLocalDirectory(string Target)
        {
            if (string.IsNullOrWhiteSpace(Target))
                Target = ArchiveShellCommon.ArchiveShell_CurrentDirectory;
            if (Checking.FolderExists(Filesystem.NeutralizePath(Target, ArchiveShellCommon.ArchiveShell_CurrentDirectory)))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "{0} found. Changing...", Target);
                ArchiveShellCommon.ArchiveShell_CurrentDirectory = Target;
                return true;
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "{0} not found.", Target);
                return false;
            }
        }

    }
}
