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

using System.IO;
using KS.ConsoleBase.Themes;
using KS.Files;
using KS.Languages;
using KS.Misc.Configuration;
using KS.Misc.Writers.DebugWriters;
using Newtonsoft.Json;
using Terminaux.Colors;
using TermColorTools = Terminaux.Colors.ColorTools;
using Terminaux.Colors.Models.Parsing;

namespace KS.ConsoleBase.Colors
{
	public static class KernelColorTools
	{

		/// <summary>
        /// Enumeration for color types
        /// </summary>
		public enum ColTypes : int
		{
			/// <summary>
            /// Neutral text (for general purposes)
            /// </summary>
			Neutral,
			/// <summary>
            /// Input text
            /// </summary>
			Input,
			/// <summary>
            /// Continuable kernel panic text (usually sync'd with Warning)
            /// </summary>
			Continuable,
			/// <summary>
            /// Uncontinuable kernel panic text (usually sync'd with Error)
            /// </summary>
			Uncontinuable,
			/// <summary>
            /// Host name color
            /// </summary>
			HostName,
			/// <summary>
            /// User name color
            /// </summary>
			UserName,
			/// <summary>
            /// License color
            /// </summary>
			License,
			/// <summary>
            /// Gray color (for special purposes)
            /// </summary>
			Gray,
			/// <summary>
            /// List value text
            /// </summary>
			ListValue,
			/// <summary>
            /// List entry text
            /// </summary>
			ListEntry,
			/// <summary>
            /// Stage text
            /// </summary>
			Stage,
			/// <summary>
            /// Error text
            /// </summary>
			Error,
			/// <summary>
            /// Warning text
            /// </summary>
			Warning,
			/// <summary>
            /// Option text
            /// </summary>
			Option,
			/// <summary>
            /// Banner text
            /// </summary>
			Banner,
			/// <summary>
            /// Notification title text
            /// </summary>
			NotificationTitle,
			/// <summary>
            /// Notification description text
            /// </summary>
			NotificationDescription,
			/// <summary>
            /// Notification progress text
            /// </summary>
			NotificationProgress,
			/// <summary>
            /// Notification failure text
            /// </summary>
			NotificationFailure,
			/// <summary>
            /// Question text
            /// </summary>
			Question,
			/// <summary>
            /// Success text
            /// </summary>
			Success,
			/// <summary>
            /// User dollar sign on shell text
            /// </summary>
			UserDollarSign,
			/// <summary>
            /// Tip text
            /// </summary>
			Tip,
			/// <summary>
            /// Separator text
            /// </summary>
			SeparatorText,
			/// <summary>
            /// Separator color
            /// </summary>
			Separator,
			/// <summary>
            /// List title text
            /// </summary>
			ListTitle,
			/// <summary>
            /// Development warning text
            /// </summary>
			DevelopmentWarning,
			/// <summary>
            /// Stage time text
            /// </summary>
			StageTime,
			/// <summary>
            /// General progress text
            /// </summary>
			Progress,
			/// <summary>
            /// Back option text
            /// </summary>
			BackOption,
			/// <summary>
            /// Low priority notification border color
            /// </summary>
			LowPriorityBorder,
			/// <summary>
            /// Medium priority notification border color
            /// </summary>
			MediumPriorityBorder,
			/// <summary>
            /// High priority notification border color
            /// </summary>
			HighPriorityBorder,
			/// <summary>
            /// Table separator
            /// </summary>
			TableSeparator,
			/// <summary>
            /// Table header
            /// </summary>
			TableHeader,
			/// <summary>
            /// Table value
            /// </summary>
			TableValue,
			/// <summary>
            /// Selected option
            /// </summary>
			SelectedOption,
			/// <summary>
            /// Alternative option
            /// </summary>
			AlternativeOption
		}

		/// <summary>
        /// Color type enumeration
        /// </summary>
		public enum ColorType
		{
			/// <summary>
            /// Color is a true color
            /// </summary>
			TrueColor,
			/// <summary>
            /// Color is a 256-bit color
            /// </summary>
			_255Color
		}

