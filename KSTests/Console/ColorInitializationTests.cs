

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

using KS.ConsoleBase.Colors;
using NUnit.Framework;
using Shouldly;
using System;
using Terminaux.Colors;

namespace KSTests
{

	[TestFixture]
	public class ColorInitializationTests
	{

		/// <summary>
		/// Tests initializing color instance from 255 colors
		/// </summary>
		[Test]
		[Description("Initialization")]
		public void TestInitializeColorInstanceFrom255Colors()
		{
			// Create instance
			var ColorInstance = new Color(18);

			// Check for null
			ColorInstance.ShouldNotBeNull();
			ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
			ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
			ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

			// Check for property correctness
			ColorInstance.PlainSequence.ShouldBe("18");
			ColorInstance.Type.ShouldBe(ColorType._255Color);
			ColorInstance.VTSequenceBackground.ShouldBe(Convert.ToString(Color255.GetEsc()) + "[48;5;18m");
			ColorInstance.VTSequenceForeground.ShouldBe(Convert.ToString(Color255.GetEsc()) + "[38;5;18m");
			ColorInstance.R.ShouldBe(0);
			ColorInstance.G.ShouldBe(0);
			ColorInstance.B.ShouldBe(135);
			ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
			ColorInstance.Hex.ShouldBe("#000087");
		}

		/// <summary>
		/// Tests initializing color instance from true color
		/// </summary>
		[Test]
		[Description("Initialization")]
		public void TestInitializeColorInstanceFromTrueColor()
		{
			// Create instance
			var ColorInstance = new Color("94;0;63");

			// Check for null
			ColorInstance.ShouldNotBeNull();
			ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
			ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
			ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

			// Check for property correctness
			ColorInstance.PlainSequence.ShouldBe("94;0;63");
			ColorInstance.Type.ShouldBe(ColorType.TrueColor);
			ColorInstance.VTSequenceBackground.ShouldBe(Convert.ToString(Color255.GetEsc()) + "[48;2;94;0;63m");
			ColorInstance.VTSequenceForeground.ShouldBe(Convert.ToString(Color255.GetEsc()) + "[38;2;94;0;63m");
			ColorInstance.R.ShouldBe(94);
			ColorInstance.G.ShouldBe(0);
			ColorInstance.B.ShouldBe(63);
			ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
			ColorInstance.Hex.ShouldBe("#5E003F");
		}

		/// <summary>
		/// Tests initializing color instance from true color
		/// </summary>
		[Test]
		[Description("Initialization")]
		public void TestInitializeColorInstanceFromHex()
		{
			// Create instance
			var ColorInstance = new Color("#0F0F0F");

			// Check for null
			ColorInstance.ShouldNotBeNull();
			ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
			ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
			ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

			// Check for property correctness
			ColorInstance.PlainSequence.ShouldBe("15;15;15");
			ColorInstance.Type.ShouldBe(ColorType.TrueColor);
			ColorInstance.VTSequenceBackground.ShouldBe(Convert.ToString(Color255.GetEsc()) + "[48;2;15;15;15m");
			ColorInstance.VTSequenceForeground.ShouldBe(Convert.ToString(Color255.GetEsc()) + "[38;2;15;15;15m");
			ColorInstance.R.ShouldBe(15);
			ColorInstance.G.ShouldBe(15);
			ColorInstance.B.ShouldBe(15);
			ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
			ColorInstance.Hex.ShouldBe("#0F0F0F");
		}

	}
}
