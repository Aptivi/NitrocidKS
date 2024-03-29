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

using Terminaux.Inputs.Presentation.Elements;
using Terminaux.Inputs.Presentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Nitrocid.Tests.Misc.Presentation
{

    [TestClass]
    public class PresentationTests
    {

        /// <summary>
        /// Tests building the presentation
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void BuildPresentation()
        {
            // Presentation...
            var presentation = new Slideshow("MyPresentation",
            [
                new PresentationPage("Page one",
                [
                    new TextElement()
                    {
                        Arguments = ["Hello, world!"],
                        InvokeAction = () => Console.WriteLine("Invoke action")
                    }
                ]),
                new PresentationPage("Page two",
                [
                    new InputElement()
                    {
                        Arguments = ["Hello, world!"],
                        InvokeActionInput = (obj) => Console.WriteLine(obj.ToString())
                    }
                ]),
            ]);

            // General info
            presentation.ShouldNotBeNull();
            presentation.Name.ShouldBe("MyPresentation");
            presentation.Pages.Count.ShouldBe(2);

            // Individual pages
            var pageOne = presentation.Pages[0];
            var pageTwo = presentation.Pages[1];

            // Checking their correctness...
            pageOne.ShouldNotBeNull();
            pageTwo.ShouldNotBeNull();
            pageOne.Name.ShouldBe("Page one");
            pageTwo.Name.ShouldBe("Page two");
            pageOne.Elements.ShouldNotBeNull();
            pageTwo.Elements.ShouldNotBeNull();
            pageOne.Elements.ShouldNotBeEmpty();
            pageTwo.Elements.ShouldNotBeEmpty();

            // Individual elements
            var elementForPageOne = pageOne.Elements[0];
            var elementForPageTwo = pageTwo.Elements[0];

            // Checking their correctness...
            elementForPageOne.ShouldNotBeNull();
            elementForPageTwo.ShouldNotBeNull();
            elementForPageOne.IsInput.ShouldBeFalse();
            elementForPageTwo.IsInput.ShouldBeTrue();
            elementForPageOne.Arguments.ShouldNotBeNull();
            elementForPageTwo.Arguments.ShouldNotBeNull();
            elementForPageOne.Arguments.ShouldNotBeEmpty();
            elementForPageTwo.Arguments.ShouldNotBeEmpty();
            elementForPageOne.InvokeAction.ShouldNotBeNull();
            elementForPageTwo.InvokeActionInput.ShouldNotBeNull();

            // Verification...
            PresentationTools.PresentationContainsInput(presentation).ShouldBeTrue();
        }

    }

}
