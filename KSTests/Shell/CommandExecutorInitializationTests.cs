using System;

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

using KS.Shell.ShellBase.Commands;
using NUnit.Framework;
using Shouldly;

namespace KSTests
{

	[TestFixture]
	public class CommandExecutorInitializationTests
	{

		private static CommandExecutor CommandInstance;

		/// <summary>
		/// Tests initializing the command instance from base
		/// </summary>
		[Test]
		[Description("Initialization")]
		public void TestInitializeCommandExecutorFromBase()
		{
			// Create instance
			CommandInstance = new CommandTest();

			// Check for null
			CommandInstance.ShouldNotBeNull();
		}

		/// <summary>
		/// Tests initializing the command instance from base
		/// </summary>
		[Test]
		[Description("Initialization")]
		public void TestInitializedCommandExecution()
		{
			Should.NotThrow(new Action(() => CommandInstance.Execute("", [], [], [])));
		}

		/// <summary>
		/// Tests initializing the command instance from base
		/// </summary>
		[Test]
		[Description("Initialization")]
		public void TestInitializedCommandExecutionWithArguments()
		{
			Should.NotThrow(new Action(() => CommandInstance.Execute("Hello World", ["Hello", "World"], ["Hello", "World"], [])));
		}

		/// <summary>
		/// Tests initializing the command instance from base
		/// </summary>
		[Test]
		[Description("Initialization")]
		public void TestInitializedCommandExecutionWithSwitches()
		{
			Should.NotThrow(new Action(() => CommandInstance.Execute("-s", ["-s"], [], ["-s"])));
		}

		/// <summary>
		/// Tests initializing the command instance from base
		/// </summary>
		[Test]
		[Description("Initialization")]
		public void TestInitializedCommandExecutionWithArgumentsAndSwitches()
		{
			Should.NotThrow(new Action(() => CommandInstance.Execute("-s Hello!", ["-s", "Hello!"], ["Hello!"], ["-s"])));
		}

	}
}