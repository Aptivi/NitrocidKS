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
using System.Reflection;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Misc.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Textify.Figlet;
using Terminaux.Colors.Data;

namespace Nitrocid.Tests.Misc.Reflection
{

    [TestClass]
    public class PropertyManagementTests
    {

        public static string TryToChangeIt { get; set; } = "No.";

        /// <summary>
        /// Tests checking property
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestCheckProperty() =>
            PropertyManager.CheckProperty(nameof(KernelSaverConfig.MatrixBleedDelay)).ShouldBeTrue();

        /// <summary>
        /// Tests getting value
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetPropertyValueInstance()
        {
            string Value = Convert.ToString(PropertyManager.GetPropertyValueInstance(Config.SaverConfig, nameof(KernelSaverConfig.MatrixBleedDelay)));
            Value.ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests setting value
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestSetPropertyValueInstance()
        {
            PropertyManager.SetPropertyValueInstance(Config.SaverConfig, nameof(KernelSaverConfig.MatrixBleedDelay), 50);
            int Value = Convert.ToInt32(PropertyManager.GetPropertyValueInstance(Config.SaverConfig, nameof(KernelSaverConfig.MatrixBleedDelay)));
            Value.ShouldBe(50);
        }

        /// <summary>
        /// Tests getting value
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetPropertyValue()
        {
            string Value = Convert.ToString(PropertyManager.GetPropertyValue(nameof(TryToChangeIt), typeof(PropertyManagementTests)));
            Value.ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests setting value
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestSetPropertyValue()
        {
            PropertyManager.SetPropertyValue(nameof(TryToChangeIt), "Yes!", typeof(PropertyManagementTests));
            string Value = Convert.ToString(PropertyManager.GetPropertyValue(nameof(TryToChangeIt), typeof(PropertyManagementTests)));
            Value.ShouldBe("Yes!");
        }

        /// <summary>
        /// Tests getting variable
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetConfigProperty()
        {
            var PropertyInfo = PropertyManager.GetProperty(nameof(KernelSaverConfig.MatrixBleedDelay));
            PropertyInfo.Name.ShouldBe(nameof(KernelSaverConfig.MatrixBleedDelay));
        }

        /// <summary>
        /// Tests getting property
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetProperty()
        {
            var Property = PropertyManager.GetProperty(nameof(KernelMainConfig.CheckUpdateStart), typeof(KernelMainConfig));
            Property.ShouldNotBeNull();
            Property.Name.ShouldBe(nameof(KernelMainConfig.CheckUpdateStart));
            Property.DeclaringType.ShouldBe(typeof(KernelMainConfig));
        }

        /// <summary>
        /// Tests getting property
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetPropertyGeneral()
        {
            var Property = PropertyManager.GetPropertyGeneral(nameof(KernelMainConfig.CheckUpdateStart));
            Property.ShouldNotBeNull();
            Property.Name.ShouldBe(nameof(KernelMainConfig.CheckUpdateStart));
            Property.DeclaringType.ShouldBe(typeof(KernelMainConfig));
        }

        /// <summary>
        /// Tests getting properties
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetProperties()
        {
            var Properties = PropertyManager.GetProperties(typeof(Config));
            Properties.ShouldNotBeNull();
            Properties.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests getting properties
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetPropertiesNoEvaluation()
        {
            var Properties = PropertyManager.GetPropertiesNoEvaluation(typeof(KernelMainConfig));
            Properties.ShouldNotBeNull();
            Properties.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests getting properties
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetPropertiesInstance()
        {
            var Properties = PropertyManager.GetProperties(ConsoleColorData.Black, typeof(ConsoleColorData));
            Properties.ShouldNotBeNull();
            Properties.ShouldNotBeEmpty();
        }
    }
}
