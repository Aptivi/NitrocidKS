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
using GitCommand = LibGit2Sharp.Commands;
using LibGit2Sharp;
using KS.Kernel.Time;
using KS.Kernel.Time.Renderers;

namespace Nitrocid.Extras.GitShell.Git.Commands
{
    /// <summary>
    /// Pull all updates from the server
    /// </summary>
    /// <remarks>
    /// This command pulls all updates from the server.
    /// </remarks>
    class Git_PullCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            if (!GitShellCommon.isIdentified)
            {
                TextWriterColor.Write(Translate.DoTranslation("You need to identify yourself before using this command. Use") + " 'setid' " + Translate.DoTranslation("to identify yourself."), true, KernelColorType.Error);
                return 14;
            }
            var merger = new Signature(GitShellCommon.name, GitShellCommon.email, new(TimeDateTools.KernelDateTime, TimeZoneRenderers.ShowTimeZoneUtcOffsetLocal()));
            var pullOptions = new PullOptions();
            var pullResult = GitCommand.Pull(GitShellCommon.Repository, merger, pullOptions);
            switch (pullResult.Status)
            {
                case MergeStatus.UpToDate:
                    TextWriterColor.Write(Translate.DoTranslation("Your local copy of the repo is up to date!"));
                    break;
                case MergeStatus.FastForward:
                    TextWriterColor.Write(Translate.DoTranslation("Fast forwarded to") + $":");
                    TextWriterColor.Write($"  {pullResult.Commit.Sha[..7]}: {pullResult.Commit.MessageShort}", true, KernelColorType.ListValue);
                    break;
                case MergeStatus.NonFastForward:
                    TextWriterColor.Write(Translate.DoTranslation("Updated repository to") + $":");
                    TextWriterColor.Write($"  {pullResult.Commit.Sha[..7]}: {pullResult.Commit.MessageShort}", true, KernelColorType.ListValue);
                    break;
                case MergeStatus.Conflicts:
                    TextWriterColor.Write(Translate.DoTranslation("Your merge resulted in conflicts. Please resolve any of the conflicts."), true, KernelColorType.Warning);
                    break;
            }
            return 0;
        }

    }
}
