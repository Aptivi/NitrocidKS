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

using System;
using System.Linq;

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

using System.Net.NetworkInformation;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Network;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class PingCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            // If the pinged address is actually a number of times
            int PingTimes = 4;
            int StepsToSkip = 0;
            if (StringQuery.IsStringNumeric(ListArgs[0]))
            {
                DebugWriter.Wdbg(DebugLevel.I, "ListArgs(0) is numeric. Assuming number of times: {0}", ListArgs[0]);
                PingTimes = Convert.ToInt32(ListArgs[0]);
                StepsToSkip = 1;
            }
            foreach (string PingedAddress in ListArgs.Skip(StepsToSkip))
            {
                if (!string.IsNullOrEmpty(PingedAddress))
                {
                    SeparatorWriterColor.WriteSeparator(PingedAddress, true);
                    for (int CurrentTime = 1, loopTo = PingTimes; CurrentTime <= loopTo; CurrentTime++)
                    {
                        try
                        {
                            var PingReplied = NetworkTools.PingAddress(PingedAddress);
                            if (PingReplied.Status == IPStatus.Success)
                            {
                                TextWriterColor.Write("[{1}] " + Translate.DoTranslation("Ping succeeded in {0} ms."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), PingReplied.RoundtripTime, CurrentTime);
                            }
                            else
                            {
                                TextWriterColor.Write("[{2}] " + Translate.DoTranslation("Failed to ping {0}: {1}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), PingedAddress, PingReplied.Status, CurrentTime);
                            }
                        }
                        catch (Exception ex)
                        {
                            TextWriterColor.Write("[{2}] " + Translate.DoTranslation("Failed to ping {0}: {1}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), PingedAddress, ex.Message, CurrentTime);
                            DebugWriter.WStkTrc(ex);
                        }
                    }
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Address may not be empty."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                }
            }
        }

    }
}