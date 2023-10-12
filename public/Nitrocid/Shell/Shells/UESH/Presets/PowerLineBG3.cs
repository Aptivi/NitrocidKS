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

using System;
using System.Text;
using KS.Files.Folders;
using KS.Languages;
using KS.Network.Base;
using KS.ConsoleBase.Colors;
using System.Collections.Generic;
using KS.Users;
using KS.ConsoleBase.Writers.FancyWriters.Tools;
using Terminaux.Colors;
using KS.Shell.ShellBase.Shells;
using KS.Kernel.Configuration;
using KS.Shell.Prompts;

namespace KS.Shell.Shells.UESH.Presets
{
    /// <summary>
    /// PowerLine BG 3 preset
    /// </summary>
    public class PowerLineBG3Preset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "PowerLineBG3";

        /// <inheritdoc/>
        public override string PresetPrompt =>
            PresetPromptBuilder();

        /// <inheritdoc/>
        public override string PresetPromptCompletion =>
            PresetPromptCompletionBuilder();

        internal override string PresetPromptBuilder()
        {
            // PowerLine glyphs
            char TransitionPartChar = Convert.ToChar(0xE0B1);
            char PadlockChar = Convert.ToChar(0xE0A2);

            // PowerLine presets
            List<PowerLineSegment> segments = new()
            {
                new PowerLineSegment(new Color(255, 255, 85), new Color(25, 25, 25), UserManagement.CurrentUser.Username, default, TransitionPartChar),
                new PowerLineSegment(new Color(255, 255, 85), new Color(25, 25, 25), NetworkTools.HostName, PadlockChar, TransitionPartChar),
                new PowerLineSegment(new Color(255, 255, 85), new Color(25, 25, 25), $"{CurrentDirectory.CurrentDir} [{ShellStart.ShellStack.Count}]", default, TransitionPartChar),
            };

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Build the preset
            if (!KernelFlags.Maintenance)
            {
                // Use RenderSegments to render our segments
                PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);
            }
            else
            {
                // Maintenance mode
                PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append(Translate.DoTranslation("Maintenance Mode") + "> ");
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);
            }

            // Present final string
            return PresetStringBuilder.ToString();
        }

        internal override string PresetPromptCompletionBuilder()
        {
            // Segments
            List<PowerLineSegment> segments = new()
            {
                new PowerLineSegment(new Color(255, 255, 85), new Color(25, 25, 25), "+"),
            };

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Use RenderSegments to render our segments
            PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() =>
            PresetPromptBuilder();

        string IPromptPreset.PresetPromptCompletionBuilder() =>
            PresetPromptCompletionBuilder();

    }
}