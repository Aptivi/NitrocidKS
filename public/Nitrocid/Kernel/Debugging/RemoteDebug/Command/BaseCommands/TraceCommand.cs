﻿
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Languages;
using System;

namespace KS.Kernel.Debugging.RemoteDebug.Command.BaseCommands
{
    internal class TraceCommand : RemoteDebugBaseCommand
    {
        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, RemoteDebugDeviceInfo Address)
        {
            if (DebugWriter.DebugStackTraces.Count != 0)
            {
                if (ListArgsOnly.Length != 0)
                {
                    try
                    {
                        DebugWriter.WriteDebugDevicesOnly(DebugLevel.I, DebugWriter.DebugStackTraces[Convert.ToInt32(ListArgsOnly[0])], true);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebugDevicesOnly(DebugLevel.I, Translate.DoTranslation("Index {0} invalid. There are {1} stack traces. Index is zero-based, so try subtracting by 1.") + " {2}", true, ListArgsOnly[0], DebugWriter.DebugStackTraces.Count, ex.Message);
                    }
                }
                else
                {
                    DebugWriter.WriteDebugDevicesOnly(DebugLevel.I, DebugWriter.DebugStackTraces[0], true);
                }
            }
            else
            {
                DebugWriter.WriteDebugDevicesOnly(DebugLevel.I, Translate.DoTranslation("No stack trace"), true);
            }
        }
    }
}
