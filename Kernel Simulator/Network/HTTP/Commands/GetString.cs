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
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;

namespace KS.Network.HTTP.Commands
{
    class HTTP_GetStringCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (HTTPShellCommon.HTTPConnected == true)
            {
                // Print a message
                TextWriters.Write(Translate.DoTranslation("Getting {0}..."), true, KernelColorTools.ColTypes.Progress, ListArgs[0]);

                try
                {
                    var ResponseTask = HTTPTools.HttpGetString(ListArgs[0]);
                    ResponseTask.Wait();
                    string Response = ResponseTask.Result;
                    TextWriters.Write(Response, true, KernelColorTools.ColTypes.Neutral);
                }
                catch (AggregateException aex)
                {
                    TextWriters.Write(aex.Message + ":", true, KernelColorTools.ColTypes.Error);
                    foreach (Exception InnerException in aex.InnerExceptions)
                    {
                        TextWriters.Write("- " + InnerException.Message, true, KernelColorTools.ColTypes.Error);
                        if (InnerException.InnerException is not null)
                        {
                            TextWriters.Write("- " + InnerException.InnerException.Message, true, KernelColorTools.ColTypes.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TextWriters.Write(ex.Message, true, KernelColorTools.ColTypes.Error);
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("You must connect to server before performing transmission."), true, KernelColorTools.ColTypes.Error);
            }
        }

    }
}