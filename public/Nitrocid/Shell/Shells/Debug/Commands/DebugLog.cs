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
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;
using KS.Files;
using KS.Files.Folders;
using System.IO;

namespace KS.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can view the debug log of a session
    /// </summary>
    /// <remarks>
    /// This command lets you view the detailed debug log of a session by printing the contents of the whole file directly to the console. You'll most likely need to wrap the output.
    /// </remarks>
    class Debug_DebugLogCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            // Try to parse the session number.
            string sessionNumStr = ListArgsOnly[0];
            if (!int.TryParse(sessionNumStr, out int sessionNum))
            {
                // There is invalid session number being requested
                TextWriterColor.Write(Translate.DoTranslation("Invalid session number") + $" {sessionNumStr}", true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Debug;
            }

            // Now, check to see if we have this session number. Get all the debug logs and compare.
            string TargetPath = Paths.GetKernelPath(KernelPathType.Debugging);
            TargetPath = TargetPath[..TargetPath.LastIndexOf(".log")] + "*.log";
            string[] debugs = Listing.GetFilesystemEntries(TargetPath);
            string finalDebug = "";
            foreach (string debug in debugs)
            {
                // Don't try to parse the rest of the files if we already have one.
                if (!string.IsNullOrEmpty(finalDebug))
                    break;

                // Check the debug path and compare it with the requested session number.
                if (debug.Contains($"kernelDbg-{sessionNum}.log"))
                    finalDebug = debug;
            }

            // Check to see if we really have the file path
            if (string.IsNullOrEmpty(finalDebug))
            {
                // There is no such session number being requested
                TextWriterColor.Write(Translate.DoTranslation("No such session number") + $" {sessionNumStr}", true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Debug;
            }

            // Get the contents of the file and write it to the console
            string contents = File.ReadAllText(finalDebug);
            TextWriterColor.Write(contents);
            return 0;
        }

    }
}
