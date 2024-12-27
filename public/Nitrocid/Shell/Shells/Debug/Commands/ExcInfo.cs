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

using System;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can view an exception info
    /// </summary>
    /// <remarks>
    /// This command lets you view the exception information from the list of available kernel exceptions, including the name, the number, and the message.
    /// </remarks>
    class ExcInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we really have the type
            string exceptionStr = parameters.ArgumentsList[0];
            if (!Enum.TryParse(exceptionStr, out KernelExceptionType type))
            {
                // There is no such exception number being requested
                TextWriters.Write(Translate.DoTranslation("No such exception type") + $" {exceptionStr}", true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Debug);
            }

            // Get the exception type and its message
            TextWriters.Write("- " + Translate.DoTranslation("Exception type name") + ": ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{type} [{(int)type}]", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Message") + ": ", false, KernelColorType.ListEntry);
            TextWriters.Write(KernelExceptionMessages.GetMessageFromType(type), true, KernelColorType.ListValue);
            return 0;
        }

    }
}
