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
using System.Linq;

namespace Nitrocid.Extras.GitShell.Git.Commands
{
    /// <summary>
    /// Git repository status
    /// </summary>
    /// <remarks>
    /// This command prints a Git repository status.
    /// </remarks>
    class Git_StatusCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var status = GitShellCommon.Repository.RetrieveStatus();
            TextWriterColor.Write(Translate.DoTranslation("Status for branch {0}..."), GitShellCommon.BranchName);

            // Check to see if the repo has been modified
            if (!status.IsDirty)
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("No modifications are done."), true, KernelColorType.Success);
                return 0;
            }

            // Show all the statuses starting from untracked...
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Untracked files") + ":", true, KernelColorType.ListEntry);
            if (status.Untracked.Any())
            {
                foreach (var item in status.Untracked)
                    TextWriterColor.WriteKernelColor("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("No untracked files."), true, KernelColorType.ListValue);
            TextWriterColor.Write();

            // ...added...
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Added files") + ":", true, KernelColorType.ListEntry);
            if (status.Added.Any())
            {
                foreach (var item in status.Added)
                    TextWriterColor.WriteKernelColor("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("No added files."), true, KernelColorType.ListValue);
            TextWriterColor.Write();

            // ...modified...
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Modified files") + ":", true, KernelColorType.ListEntry);
            if (status.Modified.Any())
            {
                foreach (var item in status.Modified)
                    TextWriterColor.WriteKernelColor("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("No modified files."), true, KernelColorType.ListValue);
            TextWriterColor.Write();

            // ...removed...
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Removed files") + ":", true, KernelColorType.ListEntry);
            if (status.Removed.Any())
            {
                foreach (var item in status.Removed)
                    TextWriterColor.WriteKernelColor("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("No removed files."), true, KernelColorType.ListValue);
            TextWriterColor.Write();

            // ...staged...
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Staged files") + ":", true, KernelColorType.ListEntry);
            if (status.Staged.Any())
            {
                foreach (var item in status.Staged)
                    TextWriterColor.WriteKernelColor("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("No staged files."), true, KernelColorType.ListValue);
            TextWriterColor.Write();

            // ...renamed...
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Renamed staged files") + ":", true, KernelColorType.ListEntry);
            if (status.RenamedInIndex.Any())
            {
                foreach (var item in status.RenamedInIndex)
                    TextWriterColor.WriteKernelColor("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("No renamed staged files."), true, KernelColorType.ListValue);
            TextWriterColor.Write();

            // ...renamed unstaged...
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Renamed files") + ":", true, KernelColorType.ListEntry);
            if (status.RenamedInWorkDir.Any())
            {
                foreach (var item in status.RenamedInWorkDir)
                    TextWriterColor.WriteKernelColor("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("No renamed files."), true, KernelColorType.ListValue);
            TextWriterColor.Write();

            // ...and missing
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Missing files") + ":", true, KernelColorType.ListEntry);
            if (status.Missing.Any())
            {
                foreach (var item in status.Missing)
                    TextWriterColor.WriteKernelColor("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("No missing files."), true, KernelColorType.ListValue);
            TextWriterColor.Write();
            return 0;
        }

    }
}