using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;

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

using KS.Misc.Forecast;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using Nettify.Weather;

namespace KS.Shell.Commands
{
	class WeatherCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			var ListMode = default(bool);
			if (ListSwitchesOnly.Contains("-list"))
				ListMode = true;
			if (ListMode)
			{
				var Cities = WeatherForecast.ListAllCities();
				ListWriterColor.WriteList(Cities);
			}
			else
			{
				string APIKey = Forecast.ApiKey;
				if (ListArgsOnly.Length > 1)
				{
					APIKey = ListArgsOnly[1];
				}
				else if (string.IsNullOrEmpty(APIKey))
				{
					TextWriterColor.Write(Translate.DoTranslation("You can get your own API key at https://home.openweathermap.org/api_keys."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
					TextWriterColor.Write(Translate.DoTranslation("Enter your API key:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
					APIKey = Input.ReadLineNoInput();
					Forecast.ApiKey = APIKey;
				}
				Forecast.PrintWeatherInfo(ListArgsOnly[0], APIKey);
			}
		}

		public override void HelpHelper()
		{
			TextWriterColor.Write(Translate.DoTranslation("You can always consult http://bulk.openweathermap.org/sample/city.list.json.gz for the list of cities with their IDs.") + " " + Translate.DoTranslation("Or, pass \"listcities\" to this command."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
			TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
			TextWriterColor.Write("  -list: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
			TextWriterColor.Write(Translate.DoTranslation("Shows all the available cities"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
		}

	}
}