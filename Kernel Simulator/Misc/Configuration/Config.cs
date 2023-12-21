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

using System.Globalization;
using System.IO;
using FluentFTP;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Themes;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Kernel;
using KS.Languages;
using KS.Login;
using KS.ManPages;
using KS.Misc.Editors.JsonShell;
using KS.Misc.Editors.TextEdit;
using KS.Misc.Games;
using KS.Misc.Notifications;
using KS.Misc.Probers;
using KS.Misc.Screensaver.Displays;
using KS.Misc.Splash;
using KS.Misc.Text;
using KS.Misc.Timers;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Writers.FancyWriters.Tools;
using KS.Misc.Writers.MiscWriters;
using KS.Modifications;
using KS.Network;
using KS.Network.FTP;
using KS.Network.Mail;
using KS.Network.Mail.Directory;
using KS.Network.RemoteDebug;
using KS.Network.RPC;
using KS.Network.RSS;
using KS.Network.SFTP;
using KS.Network.SSH;
using KS.Network.Transfer;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Shells;
using MimeKit.Text;
using Nettify.Weather;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terminaux.Colors;
using Terminaux.Inputs.Styles.Choice;

namespace KS.Misc.Configuration
{
	public static class Config
	{

		/// <summary>
        /// Base config token to be loaded each kernel startup.
        /// </summary>
		internal static JObject ConfigToken;

		/// <summary>
        /// Config category enumeration
        /// </summary>
		public enum ConfigCategory
		{
			/// <summary>
            /// All general kernel settings, mainly for maintaining the kernel.
            /// </summary>
			General,
			/// <summary>
            /// Color settings
            /// </summary>
			Colors,
			/// <summary>
            /// Hardware settings
            /// </summary>
			Hardware,
			/// <summary>
            /// Login settings
            /// </summary>
			Login,
			/// <summary>
            /// Shell settings
            /// </summary>
			Shell,
			/// <summary>
            /// Filesystem settings
            /// </summary>
			Filesystem,
			/// <summary>
            /// Network settings
            /// </summary>
			Network,
			/// <summary>
            /// Screensaver settings
            /// </summary>
			Screensaver,
			/// <summary>
            /// Miscellaneous settings
            /// </summary>
			Misc
		}

