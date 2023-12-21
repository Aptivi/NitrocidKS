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

using System;

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

using System.Runtime.InteropServices;
using KS.Misc.Writers.DebugWriters;
using KS.Resources;
using Newtonsoft.Json.Linq;

namespace KS.ConsoleBase.Colors
{
    public static class Color255
    {

        /// <summary>
        /// The 255 console colors data JSON token to get information about these colors
        /// </summary>
        public static readonly JToken ColorDataJson = JToken.Parse(KernelResources.ConsoleColorsData);

        /// <summary>
        /// [Windows] Sets console mode
        /// </summary>
        /// <param name="hConsoleHandle">Console Handle</param>
        /// <param name="mode">Mode</param>
        /// <returns>True if succeeded, false if failed</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

        /// <summary>
        /// [Windows] Gets console mode
        /// </summary>
        /// <param name="handle">Console handle</param>
        /// <param name="mode">Mode</param>
        /// <returns>True if succeeded, false if failed</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr handle, out int mode);

        /// <summary>
        /// [Windows] Gets console handle
        /// </summary>
        /// <param name="handle">Handle number</param>
        /// <returns>True if succeeded, false if failed</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int handle);

        /// <summary>
        /// [Windows] Initializes 255 color support
        /// </summary>
        public static void Initialize255()
        {
            var handle = GetStdHandle(-11);
            DebugWriter.Wdbg(DebugLevel.I, "Integer pointer {0}", handle);
            GetConsoleMode(handle, out int mode);
            DebugWriter.Wdbg(DebugLevel.I, "Mode: {0}", mode);
            if (!(mode == 7))
            {
                SetConsoleMode(handle, mode | 0x4);
                DebugWriter.Wdbg(DebugLevel.I, "Added support for VT escapes.");
            }
        }

        /// <summary>
        /// A simplification for <see cref="Convert.ToChar(int)"/> function to return the ESC character
        /// </summary>
        /// <returns>ESC</returns>
        public static char GetEsc()
        {
            return Convert.ToChar(0x1B);
        }

    }
}
