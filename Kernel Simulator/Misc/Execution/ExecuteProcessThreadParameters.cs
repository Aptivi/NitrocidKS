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

namespace KS.Misc.Execution
{
    /// <summary>
    /// Thread parameters for ExecuteProcess()
    /// </summary>
    internal class ExecuteProcessThreadParameters
    {
        /// <summary>
        /// Full path to file
        /// </summary>
        internal string File;
        /// <summary>
        /// Arguments, if any
        /// </summary>
        internal string Args;

        internal ExecuteProcessThreadParameters(string File, string Args)
        {
            this.File = File;
            this.Args = Args;
        }
    }
}
