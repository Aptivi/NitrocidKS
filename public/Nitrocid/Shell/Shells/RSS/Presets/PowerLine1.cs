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
using KS.ConsoleBase.Colors;
using System.Collections.Generic;
using KS.ConsoleBase.Writers.FancyWriters.Tools;
using Terminaux.Colors;
using KS.Shell.Prompts;

namespace KS.Shell.Shells.RSS.Presets
{
    /// <summary>
    /// PowerLine 1 preset
    /// </summary>
    public class RSSPowerLine1Preset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "PowerLine1";

        /// <inheritdoc/>
        public override string PresetShellType { get; } = "RSSShell";

        /// <inheritdoc/>
        public override string PresetPrompt =>
            PresetPromptBuilder();

        /// <inheritdoc/>
        public override string PresetPromptCompletion =>
            PresetPromptCompletionBuilder();

        /// <inheritdoc/>
        public override string PresetPromptShowcase =>
            PresetPromptBuilderShowcase();

        /// <inheritdoc/>
        public override string PresetPromptCompletionShowcase =>
            PresetPromptCompletionBuilder();

        internal override string PresetPromptBuilder()
        {
            // Segments
            List<PowerLineSegment> segments = new()
            {
                new PowerLineSegment(new Color(85, 255, 255), new Color(43, 127, 127), new Uri(RSSShellCommon.RSSFeedLink).Host),
            };

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Use RenderSegments to render our segments
            PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

        internal override string PresetPromptBuilderShowcase()
        {
            // Segments
            List<PowerLineSegment> segments = new()
            {
                new PowerLineSegment(new Color(85, 255, 255), new Color(43, 127, 127), "rss.fabrikam.com/news/tech.rss"),
            };

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Use RenderSegments to render our segments
            PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

        internal override string PresetPromptCompletionBuilder()
        {
            // Segments
            List<PowerLineSegment> segments = new()
            {
                new PowerLineSegment(new Color(85, 255, 255), new Color(43, 127, 127), "+"),
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
