

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

using KS.Network.Mail;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Commands
{
	class MailCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			if (MailShellCommon.KeepAlive)
			{
				ShellStart.StartShell(ShellType.MailShell);
			}
			else if ((((ListArgs?.Length) is { } arg1 ? arg1 == 0 : null) | ListArgs is null) == true)
			{
				MailLogin.PromptUser();
			}
			else if (!string.IsNullOrEmpty(ListArgs[0]))
			{
				MailLogin.PromptPassword(ListArgs[0]);
			}
		}

	}
}