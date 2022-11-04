﻿
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

using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.HTTP;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.HTTP.Commands
{
    /// <summary>
    /// Removes content from the HTTP server
    /// </summary>
    /// <remarks>
    /// If you want to test a DELETE function of the REST API, you can do so using this command.
    /// </remarks>
    class HTTP_DeleteCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (HTTPShellCommon.HTTPConnected == true)
            {
                // Print a message
                TextWriterColor.Write(Translate.DoTranslation("Deleting {0}..."), true, ColorTools.ColTypes.Progress, ListArgsOnly[0]);

                // Make a confirmation message so user will not accidentally delete a file or folder
                TextWriterColor.Write(Translate.DoTranslation("Are you sure you want to delete {0} <y/n>?") + " ", false, ColorTools.ColTypes.Input, ListArgsOnly[0]);
                string answer = Convert.ToString(ConsoleBase.ConsoleWrapper.ReadKey().KeyChar);
                TextWriterColor.Write();

                try
                {
                    var DeleteTask = HTTPTools.HttpDelete(ListArgsOnly[0]);
                    DeleteTask.Wait();
                }
                catch (AggregateException aex)
                {
                    TextWriterColor.Write(aex.Message + ":", true, ColorTools.ColTypes.Error);
                    foreach (Exception InnerException in aex.InnerExceptions)
                    {
                        TextWriterColor.Write("- " + InnerException.Message, true, ColorTools.ColTypes.Error);
                        if (InnerException.InnerException is not null)
                        {
                            TextWriterColor.Write("- " + InnerException.InnerException.Message, true, ColorTools.ColTypes.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(ex.Message, true, ColorTools.ColTypes.Error);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("You must connect to server with administrative privileges before performing the deletion."), true, ColorTools.ColTypes.Error);
            }
        }

    }
}