using KS.Kernel;
using KS.Shell.ShellBase.Commands;
using Microsoft.VisualBasic.CompilerServices;

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

namespace KS.Shell.Commands
{
	class RebootCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			if (((ListArgs?.Length) is { } arg1 ? arg1 == 0 : (bool?)null) == false)
			{
				if (ListArgs[0] == "safe")
				{
					KernelTools.PowerManage(PowerMode.RebootSafe);
				}
				else if (!string.IsNullOrEmpty(ListArgs[0]))
				{
					if ((ListArgs?.Length) is { } arg2 && arg2 > 1)
					{
						KernelTools.PowerManage(PowerMode.RemoteRestart, ListArgs[0], Conversions.ToInteger(ListArgs[1]));
					}
					else
					{
						KernelTools.PowerManage(PowerMode.RemoteRestart, ListArgs[0]);
					}
				}
				else
				{
					KernelTools.PowerManage(PowerMode.Reboot);
				}
			}
			else
			{
				KernelTools.PowerManage(PowerMode.Reboot);
			}
		}

	}
}