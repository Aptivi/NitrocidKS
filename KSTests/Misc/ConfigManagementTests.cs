﻿//
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
using KS.Misc.Configuration;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Misc
{

    [TestFixture]
    public class ConfigManagementTests
    {

        /// <summary>
        /// Tests creates a new JSON object containing the kernel settings of all kinds
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetNewConfigObject()
        {
            Config.GetNewConfigObject().ShouldNotBeNull();
        }

        /// <summary>
        /// Tests config repair (Actually, it checks to see if any of the config entries is missing. If any one of them is missing, unit test fails.)
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestRepairConfig()
        {
            ConfigTools.RepairConfig().ShouldBeFalse();
        }

        /// <summary>
        /// Tests getting a config category
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetConfigCategoryStandard()
        {
            ConfigTools.GetConfigCategory(Config.ConfigCategory.General).ShouldNotBeNull();
        }

        /// <summary>
        /// Tests getting a config category with a sub-category
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetConfigCategoryWithSubcategory()
        {
            ConfigTools.GetConfigCategory(Config.ConfigCategory.Screensaver, "Matrix").ShouldNotBeNull();
        }

        /// <summary>
        /// Tests setting the value of an entry in a category
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestSetConfigValueAndWriteStandard()
        {
            var Token = ConfigTools.GetConfigCategory(Config.ConfigCategory.General);
            ConfigTools.SetConfigValue(Config.ConfigCategory.General, Token, "Prompt for Arguments on Boot", true);
            Token["Prompt for Arguments on Boot"].ToObject<bool>().ShouldBeTrue();
        }

        /// <summary>
        /// Tests setting the value of an entry in a category with the sub-category
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestSetConfigValueAndWriteWithSubcategory()
        {
            var Token = ConfigTools.GetConfigCategory(Config.ConfigCategory.Screensaver, "Matrix");
            ConfigTools.SetConfigValue(Config.ConfigCategory.Screensaver, Token, "Delay in Milliseconds", 2);
            Token["Delay in Milliseconds"].ToObject<int>().ShouldBe(2);
        }

        /// <summary>
        /// Tests checking the settings variables
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestCheckSettingsVariables()
        {
            var SettingsVariables = SettingsApp.CheckSettingsVariables();
            SettingsVariables.ShouldNotBeNull();
            SettingsVariables.ShouldNotBeEmpty();
            SettingsVariables.Values.ShouldNotContain(false);
        }

    }
}