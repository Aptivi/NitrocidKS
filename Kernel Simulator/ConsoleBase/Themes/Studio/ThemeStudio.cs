using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files.Folders;
using KS.Languages;

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

using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using Microsoft.VisualBasic.CompilerServices;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Selector;

namespace KS.ConsoleBase.Themes.Studio
{
	static class ThemeStudio
	{

		/// <summary>
        /// Starts the theme studio
        /// </summary>
        /// <param name="ThemeName">Theme name</param>
		public static void StartThemeStudio(string ThemeName)
		{
			// Inform user that we're on the studio
			Kernel.Kernel.KernelEventManager.RaiseThemeStudioStarted();
			DebugWriter.Wdbg(DebugLevel.I, "Starting theme studio with theme name {0}", ThemeName);
			string Response;
			int MaximumOptions = 38 + 9; // Colors + options
			var StudioExiting = default(bool);

			while (!StudioExiting)
			{
				DebugWriter.Wdbg(DebugLevel.I, "Studio not exiting yet. Populating {0} options...", MaximumOptions);
				ConsoleWrapper.Clear();
				TextWriterColor.Write(Translate.DoTranslation("Making a new theme \"{0}\".") + Kernel.Kernel.NewLine, true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), ThemeName);

				// List options
				TextWriterColor.Write("1) " + Translate.DoTranslation("Input color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedInputColor.PlainSequence);
				TextWriterColor.Write("2) " + Translate.DoTranslation("License color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedLicenseColor.PlainSequence);
				TextWriterColor.Write("3) " + Translate.DoTranslation("Continuable kernel error color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedContKernelErrorColor.PlainSequence);
				TextWriterColor.Write("4) " + Translate.DoTranslation("Uncontinuable kernel error color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedUncontKernelErrorColor.PlainSequence);
				TextWriterColor.Write("5) " + Translate.DoTranslation("Host name color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedHostNameShellColor.PlainSequence);
				TextWriterColor.Write("6) " + Translate.DoTranslation("User name color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedUserNameShellColor.PlainSequence);
				TextWriterColor.Write("7) " + Translate.DoTranslation("Background color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedBackgroundColor.PlainSequence);
				TextWriterColor.Write("8) " + Translate.DoTranslation("Neutral text color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedNeutralTextColor.PlainSequence);
				TextWriterColor.Write("9) " + Translate.DoTranslation("List entry color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedListEntryColor.PlainSequence);
				TextWriterColor.Write("10) " + Translate.DoTranslation("List value color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedListValueColor.PlainSequence);
				TextWriterColor.Write("11) " + Translate.DoTranslation("Stage color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedStageColor.PlainSequence);
				TextWriterColor.Write("12) " + Translate.DoTranslation("Error color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedErrorColor.PlainSequence);
				TextWriterColor.Write("13) " + Translate.DoTranslation("Warning color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedWarningColor.PlainSequence);
				TextWriterColor.Write("14) " + Translate.DoTranslation("Option color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools._SelectedOptionColor.PlainSequence);
				TextWriterColor.Write("15) " + Translate.DoTranslation("Banner color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedBannerColor.PlainSequence);
				TextWriterColor.Write("16) " + Translate.DoTranslation("Notification title color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedNotificationTitleColor.PlainSequence);
				TextWriterColor.Write("17) " + Translate.DoTranslation("Notification description color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedNotificationDescriptionColor.PlainSequence);
				TextWriterColor.Write("18) " + Translate.DoTranslation("Notification progress color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedNotificationProgressColor.PlainSequence);
				TextWriterColor.Write("19) " + Translate.DoTranslation("Notification failure color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedNotificationFailureColor.PlainSequence);
				TextWriterColor.Write("20) " + Translate.DoTranslation("Question color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedQuestionColor.PlainSequence);
				TextWriterColor.Write("21) " + Translate.DoTranslation("Success color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedSuccessColor.PlainSequence);
				TextWriterColor.Write("22) " + Translate.DoTranslation("User dollar color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedUserDollarColor.PlainSequence);
				TextWriterColor.Write("23) " + Translate.DoTranslation("Tip color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedTipColor.PlainSequence);
				TextWriterColor.Write("24) " + Translate.DoTranslation("Separator text color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedSeparatorTextColor.PlainSequence);
				TextWriterColor.Write("25) " + Translate.DoTranslation("Separator color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedSeparatorColor.PlainSequence);
				TextWriterColor.Write("26) " + Translate.DoTranslation("List title color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedListTitleColor.PlainSequence);
				TextWriterColor.Write("27) " + Translate.DoTranslation("Development warning color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedDevelopmentWarningColor.PlainSequence);
				TextWriterColor.Write("28) " + Translate.DoTranslation("Stage time color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedStageTimeColor.PlainSequence);
				TextWriterColor.Write("29) " + Translate.DoTranslation("Progress color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedProgressColor.PlainSequence);
				TextWriterColor.Write("30) " + Translate.DoTranslation("Back option color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedBackOptionColor.PlainSequence);
				TextWriterColor.Write("31) " + Translate.DoTranslation("Low priority border color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedLowPriorityBorderColor.PlainSequence);
				TextWriterColor.Write("32) " + Translate.DoTranslation("Medium priority border color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedMediumPriorityBorderColor.PlainSequence);
				TextWriterColor.Write("33) " + Translate.DoTranslation("High priority border color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedHighPriorityBorderColor.PlainSequence);
				TextWriterColor.Write("34) " + Translate.DoTranslation("Table separator color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedTableSeparatorColor.PlainSequence);
				TextWriterColor.Write("35) " + Translate.DoTranslation("Table header color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedTableHeaderColor.PlainSequence);
				TextWriterColor.Write("36) " + Translate.DoTranslation("Table value color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedTableValueColor.PlainSequence);
				TextWriterColor.Write("37) " + Translate.DoTranslation("Selected option color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedSelectedOptionColor.PlainSequence);
				TextWriterColor.Write("38) " + Translate.DoTranslation("Alternative option color") + ": [{0}] ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), ThemeStudioTools.SelectedAlternativeOptionColor.PlainSequence);
				TextWriterColor.WritePlain("", true);

				// List saving and loading options
				TextWriterColor.Write("39) " + Translate.DoTranslation("Save Theme to Current Directory"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.AlternativeOption));
				TextWriterColor.Write("40) " + Translate.DoTranslation("Save Theme to Another Directory..."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.AlternativeOption));
				TextWriterColor.Write("41) " + Translate.DoTranslation("Save Theme to Current Directory as..."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.AlternativeOption));
				TextWriterColor.Write("42) " + Translate.DoTranslation("Save Theme to Another Directory as..."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.AlternativeOption));
				TextWriterColor.Write("43) " + Translate.DoTranslation("Load Theme From File..."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.AlternativeOption));
				TextWriterColor.Write("44) " + Translate.DoTranslation("Load Theme From Prebuilt Themes..."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.AlternativeOption));
				TextWriterColor.Write("45) " + Translate.DoTranslation("Load Current Colors"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.AlternativeOption));
				TextWriterColor.Write("46) " + Translate.DoTranslation("Preview..."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.AlternativeOption));
				TextWriterColor.Write("47) " + Translate.DoTranslation("Exit"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.AlternativeOption));
				TextWriterColor.WritePlain("", true);

				// Prompt user
				DebugWriter.Wdbg(DebugLevel.I, "Waiting for user input...");
				TextWriterColor.Write("> ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
				Response = Input.ReadLine();
				DebugWriter.Wdbg(DebugLevel.I, "Got response: {0}", Response);

				// Check for response integrity
				if (StringQuery.IsStringNumeric(Response))
				{
					DebugWriter.Wdbg(DebugLevel.I, "Response is numeric.");
					int NumericResponse = Conversions.ToInteger(Response);
					DebugWriter.Wdbg(DebugLevel.I, "Checking response...");
					if (NumericResponse >= 1 & NumericResponse <= MaximumOptions)
					{
						DebugWriter.Wdbg(DebugLevel.I, "Numeric response {0} is >= 1 and <= {0}.", NumericResponse, MaximumOptions);
						Color SelectedColorInstance;
						switch (NumericResponse)
						{
							case 1: // Input color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedInputColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedInputColor = ColorWheelReturn;
									break;
								}
							case 2: // License color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedLicenseColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedLicenseColor = ColorWheelReturn;
									break;
								}
							case 3: // Continuable kernel error color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedContKernelErrorColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedContKernelErrorColor = ColorWheelReturn;
									break;
								}
							case 4: // Uncontinuable kernel error color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedUncontKernelErrorColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedUncontKernelErrorColor = ColorWheelReturn;
									break;
								}
							case 5: // Host name color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedHostNameShellColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedHostNameShellColor = ColorWheelReturn;
									break;
								}
							case 6: // User name color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedUserNameShellColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedUserNameShellColor = ColorWheelReturn;
									break;
								}
							case 7: // Background color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedBackgroundColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedBackgroundColor = ColorWheelReturn;
									break;
								}
							case 8: // Neutral text color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedNeutralTextColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedNeutralTextColor = ColorWheelReturn;
									break;
								}
							case 9: // list entry color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedListEntryColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedListEntryColor = ColorWheelReturn;
									break;
								}
							case 10: // list value color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedListValueColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedListValueColor = ColorWheelReturn;
									break;
								}
							case 11: // Stage color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedStageColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedStageColor = ColorWheelReturn;
									break;
								}
							case 12: // Error color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedErrorColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedErrorColor = ColorWheelReturn;
									break;
								}
							case 13: // Warning color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedWarningColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedWarningColor = ColorWheelReturn;
									break;
								}
							case 14: // Option color
								{
									SelectedColorInstance = ThemeStudioTools._SelectedOptionColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools._SelectedOptionColor = ColorWheelReturn;
									break;
								}
							case 15: // Banner color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedBannerColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedBannerColor = ColorWheelReturn;
									break;
								}
							case 16: // Notification title color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedNotificationTitleColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedNotificationTitleColor = ColorWheelReturn;
									break;
								}
							case 17: // Notification description color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedNotificationDescriptionColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedNotificationDescriptionColor = ColorWheelReturn;
									break;
								}
							case 18: // Notification progress color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedNotificationProgressColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedNotificationProgressColor = ColorWheelReturn;
									break;
								}
							case 19: // Notification failure color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedNotificationFailureColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedNotificationFailureColor = ColorWheelReturn;
									break;
								}
							case 20: // Question color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedQuestionColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedQuestionColor = ColorWheelReturn;
									break;
								}
							case 21: // Success color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedSuccessColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedSuccessColor = ColorWheelReturn;
									break;
								}
							case 22: // User dollar color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedUserDollarColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedUserDollarColor = ColorWheelReturn;
									break;
								}
							case 23: // Tip color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedTipColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedTipColor = ColorWheelReturn;
									break;
								}
							case 24: // Separator text color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedSeparatorTextColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedSeparatorTextColor = ColorWheelReturn;
									break;
								}
							case 25: // Separator color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedSeparatorColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedSeparatorColor = ColorWheelReturn;
									break;
								}
							case 26: // List title color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedListTitleColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedListTitleColor = ColorWheelReturn;
									break;
								}
							case 27: // Development warning color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedDevelopmentWarningColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedDevelopmentWarningColor = ColorWheelReturn;
									break;
								}
							case 28: // Stage time color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedStageTimeColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedStageTimeColor = ColorWheelReturn;
									break;
								}
							case 29: // Progress color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedProgressColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedProgressColor = ColorWheelReturn;
									break;
								}
							case 30: // Back option color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedBackOptionColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedBackOptionColor = ColorWheelReturn;
									break;
								}
							case 31: // Low priority border color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedLowPriorityBorderColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedLowPriorityBorderColor = ColorWheelReturn;
									break;
								}
							case 32: // Medium priority border color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedMediumPriorityBorderColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedMediumPriorityBorderColor = ColorWheelReturn;
									break;
								}
							case 33: // High priority border color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedHighPriorityBorderColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedHighPriorityBorderColor = ColorWheelReturn;
									break;
								}
							case 34: // Table separator color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedTableSeparatorColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedTableSeparatorColor = ColorWheelReturn;
									break;
								}
							case 35: // Table header color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedTableHeaderColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedTableHeaderColor = ColorWheelReturn;
									break;
								}
							case 36: // Table value color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedTableValueColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedTableValueColor = ColorWheelReturn;
									break;
								}
							case 37: // Selected option color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedSelectedOptionColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedSelectedOptionColor = ColorWheelReturn;
									break;
								}
							case 38: // Selected option color
								{
									SelectedColorInstance = ThemeStudioTools.SelectedAlternativeOptionColor;
									var ColorWheelReturn = ColorSelector.OpenColorSelector(SelectedColorInstance);
									ThemeStudioTools.SelectedAlternativeOptionColor = ColorWheelReturn;
									break;
								}
							case 39: // Save theme to current directory
								{
									ThemeStudioTools.SaveThemeToCurrentDirectory(ThemeName);
									break;
								}
							case 40: // Save theme to another directory...
								{
									DebugWriter.Wdbg(DebugLevel.I, "Prompting user for directory name...");
									TextWriterColor.Write(Translate.DoTranslation("Specify directory to save theme to:") + " [{0}] ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input), CurrentDirectory.CurrentDir);
									string DirectoryName = Input.ReadLine(false);
									DirectoryName = string.IsNullOrWhiteSpace(DirectoryName) ? CurrentDirectory.CurrentDir : DirectoryName;
									DebugWriter.Wdbg(DebugLevel.I, "Got directory name {0}.", DirectoryName);
									ThemeStudioTools.SaveThemeToAnotherDirectory(ThemeName, DirectoryName);
									break;
								}
							case 41: // Save theme to current directory as...
								{
									DebugWriter.Wdbg(DebugLevel.I, "Prompting user for theme name...");
									TextWriterColor.Write(Translate.DoTranslation("Specify theme name:") + " [{0}] ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input), ThemeName);
									string AltThemeName = Input.ReadLine(false);
									AltThemeName = string.IsNullOrWhiteSpace(AltThemeName) ? ThemeName : AltThemeName;
									DebugWriter.Wdbg(DebugLevel.I, "Got theme name {0}.", AltThemeName);
									ThemeStudioTools.SaveThemeToCurrentDirectory(AltThemeName);
									break;
								}
							case 42: // Save theme to another directory as...
								{
									DebugWriter.Wdbg(DebugLevel.I, "Prompting user for theme and directory name...");
									TextWriterColor.Write(Translate.DoTranslation("Specify directory to save theme to:") + " [{0}] ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input), CurrentDirectory.CurrentDir);
									string DirectoryName = Input.ReadLine(false);
									DirectoryName = string.IsNullOrWhiteSpace(DirectoryName) ? CurrentDirectory.CurrentDir : DirectoryName;
									DebugWriter.Wdbg(DebugLevel.I, "Got directory name {0}.", DirectoryName);
									DebugWriter.Wdbg(DebugLevel.I, "Prompting user for theme name...");
									TextWriterColor.Write(Translate.DoTranslation("Specify theme name:") + " [{0}] ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input), ThemeName);
									string AltThemeName = Input.ReadLine(false);
									AltThemeName = string.IsNullOrWhiteSpace(AltThemeName) ? ThemeName : AltThemeName;
									DebugWriter.Wdbg(DebugLevel.I, "Got theme name {0}.", AltThemeName);
									ThemeStudioTools.SaveThemeToAnotherDirectory(AltThemeName, DirectoryName);
									break;
								}
							case 43: // Load Theme From File...
								{
									DebugWriter.Wdbg(DebugLevel.I, "Prompting user for theme name...");
									TextWriterColor.Write(Translate.DoTranslation("Specify theme file name wihout the .json extension:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
									string AltThemeName = Input.ReadLine(false) + ".json";
									DebugWriter.Wdbg(DebugLevel.I, "Got theme name {0}.", AltThemeName);
									ThemeStudioTools.LoadThemeFromFile(AltThemeName);
									break;
								}
							case 44: // Load Theme From Prebuilt Themes...
								{
									DebugWriter.Wdbg(DebugLevel.I, "Prompting user for theme name...");
									TextWriterColor.Write(Translate.DoTranslation("Specify theme name:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
									string AltThemeName = Input.ReadLine(false);
									DebugWriter.Wdbg(DebugLevel.I, "Got theme name {0}.", AltThemeName);
									ThemeStudioTools.LoadThemeFromResource(AltThemeName);
									break;
								}
							case 45: // Load Current Colors
								{
									DebugWriter.Wdbg(DebugLevel.I, "Loading current colors...");
									ThemeStudioTools.LoadThemeFromCurrentColors();
									break;
								}
							case 46: // Preview...
								{
									DebugWriter.Wdbg(DebugLevel.I, "Printing text with colors of theme...");
									ThemeStudioTools.PreparePreview();
									break;
								}
							case 47: // Exit
								{
									DebugWriter.Wdbg(DebugLevel.I, "Exiting studio...");
									StudioExiting = true;
									break;
								}
						}
					}
					else
					{
						DebugWriter.Wdbg(DebugLevel.W, "Option is not valid. Returning...");
						TextWriterColor.Write(Translate.DoTranslation("Specified option {0} is invalid."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), NumericResponse);
						TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
						Input.DetectKeypress();
					}
				}
				else
				{
					DebugWriter.Wdbg(DebugLevel.W, "Answer is not numeric.");
					TextWriterColor.Write(Translate.DoTranslation("The answer must be numeric."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
					TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
					Input.DetectKeypress();
				}
			}

			// Raise event
			Kernel.Kernel.KernelEventManager.RaiseThemeStudioExit();
		}

	}
}