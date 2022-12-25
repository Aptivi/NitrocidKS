﻿
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

using KS.Drivers.RNG;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class TestRNG : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests the random number generator");
        public override void Run()
        {
            int randomNum = RandomDriver.Random();
            double randomDbl = RandomDriver.RandomDouble();
            TextWriterColor.Write(Translate.DoTranslation("Returned numbers:") + $" [{randomNum} | {randomDbl}]");
        }
    }
}
