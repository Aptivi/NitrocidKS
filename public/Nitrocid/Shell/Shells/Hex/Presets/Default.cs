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

using System.IO;
using System.Text;
using KS.ConsoleBase.Colors;
using KS.Files.Editors.HexEdit;
using KS.Shell.Prompts;

namespace KS.Shell.Shells.Hex.Presets
{
    /// <summary>
    /// Default preset
    /// </summary>
    public class HexDefaultPreset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "Default";

        /// <inheritdoc/>
        public override string PresetPrompt =>
            PresetPromptBuilder();

        /// <inheritdoc/>
        public override string PresetPromptShowcase =>
            PresetPromptBuilderShowcase();

        /// <inheritdoc/>
        public override string PresetShellType { get; } = "HexShell";

        internal override string PresetPromptBuilder()
        {
            // Build the preset
            var PresetStringBuilder = new StringBuilder();

            // Opening
            PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append('[');

            // File name
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.UserNameShell).VTSequenceForeground);
            PresetStringBuilder.AppendFormat(Path.GetFileName(HexEditShellCommon.HexEdit_FileStream.Name));

            // Was file edited?
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.UserNameShell).VTSequenceForeground);
            PresetStringBuilder.AppendFormat("{0}", HexEditTools.HexEdit_WasHexEdited() ? "*" : "");

            // Closing
            PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append("] > ");
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

        internal override string PresetPromptBuilderShowcase()
        {
            // Build the preset
            var PresetStringBuilder = new StringBuilder();

            // Opening
            PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append('[');

            // File name
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.UserNameShell).VTSequenceForeground);
            PresetStringBuilder.AppendFormat("file.bin");

            // Was file edited?
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.UserNameShell).VTSequenceForeground);
            PresetStringBuilder.AppendFormat("*");

            // Closing
            PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append("] > ");
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}