		// Variables for colors used by previous versions of the kernel.
		public static Color InputColor = new(ConsoleColors.White);
		public static Color LicenseColor = new(ConsoleColors.White);
		public static Color ContKernelErrorColor = new(ConsoleColors.Yellow);
		public static Color UncontKernelErrorColor = new(ConsoleColors.Red);
		public static Color HostNameShellColor = new(ConsoleColors.DarkGreen);
		public static Color UserNameShellColor = new(ConsoleColors.Green);
		public static Color BackgroundColor = new(ConsoleColors.Black);
		public static Color NeutralTextColor = new(ConsoleColors.Gray);
		public static Color ListEntryColor = new(ConsoleColors.DarkYellow);
		public static Color ListValueColor = new(ConsoleColors.DarkGray);
		public static Color StageColor = new(ConsoleColors.Green);
		public static Color ErrorColor = new(ConsoleColors.Red);
		public static Color WarningColor = new(ConsoleColors.Yellow);
		public static Color OptionColor = new(ConsoleColors.DarkYellow);
		public static Color BannerColor = new(ConsoleColors.Green);
		public static Color NotificationTitleColor = new(ConsoleColors.White);
		public static Color NotificationDescriptionColor = new(ConsoleColors.Gray);
		public static Color NotificationProgressColor = new(ConsoleColors.DarkYellow);
		public static Color NotificationFailureColor = new(ConsoleColors.Red);
		public static Color QuestionColor = new(ConsoleColors.Yellow);
		public static Color SuccessColor = new(ConsoleColors.Green);
		public static Color UserDollarColor = new(ConsoleColors.Gray);
		public static Color TipColor = new(ConsoleColors.Gray);
		public static Color SeparatorTextColor = new(ConsoleColors.White);
		public static Color SeparatorColor = new(ConsoleColors.Gray);
		public static Color ListTitleColor = new(ConsoleColors.White);
		public static Color DevelopmentWarningColor = new(ConsoleColors.Yellow);
		public static Color StageTimeColor = new(ConsoleColors.Gray);
		public static Color ProgressColor = new(ConsoleColors.DarkYellow);
		public static Color BackOptionColor = new(ConsoleColors.DarkRed);
		public static Color LowPriorityBorderColor = new(ConsoleColors.White);
		public static Color MediumPriorityBorderColor = new(ConsoleColors.Yellow);
		public static Color HighPriorityBorderColor = new(ConsoleColors.Red);
		public static Color TableSeparatorColor = new(ConsoleColors.DarkGray);
		public static Color TableHeaderColor = new(ConsoleColors.White);
		public static Color TableValueColor = new(ConsoleColors.Gray);
		public static Color SelectedOptionColor = new(ConsoleColors.Yellow);
		public static Color AlternativeOptionColor = new(ConsoleColors.DarkGreen);

		/// <summary>
        /// Resets all colors to default
        /// </summary>
		public static void ResetColors()
		{
			DebugWriter.Wdbg(DebugLevel.I, "Resetting colors");
			var DefInfo = new ThemeInfo("_Default");
			InputColor = DefInfo.ThemeInputColor;
			LicenseColor = DefInfo.ThemeLicenseColor;
			ContKernelErrorColor = DefInfo.ThemeContKernelErrorColor;
			UncontKernelErrorColor = DefInfo.ThemeUncontKernelErrorColor;
			HostNameShellColor = DefInfo.ThemeHostNameShellColor;
			UserNameShellColor = DefInfo.ThemeUserNameShellColor;
			BackgroundColor = DefInfo.ThemeBackgroundColor;
			NeutralTextColor = DefInfo.ThemeNeutralTextColor;
			ListEntryColor = DefInfo.ThemeListEntryColor;
			ListValueColor = DefInfo.ThemeListValueColor;
			StageColor = DefInfo.ThemeStageColor;
			ErrorColor = DefInfo.ThemeErrorColor;
			WarningColor = DefInfo.ThemeWarningColor;
			OptionColor = DefInfo.ThemeOptionColor;
			BannerColor = DefInfo.ThemeBannerColor;
			NotificationTitleColor = DefInfo.ThemeNotificationTitleColor;
			NotificationDescriptionColor = DefInfo.ThemeNotificationDescriptionColor;
			NotificationProgressColor = DefInfo.ThemeNotificationProgressColor;
			NotificationFailureColor = DefInfo.ThemeNotificationFailureColor;
			QuestionColor = DefInfo.ThemeQuestionColor;
			SuccessColor = DefInfo.ThemeSuccessColor;
			UserDollarColor = DefInfo.ThemeUserDollarColor;
			TipColor = DefInfo.ThemeTipColor;
			SeparatorTextColor = DefInfo.ThemeSeparatorTextColor;
			SeparatorColor = DefInfo.ThemeSeparatorColor;
			ListTitleColor = DefInfo.ThemeListTitleColor;
			DevelopmentWarningColor = DefInfo.ThemeDevelopmentWarningColor;
			StageTimeColor = DefInfo.ThemeStageTimeColor;
			ProgressColor = DefInfo.ThemeProgressColor;
			BackOptionColor = DefInfo.ThemeBackOptionColor;
			LowPriorityBorderColor = DefInfo.ThemeLowPriorityBorderColor;
			MediumPriorityBorderColor = DefInfo.ThemeMediumPriorityBorderColor;
			HighPriorityBorderColor = DefInfo.ThemeHighPriorityBorderColor;
			TableSeparatorColor = DefInfo.ThemeTableSeparatorColor;
			TableHeaderColor = DefInfo.ThemeTableHeaderColor;
			TableValueColor = DefInfo.ThemeTableValueColor;
			SelectedOptionColor = DefInfo.ThemeSelectedOptionColor;
			AlternativeOptionColor = DefInfo.ThemeAlternativeOptionColor;
			LoadBack();

			// Raise event
			Kernel.Kernel.KernelEventManager.RaiseColorReset();
		}

		/// <summary>
        /// Loads the background
        /// </summary>
		public static void LoadBack()
		{
			TermColorTools.LoadBack(BackgroundColor);
		}

