//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Reflection;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using Nettify.Weather;
using System;

namespace KS.Misc.Forecast
{
    public static class Forecast
    {

        public static UnitMeasurement PreferredUnit = UnitMeasurement.Metric;
        internal static string ApiKey = "";

        /// <summary>
        /// Gets current weather info from OpenWeatherMap
        /// </summary>
        /// <param name="CityID">City ID</param>
        /// <returns>A class containing properties of weather information</returns>
        public static WeatherForecastInfo GetWeatherInfo(long CityID)
        {
            return WeatherForecastOwm.GetWeatherInfo(CityID: CityID, ApiKey, PreferredUnit);
        }

        /// <summary>
        /// Gets current weather info from OpenWeatherMap
        /// </summary>
        /// <param name="CityID">City ID</param>
        /// <param name="APIKey">API key</param>
        /// <returns>A class containing properties of weather information</returns>
        public static WeatherForecastInfo GetWeatherInfo(long CityID, string APIKey)
        {
            return WeatherForecastOwm.GetWeatherInfo(CityID: CityID, APIKey, PreferredUnit);
        }

        /// <summary>
        /// Gets current weather info from OpenWeatherMap
        /// </summary>
        /// <param name="CityName">City name</param>
        /// <returns>A class containing properties of weather information</returns>
        public static WeatherForecastInfo GetWeatherInfo(string CityName)
        {
            return WeatherForecastOwm.GetWeatherInfo(CityName: CityName, ApiKey, PreferredUnit);
        }

        /// <summary>
        /// Gets current weather info from OpenWeatherMap
        /// </summary>
        /// <param name="CityName">City name</param>
        /// <param name="APIKey">API key</param>
        /// <returns>A class containing properties of weather information</returns>
        public static WeatherForecastInfo GetWeatherInfo(string CityName, string APIKey)
        {
            return WeatherForecastOwm.GetWeatherInfo(CityName: CityName, APIKey, PreferredUnit);
        }

        /// <summary>
        /// Prints the weather information to the console
        /// </summary>
        /// <param name="CityID">City ID or name</param>
        public static void PrintWeatherInfo(string CityID)
        {
            PrintWeatherInfo(CityID, ApiKey);
        }

        /// <summary>
        /// Prints the weather information to the console
        /// </summary>
        /// <param name="CityID">City ID or name</param>
        /// <param name="APIKey">API Key</param>
        public static void PrintWeatherInfo(string CityID, string APIKey)
        {
            WeatherForecastInfo WeatherInfo;
            string WeatherSpecifier = "°";
            string WindSpeedSpecifier = "m.s";
            if (StringQuery.IsStringNumeric(CityID))
                WeatherInfo = GetWeatherInfo(Convert.ToInt64(CityID), APIKey);
            else
                WeatherInfo = GetWeatherInfo(CityID, APIKey);
            string name = (string)WeatherInfo.WeatherToken["name"];
            double feelsLike = (double)WeatherInfo.WeatherToken["main"]["feels_like"];
            double pressure = (double)WeatherInfo.WeatherToken["main"]["pressure"];
            DebugWriter.Wdbg(DebugLevel.I, "City name: {0}, City ID: {1}", name, CityID);
            TextFancyWriters.WriteSeparator(Translate.DoTranslation("-- Weather info for {0} --"), ColTypes: KernelColorTools.ColTypes.Separator, name);
            TextWriters.Write(Translate.DoTranslation("Weather: {0}"), true, KernelColorTools.ColTypes.Neutral, WeatherInfo.Weather);
            if (WeatherInfo.TemperatureMeasurement == UnitMeasurement.Metric)
            {
                WeatherSpecifier += "C";
            }
            else if (WeatherInfo.TemperatureMeasurement == UnitMeasurement.Kelvin)
            {
                WeatherSpecifier += "K";
            }
            else if (WeatherInfo.TemperatureMeasurement == UnitMeasurement.Imperial)
            {
                WeatherSpecifier += "F";
                WindSpeedSpecifier = "mph";
            }
            TextWriters.Write(Translate.DoTranslation("Temperature: {0}") + WeatherSpecifier, true, KernelColorTools.ColTypes.Neutral, WeatherInfo.Temperature.ToString("N2"));
            TextWriters.Write(Translate.DoTranslation("Feels like: {0}") + WeatherSpecifier, true, KernelColorTools.ColTypes.Neutral, feelsLike.ToString("N2"));
            TextWriters.Write(Translate.DoTranslation("Wind speed: {0}") + " {1}", true, KernelColorTools.ColTypes.Neutral, WeatherInfo.WindSpeed.ToString("N2"), WindSpeedSpecifier);
            TextWriters.Write(Translate.DoTranslation("Wind direction: {0}") + "°", true, KernelColorTools.ColTypes.Neutral, WeatherInfo.WindDirection.ToString("N2"));
            TextWriters.Write(Translate.DoTranslation("Pressure: {0}") + " hPa", true, KernelColorTools.ColTypes.Neutral, pressure.ToString("N2"));
            TextWriters.Write(Translate.DoTranslation("Humidity: {0}") + "%", true, KernelColorTools.ColTypes.Neutral, WeatherInfo.Humidity.ToString("N2"));
        }

    }
}
