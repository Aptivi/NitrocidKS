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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using LibGit2Sharp;
using GitCommand = LibGit2Sharp.Commands;
using System.Linq;

namespace Nitrocid.Extras.GitShell.Git.Commands
{
    /// <summary>
    /// Check out a branch
    /// </summary>
    /// <remarks>
    /// This command lets you checkout a branch in your Git repository.
    /// </remarks>
    class Git_CheckoutCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var status = GitShellCommon.Repository.RetrieveStatus();

            // Check to see if the repo has been modified
            if (status.IsDirty)
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Save your work first by creating a commit before checking out a branch."), true, KernelColorType.Error);
                return 9;
            }

            // Check for existence
            var branches = GitShellCommon.Repository.Branches;
            var branchFriendlyNames = branches.Select((branch) => branch.FriendlyName).ToArray();
            var branchCanonNames = branches.Select((branch) => branch.CanonicalName).ToArray();
            string requestedBranch = parameters.ArgumentsList[0];
            if (!branchFriendlyNames.Contains(requestedBranch) && !branchCanonNames.Contains(requestedBranch))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Branch doesn't exist.") + $" {requestedBranch}", true, KernelColorType.Error);
                return 10;
            }

            // Now, checkout the branch.
            string canonCheckout = branchCanonNames.First((branchName) => branchName.Contains(requestedBranch));
            var branch = branches.First((branch) => branch.CanonicalName == canonCheckout);
            GitCommand.Checkout(GitShellCommon.Repository, branch);
            GitShellCommon.branchName = GitShellCommon.Repository.Head.CanonicalName;
            return 0;
        }

    }
}