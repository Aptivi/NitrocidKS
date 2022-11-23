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

using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using System.Collections.Generic;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class TestListWriterInt : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests the list writer with the integer and integer array");
        public override void Run()
        {
            var NormalIntegerList = new List<int>() { 1, 2, 3 };
            var ArrayIntegerList = new List<int[]>() { { new int[] { 1, 2, 3 } }, { new int[] { 1, 2, 3 } }, { new int[] { 1, 2, 3 } } };
            TextWriterColor.Write(Translate.DoTranslation("Normal integer list:"));
            ListWriterColor.WriteList(NormalIntegerList);
            TextWriterColor.Write(Translate.DoTranslation("Array integer list:"));
            ListWriterColor.WriteList(ArrayIntegerList);
        }
    }
}
