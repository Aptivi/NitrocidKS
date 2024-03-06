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

using Nitrocid.Files.Operations;
using Nitrocid.Files.Paths;
using Nitrocid.Users.Login.Motd;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Nitrocid.Misc.Text.Probers.Placeholder;
using System.IO;

namespace Nitrocid.Tests.Misc.Probers
{

    [TestClass]
    public class MOTDManagementTests
    {

        /// <summary>
        /// Tests setting MOTD
        /// </summary>
        [TestMethod]
        [Description("Setting")]
        public void TestInitMOTD()
        {
            MotdParse.SetMotd(PlaceParse.ProbePlaces("Hello, I am on <system>"));
            var MOTDFile = new StreamReader(PathsManagement.GetKernelPath(KernelPathType.MOTD));
            MOTDFile.ReadLine().ShouldBe(PlaceParse.ProbePlaces("Hello, I am on <system>"));
            MOTDFile.Close();
        }

        /// <summary>
        /// Tests setting MAL
        /// </summary>
        [TestMethod]
        [Description("Setting")]
        public void TestInitMAL()
        {
            MalParse.SetMal(PlaceParse.ProbePlaces("Hello, I am on <system>"));
            var MALFile = new StreamReader(PathsManagement.GetKernelPath(KernelPathType.MAL));
            MALFile.ReadLine().ShouldBe(PlaceParse.ProbePlaces("Hello, I am on <system>"));
            MALFile.Close();
        }

        /// <summary>
        /// Tests reading MOTD from file
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestReadMOTDFromFile()
        {
            MotdParse.ReadMotd();
            string MOTDLine = Reading.ReadContentsText(PathsManagement.GetKernelPath(KernelPathType.MOTD));
            MOTDLine.ShouldBe(MotdParse.MotdMessage);
        }

        /// <summary>
        /// Tests reading MAL from file
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestReadMALFromFile()
        {
            MalParse.ReadMal();
            string MALLine = Reading.ReadContentsText(PathsManagement.GetKernelPath(KernelPathType.MAL));
            MALLine.ShouldBe(MalParse.MalMessage);
        }

    }
}
