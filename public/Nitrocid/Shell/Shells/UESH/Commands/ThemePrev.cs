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

using Terminaux.Inputs.Styles.Selection;
using Nitrocid.ConsoleBase.Themes;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Files;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Colors;
using Terminaux.Inputs;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Previews a theme
    /// </summary>
    /// <remarks>
    /// This comand lets you preview a theme in an interactive way.
    /// </remarks>
    class ThemePrevCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string selectedTheme = parameters.ArgumentsList.Length > 0 ? parameters.ArgumentsList[0] : "";
            string ThemePath;

            // Let the user select a theme
            if (parameters.ArgumentsList.Length == 0)
            {
                while (true)
                {
                    // Let the user select a theme category
                    string[] categoryNames = Enum.GetNames(typeof(ThemeCategory));
                    List<InputChoiceInfo> themeCategoryChoices = [];
                    List<InputChoiceInfo> themeCategoryAltChoices =
                    [
                        new($"{categoryNames.Length + 1}", Translate.DoTranslation("Exit"))
                    ];
                    for (int i = 0; i < categoryNames.Length; i++)
                    {
                        string category = categoryNames[i];
                        var ici = new InputChoiceInfo(
                            $"{i + 1}",
                            $"{category}"
                        );
                        themeCategoryChoices.Add(ici);
                    }
                    int categoryIndex = SelectionStyle.PromptSelection(Translate.DoTranslation("Select a category"), [.. themeCategoryChoices], [.. themeCategoryAltChoices]) - 1;

                    // If the color index is -2, exit. PromptSelection returns -1 if ESC is pressed to cancel selecting. However, the index just decreases to -2
                    // even if that PromptSelection returned the abovementioned value, so bail if index is -2
                    if (categoryIndex == -2 || categoryIndex >= categoryNames.Length)
                    {
                        ColorTools.LoadBack();
                        return 3;
                    }

                    // Let the user select a theme
                    var finalCategory = Enum.Parse<ThemeCategory>(categoryNames[categoryIndex]);
                    List<InputChoiceInfo> themeChoices = [];
                    List<InputChoiceInfo> themeAltChoices =
                    [
                        new("<--", Translate.DoTranslation("Back"))
                    ];
                    foreach (string theme in ThemeTools.GetInstalledThemesByCategory(finalCategory).Keys)
                    {
                        var themeInstance = ThemeTools.GetThemeInfo(theme);
                        string name = themeInstance.Name;
                        string desc = themeInstance.Localizable ? Translate.DoTranslation(themeInstance.Description) : themeInstance.Description;
                        var ici = new InputChoiceInfo(
                            theme,
                            $"{name}{(themeInstance.IsEvent ? $" - [{themeInstance.StartMonth}/{themeInstance.StartDay} -> {themeInstance.EndMonth}/{themeInstance.EndDay} / {(themeInstance.IsExpired ? Translate.DoTranslation("Expired") : Translate.DoTranslation("Available"))}]" : "")}",
                            desc
                        );
                        themeChoices.Add(ici);
                    }
                    int colorIndex = SelectionStyle.PromptSelection(Translate.DoTranslation("Select a theme"), [.. themeChoices], [.. themeAltChoices]) - 1;

                    // If the color index is -2, exit. PromptSelection returns -1 if ESC is pressed to cancel selecting. However, the index just decreases to -2
                    // even if that PromptSelection returned the abovementioned value, so bail if index is -2
                    if (colorIndex == -2)
                    {
                        ColorTools.LoadBack();
                        return 3;
                    }
                    else if (colorIndex != themeChoices.Count)
                    {
                        // Get the theme name from index
                        selectedTheme = ThemeTools.GetInstalledThemesByCategory(finalCategory).Keys.ElementAt(colorIndex);
                    }

                    // Load the theme to the instance
                    ThemePath = FilesystemTools.NeutralizePath(selectedTheme);
                    ThemeInfo Theme;
                    if (Checking.FileExists(ThemePath))
                        Theme = new ThemeInfo(ThemePath);
                    else
                        Theme = ThemeTools.GetThemeInfo(selectedTheme);

                    // Now, preview the theme
                    ThemePreviewTools.PreviewTheme(Theme);
                }
            }
            return 0;
        }

        public override void HelpHelper() =>
            TextWriterColor.Write("<Theme>: ThemeName.json, " + string.Join(", ", ThemeTools.GetInstalledThemes().Keys));

    }
}
