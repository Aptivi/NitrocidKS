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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.Encryption;
using Nitrocid.Languages;
using System.Diagnostics;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class TestSHA384 : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Encrypts a string using SHA384");
        public override TestSection TestSection => TestSection.Drivers;
        public override void Run(params string[] args)
        {
            // Time when you're on a breakpoint is counted
            var spent = new Stopwatch();
            spent.Start();
            TextWriterColor.Write(Encryption.GetEncryptedString("Nitrocid KS", "SHA384"));
            TextWriterColor.Write(Translate.DoTranslation("Time spent: {0} milliseconds"), spent.ElapsedMilliseconds);
            spent.Stop();
        }
    }
}