		/// <summary>
        /// Creates a new JSON object containing the kernel settings of all kinds
        /// </summary>
        /// <returns>A pristine config object</returns>
		public static JObject GetNewConfigObject()
		{
			var ConfigurationObject = new JObject();

			// The General Section
			var GeneralConfig = new JObject() { { "Prompt for Arguments on Boot", Flags.ArgsOnBoot }, { "Maintenance Mode", Flags.Maintenance }, { "Check for Updates on Startup", Flags.CheckUpdateStart }, { "Custom Startup Banner", WelcomeMessage.CustomBanner }, { "Change Culture when Switching Languages", Flags.LangChangeCulture }, { "Language", LanguageManager.CurrentLanguage }, { "Culture", CultureManager.CurrentCult.Name }, { "Show app information during boot", Flags.ShowAppInfoOnBoot }, { "Parse command-line arguments", Flags.ParseCommandLineArguments }, { "Show stage finish times", Flags.ShowStageFinishTimes }, { "Start kernel modifications on boot", Flags.StartKernelMods }, { "Show current time before login", Flags.ShowCurrentTimeBeforeLogin }, { "Notify for any fault during boot", Flags.NotifyFaultsBoot }, { "Show stack trace on kernel error", Flags.ShowStackTraceOnKernelError }, { "Automatically download updates", Flags.AutoDownloadUpdate }, { "Enable event debugging", Flags.EventDebug }, { "New welcome banner", Flags.NewWelcomeStyle }, { "Stylish splash screen", Flags.EnableSplash }, { "Splash name", SplashManager.SplashName }, { "Banner figlet font", KernelTools.BannerFigletFont }, { "Simulate No APM Mode", Flags.SimulateNoAPM } };
			ConfigurationObject.Add("General", GeneralConfig);

			// The Colors Section
			var ColorConfig = new JObject() { { "User Name Shell Color", KernelColorTools.UserNameShellColor.PlainSequenceEnclosed }, { "Host Name Shell Color", KernelColorTools.HostNameShellColor.PlainSequenceEnclosed }, { "Continuable Kernel Error Color", KernelColorTools.ContKernelErrorColor.PlainSequenceEnclosed }, { "Uncontinuable Kernel Error Color", KernelColorTools.UncontKernelErrorColor.PlainSequenceEnclosed }, { "Text Color", KernelColorTools.NeutralTextColor.PlainSequenceEnclosed }, { "License Color", KernelColorTools.LicenseColor.PlainSequenceEnclosed }, { "Background Color", KernelColorTools.BackgroundColor.PlainSequenceEnclosed }, { "Input Color", KernelColorTools.InputColor.PlainSequenceEnclosed }, { "List Entry Color", KernelColorTools.ListEntryColor.PlainSequenceEnclosed }, { "List Value Color", KernelColorTools.ListValueColor.PlainSequenceEnclosed }, { "Kernel Stage Color", KernelColorTools.StageColor.PlainSequenceEnclosed }, { "Error Text Color", KernelColorTools.ErrorColor.PlainSequenceEnclosed }, { "Warning Text Color", KernelColorTools.WarningColor.PlainSequenceEnclosed }, { "Option Color", KernelColorTools.OptionColor.PlainSequenceEnclosed }, { "Banner Color", KernelColorTools.BannerColor.PlainSequenceEnclosed }, { "Notification Title Color", KernelColorTools.NotificationTitleColor.PlainSequenceEnclosed }, { "Notification Description Color", KernelColorTools.NotificationDescriptionColor.PlainSequenceEnclosed }, { "Notification Progress Color", KernelColorTools.NotificationProgressColor.PlainSequenceEnclosed }, { "Notification Failure Color", KernelColorTools.NotificationFailureColor.PlainSequenceEnclosed }, { "Question Color", KernelColorTools.QuestionColor.PlainSequenceEnclosed }, { "Success Color", KernelColorTools.SuccessColor.PlainSequenceEnclosed }, { "User Dollar Color", KernelColorTools.UserDollarColor.PlainSequenceEnclosed }, { "Tip Color", KernelColorTools.TipColor.PlainSequenceEnclosed }, { "Separator Text Color", KernelColorTools.SeparatorTextColor.PlainSequenceEnclosed }, { "Separator Color", KernelColorTools.SeparatorColor.PlainSequenceEnclosed }, { "List Title Color", KernelColorTools.ListTitleColor.PlainSequenceEnclosed }, { "Development Warning Color", KernelColorTools.DevelopmentWarningColor.PlainSequenceEnclosed }, { "Stage Time Color", KernelColorTools.StageTimeColor.PlainSequenceEnclosed }, { "Progress Color", KernelColorTools.ProgressColor.PlainSequenceEnclosed }, { "Back Option Color", KernelColorTools.BackOptionColor.PlainSequenceEnclosed }, { "Low Priority Border Color", KernelColorTools.LowPriorityBorderColor.PlainSequenceEnclosed }, { "Medium Priority Border Color", KernelColorTools.MediumPriorityBorderColor.PlainSequenceEnclosed }, { "High Priority Border Color", KernelColorTools.HighPriorityBorderColor.PlainSequenceEnclosed }, { "Table Separator Color", KernelColorTools.TableSeparatorColor.PlainSequenceEnclosed }, { "Table Header Color", KernelColorTools.TableHeaderColor.PlainSequenceEnclosed }, { "Table Value Color", KernelColorTools.TableValueColor.PlainSequenceEnclosed }, { "Selected Option Color", KernelColorTools.SelectedOptionColor.PlainSequenceEnclosed }, { "Alternative Option Color", KernelColorTools.AlternativeOptionColor.PlainSequenceEnclosed } };
			ConfigurationObject.Add("Colors", ColorConfig);

			// The Hardware Section
			var HardwareConfig = new JObject() { { "Quiet Probe", Flags.QuietHardwareProbe }, { "Full Probe", Flags.FullHardwareProbe }, { "Verbose Probe", Flags.VerboseHardwareProbe } };
			ConfigurationObject.Add("Hardware", HardwareConfig);

			// The Login Section
			var LoginConfig = new JObject() { { "Show MOTD on Log-in", Flags.ShowMOTD }, { "Clear Screen on Log-in", Flags.ClearOnLogin }, { "Host Name", Kernel.Kernel.HostName }, { "Show available usernames", Flags.ShowAvailableUsers }, { "MOTD Path", MOTDParse.MOTDFilePath }, { "MAL Path", MOTDParse.MALFilePath }, { "Username prompt style", Login.Login.UsernamePrompt }, { "Password prompt style", Login.Login.PasswordPrompt }, { "Show MAL on Log-in", Flags.ShowMAL }, { "Include anonymous users", UserManagement.IncludeAnonymous }, { "Include disabled users", UserManagement.IncludeDisabled } };
			ConfigurationObject.Add("Login", LoginConfig);

			// The Shell Section
			var ShellConfig = new JObject() { { "Colored Shell", Shell.Shell.ColoredShell }, { "Simplified Help Command", Flags.SimHelp }, { "Current Directory", CurrentDirectory.CurrentDir }, { "Lookup Directories", $"\"{Shell.Shell.PathsToLookup}\"" }, { "Prompt Preset", PromptPresetManager.UESHShellCurrentPreset.PresetName }, { "FTP Prompt Preset", PromptPresetManager.FTPShellCurrentPreset.PresetName }, { "Mail Prompt Preset", PromptPresetManager.MailShellCurrentPreset.PresetName }, { "SFTP Prompt Preset", PromptPresetManager.SFTPShellCurrentPreset.PresetName }, { "RSS Prompt Preset", PromptPresetManager.RSSShellCurrentPreset.PresetName }, { "Text Edit Prompt Preset", PromptPresetManager.TextShellCurrentPreset.PresetName }, { "Zip Shell Prompt Preset", PromptPresetManager.ZipShellCurrentPreset.PresetName }, { "Test Shell Prompt Preset", PromptPresetManager.TestShellCurrentPreset.PresetName }, { "JSON Shell Prompt Preset", PromptPresetManager.JsonShellCurrentPreset.PresetName }, { "Hex Edit Prompt Preset", PromptPresetManager.HexShellCurrentPreset.PresetName }, { "HTTP Shell Prompt Preset", PromptPresetManager.HTTPShellCurrentPreset.PresetName }, { "RAR Shell Prompt Preset", PromptPresetManager.RARShellCurrentPreset.PresetName }, { "Probe injected commands", Flags.ProbeInjectedCommands }, { "Start color wheel in true color mode", Flags.ColorWheelTrueColor }, { "Default choice output type", (JToken)ConsoleBase.Inputs.Styles.ChoiceStyle.DefaultChoiceOutputType } };
			ConfigurationObject.Add("Shell", ShellConfig);

			// The Filesystem Section
			var FilesystemConfig = new JObject() { { "Filesystem sort mode", Listing.SortMode.ToString() }, { "Filesystem sort direction", Listing.SortDirection.ToString() }, { "Show Hidden Files", Flags.HiddenFiles }, { "Size parse mode", Flags.FullParseMode }, { "Show progress on filesystem operations", Filesystem.ShowFilesystemProgress }, { "Show file details in list", Listing.ShowFileDetailsList }, { "Suppress unauthorized messages", Flags.SuppressUnauthorizedMessages }, { "Print line numbers on printing file contents", Flags.PrintLineNumbers }, { "Sort the list", Listing.SortList }, { "Show total size in list", Listing.ShowTotalSizeInList } };
			ConfigurationObject.Add("Filesystem", FilesystemConfig);

			// The Network Section
			var NetworkConfig = new JObject() { { "Debug Port", RemoteDebugger.DebugPort }, { "Download Retry Times", NetworkTools.DownloadRetries }, { "Upload Retry Times", NetworkTools.UploadRetries }, { "Show progress bar while downloading or uploading from \"get\" or \"put\" command", Flags.ShowProgress }, { "Log FTP username", Flags.FTPLoggerUsername }, { "Log FTP IP address", Flags.FTPLoggerIP }, { "Return only first FTP profile", Flags.FTPFirstProfileOnly }, { "Show mail message preview", MailManager.ShowPreview }, { "Record chat to debug log", Flags.RecordChatToDebugLog }, { "Show SSH banner", SSH.SSHBanner }, { "Enable RPC", RemoteProcedure.RPCEnabled }, { "RPC Port", RemoteProcedure.RPCPort }, { "Show file details in FTP list", FTPShellCommon.FtpShowDetailsInList }, { "Username prompt style for FTP", FTPShellCommon.FtpUserPromptStyle }, { "Password prompt style for FTP", FTPShellCommon.FtpPassPromptStyle }, { "Use first FTP profile", FTPShellCommon.FtpUseFirstProfile }, { "Add new connections to FTP speed dial", FTPShellCommon.FtpNewConnectionsToSpeedDial }, { "Try to validate secure FTP certificates", FTPShellCommon.FtpTryToValidateCertificate }, { "Show FTP MOTD on connection", FTPShellCommon.FtpShowMotd }, { "Always accept invalid FTP certificates", FTPShellCommon.FtpAlwaysAcceptInvalidCerts }, { "Username prompt style for mail", MailLogin.Mail_UserPromptStyle }, { "Password prompt style for mail", MailLogin.Mail_PassPromptStyle }, { "IMAP prompt style for mail", MailLogin.Mail_IMAPPromptStyle }, { "SMTP prompt style for mail", MailLogin.Mail_SMTPPromptStyle }, { "Automatically detect mail server", MailLogin.Mail_AutoDetectServer }, { "Enable mail debug", MailLogin.Mail_Debug }, { "Notify for new mail messages", MailShellCommon.Mail_NotifyNewMail }, { "GPG password prompt style for mail", MailLogin.Mail_GPGPromptStyle }, { "Send IMAP ping interval", MailShellCommon.Mail_ImapPingInterval }, { "Send SMTP ping interval", MailShellCommon.Mail_SmtpPingInterval }, { "Mail text format", MailShellCommon.Mail_TextFormat.ToString() }, { "Automatically start remote debug on startup", RemoteDebugger.RDebugAutoStart }, { "Remote debug message format", RemoteDebugger.RDebugMessageFormat }, { "RSS feed URL prompt style", RSSShellCommon.RSSFeedUrlPromptStyle }, { "Auto refresh RSS feed", RSSShellCommon.RSSRefreshFeeds }, { "Auto refresh RSS feed interval", RSSShellCommon.RSSRefreshInterval }, { "Show file details in SFTP list", SFTPShellCommon.SFTPShowDetailsInList }, { "Username prompt style for SFTP", SFTPShellCommon.SFTPUserPromptStyle }, { "Add new connections to SFTP speed dial", SFTPShellCommon.SFTPNewConnectionsToSpeedDial }, { "Ping timeout", NetworkTools.PingTimeout }, { "Show extensive adapter info", Flags.ExtensiveAdapterInformation }, { "Show general network information", Flags.GeneralNetworkInformation }, { "Download percentage text", NetworkTransfer.DownloadPercentagePrint }, { "Upload percentage text", NetworkTransfer.UploadPercentagePrint }, { "Recursive hashing for FTP", FTPShellCommon.FtpRecursiveHashing }, { "Maximum number of e-mails in one page", MailShellCommon.Mail_MaxMessagesInPage }, { "Show mail transfer progress", MailShellCommon.Mail_ShowProgress }, { "Mail transfer progress", MailShellCommon.Mail_ProgressStyle }, { "Mail transfer progress (single)", MailShellCommon.Mail_ProgressStyleSingle }, { "Show notification for download progress", NetworkTransfer.DownloadNotificationProvoke }, { "Show notification for upload progress", NetworkTransfer.UploadNotificationProvoke }, { "RSS feed fetch timeout", RSSShellCommon.RSSFetchTimeout }, { "Verify retry attempts for FTP transmission", FTPShellCommon.FtpVerifyRetryAttempts }, { "FTP connection timeout", FTPShellCommon.FtpConnectTimeout }, { "FTP data connection timeout", FTPShellCommon.FtpDataConnectTimeout }, { "FTP IP versions", (JToken)FTPShellCommon.FtpProtocolVersions }, { "Notify on remote debug connection error", Flags.NotifyOnRemoteDebugConnectionError } };
			ConfigurationObject.Add("Network", NetworkConfig);

			// The Screensaver Section
			var ScreensaverConfig = new JObject() { { "Screensaver", Screensaver.Screensaver.DefSaverName }, { "Screensaver Timeout in ms", Screensaver.Screensaver.ScrnTimeout }, { "Enable screensaver debugging", Screensaver.Screensaver.ScreensaverDebug }, { "Ask for password after locking", Screensaver.Screensaver.PasswordLock } };

			// ColorMix config json object
			var ColorMixConfig = new JObject() { { "Activate 255 Color Mode", ColorMixSettings.ColorMix255Colors }, { "Activate True Color Mode", ColorMixSettings.ColorMixTrueColor }, { "Delay in Milliseconds", ColorMixSettings.ColorMixDelay }, { "Background color", new Color(ColorMixSettings.ColorMixBackgroundColor).Type == ColorType.TrueColor ? $"\"{ColorMixSettings.ColorMixBackgroundColor}\"" : ColorMixSettings.ColorMixBackgroundColor }, { "Minimum red color level", ColorMixSettings.ColorMixMinimumRedColorLevel }, { "Minimum green color level", ColorMixSettings.ColorMixMinimumGreenColorLevel }, { "Minimum blue color level", ColorMixSettings.ColorMixMinimumBlueColorLevel }, { "Minimum color level", ColorMixSettings.ColorMixMinimumColorLevel }, { "Maximum red color level", ColorMixSettings.ColorMixMaximumRedColorLevel }, { "Maximum green color level", ColorMixSettings.ColorMixMaximumGreenColorLevel }, { "Maximum blue color level", ColorMixSettings.ColorMixMaximumBlueColorLevel }, { "Maximum color level", ColorMixSettings.ColorMixMaximumColorLevel } };
			ScreensaverConfig.Add("ColorMix", ColorMixConfig);

			// Disco config json object
			var DiscoConfig = new JObject() { { "Activate 255 Color Mode", DiscoSettings.Disco255Colors }, { "Activate True Color Mode", DiscoSettings.DiscoTrueColor }, { "Delay in Milliseconds", DiscoSettings.DiscoDelay }, { "Use Beats Per Minute", DiscoSettings.DiscoUseBeatsPerMinute }, { "Cycle Colors", DiscoSettings.DiscoCycleColors }, { "Enable Black and White Mode", DiscoSettings.DiscoEnableFedMode }, { "Minimum red color level", DiscoSettings.DiscoMinimumRedColorLevel }, { "Minimum green color level", DiscoSettings.DiscoMinimumGreenColorLevel }, { "Minimum blue color level", DiscoSettings.DiscoMinimumBlueColorLevel }, { "Minimum color level", DiscoSettings.DiscoMinimumColorLevel }, { "Maximum red color level", DiscoSettings.DiscoMaximumRedColorLevel }, { "Maximum green color level", DiscoSettings.DiscoMaximumGreenColorLevel }, { "Maximum blue color level", DiscoSettings.DiscoMaximumBlueColorLevel }, { "Maximum color level", DiscoSettings.DiscoMaximumColorLevel } };
			ScreensaverConfig.Add("Disco", DiscoConfig);

			// GlitterColor config json object
			var GlitterColorConfig = new JObject() { { "Activate 255 Color Mode", GlitterColorSettings.GlitterColor255Colors }, { "Activate True Color Mode", GlitterColorSettings.GlitterColorTrueColor }, { "Delay in Milliseconds", GlitterColorSettings.GlitterColorDelay }, { "Minimum red color level", GlitterColorSettings.GlitterColorMinimumRedColorLevel }, { "Minimum green color level", GlitterColorSettings.GlitterColorMinimumGreenColorLevel }, { "Minimum blue color level", GlitterColorSettings.GlitterColorMinimumBlueColorLevel }, { "Minimum color level", GlitterColorSettings.GlitterColorMinimumColorLevel }, { "Maximum red color level", GlitterColorSettings.GlitterColorMaximumRedColorLevel }, { "Maximum green color level", GlitterColorSettings.GlitterColorMaximumGreenColorLevel }, { "Maximum blue color level", GlitterColorSettings.GlitterColorMaximumBlueColorLevel }, { "Maximum color level", GlitterColorSettings.GlitterColorMaximumColorLevel } };
			ScreensaverConfig.Add("GlitterColor", GlitterColorConfig);

			// Lines config json object
			var LinesConfig = new JObject() { { "Activate 255 Color Mode", LinesSettings.Lines255Colors }, { "Activate True Color Mode", LinesSettings.LinesTrueColor }, { "Delay in Milliseconds", LinesSettings.LinesDelay }, { "Line character", LinesSettings.LinesLineChar }, { "Background color", new Color(LinesSettings.LinesBackgroundColor).Type == ColorType.TrueColor ? $"\"{LinesSettings.LinesBackgroundColor}\"" : LinesSettings.LinesBackgroundColor }, { "Minimum red color level", LinesSettings.LinesMinimumRedColorLevel }, { "Minimum green color level", LinesSettings.LinesMinimumGreenColorLevel }, { "Minimum blue color level", LinesSettings.LinesMinimumBlueColorLevel }, { "Minimum color level", LinesSettings.LinesMinimumColorLevel }, { "Maximum red color level", LinesSettings.LinesMaximumRedColorLevel }, { "Maximum green color level", LinesSettings.LinesMaximumGreenColorLevel }, { "Maximum blue color level", LinesSettings.LinesMaximumBlueColorLevel }, { "Maximum color level", LinesSettings.LinesMaximumColorLevel } };
			ScreensaverConfig.Add("Lines", LinesConfig);

			// Dissolve config json object
			var DissolveConfig = new JObject() { { "Activate 255 Color Mode", DissolveSettings.Dissolve255Colors }, { "Activate True Color Mode", DissolveSettings.DissolveTrueColor }, { "Background color", new Color(DissolveSettings.DissolveBackgroundColor).Type == ColorType.TrueColor ? $"\"{DissolveSettings.DissolveBackgroundColor}\"" : DissolveSettings.DissolveBackgroundColor }, { "Minimum red color level", DissolveSettings.DissolveMinimumRedColorLevel }, { "Minimum green color level", DissolveSettings.DissolveMinimumGreenColorLevel }, { "Minimum blue color level", DissolveSettings.DissolveMinimumBlueColorLevel }, { "Minimum color level", DissolveSettings.DissolveMinimumColorLevel }, { "Maximum red color level", DissolveSettings.DissolveMaximumRedColorLevel }, { "Maximum green color level", DissolveSettings.DissolveMaximumGreenColorLevel }, { "Maximum blue color level", DissolveSettings.DissolveMaximumBlueColorLevel }, { "Maximum color level", DissolveSettings.DissolveMaximumColorLevel } };
			ScreensaverConfig.Add("Dissolve", DissolveConfig);

			// BouncingBlock config json object
			var BouncingBlockConfig = new JObject() { { "Activate 255 Color Mode", BouncingBlockSettings.BouncingBlock255Colors }, { "Activate True Color Mode", BouncingBlockSettings.BouncingBlockTrueColor }, { "Delay in Milliseconds", BouncingBlockSettings.BouncingBlockDelay }, { "Background color", new Color(BouncingBlockSettings.BouncingBlockBackgroundColor).Type == ColorType.TrueColor ? $"\"{BouncingBlockSettings.BouncingBlockBackgroundColor}\"" : BouncingBlockSettings.BouncingBlockBackgroundColor }, { "Foreground color", new Color(BouncingBlockSettings.BouncingBlockForegroundColor).Type == ColorType.TrueColor ? $"\"{BouncingBlockSettings.BouncingBlockForegroundColor}\"" : BouncingBlockSettings.BouncingBlockForegroundColor }, { "Minimum red color level", BouncingBlockSettings.BouncingBlockMinimumRedColorLevel }, { "Minimum green color level", BouncingBlockSettings.BouncingBlockMinimumGreenColorLevel }, { "Minimum blue color level", BouncingBlockSettings.BouncingBlockMinimumBlueColorLevel }, { "Minimum color level", BouncingBlockSettings.BouncingBlockMinimumColorLevel }, { "Maximum red color level", BouncingBlockSettings.BouncingBlockMaximumRedColorLevel }, { "Maximum green color level", BouncingBlockSettings.BouncingBlockMaximumGreenColorLevel }, { "Maximum blue color level", BouncingBlockSettings.BouncingBlockMaximumBlueColorLevel }, { "Maximum color level", BouncingBlockSettings.BouncingBlockMaximumColorLevel } };
			ScreensaverConfig.Add("BouncingBlock", BouncingBlockConfig);

			// ProgressClock config json object
			var ProgressClockConfig = new JObject() { { "Activate 255 Color Mode", ProgressClockSettings.ProgressClock255Colors }, { "Activate True Color Mode", ProgressClockSettings.ProgressClockTrueColor }, { "Cycle Colors", ProgressClockSettings.ProgressClockCycleColors }, { "Ticks to change color", ProgressClockSettings.ProgressClockCycleColorsTicks }, { "Color of Seconds Bar", ProgressClockSettings.ProgressClockSecondsProgressColor }, { "Color of Minutes Bar", ProgressClockSettings.ProgressClockMinutesProgressColor }, { "Color of Hours Bar", ProgressClockSettings.ProgressClockHoursProgressColor }, { "Color of Information", ProgressClockSettings.ProgressClockProgressColor }, { "Delay in Milliseconds", ProgressClockSettings.ProgressClockDelay }, { "Upper left corner character for hours bar", ProgressClockSettings.ProgressClockUpperLeftCornerCharHours }, { "Upper left corner character for minutes bar", ProgressClockSettings.ProgressClockUpperLeftCornerCharMinutes }, { "Upper left corner character for seconds bar", ProgressClockSettings.ProgressClockUpperLeftCornerCharSeconds }, { "Upper right corner character for hours bar", ProgressClockSettings.ProgressClockUpperRightCornerCharHours }, { "Upper right corner character for minutes bar", ProgressClockSettings.ProgressClockUpperRightCornerCharMinutes }, { "Upper right corner character for seconds bar", ProgressClockSettings.ProgressClockUpperRightCornerCharSeconds }, { "Lower left corner character for hours bar", ProgressClockSettings.ProgressClockLowerRightCornerCharHours }, { "Lower left corner character for minutes bar", ProgressClockSettings.ProgressClockLowerLeftCornerCharMinutes }, { "Lower left corner character for seconds bar", ProgressClockSettings.ProgressClockLowerLeftCornerCharSeconds }, { "Lower right corner character for hours bar", ProgressClockSettings.ProgressClockLowerRightCornerCharHours }, { "Lower right corner character for minutes bar", ProgressClockSettings.ProgressClockLowerRightCornerCharMinutes }, { "Lower right corner character for seconds bar", ProgressClockSettings.ProgressClockLowerRightCornerCharSeconds }, { "Upper frame character for hours bar", ProgressClockSettings.ProgressClockUpperFrameCharHours }, { "Upper frame character for minutes bar", ProgressClockSettings.ProgressClockUpperFrameCharMinutes }, { "Upper frame character for seconds bar", ProgressClockSettings.ProgressClockUpperFrameCharSeconds }, { "Lower frame character for hours bar", ProgressClockSettings.ProgressClockLowerFrameCharHours }, { "Lower frame character for minutes bar", ProgressClockSettings.ProgressClockLowerFrameCharMinutes }, { "Lower frame character for seconds bar", ProgressClockSettings.ProgressClockLowerFrameCharSeconds }, { "Left frame character for hours bar", ProgressClockSettings.ProgressClockLeftFrameCharHours }, { "Left frame character for minutes bar", ProgressClockSettings.ProgressClockLeftFrameCharMinutes }, { "Left frame character for seconds bar", ProgressClockSettings.ProgressClockLeftFrameCharSeconds }, { "Right frame character for hours bar", ProgressClockSettings.ProgressClockRightFrameCharHours }, { "Right frame character for minutes bar", ProgressClockSettings.ProgressClockRightFrameCharMinutes }, { "Right frame character for seconds bar", ProgressClockSettings.ProgressClockRightFrameCharSeconds }, { "Information text for hours", ProgressClockSettings.ProgressClockInfoTextHours }, { "Information text for minutes", ProgressClockSettings.ProgressClockInfoTextMinutes }, { "Information text for seconds", ProgressClockSettings.ProgressClockInfoTextSeconds }, { "Minimum red color level for hours", ProgressClockSettings.ProgressClockMinimumRedColorLevelHours }, { "Minimum green color level for hours", ProgressClockSettings.ProgressClockMinimumGreenColorLevelHours }, { "Minimum blue color level for hours", ProgressClockSettings.ProgressClockMinimumBlueColorLevelHours }, { "Minimum color level for hours", ProgressClockSettings.ProgressClockMinimumColorLevelHours }, { "Maximum red color level for hours", ProgressClockSettings.ProgressClockMaximumRedColorLevelHours }, { "Maximum green color level for hours", ProgressClockSettings.ProgressClockMaximumGreenColorLevelHours }, { "Maximum blue color level for hours", ProgressClockSettings.ProgressClockMaximumBlueColorLevelHours }, { "Maximum color level for hours", ProgressClockSettings.ProgressClockMaximumColorLevelHours }, { "Minimum red color level for minutes", ProgressClockSettings.ProgressClockMinimumRedColorLevelMinutes }, { "Minimum green color level for minutes", ProgressClockSettings.ProgressClockMinimumGreenColorLevelMinutes }, { "Minimum blue color level for minutes", ProgressClockSettings.ProgressClockMinimumBlueColorLevelMinutes }, { "Minimum color level for minutes", ProgressClockSettings.ProgressClockMinimumColorLevelMinutes }, { "Maximum red color level for minutes", ProgressClockSettings.ProgressClockMaximumRedColorLevelMinutes }, { "Maximum green color level for minutes", ProgressClockSettings.ProgressClockMaximumGreenColorLevelMinutes }, { "Maximum blue color level for minutes", ProgressClockSettings.ProgressClockMaximumBlueColorLevelMinutes }, { "Maximum color level for minutes", ProgressClockSettings.ProgressClockMaximumColorLevelMinutes }, { "Minimum red color level for seconds", ProgressClockSettings.ProgressClockMinimumRedColorLevelSeconds }, { "Minimum green color level for seconds", ProgressClockSettings.ProgressClockMinimumGreenColorLevelSeconds }, { "Minimum blue color level for seconds", ProgressClockSettings.ProgressClockMinimumBlueColorLevelSeconds }, { "Minimum color level for seconds", ProgressClockSettings.ProgressClockMinimumColorLevelSeconds }, { "Maximum red color level for seconds", ProgressClockSettings.ProgressClockMaximumRedColorLevelSeconds }, { "Maximum green color level for seconds", ProgressClockSettings.ProgressClockMaximumGreenColorLevelSeconds }, { "Maximum blue color level for seconds", ProgressClockSettings.ProgressClockMaximumBlueColorLevelSeconds }, { "Maximum color level for seconds", ProgressClockSettings.ProgressClockMaximumColorLevelSeconds }, { "Minimum red color level", ProgressClockSettings.ProgressClockMinimumRedColorLevel }, { "Minimum green color level", ProgressClockSettings.ProgressClockMinimumGreenColorLevel }, { "Minimum blue color level", ProgressClockSettings.ProgressClockMinimumBlueColorLevel }, { "Minimum color level", ProgressClockSettings.ProgressClockMinimumColorLevel }, { "Maximum red color level", ProgressClockSettings.ProgressClockMaximumRedColorLevel }, { "Maximum green color level", ProgressClockSettings.ProgressClockMaximumGreenColorLevel }, { "Maximum blue color level", ProgressClockSettings.ProgressClockMaximumBlueColorLevel }, { "Maximum color level", ProgressClockSettings.ProgressClockMaximumColorLevel } };
			ScreensaverConfig.Add("ProgressClock", ProgressClockConfig);

			// Lighter config json object
			var LighterConfig = new JObject() { { "Activate 255 Color Mode", LighterSettings.Lighter255Colors }, { "Activate True Color Mode", LighterSettings.LighterTrueColor }, { "Delay in Milliseconds", LighterSettings.LighterDelay }, { "Max Positions Count", LighterSettings.LighterMaxPositions }, { "Background color", new Color(LighterSettings.LighterBackgroundColor).Type == ColorType.TrueColor ? $"\"{LighterSettings.LighterBackgroundColor}\"" : LighterSettings.LighterBackgroundColor }, { "Minimum red color level", LighterSettings.LighterMinimumRedColorLevel }, { "Minimum green color level", LighterSettings.LighterMinimumGreenColorLevel }, { "Minimum blue color level", LighterSettings.LighterMinimumBlueColorLevel }, { "Minimum color level", LighterSettings.LighterMinimumColorLevel }, { "Maximum red color level", LighterSettings.LighterMaximumRedColorLevel }, { "Maximum green color level", LighterSettings.LighterMaximumGreenColorLevel }, { "Maximum blue color level", LighterSettings.LighterMaximumBlueColorLevel }, { "Maximum color level", LighterSettings.LighterMaximumColorLevel } };
			ScreensaverConfig.Add("Lighter", LighterConfig);

			// Wipe config json object
			var WipeConfig = new JObject() { { "Activate 255 Color Mode", WipeSettings.Wipe255Colors }, { "Activate True Color Mode", WipeSettings.WipeTrueColor }, { "Delay in Milliseconds", WipeSettings.WipeDelay }, { "Wipes to change direction", WipeSettings.WipeWipesNeededToChangeDirection }, { "Background color", new Color(WipeSettings.WipeBackgroundColor).Type == ColorType.TrueColor ? $"\"{WipeSettings.WipeBackgroundColor}\"" : WipeSettings.WipeBackgroundColor }, { "Minimum red color level", WipeSettings.WipeMinimumRedColorLevel }, { "Minimum green color level", WipeSettings.WipeMinimumGreenColorLevel }, { "Minimum blue color level", WipeSettings.WipeMinimumBlueColorLevel }, { "Minimum color level", WipeSettings.WipeMinimumColorLevel }, { "Maximum red color level", WipeSettings.WipeMaximumRedColorLevel }, { "Maximum green color level", WipeSettings.WipeMaximumGreenColorLevel }, { "Maximum blue color level", WipeSettings.WipeMaximumBlueColorLevel }, { "Maximum color level", WipeSettings.WipeMaximumColorLevel } };
			ScreensaverConfig.Add("Wipe", WipeConfig);

			// Matrix config json object
			var MatrixConfig = new JObject() { { "Delay in Milliseconds", MatrixSettings.MatrixDelay } };
			ScreensaverConfig.Add("Matrix", MatrixConfig);

			// GlitterMatrix config json object
			var GlitterMatrixConfig = new JObject() { { "Delay in Milliseconds", GlitterMatrixSettings.GlitterMatrixDelay }, { "Background color", new Color(GlitterMatrixSettings.GlitterMatrixBackgroundColor).Type == ColorType.TrueColor ? $"\"{GlitterMatrixSettings.GlitterMatrixBackgroundColor}\"" : GlitterMatrixSettings.GlitterMatrixBackgroundColor }, { "Foreground color", new Color(GlitterMatrixSettings.GlitterMatrixForegroundColor).Type == ColorType.TrueColor ? $"\"{GlitterMatrixSettings.GlitterMatrixForegroundColor}\"" : GlitterMatrixSettings.GlitterMatrixForegroundColor } };
			ScreensaverConfig.Add("GlitterMatrix", GlitterMatrixConfig);

			// BouncingText config json object
			var BouncingTextConfig = new JObject() { { "Activate 255 Color Mode", BouncingTextSettings.BouncingText255Colors }, { "Activate True Color Mode", BouncingTextSettings.BouncingTextTrueColor }, { "Delay in Milliseconds", BouncingTextSettings.BouncingTextDelay }, { "Text Shown", BouncingTextSettings.BouncingTextWrite }, { "Background color", new Color(BouncingTextSettings.BouncingTextBackgroundColor).Type == ColorType.TrueColor ? $"\"{BouncingTextSettings.BouncingTextBackgroundColor}\"" : BouncingTextSettings.BouncingTextBackgroundColor }, { "Foreground color", new Color(BouncingTextSettings.BouncingTextForegroundColor).Type == ColorType.TrueColor ? $"\"{BouncingTextSettings.BouncingTextForegroundColor}\"" : BouncingTextSettings.BouncingTextForegroundColor }, { "Minimum red color level", BouncingTextSettings.BouncingTextMinimumRedColorLevel }, { "Minimum green color level", BouncingTextSettings.BouncingTextMinimumGreenColorLevel }, { "Minimum blue color level", BouncingTextSettings.BouncingTextMinimumBlueColorLevel }, { "Minimum color level", BouncingTextSettings.BouncingTextMinimumColorLevel }, { "Maximum red color level", BouncingTextSettings.BouncingTextMaximumRedColorLevel }, { "Maximum green color level", BouncingTextSettings.BouncingTextMaximumGreenColorLevel }, { "Maximum blue color level", BouncingTextSettings.BouncingTextMaximumBlueColorLevel }, { "Maximum color level", BouncingTextSettings.BouncingTextMaximumColorLevel } };
			ScreensaverConfig.Add("BouncingText", BouncingTextConfig);

			// Fader config json object
			var FaderConfig = new JObject() { { "Delay in Milliseconds", FaderSettings.FaderDelay }, { "Fade Out Delay in Milliseconds", FaderSettings.FaderFadeOutDelay }, { "Text Shown", FaderSettings.FaderWrite }, { "Max Fade Steps", FaderSettings.FaderMaxSteps }, { "Background color", new Color(FaderSettings.FaderBackgroundColor).Type == ColorType.TrueColor ? $"\"{FaderSettings.FaderBackgroundColor}\"" : FaderSettings.FaderBackgroundColor }, { "Minimum red color level", FaderSettings.FaderMinimumRedColorLevel }, { "Minimum green color level", FaderSettings.FaderMinimumGreenColorLevel }, { "Minimum blue color level", FaderSettings.FaderMinimumBlueColorLevel }, { "Maximum red color level", FaderSettings.FaderMaximumRedColorLevel }, { "Maximum green color level", FaderSettings.FaderMaximumGreenColorLevel }, { "Maximum blue color level", FaderSettings.FaderMaximumBlueColorLevel } };
			ScreensaverConfig.Add("Fader", FaderConfig);

			// FaderBack config json object
			var FaderBackConfig = new JObject() { { "Delay in Milliseconds", FaderBackSettings.FaderBackDelay }, { "Fade Out Delay in Milliseconds", FaderBackSettings.FaderBackFadeOutDelay }, { "Max Fade Steps", FaderBackSettings.FaderBackMaxSteps }, { "Minimum red color level", FaderBackSettings.FaderBackMinimumRedColorLevel }, { "Minimum green color level", FaderBackSettings.FaderBackMinimumGreenColorLevel }, { "Minimum blue color level", FaderBackSettings.FaderBackMinimumBlueColorLevel }, { "Maximum red color level", FaderBackSettings.FaderBackMaximumRedColorLevel }, { "Maximum green color level", FaderBackSettings.FaderBackMaximumGreenColorLevel }, { "Maximum blue color level", FaderBackSettings.FaderBackMaximumBlueColorLevel } };
			ScreensaverConfig.Add("FaderBack", FaderBackConfig);

			// BeatFader config json object
			var BeatFaderConfig = new JObject() { { "Activate 255 Color Mode", BeatFaderSettings.BeatFader255Colors }, { "Activate True Color Mode", BeatFaderSettings.BeatFaderTrueColor }, { "Delay in Beats Per Minute", BeatFaderSettings.BeatFaderDelay }, { "Cycle Colors", BeatFaderSettings.BeatFaderCycleColors }, { "Beat Color", BeatFaderSettings.BeatFaderBeatColor }, { "Max Fade Steps", BeatFaderSettings.BeatFaderMaxSteps }, { "Minimum red color level", BeatFaderSettings.BeatFaderMinimumRedColorLevel }, { "Minimum green color level", BeatFaderSettings.BeatFaderMinimumGreenColorLevel }, { "Minimum blue color level", BeatFaderSettings.BeatFaderMinimumBlueColorLevel }, { "Minimum color level", BeatFaderSettings.BeatFaderMinimumColorLevel }, { "Maximum red color level", BeatFaderSettings.BeatFaderMaximumRedColorLevel }, { "Maximum green color level", BeatFaderSettings.BeatFaderMaximumGreenColorLevel }, { "Maximum blue color level", BeatFaderSettings.BeatFaderMaximumBlueColorLevel }, { "Maximum color level", BeatFaderSettings.BeatFaderMaximumColorLevel } };
			ScreensaverConfig.Add("BeatFader", BeatFaderConfig);

			// Typo config json object
			var TypoConfig = new JObject() { { "Delay in Milliseconds", TypoSettings.TypoDelay }, { "Write Again Delay in Milliseconds", TypoSettings.TypoWriteAgainDelay }, { "Text Shown", TypoSettings.TypoWrite }, { "Minimum writing speed in WPM", TypoSettings.TypoWritingSpeedMin }, { "Maximum writing speed in WPM", TypoSettings.TypoWritingSpeedMax }, { "Probability of typo in percent", TypoSettings.TypoMissStrikePossibility }, { "Probability of miss in percent", TypoSettings.TypoMissPossibility }, { "Text color", new Color(TypoSettings.TypoTextColor).Type == ColorType.TrueColor ? $"\"{TypoSettings.TypoTextColor}\"" : TypoSettings.TypoTextColor } };
			ScreensaverConfig.Add("Typo", TypoConfig);

			// Marquee config json object
			var MarqueeConfig = new JObject() { { "Activate 255 Color Mode", MarqueeSettings.Marquee255Colors }, { "Activate True Color Mode", MarqueeSettings.MarqueeTrueColor }, { "Delay in Milliseconds", MarqueeSettings.MarqueeDelay }, { "Text Shown", MarqueeSettings.MarqueeWrite }, { "Always Centered", MarqueeSettings.MarqueeAlwaysCentered }, { "Use Console API", MarqueeSettings.MarqueeUseConsoleAPI }, { "Background color", new Color(MarqueeSettings.MarqueeBackgroundColor).Type == ColorType.TrueColor ? $"\"{MarqueeSettings.MarqueeBackgroundColor}\"" : MarqueeSettings.MarqueeBackgroundColor }, { "Minimum red color level", MarqueeSettings.MarqueeMinimumRedColorLevel }, { "Minimum green color level", MarqueeSettings.MarqueeMinimumGreenColorLevel }, { "Minimum blue color level", MarqueeSettings.MarqueeMinimumBlueColorLevel }, { "Minimum color level", MarqueeSettings.MarqueeMinimumColorLevel }, { "Maximum red color level", MarqueeSettings.MarqueeMaximumRedColorLevel }, { "Maximum green color level", MarqueeSettings.MarqueeMaximumGreenColorLevel }, { "Maximum blue color level", MarqueeSettings.MarqueeMaximumBlueColorLevel }, { "Maximum color level", MarqueeSettings.MarqueeMaximumColorLevel } };
			ScreensaverConfig.Add("Marquee", MarqueeConfig);

			// Linotypo config json object
			var LinotypoConfig = new JObject() { { "Delay in Milliseconds", LinotypoSettings.LinotypoDelay }, { "New Screen Delay in Milliseconds", LinotypoSettings.LinotypoNewScreenDelay }, { "Text Shown", LinotypoSettings.LinotypoWrite }, { "Minimum writing speed in WPM", LinotypoSettings.LinotypoWritingSpeedMin }, { "Maximum writing speed in WPM", LinotypoSettings.LinotypoWritingSpeedMax }, { "Probability of typo in percent", LinotypoSettings.LinotypoMissStrikePossibility }, { "Column Count", LinotypoSettings.LinotypoTextColumns }, { "Line Fill Threshold", LinotypoSettings.LinotypoEtaoinThreshold }, { "Line Fill Capping Probability in percent", LinotypoSettings.LinotypoEtaoinCappingPossibility }, { "Line Fill Type", (JToken)LinotypoSettings.LinotypoEtaoinType }, { "Probability of miss in percent", LinotypoSettings.LinotypoMissPossibility }, { "Text color", new Color(LinotypoSettings.LinotypoTextColor).Type == ColorType.TrueColor ? $"\"{LinotypoSettings.LinotypoTextColor}\"" : LinotypoSettings.LinotypoTextColor } };
			ScreensaverConfig.Add("Linotypo", LinotypoConfig);

			// Typewriter config json object
			var TypewriterConfig = new JObject() { { "Delay in Milliseconds", TypewriterSettings.TypewriterDelay }, { "New Screen Delay in Milliseconds", TypewriterSettings.TypewriterNewScreenDelay }, { "Text Shown", TypewriterSettings.TypewriterWrite }, { "Minimum writing speed in WPM", TypewriterSettings.TypewriterWritingSpeedMin }, { "Maximum writing speed in WPM", TypewriterSettings.TypewriterWritingSpeedMax }, { "Text color", new Color(TypewriterSettings.TypewriterTextColor).Type == ColorType.TrueColor ? $"\"{TypewriterSettings.TypewriterTextColor}\"" : TypewriterSettings.TypewriterTextColor } };
			ScreensaverConfig.Add("Typewriter", TypewriterConfig);

			// FlashColor config json object
			var FlashColorConfig = new JObject() { { "Activate 255 Color Mode", FlashColorSettings.FlashColor255Colors }, { "Activate True Color Mode", FlashColorSettings.FlashColorTrueColor }, { "Delay in Milliseconds", FlashColorSettings.FlashColorDelay }, { "Background color", new Color(FlashColorSettings.FlashColorBackgroundColor).Type == ColorType.TrueColor ? $"\"{FlashColorSettings.FlashColorBackgroundColor}\"" : FlashColorSettings.FlashColorBackgroundColor }, { "Minimum red color level", FlashColorSettings.FlashColorMinimumRedColorLevel }, { "Minimum green color level", FlashColorSettings.FlashColorMinimumGreenColorLevel }, { "Minimum blue color level", FlashColorSettings.FlashColorMinimumBlueColorLevel }, { "Minimum color level", FlashColorSettings.FlashColorMinimumColorLevel }, { "Maximum red color level", FlashColorSettings.FlashColorMaximumRedColorLevel }, { "Maximum green color level", FlashColorSettings.FlashColorMaximumGreenColorLevel }, { "Maximum blue color level", FlashColorSettings.FlashColorMaximumBlueColorLevel }, { "Maximum color level", FlashColorSettings.FlashColorMaximumColorLevel } };
			ScreensaverConfig.Add("FlashColor", FlashColorConfig);

			// SpotWrite config json object
			var SpotWriteonfig = new JObject() { { "Delay in Milliseconds", SpotWriteSettings.SpotWriteDelay }, { "New Screen Delay in Milliseconds", SpotWriteSettings.SpotWriteNewScreenDelay }, { "Text Shown", SpotWriteSettings.SpotWriteWrite }, { "Text color", SpotWriteSettings.SpotWriteTextColor } };
			ScreensaverConfig.Add("SpotWrite", SpotWriteonfig);

			// Ramp config json object
			var RampConfig = new JObject() { { "Activate 255 Color Mode", RampSettings.Ramp255Colors }, { "Activate True Color Mode", RampSettings.RampTrueColor }, { "Delay in Milliseconds", RampSettings.RampDelay }, { "Next ramp interval", RampSettings.RampDelay }, { "Upper left corner character for ramp bar", RampSettings.RampUpperLeftCornerChar }, { "Upper right corner character for ramp bar", RampSettings.RampUpperRightCornerChar }, { "Lower left corner character for ramp bar", RampSettings.RampLowerLeftCornerChar }, { "Lower right corner character for ramp bar", RampSettings.RampLowerRightCornerChar }, { "Upper frame character for ramp bar", RampSettings.RampUpperFrameChar }, { "Lower frame character for ramp bar", RampSettings.RampLowerFrameChar }, { "Left frame character for ramp bar", RampSettings.RampLeftFrameChar }, { "Right frame character for ramp bar", RampSettings.RampRightFrameChar }, { "Minimum red color level for start color", RampSettings.RampMinimumRedColorLevelStart }, { "Minimum green color level for start color", RampSettings.RampMinimumGreenColorLevelStart }, { "Minimum blue color level for start color", RampSettings.RampMinimumBlueColorLevelStart }, { "Minimum color level for start color", RampSettings.RampMinimumColorLevelStart }, { "Maximum red color level for start color", RampSettings.RampMaximumRedColorLevelStart }, { "Maximum green color level for start color", RampSettings.RampMaximumGreenColorLevelStart }, { "Maximum blue color level for start color", RampSettings.RampMaximumBlueColorLevelStart }, { "Maximum color level for start color", RampSettings.RampMaximumColorLevelStart }, { "Minimum red color level for end color", RampSettings.RampMinimumRedColorLevelEnd }, { "Minimum green color level for end color", RampSettings.RampMinimumGreenColorLevelEnd }, { "Minimum blue color level for end color", RampSettings.RampMinimumBlueColorLevelEnd }, { "Minimum color level for end color", RampSettings.RampMinimumColorLevelEnd }, { "Maximum red color level for end color", RampSettings.RampMaximumRedColorLevelEnd }, { "Maximum green color level for end color", RampSettings.RampMaximumGreenColorLevelEnd }, { "Maximum blue color level for end color", RampSettings.RampMaximumBlueColorLevelEnd }, { "Maximum color level for end color", RampSettings.RampMaximumColorLevelEnd }, { "Upper left corner color for ramp bar", RampSettings.RampUpperLeftCornerColor }, { "Upper right corner color for ramp bar", RampSettings.RampUpperRightCornerColor }, { "Lower left corner color for ramp bar", RampSettings.RampLowerLeftCornerColor }, { "Lower right corner color for ramp bar", RampSettings.RampLowerRightCornerColor }, { "Upper frame color for ramp bar", RampSettings.RampUpperFrameColor }, { "Lower frame color for ramp bar", RampSettings.RampLowerFrameColor }, { "Left frame color for ramp bar", RampSettings.RampLeftFrameColor }, { "Right frame color for ramp bar", RampSettings.RampRightFrameColor }, { "Use border colors for ramp bar", RampSettings.RampUseBorderColors } };
			ScreensaverConfig.Add("Ramp", RampConfig);

			// StackBox config json object
			var StackBoxConfig = new JObject() { { "Activate 255 Color Mode", StackBoxSettings.StackBox255Colors }, { "Activate True Color Mode", StackBoxSettings.StackBoxTrueColor }, { "Delay in Milliseconds", StackBoxSettings.StackBoxDelay }, { "Minimum red color level", StackBoxSettings.StackBoxMinimumRedColorLevel }, { "Minimum green color level", StackBoxSettings.StackBoxMinimumGreenColorLevel }, { "Minimum blue color level", StackBoxSettings.StackBoxMinimumBlueColorLevel }, { "Minimum color level", StackBoxSettings.StackBoxMinimumColorLevel }, { "Maximum red color level", StackBoxSettings.StackBoxMaximumRedColorLevel }, { "Maximum green color level", StackBoxSettings.StackBoxMaximumGreenColorLevel }, { "Maximum blue color level", StackBoxSettings.StackBoxMaximumBlueColorLevel }, { "Maximum color level", StackBoxSettings.StackBoxMaximumColorLevel }, { "Fill the boxes", StackBoxSettings.StackBoxFill } };
			ScreensaverConfig.Add("StackBox", StackBoxConfig);

			// Snaker config json object
			var SnakerConfig = new JObject() { { "Activate 255 Color Mode", SnakerSettings.Snaker255Colors }, { "Activate True Color Mode", SnakerSettings.SnakerTrueColor }, { "Delay in Milliseconds", SnakerSettings.SnakerDelay }, { "Stage delay in milliseconds", SnakerSettings.SnakerStageDelay }, { "Minimum red color level", SnakerSettings.SnakerMinimumRedColorLevel }, { "Minimum green color level", SnakerSettings.SnakerMinimumGreenColorLevel }, { "Minimum blue color level", SnakerSettings.SnakerMinimumBlueColorLevel }, { "Minimum color level", SnakerSettings.SnakerMinimumColorLevel }, { "Maximum red color level", SnakerSettings.SnakerMaximumRedColorLevel }, { "Maximum green color level", SnakerSettings.SnakerMaximumGreenColorLevel }, { "Maximum blue color level", SnakerSettings.SnakerMaximumBlueColorLevel }, { "Maximum color level", SnakerSettings.SnakerMaximumColorLevel } };
			ScreensaverConfig.Add("Snaker", SnakerConfig);

			// BarRot config json object
			var BarRotConfig = new JObject() { { "Activate 255 Color Mode", BarRotSettings.BarRot255Colors }, { "Activate True Color Mode", BarRotSettings.BarRotTrueColor }, { "Delay in Milliseconds", BarRotSettings.BarRotDelay }, { "Next ramp rot interval", BarRotSettings.BarRotNextRampDelay }, { "Upper left corner character for ramp bar", BarRotSettings.BarRotUpperLeftCornerChar }, { "Upper right corner character for ramp bar", BarRotSettings.BarRotUpperRightCornerChar }, { "Lower left corner character for ramp bar", BarRotSettings.BarRotLowerLeftCornerChar }, { "Lower right corner character for ramp bar", BarRotSettings.BarRotLowerRightCornerChar }, { "Upper frame character for ramp bar", BarRotSettings.BarRotUpperFrameChar }, { "Lower frame character for ramp bar", BarRotSettings.BarRotLowerFrameChar }, { "Left frame character for ramp bar", BarRotSettings.BarRotLeftFrameChar }, { "Right frame character for ramp bar", BarRotSettings.BarRotRightFrameChar }, { "Minimum red color level for start color", BarRotSettings.BarRotMinimumRedColorLevelStart }, { "Minimum green color level for start color", BarRotSettings.BarRotMinimumGreenColorLevelStart }, { "Minimum blue color level for start color", BarRotSettings.BarRotMinimumBlueColorLevelStart }, { "Maximum red color level for start color", BarRotSettings.BarRotMaximumRedColorLevelStart }, { "Maximum green color level for start color", BarRotSettings.BarRotMaximumGreenColorLevelStart }, { "Maximum blue color level for start color", BarRotSettings.BarRotMaximumBlueColorLevelStart }, { "Minimum red color level for end color", BarRotSettings.BarRotMinimumRedColorLevelEnd }, { "Minimum green color level for end color", BarRotSettings.BarRotMinimumGreenColorLevelEnd }, { "Minimum blue color level for end color", BarRotSettings.BarRotMinimumBlueColorLevelEnd }, { "Maximum red color level for end color", BarRotSettings.BarRotMaximumRedColorLevelEnd }, { "Maximum green color level for end color", BarRotSettings.BarRotMaximumGreenColorLevelEnd }, { "Maximum blue color level for end color", BarRotSettings.BarRotMaximumBlueColorLevelEnd }, { "Upper left corner color for ramp bar", BarRotSettings.BarRotUpperLeftCornerColor }, { "Upper right corner color for ramp bar", BarRotSettings.BarRotUpperRightCornerColor }, { "Lower left corner color for ramp bar", BarRotSettings.BarRotLowerLeftCornerColor }, { "Lower right corner color for ramp bar", BarRotSettings.BarRotLowerRightCornerColor }, { "Upper frame color for ramp bar", BarRotSettings.BarRotUpperFrameColor }, { "Lower frame color for ramp bar", BarRotSettings.BarRotLowerFrameColor }, { "Left frame color for ramp bar", BarRotSettings.BarRotLeftFrameColor }, { "Right frame color for ramp bar", BarRotSettings.BarRotRightFrameColor }, { "Use border colors for ramp bar", BarRotSettings.BarRotUseBorderColors } };
			ScreensaverConfig.Add("BarRot", BarRotConfig);

			// Fireworks config json object
			var FireworksConfig = new JObject() { { "Activate 255 Color Mode", FireworksSettings.Fireworks255Colors }, { "Activate True Color Mode", FireworksSettings.FireworksTrueColor }, { "Delay in Milliseconds", FireworksSettings.FireworksDelay }, { "Firework explosion radius", FireworksSettings.FireworksRadius }, { "Minimum red color level", FireworksSettings.FireworksMinimumRedColorLevel }, { "Minimum green color level", FireworksSettings.FireworksMinimumGreenColorLevel }, { "Minimum blue color level", FireworksSettings.FireworksMinimumBlueColorLevel }, { "Minimum color level", FireworksSettings.FireworksMinimumColorLevel }, { "Maximum red color level", FireworksSettings.FireworksMaximumRedColorLevel }, { "Maximum green color level", FireworksSettings.FireworksMaximumGreenColorLevel }, { "Maximum blue color level", FireworksSettings.FireworksMaximumBlueColorLevel }, { "Maximum color level", FireworksSettings.FireworksMaximumColorLevel } };
			ScreensaverConfig.Add("Fireworks", FireworksConfig);

			// Figlet config json object
			var FigletConfig = new JObject() { { "Activate 255 Color Mode", FigletSettings.Figlet255Colors }, { "Activate True Color Mode", FigletSettings.FigletTrueColor }, { "Delay in Milliseconds", FigletSettings.FigletDelay }, { "Text Shown", FigletSettings.FigletText }, { "Figlet font", FigletSettings.FigletFont }, { "Minimum red color level", FigletSettings.FigletMinimumRedColorLevel }, { "Minimum green color level", FigletSettings.FigletMinimumGreenColorLevel }, { "Minimum blue color level", FigletSettings.FigletMinimumBlueColorLevel }, { "Minimum color level", FigletSettings.FigletMinimumColorLevel }, { "Maximum red color level", FigletSettings.FigletMaximumRedColorLevel }, { "Maximum green color level", FigletSettings.FigletMaximumGreenColorLevel }, { "Maximum blue color level", FigletSettings.FigletMaximumBlueColorLevel }, { "Maximum color level", FigletSettings.FigletMaximumColorLevel } };
			ScreensaverConfig.Add("Figlet", FigletConfig);

			// FlashText config json object
			var FlashTextConfig = new JObject() { { "Activate 255 Color Mode", FlashTextSettings.FlashText255Colors }, { "Activate True Color Mode", FlashTextSettings.FlashTextTrueColor }, { "Delay in Milliseconds", FlashTextSettings.FlashTextDelay }, { "Text Shown", FlashTextSettings.FlashTextWrite }, { "Background color", new Color(FlashTextSettings.FlashTextBackgroundColor).Type == ColorType.TrueColor ? $"\"{FlashTextSettings.FlashTextBackgroundColor}\"" : FlashTextSettings.FlashTextBackgroundColor }, { "Minimum red color level", FlashTextSettings.FlashTextMinimumRedColorLevel }, { "Minimum green color level", FlashTextSettings.FlashTextMinimumGreenColorLevel }, { "Minimum blue color level", FlashTextSettings.FlashTextMinimumBlueColorLevel }, { "Minimum color level", FlashTextSettings.FlashTextMinimumColorLevel }, { "Maximum red color level", FlashTextSettings.FlashTextMaximumRedColorLevel }, { "Maximum green color level", FlashTextSettings.FlashTextMaximumGreenColorLevel }, { "Maximum blue color level", FlashTextSettings.FlashTextMaximumBlueColorLevel }, { "Maximum color level", FlashTextSettings.FlashTextMaximumColorLevel } };
			ScreensaverConfig.Add("FlashText", FlashTextConfig);

			// Noise config json object
			var NoiseConfig = new JObject() { { "New Screen Delay in Milliseconds", NoiseSettings.NoiseNewScreenDelay }, { "Noise density", NoiseSettings.NoiseDensity } };
			ScreensaverConfig.Add("Noise", NoiseConfig);

			// PersonLookup config json object
			var PersonLookupConfig = new JObject() { { "Delay in Milliseconds", PersonLookupSettings.PersonLookupDelay }, { "New Screen Delay in Milliseconds", PersonLookupSettings.PersonLookupLookedUpDelay }, { "Minimum names count", PersonLookupSettings.PersonLookupMinimumNames }, { "Maximum names count", PersonLookupSettings.PersonLookupMaximumNames }, { "Minimum age years count", PersonLookupSettings.PersonLookupMinimumAgeYears }, { "Maximum age years count", PersonLookupSettings.PersonLookupMaximumAgeYears } };
			ScreensaverConfig.Add("PersonLookup", PersonLookupConfig);

			// DateAndTime config json object
			var DateAndTimeConfig = new JObject() { { "Activate 255 Color Mode", DateAndTimeSettings.DateAndTime255Colors }, { "Activate True Color Mode", DateAndTimeSettings.DateAndTimeTrueColor }, { "Delay in Milliseconds", DateAndTimeSettings.DateAndTimeDelay }, { "Minimum red color level", DateAndTimeSettings.DateAndTimeMinimumRedColorLevel }, { "Minimum green color level", DateAndTimeSettings.DateAndTimeMinimumGreenColorLevel }, { "Minimum blue color level", DateAndTimeSettings.DateAndTimeMinimumBlueColorLevel }, { "Minimum color level", DateAndTimeSettings.DateAndTimeMinimumColorLevel }, { "Maximum red color level", DateAndTimeSettings.DateAndTimeMaximumRedColorLevel }, { "Maximum green color level", DateAndTimeSettings.DateAndTimeMaximumGreenColorLevel }, { "Maximum blue color level", DateAndTimeSettings.DateAndTimeMaximumBlueColorLevel }, { "Maximum color level", DateAndTimeSettings.DateAndTimeMaximumColorLevel } };
			ScreensaverConfig.Add("DateAndTime", DateAndTimeConfig);

			// Glitch config json object
			var GlitchConfig = new JObject() { { "Delay in Milliseconds", GlitchSettings.GlitchDelay }, { "Glitch density", GlitchSettings.GlitchDensity } };
			ScreensaverConfig.Add("Glitch", GlitchConfig);

			// Indeterminate config json object
			var IndeterminateConfig = new JObject() { { "Activate 255 Color Mode", IndeterminateSettings.Indeterminate255Colors }, { "Activate True Color Mode", IndeterminateSettings.IndeterminateTrueColor }, { "Delay in Milliseconds", IndeterminateSettings.IndeterminateDelay }, { "Upper left corner character for ramp bar", IndeterminateSettings.IndeterminateUpperLeftCornerChar }, { "Upper right corner character for ramp bar", IndeterminateSettings.IndeterminateUpperRightCornerChar }, { "Lower left corner character for ramp bar", IndeterminateSettings.IndeterminateLowerLeftCornerChar }, { "Lower right corner character for ramp bar", IndeterminateSettings.IndeterminateLowerRightCornerChar }, { "Upper frame character for ramp bar", IndeterminateSettings.IndeterminateUpperFrameChar }, { "Lower frame character for ramp bar", IndeterminateSettings.IndeterminateLowerFrameChar }, { "Left frame character for ramp bar", IndeterminateSettings.IndeterminateLeftFrameChar }, { "Right frame character for ramp bar", IndeterminateSettings.IndeterminateRightFrameChar }, { "Minimum red color level", IndeterminateSettings.IndeterminateMinimumRedColorLevel }, { "Minimum green color level", IndeterminateSettings.IndeterminateMinimumGreenColorLevel }, { "Minimum blue color level", IndeterminateSettings.IndeterminateMinimumBlueColorLevel }, { "Minimum color level", IndeterminateSettings.IndeterminateMinimumColorLevel }, { "Maximum red color level", IndeterminateSettings.IndeterminateMaximumRedColorLevel }, { "Maximum green color level", IndeterminateSettings.IndeterminateMaximumGreenColorLevel }, { "Maximum blue color level", IndeterminateSettings.IndeterminateMaximumBlueColorLevel }, { "Maximum color level", IndeterminateSettings.IndeterminateMaximumColorLevel }, { "Upper left corner color for ramp bar", IndeterminateSettings.IndeterminateUpperLeftCornerColor }, { "Upper right corner color for ramp bar", IndeterminateSettings.IndeterminateUpperRightCornerColor }, { "Lower left corner color for ramp bar", IndeterminateSettings.IndeterminateLowerLeftCornerColor }, { "Lower right corner color for ramp bar", IndeterminateSettings.IndeterminateLowerRightCornerColor }, { "Upper frame color for ramp bar", IndeterminateSettings.IndeterminateUpperFrameColor }, { "Lower frame color for ramp bar", IndeterminateSettings.IndeterminateLowerFrameColor }, { "Left frame color for ramp bar", IndeterminateSettings.IndeterminateLeftFrameColor }, { "Right frame color for ramp bar", IndeterminateSettings.IndeterminateRightFrameColor }, { "Use border colors for ramp bar", IndeterminateSettings.IndeterminateUseBorderColors } };
			ScreensaverConfig.Add("Indeterminate", IndeterminateConfig);

			// Pulse config json object
			var PulseConfig = new JObject() { { "Delay in Milliseconds", PulseSettings.PulseDelay }, { "Max Fade Steps", PulseSettings.PulseMaxSteps }, { "Minimum red color level", PulseSettings.PulseMinimumRedColorLevel }, { "Minimum green color level", PulseSettings.PulseMinimumGreenColorLevel }, { "Minimum blue color level", PulseSettings.PulseMinimumBlueColorLevel }, { "Maximum red color level", PulseSettings.PulseMaximumRedColorLevel }, { "Maximum green color level", PulseSettings.PulseMaximumGreenColorLevel }, { "Maximum blue color level", PulseSettings.PulseMaximumBlueColorLevel } };
			ScreensaverConfig.Add("Pulse", PulseConfig);

			// BeatPulse config json object
			var BeatPulseConfig = new JObject() { { "Activate 255 Color Mode", BeatPulseSettings.BeatPulse255Colors }, { "Activate True Color Mode", BeatPulseSettings.BeatPulseTrueColor }, { "Delay in Beats Per Minute", BeatPulseSettings.BeatPulseDelay }, { "Cycle Colors", BeatPulseSettings.BeatPulseCycleColors }, { "Beat Color", BeatPulseSettings.BeatPulseBeatColor }, { "Max Fade Steps", BeatPulseSettings.BeatPulseMaxSteps }, { "Minimum red color level", BeatPulseSettings.BeatPulseMinimumRedColorLevel }, { "Minimum green color level", BeatPulseSettings.BeatPulseMinimumGreenColorLevel }, { "Minimum blue color level", BeatPulseSettings.BeatPulseMinimumBlueColorLevel }, { "Minimum color level", BeatPulseSettings.BeatPulseMinimumColorLevel }, { "Maximum red color level", BeatPulseSettings.BeatPulseMaximumRedColorLevel }, { "Maximum green color level", BeatPulseSettings.BeatPulseMaximumGreenColorLevel }, { "Maximum blue color level", BeatPulseSettings.BeatPulseMaximumBlueColorLevel }, { "Maximum color level", BeatPulseSettings.BeatPulseMaximumColorLevel } };
			ScreensaverConfig.Add("BeatPulse", BeatPulseConfig);

			// EdgePulse config json object
			var EdgePulseConfig = new JObject() { { "Delay in Milliseconds", EdgePulseSettings.EdgePulseDelay }, { "Max Fade Steps", EdgePulseSettings.EdgePulseMaxSteps }, { "Minimum red color level", EdgePulseSettings.EdgePulseMinimumRedColorLevel }, { "Minimum green color level", EdgePulseSettings.EdgePulseMinimumGreenColorLevel }, { "Minimum blue color level", EdgePulseSettings.EdgePulseMinimumBlueColorLevel }, { "Maximum red color level", EdgePulseSettings.EdgePulseMaximumRedColorLevel }, { "Maximum green color level", EdgePulseSettings.EdgePulseMaximumGreenColorLevel }, { "Maximum blue color level", EdgePulseSettings.EdgePulseMaximumBlueColorLevel } };
			ScreensaverConfig.Add("EdgePulse", EdgePulseConfig);

			// BeatEdgePulse config json object
			var BeatEdgePulseConfig = new JObject() { { "Activate 255 Color Mode", BeatEdgePulseSettings.BeatEdgePulse255Colors }, { "Activate True Color Mode", BeatEdgePulseSettings.BeatEdgePulseTrueColor }, { "Delay in Beats Per Minute", BeatEdgePulseSettings.BeatEdgePulseDelay }, { "Cycle Colors", BeatEdgePulseSettings.BeatEdgePulseCycleColors }, { "Beat Color", BeatEdgePulseSettings.BeatEdgePulseBeatColor }, { "Max Fade Steps", BeatEdgePulseSettings.BeatEdgePulseMaxSteps }, { "Minimum red color level", BeatEdgePulseSettings.BeatEdgePulseMinimumRedColorLevel }, { "Minimum green color level", BeatEdgePulseSettings.BeatEdgePulseMinimumGreenColorLevel }, { "Minimum blue color level", BeatEdgePulseSettings.BeatEdgePulseMinimumBlueColorLevel }, { "Minimum color level", BeatEdgePulseSettings.BeatEdgePulseMinimumColorLevel }, { "Maximum red color level", BeatEdgePulseSettings.BeatEdgePulseMaximumRedColorLevel }, { "Maximum green color level", BeatEdgePulseSettings.BeatEdgePulseMaximumGreenColorLevel }, { "Maximum blue color level", BeatEdgePulseSettings.BeatEdgePulseMaximumBlueColorLevel }, { "Maximum color level", BeatEdgePulseSettings.BeatEdgePulseMaximumColorLevel } };
			ScreensaverConfig.Add("BeatEdgePulse", BeatEdgePulseConfig);

			// Add a screensaver config json object to Screensaver section
			ConfigurationObject.Add("Screensaver", ScreensaverConfig);

			// The Splash Section
			var SplashConfig = new JObject();

			// Simple config json object
			var SplashSimpleConfig = new JObject() { { "Progress text location", (JToken)SplashSettings.SimpleProgressTextLocation } };
			SplashConfig.Add("Simple", SplashSimpleConfig);

			// Progress config json object
			var SplashProgressConfig = new JObject() { { "Progress bar color", SplashSettings.ProgressProgressColor }, { "Progress text location", (JToken)SplashSettings.ProgressProgressTextLocation } };
			SplashConfig.Add("Progress", SplashProgressConfig);

			// Add a splash config json object to Splash section
			ConfigurationObject.Add("Splash", SplashConfig);

			// Misc Section
			var MiscConfig = new JObject() { { "Show Time/Date on Upper Right Corner", Flags.CornerTimeDate }, { "Marquee on startup", Flags.StartScroll }, { "Long Time and Date", Flags.LongTimeDate }, { "Preferred Unit for Temperature", (JToken)Forecast.Forecast.PreferredUnit }, { "Enable text editor autosave", TextEditShellCommon.TextEdit_AutoSaveFlag }, { "Text editor autosave interval", TextEditShellCommon.TextEdit_AutoSaveInterval }, { "Wrap list outputs", Flags.WrapListOutputs }, { "Draw notification border", Flags.DrawBorderNotification }, { "Blacklisted mods", ModManager.BlacklistedModsString }, { "Solver minimum number", Solver.SolverMinimumNumber }, { "Solver maximum number", Solver.SolverMaximumNumber }, { "Solver show input", Solver.SolverShowInput }, { "Upper left corner character for notification border", Notifications.Notifications.NotifyUpperLeftCornerChar }, { "Upper right corner character for notification border", Notifications.Notifications.NotifyUpperRightCornerChar }, { "Lower left corner character for notification border", Notifications.Notifications.NotifyLowerLeftCornerChar }, { "Lower right corner character for notification border", Notifications.Notifications.NotifyLowerRightCornerChar }, { "Upper frame character for notification border", Notifications.Notifications.NotifyUpperFrameChar }, { "Lower frame character for notification border", Notifications.Notifications.NotifyLowerFrameChar }, { "Left frame character for notification border", Notifications.Notifications.NotifyLeftFrameChar }, { "Right frame character for notification border", Notifications.Notifications.NotifyRightFrameChar }, { "Manual page information style", PageViewer.ManpageInfoStyle }, { "Default difficulty for SpeedPress", (JToken)SpeedPress.SpeedPressCurrentDifficulty }, { "Keypress timeout for SpeedPress", SpeedPress.SpeedPressTimeout }, { "Show latest RSS headline on login", RSSTools.ShowHeadlineOnLogin }, { "RSS headline URL", RSSTools.RssHeadlineUrl }, { "Save all events and/or reminders destructively", Flags.SaveEventsRemindersDestructively }, { "Default JSON formatting for JSON shell", (JToken)JsonShellCommon.JsonShell_Formatting }, { "Enable Figlet for timer", Flags.EnableFigletTimer }, { "Figlet font for timer", TimerScreen.TimerFigletFont }, { "Show the commands count on help", Flags.ShowCommandsCount }, { "Show the shell commands count on help", Flags.ShowShellCommandsCount }, { "Show the mod commands count on help", Flags.ShowModCommandsCount }, { "Show the aliases count on help", Flags.ShowShellAliasesCount }, { "Password mask character", Input.CurrentMask }, { "Upper left corner character for progress bars", ProgressTools.ProgressUpperLeftCornerChar }, { "Upper right corner character for progress bars", ProgressTools.ProgressUpperRightCornerChar }, { "Lower left corner character for progress bars", ProgressTools.ProgressLowerLeftCornerChar }, { "Lower right corner character for progress bars", ProgressTools.ProgressLowerRightCornerChar }, { "Upper frame character for progress bars", ProgressTools.ProgressUpperFrameChar }, { "Lower frame character for progress bars", ProgressTools.ProgressLowerFrameChar }, { "Left frame character for progress bars", ProgressTools.ProgressLeftFrameChar }, { "Right frame character for progress bars", ProgressTools.ProgressRightFrameChar }, { "Users count for love or hate comments", LoveHateRespond.LoveOrHateUsersCount }, { "Input history enabled", Flags.InputHistoryEnabled }, { "Use PowerLine for rendering spaceship", MeteorShooter.MeteorUsePowerLine }, { "Meteor game speed", MeteorShooter.MeteorSpeed } };
			ConfigurationObject.Add("Misc", MiscConfig);
			return ConfigurationObject;
		}

