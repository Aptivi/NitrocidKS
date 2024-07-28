//
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

using SpecProbe.Software.Platform;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;

namespace KS.Login
{
    /// <summary>
    /// User tools for Windows platforms
    /// </summary>
    public static class WindowsUserTools
    {
        /// <summary>
        /// Checks to see if the current user is an administrator
        /// </summary>
        [SuppressMessage("CA1416", "CA1416")]
        [SuppressMessage("CodeQuality", "IDE0079")]
        public static bool IsAdministrator()
        {
            if (PlatformHelper.IsOnWindows())
            {
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            else
                // Assume that the user is admin for other systems.
                return true;
        }
    }
}
