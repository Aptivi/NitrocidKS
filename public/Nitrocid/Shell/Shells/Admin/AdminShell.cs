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

using System;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Users.Permissions;

namespace KS.Shell.Shells.Admin
{
    /// <summary>
    /// The admin shell
    /// </summary>
    public class AdminShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "AdminShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.OpenAdminShell);

            while (!Bail)
            {
                try
                {
                    // Prompt for the command
                    ShellManager.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    CancellationHandlers.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Error in administrative shell: {0}"), true, KernelColorType.Error, ex.Message);
                    DebugWriter.WriteDebug(DebugLevel.E, "Error: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
        }

    }
}