		/// <summary>
        /// Creates the kernel configuration file
        /// </summary>
        /// <exception cref="Exceptions.ConfigException"></exception>
		public static void CreateConfig()
		{
			CreateConfig(Paths.GetKernelPath(KernelPathType.Configuration));
		}

		/// <summary>
        /// Creates the kernel configuration file with custom path
        /// </summary>
        /// <exception cref="Exceptions.ConfigException"></exception>
		public static void CreateConfig(string ConfigPath)
		{
			Filesystem.ThrowOnInvalidPath(ConfigPath);
			var ConfigurationObject = GetNewConfigObject();

			// Save Config
			File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(ConfigurationObject, Formatting.Indented));
			Kernel.Kernel.KernelEventManager.RaiseConfigSaved();
		}

		/// <summary>
        /// Creates the kernel configuration file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful.</returns>
        /// <exception cref="Exceptions.ConfigException"></exception>
		public static bool TryCreateConfig()
		{
			return TryCreateConfig(Paths.GetKernelPath(KernelPathType.Configuration));
		}

		/// <summary>
        /// Creates the kernel configuration file with custom path
        /// </summary>
        /// <returns>True if successful; False if unsuccessful.</returns>
        /// <exception cref="Exceptions.ConfigException"></exception>
		public static bool TryCreateConfig(string ConfigPath)
		{
			try
			{
				CreateConfig(ConfigPath);
				return true;
			}
			catch (Exception ex)
			{
				Kernel.Kernel.KernelEventManager.RaiseConfigSaveError(ex);
				DebugWriter.WStkTrc(ex);
				return false;
			}
		}

		/// <summary>
        /// Configures the kernel according to the kernel configuration file
        /// </summary>
        /// <exception cref="Exceptions.ConfigException"></exception>
		public static void ReadConfig()
		{
			ReadConfig(Paths.GetKernelPath(KernelPathType.Configuration));
		}

		/// <summary>
        /// Configures the kernel according to the kernel configuration file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="Exceptions.ConfigException"></exception>
		public static bool TryReadConfig()
		{
			return TryReadConfig(Paths.GetKernelPath(KernelPathType.Configuration));
		}

