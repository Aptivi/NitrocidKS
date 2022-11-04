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

using KS.Network.SSH;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can interact with the Secure SHell server (SSH) to remotely interact with the shell.
    /// </summary>
    /// <remarks>
    /// Secure SHell server (SSH) is a type of server which lets another computer connect to it to run commands in it. In the recent iterations, it is bound to support X11 forwarding. Our implementation is pretty basic, and uses the SSH.NET library by Renci.
    /// <br></br>
    /// This command lets you connect to another computer to remotely interact with the shell.
    /// </remarks>
    class SshellCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var AddressDelimiter = ListArgsOnly[0].Split(':');
            string Address = AddressDelimiter[0];
            if (AddressDelimiter.Length > 1)
            {
                int Port = Convert.ToInt32(AddressDelimiter[1]);
                SSH.InitializeSSH(Address, Port, ListArgsOnly[1], SSH.ConnectionType.Shell);
            }
            else
            {
                SSH.InitializeSSH(Address, 22, ListArgsOnly[1], SSH.ConnectionType.Shell);
            }
        }

    }
}
