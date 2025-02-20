﻿using Nitrocid.Kernel.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace KSMod
{
    public class ModName : IMod
    {
        public string Name => "My Mod";

        public string Version => "1.0.0";

        public Version MinimumSupportedApiVersion => new(3, 1, 27, 45);

        public ModLoadPriority LoadPriority => ModLoadPriority.Optional;

        public void StartMod()
        {

        }

        public void StopMod()
        {

        }
    }
}

// Refer to https://aptivi.github.io/Nitrocid for up-to-date API documentation for mod developers.
