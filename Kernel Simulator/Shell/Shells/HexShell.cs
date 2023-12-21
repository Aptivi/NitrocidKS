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

using System.Threading;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Editors.HexEdit;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.Prompts;
using KS.Shell.ShellBase;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells
{
	public class HexShell : ShellExecutor, IShell
	{

		public override ShellType ShellType
		{
			get
			{
				return ShellType.HexShell;
			}
		}

		public override bool Bail { get; set; }

		public override void InitializeShell(params object[] ShellArgs)
		{
			// Get file path
			string FilePath = "";
			if (ShellArgs.Length > 0)
			{
				FilePath = Convert.ToString(ShellArgs[0]);
			}
			else
			{
				TextWriterColor.Write(Translate.DoTranslation("File not specified. Exiting shell..."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
				Bail = true;
			}
			TextWriterColor.Write(Translate.DoTranslation("Please note that editing binary files using this shell is experimental and may lead to data corruption or data loss if not used properly.") + Kernel.Kernel.NewLine + Translate.DoTranslation("DON'T LAUNCH THE SHELL UNLESS YOU KNOW WHAT YOU'RE DOING!"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Warning));

			// Actual shell logic
			while (!Bail)
			{
				try
				{
					// Open file if not open
					if (HexEditShellCommon.HexEdit_FileStream is null)
					{
						DebugWriter.Wdbg(DebugLevel.W, "File not open yet. Trying to open {0}...", FilePath);
						if (!HexEditTools.HexEdit_OpenBinaryFile(FilePath))
						{
							TextWriterColor.Write(Translate.DoTranslation("Failed to open file. Exiting shell..."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
							Bail = true;
							break;
						}
						HexEditShellCommon.HexEdit_AutoSave.Start();
					}

					// See UESHShell.vb for more info
					lock (CancellationHandlers.GetCancelSyncLock(ShellType))
					{
						// Restore the console state
						if (Kernel.Kernel.DefConsoleOut is not null)
						{
							Console.SetOut(Kernel.Kernel.DefConsoleOut);
						}

						// Prepare for prompt
						PromptPresetManager.WriteShellPrompt(ShellType);

						// Raise the event
						Kernel.Kernel.KernelEventManager.RaiseHexShellInitialized();
					}

					// Prompt for command
					string WrittenCommand = Input.ReadLine();
					if ((string.IsNullOrEmpty(WrittenCommand) | (WrittenCommand?.StartsWithAnyOf([" ", "#"]))) == false)
					{
						Kernel.Kernel.KernelEventManager.RaiseHexPreExecuteCommand(WrittenCommand);
						Shell.GetLine(WrittenCommand, false, "", ShellType.HexShell);
						Kernel.Kernel.KernelEventManager.RaiseHexPostExecuteCommand(WrittenCommand);
					}
				}
				catch (ThreadInterruptedException taex)
				{
					Flags.CancelRequested = false;
					Bail = true;
				}
				catch (Exception ex)
				{
					DebugWriter.WStkTrc(ex);
					TextWriterColor.Write(Translate.DoTranslation("There was an error in the shell.") + Kernel.Kernel.NewLine + "Error {0}: {1}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.GetType().FullName, ex.Message);
					continue;
				}
			}

			// Close file
			HexEditTools.HexEdit_CloseBinaryFile();
			HexEditShellCommon.HexEdit_AutoSave.Stop();
		}

	}
}