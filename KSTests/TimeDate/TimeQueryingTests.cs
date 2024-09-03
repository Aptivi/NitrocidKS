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
using NUnit.Framework;
using Shouldly;

namespace KSTests.TimeDate
{

    [TestFixture]
    public class TimeQueryingTests
    {

        /// <summary>
        /// Tests getting remaining time from now
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetRemainingTimeFromNow()
        {
            string RemainingTime = KS.TimeDate.TimeDate.GetRemainingTimeFromNow(1000);
            RemainingTime.ShouldNotBeNullOrEmpty();
            RemainingTime.ShouldBe("0.00:00:01.000");
        }

    }
}