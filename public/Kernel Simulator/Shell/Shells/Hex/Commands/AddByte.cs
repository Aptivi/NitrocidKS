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
using KS.Misc.Editors.HexEdit;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Adds a new byte at the end of the file
    /// </summary>
    /// <remarks>
    /// You can use this command to add a new byte at the end of the file.
    /// </remarks>
    class HexEdit_AddByteCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            byte ByteContent = Convert.ToByte(StringArgs, 16);
            HexEditTools.HexEdit_AddNewByte(ByteContent);
        }

    }
}