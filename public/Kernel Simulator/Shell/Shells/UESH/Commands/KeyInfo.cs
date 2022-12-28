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

using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using KS.ConsoleBase.Inputs;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can view the information about a pressed key
    /// </summary>
    /// <remarks>
    /// This command lets you view the details about a pressed key on your keyboard, including the pressed key and character, the hexadecimal representation of the letter, the pressed modifiers, and the keyboard shortcut.
    /// </remarks>
    class KeyInfoCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            TextWriterColor.Write(Translate.DoTranslation("Enter a key or a combination of keys to display its information."));
            var KeyPress = Input.DetectKeypress();

            // Pressed key
            TextWriterColor.Write("- " + Translate.DoTranslation("Pressed key") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(KeyPress.Key.ToString(), true, KernelColorType.ListValue);

            // If the pressed key is a control key, don't write the actual key char so as not to corrupt the output
            if (!char.IsControl(KeyPress.KeyChar))
            {
                TextWriterColor.Write("- " + Translate.DoTranslation("Pressed key character") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(Convert.ToString(KeyPress.KeyChar), true, KernelColorType.ListValue);
            }

            // Pressed key character code
            TextWriterColor.Write("- " + Translate.DoTranslation("Pressed key character code") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.Write($"0x{Convert.ToInt32(KeyPress.KeyChar):X2} [{Convert.ToInt32(KeyPress.KeyChar)}]", true, KernelColorType.ListValue);

            // Pressed modifiers
            TextWriterColor.Write("- " + Translate.DoTranslation("Pressed modifiers") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(KeyPress.Modifiers.ToString(), true, KernelColorType.ListValue);

            // Keyboard shortcut
            TextWriterColor.Write("- " + Translate.DoTranslation("Keyboard shortcut") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.Write($"{string.Join(" + ", KeyPress.Modifiers.ToString().Split(new string[] { ", " }, StringSplitOptions.None))} + {KeyPress.Key}", true, KernelColorType.ListValue);
        }

    }
}
