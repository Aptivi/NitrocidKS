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

using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.Misc.Reflection
{
    /// <summary>
    /// Array tools that are useful
    /// </summary>
    public static class ArrayTools
    {
        /// <summary>
        /// Randomizes the array by shuffling elements, irrespective of the type, using a type of <seealso href="http://en.wikipedia.org/wiki/Schwartzian_transform">Schwartzian transform</seealso>
        /// </summary>
        /// <typeparam name="T">Target type. It's not necessarily an integer.</typeparam>
        /// <param name="array">Target array to sort randomly</param>
        /// <returns>A new array containing elements that are shuffled.</returns>
        public static T[] RandomizeArray<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
                return array;

            // First, create a new list of random numbers with the array's value indexes
            List<(double, T)> valuesToShuffle = [];
            for (int i = 0; i < array.Length; i++)
                valuesToShuffle.Add((RandomDriver.RandomDouble(), array[i]));

            // Then, randomize the array after ordering the numbers
            var randomized = valuesToShuffle
                .OrderBy((val) => val.Item1)
                .Select((kvp) => kvp.Item2)
                .ToArray();
            return randomized;
        }
    }
}
