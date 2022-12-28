﻿
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using System.IO;
using KS.ConsoleBase.Colors;
using KS.Drivers.Encryption;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Verifies the file
    /// </summary>
    /// <remarks>
    /// If you've previously calculated the hash sum of a file using the supported algorithms, and you have the expected hash or file that contains the hashes list, you can use this command to verify the sanity of the file.
    /// <br></br>
    /// It can handle three types:
    /// <br></br>
    /// <list type="bullet">
    /// <item>
    /// <term>First</term>
    /// <description>It can verify files by comparing expected hash with the actual hash.</description>
    /// </item>
    /// <item>
    /// <term>Second</term>
    /// <description>It can verify files by opening the hashes file that sumfiles generated and finding the hash of the file.</description>
    /// </item>
    /// <item>
    /// <term>Third</term>
    /// <description>It can verify files by opening the hashes file that some other tool generated and finding the hash of the file, assuming that it's in this format: <c>&lt;expected hash&gt; &lt;file name&gt;</c></description>
    /// </item>
    /// </list>
    /// <br></br>
    /// If the hashes match, that means that the file is not corrupted. However, if they don't match, the file is corrupted and must be redownloaded.
    /// <br></br>
    /// If you run across a hash file that verify can't parse, feel free to post an issue or make a PR.
    /// </remarks>
    class VerifyCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            try
            {
                string HashFile = Filesystem.NeutralizePath(ListArgsOnly[2]);
                if (Checking.FileExists(HashFile))
                {
                    if (HashVerifier.VerifyHashFromHashesFile(ListArgsOnly[3], ListArgsOnly[0], ListArgsOnly[2], ListArgsOnly[1]))
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Hashes match."));
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Hashes don't match."), true, KernelColorType.Warning);
                    }
                }
                else if (HashVerifier.VerifyHashFromHash(ListArgsOnly[3], ListArgsOnly[0], ListArgsOnly[2], ListArgsOnly[1]))
                {
                    TextWriterColor.Write(Translate.DoTranslation("Hashes match."));
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Hashes don't match."), true, KernelColorType.Warning);
                }
            }
            catch (KernelException ihae) when (ihae.ExceptionType == KernelExceptionType.InvalidHashAlgorithm)
            {
                DebugWriter.WriteDebugStackTrace(ihae);
                TextWriterColor.Write(Translate.DoTranslation("Invalid encryption algorithm."), true, KernelColorType.Error);
            }
            catch (KernelException ihe) when (ihe.ExceptionType == KernelExceptionType.InvalidHash)
            {
                DebugWriter.WriteDebugStackTrace(ihe);
                TextWriterColor.Write(Translate.DoTranslation("Hashes are malformed."), true, KernelColorType.Error);
            }
            catch (FileNotFoundException fnfe)
            {
                DebugWriter.WriteDebugStackTrace(fnfe);
                TextWriterColor.Write(Translate.DoTranslation("{0} is not found."), true, KernelColorType.Error, ListArgsOnly[3]);
            }
        }

    }
}
