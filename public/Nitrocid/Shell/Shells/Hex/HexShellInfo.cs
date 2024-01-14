﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.Prompts;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.Shells.Hex.Commands;
using Nitrocid.Shell.Shells.Hex.Presets;

namespace Nitrocid.Shell.Shells.Hex
{
    /// <summary>
    /// Common hex shell class
    /// </summary>
    internal class HexShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// Hex commands
        /// </summary>
        public override List<CommandInfo> Commands => new()
        {
            new CommandInfo("addbyte", /* Localizable */ "Adds a new byte at the end of the file",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "byte")
                    })
                ], new AddByteCommand()),

            new CommandInfo("addbytes", /* Localizable */ "Adds the new bytes at the end of the file",
                [
                    new CommandArgumentInfo()
                ], new AddBytesCommand()),

            new CommandInfo("addbyteto", /* Localizable */ "Adds a new byte to the specified position",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "byte"),
                        new CommandArgumentPart(true, "pos", new()
                        {
                            IsNumeric = true,
                        })
                    })
                ], new AddByteToCommand()),

            new CommandInfo("clear", /* Localizable */ "Clears the binary file",
                [
                    new CommandArgumentInfo()
                ], new ClearCommand()),

            new CommandInfo("delbyte", /* Localizable */ "Deletes a byte using the byte number",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "bytenumber", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        })
                    })
                ], new DelByteCommand()),

            new CommandInfo("delbytes", /* Localizable */ "Deletes the range of bytes",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "startbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(false, "endbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        })
                    })
                ], new DelBytesCommand()),

            new CommandInfo("exitnosave", /* Localizable */ "Exits the hex editor",
                [
                    new CommandArgumentInfo()
                ], new ExitNoSaveCommand()),

            new CommandInfo("print", /* Localizable */ "Prints the contents of the file with byte numbers to the console",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(false, "startbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(false, "endbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        })
                    })
                ], new PrintCommand(), CommandFlags.Wrappable),

            new CommandInfo("querybyte", /* Localizable */ "Queries a byte in a specified range of bytes or all bytes",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "byte"),
                        new CommandArgumentPart(false, "startbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(false, "endbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        })
                    })
                ], new QueryByteCommand(), CommandFlags.Wrappable),

            new CommandInfo("replace", /* Localizable */ "Replaces a byte with another one",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "byte"),
                        new CommandArgumentPart(true, "replacebyte")
                    })
                ], new ReplaceCommand()),

            new CommandInfo("save", /* Localizable */ "Saves the file",
                [
                    new CommandArgumentInfo()
                ], new SaveCommand()),

            new CommandInfo("tui", /* Localizable */ "Opens the interactive hex editor TUI",
                [
                    new CommandArgumentInfo()
                ], new TuiCommand()),
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new HexDefaultPreset() },
            { "PowerLine1", new HexPowerLine1Preset() },
            { "PowerLine2", new HexPowerLine2Preset() },
            { "PowerLine3", new HexPowerLine3Preset() },
            { "PowerLineBG1", new HexPowerLineBG1Preset() },
            { "PowerLineBG2", new HexPowerLineBG2Preset() },
            { "PowerLineBG3", new HexPowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new HexShell();

        public override PromptPresetBase CurrentPreset =>
            PromptPresetManager.GetAllPresetsFromShell(ShellType)[PromptPresetManager.CurrentPresets[ShellType]];

    }
}
