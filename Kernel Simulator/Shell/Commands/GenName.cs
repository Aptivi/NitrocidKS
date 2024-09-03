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

using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
using Terminaux.Writer.ConsoleWriters;
using Textify.Data.Analysis.NameGen;

namespace KS.Shell.Commands
{
    class GenNameCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            int NamesCount = 10;
            string NamePrefix = "";
            string NameSuffix = "";
            string SurnamePrefix = "";
            string SurnameSuffix = "";
            string[] NamesList;
            if (ListArgsOnly.Length >= 1)
                NamesCount = int.Parse(ListArgsOnly[0]);
            if (ListArgsOnly.Length >= 2)
                NamePrefix = ListArgsOnly[1];
            if (ListArgsOnly.Length >= 3)
                NameSuffix = ListArgsOnly[2];
            if (ListArgsOnly.Length >= 4)
                SurnamePrefix = ListArgsOnly[3];
            if (ListArgsOnly.Length >= 5)
                SurnameSuffix = ListArgsOnly[4];

            // Generate n names
            NameGenerator.PopulateNames();
            NamesList = NameGenerator.GenerateNames(NamesCount, NamePrefix, NameSuffix, SurnamePrefix, SurnameSuffix);
            ListWriterColor.WriteList(NamesList);
        }

    }
}