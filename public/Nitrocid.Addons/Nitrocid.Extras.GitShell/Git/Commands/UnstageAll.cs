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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using LibGit2Sharp;
using GitCommand = LibGit2Sharp.Commands;
using System;

namespace Nitrocid.Extras.GitShell.Git.Commands
{
    /// <summary>
    /// Stages all unstaged files
    /// </summary>
    /// <remarks>
    /// This command lets you stage all unstaged files in your Git repository.
    /// </remarks>
    class UnstageAllCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var status = GitShellCommon.Repository.RetrieveStatus();

            // Check to see if the repo has been modified
            if (!status.IsDirty)
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("No modifications are done to unstage."), true, KernelColorType.Success);
                return 0;
            }

            // Stage all unstaged changes...
            var modified = status.Staged;
            foreach (var item in modified)
            {
                try
                {
                    GitCommand.Unstage(GitShellCommon.Repository, item.FilePath);
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Unstaged file {0} successfully!"), true, KernelColorType.Success, item.FilePath);
                }
                catch (Exception ex)
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Failed to unstage file {0}.") + "{1}", true, KernelColorType.Error, item.FilePath, ex.Message);
                }
            }
            return 0;
        }

    }
}
