

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

using KS.Network.SSH;
using KS.Shell.ShellBase.Commands;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Shell.Commands
{
	class SshellCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			string[] AddressDelimiter = ListArgs[0].Split(':');
			string Address = AddressDelimiter[0];
			if (AddressDelimiter.Length > 1)
			{
				int Port = Conversions.ToInteger(AddressDelimiter[1]);
				SSH.InitializeSSH(Address, Port, ListArgs[1], SSH.ConnectionType.Shell);
			}
			else
			{
				SSH.InitializeSSH(Address, 22, ListArgs[1], SSH.ConnectionType.Shell);
			}
		}

	}
}