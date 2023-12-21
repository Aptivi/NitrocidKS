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
using System.Collections.Generic;
using KS.Languages;
using KS.Misc.Platform;

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

namespace KS.Files
{
    public static class Paths
    {

        /// <summary>
        /// Platform-dependent home path
        /// </summary>
        public static string HomePath
        {
            get
            {
                if (PlatformDetector.IsOnUnix())
                {
                    return Environment.GetEnvironmentVariable("HOME");
                }
                else
                {
                    return Environment.GetEnvironmentVariable("USERPROFILE").Replace(@"\", "/");
                }
            }
        }

        /// <summary>
        /// Platform-dependent temp path
        /// </summary>
        public static string TempPath
        {
            get
            {
                if (PlatformDetector.IsOnUnix())
                {
                    return "/tmp";
                }
                else
                {
                    return Environment.GetEnvironmentVariable("TEMP").Replace(@"\", "/");
                }
            }
        }

        /// <summary>
        /// Retro Kernel Simulator download path
        /// </summary>
        public static string RetroKSDownloadPath
        {
            get
            {
#if NETCOREAPP
				if (PlatformDetector.IsOnUnix())
				{
					return Environment.GetEnvironmentVariable("HOME") + "/.config/retroks/exec/coreclr";
				}
				else
				{
					return (Environment.GetEnvironmentVariable("LOCALAPPDATA") + "/RetroKS/exec/coreclr").Replace(@"\", "/");
				}
#else
                if (PlatformDetector.IsOnUnix())
                {
                    return Environment.GetEnvironmentVariable("HOME") + "/.config/retroks/exec/fx";
                }
                else
                {
                    return (Environment.GetEnvironmentVariable("LOCALAPPDATA") + "/RetroKS/exec/fx").Replace(@"\", "/");
                }
#endif
            }
        }

        // Variables
        internal static Dictionary<string, string> KernelPaths = [];

        /// <summary>
        /// Initializes the paths
        /// </summary>
        public static void InitPaths()
        {
            if (!KernelPaths.ContainsKey("Mods"))
                KernelPaths.Add("Mods", HomePath + "/KSMods/");
            if (!KernelPaths.ContainsKey("Configuration"))
                KernelPaths.Add("Configuration", HomePath + "/KernelConfig.json");
            if (!KernelPaths.ContainsKey("Debugging"))
                KernelPaths.Add("Debugging", HomePath + "/kernelDbg.log");
            if (!KernelPaths.ContainsKey("Aliases"))
                KernelPaths.Add("Aliases", HomePath + "/Aliases.json");
            if (!KernelPaths.ContainsKey("Users"))
                KernelPaths.Add("Users", HomePath + "/Users.json");
            if (!KernelPaths.ContainsKey("FTPSpeedDial"))
                KernelPaths.Add("FTPSpeedDial", HomePath + "/FTP_SpeedDial.json");
            if (!KernelPaths.ContainsKey("SFTPSpeedDial"))
                KernelPaths.Add("SFTPSpeedDial", HomePath + "/SFTP_SpeedDial.json");
            if (!KernelPaths.ContainsKey("DebugDevNames"))
                KernelPaths.Add("DebugDevNames", HomePath + "/DebugDeviceNames.json");
            if (!KernelPaths.ContainsKey("MOTD"))
                KernelPaths.Add("MOTD", HomePath + "/MOTD.txt");
            if (!KernelPaths.ContainsKey("MAL"))
                KernelPaths.Add("MAL", HomePath + "/MAL.txt");
            if (!KernelPaths.ContainsKey("CustomSaverSettings"))
                KernelPaths.Add("CustomSaverSettings", HomePath + "/CustomSaverSettings.json");
            if (!KernelPaths.ContainsKey("Events"))
                KernelPaths.Add("Events", HomePath + "/KSEvents/");
            if (!KernelPaths.ContainsKey("Reminders"))
                KernelPaths.Add("Reminders", HomePath + "/KSReminders/");
            if (!KernelPaths.ContainsKey("CustomLanguages"))
                KernelPaths.Add("CustomLanguages", HomePath + "/KSLanguages/");
            if (!KernelPaths.ContainsKey("CustomSplashes"))
                KernelPaths.Add("CustomSplashes", HomePath + "/KSSplashes/");
        }

        /// <summary>
        /// Gets the neutralized kernel path
        /// </summary>
        /// <param name="PathType">Kernel path type</param>
        /// <returns>A kernel path</returns>
        public static string GetKernelPath(KernelPathType PathType)
        {
            if (Enum.IsDefined(typeof(KernelPathType), PathType))
            {
                return Filesystem.NeutralizePath(KernelPaths[PathType.ToString()]);
            }
            else
            {
                throw new Kernel.Exceptions.InvalidKernelPathException(Translate.DoTranslation("Invalid kernel path type."));
            }
        }

    }
}
