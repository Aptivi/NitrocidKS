﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Journaling;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Shell.Shells.Admin.Commands
{
    /// <summary>
    /// Gets the current kernel journal log
    /// </summary>
    /// <remarks>
    /// This command gets the current kernel journal log from the <see cref="KernelPathType.Journaling"/> path.
    /// </remarks>
    class JournalCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length > 0)
            {
                // Check to see if invalid number is provided
                if (!int.TryParse(parameters.ArgumentsList[0], out int sessionNum))
                {
                    TextWriters.Write(Translate.DoTranslation("Session number is invalid."), KernelColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Journaling);
                }
                var entries = JournalManager.GetJournalEntries(sessionNum);
                JournalManager.PrintJournalLog(entries);
            }
            else
                JournalManager.PrintJournalLog();
            return 0;
        }
    }
}
