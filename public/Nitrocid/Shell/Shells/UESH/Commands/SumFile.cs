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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers;
using KS.Drivers.Encryption;
using KS.Files;
using KS.Files.Operations.Querying;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Calculates the sum of a file
    /// </summary>
    /// <remarks>
    /// Calculating the hash sum of files is important, because it lets users verify if the file is corrupt or not. It calculates the sum of a file using either the MD5, SHA1, SHA256, or SHA512 algorithms.
    /// </remarks>
    class SumFileCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string file = FilesystemTools.NeutralizePath(parameters.ArgumentsList[1]);
            string @out = "";
            bool UseRelative = parameters.SwitchesList.Contains("-relative");
            var FileBuilder = new StringBuilder();
            if (!(parameters.ArgumentsList.Length < 3))
            {
                @out = FilesystemTools.NeutralizePath(parameters.ArgumentsList[2]);
            }
            if (Checking.FileExists(file))
            {
                if (DriverHandler.IsRegistered(DriverTypes.Encryption, parameters.ArgumentsList[0]))
                {
                    // Time when you're on a breakpoint is counted
                    var spent = new Stopwatch();
                    spent.Start();
                    string encrypted = Encryption.GetEncryptedFile(file, parameters.ArgumentsList[0]);
                    TextWriterColor.Write(encrypted);
                    TextWriterColor.Write(Translate.DoTranslation("Time spent: {0} milliseconds"), spent.ElapsedMilliseconds);
                    if (UseRelative)
                    {
                        FileBuilder.AppendLine($"- {parameters.ArgumentsList[1]}: {encrypted} ({parameters.ArgumentsList[0]})");
                    }
                    else
                    {
                        FileBuilder.AppendLine($"- {file}: {encrypted} ({parameters.ArgumentsList[0]})");
                    }
                    spent.Stop();
                }
                else
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Invalid encryption algorithm."), true, KernelColorType.Error);
                    return 10000 + (int)KernelExceptionType.Encryption;
                }
                if (!string.IsNullOrEmpty(@out))
                {
                    var FStream = new StreamWriter(@out);
                    FStream.Write(FileBuilder.ToString());
                    FStream.Flush();
                }
                return 0;
            }
            else
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("{0} is not found."), true, KernelColorType.Error, file);
                return 10000 + (int)KernelExceptionType.Encryption;
            }
        }

    }
}