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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Encryption;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class SumFileCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string file = Filesystem.NeutralizePath(ListArgsOnly[1]);
            string @out = "";
            bool UseRelative = ListSwitchesOnly.Contains("-relative");
            var FileBuilder = new StringBuilder();
            if (!(ListArgsOnly.Length < 3))
            {
                @out = Filesystem.NeutralizePath(ListArgsOnly[2]);
            }
            if (Checking.FileExists(file))
            {
                Encryption.Algorithms AlgorithmEnum;
                if (ListArgsOnly[0] == "all")
                {
                    foreach (string Algorithm in Enum.GetNames(typeof(Encryption.Algorithms)))
                    {
                        AlgorithmEnum = (Encryption.Algorithms)Convert.ToInt32(Enum.Parse(typeof(Encryption.Algorithms), Algorithm));
                        var spent = new Stopwatch();
                        spent.Start(); // Time when you're on a breakpoint is counted
                        string encrypted = Encryption.GetEncryptedFile(file, AlgorithmEnum);
                        TextWriters.Write("{0} ({1})", true, KernelColorTools.ColTypes.Neutral, encrypted, AlgorithmEnum);
                        TextWriters.Write(Translate.DoTranslation("Time spent: {0} milliseconds"), true, KernelColorTools.ColTypes.Neutral, spent.ElapsedMilliseconds);
                        if (UseRelative)
                        {
                            FileBuilder.AppendLine($"- {ListArgsOnly[1]}: {encrypted} ({AlgorithmEnum})");
                        }
                        else
                        {
                            FileBuilder.AppendLine($"- {file}: {encrypted} ({AlgorithmEnum})");
                        }
                        spent.Stop();
                    }
                }
                else if (Enum.TryParse(ListArgsOnly[0], out AlgorithmEnum))
                {
                    var spent = new Stopwatch();
                    spent.Start(); // Time when you're on a breakpoint is counted
                    string encrypted = Encryption.GetEncryptedFile(file, AlgorithmEnum);
                    TextWriters.Write(encrypted, true, KernelColorTools.ColTypes.Neutral);
                    TextWriters.Write(Translate.DoTranslation("Time spent: {0} milliseconds"), true, KernelColorTools.ColTypes.Neutral, spent.ElapsedMilliseconds);
                    if (UseRelative)
                    {
                        FileBuilder.AppendLine($"- {ListArgsOnly[1]}: {encrypted} ({AlgorithmEnum})");
                    }
                    else
                    {
                        FileBuilder.AppendLine($"- {file}: {encrypted} ({AlgorithmEnum})");
                    }
                    spent.Stop();
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Invalid encryption algorithm."), true, KernelColorTools.ColTypes.Error);
                }
                if (!string.IsNullOrEmpty(@out))
                {
                    var FStream = new StreamWriter(@out);
                    FStream.Write(FileBuilder.ToString());
                    FStream.Flush();
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("{0} is not found."), true, KernelColorTools.ColTypes.Error, file);
            }
        }

        public override void HelpHelper()
        {
            TextWriters.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, KernelColorTools.ColTypes.Neutral);
            TextWriters.Write("  -relative: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Uses relative path instead of absolute"), true, KernelColorTools.ColTypes.ListValue);
        }

    }
}
