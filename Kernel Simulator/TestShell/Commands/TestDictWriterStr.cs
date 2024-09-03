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

using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
using Terminaux.Writer.ConsoleWriters;
namespace KS.TestShell.Commands
{
    class Test_TestDictWriterStrCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var NormalStringDict = new Dictionary<string, string>() { { "One", "String 1" }, { "Two", "String 2" }, { "Three", "String 3" } };
            var ArrayStringDict = new Dictionary<string, string[]>() { { "One", new string[] { "String 1", "String 2", "String 3" } }, { "Two", new string[] { "String 1", "String 2", "String 3" } }, { "Three", new string[] { "String 1", "String 2", "String 3" } } };
            TextWriters.Write(Translate.DoTranslation("Normal string dictionary:"), true, KernelColorTools.ColTypes.Neutral);
            ListWriterColor.WriteList(NormalStringDict);
            TextWriters.Write(Translate.DoTranslation("Array string dictionary:"), true, KernelColorTools.ColTypes.Neutral);
            ListWriterColor.WriteList(ArrayStringDict);
        }

    }
}