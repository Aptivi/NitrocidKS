using System.Data;

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

using System.IO;
using System.Linq;
using KS.Languages;
using KS.Misc.Text;
using KS.Network.RemoteDebug.Interface;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Network.RemoteDebug.Commands
{
	class Debug_RegisterCommand : RemoteDebugCommandExecutor, IRemoteDebugCommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, StreamWriter SocketStreamWriter, string DeviceAddress)
		{
			if (string.IsNullOrWhiteSpace(Conversions.ToString(RemoteDebugTools.GetDeviceProperty(DeviceAddress, RemoteDebugTools.DeviceProperty.Name))))
			{
				if (ListArgs.Length != 0)
				{
					RemoteDebugTools.SetDeviceProperty(DeviceAddress, RemoteDebugTools.DeviceProperty.Name, ListArgs[0]);
					RemoteDebugger.DebugDevices.Select((Device) =>
					{
						if ((Device.ClientIP ?? "") == (DeviceAddress ?? ""))
						{
							return Device;
						}
						return null;
					}).ElementAtOrDefault(0).ClientName = ListArgs[0];
					SocketStreamWriter.WriteLine(Translate.DoTranslation("Hi, {0}!").FormatString(ListArgs[0]));
				}
				else
				{
					SocketStreamWriter.WriteLine(Translate.DoTranslation("You need to write your name."));
				}
			}
			else
			{
				SocketStreamWriter.WriteLine(Translate.DoTranslation("You're already registered."));
			}
		}

	}
}