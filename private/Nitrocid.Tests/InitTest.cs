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

using System;
using System.IO;
using NUnit.Framework;
using Nitrocid.Kernel;
using Nitrocid.Files;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Files.Operations;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Modifications;

namespace Nitrocid.Tests
{
    [SetUpFixture]
    public class InitTest
    {
        internal static string PathToTestSlotFolder = "";

        /// <summary>
        /// Initialize everything that is required before starting unit tests
        /// </summary>
        [OneTimeSetUp]
        public static void ReadyEverything()
        {
            if (!Checking.FileExists(PathsManagement.GetKernelPath(KernelPathType.Configuration)))
            {
                // Check to see if we have an appdata folder for KS
                if (!Checking.FolderExists(PathsManagement.AppDataPath))
                    Making.MakeDirectory(PathsManagement.AppDataPath, false);

                // Now, create config
                Config.CreateConfig();
            }
            else
            {
                if (!Checking.FileExists(PathsManagement.GetKernelPath(KernelPathType.Configuration) + ".old"))
                    File.Move(PathsManagement.GetKernelPath(KernelPathType.Configuration), PathsManagement.GetKernelPath(KernelPathType.Configuration) + ".old");
                Config.CreateConfig();
            }
            Config.ReadConfig(Config.MainConfig, PathsManagement.GetKernelPath(KernelPathType.Configuration));

            // NUnit sets current directory to a wrong directory, so set it to the test context directory
            string TestAssemblyDir = TestContext.CurrentContext.TestDirectory;
            Environment.CurrentDirectory = TestAssemblyDir;
            PathToTestSlotFolder = Path.GetFullPath("FilesystemSlot");
            PathToTestSlotFolder = FilesystemTools.NeutralizePath(PathToTestSlotFolder);

            // Make a slot for filesystem-related tests
            if (!Checking.FolderExists(PathToTestSlotFolder))
                Making.MakeDirectory(PathToTestSlotFolder, false);

            // Enable debugging
            string debugPath = TestAssemblyDir + "/UnitTestDebug.log";
            DebugWriter.DebugPath = debugPath;
            KernelEntry.DebugMode = true;

            // Load necessary addons for testing
            AddonTools.ProcessAddons(ModLoadPriority.Important);
            AddonTools.ProcessAddons(ModLoadPriority.Optional);
        }

        /// <summary>
        /// Clean up everything that the unit tests made
        /// </summary>
        [OneTimeTearDown]
        public static void CleanEverything()
        {
            if (Checking.FolderExists(Path.GetFullPath("ResultSlot")))
                Removing.RemoveDirectory(Path.GetFullPath("ResultSlot"));
            Directory.Move(PathToTestSlotFolder, Path.GetFullPath("ResultSlot"));
            if (Checking.FileExists(PathsManagement.GetKernelPath(KernelPathType.Configuration) + ".old"))
            {
                if (Checking.FileExists(PathsManagement.GetKernelPath(KernelPathType.Configuration)))
                    File.Delete(PathsManagement.GetKernelPath(KernelPathType.Configuration));
                File.Move(PathsManagement.GetKernelPath(KernelPathType.Configuration) + ".old", PathsManagement.GetKernelPath(KernelPathType.Configuration));
            }
        }
    }
}
