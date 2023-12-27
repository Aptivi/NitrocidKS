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

using System.IO;
using KS.Files;
using KS.Kernel;
using KS.Misc.Probers;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Misc
{

    [TestFixture]
    [Order(2)]
    public class MOTDManagementTests
    {

        /// <summary>
        /// Tests reading MOTD from file
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestReadMOTDFromFile()
        {
            MOTDParse.ReadMOTD(MOTDParse.MessageType.MOTD);
            string MOTDLine = File.ReadAllText(Paths.GetKernelPath(KernelPathType.MOTD));
            MOTDLine.ShouldBe(KS.Kernel.Kernel.MOTDMessage);
        }

        /// <summary>
        /// Tests reading MAL from file
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestReadMALFromFile()
        {
            MOTDParse.ReadMOTD(MOTDParse.MessageType.MAL);
            string MALLine = File.ReadAllText(Paths.GetKernelPath(KernelPathType.MAL));
            MALLine.ShouldBe(KS.Kernel.Kernel.MAL);
        }

    }
}
