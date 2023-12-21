using System;
using System.Collections.Generic;
using System.Diagnostics;

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
using System.Linq;
using System.Threading;
using KS.Arguments;
using KS.Arguments.ArgumentBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Files.Querying;
using KS.Hardware;
using KS.Languages;
using KS.Login;
using KS.Misc.Calendar.Events;
using KS.Misc.Calendar.Reminders;
using KS.Misc.Configuration;
using KS.Misc.Notifications;
using KS.Misc.Reflection;
using KS.Misc.Splash;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Misc.Writers.MiscWriters;
using KS.Modifications;
using KS.Network.Mail;
using KS.Network.RemoteDebug;
using KS.Network.RPC;
using KS.Network.Transfer;
using KS.Scripting;
using KS.Shell.ShellBase;
using KS.Shell.ShellBase.Aliases;
using KS.TimeDate;
using Newtonsoft.Json.Linq;
using Terminaux.Base;
using TermExts = Terminaux.Base.ConsoleExtensions;

namespace KS.Kernel
{
	public static class KernelTools
	{

		// Variables
		public static string BannerFigletFont = "Banner";
		internal static KernelThread RPCPowerListener = new("RPC Power Listener Thread", true, (powerMode) => PowerManage((PowerMode)powerMode));
		internal static Exception LastKernelErrorException;

		// ----------------------------------------------- Kernel errors -----------------------------------------------