		/// <summary>
        /// Configures the kernel according to the custom kernel configuration file
        /// </summary>
        /// <exception cref="Exceptions.ConfigException"></exception>
		public static void ReadConfig(string ConfigPath)
		{
			// Parse configuration. NOTE: Question marks between parentheses are for nullable types.
			Filesystem.ThrowOnInvalidPath(ConfigPath);
			InitializeConfigToken(ConfigPath);
			DebugWriter.Wdbg(DebugLevel.I, "Config loaded with {0} sections", ConfigToken.Count);

			// ----------------------------- Important configuration -----------------------------
			// Language
			Flags.LangChangeCulture = (bool)((ConfigToken["General"]?["Change Culture when Switching Languages"]) ?? false);
			if (Flags.LangChangeCulture)
				CultureManager.CurrentCult = new CultureInfo(ConfigToken["General"]?["Culture"] is not null ? ConfigToken["General"]["Culture"].ToString() : "en-US");
			LanguageManager.SetLang((string)((ConfigToken["General"]?["Language"]) ?? "eng"));

			// Colored Shell
			bool UncoloredDetected = ConfigToken["Shell"]?["Colored Shell"] is not null && !ConfigToken["Shell"]["Colored Shell"].ToObject<bool>();
			if (UncoloredDetected)
			{
				DebugWriter.Wdbg(DebugLevel.W, "Detected uncolored shell. Removing colors...");
				ThemeTools.ApplyThemeFromResources("LinuxUncolored");
				Shell.Shell.ColoredShell = false;
			}

			// ----------------------------- General configuration -----------------------------
			// Colors Section
			DebugWriter.Wdbg(DebugLevel.I, "Loading colors...");
			if (Shell.Shell.ColoredShell)
			{
				// We use New Color() to parse entered color. This is to ensure that the kernel can use the correct VT sequence.
				KernelColorTools.UserNameShellColor = new Color(((ConfigToken["Colors"]?["User Name Shell Color"]) ?? ConsoleColors.Green).ToString());
				KernelColorTools.HostNameShellColor = new Color(((ConfigToken["Colors"]?["Host Name Shell Color"]) ?? ConsoleColors.DarkGreen).ToString());
				KernelColorTools.ContKernelErrorColor = new Color(((ConfigToken["Colors"]?["Continuable Kernel Error Color"]) ?? ConsoleColors.Yellow).ToString());
				KernelColorTools.UncontKernelErrorColor = new Color(((ConfigToken["Colors"]?["Uncontinuable Kernel Error Color"]) ?? ConsoleColors.Red).ToString());
				KernelColorTools.NeutralTextColor = new Color(((ConfigToken["Colors"]?["Text Color"]) ?? ConsoleColors.Gray).ToString());
				KernelColorTools.LicenseColor = new Color(((ConfigToken["Colors"]?["License Color"]) ?? ConsoleColors.White).ToString());
				KernelColorTools.BackgroundColor = new Color(((ConfigToken["Colors"]?["Background Color"]) ?? ConsoleColors.Black).ToString());
				KernelColorTools.InputColor = new Color(((ConfigToken["Colors"]?["Input Color"]) ?? ConsoleColors.White).ToString());
				KernelColorTools.ListEntryColor = new Color(((ConfigToken["Colors"]?["List Entry Color"]) ?? ConsoleColors.DarkYellow).ToString());
				KernelColorTools.ListValueColor = new Color(((ConfigToken["Colors"]?["List Value Color"]) ?? ConsoleColors.DarkGray).ToString());
				KernelColorTools.StageColor = new Color(((ConfigToken["Colors"]?["Kernel Stage Color"]) ?? ConsoleColors.Green).ToString());
				KernelColorTools.ErrorColor = new Color(((ConfigToken["Colors"]?["Error Text Color"]) ?? ConsoleColors.Red).ToString());
				KernelColorTools.WarningColor = new Color(((ConfigToken["Colors"]?["Warning Text Color"]) ?? ConsoleColors.Yellow).ToString());
				KernelColorTools.OptionColor = new Color(((ConfigToken["Colors"]?["Option Color"]) ?? ConsoleColors.DarkYellow).ToString());
				KernelColorTools.BannerColor = new Color(((ConfigToken["Colors"]?["Banner Color"]) ?? ConsoleColors.Green).ToString());
				KernelColorTools.NotificationTitleColor = new Color(((ConfigToken["Colors"]?["Notification Title Color"]) ?? ConsoleColors.White).ToString());
				KernelColorTools.NotificationDescriptionColor = new Color(((ConfigToken["Colors"]?["Notification Description Color"]) ?? ConsoleColors.Gray).ToString());
				KernelColorTools.NotificationProgressColor = new Color(((ConfigToken["Colors"]?["Notification Progress Color"]) ?? ConsoleColors.DarkYellow).ToString());
				KernelColorTools.NotificationFailureColor = new Color(((ConfigToken["Colors"]?["Notification Failure Color"]) ?? ConsoleColors.Red).ToString());
				KernelColorTools.QuestionColor = new Color(((ConfigToken["Colors"]?["Question Color"]) ?? ConsoleColors.Yellow).ToString());
				KernelColorTools.SuccessColor = new Color(((ConfigToken["Colors"]?["Success Color"]) ?? ConsoleColors.Green).ToString());
				KernelColorTools.UserDollarColor = new Color(((ConfigToken["Colors"]?["User Dollar Color"]) ?? ConsoleColors.Gray).ToString());
				KernelColorTools.TipColor = new Color(((ConfigToken["Colors"]?["Tip Color"]) ?? ConsoleColors.Gray).ToString());
				KernelColorTools.SeparatorTextColor = new Color(((ConfigToken["Colors"]?["Separator Text Color"]) ?? ConsoleColors.White).ToString());
				KernelColorTools.SeparatorColor = new Color(((ConfigToken["Colors"]?["Separator Color"]) ?? ConsoleColors.Gray).ToString());
				KernelColorTools.ListTitleColor = new Color(((ConfigToken["Colors"]?["List Title Color"]) ?? ConsoleColors.White).ToString());
				KernelColorTools.DevelopmentWarningColor = new Color(((ConfigToken["Colors"]?["Development Warning Color"]) ?? ConsoleColors.Yellow).ToString());
				KernelColorTools.StageTimeColor = new Color(((ConfigToken["Colors"]?["Stage Time Color"]) ?? ConsoleColors.Gray).ToString());
				KernelColorTools.ProgressColor = new Color(((ConfigToken["Colors"]?["Progress Color"]) ?? ConsoleColors.DarkYellow).ToString());
				KernelColorTools.BackOptionColor = new Color(((ConfigToken["Colors"]?["Back Option Color"]) ?? ConsoleColors.DarkRed).ToString());
				KernelColorTools.LowPriorityBorderColor = new Color(((ConfigToken["Colors"]?["Low Priority Border Color"]) ?? ConsoleColors.White).ToString());
				KernelColorTools.MediumPriorityBorderColor = new Color(((ConfigToken["Colors"]?["Medium Priority Border Color"]) ?? ConsoleColors.Yellow).ToString());
				KernelColorTools.HighPriorityBorderColor = new Color(((ConfigToken["Colors"]?["High Priority Border Color"]) ?? ConsoleColors.Red).ToString());
				KernelColorTools.TableSeparatorColor = new Color(((ConfigToken["Colors"]?["Table Separator Color"]) ?? ConsoleColors.DarkGray).ToString());
				KernelColorTools.TableHeaderColor = new Color(((ConfigToken["Colors"]?["Table Header Color"]) ?? ConsoleColors.White).ToString());
				KernelColorTools.TableValueColor = new Color(((ConfigToken["Colors"]?["Table Value Color"]) ?? ConsoleColors.Gray).ToString());
				KernelColorTools.SelectedOptionColor = new Color(((ConfigToken["Colors"]?["Selected Option Color"]) ?? ConsoleColors.Yellow).ToString());
				KernelColorTools.AlternativeOptionColor = new Color(((ConfigToken["Colors"]?["Alternative Option Color"]) ?? ConsoleColors.DarkGreen).ToString());
				KernelColorTools.LoadBack();
			}

			// General Section
			DebugWriter.Wdbg(DebugLevel.I, "Parsing general section...");
			Flags.Maintenance = (bool)((ConfigToken["General"]?["Maintenance Mode"]) ?? false);
			Flags.ArgsOnBoot = (bool)((ConfigToken["General"]?["Prompt for Arguments on Boot"]) ?? false);
			Flags.CheckUpdateStart = (bool)((ConfigToken["General"]?["Check for Updates on Startup"]) ?? true);
			if (!string.IsNullOrWhiteSpace((string)ConfigToken["General"]?["Custom Startup Banner"]))
				WelcomeMessage.CustomBanner = (string)ConfigToken["General"]?["Custom Startup Banner"];
			Flags.ShowAppInfoOnBoot = (bool)((ConfigToken["General"]?["Show app information during boot"]) ?? true);
			Flags.ParseCommandLineArguments = (bool)((ConfigToken["General"]?["Parse command-line arguments"]) ?? true);
			Flags.ShowStageFinishTimes = (bool)((ConfigToken["General"]?["Show stage finish times"]) ?? false);
			Flags.StartKernelMods = (bool)((ConfigToken["General"]?["Start kernel modifications on boot"]) ?? true);
			Flags.ShowCurrentTimeBeforeLogin = (bool)((ConfigToken["General"]?["Show current time before login"]) ?? true);
			Flags.NotifyFaultsBoot = (bool)((ConfigToken["General"]?["Notify for any fault during boot"]) ?? true);
			Flags.ShowStackTraceOnKernelError = (bool)((ConfigToken["General"]?["Show stack trace on kernel error"]) ?? false);
			Flags.AutoDownloadUpdate = (bool)((ConfigToken["General"]?["Automatically download updates"]) ?? true);
			Flags.EventDebug = (bool)((ConfigToken["General"]?["Enable event debugging"]) ?? false);
			Flags.NewWelcomeStyle = (bool)((ConfigToken["General"]?["New welcome banner"]) ?? true);
			Flags.EnableSplash = (bool)((ConfigToken["General"]?["Stylish splash screen"]) ?? true);
			SplashManager.SplashName = (string)((ConfigToken["General"]?["Splash name"]) ?? "Simple");
			KernelTools.BannerFigletFont = (string)((ConfigToken["General"]?["Banner figlet font"]) ?? "Banner");
			Flags.SimulateNoAPM = (bool)((ConfigToken["General"]?["Simulate No APM Mode"]) ?? false);

			// Login Section
			DebugWriter.Wdbg(DebugLevel.I, "Parsing login section...");
			Flags.ClearOnLogin = (bool)((ConfigToken["Login"]?["Clear Screen on Log-in"]) ?? false);
			Flags.ShowMOTD = (bool)((ConfigToken["Login"]?["Show MOTD on Log-in"]) ?? true);
			Flags.ShowAvailableUsers = (bool)((ConfigToken["Login"]?["Show available usernames"]) ?? true);
			if (!string.IsNullOrWhiteSpace((string)ConfigToken["Login"]?["Host Name"]))
				Kernel.Kernel.HostName = (string)ConfigToken["Login"]?["Host Name"];
			if (!string.IsNullOrWhiteSpace((string)ConfigToken["Login"]?["MOTD Path"]) & Parsing.TryParsePath((string)ConfigToken["Login"]?["MOTD Path"]))
				MOTDParse.MOTDFilePath = (string)ConfigToken["Login"]?["MOTD Path"];
			if (!string.IsNullOrWhiteSpace((string)ConfigToken["Login"]?["MAL Path"]) & Parsing.TryParsePath((string)ConfigToken["Login"]?["MAL Path"]))
				MOTDParse.MALFilePath = (string)ConfigToken["Login"]?["MAL Path"];
			Login.Login.UsernamePrompt = (string)((ConfigToken["Login"]?["Username prompt style"]) ?? "");
			Login.Login.PasswordPrompt = (string)((ConfigToken["Login"]?["Password prompt style"]) ?? "");
			Flags.ShowMAL = (bool)((ConfigToken["Login"]?["Show MAL on Log-in"]) ?? true);
			UserManagement.IncludeAnonymous = (bool)((ConfigToken["Login"]?["Include anonymous users"]) ?? false);
			UserManagement.IncludeDisabled = (bool)((ConfigToken["Login"]?["Include disabled users"]) ?? false);

			// Shell Section
			DebugWriter.Wdbg(DebugLevel.I, "Parsing shell section...");
			Flags.SimHelp = (bool)((ConfigToken["Shell"]?["Simplified Help Command"]) ?? false);
			CurrentDirectory.CurrentDir = (string)((ConfigToken["Shell"]?["Current Directory"]) ?? Paths.HomePath);
			Shell.Shell.PathsToLookup = !string.IsNullOrEmpty((string)ConfigToken["Shell"]?["Lookup Directories"]) ? (ConfigToken["Shell"]?["Lookup Directories"].ToString().ReleaseDoubleQuotes()) : Environment.GetEnvironmentVariable("PATH");
			PromptPresetManager.SetPreset((string)((ConfigToken["Shell"]?["Prompt Preset"]) ?? "Default"), ShellType.Shell, false);
			PromptPresetManager.SetPreset((string)((ConfigToken["Shell"]?["FTP Prompt Preset"]) ?? "Default"), ShellType.FTPShell, false);
			PromptPresetManager.SetPreset((string)((ConfigToken["Shell"]?["Mail Prompt Preset"]) ?? "Default"), ShellType.MailShell, false);
			PromptPresetManager.SetPreset((string)((ConfigToken["Shell"]?["SFTP Prompt Preset"]) ?? "Default"), ShellType.SFTPShell, false);
			PromptPresetManager.SetPreset((string)((ConfigToken["Shell"]?["RSS Prompt Preset"]) ?? "Default"), ShellType.RSSShell, false);
			PromptPresetManager.SetPreset((string)((ConfigToken["Shell"]?["Text Edit Prompt Preset"]) ?? "Default"), ShellType.TextShell, false);
			PromptPresetManager.SetPreset((string)((ConfigToken["Shell"]?["Zip Shell Prompt Preset"]) ?? "Default"), ShellType.ZIPShell, false);
			PromptPresetManager.SetPreset((string)((ConfigToken["Shell"]?["Test Shell Prompt Preset"]) ?? "Default"), ShellType.TestShell, false);
			PromptPresetManager.SetPreset((string)((ConfigToken["Shell"]?["JSON Shell Prompt Preset"]) ?? "Default"), ShellType.JsonShell, false);
			PromptPresetManager.SetPreset((string)((ConfigToken["Shell"]?["Hex Edit Prompt Preset"]) ?? "Default"), ShellType.HexShell, false);
			PromptPresetManager.SetPreset((string)((ConfigToken["Shell"]?["HTTP Shell Prompt Preset"]) ?? "Default"), ShellType.HTTPShell, false);
			PromptPresetManager.SetPreset((string)((ConfigToken["Shell"]?["RAR Shell Prompt Preset"]) ?? "Default"), ShellType.RARShell, false);
			Flags.ProbeInjectedCommands = (bool)((ConfigToken["Shell"]?["Probe injected commands"]) ?? true);
			Flags.ColorWheelTrueColor = (bool)((ConfigToken["Shell"]?["Start color wheel in true color mode"]) ?? true);
			ConsoleBase.Inputs.Styles.ChoiceStyle.DefaultChoiceOutputType = ConfigToken["Shell"]?["Default choice output type"] is not null ? Enum.TryParse((string)ConfigToken["Shell"]?["Default choice output type"], out ConsoleBase.Inputs.Styles.ChoiceStyle.DefaultChoiceOutputType) ? ConsoleBase.Inputs.Styles.ChoiceStyle.DefaultChoiceOutputType : ChoiceOutputType.Modern : ChoiceOutputType.Modern;

			// Filesystem Section
			DebugWriter.Wdbg(DebugLevel.I, "Parsing filesystem section...");
			Flags.FullParseMode = (bool)((ConfigToken["Filesystem"]?["Size parse mode"]) ?? false);
			Flags.HiddenFiles = (bool)((ConfigToken["Filesystem"]?["Show Hidden Files"]) ?? false);
			Listing.SortMode = ConfigToken["Filesystem"]?["Filesystem sort mode"] is not null ? Enum.TryParse((string)ConfigToken["Filesystem"]?["Filesystem sort mode"], out Listing.SortMode) ? Listing.SortMode : FilesystemSortOptions.FullName : FilesystemSortOptions.FullName;
			Listing.SortDirection = ConfigToken["Filesystem"]?["Filesystem sort direction"] is not null ? Enum.TryParse((string)ConfigToken["Filesystem"]?["Filesystem sort direction"], out Listing.SortDirection) ? Listing.SortDirection : FilesystemSortDirection.Ascending : FilesystemSortDirection.Ascending;
			Filesystem.ShowFilesystemProgress = (bool)((ConfigToken["Filesystem"]?["Show progress on filesystem operations"]) ?? true);
			Listing.ShowFileDetailsList = (bool)((ConfigToken["Filesystem"]?["Show file details in list"]) ?? true);
			Flags.SuppressUnauthorizedMessages = (bool)((ConfigToken["Filesystem"]?["Suppress unauthorized messages"]) ?? true);
			Flags.PrintLineNumbers = (bool)((ConfigToken["Filesystem"]?["Print line numbers on printing file contents"]) ?? false);
			Listing.SortList = (bool)((ConfigToken["Filesystem"]?["Sort the list"]) ?? true);
			Listing.ShowTotalSizeInList = (bool)((ConfigToken["Filesystem"]?["Show total size in list"]) ?? false);

			// Hardware Section
			DebugWriter.Wdbg(DebugLevel.I, "Parsing hardware section...");
			Flags.QuietHardwareProbe = (bool)((ConfigToken["Hardware"]?["Quiet Probe"]) ?? false);
			Flags.FullHardwareProbe = (bool)((ConfigToken["Hardware"]?["Full Probe"]) ?? false);
			Flags.VerboseHardwareProbe = (bool)((ConfigToken["Hardware"]?["Verbose Probe"]) ?? false);

			// Network Section
			DebugWriter.Wdbg(DebugLevel.I, "Parsing network section...");
			int argresult = 0;
			RemoteDebugger.DebugPort = (int)(int.TryParse((string)ConfigToken["Network"]?["Debug Port"], out argresult) ? (ConfigToken["Network"]?["Debug Port"]) : 3014);
			int argresult1 = 0;
			NetworkTools.DownloadRetries = (int)(int.TryParse((string)ConfigToken["Network"]?["Download Retry Times"], out argresult1) ? (ConfigToken["Network"]?["Download Retry Times"]) : 3);
			int argresult2 = 0;
			NetworkTools.UploadRetries = (int)(int.TryParse((string)ConfigToken["Network"]?["Upload Retry Times"], out argresult2) ? (ConfigToken["Network"]?["Upload Retry Times"]) : 3);
			Flags.ShowProgress = (bool)((ConfigToken["Network"]?["Show progress bar while downloading or uploading from \"get\" or \"put\" command"]) ?? true);
			Flags.FTPLoggerUsername = (bool)((ConfigToken["Network"]?["Log FTP username"]) ?? false);
			Flags.FTPLoggerIP = (bool)((ConfigToken["Network"]?["Log FTP IP address"]) ?? false);
			Flags.FTPFirstProfileOnly = (bool)((ConfigToken["Network"]?["Return only first FTP profile"]) ?? false);
			MailManager.ShowPreview = (bool)((ConfigToken["Network"]?["Show mail message preview"]) ?? false);
			Flags.RecordChatToDebugLog = (bool)((ConfigToken["Network"]?["Record chat to debug log"]) ?? true);
			SSH.SSHBanner = (bool)((ConfigToken["Network"]?["Show SSH banner"]) ?? false);
			RemoteProcedure.RPCEnabled = (bool)((ConfigToken["Network"]?["Enable RPC"]) ?? true);
			int argresult3 = 0;
			RemoteProcedure.RPCPort = (int)(int.TryParse((string)ConfigToken["Network"]?["RPC Port"], out argresult3) ? (ConfigToken["Network"]?["RPC Port"]) : 12345);
			FTPShellCommon.FtpShowDetailsInList = (bool)((ConfigToken["Network"]?["Show file details in FTP list"]) ?? true);
			FTPShellCommon.FtpUserPromptStyle = (string)((ConfigToken["Network"]?["Username prompt style for FTP"]) ?? "");
			FTPShellCommon.FtpPassPromptStyle = (string)((ConfigToken["Network"]?["Password prompt style for FTP"]) ?? "");
			FTPShellCommon.FtpUseFirstProfile = (bool)((ConfigToken["Network"]?["Use first FTP profile"]) ?? true);
			FTPShellCommon.FtpNewConnectionsToSpeedDial = (bool)((ConfigToken["Network"]?["Add new connections to FTP speed dial"]) ?? true);
			FTPShellCommon.FtpTryToValidateCertificate = (bool)((ConfigToken["Network"]?["Try to validate secure FTP certificates"]) ?? true);
			FTPShellCommon.FtpShowMotd = (bool)((ConfigToken["Network"]?["Show FTP MOTD on connection"]) ?? true);
			FTPShellCommon.FtpAlwaysAcceptInvalidCerts = (bool)((ConfigToken["Network"]?["Always accept invalid FTP certificates"]) ?? true);
			MailLogin.Mail_UserPromptStyle = (string)((ConfigToken["Network"]?["Username prompt style for mail"]) ?? "");
			MailLogin.Mail_PassPromptStyle = (string)((ConfigToken["Network"]?["Password prompt style for mail"]) ?? "");
			MailLogin.Mail_IMAPPromptStyle = (string)((ConfigToken["Network"]?["IMAP prompt style for mail"]) ?? "");
			MailLogin.Mail_SMTPPromptStyle = (string)((ConfigToken["Network"]?["SMTP prompt style for mail"]) ?? "");
			MailLogin.Mail_AutoDetectServer = (bool)((ConfigToken["Network"]?["Automatically detect mail server"]) ?? true);
			MailLogin.Mail_Debug = (bool)((ConfigToken["Network"]?["Enable mail debug"]) ?? false);
			MailShellCommon.Mail_NotifyNewMail = (bool)((ConfigToken["Network"]?["Notify for new mail messages"]) ?? true);
			MailLogin.Mail_GPGPromptStyle = (string)((ConfigToken["Network"]?["GPG password prompt style for mail"]) ?? true);
			int argresult4 = 0;
			MailShellCommon.Mail_ImapPingInterval = (int)(int.TryParse((string)ConfigToken["Network"]?["Send IMAP ping interval"], out argresult4) ? (ConfigToken["Network"]?["Send IMAP ping interval"]) : 30000);
			int argresult5 = 0;
			MailShellCommon.Mail_SmtpPingInterval = (int)(int.TryParse((string)ConfigToken["Network"]?["Send SMTP ping interval"], out argresult5) ? (ConfigToken["Network"]?["Send SMTP ping interval"]) : 30000);
			MailShellCommon.Mail_TextFormat = ConfigToken["Network"]?["Mail text format"] is not null ? Enum.TryParse((string)ConfigToken["Network"]?["Mail text format"], out MailShellCommon.Mail_TextFormat) ? MailShellCommon.Mail_TextFormat : TextFormat.Plain : TextFormat.Plain;
			RemoteDebugger.RDebugAutoStart = (bool)((ConfigToken["Network"]?["Automatically start remote debug on startup"]) ?? true);
			RemoteDebugger.RDebugMessageFormat = (string)((ConfigToken["Network"]?["Remote debug message format"]) ?? "");
			RSSShellCommon.RSSFeedUrlPromptStyle = (string)((ConfigToken["Network"]?["RSS feed URL prompt style"]) ?? "");
			RSSShellCommon.RSSRefreshFeeds = (bool)((ConfigToken["Network"]?["Auto refresh RSS feed"]) ?? true);
			int argresult6 = 0;
			RSSShellCommon.RSSRefreshInterval = (int)(int.TryParse((string)ConfigToken["Network"]?["Auto refresh RSS feed interval"], out argresult6) ? (ConfigToken["Network"]?["Auto refresh RSS feed interval"]) : 60000);
			SFTPShellCommon.SFTPShowDetailsInList = (bool)((ConfigToken["Network"]?["Show file details in SFTP list"]) ?? true);
			SFTPShellCommon.SFTPUserPromptStyle = (string)((ConfigToken["Network"]?["Username prompt style for SFTP"]) ?? "");
			SFTPShellCommon.SFTPNewConnectionsToSpeedDial = (bool)((ConfigToken["Network"]?["Add new connections to SFTP speed dial"]) ?? true);
			int argresult7 = 0;
			NetworkTools.PingTimeout = (int)(int.TryParse((string)ConfigToken["Network"]?["Ping timeout"], out argresult7) ? (ConfigToken["Network"]?["Ping timeout"]) : 60000);
			Flags.ExtensiveAdapterInformation = (bool)((ConfigToken["Network"]?["Show extensive adapter info"]) ?? true);
			Flags.GeneralNetworkInformation = (bool)((ConfigToken["Network"]?["Show general network information"]) ?? true);
			NetworkTransfer.DownloadPercentagePrint = (string)((ConfigToken["Network"]?["Download percentage text"]) ?? "");
			NetworkTransfer.UploadPercentagePrint = (string)((ConfigToken["Network"]?["Upload percentage text"]) ?? "");
			FTPShellCommon.FtpRecursiveHashing = (bool)((ConfigToken["Network"]?["Recursive hashing for FTP"]) ?? false);
			int argresult8 = 0;
			MailShellCommon.Mail_MaxMessagesInPage = (int)(int.TryParse((string)ConfigToken["Network"]?["Maximum number of e-mails in one page"], out argresult8) ? (ConfigToken["Network"]?["Maximum number of e-mails in one page"]) : 10);
			MailShellCommon.Mail_ShowProgress = (bool)((ConfigToken["Network"]?["Show mail transfer progress"]) ?? false);
			MailShellCommon.Mail_ProgressStyle = (string)((ConfigToken["Network"]?["Mail transfer progress"]) ?? "");
			MailShellCommon.Mail_ProgressStyleSingle = (string)((ConfigToken["Network"]?["Mail transfer progress (single)"]) ?? "");
			NetworkTransfer.DownloadNotificationProvoke = (bool)((ConfigToken["Network"]?["Show notification for download progress"]) ?? false);
			NetworkTransfer.UploadNotificationProvoke = (bool)((ConfigToken["Network"]?["Show notification for upload progress"]) ?? false);
			int argresult9 = 0;
			RSSShellCommon.RSSFetchTimeout = (int)(int.TryParse((string)ConfigToken["Network"]?["RSS feed fetch timeout"], out argresult9) ? (ConfigToken["Network"]?["RSS feed fetch timeout"]) : 60000);
			int argresult10 = 0;
			FTPShellCommon.FtpVerifyRetryAttempts = (int)(int.TryParse((string)ConfigToken["Network"]?["Verify retry attempts for FTP transmission"], out argresult10) ? (ConfigToken["Network"]?["Verify retry attempts for FTP transmission"]) : 3);
			int argresult11 = 0;
			FTPShellCommon.FtpConnectTimeout = (int)(int.TryParse((string)ConfigToken["Network"]?["FTP connection timeout"], out argresult11) ? (ConfigToken["Network"]?["FTP connection timeout"]) : 15000);
			int argresult12 = 0;
			FTPShellCommon.FtpDataConnectTimeout = (int)(int.TryParse((string)ConfigToken["Network"]?["FTP data connection timeout"], out argresult12) ? (ConfigToken["Network"]?["FTP data connection timeout"]) : 15000);
			FTPShellCommon.FtpProtocolVersions = ConfigToken["Network"]?["FTP IP versions"] is not null ? Enum.TryParse((string)ConfigToken["Network"]?["FTP IP versions"], out FTPShellCommon.FtpProtocolVersions) ? FTPShellCommon.FtpProtocolVersions : FtpIpVersion.ANY : FtpIpVersion.ANY;
			Flags.NotifyOnRemoteDebugConnectionError = (bool)((ConfigToken["Network"]?["Notify on remote debug connection error"]) ?? true);

			// Screensaver Section
			Screensaver.Screensaver.DefSaverName = (string)((ConfigToken["Screensaver"]?["Screensaver"]) ?? "matrix");
			int argresult13 = 0;
			Screensaver.Screensaver.ScrnTimeout = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Screensaver Timeout in ms"], out argresult13) ? (ConfigToken["Screensaver"]?["Screensaver Timeout in ms"]) : 300000);
			Screensaver.Screensaver.ScreensaverDebug = (bool)((ConfigToken["Screensaver"]?["Enable screensaver debugging"]) ?? false);
			Screensaver.Screensaver.PasswordLock = (bool)((ConfigToken["Screensaver"]?["Ask for password after locking"]) ?? true);

			// Screensaver-specific settings go below:
			// > ColorMix
			ColorMixSettings.ColorMix255Colors = (bool)((ConfigToken["Screensaver"]?["ColorMix"]?["Activate 255 Color Mode"]) ?? false);
			ColorMixSettings.ColorMixTrueColor = (bool)((ConfigToken["Screensaver"]?["ColorMix"]?["Activate True Color Mode"]) ?? true);
			int argresult14 = 0;
			ColorMixSettings.ColorMixDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ColorMix"]?["Delay in Milliseconds"], out argresult14) ? (ConfigToken["Screensaver"]?["ColorMix"]?["Delay in Milliseconds"]) : 1);
			ColorMixSettings.ColorMixBackgroundColor = new Color(((ConfigToken["Screensaver"]?["ColorMix"]?["Background color"]) ?? ConsoleColors.Red).ToString()).PlainSequence;
			int argresult15 = 0;
			ColorMixSettings.ColorMixMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ColorMix"]?["Minimum red color level"], out argresult15) ? (ConfigToken["Screensaver"]?["ColorMix"]?["Minimum red color level"]) : 0);
			int argresult16 = 0;
			ColorMixSettings.ColorMixMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ColorMix"]?["Minimum green color level"], out argresult16) ? (ConfigToken["Screensaver"]?["ColorMix"]?["Minimum green color level"]) : 0);
			int argresult17 = 0;
			ColorMixSettings.ColorMixMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ColorMix"]?["Minimum blue color level"], out argresult17) ? (ConfigToken["Screensaver"]?["ColorMix"]?["Minimum blue color level"]) : 0);
			int argresult18 = 0;
			ColorMixSettings.ColorMixMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ColorMix"]?["Minimum color level"], out argresult18) ? (ConfigToken["Screensaver"]?["ColorMix"]?["Minimum color level"]) : 0);
			int argresult19 = 0;
			ColorMixSettings.ColorMixMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ColorMix"]?["Maximum red color level"], out argresult19) ? (ConfigToken["Screensaver"]?["ColorMix"]?["Maximum red color level"]) : 255);
			int argresult20 = 0;
			ColorMixSettings.ColorMixMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ColorMix"]?["Maximum green color level"], out argresult20) ? (ConfigToken["Screensaver"]?["ColorMix"]?["Maximum green color level"]) : 255);
			int argresult21 = 0;
			ColorMixSettings.ColorMixMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ColorMix"]?["Maximum blue color level"], out argresult21) ? (ConfigToken["Screensaver"]?["ColorMix"]?["Maximum blue color level"]) : 255);
			int argresult22 = 0;
			ColorMixSettings.ColorMixMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ColorMix"]?["Maximum color level"], out argresult22) ? (ConfigToken["Screensaver"]?["ColorMix"]?["Maximum color level"]) : 255);

			// > Disco
			DiscoSettings.Disco255Colors = (bool)((ConfigToken["Screensaver"]?["Disco"]?["Activate 255 Color Mode"]) ?? false);
			DiscoSettings.DiscoTrueColor = (bool)((ConfigToken["Screensaver"]?["Disco"]?["Activate True Color Mode"]) ?? true);
			DiscoSettings.DiscoCycleColors = (bool)((ConfigToken["Screensaver"]?["Disco"]?["Cycle Colors"]) ?? false);
			int argresult23 = 0;
			DiscoSettings.DiscoDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Disco"]?["Delay in Milliseconds"], out argresult23) ? (ConfigToken["Screensaver"]?["Disco"]?["Delay in Milliseconds"]) : 100);
			DiscoSettings.DiscoUseBeatsPerMinute = (bool)((ConfigToken["Screensaver"]?["Disco"]?["Use Beats Per Minute"]) ?? false);
			DiscoSettings.DiscoEnableFedMode = (bool)((ConfigToken["Screensaver"]?["Disco"]?["Enable Black and White Mode"]) ?? false);
			int argresult24 = 0;
			DiscoSettings.DiscoMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Disco"]?["Minimum red color level"], out argresult24) ? (ConfigToken["Screensaver"]?["Disco"]?["Minimum red color level"]) : 0);
			int argresult25 = 0;
			DiscoSettings.DiscoMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Disco"]?["Minimum green color level"], out argresult25) ? (ConfigToken["Screensaver"]?["Disco"]?["Minimum green color level"]) : 0);
			int argresult26 = 0;
			DiscoSettings.DiscoMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Disco"]?["Minimum blue color level"], out argresult26) ? (ConfigToken["Screensaver"]?["Disco"]?["Minimum blue color level"]) : 0);
			int argresult27 = 0;
			DiscoSettings.DiscoMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Disco"]?["Minimum color level"], out argresult27) ? (ConfigToken["Screensaver"]?["Disco"]?["Minimum color level"]) : 0);
			int argresult28 = 0;
			DiscoSettings.DiscoMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Disco"]?["Maximum red color level"], out argresult28) ? (ConfigToken["Screensaver"]?["Disco"]?["Maximum red color level"]) : 255);
			int argresult29 = 0;
			DiscoSettings.DiscoMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Disco"]?["Maximum green color level"], out argresult29) ? (ConfigToken["Screensaver"]?["Disco"]?["Maximum green color level"]) : 255);
			int argresult30 = 0;
			DiscoSettings.DiscoMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Disco"]?["Maximum blue color level"], out argresult30) ? (ConfigToken["Screensaver"]?["Disco"]?["Maximum blue color level"]) : 255);
			int argresult31 = 0;
			DiscoSettings.DiscoMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Disco"]?["Maximum color level"], out argresult31) ? (ConfigToken["Screensaver"]?["Disco"]?["Maximum color level"]) : 255);

			// > GlitterColor
			GlitterColorSettings.GlitterColor255Colors = (bool)((ConfigToken["Screensaver"]?["GlitterColor"]?["Activate 255 Color Mode"]) ?? false);
			GlitterColorSettings.GlitterColorTrueColor = (bool)((ConfigToken["Screensaver"]?["GlitterColor"]?["Activate True Color Mode"]) ?? true);
			int argresult32 = 0;
			GlitterColorSettings.GlitterColorDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["GlitterColor"]?["Delay in Milliseconds"], out argresult32) ? (ConfigToken["Screensaver"]?["GlitterColor"]?["Delay in Milliseconds"]) : 1);
			int argresult33 = 0;
			GlitterColorSettings.GlitterColorMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["GlitterColor"]?["Minimum red color level"], out argresult33) ? (ConfigToken["Screensaver"]?["GlitterColor"]?["Minimum red color level"]) : 0);
			int argresult34 = 0;
			GlitterColorSettings.GlitterColorMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["GlitterColor"]?["Minimum green color level"], out argresult34) ? (ConfigToken["Screensaver"]?["GlitterColor"]?["Minimum green color level"]) : 0);
			int argresult35 = 0;
			GlitterColorSettings.GlitterColorMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["GlitterColor"]?["Minimum blue color level"], out argresult35) ? (ConfigToken["Screensaver"]?["GlitterColor"]?["Minimum blue color level"]) : 0);
			int argresult36 = 0;
			GlitterColorSettings.GlitterColorMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["GlitterColor"]?["Minimum color level"], out argresult36) ? (ConfigToken["Screensaver"]?["GlitterColor"]?["Minimum color level"]) : 0);
			int argresult37 = 0;
			GlitterColorSettings.GlitterColorMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["GlitterColor"]?["Maximum red color level"], out argresult37) ? (ConfigToken["Screensaver"]?["GlitterColor"]?["Maximum red color level"]) : 255);
			int argresult38 = 0;
			GlitterColorSettings.GlitterColorMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["GlitterColor"]?["Maximum green color level"], out argresult38) ? (ConfigToken["Screensaver"]?["GlitterColor"]?["Maximum green color level"]) : 255);
			int argresult39 = 0;
			GlitterColorSettings.GlitterColorMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["GlitterColor"]?["Maximum blue color level"], out argresult39) ? (ConfigToken["Screensaver"]?["GlitterColor"]?["Maximum blue color level"]) : 255);
			int argresult40 = 0;
			GlitterColorSettings.GlitterColorMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["GlitterColor"]?["Maximum color level"], out argresult40) ? (ConfigToken["Screensaver"]?["GlitterColor"]?["Maximum color level"]) : 255);

			// > GlitterMatrix
			int argresult41 = 0;
			GlitterMatrixSettings.GlitterMatrixDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["GlitterMatrix"]?["Delay in Milliseconds"], out argresult41) ? (ConfigToken["Screensaver"]?["GlitterMatrix"]?["Delay in Milliseconds"]) : 1);
			GlitterMatrixSettings.GlitterMatrixBackgroundColor = new Color(((ConfigToken["Screensaver"]?["GlitterMatrix"]?["Background color"]) ?? ConsoleColors.Black).ToString()).PlainSequence;
			GlitterMatrixSettings.GlitterMatrixForegroundColor = new Color(((ConfigToken["Screensaver"]?["GlitterMatrix"]?["Foreground color"]) ?? ConsoleColors.Green).ToString()).PlainSequence;

			// > Lines
			LinesSettings.Lines255Colors = (bool)((ConfigToken["Screensaver"]?["Lines"]?["Activate 255 Color Mode"]) ?? false);
			LinesSettings.LinesTrueColor = (bool)((ConfigToken["Screensaver"]?["Lines"]?["Activate True Color Mode"]) ?? true);
			int argresult42 = 0;
			LinesSettings.LinesDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lines"]?["Delay in Milliseconds"], out argresult42) ? (ConfigToken["Screensaver"]?["Lines"]?["Delay in Milliseconds"]) : 500);
			LinesSettings.LinesLineChar = (string)((ConfigToken["Screensaver"]?["Lines"]?["Line character"]) ?? "-");
			LinesSettings.LinesBackgroundColor = new Color(((ConfigToken["Screensaver"]?["Lines"]?["Background color"]) ?? ConsoleColors.Black).ToString()).PlainSequence;
			int argresult43 = 0;
			LinesSettings.LinesMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lines"]?["Minimum red color level"], out argresult43) ? (ConfigToken["Screensaver"]?["Lines"]?["Minimum red color level"]) : 0);
			int argresult44 = 0;
			LinesSettings.LinesMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lines"]?["Minimum green color level"], out argresult44) ? (ConfigToken["Screensaver"]?["Lines"]?["Minimum green color level"]) : 0);
			int argresult45 = 0;
			LinesSettings.LinesMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lines"]?["Minimum blue color level"], out argresult45) ? (ConfigToken["Screensaver"]?["Lines"]?["Minimum blue color level"]) : 0);
			int argresult46 = 0;
			LinesSettings.LinesMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lines"]?["Minimum color level"], out argresult46) ? (ConfigToken["Screensaver"]?["Lines"]?["Minimum color level"]) : 0);
			int argresult47 = 0;
			LinesSettings.LinesMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lines"]?["Maximum red color level"], out argresult47) ? (ConfigToken["Screensaver"]?["Lines"]?["Maximum red color level"]) : 255);
			int argresult48 = 0;
			LinesSettings.LinesMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lines"]?["Maximum green color level"], out argresult48) ? (ConfigToken["Screensaver"]?["Lines"]?["Maximum green color level"]) : 255);
			int argresult49 = 0;
			LinesSettings.LinesMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lines"]?["Maximum blue color level"], out argresult49) ? (ConfigToken["Screensaver"]?["Lines"]?["Maximum blue color level"]) : 255);
			int argresult50 = 0;
			LinesSettings.LinesMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lines"]?["Maximum color level"], out argresult50) ? (ConfigToken["Screensaver"]?["Lines"]?["Maximum color level"]) : 255);

			// > Dissolve
			DissolveSettings.Dissolve255Colors = (bool)((ConfigToken["Screensaver"]?["Dissolve"]?["Activate 255 Color Mode"]) ?? false);
			DissolveSettings.DissolveTrueColor = (bool)((ConfigToken["Screensaver"]?["Dissolve"]?["Activate True Color Mode"]) ?? true);
			DissolveSettings.DissolveBackgroundColor = new Color(((ConfigToken["Screensaver"]?["Dissolve"]?["Background color"]) ?? ConsoleColors.Black).ToString()).PlainSequence;
			int argresult51 = 0;
			DissolveSettings.DissolveMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Dissolve"]?["Minimum red color level"], out argresult51) ? (ConfigToken["Screensaver"]?["Dissolve"]?["Minimum red color level"]) : 0);
			int argresult52 = 0;
			DissolveSettings.DissolveMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Dissolve"]?["Minimum green color level"], out argresult52) ? (ConfigToken["Screensaver"]?["Dissolve"]?["Minimum green color level"]) : 0);
			int argresult53 = 0;
			DissolveSettings.DissolveMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Dissolve"]?["Minimum blue color level"], out argresult53) ? (ConfigToken["Screensaver"]?["Dissolve"]?["Minimum blue color level"]) : 0);
			int argresult54 = 0;
			DissolveSettings.DissolveMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Dissolve"]?["Minimum color level"], out argresult54) ? (ConfigToken["Screensaver"]?["Dissolve"]?["Minimum color level"]) : 0);
			int argresult55 = 0;
			DissolveSettings.DissolveMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Dissolve"]?["Maximum red color level"], out argresult55) ? (ConfigToken["Screensaver"]?["Dissolve"]?["Maximum red color level"]) : 255);
			int argresult56 = 0;
			DissolveSettings.DissolveMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Dissolve"]?["Maximum green color level"], out argresult56) ? (ConfigToken["Screensaver"]?["Dissolve"]?["Maximum green color level"]) : 255);
			int argresult57 = 0;
			DissolveSettings.DissolveMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Dissolve"]?["Maximum blue color level"], out argresult57) ? (ConfigToken["Screensaver"]?["Dissolve"]?["Maximum blue color level"]) : 255);
			int argresult58 = 0;
			DissolveSettings.DissolveMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Dissolve"]?["Maximum color level"], out argresult58) ? (ConfigToken["Screensaver"]?["Dissolve"]?["Maximum color level"]) : 255);

			// > BouncingBlock
			BouncingBlockSettings.BouncingBlock255Colors = (bool)((ConfigToken["Screensaver"]?["BouncingBlock"]?["Activate 255 Color Mode"]) ?? false);
			BouncingBlockSettings.BouncingBlockTrueColor = (bool)((ConfigToken["Screensaver"]?["BouncingBlock"]?["Activate True Color Mode"]) ?? true);
			int argresult59 = 0;
			BouncingBlockSettings.BouncingBlockDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingBlock"]?["Delay in Milliseconds"], out argresult59) ? (ConfigToken["Screensaver"]?["BouncingBlock"]?["Delay in Milliseconds"]) : 10);
			BouncingBlockSettings.BouncingBlockBackgroundColor = new Color(((ConfigToken["Screensaver"]?["BouncingBlock"]?["Background color"]) ?? ConsoleColors.Black).ToString()).PlainSequence;
			BouncingBlockSettings.BouncingBlockForegroundColor = new Color(((ConfigToken["Screensaver"]?["BouncingBlock"]?["Foreground color"]) ?? ConsoleColors.White).ToString()).PlainSequence;
			int argresult60 = 0;
			BouncingBlockSettings.BouncingBlockMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingBlock"]?["Minimum red color level"], out argresult60) ? (ConfigToken["Screensaver"]?["BouncingBlock"]?["Minimum red color level"]) : 0);
			int argresult61 = 0;
			BouncingBlockSettings.BouncingBlockMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingBlock"]?["Minimum green color level"], out argresult61) ? (ConfigToken["Screensaver"]?["BouncingBlock"]?["Minimum green color level"]) : 0);
			int argresult62 = 0;
			BouncingBlockSettings.BouncingBlockMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingBlock"]?["Minimum blue color level"], out argresult62) ? (ConfigToken["Screensaver"]?["BouncingBlock"]?["Minimum blue color level"]) : 0);
			int argresult63 = 0;
			BouncingBlockSettings.BouncingBlockMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingBlock"]?["Minimum color level"], out argresult63) ? (ConfigToken["Screensaver"]?["BouncingBlock"]?["Minimum color level"]) : 0);
			int argresult64 = 0;
			BouncingBlockSettings.BouncingBlockMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingBlock"]?["Maximum red color level"], out argresult64) ? (ConfigToken["Screensaver"]?["BouncingBlock"]?["Maximum red color level"]) : 255);
			int argresult65 = 0;
			BouncingBlockSettings.BouncingBlockMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingBlock"]?["Maximum green color level"], out argresult65) ? (ConfigToken["Screensaver"]?["BouncingBlock"]?["Maximum green color level"]) : 255);
			int argresult66 = 0;
			BouncingBlockSettings.BouncingBlockMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingBlock"]?["Maximum blue color level"], out argresult66) ? (ConfigToken["Screensaver"]?["BouncingBlock"]?["Maximum blue color level"]) : 255);
			int argresult67 = 0;
			BouncingBlockSettings.BouncingBlockMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingBlock"]?["Maximum color level"], out argresult67) ? (ConfigToken["Screensaver"]?["BouncingBlock"]?["Maximum color level"]) : 255);

			// > BouncingText
			BouncingTextSettings.BouncingText255Colors = (bool)((ConfigToken["Screensaver"]?["BouncingText"]?["Activate 255 Color Mode"]) ?? false);
			BouncingTextSettings.BouncingTextTrueColor = (bool)((ConfigToken["Screensaver"]?["BouncingText"]?["Activate True Color Mode"]) ?? true);
			int argresult68 = 0;
			BouncingTextSettings.BouncingTextDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingText"]?["Delay in Milliseconds"], out argresult68) ? (ConfigToken["Screensaver"]?["BouncingText"]?["Delay in Milliseconds"]) : 10);
			BouncingTextSettings.BouncingTextWrite = (string)((ConfigToken["Screensaver"]?["BouncingText"]?["Text Shown"]) ?? "Kernel Simulator");
			BouncingTextSettings.BouncingTextBackgroundColor = new Color(((ConfigToken["Screensaver"]?["BouncingText"]?["Background color"]) ?? ConsoleColors.Black).ToString()).PlainSequence;
			BouncingTextSettings.BouncingTextForegroundColor = new Color(((ConfigToken["Screensaver"]?["BouncingText"]?["Foreground color"]) ?? ConsoleColors.White).ToString()).PlainSequence;
			int argresult69 = 0;
			BouncingTextSettings.BouncingTextMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingText"]?["Minimum red color level"], out argresult69) ? (ConfigToken["Screensaver"]?["BouncingText"]?["Minimum red color level"]) : 0);
			int argresult70 = 0;
			BouncingTextSettings.BouncingTextMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingText"]?["Minimum green color level"], out argresult70) ? (ConfigToken["Screensaver"]?["BouncingText"]?["Minimum green color level"]) : 0);
			int argresult71 = 0;
			BouncingTextSettings.BouncingTextMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingText"]?["Minimum blue color level"], out argresult71) ? (ConfigToken["Screensaver"]?["BouncingText"]?["Minimum blue color level"]) : 0);
			int argresult72 = 0;
			BouncingTextSettings.BouncingTextMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingText"]?["Minimum color level"], out argresult72) ? (ConfigToken["Screensaver"]?["BouncingText"]?["Minimum color level"]) : 0);
			int argresult73 = 0;
			BouncingTextSettings.BouncingTextMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingText"]?["Maximum red color level"], out argresult73) ? (ConfigToken["Screensaver"]?["BouncingText"]?["Maximum red color level"]) : 255);
			int argresult74 = 0;
			BouncingTextSettings.BouncingTextMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingText"]?["Maximum green color level"], out argresult74) ? (ConfigToken["Screensaver"]?["BouncingText"]?["Maximum green color level"]) : 255);
			int argresult75 = 0;
			BouncingTextSettings.BouncingTextMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingText"]?["Maximum blue color level"], out argresult75) ? (ConfigToken["Screensaver"]?["BouncingText"]?["Maximum blue color level"]) : 255);
			int argresult76 = 0;
			BouncingTextSettings.BouncingTextMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BouncingText"]?["Maximum color level"], out argresult76) ? (ConfigToken["Screensaver"]?["BouncingText"]?["Maximum color level"]) : 255);

			// > ProgressClock
			ProgressClockSettings.ProgressClock255Colors = (bool)((ConfigToken["Screensaver"]?["ProgressClock"]?["Activate 255 Color Mode"]) ?? false);
			ProgressClockSettings.ProgressClockTrueColor = (bool)((ConfigToken["Screensaver"]?["ProgressClock"]?["Activate True Color Mode"]) ?? true);
			ProgressClockSettings.ProgressClockCycleColors = (bool)((ConfigToken["Screensaver"]?["ProgressClock"]?["Cycle Colors"]) ?? true);
			ProgressClockSettings.ProgressClockSecondsProgressColor = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Color of Seconds Bar"]) ?? 4);
			ProgressClockSettings.ProgressClockMinutesProgressColor = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Color of Minutes Bar"]) ?? 5);
			ProgressClockSettings.ProgressClockHoursProgressColor = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Color of Hours Bar"]) ?? 6);
			ProgressClockSettings.ProgressClockProgressColor = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Color of Information"]) ?? 7);
			int argresult77 = 0;
			ProgressClockSettings.ProgressClockCycleColorsTicks = (long)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Ticks to change color"], out argresult77) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Ticks to change color"]) : 20);
			int argresult78 = 0;
			ProgressClockSettings.ProgressClockDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Delay in Milliseconds"], out argresult78) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Delay in Milliseconds"]) : 500);
			ProgressClockSettings.ProgressClockUpperLeftCornerCharHours = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Upper left corner character for hours bar"]) ?? "╔");
			ProgressClockSettings.ProgressClockUpperLeftCornerCharMinutes = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Upper left corner character for minutes bar"]) ?? "╔");
			ProgressClockSettings.ProgressClockUpperLeftCornerCharSeconds = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Upper left corner character for seconds bar"]) ?? "╔");
			ProgressClockSettings.ProgressClockUpperRightCornerCharHours = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Upper right corner character for hours bar"]) ?? "╗");
			ProgressClockSettings.ProgressClockUpperRightCornerCharMinutes = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Upper right corner character for minutes bar"]) ?? "╗");
			ProgressClockSettings.ProgressClockUpperRightCornerCharSeconds = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Upper right corner character for seconds bar"]) ?? "╗");
			ProgressClockSettings.ProgressClockLowerLeftCornerCharHours = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Lower left corner character for hours bar"]) ?? "╚");
			ProgressClockSettings.ProgressClockLowerLeftCornerCharMinutes = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Lower left corner character for minutes bar"]) ?? "╚");
			ProgressClockSettings.ProgressClockLowerLeftCornerCharSeconds = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Lower left corner character for seconds bar"]) ?? "╚");
			ProgressClockSettings.ProgressClockLowerRightCornerCharHours = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Lower right corner character for hours bar"]) ?? "╝");
			ProgressClockSettings.ProgressClockLowerRightCornerCharMinutes = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Lower right corner character for minutes bar"]) ?? "╝");
			ProgressClockSettings.ProgressClockLowerRightCornerCharSeconds = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Lower right corner character for seconds bar"]) ?? "╝");
			ProgressClockSettings.ProgressClockUpperFrameCharHours = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Upper frame character for hours bar"]) ?? "═");
			ProgressClockSettings.ProgressClockUpperFrameCharMinutes = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Upper frame character for minutes bar"]) ?? "═");
			ProgressClockSettings.ProgressClockUpperFrameCharSeconds = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Upper frame character for seconds bar"]) ?? "═");
			ProgressClockSettings.ProgressClockLowerFrameCharHours = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Lower frame character for hours bar"]) ?? "═");
			ProgressClockSettings.ProgressClockLowerFrameCharMinutes = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Lower frame character for minutes bar"]) ?? "═");
			ProgressClockSettings.ProgressClockLowerFrameCharSeconds = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Lower frame character for seconds bar"]) ?? "═");
			ProgressClockSettings.ProgressClockLeftFrameCharHours = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Left frame character for hours bar"]) ?? "║");
			ProgressClockSettings.ProgressClockLeftFrameCharMinutes = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Left frame character for minutes bar"]) ?? "║");
			ProgressClockSettings.ProgressClockLeftFrameCharSeconds = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Left frame character for seconds bar"]) ?? "║");
			ProgressClockSettings.ProgressClockRightFrameCharHours = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Right frame character for hours bar"]) ?? "║");
			ProgressClockSettings.ProgressClockRightFrameCharMinutes = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Right frame character for minutes bar"]) ?? "║");
			ProgressClockSettings.ProgressClockRightFrameCharSeconds = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Right frame character for seconds bar"]) ?? "║");
			ProgressClockSettings.ProgressClockInfoTextHours = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Information text for hours"]) ?? "");
			ProgressClockSettings.ProgressClockInfoTextMinutes = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Information text for minutes"]) ?? "");
			ProgressClockSettings.ProgressClockInfoTextSeconds = (string)((ConfigToken["Screensaver"]?["ProgressClock"]?["Information text for seconds"]) ?? "");
			int argresult79 = 0;
			ProgressClockSettings.ProgressClockMinimumRedColorLevelHours = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum red color level for hours"], out argresult79) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum red color level for hours"]) : 0);
			int argresult80 = 0;
			ProgressClockSettings.ProgressClockMinimumGreenColorLevelHours = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum green color level for hours"], out argresult80) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum green color level for hours"]) : 0);
			int argresult81 = 0;
			ProgressClockSettings.ProgressClockMinimumBlueColorLevelHours = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum blue color level for hours"], out argresult81) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum blue color level for hours"]) : 0);
			int argresult82 = 0;
			ProgressClockSettings.ProgressClockMinimumColorLevelHours = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum color level for hours"], out argresult82) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum color level for hours"]) : 0);
			int argresult83 = 0;
			ProgressClockSettings.ProgressClockMaximumRedColorLevelHours = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum red color level for hours"], out argresult83) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum red color level for hours"]) : 255);
			int argresult84 = 0;
			ProgressClockSettings.ProgressClockMaximumGreenColorLevelHours = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum green color level for hours"], out argresult84) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum green color level for hours"]) : 255);
			int argresult85 = 0;
			ProgressClockSettings.ProgressClockMaximumBlueColorLevelHours = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum blue color level for hours"], out argresult85) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum blue color level for hours"]) : 255);
			int argresult86 = 0;
			ProgressClockSettings.ProgressClockMaximumColorLevelHours = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum color level for hours"], out argresult86) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum color level for hours"]) : 255);
			int argresult87 = 0;
			ProgressClockSettings.ProgressClockMinimumRedColorLevelMinutes = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum red color level for minutes"], out argresult87) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum red color level for minutes"]) : 0);
			int argresult88 = 0;
			ProgressClockSettings.ProgressClockMinimumGreenColorLevelMinutes = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum green color level for minutes"], out argresult88) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum green color level for minutes"]) : 0);
			int argresult89 = 0;
			ProgressClockSettings.ProgressClockMinimumBlueColorLevelMinutes = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum blue color level for minutes"], out argresult89) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum blue color level for minutes"]) : 0);
			int argresult90 = 0;
			ProgressClockSettings.ProgressClockMinimumColorLevelMinutes = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum color level for minutes"], out argresult90) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum color level for minutes"]) : 0);
			int argresult91 = 0;
			ProgressClockSettings.ProgressClockMaximumRedColorLevelMinutes = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum red color level for minutes"], out argresult91) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum red color level for minutes"]) : 255);
			int argresult92 = 0;
			ProgressClockSettings.ProgressClockMaximumGreenColorLevelMinutes = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum green color level for minutes"], out argresult92) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum green color level for minutes"]) : 255);
			int argresult93 = 0;
			ProgressClockSettings.ProgressClockMaximumBlueColorLevelMinutes = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum blue color level for minutes"], out argresult93) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum blue color level for minutes"]) : 255);
			int argresult94 = 0;
			ProgressClockSettings.ProgressClockMaximumColorLevelMinutes = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum color level for minutes"], out argresult94) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum color level for minutes"]) : 255);
			int argresult95 = 0;
			ProgressClockSettings.ProgressClockMinimumRedColorLevelSeconds = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum red color level for seconds"], out argresult95) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum red color level for seconds"]) : 0);
			int argresult96 = 0;
			ProgressClockSettings.ProgressClockMinimumGreenColorLevelSeconds = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum green color level for seconds"], out argresult96) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum green color level for seconds"]) : 0);
			int argresult97 = 0;
			ProgressClockSettings.ProgressClockMinimumBlueColorLevelSeconds = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum blue color level for seconds"], out argresult97) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum blue color level for seconds"]) : 0);
			int argresult98 = 0;
			ProgressClockSettings.ProgressClockMinimumColorLevelSeconds = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum color level for seconds"], out argresult98) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum color level for seconds"]) : 0);
			int argresult99 = 0;
			ProgressClockSettings.ProgressClockMaximumRedColorLevelSeconds = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum red color level for seconds"], out argresult99) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum red color level for seconds"]) : 255);
			int argresult100 = 0;
			ProgressClockSettings.ProgressClockMaximumGreenColorLevelSeconds = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum green color level for seconds"], out argresult100) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum green color level for seconds"]) : 255);
			int argresult101 = 0;
			ProgressClockSettings.ProgressClockMaximumBlueColorLevelSeconds = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum blue color level for seconds"], out argresult101) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum blue color level for seconds"]) : 255);
			int argresult102 = 0;
			ProgressClockSettings.ProgressClockMaximumColorLevelSeconds = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum color level for seconds"], out argresult102) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum color level for seconds"]) : 255);
			int argresult103 = 0;
			ProgressClockSettings.ProgressClockMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum red color level"], out argresult103) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum red color level"]) : 0);
			int argresult104 = 0;
			ProgressClockSettings.ProgressClockMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum green color level"], out argresult104) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum green color level"]) : 0);
			int argresult105 = 0;
			ProgressClockSettings.ProgressClockMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum blue color level"], out argresult105) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum blue color level"]) : 0);
			int argresult106 = 0;
			ProgressClockSettings.ProgressClockMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum color level"], out argresult106) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Minimum color level"]) : 0);
			int argresult107 = 0;
			ProgressClockSettings.ProgressClockMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum red color level"], out argresult107) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum red color level"]) : 255);
			int argresult108 = 0;
			ProgressClockSettings.ProgressClockMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum green color level"], out argresult108) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum green color level"]) : 255);
			int argresult109 = 0;
			ProgressClockSettings.ProgressClockMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum blue color level"], out argresult109) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum blue color level"]) : 255);
			int argresult110 = 0;
			ProgressClockSettings.ProgressClockMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum color level"], out argresult110) ? (ConfigToken["Screensaver"]?["ProgressClock"]?["Maximum color level"]) : 255);

			// > Lighter
			LighterSettings.Lighter255Colors = (bool)((ConfigToken["Screensaver"]?["Lighter"]?["Activate 255 Color Mode"]) ?? false);
			LighterSettings.LighterTrueColor = (bool)((ConfigToken["Screensaver"]?["Lighter"]?["Activate True Color Mode"]) ?? true);
			int argresult111 = 0;
			LighterSettings.LighterDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lighter"]?["Delay in Milliseconds"], out argresult111) ? (ConfigToken["Screensaver"]?["Lighter"]?["Delay in Milliseconds"]) : 100);
			int argresult112 = 0;
			LighterSettings.LighterMaxPositions = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lighter"]?["Max Positions Count"], out argresult112) ? (ConfigToken["Screensaver"]?["Lighter"]?["Max Positions Count"]) : 10);
			LighterSettings.LighterBackgroundColor = new Color(((ConfigToken["Screensaver"]?["Lighter"]?["Background color"]) ?? ConsoleColors.Black).ToString()).PlainSequence;
			int argresult113 = 0;
			LighterSettings.LighterMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lighter"]?["Minimum red color level"], out argresult113) ? (ConfigToken["Screensaver"]?["Lighter"]?["Minimum red color level"]) : 0);
			int argresult114 = 0;
			LighterSettings.LighterMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lighter"]?["Minimum green color level"], out argresult114) ? (ConfigToken["Screensaver"]?["Lighter"]?["Minimum green color level"]) : 0);
			int argresult115 = 0;
			LighterSettings.LighterMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lighter"]?["Minimum blue color level"], out argresult115) ? (ConfigToken["Screensaver"]?["Lighter"]?["Minimum blue color level"]) : 0);
			int argresult116 = 0;
			LighterSettings.LighterMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lighter"]?["Minimum color level"], out argresult116) ? (ConfigToken["Screensaver"]?["Lighter"]?["Minimum color level"]) : 0);
			int argresult117 = 0;
			LighterSettings.LighterMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lighter"]?["Maximum red color level"], out argresult117) ? (ConfigToken["Screensaver"]?["Lighter"]?["Maximum red color level"]) : 255);
			int argresult118 = 0;
			LighterSettings.LighterMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lighter"]?["Maximum green color level"], out argresult118) ? (ConfigToken["Screensaver"]?["Lighter"]?["Maximum green color level"]) : 255);
			int argresult119 = 0;
			LighterSettings.LighterMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lighter"]?["Maximum blue color level"], out argresult119) ? (ConfigToken["Screensaver"]?["Lighter"]?["Maximum blue color level"]) : 255);
			int argresult120 = 0;
			LighterSettings.LighterMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Lighter"]?["Maximum color level"], out argresult120) ? (ConfigToken["Screensaver"]?["Lighter"]?["Maximum color level"]) : 255);

			// > Wipe
			WipeSettings.Wipe255Colors = (bool)((ConfigToken["Screensaver"]?["Wipe"]?["Activate 255 Color Mode"]) ?? false);
			WipeSettings.WipeTrueColor = (bool)((ConfigToken["Screensaver"]?["Wipe"]?["Activate True Color Mode"]) ?? true);
			int argresult121 = 0;
			WipeSettings.WipeDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Wipe"]?["Delay in Milliseconds"], out argresult121) ? (ConfigToken["Screensaver"]?["Wipe"]?["Delay in Milliseconds"]) : 10);
			int argresult122 = 0;
			WipeSettings.WipeWipesNeededToChangeDirection = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Wipe"]?["Wipes to change direction"], out argresult122) ? (ConfigToken["Screensaver"]?["Wipe"]?["Wipes to change direction"]) : 10);
			WipeSettings.WipeBackgroundColor = new Color(((ConfigToken["Screensaver"]?["Wipe"]?["Background color"]) ?? ConsoleColors.Black).ToString()).PlainSequence;
			int argresult123 = 0;
			WipeSettings.WipeMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Wipe"]?["Minimum red color level"], out argresult123) ? (ConfigToken["Screensaver"]?["Wipe"]?["Minimum red color level"]) : 0);
			int argresult124 = 0;
			WipeSettings.WipeMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Wipe"]?["Minimum green color level"], out argresult124) ? (ConfigToken["Screensaver"]?["Wipe"]?["Minimum green color level"]) : 0);
			int argresult125 = 0;
			WipeSettings.WipeMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Wipe"]?["Minimum blue color level"], out argresult125) ? (ConfigToken["Screensaver"]?["Wipe"]?["Minimum blue color level"]) : 0);
			int argresult126 = 0;
			WipeSettings.WipeMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Wipe"]?["Minimum color level"], out argresult126) ? (ConfigToken["Screensaver"]?["Wipe"]?["Minimum color level"]) : 0);
			int argresult127 = 0;
			WipeSettings.WipeMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Wipe"]?["Maximum red color level"], out argresult127) ? (ConfigToken["Screensaver"]?["Wipe"]?["Maximum red color level"]) : 255);
			int argresult128 = 0;
			WipeSettings.WipeMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Wipe"]?["Maximum green color level"], out argresult128) ? (ConfigToken["Screensaver"]?["Wipe"]?["Maximum green color level"]) : 255);
			int argresult129 = 0;
			WipeSettings.WipeMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Wipe"]?["Maximum blue color level"], out argresult129) ? (ConfigToken["Screensaver"]?["Wipe"]?["Maximum blue color level"]) : 255);
			int argresult130 = 0;
			WipeSettings.WipeMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Wipe"]?["Maximum color level"], out argresult130) ? (ConfigToken["Screensaver"]?["Wipe"]?["Maximum color level"]) : 255);

			// > Fader
			int argresult131 = 0;
			FaderSettings.FaderDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fader"]?["Delay in Milliseconds"], out argresult131) ? (ConfigToken["Screensaver"]?["Fader"]?["Delay in Milliseconds"]) : 50);
			int argresult132 = 0;
			FaderSettings.FaderFadeOutDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fader"]?["Fade Out Delay in Milliseconds"], out argresult132) ? (ConfigToken["Screensaver"]?["Fader"]?["Fade Out Delay in Milliseconds"]) : 3000);
			FaderSettings.FaderWrite = (string)((ConfigToken["Screensaver"]?["Fader"]?["Text Shown"]) ?? "Kernel Simulator");
			int argresult133 = 0;
			FaderSettings.FaderMaxSteps = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fader"]?["Max Fade Steps"], out argresult133) ? (ConfigToken["Screensaver"]?["Fader"]?["Max Fade Steps"]) : 25);
			FaderSettings.FaderBackgroundColor = new Color(((ConfigToken["Screensaver"]?["Fader"]?["Background color"]) ?? ConsoleColors.Black).ToString()).PlainSequence;
			int argresult134 = 0;
			FaderSettings.FaderMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fader"]?["Minimum red color level"], out argresult134) ? (ConfigToken["Screensaver"]?["Fader"]?["Minimum red color level"]) : 0);
			int argresult135 = 0;
			FaderSettings.FaderMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fader"]?["Minimum green color level"], out argresult135) ? (ConfigToken["Screensaver"]?["Fader"]?["Minimum green color level"]) : 0);
			int argresult136 = 0;
			FaderSettings.FaderMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fader"]?["Minimum blue color level"], out argresult136) ? (ConfigToken["Screensaver"]?["Fader"]?["Minimum blue color level"]) : 0);
			int argresult137 = 0;
			FaderSettings.FaderMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fader"]?["Maximum red color level"], out argresult137) ? (ConfigToken["Screensaver"]?["Fader"]?["Maximum red color level"]) : 255);
			int argresult138 = 0;
			FaderSettings.FaderMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fader"]?["Maximum green color level"], out argresult138) ? (ConfigToken["Screensaver"]?["Fader"]?["Maximum green color level"]) : 255);
			int argresult139 = 0;
			FaderSettings.FaderMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fader"]?["Maximum blue color level"], out argresult139) ? (ConfigToken["Screensaver"]?["Fader"]?["Maximum blue color level"]) : 255);

			// > FaderBack
			int argresult140 = 0;
			FaderBackSettings.FaderBackDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FaderBack"]?["Delay in Milliseconds"], out argresult140) ? (ConfigToken["Screensaver"]?["FaderBack"]?["Delay in Milliseconds"]) : 50);
			int argresult141 = 0;
			FaderBackSettings.FaderBackFadeOutDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FaderBack"]?["Fade Out Delay in Milliseconds"], out argresult141) ? (ConfigToken["Screensaver"]?["FaderBack"]?["Fade Out Delay in Milliseconds"]) : 3000);
			int argresult142 = 0;
			FaderBackSettings.FaderBackMaxSteps = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FaderBack"]?["Max Fade Steps"], out argresult142) ? (ConfigToken["Screensaver"]?["FaderBack"]?["Max Fade Steps"]) : 25);
			int argresult143 = 0;
			FaderBackSettings.FaderBackMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FaderBack"]?["Minimum red color level"], out argresult143) ? (ConfigToken["Screensaver"]?["FaderBack"]?["Minimum red color level"]) : 0);
			int argresult144 = 0;
			FaderBackSettings.FaderBackMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FaderBack"]?["Minimum green color level"], out argresult144) ? (ConfigToken["Screensaver"]?["FaderBack"]?["Minimum green color level"]) : 0);
			int argresult145 = 0;
			FaderBackSettings.FaderBackMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FaderBack"]?["Minimum blue color level"], out argresult145) ? (ConfigToken["Screensaver"]?["FaderBack"]?["Minimum blue color level"]) : 0);
			int argresult146 = 0;
			FaderBackSettings.FaderBackMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FaderBack"]?["Maximum red color level"], out argresult146) ? (ConfigToken["Screensaver"]?["FaderBack"]?["Maximum red color level"]) : 255);
			int argresult147 = 0;
			FaderBackSettings.FaderBackMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FaderBack"]?["Maximum green color level"], out argresult147) ? (ConfigToken["Screensaver"]?["FaderBack"]?["Maximum green color level"]) : 255);
			int argresult148 = 0;
			FaderBackSettings.FaderBackMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FaderBack"]?["Maximum blue color level"], out argresult148) ? (ConfigToken["Screensaver"]?["FaderBack"]?["Maximum blue color level"]) : 255);

			// > BeatFader
			BeatFaderSettings.BeatFader255Colors = (bool)((ConfigToken["Screensaver"]?["BeatFader"]?["Activate 255 Color Mode"]) ?? false);
			BeatFaderSettings.BeatFaderTrueColor = (bool)((ConfigToken["Screensaver"]?["BeatFader"]?["Activate True Color Mode"]) ?? true);
			BeatFaderSettings.BeatFaderCycleColors = (bool)((ConfigToken["Screensaver"]?["BeatFader"]?["Cycle Colors"]) ?? true);
			BeatFaderSettings.BeatFaderBeatColor = (string)((ConfigToken["Screensaver"]?["BeatFader"]?["Beat Color"]) ?? 17);
			int argresult149 = 0;
			BeatFaderSettings.BeatFaderDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatFader"]?["Delay in Beats Per Minute"], out argresult149) ? (ConfigToken["Screensaver"]?["BeatFader"]?["Delay in Beats Per Minute"]) : 120);
			int argresult150 = 0;
			BeatFaderSettings.BeatFaderMaxSteps = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatFader"]?["Max Fade Steps"], out argresult150) ? (ConfigToken["Screensaver"]?["BeatFader"]?["Max Fade Steps"]) : 25);
			int argresult151 = 0;
			BeatFaderSettings.BeatFaderMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatFader"]?["Minimum red color level"], out argresult151) ? (ConfigToken["Screensaver"]?["BeatFader"]?["Minimum red color level"]) : 0);
			int argresult152 = 0;
			BeatFaderSettings.BeatFaderMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatFader"]?["Minimum green color level"], out argresult152) ? (ConfigToken["Screensaver"]?["BeatFader"]?["Minimum green color level"]) : 0);
			int argresult153 = 0;
			BeatFaderSettings.BeatFaderMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatFader"]?["Minimum blue color level"], out argresult153) ? (ConfigToken["Screensaver"]?["BeatFader"]?["Minimum blue color level"]) : 0);
			int argresult154 = 0;
			BeatFaderSettings.BeatFaderMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatFader"]?["Minimum color level"], out argresult154) ? (ConfigToken["Screensaver"]?["BeatFader"]?["Minimum color level"]) : 0);
			int argresult155 = 0;
			BeatFaderSettings.BeatFaderMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatFader"]?["Maximum red color level"], out argresult155) ? (ConfigToken["Screensaver"]?["BeatFader"]?["Maximum red color level"]) : 255);
			int argresult156 = 0;
			BeatFaderSettings.BeatFaderMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatFader"]?["Maximum green color level"], out argresult156) ? (ConfigToken["Screensaver"]?["BeatFader"]?["Maximum green color level"]) : 255);
			int argresult157 = 0;
			BeatFaderSettings.BeatFaderMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatFader"]?["Maximum blue color level"], out argresult157) ? (ConfigToken["Screensaver"]?["BeatFader"]?["Maximum blue color level"]) : 255);
			int argresult158 = 0;
			BeatFaderSettings.BeatFaderMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatFader"]?["Maximum color level"], out argresult158) ? (ConfigToken["Screensaver"]?["BeatFader"]?["Maximum color level"]) : 255);

			// > Typo
			int argresult159 = 0;
			TypoSettings.TypoDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Typo"]?["Delay in Milliseconds"], out argresult159) ? (ConfigToken["Screensaver"]?["Typo"]?["Delay in Milliseconds"]) : 50);
			int argresult160 = 0;
			TypoSettings.TypoWriteAgainDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Typo"]?["Write Again Delay in Milliseconds"], out argresult160) ? (ConfigToken["Screensaver"]?["Typo"]?["Write Again Delay in Milliseconds"]) : 3000);
			TypoSettings.TypoWrite = (string)((ConfigToken["Screensaver"]?["Typo"]?["Text Shown"]) ?? "Kernel Simulator");
			int argresult161 = 0;
			TypoSettings.TypoWritingSpeedMin = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Typo"]?["Minimum writing speed in WPM"], out argresult161) ? (ConfigToken["Screensaver"]?["Typo"]?["Minimum writing speed in WPM"]) : 50);
			int argresult162 = 0;
			TypoSettings.TypoWritingSpeedMax = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Typo"]?["Maximum writing speed in WPM"], out argresult162) ? (ConfigToken["Screensaver"]?["Typo"]?["Maximum writing speed in WPM"]) : 80);
			int argresult163 = 0;
			TypoSettings.TypoMissStrikePossibility = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Typo"]?["Probability of typo in percent"], out argresult163) ? (ConfigToken["Screensaver"]?["Typo"]?["Probability of typo in percent"]) : 20);
			int argresult164 = 0;
			TypoSettings.TypoMissPossibility = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Typo"]?["Probability of miss in percent"], out argresult164) ? (ConfigToken["Screensaver"]?["Typo"]?["Probability of miss in percent"]) : 10);
			TypoSettings.TypoTextColor = new Color(((ConfigToken["Screensaver"]?["Typo"]?["Text color"]) ?? ConsoleColors.White).ToString()).PlainSequence;

			// > Marquee
			MarqueeSettings.Marquee255Colors = (bool)((ConfigToken["Screensaver"]?["Marquee"]?["Activate 255 Color Mode"]) ?? false);
			MarqueeSettings.MarqueeTrueColor = (bool)((ConfigToken["Screensaver"]?["Marquee"]?["Activate True Color Mode"]) ?? true);
			MarqueeSettings.MarqueeWrite = (string)((ConfigToken["Screensaver"]?["Marquee"]?["Text Shown"]) ?? "Kernel Simulator");
			int argresult165 = 0;
			MarqueeSettings.MarqueeDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Marquee"]?["Delay in Milliseconds"], out argresult165) ? (ConfigToken["Screensaver"]?["Marquee"]?["Delay in Milliseconds"]) : 10);
			MarqueeSettings.MarqueeAlwaysCentered = (bool)((ConfigToken["Screensaver"]?["Marquee"]?["Always Centered"]) ?? true);
			MarqueeSettings.MarqueeUseConsoleAPI = (bool)((ConfigToken["Screensaver"]?["Marquee"]?["Use Console API"]) ?? false);
			int argresult166 = 0;
			MarqueeSettings.MarqueeMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Marquee"]?["Minimum red color level"], out argresult166) ? (ConfigToken["Screensaver"]?["Marquee"]?["Minimum red color level"]) : 0);
			int argresult167 = 0;
			MarqueeSettings.MarqueeMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Marquee"]?["Minimum green color level"], out argresult167) ? (ConfigToken["Screensaver"]?["Marquee"]?["Minimum green color level"]) : 0);
			int argresult168 = 0;
			MarqueeSettings.MarqueeMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Marquee"]?["Minimum blue color level"], out argresult168) ? (ConfigToken["Screensaver"]?["Marquee"]?["Minimum blue color level"]) : 0);
			int argresult169 = 0;
			MarqueeSettings.MarqueeMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Marquee"]?["Minimum color level"], out argresult169) ? (ConfigToken["Screensaver"]?["Marquee"]?["Minimum color level"]) : 0);
			int argresult170 = 0;
			MarqueeSettings.MarqueeMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Marquee"]?["Maximum red color level"], out argresult170) ? (ConfigToken["Screensaver"]?["Marquee"]?["Maximum red color level"]) : 255);
			int argresult171 = 0;
			MarqueeSettings.MarqueeMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Marquee"]?["Maximum green color level"], out argresult171) ? (ConfigToken["Screensaver"]?["Marquee"]?["Maximum green color level"]) : 255);
			int argresult172 = 0;
			MarqueeSettings.MarqueeMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Marquee"]?["Maximum blue color level"], out argresult172) ? (ConfigToken["Screensaver"]?["Marquee"]?["Maximum blue color level"]) : 255);
			int argresult173 = 0;
			MarqueeSettings.MarqueeMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Marquee"]?["Maximum color level"], out argresult173) ? (ConfigToken["Screensaver"]?["Marquee"]?["Maximum color level"]) : 255);
			MarqueeSettings.MarqueeBackgroundColor = new Color(((ConfigToken["Screensaver"]?["Marquee"]?["Background color"]) ?? ConsoleColors.Black).ToString()).PlainSequence;

			// > Matrix
			int argresult174 = 0;
			MatrixSettings.MatrixDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Matrix"]?["Delay in Milliseconds"], out argresult174) ? (ConfigToken["Screensaver"]?["Matrix"]?["Delay in Milliseconds"]) : 1);

			// > Linotypo
			int argresult175 = 0;
			LinotypoSettings.LinotypoDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Linotypo"]?["Delay in Milliseconds"], out argresult175) ? (ConfigToken["Screensaver"]?["Linotypo"]?["Delay in Milliseconds"]) : 50);
			int argresult176 = 0;
			LinotypoSettings.LinotypoNewScreenDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Linotypo"]?["New Screen Delay in Milliseconds"], out argresult176) ? (ConfigToken["Screensaver"]?["Linotypo"]?["New Screen Delay in Milliseconds"]) : 3000);
			LinotypoSettings.LinotypoWrite = (string)((ConfigToken["Screensaver"]?["Linotypo"]?["Text Shown"]) ?? "Kernel Simulator");
			int argresult177 = 0;
			LinotypoSettings.LinotypoWritingSpeedMin = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Linotypo"]?["Minimum writing speed in WPM"], out argresult177) ? (ConfigToken["Screensaver"]?["Linotypo"]?["Minimum writing speed in WPM"]) : 50);
			int argresult178 = 0;
			LinotypoSettings.LinotypoWritingSpeedMax = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Linotypo"]?["Maximum writing speed in WPM"], out argresult178) ? (ConfigToken["Screensaver"]?["Linotypo"]?["Maximum writing speed in WPM"]) : 80);
			int argresult179 = 0;
			LinotypoSettings.LinotypoMissStrikePossibility = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Linotypo"]?["Probability of typo in percent"], out argresult179) ? (ConfigToken["Screensaver"]?["Linotypo"]?["Probability of typo in percent"]) : 1);
			int argresult180 = 0;
			LinotypoSettings.LinotypoTextColumns = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Linotypo"]?["Column Count"], out argresult180) ? (ConfigToken["Screensaver"]?["Linotypo"]?["Column Count"]) : 3);
			int argresult181 = 0;
			LinotypoSettings.LinotypoEtaoinThreshold = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Linotypo"]?["Line Fill Threshold"], out argresult181) ? (ConfigToken["Screensaver"]?["Linotypo"]?["Line Fill Threshold"]) : 5);
			int argresult182 = 0;
			LinotypoSettings.LinotypoEtaoinCappingPossibility = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Linotypo"]?["Line Fill Capping Probability in percent"], out argresult182) ? (ConfigToken["Screensaver"]?["Linotypo"]?["Line Fill Capping Probability in percent"]) : 5);
			bool localTryParse() { var argresult = LinotypoSettings.LinotypoEtaoinType; var ret = Enum.TryParse((string)ConfigToken["Screensaver"]?["Linotypo"]?["Line Fill Type"], out argresult); LinotypoSettings.LinotypoEtaoinType = argresult; return ret; }

			LinotypoSettings.LinotypoEtaoinType = ConfigToken["Screensaver"]?["Linotypo"]?["Line Fill Type"] is not null ? localTryParse() ? LinotypoSettings.LinotypoEtaoinType : LinotypoSettings.FillType.EtaoinPattern : LinotypoSettings.FillType.EtaoinPattern;
			int argresult183 = 0;
			LinotypoSettings.LinotypoMissPossibility = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Linotypo"]?["Probability of miss in percent"], out argresult183) ? (ConfigToken["Screensaver"]?["Linotypo"]?["Probability of miss in percent"]) : 10);
			LinotypoSettings.LinotypoTextColor = new Color(((ConfigToken["Screensaver"]?["Linotypo"]?["Text color"]) ?? ConsoleColors.White).ToString()).PlainSequence;

			// > Typewriter
			int argresult184 = 0;
			TypewriterSettings.TypewriterDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Typewriter"]?["Delay in Milliseconds"], out argresult184) ? (ConfigToken["Screensaver"]?["Typewriter"]?["Delay in Milliseconds"]) : 50);
			int argresult185 = 0;
			TypewriterSettings.TypewriterNewScreenDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Typewriter"]?["New Screen Delay in Milliseconds"], out argresult185) ? (ConfigToken["Screensaver"]?["Typewriter"]?["New Screen Delay in Milliseconds"]) : 3000);
			TypewriterSettings.TypewriterWrite = (string)((ConfigToken["Screensaver"]?["Typewriter"]?["Text Shown"]) ?? "Kernel Simulator");
			int argresult186 = 0;
			TypewriterSettings.TypewriterWritingSpeedMin = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Typewriter"]?["Minimum writing speed in WPM"], out argresult186) ? (ConfigToken["Screensaver"]?["Typewriter"]?["Minimum writing speed in WPM"]) : 50);
			int argresult187 = 0;
			TypewriterSettings.TypewriterWritingSpeedMax = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Typewriter"]?["Maximum writing speed in WPM"], out argresult187) ? (ConfigToken["Screensaver"]?["Typewriter"]?["Maximum writing speed in WPM"]) : 80);
			TypewriterSettings.TypewriterTextColor = new Color(((ConfigToken["Screensaver"]?["Typewriter"]?["Text color"]) ?? ConsoleColors.White).ToString()).PlainSequence;

			// > FlashColor
			FlashColorSettings.FlashColor255Colors = (bool)((ConfigToken["Screensaver"]?["FlashColor"]?["Activate 255 Color Mode"]) ?? false);
			FlashColorSettings.FlashColorTrueColor = (bool)((ConfigToken["Screensaver"]?["FlashColor"]?["Activate True Color Mode"]) ?? true);
			int argresult188 = 0;
			FlashColorSettings.FlashColorDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashColor"]?["Delay in Milliseconds"], out argresult188) ? (ConfigToken["Screensaver"]?["FlashColor"]?["Delay in Milliseconds"]) : 1);
			FlashColorSettings.FlashColorBackgroundColor = new Color(((ConfigToken["Screensaver"]?["FlashColor"]?["Background color"]) ?? ConsoleColors.Black).ToString()).PlainSequence;
			int argresult189 = 0;
			FlashColorSettings.FlashColorMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashColor"]?["Minimum red color level"], out argresult189) ? (ConfigToken["Screensaver"]?["FlashColor"]?["Minimum red color level"]) : 0);
			int argresult190 = 0;
			FlashColorSettings.FlashColorMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashColor"]?["Minimum green color level"], out argresult190) ? (ConfigToken["Screensaver"]?["FlashColor"]?["Minimum green color level"]) : 0);
			int argresult191 = 0;
			FlashColorSettings.FlashColorMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashColor"]?["Minimum blue color level"], out argresult191) ? (ConfigToken["Screensaver"]?["FlashColor"]?["Minimum blue color level"]) : 0);
			int argresult192 = 0;
			FlashColorSettings.FlashColorMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashColor"]?["Minimum color level"], out argresult192) ? (ConfigToken["Screensaver"]?["FlashColor"]?["Minimum color level"]) : 0);
			int argresult193 = 0;
			FlashColorSettings.FlashColorMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashColor"]?["Maximum red color level"], out argresult193) ? (ConfigToken["Screensaver"]?["FlashColor"]?["Maximum red color level"]) : 255);
			int argresult194 = 0;
			FlashColorSettings.FlashColorMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashColor"]?["Maximum green color level"], out argresult194) ? (ConfigToken["Screensaver"]?["FlashColor"]?["Maximum green color level"]) : 255);
			int argresult195 = 0;
			FlashColorSettings.FlashColorMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashColor"]?["Maximum blue color level"], out argresult195) ? (ConfigToken["Screensaver"]?["FlashColor"]?["Maximum blue color level"]) : 255);
			int argresult196 = 0;
			FlashColorSettings.FlashColorMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashColor"]?["Maximum color level"], out argresult196) ? (ConfigToken["Screensaver"]?["FlashColor"]?["Maximum color level"]) : 255);

			// > SpotWrite
			int argresult197 = 0;
			SpotWriteSettings.SpotWriteDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["SpotWrite"]?["Delay in Milliseconds"], out argresult197) ? (ConfigToken["Screensaver"]?["SpotWrite"]?["Delay in Milliseconds"]) : 50);
			int argresult198 = 0;
			SpotWriteSettings.SpotWriteNewScreenDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["SpotWrite"]?["New Screen Delay in Milliseconds"], out argresult198) ? (ConfigToken["Screensaver"]?["SpotWrite"]?["New Screen Delay in Milliseconds"]) : 3000);
			SpotWriteSettings.SpotWriteWrite = (string)((ConfigToken["Screensaver"]?["SpotWrite"]?["Text Shown"]) ?? "Kernel Simulator");
			SpotWriteSettings.SpotWriteTextColor = new Color(((ConfigToken["Screensaver"]?["SpotWrite"]?["Text color"]) ?? ConsoleColors.White).ToString()).PlainSequence;

			// > Ramp
			RampSettings.Ramp255Colors = (bool)((ConfigToken["Screensaver"]?["Ramp"]?["Activate 255 Color Mode"]) ?? false);
			RampSettings.RampTrueColor = (bool)((ConfigToken["Screensaver"]?["Ramp"]?["Activate True Color Mode"]) ?? true);
			int argresult199 = 0;
			RampSettings.RampDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Delay in Milliseconds"], out argresult199) ? (ConfigToken["Screensaver"]?["Ramp"]?["Delay in Milliseconds"]) : 20);
			int argresult200 = 0;
			RampSettings.RampNextRampDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Next ramp interval"], out argresult200) ? (ConfigToken["Screensaver"]?["Ramp"]?["Next ramp interval"]) : 250);
			RampSettings.RampUpperLeftCornerChar = (string)((ConfigToken["Screensaver"]?["Ramp"]?["Upper left corner character for ramp bar"]) ?? "╔");
			RampSettings.RampUpperRightCornerChar = (string)((ConfigToken["Screensaver"]?["Ramp"]?["Upper right corner character for ramp bar"]) ?? "╗");
			RampSettings.RampLowerLeftCornerChar = (string)((ConfigToken["Screensaver"]?["Ramp"]?["Lower left corner character for ramp bar"]) ?? "╚");
			RampSettings.RampLowerRightCornerChar = (string)((ConfigToken["Screensaver"]?["Ramp"]?["Lower right corner character for ramp bar"]) ?? "╝");
			RampSettings.RampUpperFrameChar = (string)((ConfigToken["Screensaver"]?["Ramp"]?["Upper frame character for ramp bar"]) ?? "═");
			RampSettings.RampLowerFrameChar = (string)((ConfigToken["Screensaver"]?["Ramp"]?["Lower frame character for ramp bar"]) ?? "═");
			RampSettings.RampLeftFrameChar = (string)((ConfigToken["Screensaver"]?["Ramp"]?["Left frame character for ramp bar"]) ?? "║");
			RampSettings.RampRightFrameChar = (string)((ConfigToken["Screensaver"]?["Ramp"]?["Right frame character for ramp bar"]) ?? "║");
			int argresult201 = 0;
			RampSettings.RampMinimumRedColorLevelStart = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Minimum red color level for start color"], out argresult201) ? (ConfigToken["Screensaver"]?["Ramp"]?["Minimum red color level for start color"]) : 0);
			int argresult202 = 0;
			RampSettings.RampMinimumGreenColorLevelStart = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Minimum green color level for start color"], out argresult202) ? (ConfigToken["Screensaver"]?["Ramp"]?["Minimum green color level for start color"]) : 0);
			int argresult203 = 0;
			RampSettings.RampMinimumBlueColorLevelStart = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Minimum blue color level for start color"], out argresult203) ? (ConfigToken["Screensaver"]?["Ramp"]?["Minimum blue color level for start color"]) : 0);
			int argresult204 = 0;
			RampSettings.RampMinimumColorLevelStart = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Minimum color level for start color"], out argresult204) ? (ConfigToken["Screensaver"]?["Ramp"]?["Minimum color level for start color"]) : 0);
			int argresult205 = 0;
			RampSettings.RampMaximumRedColorLevelStart = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Maximum red color level for start color"], out argresult205) ? (ConfigToken["Screensaver"]?["Ramp"]?["Maximum red color level for start color"]) : 255);
			int argresult206 = 0;
			RampSettings.RampMaximumGreenColorLevelStart = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Maximum green color level for start color"], out argresult206) ? (ConfigToken["Screensaver"]?["Ramp"]?["Maximum green color level for start color"]) : 255);
			int argresult207 = 0;
			RampSettings.RampMaximumBlueColorLevelStart = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Maximum blue color level for start color"], out argresult207) ? (ConfigToken["Screensaver"]?["Ramp"]?["Maximum blue color level for start color"]) : 255);
			int argresult208 = 0;
			RampSettings.RampMaximumColorLevelStart = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Maximum color level for start color"], out argresult208) ? (ConfigToken["Screensaver"]?["Ramp"]?["Maximum color level for start color"]) : 255);
			int argresult209 = 0;
			RampSettings.RampMinimumRedColorLevelEnd = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Minimum red color level for end color"], out argresult209) ? (ConfigToken["Screensaver"]?["Ramp"]?["Minimum red color level for end color"]) : 0);
			int argresult210 = 0;
			RampSettings.RampMinimumGreenColorLevelEnd = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Minimum green color level for end color"], out argresult210) ? (ConfigToken["Screensaver"]?["Ramp"]?["Minimum green color level for end color"]) : 0);
			int argresult211 = 0;
			RampSettings.RampMinimumBlueColorLevelEnd = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Minimum blue color level for end color"], out argresult211) ? (ConfigToken["Screensaver"]?["Ramp"]?["Minimum blue color level for end color"]) : 0);
			int argresult212 = 0;
			RampSettings.RampMinimumColorLevelEnd = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Minimum color level for end color"], out argresult212) ? (ConfigToken["Screensaver"]?["Ramp"]?["Minimum color level for end color"]) : 0);
			int argresult213 = 0;
			RampSettings.RampMaximumRedColorLevelEnd = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Maximum red color level for end color"], out argresult213) ? (ConfigToken["Screensaver"]?["Ramp"]?["Maximum red color level for end color"]) : 255);
			int argresult214 = 0;
			RampSettings.RampMaximumGreenColorLevelEnd = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Maximum green color level for end color"], out argresult214) ? (ConfigToken["Screensaver"]?["Ramp"]?["Maximum green color level for end color"]) : 255);
			int argresult215 = 0;
			RampSettings.RampMaximumBlueColorLevelEnd = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Maximum blue color level for end color"], out argresult215) ? (ConfigToken["Screensaver"]?["Ramp"]?["Maximum blue color level for end color"]) : 255);
			int argresult216 = 0;
			RampSettings.RampMaximumColorLevelEnd = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Ramp"]?["Maximum color level for end color"], out argresult216) ? (ConfigToken["Screensaver"]?["Ramp"]?["Maximum color level for end color"]) : 255);
			RampSettings.RampUpperLeftCornerColor = new Color(((ConfigToken["Screensaver"]?["Ramp"]?["Upper left corner color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			RampSettings.RampUpperRightCornerColor = new Color(((ConfigToken["Screensaver"]?["Ramp"]?["Upper right corner color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			RampSettings.RampLowerLeftCornerColor = new Color(((ConfigToken["Screensaver"]?["Ramp"]?["Lower left corner color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			RampSettings.RampLowerRightCornerColor = new Color(((ConfigToken["Screensaver"]?["Ramp"]?["Lower right corner color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			RampSettings.RampUpperFrameColor = new Color(((ConfigToken["Screensaver"]?["Ramp"]?["Upper frame color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			RampSettings.RampLowerFrameColor = new Color(((ConfigToken["Screensaver"]?["Ramp"]?["Lower frame color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			RampSettings.RampLeftFrameColor = new Color(((ConfigToken["Screensaver"]?["Ramp"]?["Left frame color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			RampSettings.RampRightFrameColor = new Color(((ConfigToken["Screensaver"]?["Ramp"]?["Right frame color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			RampSettings.RampUseBorderColors = (bool)((ConfigToken["Screensaver"]?["Ramp"]?["Use border colors for ramp bar"]) ?? false);

			// > StackBox
			StackBoxSettings.StackBox255Colors = (bool)((ConfigToken["Screensaver"]?["StackBox"]?["Activate 255 Color Mode"]) ?? false);
			StackBoxSettings.StackBoxTrueColor = (bool)((ConfigToken["Screensaver"]?["StackBox"]?["Activate True Color Mode"]) ?? true);
			int argresult217 = 0;
			StackBoxSettings.StackBoxDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["StackBox"]?["Delay in Milliseconds"], out argresult217) ? (ConfigToken["Screensaver"]?["StackBox"]?["Delay in Milliseconds"]) : 10);
			int argresult218 = 0;
			StackBoxSettings.StackBoxMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["StackBox"]?["Minimum red color level"], out argresult218) ? (ConfigToken["Screensaver"]?["StackBox"]?["Minimum red color level"]) : 0);
			int argresult219 = 0;
			StackBoxSettings.StackBoxMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["StackBox"]?["Minimum green color level"], out argresult219) ? (ConfigToken["Screensaver"]?["StackBox"]?["Minimum green color level"]) : 0);
			int argresult220 = 0;
			StackBoxSettings.StackBoxMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["StackBox"]?["Minimum blue color level"], out argresult220) ? (ConfigToken["Screensaver"]?["StackBox"]?["Minimum blue color level"]) : 0);
			int argresult221 = 0;
			StackBoxSettings.StackBoxMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["StackBox"]?["Minimum color level"], out argresult221) ? (ConfigToken["Screensaver"]?["StackBox"]?["Minimum color level"]) : 0);
			int argresult222 = 0;
			StackBoxSettings.StackBoxMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["StackBox"]?["Maximum red color level"], out argresult222) ? (ConfigToken["Screensaver"]?["StackBox"]?["Maximum red color level"]) : 255);
			int argresult223 = 0;
			StackBoxSettings.StackBoxMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["StackBox"]?["Maximum green color level"], out argresult223) ? (ConfigToken["Screensaver"]?["StackBox"]?["Maximum green color level"]) : 255);
			int argresult224 = 0;
			StackBoxSettings.StackBoxMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["StackBox"]?["Maximum blue color level"], out argresult224) ? (ConfigToken["Screensaver"]?["StackBox"]?["Maximum blue color level"]) : 255);
			int argresult225 = 0;
			StackBoxSettings.StackBoxMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["StackBox"]?["Maximum color level"], out argresult225) ? (ConfigToken["Screensaver"]?["StackBox"]?["Maximum color level"]) : 255);
			StackBoxSettings.StackBoxFill = (bool)((ConfigToken["Screensaver"]?["StackBox"]?["Fill the boxes"]) ?? true);

			// > Snaker
			SnakerSettings.Snaker255Colors = (bool)((ConfigToken["Screensaver"]?["Snaker"]?["Activate 255 Color Mode"]) ?? false);
			SnakerSettings.SnakerTrueColor = (bool)((ConfigToken["Screensaver"]?["Snaker"]?["Activate True Color Mode"]) ?? true);
			int argresult226 = 0;
			SnakerSettings.SnakerDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Snaker"]?["Delay in Milliseconds"], out argresult226) ? (ConfigToken["Screensaver"]?["Snaker"]?["Delay in Milliseconds"]) : 100);
			int argresult227 = 0;
			SnakerSettings.SnakerStageDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Snaker"]?["Stage delay in milliseconds"], out argresult227) ? (ConfigToken["Screensaver"]?["Snaker"]?["Stage delay in milliseconds"]) : 5000);
			int argresult228 = 0;
			SnakerSettings.SnakerMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Snaker"]?["Minimum red color level"], out argresult228) ? (ConfigToken["Screensaver"]?["Snaker"]?["Minimum red color level"]) : 0);
			int argresult229 = 0;
			SnakerSettings.SnakerMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Snaker"]?["Minimum green color level"], out argresult229) ? (ConfigToken["Screensaver"]?["Snaker"]?["Minimum green color level"]) : 0);
			int argresult230 = 0;
			SnakerSettings.SnakerMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Snaker"]?["Minimum blue color level"], out argresult230) ? (ConfigToken["Screensaver"]?["Snaker"]?["Minimum blue color level"]) : 0);
			int argresult231 = 0;
			SnakerSettings.SnakerMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Snaker"]?["Minimum color level"], out argresult231) ? (ConfigToken["Screensaver"]?["Snaker"]?["Minimum color level"]) : 0);
			int argresult232 = 0;
			SnakerSettings.SnakerMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Snaker"]?["Maximum red color level"], out argresult232) ? (ConfigToken["Screensaver"]?["Snaker"]?["Maximum red color level"]) : 255);
			int argresult233 = 0;
			SnakerSettings.SnakerMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Snaker"]?["Maximum green color level"], out argresult233) ? (ConfigToken["Screensaver"]?["Snaker"]?["Maximum green color level"]) : 255);
			int argresult234 = 0;
			SnakerSettings.SnakerMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Snaker"]?["Maximum blue color level"], out argresult234) ? (ConfigToken["Screensaver"]?["Snaker"]?["Maximum blue color level"]) : 255);
			int argresult235 = 0;
			SnakerSettings.SnakerMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Snaker"]?["Maximum color level"], out argresult235) ? (ConfigToken["Screensaver"]?["Snaker"]?["Maximum color level"]) : 255);

			// > BarRot
			BarRotSettings.BarRot255Colors = (bool)((ConfigToken["Screensaver"]?["BarRot"]?["Activate 255 Color Mode"]) ?? false);
			BarRotSettings.BarRotTrueColor = (bool)((ConfigToken["Screensaver"]?["BarRot"]?["Activate True Color Mode"]) ?? true);
			int argresult236 = 0;
			BarRotSettings.BarRotDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BarRot"]?["Delay in Milliseconds"], out argresult236) ? (ConfigToken["Screensaver"]?["BarRot"]?["Delay in Milliseconds"]) : 10);
			int argresult237 = 0;
			BarRotSettings.BarRotNextRampDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BarRot"]?["Next ramp rot interval"], out argresult237) ? (ConfigToken["Screensaver"]?["BarRot"]?["Next ramp rot interval"]) : 250);
			BarRotSettings.BarRotUpperLeftCornerChar = (string)((ConfigToken["Screensaver"]?["BarRot"]?["Upper left corner character for ramp bar"]) ?? "╔");
			BarRotSettings.BarRotUpperRightCornerChar = (string)((ConfigToken["Screensaver"]?["BarRot"]?["Upper right corner character for ramp bar"]) ?? "╗");
			BarRotSettings.BarRotLowerLeftCornerChar = (string)((ConfigToken["Screensaver"]?["BarRot"]?["Lower left corner character for ramp bar"]) ?? "╚");
			BarRotSettings.BarRotLowerRightCornerChar = (string)((ConfigToken["Screensaver"]?["BarRot"]?["Lower right corner character for ramp bar"]) ?? "╝");
			BarRotSettings.BarRotUpperFrameChar = (string)((ConfigToken["Screensaver"]?["BarRot"]?["Upper frame character for ramp bar"]) ?? "═");
			BarRotSettings.BarRotLowerFrameChar = (string)((ConfigToken["Screensaver"]?["BarRot"]?["Lower frame character for ramp bar"]) ?? "═");
			BarRotSettings.BarRotLeftFrameChar = (string)((ConfigToken["Screensaver"]?["BarRot"]?["Left frame character for ramp bar"]) ?? "║");
			BarRotSettings.BarRotRightFrameChar = (string)((ConfigToken["Screensaver"]?["BarRot"]?["Right frame character for ramp bar"]) ?? "║");
			int argresult238 = 0;
			BarRotSettings.BarRotMinimumRedColorLevelStart = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BarRot"]?["Minimum red color level for start color"], out argresult238) ? (ConfigToken["Screensaver"]?["BarRot"]?["Minimum red color level for start color"]) : 0);
			int argresult239 = 0;
			BarRotSettings.BarRotMinimumGreenColorLevelStart = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BarRot"]?["Minimum green color level for start color"], out argresult239) ? (ConfigToken["Screensaver"]?["BarRot"]?["Minimum green color level for start color"]) : 0);
			int argresult240 = 0;
			BarRotSettings.BarRotMinimumBlueColorLevelStart = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BarRot"]?["Minimum blue color level for start color"], out argresult240) ? (ConfigToken["Screensaver"]?["BarRot"]?["Minimum blue color level for start color"]) : 0);
			int argresult241 = 0;
			BarRotSettings.BarRotMaximumRedColorLevelStart = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BarRot"]?["Maximum red color level for start color"], out argresult241) ? (ConfigToken["Screensaver"]?["BarRot"]?["Maximum red color level for start color"]) : 255);
			int argresult242 = 0;
			BarRotSettings.BarRotMaximumGreenColorLevelStart = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BarRot"]?["Maximum green color level for start color"], out argresult242) ? (ConfigToken["Screensaver"]?["BarRot"]?["Maximum green color level for start color"]) : 255);
			int argresult243 = 0;
			BarRotSettings.BarRotMaximumBlueColorLevelStart = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BarRot"]?["Maximum blue color level for start color"], out argresult243) ? (ConfigToken["Screensaver"]?["BarRot"]?["Maximum blue color level for start color"]) : 255);
			int argresult244 = 0;
			BarRotSettings.BarRotMinimumRedColorLevelEnd = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BarRot"]?["Minimum red color level for end color"], out argresult244) ? (ConfigToken["Screensaver"]?["BarRot"]?["Minimum red color level for end color"]) : 0);
			int argresult245 = 0;
			BarRotSettings.BarRotMinimumGreenColorLevelEnd = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BarRot"]?["Minimum green color level for end color"], out argresult245) ? (ConfigToken["Screensaver"]?["BarRot"]?["Minimum green color level for end color"]) : 0);
			int argresult246 = 0;
			BarRotSettings.BarRotMinimumBlueColorLevelEnd = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BarRot"]?["Minimum blue color level for end color"], out argresult246) ? (ConfigToken["Screensaver"]?["BarRot"]?["Minimum blue color level for end color"]) : 0);
			int argresult247 = 0;
			BarRotSettings.BarRotMaximumRedColorLevelEnd = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BarRot"]?["Maximum red color level for end color"], out argresult247) ? (ConfigToken["Screensaver"]?["BarRot"]?["Maximum red color level for end color"]) : 255);
			int argresult248 = 0;
			BarRotSettings.BarRotMaximumGreenColorLevelEnd = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BarRot"]?["Maximum green color level for end color"], out argresult248) ? (ConfigToken["Screensaver"]?["BarRot"]?["Maximum green color level for end color"]) : 255);
			int argresult249 = 0;
			BarRotSettings.BarRotMaximumBlueColorLevelEnd = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BarRot"]?["Maximum blue color level for end color"], out argresult249) ? (ConfigToken["Screensaver"]?["BarRot"]?["Maximum blue color level for end color"]) : 255);
			BarRotSettings.BarRotUpperLeftCornerColor = new Color(((ConfigToken["Screensaver"]?["BarRot"]?["Upper left corner color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			BarRotSettings.BarRotUpperRightCornerColor = new Color(((ConfigToken["Screensaver"]?["BarRot"]?["Upper right corner color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			BarRotSettings.BarRotLowerLeftCornerColor = new Color(((ConfigToken["Screensaver"]?["BarRot"]?["Lower left corner color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			BarRotSettings.BarRotLowerRightCornerColor = new Color(((ConfigToken["Screensaver"]?["BarRot"]?["Lower right corner color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			BarRotSettings.BarRotUpperFrameColor = new Color(((ConfigToken["Screensaver"]?["BarRot"]?["Upper frame color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			BarRotSettings.BarRotLowerFrameColor = new Color(((ConfigToken["Screensaver"]?["BarRot"]?["Lower frame color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			BarRotSettings.BarRotLeftFrameColor = new Color(((ConfigToken["Screensaver"]?["BarRot"]?["Left frame color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			BarRotSettings.BarRotRightFrameColor = new Color(((ConfigToken["Screensaver"]?["BarRot"]?["Right frame color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			BarRotSettings.BarRotUseBorderColors = (bool)((ConfigToken["Screensaver"]?["BarRot"]?["Use border colors for ramp bar"]) ?? false);

			// > Fireworks
			FireworksSettings.Fireworks255Colors = (bool)((ConfigToken["Screensaver"]?["Fireworks"]?["Activate 255 Color Mode"]) ?? false);
			FireworksSettings.FireworksTrueColor = (bool)((ConfigToken["Screensaver"]?["Fireworks"]?["Activate True Color Mode"]) ?? true);
			int argresult250 = 0;
			FireworksSettings.FireworksDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fireworks"]?["Delay in Milliseconds"], out argresult250) ? (ConfigToken["Screensaver"]?["Fireworks"]?["Delay in Milliseconds"]) : 10);
			int argresult251 = 0;
			FireworksSettings.FireworksRadius = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fireworks"]?["Firework explosion radius"], out argresult251) ? (ConfigToken["Screensaver"]?["Fireworks"]?["Firework explosion radius"]) : 5);
			int argresult252 = 0;
			FireworksSettings.FireworksMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fireworks"]?["Minimum red color level"], out argresult252) ? (ConfigToken["Screensaver"]?["Fireworks"]?["Minimum red color level"]) : 0);
			int argresult253 = 0;
			FireworksSettings.FireworksMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fireworks"]?["Minimum green color level"], out argresult253) ? (ConfigToken["Screensaver"]?["Fireworks"]?["Minimum green color level"]) : 0);
			int argresult254 = 0;
			FireworksSettings.FireworksMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fireworks"]?["Minimum blue color level"], out argresult254) ? (ConfigToken["Screensaver"]?["Fireworks"]?["Minimum blue color level"]) : 0);
			int argresult255 = 0;
			FireworksSettings.FireworksMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fireworks"]?["Minimum color level"], out argresult255) ? (ConfigToken["Screensaver"]?["Fireworks"]?["Minimum color level"]) : 0);
			int argresult256 = 0;
			FireworksSettings.FireworksMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fireworks"]?["Maximum red color level"], out argresult256) ? (ConfigToken["Screensaver"]?["Fireworks"]?["Maximum red color level"]) : 255);
			int argresult257 = 0;
			FireworksSettings.FireworksMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fireworks"]?["Maximum green color level"], out argresult257) ? (ConfigToken["Screensaver"]?["Fireworks"]?["Maximum green color level"]) : 255);
			int argresult258 = 0;
			FireworksSettings.FireworksMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fireworks"]?["Maximum blue color level"], out argresult258) ? (ConfigToken["Screensaver"]?["Fireworks"]?["Maximum blue color level"]) : 255);
			int argresult259 = 0;
			FireworksSettings.FireworksMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Fireworks"]?["Maximum color level"], out argresult259) ? (ConfigToken["Screensaver"]?["Fireworks"]?["Maximum color level"]) : 255);

			// > Figlet
			FigletSettings.Figlet255Colors = (bool)((ConfigToken["Screensaver"]?["Figlet"]?["Activate 255 Color Mode"]) ?? false);
			FigletSettings.FigletTrueColor = (bool)((ConfigToken["Screensaver"]?["Figlet"]?["Activate True Color Mode"]) ?? true);
			int argresult260 = 0;
			FigletSettings.FigletDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Figlet"]?["Delay in Milliseconds"], out argresult260) ? (ConfigToken["Screensaver"]?["Figlet"]?["Delay in Milliseconds"]) : 10);
			FigletSettings.FigletText = (string)((ConfigToken["Screensaver"]?["Figlet"]?["Text Shown"]) ?? "Kernel Simulator");
			FigletSettings.FigletFont = (string)((ConfigToken["Screensaver"]?["Figlet"]?["Figlet font"]) ?? "Small");
			int argresult261 = 0;
			FigletSettings.FigletMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Figlet"]?["Minimum red color level"], out argresult261) ? (ConfigToken["Screensaver"]?["Figlet"]?["Minimum red color level"]) : 0);
			int argresult262 = 0;
			FigletSettings.FigletMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Figlet"]?["Minimum green color level"], out argresult262) ? (ConfigToken["Screensaver"]?["Figlet"]?["Minimum green color level"]) : 0);
			int argresult263 = 0;
			FigletSettings.FigletMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Figlet"]?["Minimum blue color level"], out argresult263) ? (ConfigToken["Screensaver"]?["Figlet"]?["Minimum blue color level"]) : 0);
			int argresult264 = 0;
			FigletSettings.FigletMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Figlet"]?["Minimum color level"], out argresult264) ? (ConfigToken["Screensaver"]?["Figlet"]?["Minimum color level"]) : 0);
			int argresult265 = 0;
			FigletSettings.FigletMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Figlet"]?["Maximum red color level"], out argresult265) ? (ConfigToken["Screensaver"]?["Figlet"]?["Maximum red color level"]) : 255);
			int argresult266 = 0;
			FigletSettings.FigletMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Figlet"]?["Maximum green color level"], out argresult266) ? (ConfigToken["Screensaver"]?["Figlet"]?["Maximum green color level"]) : 255);
			int argresult267 = 0;
			FigletSettings.FigletMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Figlet"]?["Maximum blue color level"], out argresult267) ? (ConfigToken["Screensaver"]?["Figlet"]?["Maximum blue color level"]) : 255);
			int argresult268 = 0;
			FigletSettings.FigletMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Figlet"]?["Maximum color level"], out argresult268) ? (ConfigToken["Screensaver"]?["Figlet"]?["Maximum color level"]) : 255);

			// > FlashText
			FlashTextSettings.FlashText255Colors = (bool)((ConfigToken["Screensaver"]?["FlashText"]?["Activate 255 Color Mode"]) ?? false);
			FlashTextSettings.FlashTextTrueColor = (bool)((ConfigToken["Screensaver"]?["FlashText"]?["Activate True Color Mode"]) ?? true);
			int argresult269 = 0;
			FlashTextSettings.FlashTextDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashText"]?["Delay in Milliseconds"], out argresult269) ? (ConfigToken["Screensaver"]?["FlashText"]?["Delay in Milliseconds"]) : 10);
			FlashTextSettings.FlashTextWrite = (string)((ConfigToken["Screensaver"]?["FlashText"]?["Text Shown"]) ?? "Kernel Simulator");
			FlashTextSettings.FlashTextBackgroundColor = new Color(((ConfigToken["Screensaver"]?["FlashText"]?["Background color"]) ?? ConsoleColors.Black).ToString()).PlainSequence;
			int argresult270 = 0;
			FlashTextSettings.FlashTextMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashText"]?["Minimum red color level"], out argresult270) ? (ConfigToken["Screensaver"]?["FlashText"]?["Minimum red color level"]) : 0);
			int argresult271 = 0;
			FlashTextSettings.FlashTextMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashText"]?["Minimum green color level"], out argresult271) ? (ConfigToken["Screensaver"]?["FlashText"]?["Minimum green color level"]) : 0);
			int argresult272 = 0;
			FlashTextSettings.FlashTextMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashText"]?["Minimum blue color level"], out argresult272) ? (ConfigToken["Screensaver"]?["FlashText"]?["Minimum blue color level"]) : 0);
			int argresult273 = 0;
			FlashTextSettings.FlashTextMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashText"]?["Minimum color level"], out argresult273) ? (ConfigToken["Screensaver"]?["FlashText"]?["Minimum color level"]) : 0);
			int argresult274 = 0;
			FlashTextSettings.FlashTextMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashText"]?["Maximum red color level"], out argresult274) ? (ConfigToken["Screensaver"]?["FlashText"]?["Maximum red color level"]) : 255);
			int argresult275 = 0;
			FlashTextSettings.FlashTextMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashText"]?["Maximum green color level"], out argresult275) ? (ConfigToken["Screensaver"]?["FlashText"]?["Maximum green color level"]) : 255);
			int argresult276 = 0;
			FlashTextSettings.FlashTextMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashText"]?["Maximum blue color level"], out argresult276) ? (ConfigToken["Screensaver"]?["FlashText"]?["Maximum blue color level"]) : 255);
			int argresult277 = 0;
			FlashTextSettings.FlashTextMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["FlashText"]?["Maximum color level"], out argresult277) ? (ConfigToken["Screensaver"]?["FlashText"]?["Maximum color level"]) : 255);

			// > Noise
			NoiseSettings.NoiseNewScreenDelay = (int)((ConfigToken["Screensaver"]?["Noise"]?["New Screen Delay in Milliseconds"]) ?? 5000);
			NoiseSettings.NoiseDensity = (int)((ConfigToken["Screensaver"]?["Noise"]?["Noise density"]) ?? 40);

			// > PersonLookup
			int argresult278 = 0;
			PersonLookupSettings.PersonLookupDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["PersonLookup"]?["Delay in Milliseconds"], out argresult278) ? (ConfigToken["Screensaver"]?["PersonLookup"]?["Delay in Milliseconds"]) : 75);
			int argresult279 = 0;
			PersonLookupSettings.PersonLookupLookedUpDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["PersonLookup"]?["New Screen Delay in Milliseconds"], out argresult279) ? (ConfigToken["Screensaver"]?["PersonLookup"]?["New Screen Delay in Milliseconds"]) : 10000);
			int argresult280 = 0;
			PersonLookupSettings.PersonLookupMinimumNames = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["PersonLookup"]?["Minimum names count"], out argresult280) ? (ConfigToken["Screensaver"]?["PersonLookup"]?["Minimum names count"]) : 10);
			int argresult281 = 0;
			PersonLookupSettings.PersonLookupMaximumNames = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["PersonLookup"]?["Maximum names count"], out argresult281) ? (ConfigToken["Screensaver"]?["PersonLookup"]?["Maximum names count"]) : 1000);
			int argresult282 = 0;
			PersonLookupSettings.PersonLookupMinimumAgeYears = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["PersonLookup"]?["Minimum age years count"], out argresult282) ? (ConfigToken["Screensaver"]?["PersonLookup"]?["Minimum age years count"]) : 18);
			int argresult283 = 0;
			PersonLookupSettings.PersonLookupMaximumAgeYears = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["PersonLookup"]?["Maximum age years count"], out argresult283) ? (ConfigToken["Screensaver"]?["PersonLookup"]?["Maximum age years count"]) : 100);

			// > DateAndTime
			DateAndTimeSettings.DateAndTime255Colors = (bool)((ConfigToken["Screensaver"]?["DateAndTime"]?["Activate 255 Color Mode"]) ?? false);
			DateAndTimeSettings.DateAndTimeTrueColor = (bool)((ConfigToken["Screensaver"]?["DateAndTime"]?["Activate True Color Mode"]) ?? true);
			int argresult284 = 0;
			DateAndTimeSettings.DateAndTimeDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["DateAndTime"]?["Delay in Milliseconds"], out argresult284) ? (ConfigToken["Screensaver"]?["DateAndTime"]?["Delay in Milliseconds"]) : 1000);
			int argresult285 = 0;
			DateAndTimeSettings.DateAndTimeMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["DateAndTime"]?["Minimum red color level"], out argresult285) ? (ConfigToken["Screensaver"]?["DateAndTime"]?["Minimum red color level"]) : 0);
			int argresult286 = 0;
			DateAndTimeSettings.DateAndTimeMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["DateAndTime"]?["Minimum green color level"], out argresult286) ? (ConfigToken["Screensaver"]?["DateAndTime"]?["Minimum green color level"]) : 0);
			int argresult287 = 0;
			DateAndTimeSettings.DateAndTimeMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["DateAndTime"]?["Minimum blue color level"], out argresult287) ? (ConfigToken["Screensaver"]?["DateAndTime"]?["Minimum blue color level"]) : 0);
			int argresult288 = 0;
			DateAndTimeSettings.DateAndTimeMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["DateAndTime"]?["Minimum color level"], out argresult288) ? (ConfigToken["Screensaver"]?["DateAndTime"]?["Minimum color level"]) : 0);
			int argresult289 = 0;
			DateAndTimeSettings.DateAndTimeMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["DateAndTime"]?["Maximum red color level"], out argresult289) ? (ConfigToken["Screensaver"]?["DateAndTime"]?["Maximum red color level"]) : 255);
			int argresult290 = 0;
			DateAndTimeSettings.DateAndTimeMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["DateAndTime"]?["Maximum green color level"], out argresult290) ? (ConfigToken["Screensaver"]?["DateAndTime"]?["Maximum green color level"]) : 255);
			int argresult291 = 0;
			DateAndTimeSettings.DateAndTimeMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["DateAndTime"]?["Maximum blue color level"], out argresult291) ? (ConfigToken["Screensaver"]?["DateAndTime"]?["Maximum blue color level"]) : 255);
			int argresult292 = 0;
			DateAndTimeSettings.DateAndTimeMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["DateAndTime"]?["Maximum color level"], out argresult292) ? (ConfigToken["Screensaver"]?["DateAndTime"]?["Maximum color level"]) : 255);

			// > Glitch
			int argresult293 = 0;
			GlitchSettings.GlitchDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Glitch"]?["Delay in Milliseconds"], out argresult293) ? (ConfigToken["Screensaver"]?["Glitch"]?["Delay in Milliseconds"]) : 10);
			int argresult294 = 0;
			GlitchSettings.GlitchDensity = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Glitch"]?["Glitch density"], out argresult294) ? (ConfigToken["Screensaver"]?["Glitch"]?["Glitch density"]) : 40);

			// > Indeterminate
			IndeterminateSettings.Indeterminate255Colors = (bool)((ConfigToken["Screensaver"]?["Indeterminate"]?["Activate 255 Color Mode"]) ?? false);
			IndeterminateSettings.IndeterminateTrueColor = (bool)((ConfigToken["Screensaver"]?["Indeterminate"]?["Activate True Color Mode"]) ?? true);
			int argresult295 = 0;
			IndeterminateSettings.IndeterminateDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Indeterminate"]?["Delay in Milliseconds"], out argresult295) ? (ConfigToken["Screensaver"]?["Indeterminate"]?["Delay in Milliseconds"]) : 20);
			IndeterminateSettings.IndeterminateUpperLeftCornerChar = (string)((ConfigToken["Screensaver"]?["Indeterminate"]?["Upper left corner character for ramp bar"]) ?? "╔");
			IndeterminateSettings.IndeterminateUpperRightCornerChar = (string)((ConfigToken["Screensaver"]?["Indeterminate"]?["Upper right corner character for ramp bar"]) ?? "╗");
			IndeterminateSettings.IndeterminateLowerLeftCornerChar = (string)((ConfigToken["Screensaver"]?["Indeterminate"]?["Lower left corner character for ramp bar"]) ?? "╚");
			IndeterminateSettings.IndeterminateLowerRightCornerChar = (string)((ConfigToken["Screensaver"]?["Indeterminate"]?["Lower right corner character for ramp bar"]) ?? "╝");
			IndeterminateSettings.IndeterminateUpperFrameChar = (string)((ConfigToken["Screensaver"]?["Indeterminate"]?["Upper frame character for ramp bar"]) ?? "═");
			IndeterminateSettings.IndeterminateLowerFrameChar = (string)((ConfigToken["Screensaver"]?["Indeterminate"]?["Lower frame character for ramp bar"]) ?? "═");
			IndeterminateSettings.IndeterminateLeftFrameChar = (string)((ConfigToken["Screensaver"]?["Indeterminate"]?["Left frame character for ramp bar"]) ?? "║");
			IndeterminateSettings.IndeterminateRightFrameChar = (string)((ConfigToken["Screensaver"]?["Indeterminate"]?["Right frame character for ramp bar"]) ?? "║");
			int argresult296 = 0;
			IndeterminateSettings.IndeterminateMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Indeterminate"]?["Minimum red color level"], out argresult296) ? (ConfigToken["Screensaver"]?["Indeterminate"]?["Minimum red color level"]) : 0);
			int argresult297 = 0;
			IndeterminateSettings.IndeterminateMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Indeterminate"]?["Minimum green color level"], out argresult297) ? (ConfigToken["Screensaver"]?["Indeterminate"]?["Minimum green color level"]) : 0);
			int argresult298 = 0;
			IndeterminateSettings.IndeterminateMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Indeterminate"]?["Minimum blue color level"], out argresult298) ? (ConfigToken["Screensaver"]?["Indeterminate"]?["Minimum blue color level"]) : 0);
			int argresult299 = 0;
			IndeterminateSettings.IndeterminateMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Indeterminate"]?["Minimum color level"], out argresult299) ? (ConfigToken["Screensaver"]?["Indeterminate"]?["Minimum color level"]) : 0);
			int argresult300 = 0;
			IndeterminateSettings.IndeterminateMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Indeterminate"]?["Maximum red color level"], out argresult300) ? (ConfigToken["Screensaver"]?["Indeterminate"]?["Maximum red color level"]) : 255);
			int argresult301 = 0;
			IndeterminateSettings.IndeterminateMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Indeterminate"]?["Maximum green color level"], out argresult301) ? (ConfigToken["Screensaver"]?["Indeterminate"]?["Maximum green color level"]) : 255);
			int argresult302 = 0;
			IndeterminateSettings.IndeterminateMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Indeterminate"]?["Maximum blue color level"], out argresult302) ? (ConfigToken["Screensaver"]?["Indeterminate"]?["Maximum blue color level"]) : 255);
			int argresult303 = 0;
			IndeterminateSettings.IndeterminateMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Indeterminate"]?["Maximum color level"], out argresult303) ? (ConfigToken["Screensaver"]?["Indeterminate"]?["Maximum color level"]) : 255);
			IndeterminateSettings.IndeterminateUpperLeftCornerColor = new Color(((ConfigToken["Screensaver"]?["Indeterminate"]?["Upper left corner color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			IndeterminateSettings.IndeterminateUpperRightCornerColor = new Color(((ConfigToken["Screensaver"]?["Indeterminate"]?["Upper right corner color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			IndeterminateSettings.IndeterminateLowerLeftCornerColor = new Color(((ConfigToken["Screensaver"]?["Indeterminate"]?["Lower left corner color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			IndeterminateSettings.IndeterminateLowerRightCornerColor = new Color(((ConfigToken["Screensaver"]?["Indeterminate"]?["Lower right corner color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			IndeterminateSettings.IndeterminateUpperFrameColor = new Color(((ConfigToken["Screensaver"]?["Indeterminate"]?["Upper frame color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			IndeterminateSettings.IndeterminateLowerFrameColor = new Color(((ConfigToken["Screensaver"]?["Indeterminate"]?["Lower frame color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			IndeterminateSettings.IndeterminateLeftFrameColor = new Color(((ConfigToken["Screensaver"]?["Indeterminate"]?["Left frame color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			IndeterminateSettings.IndeterminateRightFrameColor = new Color(((ConfigToken["Screensaver"]?["Indeterminate"]?["Right frame color for ramp bar"]) ?? ConsoleColors.Gray).ToString()).PlainSequence;
			IndeterminateSettings.IndeterminateUseBorderColors = (bool)((ConfigToken["Screensaver"]?["Indeterminate"]?["Use border colors for ramp bar"]) ?? false);

			// > Pulse
			int argresult304 = 0;
			PulseSettings.PulseDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Pulse"]?["Delay in Milliseconds"], out argresult304) ? (ConfigToken["Screensaver"]?["Pulse"]?["Delay in Milliseconds"]) : 50);
			int argresult305 = 0;
			PulseSettings.PulseMaxSteps = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Pulse"]?["Max Fade Steps"], out argresult305) ? (ConfigToken["Screensaver"]?["Pulse"]?["Max Fade Steps"]) : 25);
			int argresult306 = 0;
			PulseSettings.PulseMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Pulse"]?["Minimum red color level"], out argresult306) ? (ConfigToken["Screensaver"]?["Pulse"]?["Minimum red color level"]) : 0);
			int argresult307 = 0;
			PulseSettings.PulseMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Pulse"]?["Minimum green color level"], out argresult307) ? (ConfigToken["Screensaver"]?["Pulse"]?["Minimum green color level"]) : 0);
			int argresult308 = 0;
			PulseSettings.PulseMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Pulse"]?["Minimum blue color level"], out argresult308) ? (ConfigToken["Screensaver"]?["Pulse"]?["Minimum blue color level"]) : 0);
			int argresult309 = 0;
			PulseSettings.PulseMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Pulse"]?["Maximum red color level"], out argresult309) ? (ConfigToken["Screensaver"]?["Pulse"]?["Maximum red color level"]) : 255);
			int argresult310 = 0;
			PulseSettings.PulseMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Pulse"]?["Maximum green color level"], out argresult310) ? (ConfigToken["Screensaver"]?["Pulse"]?["Maximum green color level"]) : 255);
			int argresult311 = 0;
			PulseSettings.PulseMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["Pulse"]?["Maximum blue color level"], out argresult311) ? (ConfigToken["Screensaver"]?["Pulse"]?["Maximum blue color level"]) : 255);

			// > BeatPulse
			BeatPulseSettings.BeatPulse255Colors = (bool)((ConfigToken["Screensaver"]?["BeatPulse"]?["Activate 255 Color Mode"]) ?? false);
			BeatPulseSettings.BeatPulseTrueColor = (bool)((ConfigToken["Screensaver"]?["BeatPulse"]?["Activate True Color Mode"]) ?? true);
			BeatPulseSettings.BeatPulseCycleColors = (bool)((ConfigToken["Screensaver"]?["BeatPulse"]?["Cycle Colors"]) ?? true);
			BeatPulseSettings.BeatPulseBeatColor = (string)((ConfigToken["Screensaver"]?["BeatPulse"]?["Beat Color"]) ?? 17);
			int argresult312 = 0;
			BeatPulseSettings.BeatPulseDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatPulse"]?["Delay in Beats Per Minute"], out argresult312) ? (ConfigToken["Screensaver"]?["BeatPulse"]?["Delay in Beats Per Minute"]) : 120);
			int argresult313 = 0;
			BeatPulseSettings.BeatPulseMaxSteps = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatPulse"]?["Max Fade Steps"], out argresult313) ? (ConfigToken["Screensaver"]?["BeatPulse"]?["Max Fade Steps"]) : 25);
			int argresult314 = 0;
			BeatPulseSettings.BeatPulseMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatPulse"]?["Minimum red color level"], out argresult314) ? (ConfigToken["Screensaver"]?["BeatPulse"]?["Minimum red color level"]) : 0);
			int argresult315 = 0;
			BeatPulseSettings.BeatPulseMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatPulse"]?["Minimum green color level"], out argresult315) ? (ConfigToken["Screensaver"]?["BeatPulse"]?["Minimum green color level"]) : 0);
			int argresult316 = 0;
			BeatPulseSettings.BeatPulseMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatPulse"]?["Minimum blue color level"], out argresult316) ? (ConfigToken["Screensaver"]?["BeatPulse"]?["Minimum blue color level"]) : 0);
			int argresult317 = 0;
			BeatPulseSettings.BeatPulseMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatPulse"]?["Minimum color level"], out argresult317) ? (ConfigToken["Screensaver"]?["BeatPulse"]?["Minimum color level"]) : 0);
			int argresult318 = 0;
			BeatPulseSettings.BeatPulseMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatPulse"]?["Maximum red color level"], out argresult318) ? (ConfigToken["Screensaver"]?["BeatPulse"]?["Maximum red color level"]) : 255);
			int argresult319 = 0;
			BeatPulseSettings.BeatPulseMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatPulse"]?["Maximum green color level"], out argresult319) ? (ConfigToken["Screensaver"]?["BeatPulse"]?["Maximum green color level"]) : 255);
			int argresult320 = 0;
			BeatPulseSettings.BeatPulseMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatPulse"]?["Maximum blue color level"], out argresult320) ? (ConfigToken["Screensaver"]?["BeatPulse"]?["Maximum blue color level"]) : 255);
			int argresult321 = 0;
			BeatPulseSettings.BeatPulseMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatPulse"]?["Maximum color level"], out argresult321) ? (ConfigToken["Screensaver"]?["BeatPulse"]?["Maximum color level"]) : 255);

			// > EdgePulse
			int argresult322 = 0;
			EdgePulseSettings.EdgePulseDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["EdgePulse"]?["Delay in Milliseconds"], out argresult322) ? (ConfigToken["Screensaver"]?["EdgePulse"]?["Delay in Milliseconds"]) : 50);
			int argresult323 = 0;
			EdgePulseSettings.EdgePulseMaxSteps = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["EdgePulse"]?["Max Fade Steps"], out argresult323) ? (ConfigToken["Screensaver"]?["EdgePulse"]?["Max Fade Steps"]) : 25);
			int argresult324 = 0;
			EdgePulseSettings.EdgePulseMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["EdgePulse"]?["Minimum red color level"], out argresult324) ? (ConfigToken["Screensaver"]?["EdgePulse"]?["Minimum red color level"]) : 0);
			int argresult325 = 0;
			EdgePulseSettings.EdgePulseMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["EdgePulse"]?["Minimum green color level"], out argresult325) ? (ConfigToken["Screensaver"]?["EdgePulse"]?["Minimum green color level"]) : 0);
			int argresult326 = 0;
			EdgePulseSettings.EdgePulseMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["EdgePulse"]?["Minimum blue color level"], out argresult326) ? (ConfigToken["Screensaver"]?["EdgePulse"]?["Minimum blue color level"]) : 0);
			int argresult327 = 0;
			EdgePulseSettings.EdgePulseMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["EdgePulse"]?["Maximum red color level"], out argresult327) ? (ConfigToken["Screensaver"]?["EdgePulse"]?["Maximum red color level"]) : 255);
			int argresult328 = 0;
			EdgePulseSettings.EdgePulseMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["EdgePulse"]?["Maximum green color level"], out argresult328) ? (ConfigToken["Screensaver"]?["EdgePulse"]?["Maximum green color level"]) : 255);
			int argresult329 = 0;
			EdgePulseSettings.EdgePulseMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["EdgePulse"]?["Maximum blue color level"], out argresult329) ? (ConfigToken["Screensaver"]?["EdgePulse"]?["Maximum blue color level"]) : 255);

			// > BeatEdgePulse
			BeatEdgePulseSettings.BeatEdgePulse255Colors = (bool)((ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Activate 255 Color Mode"]) ?? false);
			BeatEdgePulseSettings.BeatEdgePulseTrueColor = (bool)((ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Activate True Color Mode"]) ?? true);
			BeatEdgePulseSettings.BeatEdgePulseCycleColors = (bool)((ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Cycle Colors"]) ?? true);
			BeatEdgePulseSettings.BeatEdgePulseBeatColor = (string)((ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Beat Color"]) ?? 17);
			int argresult330 = 0;
			BeatEdgePulseSettings.BeatEdgePulseDelay = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Delay in Beats Per Minute"], out argresult330) ? (ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Delay in Beats Per Minute"]) : 120);
			int argresult331 = 0;
			BeatEdgePulseSettings.BeatEdgePulseMaxSteps = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Max Fade Steps"], out argresult331) ? (ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Max Fade Steps"]) : 25);
			int argresult332 = 0;
			BeatEdgePulseSettings.BeatEdgePulseMinimumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Minimum red color level"], out argresult332) ? (ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Minimum red color level"]) : 0);
			int argresult333 = 0;
			BeatEdgePulseSettings.BeatEdgePulseMinimumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Minimum green color level"], out argresult333) ? (ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Minimum green color level"]) : 0);
			int argresult334 = 0;
			BeatEdgePulseSettings.BeatEdgePulseMinimumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Minimum blue color level"], out argresult334) ? (ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Minimum blue color level"]) : 0);
			int argresult335 = 0;
			BeatEdgePulseSettings.BeatEdgePulseMinimumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Minimum color level"], out argresult335) ? (ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Minimum color level"]) : 0);
			int argresult336 = 0;
			BeatEdgePulseSettings.BeatEdgePulseMaximumRedColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Maximum red color level"], out argresult336) ? (ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Maximum red color level"]) : 255);
			int argresult337 = 0;
			BeatEdgePulseSettings.BeatEdgePulseMaximumGreenColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Maximum green color level"], out argresult337) ? (ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Maximum green color level"]) : 255);
			int argresult338 = 0;
			BeatEdgePulseSettings.BeatEdgePulseMaximumBlueColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Maximum blue color level"], out argresult338) ? (ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Maximum blue color level"]) : 255);
			int argresult339 = 0;
			BeatEdgePulseSettings.BeatEdgePulseMaximumColorLevel = (int)(int.TryParse((string)ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Maximum color level"], out argresult339) ? (ConfigToken["Screensaver"]?["BeatEdgePulse"]?["Maximum color level"]) : 255);

			// Splash Section - Splash-specific settings go below:
			// > Simple
			SplashSettings.SimpleProgressTextLocation = ConfigToken["Splash"]?["Simple"]?["Progress text location"] is not null ? Enum.TryParse((string)ConfigToken["Splash"]?["Simple"]?["Progress text location"], out SplashSettings.SimpleProgressTextLocation) ? SplashSettings.SimpleProgressTextLocation : TextLocation.Top : TextLocation.Top;

			// > Progress
			SplashSettings.ProgressProgressColor = new Color((ConfigToken["Splash"]?["Progress"]?["Progress bar color"].ToString()) ?? KernelColorTools.ProgressColor.PlainSequence).PlainSequence;
			SplashSettings.ProgressProgressTextLocation = ConfigToken["Splash"]?["Progress"]?["Progress text location"] is not null ? Enum.TryParse((string)ConfigToken["Splash"]?["Progress"]?["Progress text location"], out SplashSettings.ProgressProgressTextLocation) ? SplashSettings.ProgressProgressTextLocation : TextLocation.Top : TextLocation.Top;

			// Misc Section
			DebugWriter.Wdbg(DebugLevel.I, "Parsing misc section...");
			Flags.CornerTimeDate = (bool)((ConfigToken["Misc"]?["Show Time/Date on Upper Right Corner"]) ?? false);
			Flags.StartScroll = (bool)((ConfigToken["Misc"]?["Marquee on startup"]) ?? true);
			Flags.LongTimeDate = (bool)((ConfigToken["Misc"]?["Long Time and Date"]) ?? true);
			Forecast.Forecast.PreferredUnit = ConfigToken["Misc"]?["Preferred Unit for Temperature"] is not null ? Enum.TryParse((string)ConfigToken["Misc"]?["Preferred Unit for Temperature"], out Forecast.Forecast.PreferredUnit) ? Forecast.Forecast.PreferredUnit : UnitMeasurement.Metric : UnitMeasurement.Metric;
			TextEditShellCommon.TextEdit_AutoSaveFlag = (bool)((ConfigToken["Misc"]?["Enable text editor autosave"]) ?? true);
			int argresult340 = 0;
			TextEditShellCommon.TextEdit_AutoSaveInterval = (int)(int.TryParse((string)ConfigToken["Misc"]?["Text editor autosave interval"], out argresult340) ? (ConfigToken["Misc"]?["Text editor autosave interval"]) : 60);
			Flags.WrapListOutputs = (bool)((ConfigToken["Misc"]?["Wrap list outputs"]) ?? false);
			Flags.DrawBorderNotification = (bool)((ConfigToken["Misc"]?["Draw notification border"]) ?? false);
			ModManager.BlacklistedModsString = (string)((ConfigToken["Misc"]?["Blacklisted mods"]) ?? "");
			int argresult341 = 0;
			Solver.SolverMinimumNumber = (int)(int.TryParse((string)ConfigToken["Misc"]?["Solver minimum number"], out argresult341) ? (ConfigToken["Misc"]?["Solver minimum number"]) : 1000);
			int argresult342 = 0;
			Solver.SolverMaximumNumber = (int)(int.TryParse((string)ConfigToken["Misc"]?["Solver maximum number"], out argresult342) ? (ConfigToken["Misc"]?["Solver maximum number"]) : 1000);
			Solver.SolverShowInput = (bool)((ConfigToken["Misc"]?["Solver show input"]) ?? false);
			Notifications.Notifications.NotifyUpperLeftCornerChar = (string)((ConfigToken["Misc"]?["Upper left corner character for notification border"]) ?? "╔");
			Notifications.Notifications.NotifyUpperRightCornerChar = (string)((ConfigToken["Misc"]?["Upper right corner character for notification border"]) ?? "╗");
			Notifications.Notifications.NotifyLowerLeftCornerChar = (string)((ConfigToken["Misc"]?["Lower left corner character for notification border"]) ?? "╚");
			Notifications.Notifications.NotifyLowerRightCornerChar = (string)((ConfigToken["Misc"]?["Lower right corner character for notification border"]) ?? "╝");
			Notifications.Notifications.NotifyUpperFrameChar = (string)((ConfigToken["Misc"]?["Upper frame character for notification border"]) ?? "═");
			Notifications.Notifications.NotifyLowerFrameChar = (string)((ConfigToken["Misc"]?["Lower frame character for notification border"]) ?? "═");
			Notifications.Notifications.NotifyLeftFrameChar = (string)((ConfigToken["Misc"]?["Left frame character for notification border"]) ?? "║");
			Notifications.Notifications.NotifyRightFrameChar = (string)((ConfigToken["Misc"]?["Right frame character for notification border"]) ?? "║");
			PageViewer.ManpageInfoStyle = (string)((ConfigToken["Misc"]?["Manual page information style"]) ?? "");
			SpeedPress.SpeedPressCurrentDifficulty = ConfigToken["Misc"]?["Default difficulty for SpeedPress"] is not null ? Enum.TryParse((string)ConfigToken["Misc"]?["Default difficulty for SpeedPress"], out SpeedPress.SpeedPressCurrentDifficulty) ? SpeedPress.SpeedPressCurrentDifficulty : SpeedPress.SpeedPressDifficulty.Medium : SpeedPress.SpeedPressDifficulty.Medium;
			int argresult343 = 0;
			SpeedPress.SpeedPressTimeout = (int)(int.TryParse((string)ConfigToken["Misc"]?["Keypress timeout for SpeedPress"], out argresult343) ? (ConfigToken["Misc"]?["Keypress timeout for SpeedPress"]) : 3000);
			RSSTools.ShowHeadlineOnLogin = (bool)((ConfigToken["Misc"]?["Show latest RSS headline on login"]) ?? false);
			RSSTools.RssHeadlineUrl = (string)((ConfigToken["Misc"]?["RSS headline URL"]) ?? "https://www.techrepublic.com/rssfeeds/articles/");
			Flags.SaveEventsRemindersDestructively = (bool)((ConfigToken["Misc"]?["Save all events and/or reminders destructively"]) ?? false);
			JsonShellCommon.JsonShell_Formatting = ConfigToken["Misc"]?["Default JSON formatting for JSON shell"] is not null ? Enum.TryParse((string)ConfigToken["Misc"]?["Default JSON formatting for JSON shell"], out JsonShellCommon.JsonShell_Formatting) ? JsonShellCommon.JsonShell_Formatting : Formatting.Indented : Formatting.Indented;
			Flags.EnableFigletTimer = (bool)((ConfigToken["Misc"]?["Enable Figlet for timer"]) ?? false);
			TimerScreen.TimerFigletFont = (string)((ConfigToken["Misc"]?["Figlet font for timer"]) ?? "Small");
			Flags.ShowCommandsCount = (bool)((ConfigToken["Misc"]?["Show the commands count on help"]) ?? false);
			Flags.ShowShellCommandsCount = (bool)((ConfigToken["Misc"]?["Show the shell commands count on help"]) ?? true);
			Flags.ShowModCommandsCount = (bool)((ConfigToken["Misc"]?["Show the mod commands count on help"]) ?? true);
			Flags.ShowShellAliasesCount = (bool)((ConfigToken["Misc"]?["Show the aliases count on help"]) ?? true);
			Input.CurrentMask = (string)((ConfigToken["Misc"]?["Password mask character"]) ?? '*');
			ProgressTools.ProgressUpperLeftCornerChar = (string)((ConfigToken["Misc"]?["Upper left corner character for progress bars"]) ?? "╔");
			ProgressTools.ProgressUpperRightCornerChar = (string)((ConfigToken["Misc"]?["Upper right corner character for progress bars"]) ?? "╗");
			ProgressTools.ProgressLowerLeftCornerChar = (string)((ConfigToken["Misc"]?["Lower left corner character for progress bars"]) ?? "╚");
			ProgressTools.ProgressLowerRightCornerChar = (string)((ConfigToken["Misc"]?["Lower right corner character for progress bars"]) ?? "╝");
			ProgressTools.ProgressUpperFrameChar = (string)((ConfigToken["Misc"]?["Upper frame character for progress bars"]) ?? "═");
			ProgressTools.ProgressLowerFrameChar = (string)((ConfigToken["Misc"]?["Lower frame character for progress bars"]) ?? "═");
			ProgressTools.ProgressLeftFrameChar = (string)((ConfigToken["Misc"]?["Left frame character for progress bars"]) ?? "║");
			ProgressTools.ProgressRightFrameChar = (string)((ConfigToken["Misc"]?["Right frame character for progress bars"]) ?? "║");
			int argresult344 = 0;
			LoveHateRespond.LoveOrHateUsersCount = (int)(int.TryParse((string)ConfigToken["Misc"]?["Users count for love or hate comments"], out argresult344) ? (ConfigToken["Misc"]?["Users count for love or hate comments"]) : 20);
			Flags.InputHistoryEnabled = (bool)((ConfigToken["Misc"]?["Input history enabled"]) ?? true);
			MeteorShooter.MeteorUsePowerLine = (bool)((ConfigToken["Misc"]?["Use PowerLine for rendering spaceship"]) ?? true);
			int argresult345 = 0;
			MeteorShooter.MeteorSpeed = (int)(int.TryParse((string)ConfigToken["Misc"]?["Meteor game speed"], out argresult345) ? (ConfigToken["Misc"]?["Meteor game speed"]) : 10);

			// Check to see if the config needs fixes
			ConfigTools.RepairConfig();

			// Raise event
			Kernel.Kernel.KernelEventManager.RaiseConfigRead();
		}

		/// <summary>
        /// Configures the kernel according to the custom kernel configuration file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="Exceptions.ConfigException"></exception>
		public static bool TryReadConfig(string ConfigPath)
		{
			try
			{
				ReadConfig(ConfigPath);
				return true;
			}
			catch (NullReferenceException nre)
			{
				// Rare, but repair config if an NRE is caught.
				DebugWriter.Wdbg(DebugLevel.E, "Error trying to read config: {0}", nre.Message);
				ConfigTools.RepairConfig();
			}
			catch (Exception ex)
			{
				Kernel.Kernel.KernelEventManager.RaiseConfigReadError(ex);
				DebugWriter.WStkTrc(ex);
				if (!SplashReport.KernelBooted)
				{
					Notifications.Notifications.NotifySend(new Notification(Translate.DoTranslation("Error loading settings"), Translate.DoTranslation("There is an error while loading settings. You may need to check the settings file."), Notifications.Notifications.NotifPriority.Medium, Notifications.Notifications.NotifType.Normal));
				}
				DebugWriter.Wdbg(DebugLevel.E, "Error trying to read config: {0}", ex.Message);
				throw new Kernel.Exceptions.ConfigException(Translate.DoTranslation("There is an error trying to read configuration: {0}."), ex, ex.Message);
			}
			return false;
		}

		/// <summary>
        /// Main loader for configuration file
        /// </summary>
		public static void InitializeConfig()
		{
			// Make a config file if not found
			if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.Configuration)))
			{
				DebugWriter.Wdbg(DebugLevel.E, "No config file found. Creating...");
				CreateConfig();
			}

			// Load and read config
			try
			{
				TryReadConfig();
			}
			catch (Kernel.Exceptions.ConfigException cex)
			{
				TextWriterColor.Write(cex.Message, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
				DebugWriter.WStkTrc(cex);
			}
		}

		/// <summary>
        /// Initializes the config token
        /// </summary>
		public static void InitializeConfigToken()
		{
			InitializeConfigToken(Paths.GetKernelPath(KernelPathType.Configuration));
		}

		/// <summary>
        /// Initializes the config token
        /// </summary>
		public static void InitializeConfigToken(string ConfigPath)
		{
			Filesystem.ThrowOnInvalidPath(ConfigPath);
			ConfigToken = JObject.Parse(File.ReadAllText(ConfigPath));
		}

	}
}