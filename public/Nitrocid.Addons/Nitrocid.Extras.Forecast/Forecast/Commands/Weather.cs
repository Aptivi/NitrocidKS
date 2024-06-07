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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Extras.Forecast.Forecast.Commands
{
    /// <summary>
    /// Shows weather information for a specified city
    /// </summary>
    /// <remarks>
    /// We credit IBM for their decent API service for weather information for the cities around the world from The Weather Channel.
    /// <br></br>
    /// This command lets you get current weather information for a specified city by city ID from The Weather Channel servers. If you want a list, use the switch indicated below.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-list</term>
    /// <description>Searches for the written city name and returns the list of longitudes and latitudes</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class WeatherCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            TextWriters.Write("Coming soon...", KernelColorType.NeutralText);
            return 0;
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("You can either consult the below link for the list of cities with their IDs, or, pass \"-list\" to this command."));
            TextWriterColor.Write("http://bulk.openweathermap.org/sample/city.list.json.gz");
        }

    }
}