		/// <summary>
		/// Indicates that there's something wrong with the kernel.
		/// </summary>
		/// <param name="ErrorType">Specifies the error type.</param>
		/// <param name="Reboot">Specifies whether to reboot on panic or to show the message to press any key to shut down</param>
		/// <param name="RebootTime">Specifies seconds before reboot. 0 is instant. Negative numbers are not allowed.</param>
		/// <param name="Description">Explanation of what happened when it errored.</param>
		/// <param name="Exc">An exception to get stack traces, etc. Used for dump files currently.</param>
		/// <param name="Variables">Optional. Specifies variables to get on text that will be printed.</param>
		public static void KernelError(KernelErrorLevel ErrorType, bool Reboot, long RebootTime, string Description, Exception Exc, params object[] Variables)
		{
			Flags.KernelErrored = true;
			LastKernelErrorException = Exc;
			Flags.NotifyKernelError = true;

			try
			{
				// Unquiet
				Flags.QuietKernel = false;

				// Check error types and its capabilities
				DebugWriter.Wdbg(DebugLevel.I, "Error type: {0}", ErrorType);
				if (Enum.IsDefined(typeof(KernelErrorLevel), ErrorType))
				{
					if (ErrorType == KernelErrorLevel.U | ErrorType == KernelErrorLevel.D)
					{
						if (RebootTime > 5L)
						{
							// If the error type is unrecoverable, or double, and the reboot time exceeds 5 seconds, then
							// generate a second kernel error stating that there is something wrong with the reboot time.
							DebugWriter.Wdbg(DebugLevel.W, "Errors that have type {0} shouldn't exceed 5 seconds. RebootTime was {1} seconds", ErrorType, RebootTime);
							KernelError(KernelErrorLevel.D, true, 5L, Translate.DoTranslation("DOUBLE PANIC: Reboot Time exceeds maximum allowed {0} error reboot time. You found a kernel bug."), null, ((int)ErrorType).ToString());
							return;
						}
						else if (!Reboot)
						{
							// If the error type is unrecoverable, or double, and the rebooting is false where it should
							// not be false, then it can deal with this issue by enabling reboot.
							DebugWriter.Wdbg(DebugLevel.W, "Errors that have type {0} enforced Reboot = True.", ErrorType);
							TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: Reboot enabled due to error level being {0}."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Uncontinuable), ErrorType);
							Reboot = true;
						}
					}
					if (RebootTime > 3600L)
					{
						// If the reboot time exceeds 1 hour, then it will set the time to 1 minute.
						DebugWriter.Wdbg(DebugLevel.W, "RebootTime shouldn't exceed 1 hour. Was {0} seconds", RebootTime);
						TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: Time to reboot: {1} seconds, exceeds 1 hour. It is set to 1 minute."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Uncontinuable), ErrorType, RebootTime.ToString());
						RebootTime = 60L;
					}
				}
				else
				{
					// If the error type is other than D/F/C/U/S, then it will generate a second error.
					DebugWriter.Wdbg(DebugLevel.E, "Error type {0} is not valid.", ErrorType);
					KernelError(KernelErrorLevel.D, true, 5L, Translate.DoTranslation("DOUBLE PANIC: Error Type {0} invalid."), null, ((int)ErrorType).ToString());
					return;
				}

				// Format the "Description" string variable
				Description = StringManipulate.FormatString(Description, Variables);

				// Fire an event
				Kernel.KernelEventManager.RaiseKernelError(ErrorType, Reboot, RebootTime, Description, Exc, Variables);

				// Make a dump file
				GeneratePanicDump(Description, ErrorType, Exc);

				// Check error type
				switch (ErrorType)
				{
					case KernelErrorLevel.D:
						{
							// Double panic printed and reboot initiated
							DebugWriter.Wdbg(DebugLevel.F, "Double panic caused by bug in kernel crash.");
							TextWriterColor.Write(Translate.DoTranslation("[{0}] dpanic: {1} -- Rebooting in {2} seconds..."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Uncontinuable), ErrorType, Description, RebootTime.ToString());
							Thread.Sleep((int)(RebootTime * 1000L));
							DebugWriter.Wdbg(DebugLevel.F, "Rebooting");
							PowerManage(PowerMode.Reboot);
							break;
						}
					case KernelErrorLevel.C:
						{
							if (Reboot)
							{
								// Continuable kernel errors shouldn't cause the kernel to reboot.
								DebugWriter.Wdbg(DebugLevel.W, "Continuable kernel errors shouldn't have Reboot = True.");
								TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: Reboot disabled due to error level being {0}."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Warning), ErrorType);
							}
							// Print normally
							Kernel.KernelEventManager.RaiseContKernelError(ErrorType, Reboot, RebootTime, Description, Exc, Variables);
							TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: {1} -- Press any key to continue using the kernel."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Continuable), ErrorType, Description);
							if (Flags.ShowStackTraceOnKernelError & Exc is not null)
								TextWriterColor.Write(Exc.StackTrace, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Continuable));
							Input.DetectKeypress();
							break;
						}

					default:
						{
							if (Reboot)
							{
								// Offer the user to wait for the set time interval before the kernel reboots.
								DebugWriter.Wdbg(DebugLevel.F, "Kernel panic initiated with reboot time: {0} seconds, Error Type: {1}", RebootTime, ErrorType);
								TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: {1} -- Rebooting in {2} seconds..."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Uncontinuable), ErrorType, Description, RebootTime.ToString());
								if (Flags.ShowStackTraceOnKernelError & Exc is not null)
									TextWriterColor.Write(Exc.StackTrace, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Uncontinuable));
								Thread.Sleep((int)(RebootTime * 1000L));
								PowerManage(PowerMode.Reboot);
							}
							else
							{
								// If rebooting is disabled, offer the user to shutdown the kernel
								DebugWriter.Wdbg(DebugLevel.W, "Reboot is False, ErrorType is not double or continuable.");
								TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: {1} -- Press any key to shutdown."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Uncontinuable), ErrorType, Description);
								if (Flags.ShowStackTraceOnKernelError & Exc is not null)
									TextWriterColor.Write(Exc.StackTrace, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Uncontinuable));
								Input.DetectKeypress();
								PowerManage(PowerMode.Shutdown);
							}

							break;
						}
				}
			}
			catch (Exception ex)
			{
				// Check to see if it's a double panic
				if (ErrorType == KernelErrorLevel.D)
				{
					// Trigger triple fault
					DebugWriter.Wdbg(DebugLevel.F, "TRIPLE FAULT: Kernel bug: {0}", ex.Message);
					DebugWriter.WStkTrc(ex);
					Environment.FailFast("TRIPLE FAULT in trying to handle DOUBLE PANIC. KS can't continue.", ex);
				}
				else
				{
					// Alright, we have a double panic.
					DebugWriter.Wdbg(DebugLevel.F, "DOUBLE PANIC: Kernel bug: {0}", ex.Message);
					DebugWriter.WStkTrc(ex);
					KernelError(KernelErrorLevel.D, true, 5L, Translate.DoTranslation("DOUBLE PANIC: Kernel bug: {0}"), ex, ex.Message);
				}
			}
		}

		/// <summary>
		/// Generates the stack trace dump file for kernel panics
		/// </summary>
		/// <param name="Description">Error description</param>
		/// <param name="ErrorType">Error type</param>
		/// <param name="Exc">Exception</param>
		public static void GeneratePanicDump(string Description, KernelErrorLevel ErrorType, Exception Exc)
		{
			try
			{
				// Open a file stream for dump
				var Dump = new StreamWriter($"{Paths.HomePath}/dmp_{TimeDateRenderers.RenderDate(TimeDate.TimeDate.FormatType.Short).Replace("/", "-")}_{TimeDateRenderers.RenderTime(TimeDate.TimeDate.FormatType.Long).Replace(":", "-")}.txt");
				DebugWriter.Wdbg(DebugLevel.I, "Opened file stream in home directory, saved as dmp_{0}.txt", $"{TimeDateRenderers.RenderDate(TimeDate.TimeDate.FormatType.Short).Replace("/", "-")}_{TimeDateRenderers.RenderTime(TimeDate.TimeDate.FormatType.Long).Replace(":", "-")}");

				// Write info (Header)
				Dump.AutoFlush = true;
				Dump.WriteLine(Translate.DoTranslation("----------------------------- Kernel panic dump -----------------------------") + Kernel.NewLine + Kernel.NewLine + Translate.DoTranslation(">> Panic information <<") + Kernel.NewLine + Translate.DoTranslation("> Description: {0}") + Kernel.NewLine + Translate.DoTranslation("> Error type: {1}") + Kernel.NewLine + Translate.DoTranslation("> Date and Time: {2}") + Kernel.NewLine + Translate.DoTranslation("> Framework Type: {3}") + Kernel.NewLine, Description, ErrorType.ToString(), TimeDateRenderers.Render(), Kernel.KernelSimulatorMoniker);

				// Write Info (Exception)
				if (Exc is not null)
				{
					int Count = 1;
					Dump.WriteLine(Translate.DoTranslation(">> Exception information <<") + Kernel.NewLine + Translate.DoTranslation("> Exception: {0}") + Kernel.NewLine + Translate.DoTranslation("> Description: {1}") + Kernel.NewLine + Translate.DoTranslation("> HRESULT: {2}") + Kernel.NewLine + Translate.DoTranslation("> Source: {3}") + Kernel.NewLine + Kernel.NewLine + Translate.DoTranslation("> Stack trace <") + Kernel.NewLine + Kernel.NewLine + Exc.StackTrace + Kernel.NewLine + Kernel.NewLine, Exc.GetType().FullName, Exc.Message, Exc.HResult, Exc.Source);
					Dump.WriteLine(Translate.DoTranslation(">> Inner exception {0} information <<"), Count);

					// Write info (Inner exceptions)
					var InnerExc = Exc.InnerException;
					while (InnerExc is not null)
					{
						Dump.WriteLine(Translate.DoTranslation("> Exception: {0}") + Kernel.NewLine + Translate.DoTranslation("> Description: {1}") + Kernel.NewLine + Translate.DoTranslation("> HRESULT: {2}") + Kernel.NewLine + Translate.DoTranslation("> Source: {3}") + Kernel.NewLine + Kernel.NewLine + Translate.DoTranslation("> Stack trace <") + Kernel.NewLine + Kernel.NewLine + InnerExc.StackTrace + Kernel.NewLine, InnerExc.GetType().FullName, InnerExc.Message, InnerExc.HResult, InnerExc.Source);
						InnerExc = InnerExc.InnerException;
						if (InnerExc is not null)
						{
							Dump.WriteLine(Translate.DoTranslation(">> Inner exception {0} information <<"), Count);
						}
						else
						{
							Dump.WriteLine(Translate.DoTranslation(">> Exception {0} is the root cause <<"), Count - 1);
						}
						Count += 1;
					}
					Dump.WriteLine();
				}
				else
				{
					Dump.WriteLine(Translate.DoTranslation(">> No exception; might be a kernel error. <<") + Kernel.NewLine);
				}

				// Write info (Frames)
				Dump.WriteLine(Translate.DoTranslation(">> Frames, files, lines, and columns <<"));
				try
				{
					var ExcTrace = new StackTrace(Exc, true);
					int FrameNo = 1;

					// If there are frames to print the file information, write them down to the dump file.
					if (ExcTrace.FrameCount != 0)
					{
						foreach (StackFrame Frame in ExcTrace.GetFrames())
						{
							if (!(string.IsNullOrEmpty(Frame.GetFileName()) & Frame.GetFileLineNumber() == 0 & Frame.GetFileColumnNumber() == 0))
							{
								Dump.WriteLine(Translate.DoTranslation("> Frame {0}: File: {1} | Line: {2} | Column: {3}"), FrameNo, Frame.GetFileName(), Frame.GetFileLineNumber(), Frame.GetFileColumnNumber());
							}
							FrameNo += 1;
						}
					}
					else
					{
						Dump.WriteLine(Translate.DoTranslation("> There are no information about frames."));
					}
				}
				catch (Exception ex)
				{
					DebugWriter.WStkTrc(ex);
					Dump.WriteLine(Translate.DoTranslation("> There is an error when trying to get frame information. {0}: {1}"), ex.GetType().FullName, ex.Message.Replace(Kernel.NewLine, " | "));
				}

				// Close stream
				DebugWriter.Wdbg(DebugLevel.I, "Closing file stream for dump...");
				Dump.Flush();
				Dump.Close();
			}
			catch (Exception ex)
			{
				TextWriterColor.Write(Translate.DoTranslation("Dump information gatherer crashed when trying to get information about {0}: {1}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), Exc.GetType().FullName, ex.Message);
				DebugWriter.WStkTrc(ex);
			}
		}

		// ----------------------------------------------- Power management -----------------------------------------------

		/// <summary>
		/// Manage computer's (actually, simulated computer) power
		/// </summary>
		/// <param name="PowerMode">Selects the power mode</param>
		public static void PowerManage(PowerMode PowerMode)
		{
			PowerManage(PowerMode, "0.0.0.0", RemoteProcedure.RPCPort);
		}

		/// <summary>
		/// Manage computer's (actually, simulated computer) power
		/// </summary>
		/// <param name="PowerMode">Selects the power mode</param>
		public static void PowerManage(PowerMode PowerMode, string IP)
		{
			PowerManage(PowerMode, IP, RemoteProcedure.RPCPort);
		}

		/// <summary>
		/// Manage computer's (actually, simulated computer) power
		/// </summary>
		/// <param name="PowerMode">Selects the power mode</param>
		public static void PowerManage(PowerMode PowerMode, string IP, int Port)
		{
			DebugWriter.Wdbg(DebugLevel.I, "Power management has the argument of {0}", PowerMode);
			switch (PowerMode)
			{
				case PowerMode.Shutdown:
					{
						Kernel.KernelEventManager.RaisePreShutdown();
						TextWriterColor.Write(Translate.DoTranslation("Shutting down..."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
						Flags.RebootRequested = true;
						Flags.LogoutRequested = true;
						Flags.KernelShutdown = true;
						ResetEverything();
						Kernel.KernelEventManager.RaisePostShutdown();
						break;
					}
				case PowerMode.Reboot:
				case PowerMode.RebootSafe:
					{
						Kernel.KernelEventManager.RaisePreReboot();
						TextWriterColor.Write(Translate.DoTranslation("Rebooting..."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
						Flags.RebootRequested = true;
						Flags.LogoutRequested = true;
						ResetEverything();
						Kernel.KernelEventManager.RaisePostReboot();
						ConsoleWrapper.Clear();
						break;
					}
				case PowerMode.RemoteShutdown:
					{
						RPCCommands.SendCommand("<Request:Shutdown>(" + IP + ")", IP, Port);
						break;
					}
				case PowerMode.RemoteRestart:
					{
						RPCCommands.SendCommand("<Request:Reboot>(" + IP + ")", IP, Port);
						break;
					}
				case PowerMode.RemoteRestartSafe:
					{
						RPCCommands.SendCommand("<Request:RebootSafe>(" + IP + ")", IP, Port);
						break;
					}
			}
			Flags.SafeMode = PowerMode == PowerMode.RebootSafe;
		}

		// ----------------------------------------------- Init and reset -----------------------------------------------
		/// <summary>
		/// Reset everything for the next restart
		/// </summary>
		public static void ResetEverything()
		{
			// Reset every variable below
			if (Flags.ArgsInjected == false)
				ArgumentPrompt.EnteredArguments.Clear();
			PermissionManagement.UserPermissions.Clear();
			ReminderManager.Reminders.Clear();
			EventManager.CalendarEvents.Clear();
			Flags.ArgsOnBoot = false;
			Flags.SafeMode = false;
			Flags.QuietKernel = false;
			Flags.Maintenance = false;
			SplashReport._Progress = 0;
			SplashReport._ProgressText = "";
			SplashReport._KernelBooted = false;
			DebugWriter.Wdbg(DebugLevel.I, "General variables reset");

			// Reset hardware info
			HardwareProbe.HardwareInfo = null;
			DebugWriter.Wdbg(DebugLevel.I, "Hardware info reset.");

			// Disconnect all hosts from remote debugger
			RemoteDebugger.StopRDebugThread();
			DebugWriter.Wdbg(DebugLevel.I, "Remote debugger stopped");

			// Stop all mods
			ModManager.StopMods();
			DebugWriter.Wdbg(DebugLevel.I, "Mods stopped");

			// Disable Debugger
			if (Flags.DebugMode)
			{
				DebugWriter.Wdbg(DebugLevel.I, "Shutting down debugger");
				Flags.DebugMode = false;
				DebugWriter.DebugStreamWriter.Close();
				DebugWriter.DebugStreamWriter.Dispose();
			}

			// Stop RPC
			RemoteProcedure.StopRPC();

			// Disconnect from mail
			MailLogin.IMAP_Client.Disconnect(true);
			MailLogin.SMTP_Client.Disconnect(true);

			// Unload all splashes
			SplashManager.UnloadSplashes();

			// Disable safe mode
			Flags.SafeMode = false;

			// Stop the time/date change thread
			TimeDate.TimeDate.TimeDateChange.Stop();
		}

		/// <summary>
		/// Initializes everything
		/// </summary>
		public static void InitEverything(string[] Args)
		{
			// Initialize notifications
			if (!Notifications.NotifThread.IsAlive)
				Notifications.NotifThread.Start();

			// Initialize events and reminders
			if (!ReminderManager.ReminderThread.IsAlive)
				ReminderManager.ReminderThread.Start();
			if (!EventManager.EventThread.IsAlive)
				EventManager.EventThread.Start();

			// Install cancellation handler
			if (!Flags.CancellationHandlerInstalled)
				Console.CancelKeyPress += CancellationHandlers.CancelCommand;

			// Initialize resize listener
			ConsoleResizeListener.StartResizeListener();

			// Initialize aliases
			AliasManager.InitAliases();

			// Initialize date
			TimeDate.TimeDate.InitTimeDate();

			// Initialize custom languages
			LanguageManager.InstallCustomLanguages();

			// Initialize splashes
			SplashManager.LoadSplashes();

			// Create config file and then read it
			Config.InitializeConfig();

			// Load user token
			UserManagement.LoadUserToken();

			// Show welcome message.
			WelcomeMessage.WriteMessage();

			// Some information
			if (Flags.ShowAppInfoOnBoot & !Flags.EnableSplash)
			{
				SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("App information"), true, KernelColorTools.ColTypes.Stage);
				TextWriterColor.Write("OS: " + Translate.DoTranslation("Running on {0}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), Environment.OSVersion.ToString());
			}

			// Show dev version notice
			if (!Flags.EnableSplash)
			{
#if SPECIFIERDEV
				TextWriterColor.Write(Translate.DoTranslation("Looks like you were running the development version of the kernel. While you can see the aspects, it is frequently updated and might introduce bugs. It is recommended that you stay on the stable version."), true, KernelColorTools.ColTypes.DevelopmentWarning);
#elif SPECIFIERRC
				TextWriterColor.Write(DoTranslation("Looks like you were running the release candidate version. It is recommended that you stay on the stable version."), true, KernelColorTools.ColTypes.DevelopmentWarning);
#elif !SPECIFIERREL
				TextWriterColor.Write(DoTranslation("Looks like you were running an unsupported version. It's highly advisable not to use this version."), true, KernelColorTools.ColTypes.DevelopmentWarning);
#endif
			}

			// Parse real command-line arguments
			if (Flags.ParseCommandLineArguments)
				ArgumentParse.ParseArguments([.. Args], ArgumentType.CommandLineArgs);

			// Check arguments
			if (Flags.ArgsOnBoot)
			{
				Kernel.StageTimer.Stop();
				ArgumentPrompt.PromptArgs();
				Kernel.StageTimer.Start();
			}
			if (Flags.ArgsInjected)
			{
				Flags.ArgsInjected = false;
				ArgumentParse.ParseArguments(ArgumentPrompt.EnteredArguments, ArgumentType.KernelArgs);
			}

			// Load splash
			SplashManager.OpenSplash();

			// Write headers for debug
			DebugWriter.Wdbg(DebugLevel.I, "-------------------------------------------------------------------");
			DebugWriter.Wdbg(DebugLevel.I, "Kernel initialized, version {0}.", Kernel.KernelVersion);
			DebugWriter.Wdbg(DebugLevel.I, "OS: {0}", Environment.OSVersion.ToString());
			DebugWriter.Wdbg(DebugLevel.I, "Framework: {0}", Kernel.KernelSimulatorMoniker);

			// Populate ban list for debug devices
			RemoteDebugTools.PopulateBlockedDevices();

			// Load all events and reminders
			EventManager.LoadEvents();
			ReminderManager.LoadReminders();

			// Load system env vars and convert them
			UESHVariables.ConvertSystemEnvironmentVariables();
		}

		/// <summary>
		/// Fetches the GitHub repo to see if there are any updates
		/// </summary>
		/// <returns>A kernel update instance</returns>
		public static KernelUpdate FetchKernelUpdates()
		{
			try
			{
				// Because api.github.com requires the UserAgent header to be put, else, 403 error occurs. Fortunately for us, "Aptivi" is enough.
				NetworkTransfer.WClient.DefaultRequestHeaders.Add("User-Agent", "Aptivi");

				// Populate the following variables with information
				string UpdateStr = NetworkTransfer.DownloadString("https://api.github.com/repos/Aptivi/NitrocidKS/releases");
				var UpdateToken = JToken.Parse(UpdateStr);
				var UpdateInstance = new KernelUpdate(UpdateToken);

				// Return the update instance
				NetworkTransfer.WClient.DefaultRequestHeaders.Remove("User-Agent");
				return UpdateInstance;
			}
			catch (Exception ex)
			{
				DebugWriter.Wdbg(DebugLevel.E, "Failed to check for updates: {0}", ex.Message);
				DebugWriter.WStkTrc(ex);
			}
			return null;
		}

		/// <summary>
		/// Prompt for checking for kernel updates
		/// </summary>
		public static void CheckKernelUpdates()
		{
			SplashReport.ReportProgress(Translate.DoTranslation("Checking for system updates..."), 10, KernelColorTools.ColTypes.Neutral);
			var AvailableUpdate = FetchKernelUpdates();
			if (AvailableUpdate is not null)
			{
				if (!AvailableUpdate.Updated)
				{
					SplashReport.ReportProgress(Translate.DoTranslation("Found new version: "), 10, KernelColorTools.ColTypes.ListEntry);
					SplashReport.ReportProgress(AvailableUpdate.UpdateVersion.ToString(), 10, KernelColorTools.ColTypes.ListValue);
					if (Flags.AutoDownloadUpdate)
					{
						NetworkTransfer.DownloadFile(AvailableUpdate.UpdateURL.ToString(), Path.Combine(Environment.CurrentDirectory, "update.zip"));
						SplashReport.ReportProgress(Translate.DoTranslation("Downloaded the update successfully!"), 10, KernelColorTools.ColTypes.Success);
					}
					else
					{
						SplashReport.ReportProgress(Translate.DoTranslation("You can download it at: "), 10, KernelColorTools.ColTypes.ListEntry);
						SplashReport.ReportProgress(AvailableUpdate.UpdateURL.ToString(), 10, KernelColorTools.ColTypes.ListValue);
					}
				}
				else
				{
					SplashReport.ReportProgress(Translate.DoTranslation("You're up to date!"), 10, KernelColorTools.ColTypes.Neutral);
				}
			}
			else if (AvailableUpdate is null)
			{
				SplashReport.ReportProgress(Translate.DoTranslation("Failed to check for updates."), 10, KernelColorTools.ColTypes.Error);
			}
		}

		/// <summary>
		/// Removes all configuration files
		/// </summary>
		public static void FactoryReset()
		{
			// Delete every single thing found in KernelPaths
			foreach (string PathName in Paths.KernelPaths.Keys)
			{
				if (Checking.FileExists(Paths.KernelPaths[PathName]))
				{
					File.Delete(Paths.KernelPaths[PathName]);
				}
				else
				{
					Directory.Delete(Paths.KernelPaths[PathName], true);
				}
			}

			// Clear the console and reset the colors
			TermExts.ResetColors();
			ConsoleWrapper.Clear();
			Environment.Exit(0);
		}

		/// <summary>
		/// Reports the new kernel stage
		/// </summary>
		/// <param name="StageNumber">The stage number</param>
		/// <param name="StageText">The stage text</param>
		public static void ReportNewStage(int StageNumber, string StageText)
		{
			// Show the stage finish times
			if (StageNumber <= 1)
			{
				if (Flags.ShowStageFinishTimes)
				{
					SplashReport.ReportProgress(Translate.DoTranslation("Internal initialization finished in") + $" {Kernel.StageTimer.Elapsed}", 0, KernelColorTools.ColTypes.StageTime);
					Kernel.StageTimer.Restart();
				}
			}
			else if (StageNumber >= 5)
			{
				if (Flags.ShowStageFinishTimes)
				{
					SplashReport.ReportProgress(Translate.DoTranslation("Stage finished in") + $" {Kernel.StageTimer.Elapsed}", 10, KernelColorTools.ColTypes.StageTime);
					Kernel.StageTimer.Reset();
					TextWriterColor.WritePlain("", true);
				}
			}
			else if (Flags.ShowStageFinishTimes)
			{
				SplashReport.ReportProgress(Translate.DoTranslation("Stage finished in") + $" {Kernel.StageTimer.Elapsed}", 10, KernelColorTools.ColTypes.StageTime);
				Kernel.StageTimer.Restart();
			}

			// Actually report the stage
			if (StageNumber >= 1 & StageNumber <= 4)
			{
				if (!Flags.EnableSplash & !Flags.QuietKernel)
				{
					TextWriterColor.WritePlain("", true);
					SeparatorWriterColor.WriteSeparator(StageText, false, KernelColorTools.ColTypes.Stage);
				}
				DebugWriter.Wdbg(DebugLevel.I, $"- Kernel stage {StageNumber} | Text: {StageText}");
			}
		}

		/// <summary>
		/// Gets the used compiler variables for building Kernel Simulator
		/// </summary>
		/// <returns>An array containing used compiler variables</returns>
		public static string[] GetCompilerVars()
		{
			var CompilerVars = new List<string>
			{
				// Determine the compiler vars used to build KS using conditional checks
				/* TODO ERROR: Skipped IfDirectiveTrivia
#if SPECIFIERDEV
				*/
				"SPECIFIER = \"DEV\""
			};
			/* TODO ERROR: Skipped ElifDirectiveTrivia
#elif SPECIFIERRC
			*//* TODO ERROR: Skipped DisabledTextTrivia
						CompilerVars.Add("SPECIFIER = ""RC""")
			*//* TODO ERROR: Skipped ElifDirectiveTrivia
#elif SPECIFIERREL
			*//* TODO ERROR: Skipped DisabledTextTrivia
						CompilerVars.Add("SPECIFIER = ""REL""")
			*//* TODO ERROR: Skipped EndIfDirectiveTrivia
#endif
			*/
			/* TODO ERROR: Skipped IfDirectiveTrivia
# If ENABLEIMMEDIATEWINDOWDEBUG Then
			*//* TODO ERROR: Skipped DisabledTextTrivia
						CompilerVars.Add("ENABLEIMMEDIATEWINDOWDEBUG")
			*//* TODO ERROR: Skipped EndIfDirectiveTrivia
#endif
				*/
			/* TODO ERROR: Skipped IfDirectiveTrivia
			#If POP3Feature Then
			*//* TODO ERROR: Skipped DisabledTextTrivia
						CompilerVars.Add("POP3Feature")
			*//* TODO ERROR: Skipped EndIfDirectiveTrivia
			#endif
			*/
			// Return the compiler vars
			return [.. CompilerVars];
		}

	}
}
