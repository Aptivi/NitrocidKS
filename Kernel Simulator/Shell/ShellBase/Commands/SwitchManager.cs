﻿
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Collections.Generic;

namespace KS.Shell.ShellBase.Commands
{
    /// <summary>
    /// Switch management module
    /// </summary>
    public static class SwitchManager
    {
        /// <summary>
        /// Gets the switch values
        /// </summary>
        /// <returns>The list of switches with values supplied</returns>
        public static List<(string, string)> GetSwitchValues(string[] switches)
        {
            List<(string, string)> switchValues = new();

            // Iterate through switches and check to see if there is an equal sign after the switch name
            foreach (string @switch in switches)
            {
                // Check the index to see if there is an equal sign
                int switchIndex = @switch.IndexOf('=');
                if (switchIndex == -1)
                    continue;

                // Check to see if the equal sign is at the end
                if (switchIndex == @switch.Length - 1)
                    continue;

                // Get switch name and value
                string switchName = @switch.Substring(0, switchIndex);
                string switchValue = @switch.Substring(switchIndex + 1);

                // Add the values to the list
                switchValues.Add((switchName, switchValue));
            }

            // Return the final result
            return switchValues;
        }
    }
}
