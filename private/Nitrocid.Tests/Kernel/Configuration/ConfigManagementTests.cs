﻿//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Kernel.Configuration;
using KS.Kernel.Configuration.Instances;
using NUnit.Framework;
using Shouldly;
using System.Linq;

namespace Nitrocid.Tests.Kernel.Configuration
{
    [TestFixture]
    public class ConfigManagementTests
    {
        /// <summary>
        /// Tests checking the three settings instance variables
        /// </summary>
        [Test]
        [TestCase(nameof(KernelMainConfig), nameof(KernelMainConfig))]
        [TestCase(nameof(KernelSaverConfig), nameof(KernelSaverConfig))]
        [Description("Management")]
        public void TestCheckSettingsInstances(string type, string expectedType)
        {
            var instance = Config.GetKernelConfig(type);
            instance.ShouldNotBeNull();
            instance.GetType().Name.ShouldBe(expectedType);
        }

        /// <summary>
        /// Tests checking settings resources (shallow)
        /// </summary>
        [Test]
        [TestCase(nameof(KernelMainConfig))]
        [TestCase(nameof(KernelSaverConfig))]
        [Description("Management")]
        public void TestCheckSettingsResourcesShallowGet(string type)
        {
            // Shallow
            var config = Config.GetKernelConfig(type);
            config.ShouldNotBeNull();
            var res = config.SettingsEntries;
            res.ShouldNotBeNull();
            res.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests checking settings resources (deep)
        /// </summary>
        [Test]
        [TestCase(nameof(KernelMainConfig))]
        [TestCase(nameof(KernelSaverConfig))]
        [Description("Management")]
        public void TestCheckSettingsResourcesDeepGet(string type)
        {
            // Shallow
            var config = Config.GetKernelConfig(type);
            config.ShouldNotBeNull();
            var res = config.SettingsEntries;
            res.ShouldNotBeNull();
            res.ShouldNotBeEmpty();

            // Deep
            foreach (var entry in res)
            {
                entry.ShouldNotBeNull();
                entry.Keys.ShouldNotBeNull();
                entry.Keys.ShouldNotBeEmpty();
                foreach (var key in entry.Keys)
                    key.ShouldNotBeNull();
            }
        }

        /// <summary>
        /// Tests checking settings resources (deep with evaluation)
        /// </summary>
        [Test]
        [TestCase(nameof(KernelMainConfig))]
        [TestCase(nameof(KernelSaverConfig))]
        [Description("Management")]
        public void TestCheckSettingsResourcesDeepEvalGet(string type)
        {
            // Shallow
            var config = Config.GetKernelConfig(type);
            config.ShouldNotBeNull();
            var res = config.SettingsEntries;
            res.ShouldNotBeNull();
            res.ShouldNotBeEmpty();

            // Deep
            foreach (var entry in res)
            {
                entry.ShouldNotBeNull();
                entry.Keys.ShouldNotBeNull();
                entry.Keys.ShouldNotBeEmpty();
                foreach (var key in entry.Keys)
                {
                    key.ShouldNotBeNull();
                    var value = ConfigTools.GetValueFromEntry(key, config);
                    value.ShouldNotBeNull();
                    value.ShouldNotBe("Unknown");
                }
            }
        }

        /// <summary>
        /// Tests checking the settings variables
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestCheckConfigVariables()
        {
            var SettingsVariables = ConfigTools.CheckConfigVariables();
            SettingsVariables.ShouldNotBeNull();
            SettingsVariables.ShouldNotBeEmpty();
            SettingsVariables.Where((kvp) => !kvp.Value).ShouldBeEmpty();
        }
    }
}
