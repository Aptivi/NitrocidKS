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

using System.Diagnostics;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Encryption;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;

namespace KS.TestShell.Commands
{
    class Test_TestCRC32Command : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var spent = new Stopwatch();
            spent.Start(); // Time when you're on a breakpoint is counted
            TextWriters.Write(Encryption.GetEncryptedString(ListArgs[0], Encryption.Algorithms.CRC32), true, KernelColorTools.ColTypes.Neutral);
            TextWriters.Write(Translate.DoTranslation("Time spent: {0} milliseconds"), true, KernelColorTools.ColTypes.Neutral, spent.ElapsedMilliseconds);
            spent.Stop();
        }

    }
}