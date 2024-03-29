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

using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Starting.Environment.Instances;
using System.Collections.Generic;

namespace Nitrocid.Kernel.Starting.Environment
{
    internal static class EnvironmentTools
    {
        internal static bool resetEnvironment = false;
        internal static string[] arguments = [];
        internal readonly static Dictionary<string, BaseEnvironment> environments = [];
        private readonly static BaseEnvironment mainEnvironment = new NitrocidKS();
        private static BaseEnvironment environment = mainEnvironment;

        internal static void AddEnvironment(string environmentName, BaseEnvironment baseEnvironment)
        {
            if (!HasEnvironment(environmentName))
                environments.Add(environmentName, baseEnvironment);
        }

        internal static void RemoveEnvironment(string environmentName)
        {
            if (HasEnvironment(environmentName))
                environments.Remove(environmentName);
        }

        internal static bool HasEnvironment(string environmentName) =>
            environments.ContainsKey(environmentName);

        internal static BaseEnvironment GetEnvironment(string environmentName)
        {
            if (HasEnvironment(environmentName))
                return environments[environmentName];
            return mainEnvironment;
        }

        internal static void SetEnvironment(string environmentName) =>
            SetEnvironment(GetEnvironment(environmentName) ?? mainEnvironment);

        internal static void SetEnvironment(BaseEnvironment baseEnvironment)
        {
            // Verify the value
            DebugCheck.AssertNull(baseEnvironment, "attempted to set environment to null");

            // Now, set the environment
            environment = baseEnvironment;
        }

        internal static void ResetEnvironment() =>
            SetEnvironment(mainEnvironment);

        internal static void ExecuteEnvironment(string[] args)
        {
            if (environment != mainEnvironment)
                resetEnvironment = true;
            arguments = args;
            environment.EnvironmentEntry(args);
        }
    }
}
