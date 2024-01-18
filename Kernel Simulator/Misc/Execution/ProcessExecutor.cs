//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;


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
using KS.Files.Folders;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Execution
{
    public static class ProcessExecutor
    {

        internal static string ProcessData = "";
        internal static bool NewDataSpotted;
        internal static KernelThread NewDataDetector = new("New data detection for process", false, DetectNewData);

        /// <summary>
        /// Thread parameters for ExecuteProcess()
        /// </summary>
        internal class ExecuteProcessThreadParameters
        {
            /// <summary>
            /// Full path to file
            /// </summary>
            internal string File;
            /// <summary>
            /// Arguments, if any
            /// </summary>
            internal string Args;

            internal ExecuteProcessThreadParameters(string File, string Args)
            {
                this.File = File;
                this.Args = Args;
            }
        }

        /// <summary>
        /// Executes a file with specified arguments
        /// </summary>
        internal static void ExecuteProcess(ExecuteProcessThreadParameters ThreadParams)
        {
            ExecuteProcess(ThreadParams.File, ThreadParams.Args);
        }

        /// <summary>
        /// Executes a file with specified arguments
        /// </summary>
        /// <param name="File">Full path to file</param>
        /// <param name="Args">Arguments, if any</param>
        /// <returns>Application exit code. -1 if internal error occurred.</returns>
        public static int ExecuteProcess(string File, string Args)
        {
            return ExecuteProcess(File, Args, CurrentDirectory.CurrentDir);
        }

        /// <summary>
        /// Executes a file with specified arguments
        /// </summary>
        /// <param name="File">Full path to file</param>
        /// <param name="Args">Arguments, if any</param>
        /// <returns>Application exit code. -1 if internal error occurred.</returns>
        public static int ExecuteProcess(string File, string Args, string WorkingDirectory)
        {
            try
            {
                var CommandProcess = new Process();
                var CommandProcessStart = new ProcessStartInfo()
                {
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = File,
                    Arguments = Args,
                    WorkingDirectory = WorkingDirectory,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false
                };
                CommandProcess.StartInfo = CommandProcessStart;
                CommandProcess.OutputDataReceived += ExecutableOutput;
                CommandProcess.ErrorDataReceived += ExecutableOutput;

                // Start the process
                DebugWriter.Wdbg(DebugLevel.I, "Starting...");
                CommandProcess.Start();
                CommandProcess.BeginOutputReadLine();
                CommandProcess.BeginErrorReadLine();
                NewDataDetector.Start();

                // Wait for process exit
                while (!CommandProcess.HasExited | !Flags.CancelRequested)
                {
                    if (CommandProcess.HasExited)
                    {
                        break;
                    }
                    else if (Flags.CancelRequested)
                    {
                        CommandProcess.Kill();
                        break;
                    }
                }

                // Wait until no more data is entered
                while (NewDataSpotted)
                {
                }

                // Stop the new data detector
                NewDataDetector.Stop();
                ProcessData = "";

                // Assume that we've spotted new data. This is to avoid race conditions happening sometimes if the processes are exited while output is still going.
                // This is a workaround for some commands like netstat.exe that don't work with normal workarounds shown below.
                NewDataSpotted = true;
                return CommandProcess.ExitCode;
            }
            catch (ThreadInterruptedException)
            {
                Flags.CancelRequested = false;
                return -1;
            }
            catch (Exception ex)
            {
                Kernel.Kernel.KernelEventManager.RaiseProcessError(File + Args, ex);
                DebugWriter.WStkTrc(ex);
                TextWriterColor.Write(Translate.DoTranslation("Error trying to execute command") + " {2}." + Kernel.Kernel.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.GetType().FullName, ex.Message, File);
            }
            return -1;
        }

        internal static ProcessStartInfo StripEnvironmentVariables(ProcessStartInfo processStartInfo)
        {
            // --- UseShellExecute and the Environment property population Hack ---
            //
            // We need UseShellExecute to be able to use the runas verb, but it looks like that we can't start the process with the VS debugger,
            // because the StartInfo always populates the _environmentVariables field once the Environment property is populated.
            // _environmentVariables is not a public field.
            //
            // .NET expects _environmentVariables to be null when trying to start the process with the UseShellExecute being set to true,
            // but when calling Start(), .NET calls StartWithShellExecuteEx() and checks to see if that variable is null, so executing the
            // process in this way is basically impossible after evaluating the Environment property without having to somehow nullify this
            // _environmentVariables field using private reflection after evaluating the Environment property.
            //
            // if (startInfo._environmentVariables != null)
            //     throw new InvalidOperationException(SR.CantUseEnvVars);
            //
            // Please DO NOT even try to evaluate selfProcess.StartInfo.Environment in your debugger even if hovering over selfProcess.StartInfo,
            // because that would undo all the changes that we've made to the _environmentVariables and causes us to lose all the changes made
            // to this instance of StartInfo.
            //
            // if (_environmentVariables == null)
            // {
            //     IDictionary envVars = System.Environment.GetEnvironmentVariables();
            //     _environmentVariables = new DictionaryWrapper(new Dictionary<string, string?>(
            //     (...)
            // }
            //
            // This hack is only applicable to developers debugging the StartInfo instance of this specific process using VS. Nitrocid should
            // be able to restart itself as elevated normally if no debugger is attached.
            //
            // References:
            //   - https://github.com/dotnet/runtime/blob/release/8.0/src/libraries/System.Diagnostics.Process/src/System/Diagnostics/Process.Win32.cs#L47
            //   - https://github.com/dotnet/runtime/blob/release/8.0/src/libraries/System.Diagnostics.Process/src/System/Diagnostics/ProcessStartInfo.cs#L91
            //
            // Issue report: https://github.com/dotnet/runtime/issues/94338
            var privateReflection = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField;
            var startInfoType = processStartInfo.GetType();
#if NETCOREAPP
            var envVarsField = startInfoType.GetField("_environmentVariables", privateReflection);
#else
            var envVarsField = startInfoType.GetField("environmentVariables", privateReflection);
#endif
            envVarsField.SetValue(processStartInfo, null);
            // 
            // --- UseShellExecute and the Environment property population Hack End ---
            return processStartInfo;
        }

        /// <summary>
        /// Handles executable output
        /// </summary>
        /// <param name="sendingProcess">Sender</param>
        /// <param name="outLine">Output</param>
        private static void ExecutableOutput(object sendingProcess, DataReceivedEventArgs outLine)
        {
            NewDataSpotted = true;
            DebugWriter.Wdbg(DebugLevel.I, outLine.Data);
            TextWriterColor.Write(outLine.Data, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
            ProcessData += outLine.Data;
        }

        /// <summary>
        /// Detects new data
        /// </summary>
        private static void DetectNewData()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                if (NewDataSpotted)
                {
                    long OldLength = ProcessData.LongCount();
                    Thread.Sleep(50);
                    if (OldLength == ProcessData.LongCount())
                        NewDataSpotted = false;
                }
            }
        }

    }
}
