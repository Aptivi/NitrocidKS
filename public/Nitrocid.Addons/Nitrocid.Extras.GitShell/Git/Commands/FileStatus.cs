﻿//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using System.IO;

namespace Nitrocid.Extras.GitShell.Git.Commands
{
    /// <summary>
    /// File status
    /// </summary>
    /// <remarks>
    /// This command prints a file status.
    /// </remarks>
    class FileStatusCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string file = parameters.ArgumentsList[0];
            var status = GitShellCommon.Repository.RetrieveStatus(file);
            TextWriterColor.Write(Translate.DoTranslation("Status for file") + $" {Path.GetFileName(file)}: {status}");
            return 0;
        }

    }
}
