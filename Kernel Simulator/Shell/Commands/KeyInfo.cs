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
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
namespace KS.Shell.Commands
{
    class KeyInfoCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            TextWriters.Write(Translate.DoTranslation("Enter a key or a combination of keys to display its information."), true, KernelColorTools.ColTypes.Neutral);
            var KeyPress = Input.DetectKeypress();

            // Pressed key
            TextWriters.Write("- " + Translate.DoTranslation("Pressed key") + ": ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(KeyPress.Key.ToString(), true, KernelColorTools.ColTypes.ListValue);

            // If the pressed key is a control key, don't write the actual key char so as not to corrupt the output
            if (!char.IsControl(KeyPress.KeyChar))
            {
                TextWriters.Write("- " + Translate.DoTranslation("Pressed key character") + ": ", false, KernelColorTools.ColTypes.ListEntry);
                TextWriters.Write(Convert.ToString(KeyPress.KeyChar), true, KernelColorTools.ColTypes.ListValue);
            }

            // Pressed key character code
            TextWriters.Write("- " + Translate.DoTranslation("Pressed key character code") + ": ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write($"0x{Convert.ToInt32(KeyPress.KeyChar):X2} [{Convert.ToInt32(KeyPress.KeyChar)}]", true, KernelColorTools.ColTypes.ListValue);

            // Pressed modifiers
            TextWriters.Write("- " + Translate.DoTranslation("Pressed modifiers") + ": ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(KeyPress.Modifiers.ToString(), true, KernelColorTools.ColTypes.ListValue);

            // Keyboard shortcut
            TextWriters.Write("- " + Translate.DoTranslation("Keyboard shortcut") + ": ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write($"{string.Join(" +", KeyPress.Modifiers.ToString().Split(Convert.ToChar(", ")))} + {KeyPress.Key}", true, KernelColorTools.ColTypes.ListValue);
        }

    }
}