		/// <summary>
        /// Makes the color configuration permanent
        /// </summary>
		public static void MakePermanent()
		{
			Config.ConfigToken["Colors"]["User Name Shell Color"] = UserNameShellColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Host Name Shell Color"] = HostNameShellColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Continuable Kernel Error Color"] = ContKernelErrorColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Uncontinuable Kernel Error Color"] = UncontKernelErrorColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Text Color"] = NeutralTextColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["License Color"] = LicenseColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Background Color"] = BackgroundColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Input Color"] = InputColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["List Entry Color"] = ListEntryColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["List Value Color"] = ListValueColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Kernel Stage Color"] = StageColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Error Text Color"] = ErrorColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Warning Text Color"] = WarningColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Option Color"] = OptionColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Banner Color"] = BannerColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Notification Title Color"] = NotificationTitleColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Notification Description Color"] = NotificationDescriptionColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Notification Progress Color"] = NotificationProgressColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Notification Failure Color"] = NotificationFailureColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Question Color"] = QuestionColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Success Color"] = SuccessColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["User Dollar Color"] = UserDollarColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Tip Color"] = TipColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Separator Text Color"] = SeparatorTextColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Separator Color"] = SeparatorColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["List Title Color"] = ListTitleColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Development Warning Color"] = DevelopmentWarningColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Stage Time Color"] = StageTimeColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Progress Color"] = ProgressColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Back Option Color"] = BackOptionColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Low Priority Border Color"] = LowPriorityBorderColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Medium Priority Border Color"] = MediumPriorityBorderColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["High Priority Border Color"] = HighPriorityBorderColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Table Separator Color"] = TableSeparatorColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Table Header Color"] = TableHeaderColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Table Value Color"] = TableValueColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Selected Option Color"] = SelectedOptionColor.PlainSequenceEnclosed;
			Config.ConfigToken["Colors"]["Alternative Option Color"] = AlternativeOptionColor.PlainSequenceEnclosed;
			File.WriteAllText(Paths.GetKernelPath(KernelPathType.Configuration), JsonConvert.SerializeObject(Config.ConfigToken, Formatting.Indented));
		}

		/// <summary>
        /// Sets custom colors. It only works if colored shell is enabled.
        /// </summary>
        /// <param name="InputColor">Input color</param>
        /// <param name="LicenseColor">License color</param>
        /// <param name="ContKernelErrorColor">Continuable kernel error color</param>
        /// <param name="UncontKernelErrorColor">Uncontinuable kernel error color</param>
        /// <param name="HostNameShellColor">Host name color</param>
        /// <param name="UserNameShellColor">User name color</param>
        /// <param name="BackgroundColor">Background color</param>
        /// <param name="NeutralTextColor">Neutral text color</param>
        /// <param name="ListEntryColor">Command list color</param>
        /// <param name="ListValueColor">Command definition color</param>
        /// <param name="StageColor">Stage color</param>
        /// <param name="ErrorColor">Error color</param>
        /// <param name="WarningColor">Warning color</param>
        /// <param name="OptionColor">Option color</param>
        /// <param name="BannerColor">Banner color</param>
        /// <param name="NotificationTitleColor">Notification title color</param>
        /// <param name="NotificationDescriptionColor">Notification description color</param>
        /// <param name="NotificationProgressColor">Notification progress color</param>
        /// <param name="NotificationFailureColor">Notification failure color</param>
        /// <param name="QuestionColor">Question color</param>
        /// <param name="SuccessColor">Success text color</param>
        /// <param name="UserDollarColor">User dollar color</param>
        /// <param name="TipColor">Tip color</param>
        /// <param name="SeparatorTextColor">Separator text color</param>
        /// <param name="SeparatorColor">Separator color</param>
        /// <param name="ListTitleColor">List title color</param>
        /// <param name="DevelopmentWarningColor">Development warning color</param>
        /// <param name="StageTimeColor">Stage time color</param>
        /// <param name="ProgressColor">Progress color</param>
        /// <param name="BackOptionColor">Back option color</param>
        /// <param name="LowPriorityBorderColor">Low priority notification border color</param>
        /// <param name="MediumPriorityBorderColor">Medium priority notification border color</param>
        /// <param name="HighPriorityBorderColor">High priority notification border color</param>
        /// <param name="TableSeparatorColor">Table separator color</param>
        /// <param name="TableHeaderColor">Table header color</param>
        /// <param name="TableValueColor">Table value color</param>
        /// <param name="SelectedOptionColor">Selected option color</param>
        /// <param name="AlternativeOptionColor">Alternative option color</param>
        /// <returns>True if successful; False if unsuccessful</returns>
		public static bool TrySetColors(string InputColor, string LicenseColor, string ContKernelErrorColor, string UncontKernelErrorColor, string HostNameShellColor, string UserNameShellColor, string BackgroundColor, string NeutralTextColor, string ListEntryColor, string ListValueColor, string StageColor, string ErrorColor, string WarningColor, string OptionColor, string BannerColor, string NotificationTitleColor, string NotificationDescriptionColor, string NotificationProgressColor, string NotificationFailureColor, string QuestionColor, string SuccessColor, string UserDollarColor, string TipColor, string SeparatorTextColor, string SeparatorColor, string ListTitleColor, string DevelopmentWarningColor, string StageTimeColor, string ProgressColor, string BackOptionColor, string LowPriorityBorderColor, string MediumPriorityBorderColor, string HighPriorityBorderColor, string TableSeparatorColor, string TableHeaderColor, string TableValueColor, string SelectedOptionColor, string AlternativeOptionColor)
		{
			try
			{
				SetColors(InputColor, LicenseColor, ContKernelErrorColor, UncontKernelErrorColor, HostNameShellColor, UserNameShellColor, BackgroundColor, NeutralTextColor, ListEntryColor, ListValueColor, StageColor, ErrorColor, WarningColor, OptionColor, BannerColor, NotificationTitleColor, NotificationDescriptionColor, NotificationProgressColor, NotificationFailureColor, QuestionColor, SuccessColor, UserDollarColor, TipColor, SeparatorTextColor, SeparatorColor, ListTitleColor, DevelopmentWarningColor, StageTimeColor, ProgressColor, BackOptionColor, LowPriorityBorderColor, MediumPriorityBorderColor, HighPriorityBorderColor, TableSeparatorColor, TableHeaderColor, TableValueColor, SelectedOptionColor, AlternativeOptionColor);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		/// <summary>
        /// Sets custom colors. It only works if colored shell is enabled.
        /// </summary>
        /// <param name="InputColor">Input color</param>
        /// <param name="LicenseColor">License color</param>
        /// <param name="ContKernelErrorColor">Continuable kernel error color</param>
        /// <param name="UncontKernelErrorColor">Uncontinuable kernel error color</param>
        /// <param name="HostNameShellColor">Host name color</param>
        /// <param name="UserNameShellColor">User name color</param>
        /// <param name="BackgroundColor">Background color</param>
        /// <param name="NeutralTextColor">Neutral text color</param>
        /// <param name="ListEntryColor">Command list color</param>
        /// <param name="ListValueColor">Command definition color</param>
        /// <param name="StageColor">Stage color</param>
        /// <param name="ErrorColor">Error color</param>
        /// <param name="WarningColor">Warning color</param>
        /// <param name="OptionColor">Option color</param>
        /// <param name="BannerColor">Banner color</param>
        /// <param name="NotificationTitleColor">Notification title color</param>
        /// <param name="NotificationDescriptionColor">Notification description color</param>
        /// <param name="NotificationProgressColor">Notification progress color</param>
        /// <param name="NotificationFailureColor">Notification failure color</param>
        /// <param name="QuestionColor">Question color</param>
        /// <param name="SuccessColor">Success text color</param>
        /// <param name="UserDollarColor">User dollar color</param>
        /// <param name="TipColor">Tip color</param>
        /// <param name="SeparatorTextColor">Separator text color</param>
        /// <param name="SeparatorColor">Separator color</param>
        /// <param name="ListTitleColor">List title color</param>
        /// <param name="DevelopmentWarningColor">Development warning color</param>
        /// <param name="StageTimeColor">Stage time color</param>
        /// <param name="ProgressColor">Progress color</param>
        /// <param name="BackOptionColor">Back option color</param>
        /// <param name="LowPriorityBorderColor">Low priority notification border color</param>
        /// <param name="MediumPriorityBorderColor">Medium priority notification border color</param>
        /// <param name="HighPriorityBorderColor">High priority notification border color</param>
        /// <param name="TableSeparatorColor">Table separator color</param>
        /// <param name="TableHeaderColor">Table header color</param>
        /// <param name="TableValueColor">Table value color</param>
        /// <param name="SelectedOptionColor">Selected option color</param>
        /// <param name="AlternativeOptionColor">Alternative option color</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exceptions.ColorException"></exception>
		public static void SetColors(string InputColor, string LicenseColor, string ContKernelErrorColor, string UncontKernelErrorColor, string HostNameShellColor, string UserNameShellColor, string BackgroundColor, string NeutralTextColor, string ListEntryColor, string ListValueColor, string StageColor, string ErrorColor, string WarningColor, string OptionColor, string BannerColor, string NotificationTitleColor, string NotificationDescriptionColor, string NotificationProgressColor, string NotificationFailureColor, string QuestionColor, string SuccessColor, string UserDollarColor, string TipColor, string SeparatorTextColor, string SeparatorColor, string ListTitleColor, string DevelopmentWarningColor, string StageTimeColor, string ProgressColor, string BackOptionColor, string LowPriorityBorderColor, string MediumPriorityBorderColor, string HighPriorityBorderColor, string TableSeparatorColor, string TableHeaderColor, string TableValueColor, string SelectedOptionColor, string AlternativeOptionColor)
		{
			// Check colors for null and set them to "def" if found
			if (string.IsNullOrEmpty(OptionColor))
				OptionColor = "def";
			if (string.IsNullOrEmpty(WarningColor))
				WarningColor = "def";
			if (string.IsNullOrEmpty(ErrorColor))
				ErrorColor = "def";
			if (string.IsNullOrEmpty(StageColor))
				StageColor = "def";
			if (string.IsNullOrEmpty(ListValueColor))
				ListValueColor = "def";
			if (string.IsNullOrEmpty(ListEntryColor))
				ListEntryColor = "def";
			if (string.IsNullOrEmpty(NeutralTextColor))
				NeutralTextColor = "def";
			if (string.IsNullOrEmpty(BackgroundColor))
				BackgroundColor = "def";
			if (string.IsNullOrEmpty(UserNameShellColor))
				UserNameShellColor = "def";
			if (string.IsNullOrEmpty(HostNameShellColor))
				HostNameShellColor = "def";
			if (string.IsNullOrEmpty(UncontKernelErrorColor))
				UncontKernelErrorColor = "def";
			if (string.IsNullOrEmpty(ContKernelErrorColor))
				ContKernelErrorColor = "def";
			if (string.IsNullOrEmpty(LicenseColor))
				LicenseColor = "def";
			if (string.IsNullOrEmpty(InputColor))
				InputColor = "def";
			if (string.IsNullOrEmpty(BannerColor))
				BannerColor = "def";
			if (string.IsNullOrEmpty(NotificationTitleColor))
				NotificationTitleColor = "def";
			if (string.IsNullOrEmpty(NotificationDescriptionColor))
				NotificationDescriptionColor = "def";
			if (string.IsNullOrEmpty(NotificationProgressColor))
				NotificationProgressColor = "def";
			if (string.IsNullOrEmpty(NotificationFailureColor))
				NotificationFailureColor = "def";
			if (string.IsNullOrEmpty(QuestionColor))
				QuestionColor = "def";
			if (string.IsNullOrEmpty(SuccessColor))
				SuccessColor = "def";
			if (string.IsNullOrEmpty(UserDollarColor))
				UserDollarColor = "def";
			if (string.IsNullOrEmpty(TipColor))
				TipColor = "def";
			if (string.IsNullOrEmpty(SeparatorTextColor))
				SeparatorTextColor = "def";
			if (string.IsNullOrEmpty(SeparatorColor))
				SeparatorColor = "def";
			if (string.IsNullOrEmpty(ListTitleColor))
				ListTitleColor = "def";
			if (string.IsNullOrEmpty(DevelopmentWarningColor))
				DevelopmentWarningColor = "def";
			if (string.IsNullOrEmpty(StageTimeColor))
				StageTimeColor = "def";
			if (string.IsNullOrEmpty(ProgressColor))
				ProgressColor = "def";
			if (string.IsNullOrEmpty(BackOptionColor))
				BackOptionColor = "def";
			if (string.IsNullOrEmpty(LowPriorityBorderColor))
				LowPriorityBorderColor = "def";
			if (string.IsNullOrEmpty(MediumPriorityBorderColor))
				MediumPriorityBorderColor = "def";
			if (string.IsNullOrEmpty(HighPriorityBorderColor))
				HighPriorityBorderColor = "def";
			if (string.IsNullOrEmpty(TableSeparatorColor))
				TableSeparatorColor = "def";
			if (string.IsNullOrEmpty(TableHeaderColor))
				TableHeaderColor = "def";
			if (string.IsNullOrEmpty(TableValueColor))
				TableValueColor = "def";
			if (string.IsNullOrEmpty(SelectedOptionColor))
				SelectedOptionColor = "def";
			if (string.IsNullOrEmpty(AlternativeOptionColor))
				AlternativeOptionColor = "def";

			// Set colors
			if (Shell.Shell.ColoredShell == true)
			{
				// Check for defaults
				// We use New Color() to parse entered color. This is to ensure that the kernel can use the correct VT sequence.
				if (InputColor == "def")
					InputColor = new Color(ConsoleColors.White).PlainSequence;
				if (LicenseColor == "def")
					LicenseColor = new Color(ConsoleColors.White).PlainSequence;
				if (ContKernelErrorColor == "def")
					ContKernelErrorColor = new Color(ConsoleColors.Yellow).PlainSequence;
				if (UncontKernelErrorColor == "def")
					UncontKernelErrorColor = new Color(ConsoleColors.Red).PlainSequence;
				if (HostNameShellColor == "def")
					HostNameShellColor = new Color(ConsoleColors.DarkGreen).PlainSequence;
				if (UserNameShellColor == "def")
					UserNameShellColor = new Color(ConsoleColors.Green).PlainSequence;
				if (NeutralTextColor == "def")
					NeutralTextColor = new Color(ConsoleColors.Gray).PlainSequence;
				if (ListEntryColor == "def")
					ListEntryColor = new Color(ConsoleColors.DarkYellow).PlainSequence;
				if (ListValueColor == "def")
					ListValueColor = new Color(ConsoleColors.DarkGray).PlainSequence;
				if (StageColor == "def")
					StageColor = new Color(ConsoleColors.Green).PlainSequence;
				if (ErrorColor == "def")
					ErrorColor = new Color(ConsoleColors.Red).PlainSequence;
				if (WarningColor == "def")
					WarningColor = new Color(ConsoleColors.Yellow).PlainSequence;
				if (OptionColor == "def")
					OptionColor = new Color(ConsoleColors.DarkYellow).PlainSequence;
				if (BannerColor == "def")
					OptionColor = new Color(ConsoleColors.Green).PlainSequence;
				if (NotificationTitleColor == "def")
					NotificationTitleColor = new Color(ConsoleColors.White).PlainSequence;
				if (NotificationDescriptionColor == "def")
					NotificationDescriptionColor = new Color(ConsoleColors.Gray).PlainSequence;
				if (NotificationProgressColor == "def")
					NotificationProgressColor = new Color(ConsoleColors.DarkYellow).PlainSequence;
				if (NotificationFailureColor == "def")
					NotificationFailureColor = new Color(ConsoleColors.Red).PlainSequence;
				if (QuestionColor == "def")
					QuestionColor = new Color(ConsoleColors.Yellow).PlainSequence;
				if (SuccessColor == "def")
					SuccessColor = new Color(ConsoleColors.Green).PlainSequence;
				if (UserDollarColor == "def")
					UserDollarColor = new Color(ConsoleColors.Gray).PlainSequence;
				if (TipColor == "def")
					TipColor = new Color(ConsoleColors.Gray).PlainSequence;
				if (SeparatorTextColor == "def")
					SeparatorTextColor = new Color(ConsoleColors.White).PlainSequence;
				if (SeparatorColor == "def")
					SeparatorColor = new Color(ConsoleColors.Gray).PlainSequence;
				if (ListTitleColor == "def")
					ListTitleColor = new Color(ConsoleColors.White).PlainSequence;
				if (DevelopmentWarningColor == "def")
					DevelopmentWarningColor = new Color(ConsoleColors.Yellow).PlainSequence;
				if (StageTimeColor == "def")
					StageTimeColor = new Color(ConsoleColors.Gray).PlainSequence;
				if (ProgressColor == "def")
					ProgressColor = new Color(ConsoleColors.DarkYellow).PlainSequence;
				if (BackOptionColor == "def")
					BackOptionColor = new Color(ConsoleColors.DarkRed).PlainSequence;
				if (LowPriorityBorderColor == "def")
					LowPriorityBorderColor = new Color(ConsoleColors.White).PlainSequence;
				if (MediumPriorityBorderColor == "def")
					MediumPriorityBorderColor = new Color(ConsoleColors.Yellow).PlainSequence;
				if (HighPriorityBorderColor == "def")
					HighPriorityBorderColor = new Color(ConsoleColors.Red).PlainSequence;
				if (TableSeparatorColor == "def")
					TableSeparatorColor = new Color(ConsoleColors.DarkGray).PlainSequence;
				if (TableHeaderColor == "def")
					TableHeaderColor = new Color(ConsoleColors.White).PlainSequence;
				if (TableValueColor == "def")
					TableValueColor = new Color(ConsoleColors.Gray).PlainSequence;
				if (SelectedOptionColor == "def")
					SelectedOptionColor = new Color(ConsoleColors.Yellow).PlainSequence;
				if (AlternativeOptionColor == "def")
					AlternativeOptionColor = new Color(ConsoleColors.DarkGreen).PlainSequence;
				if (BackgroundColor == "def")
				{
					BackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
					LoadBack();
				}

				// Set the colors
				try
				{
					KernelColorTools.InputColor = new Color(InputColor);
					KernelColorTools.LicenseColor = new Color(LicenseColor);
					KernelColorTools.ContKernelErrorColor = new Color(ContKernelErrorColor);
					KernelColorTools.UncontKernelErrorColor = new Color(UncontKernelErrorColor);
					KernelColorTools.HostNameShellColor = new Color(HostNameShellColor);
					KernelColorTools.UserNameShellColor = new Color(UserNameShellColor);
					KernelColorTools.BackgroundColor = new Color(BackgroundColor);
					KernelColorTools.NeutralTextColor = new Color(NeutralTextColor);
					KernelColorTools.ListEntryColor = new Color(ListEntryColor);
					KernelColorTools.ListValueColor = new Color(ListValueColor);
					KernelColorTools.StageColor = new Color(StageColor);
					KernelColorTools.ErrorColor = new Color(ErrorColor);
					KernelColorTools.WarningColor = new Color(WarningColor);
					KernelColorTools.OptionColor = new Color(OptionColor);
					KernelColorTools.BannerColor = new Color(BannerColor);
					KernelColorTools.NotificationTitleColor = new Color(NotificationTitleColor);
					KernelColorTools.NotificationDescriptionColor = new Color(NotificationDescriptionColor);
					KernelColorTools.NotificationProgressColor = new Color(NotificationProgressColor);
					KernelColorTools.NotificationFailureColor = new Color(NotificationFailureColor);
					KernelColorTools.QuestionColor = new Color(QuestionColor);
					KernelColorTools.SuccessColor = new Color(SuccessColor);
					KernelColorTools.UserDollarColor = new Color(UserDollarColor);
					KernelColorTools.TipColor = new Color(TipColor);
					KernelColorTools.SeparatorTextColor = new Color(SeparatorTextColor);
					KernelColorTools.SeparatorColor = new Color(SeparatorColor);
					KernelColorTools.ListTitleColor = new Color(ListTitleColor);
					KernelColorTools.DevelopmentWarningColor = new Color(DevelopmentWarningColor);
					KernelColorTools.StageTimeColor = new Color(StageTimeColor);
					KernelColorTools.ProgressColor = new Color(ProgressColor);
					KernelColorTools.BackOptionColor = new Color(BackOptionColor);
					KernelColorTools.LowPriorityBorderColor = new Color(LowPriorityBorderColor);
					KernelColorTools.MediumPriorityBorderColor = new Color(MediumPriorityBorderColor);
					KernelColorTools.HighPriorityBorderColor = new Color(HighPriorityBorderColor);
					KernelColorTools.TableSeparatorColor = new Color(TableSeparatorColor);
					KernelColorTools.TableHeaderColor = new Color(TableHeaderColor);
					KernelColorTools.TableValueColor = new Color(TableValueColor);
					KernelColorTools.SelectedOptionColor = new Color(SelectedOptionColor);
					KernelColorTools.AlternativeOptionColor = new Color(AlternativeOptionColor);
					LoadBack();
					MakePermanent();

					// Raise event
					Kernel.Kernel.KernelEventManager.RaiseColorSet();
				}
				catch (Exception ex)
				{
					DebugWriter.WStkTrc(ex);
					Kernel.Kernel.KernelEventManager.RaiseColorSetError(ColorSetErrorReasons.InvalidColors);
					throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("One or more of the colors is invalid.") + " {0}", ex, ex.Message);
				}
			}
			else
			{
				Kernel.Kernel.KernelEventManager.RaiseColorSetError(ColorSetErrorReasons.NoColors);
				throw new InvalidOperationException(Translate.DoTranslation("Colors are not available. Turn on colored shell in the kernel config."));
			}
		}

		/// <summary>
        /// Gets the gray color according to the brightness of the background color
        /// </summary>
		public static Color GetGray()
		{
			if (BackgroundColor.Brightness == ColorBrightness.Light)
			{
				return NeutralTextColor;
			}
			else
			{
				return new Color(ConsoleColors.Gray);
			}
		}

		/// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
		public static void SetConsoleColor(ColTypes colorType)
		{
			SetConsoleColor(colorType, false);
		}

		/// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
		public static void SetConsoleColor(ColTypes colorType, bool Background)
		{
			if (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out))
			{
				switch (colorType)
				{
					case ColTypes.Neutral:
						{
							SetConsoleColor(NeutralTextColor, Background);
							break;
						}
					case ColTypes.Input:
						{
							SetConsoleColor(InputColor, Background);
							break;
						}
					case ColTypes.Continuable:
						{
							SetConsoleColor(ContKernelErrorColor, Background);
							break;
						}
					case ColTypes.Uncontinuable:
						{
							SetConsoleColor(UncontKernelErrorColor, Background);
							break;
						}
					case ColTypes.HostName:
						{
							SetConsoleColor(HostNameShellColor, Background);
							break;
						}
					case ColTypes.UserName:
						{
							SetConsoleColor(UserNameShellColor, Background);
							break;
						}
					case ColTypes.License:
						{
							SetConsoleColor(LicenseColor, Background);
							break;
						}
					case ColTypes.Gray:
						{
							SetConsoleColor(GetGray(), Background);
							break;
						}
					case ColTypes.ListValue:
						{
							SetConsoleColor(ListValueColor, Background);
							break;
						}
					case ColTypes.ListEntry:
						{
							SetConsoleColor(ListEntryColor, Background);
							break;
						}
					case ColTypes.Stage:
						{
							SetConsoleColor(StageColor, Background);
							break;
						}
					case ColTypes.Error:
						{
							SetConsoleColor(ErrorColor, Background);
							break;
						}
					case ColTypes.Warning:
						{
							SetConsoleColor(WarningColor, Background);
							break;
						}
					case ColTypes.Option:
						{
							SetConsoleColor(OptionColor, Background);
							break;
						}
					case ColTypes.Banner:
						{
							SetConsoleColor(BannerColor, Background);
							break;
						}
					case ColTypes.NotificationTitle:
						{
							SetConsoleColor(NotificationTitleColor, Background);
							break;
						}
					case ColTypes.NotificationDescription:
						{
							SetConsoleColor(NotificationDescriptionColor, Background);
							break;
						}
					case ColTypes.NotificationProgress:
						{
							SetConsoleColor(NotificationProgressColor, Background);
							break;
						}
					case ColTypes.NotificationFailure:
						{
							SetConsoleColor(NotificationFailureColor, Background);
							break;
						}
					case ColTypes.Question:
						{
							SetConsoleColor(QuestionColor, Background);
							break;
						}
					case ColTypes.Success:
						{
							SetConsoleColor(SuccessColor, Background);
							break;
						}
					case ColTypes.UserDollarSign:
						{
							SetConsoleColor(UserDollarColor, Background);
							break;
						}
					case ColTypes.Tip:
						{
							SetConsoleColor(TipColor, Background);
							break;
						}
					case ColTypes.SeparatorText:
						{
							SetConsoleColor(SeparatorTextColor, Background);
							break;
						}
					case ColTypes.Separator:
						{
							SetConsoleColor(SeparatorColor, Background);
							break;
						}
					case ColTypes.ListTitle:
						{
							SetConsoleColor(ListTitleColor, Background);
							break;
						}
					case ColTypes.DevelopmentWarning:
						{
							SetConsoleColor(DevelopmentWarningColor, Background);
							break;
						}
					case ColTypes.StageTime:
						{
							SetConsoleColor(StageTimeColor, Background);
							break;
						}
					case ColTypes.Progress:
						{
							SetConsoleColor(ProgressColor, Background);
							break;
						}
					case ColTypes.BackOption:
						{
							SetConsoleColor(BackOptionColor, Background);
							break;
						}
					case ColTypes.LowPriorityBorder:
						{
							SetConsoleColor(LowPriorityBorderColor, Background);
							break;
						}
					case ColTypes.MediumPriorityBorder:
						{
							SetConsoleColor(MediumPriorityBorderColor, Background);
							break;
						}
					case ColTypes.HighPriorityBorder:
						{
							SetConsoleColor(HighPriorityBorderColor, Background);
							break;
						}
					case ColTypes.TableSeparator:
						{
							SetConsoleColor(TableSeparatorColor, Background);
							break;
						}
					case ColTypes.TableHeader:
						{
							SetConsoleColor(TableHeaderColor, Background);
							break;
						}
					case ColTypes.TableValue:
						{
							SetConsoleColor(TableValueColor, Background);
							break;
						}
					case ColTypes.SelectedOption:
						{
							SetConsoleColor(SelectedOptionColor, Background);
							break;
						}
					case ColTypes.AlternativeOption:
						{
							SetConsoleColor(AlternativeOptionColor, Background);
							break;
						}

					default:
						{
							break;
						}
				}
				if (!Background)
					SetConsoleColor(BackgroundColor, true);
			}
		}

		/// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
		public static void SetConsoleColor(Color ColorSequence, bool Background = false)
		{
			if (Shell.Shell.ColoredShell)
			{
				TermColorTools.SetConsoleColor(ColorSequence, Background);
			}
		}

		/// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
		public static bool TrySetConsoleColor(ColTypes colorType)
		{
			return TrySetConsoleColor(colorType, false);
		}

		/// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
		public static bool TrySetConsoleColor(ColTypes colorType, bool Background)
		{
			try
			{
				SetConsoleColor(colorType, Background);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		/// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <returns>True if successful; False if unsuccessful</returns>
		public static bool TrySetConsoleColor(Color ColorSequence, bool Background)
		{
			try
			{
				SetConsoleColor(ColorSequence, Background);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static Color GetConsoleColor(ColTypes colorType)
		{
			switch (colorType)
			{
				case ColTypes.Neutral:
					{
						return NeutralTextColor;
					}
				case ColTypes.Input:
					{
						return InputColor;
					}
				case ColTypes.Continuable:
					{
						return ContKernelErrorColor;
					}
				case ColTypes.Uncontinuable:
					{
						return UncontKernelErrorColor;
					}
				case ColTypes.HostName:
					{
						return HostNameShellColor;
					}
				case ColTypes.UserName:
					{
						return UserNameShellColor;
					}
				case ColTypes.License:
					{
						return LicenseColor;
					}
				case ColTypes.Gray:
					{
						return GetGray();
					}
				case ColTypes.ListValue:
					{
						return ListValueColor;
					}
				case ColTypes.ListEntry:
					{
						return ListEntryColor;
					}
				case ColTypes.Stage:
					{
						return StageColor;
					}
				case ColTypes.Error:
					{
						return ErrorColor;
					}
				case ColTypes.Warning:
					{
						return WarningColor;
					}
				case ColTypes.Option:
					{
						return OptionColor;
					}
				case ColTypes.Banner:
					{
						return BannerColor;
					}
				case ColTypes.NotificationTitle:
					{
						return NotificationTitleColor;
					}
				case ColTypes.NotificationDescription:
					{
						return NotificationDescriptionColor;
					}
				case ColTypes.NotificationProgress:
					{
						return NotificationProgressColor;
					}
				case ColTypes.NotificationFailure:
					{
						return NotificationFailureColor;
					}
				case ColTypes.Question:
					{
						return QuestionColor;
					}
				case ColTypes.Success:
					{
						return SuccessColor;
					}
				case ColTypes.UserDollarSign:
					{
						return UserDollarColor;
					}
				case ColTypes.Tip:
					{
						return TipColor;
					}
				case ColTypes.SeparatorText:
					{
						return SeparatorTextColor;
					}
				case ColTypes.Separator:
					{
						return SeparatorColor;
					}
				case ColTypes.ListTitle:
					{
						return ListTitleColor;
					}
				case ColTypes.DevelopmentWarning:
					{
						return DevelopmentWarningColor;
					}
				case ColTypes.StageTime:
					{
						return StageTimeColor;
					}
				case ColTypes.Progress:
					{
						return ProgressColor;
					}
				case ColTypes.BackOption:
					{
						return BackOptionColor;
					}
				case ColTypes.LowPriorityBorder:
					{
						return LowPriorityBorderColor;
					}
				case ColTypes.MediumPriorityBorder:
					{
						return MediumPriorityBorderColor;
					}
				case ColTypes.HighPriorityBorder:
					{
						return HighPriorityBorderColor;
					}
				case ColTypes.TableSeparator:
					{
						return TableSeparatorColor;
					}
				case ColTypes.TableHeader:
					{
						return TableHeaderColor;
					}
				case ColTypes.TableValue:
					{
						return TableValueColor;
					}
				case ColTypes.SelectedOption:
					{
						return SelectedOptionColor;
					}
				case ColTypes.AlternativeOption:
					{
						return AlternativeOptionColor;
					}

				default:
					{
						return NeutralTextColor;
					}
			}
		}

		/// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, or a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>True if successful; False if failed</returns>
		public static bool TryParseColor(string ColorSpecifier)
		{
			try
			{
				var ColorInstance = new Color(ColorSpecifier);
				return true;
			}
			catch (Exception ex)
			{
				DebugWriter.Wdbg(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message);
				DebugWriter.WStkTrc(ex);
				return false;
			}
		}

		/// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="ColorNum">The color number</param>
        /// <returns>True if successful; False if failed</returns>
		public static bool TryParseColor(int ColorNum)
		{
			try
			{
				var ColorInstance = new Color(ColorNum);
				return true;
			}
			catch (Exception ex)
			{
				DebugWriter.Wdbg(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message);
				DebugWriter.WStkTrc(ex);
				return false;
			}
		}

		/// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>True if successful; False if failed</returns>
		public static bool TryParseColor(int R, int G, int B)
		{
			try
			{
				var ColorInstance = new Color(R, G, B);
				return true;
			}
			catch (Exception ex)
			{
				DebugWriter.Wdbg(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message);
				DebugWriter.WStkTrc(ex);
				return false;
			}
		}

		/// <summary>
        /// Converts from the hexadecimal representation of a color to the RGB sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
		public static string ConvertFromHexToRGB(string Hex)
		{
			var rgb = ParsingTools.ParseSpecifierRgbHash(Hex);
			return $"{rgb.R};{rgb.G};{rgb.B}";
		}

		/// <summary>
        /// Converts from the RGB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
		public static string ConvertFromRGBToHex(string RGBSequence)
		{
			Color rgb = RGBSequence;
			return rgb.Hex;
		}

		/// <summary>
        /// Converts from the RGB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
		public static string ConvertFromRGBToHex(int R, int G, int B)
		{
			Color rgb = $"{R};{G};{B}";
			return rgb.Hex;
		}

	}
}