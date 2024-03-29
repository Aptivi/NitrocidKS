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

using Nitrocid.Languages;
using Nitrocid.Modifications;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class BlacklistMod : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Adds a mod to the blacklist");
        public override TestSection TestSection => TestSection.Modification;
        public override bool TestInteractive => false;
        public override object TestExpectedValue => true;
        public override void Run(params string[] args)
        {
            ModManager.AddModToBlacklist("MaliciousMod");
            TestActualValue = ModManager.GetBlacklistedMods().Contains("MaliciousMod");
        }
    }
}
