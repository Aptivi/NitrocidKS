

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

using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace KS.Misc.Threading
{
	public static class ThreadManager
	{

		/// <summary>
        /// Sleeps until either the time specified, or the thread has finished or cancelled.
        /// </summary>
        /// <param name="Time">Time in milliseconds</param>
        /// <param name="ThreadWork">The working thread</param>
		public static void SleepNoBlock(long Time, BackgroundWorker ThreadWork)
		{
			var WorkFinished = default(bool);
			var TimeCount = default(long);
			ThreadWork.RunWorkerCompleted += () => WorkFinished = true;
			while (!(WorkFinished | TimeCount == Time))
			{
				Thread.Sleep(1);
				if (ThreadWork.CancellationPending)
					WorkFinished = true;
				TimeCount += 1L;
			}
		}

		/// <summary>
        /// Sleeps until either the time specified, or the thread is no longer alive.
        /// </summary>
        /// <param name="Time">Time in milliseconds</param>
        /// <param name="ThreadWork">The working thread</param>
		public static void SleepNoBlock(long Time, Thread ThreadWork)
		{
			ThreadWork.Join((int)Time);
		}

		/// <summary>
        /// Sleeps until either the time specified, or the thread is no longer alive.
        /// </summary>
        /// <param name="Time">Time in milliseconds</param>
        /// <param name="ThreadWork">The working thread</param>
		public static void SleepNoBlock(long Time, KernelThread ThreadWork)
		{
			ThreadWork.Wait(Time);
		}

		/// <summary>
        /// Gets the actual milliseconds time from the sleep time provided
        /// </summary>
        /// <param name="Time">Sleep time</param>
        /// <returns>How many milliseconds did it really sleep?</returns>
		public static int GetActualMilliseconds(int Time)
		{
			var SleepStopwatch = new Stopwatch();
			SleepStopwatch.Start();
			Thread.Sleep(Time);
			SleepStopwatch.Stop();
			return (int)SleepStopwatch.ElapsedMilliseconds;
		}

		/// <summary>
        /// Gets the actual ticks from the sleep time provided
        /// </summary>
        /// <param name="Time">Sleep time</param>
        /// <returns>How many ticks did it really sleep?</returns>
		public static long GetActualTicks(int Time)
		{
			var SleepStopwatch = new Stopwatch();
			SleepStopwatch.Start();
			Thread.Sleep(Time);
			SleepStopwatch.Stop();
			return SleepStopwatch.ElapsedTicks;
		}

	}
}