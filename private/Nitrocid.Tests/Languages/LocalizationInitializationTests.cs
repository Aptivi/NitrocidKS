﻿
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Languages;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace Nitrocid.Tests.Languages
{

    [TestFixture]
    public class LocalizationInitializationTests
    {

        private readonly JObject localizationExample = JObject.Parse(
            """
            {
              "Name": "French",
              "Transliterable": false,
              "Localizations": {
                "Hello, world!": "Bonjour le monde !",
                "This is Nitrocid KS!": "C'est Nitrocid KS !"
              }
            }
            """
        );

        /// <summary>
        /// Tests creating the new instance of the language information
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestProbeLocalizations()
        {
            Dictionary<string, string> localizations = new();
            Should.NotThrow(() => localizations = LanguageManager.ProbeLocalizations(localizationExample));
            localizations.ShouldNotBeNull();
            localizations.ShouldNotBeEmpty();
            localizations["Hello, world!"].ShouldBe("Bonjour le monde !");
            localizations["This is Nitrocid KS!"].ShouldBe("C'est Nitrocid KS !");
        }

    }
}