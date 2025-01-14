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
using Newtonsoft.Json.Linq;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Network.Transfer;
using Nitrocid.Misc.Splash;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Drivers.Network;

#if SPECIFIERREL
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Configuration;
using System.IO;
#endif

namespace Nitrocid.Kernel.Updates
{
    /// <summary>
    /// Update management module
    /// </summary>
    public static class UpdateManager
    {

        /// <summary>
        /// Fetches the GitHub repo to see if there are any updates
        /// </summary>
        /// <param name="kind">The kind of update</param>
        /// <returns>A kernel update instance</returns>
        public static KernelUpdate? FetchKernelUpdates(UpdateKind kind)
        {
            try
            {
                // Create the HTTP client
                var client = BaseNetworkDriver.CreateHttpClient();

                // Because api.github.com requires the UserAgent header to be put, else, 403 error occurs. Fortunately for us, "Aptivi" is enough.
                client.DefaultRequestHeaders.Add("User-Agent", "Aptivi");

                // Populate the following variables with information
                string UpdateStr = NetworkTransfer.DownloadString("https://api.github.com/repos/Aptivi/Nitrocid/releases", false);
                var UpdateToken = JToken.Parse(UpdateStr);
                var UpdateInstance = new KernelUpdate(UpdateToken, kind);

                // Return the update instance
                return UpdateInstance;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to check for updates: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return null;
        }

        /// <summary>
        /// Fetches the GitHub repo to see if there are any updates
        /// </summary>
        /// <returns>A kernel update instance</returns>
        public static KernelUpdate? FetchBinaryArchive()
        {
            try
            {
                // Determine the update kind by fetching the addons
                var kind = UpdateKind.Binary;
                if (AddonTools.ListAddons().Count == 0)
                    kind = UpdateKind.BinaryLite;
                return FetchKernelUpdates(kind);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to check for updates: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return null;
        }

        /// <summary>
        /// Fetches the GitHub repo for addon pack
        /// </summary>
        /// <returns>A kernel update instance</returns>
        public static KernelUpdate? FetchAddonPack()
        {
            try
            {
                return FetchKernelUpdates(UpdateKind.Addons);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to check for updates: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return null;
        }

        /// <summary>
        /// Fetches the GitHub repo for changelogs
        /// </summary>
        /// <returns>A kernel update instance</returns>
        public static KernelUpdate? FetchChangelogs()
        {
            try
            {
                return FetchKernelUpdates(UpdateKind.Changelogs);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to check for updates: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return null;
        }

        /// <summary>
        /// Prompt for checking for kernel updates
        /// </summary>
        public static void CheckKernelUpdates()
        {
            // The LocaleClean analyzer-based cleaner reports false positives for extra strings that happen to be
            // translated in the compiler pre-processor directives, so we need to move all translations here to
            // avoid this happening again and for the locale tools to actually see them.
            string devVersionWarning = Translate.DoTranslation("Checking for updates is disabled because you're running a development version.");
            string packageManagerError = Translate.DoTranslation("You've installed Nitrocid KS using your package manager. Please use it to upgrade your kernel instead.");
            string checkFailed = Translate.DoTranslation("Failed to check for updates.");
            string checking = Translate.DoTranslation("Checking for system updates...");
            string newVersion = Translate.DoTranslation("Found new version: ");
            string downloaded = Translate.DoTranslation("Downloaded the update successfully!");
            string downloadManually = Translate.DoTranslation("You can download it at: ");
            string upToDate = Translate.DoTranslation("You're up to date!");
#if SPECIFIERREL && !PACKAGEMANAGERBUILD
            // Check for updates now
            SplashReport.ReportProgress(checking, 10);
            var AvailableUpdate = FetchBinaryArchive();
            if (AvailableUpdate is not null)
            {
                if (!AvailableUpdate.Updated && AvailableUpdate.UpdateVersion is not null)
                {
                    SplashReport.ReportProgress(newVersion, 10);
                    SplashReport.ReportProgress(AvailableUpdate.UpdateVersion.ToString(), 10);
                    if (Config.MainConfig.AutoDownloadUpdate)
                    {
                        NetworkTransfer.DownloadFile(AvailableUpdate.UpdateURL.ToString(), Path.Combine(PathsManagement.ExecPath, "update.zip"));
                        SplashReport.ReportProgress(downloaded, 10);
                    }
                    else
                    {
                        SplashReport.ReportProgress(downloadManually, 10);
                        SplashReport.ReportProgress(AvailableUpdate.UpdateURL.ToString(), 10);
                    }
                }
                else
                {
                    SplashReport.ReportProgress(upToDate, 10);
                }
            }
            else if (AvailableUpdate is null)
            {
                SplashReport.ReportProgressError(checkFailed);
            }
#elif PACKAGEMANAGERBUILD
            SplashReport.ReportProgressError(packageManagerError);
#else
            SplashReport.ReportProgressWarning(devVersionWarning);
#endif
        }

        /// <summary>
        /// Gets the changelogs for the current kernel version
        /// </summary>
        /// <returns>A string representing the changelogs for this kernel version</returns>
        public static string GetVersionChangelogs()
        {
            var changelogsUpdate = FetchChangelogs() ??
                throw new KernelException(KernelExceptionType.Network, Translate.DoTranslation("Can't fetch changelogs."));
            string changes = NetworkTransfer.DownloadString(changelogsUpdate.UpdateURL.ToString());
            return changes;
        }

    }
}
