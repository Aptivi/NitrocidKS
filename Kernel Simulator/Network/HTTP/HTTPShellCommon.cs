using System.Collections.Generic;

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

using System.Net.Http;
using KS.Network.HTTP.Commands;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Network.HTTP
{
	public static class HTTPShellCommon
	{

		public static readonly Dictionary<string, CommandInfo> HTTPCommands = new() { { "delete", new CommandInfo("delete", ShellType.HTTPShell, "Deletes content from HTTP server", new CommandArgumentInfo(["<request>"], true, 1), new HTTP_DeleteCommand()) }, { "get", new CommandInfo("get", ShellType.HTTPShell, "Gets the response from the HTTP server using the specified request", new CommandArgumentInfo(["<request>"], true, 1), new HTTP_GetCommand()) }, { "getstring", new CommandInfo("getstring", ShellType.HTTPShell, "Gets the string from the HTTP server using the specified request", new CommandArgumentInfo(["<request>"], true, 1), new HTTP_GetStringCommand()) }, { "help", new CommandInfo("help", ShellType.HTTPShell, "Shows help screen", new CommandArgumentInfo(["[command]"], false, 0), new HTTP_HelpCommand()) }, { "setsite", new CommandInfo("setsite", ShellType.HTTPShell, "Sets the HTTP site. Must be a valid URI.", new CommandArgumentInfo(["<uri>"], true, 1), new HTTP_SetSiteCommand()) } };
		public static string HTTPSite;
		public static string HTTPShellPromptStyle = "";
		public static HttpClient ClientHTTP = new();
		internal static readonly Dictionary<string, CommandInfo> HTTPModCommands = [];

		/// <summary>
		/// See if the HTTP shell is connected
		/// </summary>
		public static bool HTTPConnected
		{
			get
			{
				return !string.IsNullOrEmpty(HTTPSite);
			}
		}

	}
}