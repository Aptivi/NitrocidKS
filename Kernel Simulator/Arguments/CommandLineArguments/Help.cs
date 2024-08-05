﻿//
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

using KS.Arguments.ArgumentBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.ConsoleBase.Writers;
using Terminaux.Base;
using Terminaux.Colors;

namespace KS.Arguments.CommandLineArguments
{
    class CommandLine_HelpArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            // Command-line arguments
            TextWriters.Write(Translate.DoTranslation("Command-line arguments:"), true, KernelColorTools.ColTypes.ListTitle);
            ArgumentHelpSystem.ShowArgsHelp(ArgumentType.CommandLineArgs);
            TextWriters.Write("", KernelColorTools.ColTypes.Neutral);

            // Pre-boot command-line arguments
            TextWriters.Write(Translate.DoTranslation("Pre-boot command-line arguments:"), true, KernelColorTools.ColTypes.ListTitle);
            ArgumentHelpSystem.ShowArgsHelp(ArgumentType.PreBootCommandLineArgs);
            TextWriters.Write("", KernelColorTools.ColTypes.Neutral);

            // Either start the kernel or exit it
            TextWriters.Write(Translate.DoTranslation("* Press any key to start the kernel or ESC to exit."), true, KernelColorTools.ColTypes.Tip);
            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
            {
                // Clear the console and reset the colors
                ColorTools.ResetColors();
                ConsoleWrapper.Clear();
                Environment.Exit(0);
            }
        }

    }
}
