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

using KS.Kernel.Exceptions;
using System.Collections.Generic;
using System.IO;
using Textify.General;

namespace KS.Resources
{
    internal static class ResourcesManager
    {
        private const string resourcesPrefix = "Kernel_Simulator.Resources.";
        private static readonly Dictionary<string, string> resources = [];

        internal static bool DataExists(string resource, ResourcesType type, out string data) =>
            type == ResourcesType.Misc ?
            resources.TryGetValue(resource, out data) :
            resources.TryGetValue($"{type}.{resource}", out data);

        internal static string GetData(string resource, ResourcesType type)
        {
            if (!DataExists(resource, type, out string data))
                throw new InvalidPathException($"Resource {resource} not found for type {type} in between {resources.Count} resources");
            return data;
        }

        private static void InitializeData()
        {
            var thisAsm = typeof(ResourcesManager).Assembly;
            var resourceFullNames = thisAsm.GetManifestResourceNames();
            foreach (string resourceFullName in resourceFullNames)
            {
                // Get the stream and parse its contents
                var contentStream = thisAsm.GetManifestResourceStream(resourceFullName);
                using var contentStreamReader = new StreamReader(contentStream);
                string content = contentStreamReader.ReadToEnd();
                string fileName = resourceFullName.RemovePrefix(resourcesPrefix);

                // Afterwards, add the resulting content to the resources dictionary to cache it
                resources.Add(fileName, content);
            }
        }

        static ResourcesManager() =>
            InitializeData();
    }
}
