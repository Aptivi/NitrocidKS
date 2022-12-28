﻿
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Languages;
using KS.Misc.Probers.Motd;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can change your message of the day
    /// </summary>
    /// <remarks>
    /// If you don't like the default message of the day that is generated by the kernel, then you can use this command to change the message and store it permanently on the config file.
    /// <br></br>
    /// It also has placeholder support, like if you have <c>&lt;shortdate&gt;</c> and <c>&lt;longtime&gt;</c> placeholders, the <c>&lt;shortdate&gt;</c> placeholder changes to the current system date in the MM/DD/YYYY form, and the <c>&lt;longtime&gt;</c> placeholder changes to the current system time in the HH:MM:SS AM/PM form.
    /// <br></br>
    /// If no arguments are specified, the text editor shell will open to the path of MOTD text file.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class ChMotdCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgsOnly.Length > 0)
            {
                if (string.IsNullOrEmpty(StringArgs))
                {
                    TextWriterColor.Write(Translate.DoTranslation("Blank message of the day."), true, KernelColorType.Error);
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Changing MOTD..."));
                    MotdParse.SetMotd(StringArgs);
                }
            }
            else
            {
                ShellStart.StartShell(ShellType.TextShell, Paths.GetKernelPath(KernelPathType.MOTD));
                TextWriterColor.Write(Translate.DoTranslation("Changing MOTD..."));
                MotdParse.ReadMotd();
            }
        }

    }
}