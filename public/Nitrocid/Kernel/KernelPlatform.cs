﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.Files.Operations.Querying;
using Nitrocid.Kernel.Debugging;
using SpecProbe.Software.Kernel;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Nitrocid.Kernel
{
    /// <summary>
    /// Kernel platform query
    /// </summary>
    public static class KernelPlatform
    {

        /// <summary>
        /// Checks to see if the kernel is running normally, from GRILO, or from somewhere else
        /// </summary>
        public static bool IsOnUsualEnvironment() =>
            (Assembly.GetEntryAssembly().FullName == Assembly.GetExecutingAssembly().FullName && !IsRunningFromGrilo()) || IsRunningFromGrilo();

        /// <summary>
        /// Is this system a Windows system?
        /// </summary>
        /// <returns>True if running on Windows (Windows 10, Windows 11, etc.). Otherwise, false.</returns>
        public static bool IsOnWindows() =>
            Environment.OSVersion.Platform == PlatformID.Win32NT;

        /// <summary>
        /// Is this system a Unix system? True for macOS, too!
        /// </summary>
        /// <returns>True if running on Unix (Linux, *nix, etc.). Otherwise, false.</returns>
        public static bool IsOnUnix() =>
            Environment.OSVersion.Platform == PlatformID.Unix;

        /// <summary>
        /// Is this system a Unix system that contains musl libc?
        /// </summary>
        /// <returns>True if running on Unix systems that use musl libc. Otherwise, false.</returns>
        public static bool IsOnUnixMusl()
        {
            try
            {
                if (!IsOnUnix() || IsOnMacOS() || IsOnWindows())
                    return false;
                var gnuRel = gnuGetLibcVersion();
                return false;
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// Is this system a macOS system?
        /// </summary>
        /// <returns>True if running on macOS (MacBook, iMac, etc.). Otherwise, false.</returns>
        public static bool IsOnMacOS()
        {
            if (IsOnUnix())
            {
                string System = UnameManager.GetUname(UnameTypes.KernelName);
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to find \"Darwin\" in {0}...", System);
                return System.Contains("Darwin");
            }
            else
                return false;
        }

        /// <summary>
        /// Is this system an Android system?
        /// </summary>
        /// <returns>True if running on Android phones using Termux. Otherwise, false.</returns>
        public static bool IsOnAndroid()
        {
            if (IsOnUnix() && !IsOnMacOS())
                return Checking.FileExists("/system/build.prop");
            else
                return false;
        }

        /// <summary>
        /// Polls $TERM_PROGRAM to get terminal emulator
        /// </summary>
        public static string GetTerminalEmulator() =>
            Environment.GetEnvironmentVariable("TERM_PROGRAM") ?? "";

        /// <summary>
        /// Polls $TERM to get terminal type (vt100, dumb, ...)
        /// </summary>
        public static string GetTerminalType() =>
            Environment.GetEnvironmentVariable("TERM") ?? "";

        /// <summary>
        /// Is Nitrocid KS running from GRILO?
        /// </summary>
        public static bool IsRunningFromGrilo() =>
            (Assembly.GetEntryAssembly()?.GetName()?.Name?.StartsWith("GRILO")) ?? false;

        /// <summary>
        /// Is Nitrocid KS running from TMUX?
        /// </summary>
        public static bool IsRunningFromTmux() =>
            Environment.GetEnvironmentVariable("TMUX") is not null;

        /// <summary>
        /// Is Nitrocid KS running from GNU Screen?
        /// </summary>
        public static bool IsRunningFromScreen() =>
            Environment.GetEnvironmentVariable("STY") is not null;

        /// <summary>
        /// Gets the current runtime identifier
        /// </summary>
        /// <returns>Returns a runtime identifier (win-x64 for example).</returns>
        public static string GetCurrentRid() =>
            RuntimeInformation.RuntimeIdentifier;

        /// <summary>
        /// Gets the current runtime identifier
        /// </summary>
        /// <returns>Returns a runtime identifier (win-x64 for example).</returns>
        public static string GetCurrentGenericRid() =>
            $"{(IsOnWindows() ? "win" : IsOnMacOS() ? "osx" : IsOnUnix() ? "linux" : "freebsd")}-" +
            $"{(IsOnUnixMusl() ? "musl-" : "")}" +
            $"{RuntimeInformation.OSArchitecture.ToString().ToLower()}";

        #region Interop
        [DllImport("libc", EntryPoint = "gnu_get_libc_version")]
        private static extern nint gnuGetLibcVersion();
        #endregion

    }
}
