using System;
using KS.ConsoleBase.Colors;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;
using Terminaux.Colors;

namespace KS.Misc.Animations.EdgePulse
{
	public static class EdgePulse
	{

		private static int CurrentWindowWidth;
		private static int CurrentWindowHeight;
		private static bool ResizeSyncing;

		/// <summary>
		/// Simulates the edge pulsing animation
		/// </summary>
		public static void Simulate(EdgePulseSettings Settings)
		{
			CurrentWindowWidth = ConsoleWrapper.WindowWidth;
			CurrentWindowHeight = ConsoleWrapper.WindowHeight;

			// Now, do the rest
			var RandomDriver = Settings.RandomDriver;
			int RedColorNum = RandomDriver.Next(Settings.EdgePulseMinimumRedColorLevel, Settings.EdgePulseMaximumRedColorLevel);
			int GreenColorNum = RandomDriver.Next(Settings.EdgePulseMinimumGreenColorLevel, Settings.EdgePulseMaximumGreenColorLevel);
			int BlueColorNum = RandomDriver.Next(Settings.EdgePulseMinimumBlueColorLevel, Settings.EdgePulseMaximumBlueColorLevel);
			ConsoleWrapper.CursorVisible = false;

			// Set thresholds
			double ThresholdRed = RedColorNum / (double)Settings.EdgePulseMaxSteps;
			double ThresholdGreen = GreenColorNum / (double)Settings.EdgePulseMaxSteps;
			double ThresholdBlue = BlueColorNum / (double)Settings.EdgePulseMaxSteps;
			DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

			// Fade in
			int CurrentColorRedIn = 0;
			int CurrentColorGreenIn = 0;
			int CurrentColorBlueIn = 0;
			for (int CurrentStep = Settings.EdgePulseMaxSteps; CurrentStep >= 1; CurrentStep -= 1)
			{
				if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
					ResizeSyncing = true;
				if (ResizeSyncing)
					break;
				DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.EdgePulseMaxSteps);
				ThreadManager.SleepNoBlock(Settings.EdgePulseDelay, System.Threading.Thread.CurrentThread);
				CurrentColorRedIn = (int)Math.Round(CurrentColorRedIn + ThresholdRed);
				CurrentColorGreenIn = (int)Math.Round(CurrentColorGreenIn + ThresholdGreen);
				CurrentColorBlueIn = (int)Math.Round(CurrentColorBlueIn + ThresholdBlue);
				DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn);
				if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
					ResizeSyncing = true;
				if (!ResizeSyncing)
				{
					KernelColorTools.SetConsoleColor(new Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn), true);
					FillIn();
				}
			}

			// Fade out
			for (int CurrentStep = 1, loopTo = Settings.EdgePulseMaxSteps; CurrentStep <= loopTo; CurrentStep++)
			{
				if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
					ResizeSyncing = true;
				if (ResizeSyncing)
					break;
				DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.EdgePulseMaxSteps);
				ThreadManager.SleepNoBlock(Settings.EdgePulseDelay, System.Threading.Thread.CurrentThread);
				int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
				int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
				int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
				DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
				if (!ResizeSyncing)
				{
					KernelColorTools.SetConsoleColor(new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut), true);
					FillIn();
				}
			}

			// Reset resize sync
			ResizeSyncing = false;
			CurrentWindowWidth = ConsoleWrapper.WindowWidth;
			CurrentWindowHeight = ConsoleWrapper.WindowHeight;
			ThreadManager.SleepNoBlock(Settings.EdgePulseDelay, System.Threading.Thread.CurrentThread);
		}

		private static void FillIn()
		{
			int FloorTopLeftEdge = 0;
			int FloorBottomLeftEdge = 0;
			DebugWriter.Wdbg(DebugLevel.I, "Top left edge: {0}, Bottom left edge: {1}", FloorTopLeftEdge, FloorBottomLeftEdge);

			int FloorTopRightEdge = ConsoleWrapper.WindowWidth - 1;
			int FloorBottomRightEdge = ConsoleWrapper.WindowWidth - 1;
			DebugWriter.Wdbg(DebugLevel.I, "Top right edge: {0}, Bottom right edge: {1}", FloorTopRightEdge, FloorBottomRightEdge);

			int FloorTopEdge = 0;
			int FloorBottomEdge = ConsoleWrapper.WindowHeight - 1;
			DebugWriter.Wdbg(DebugLevel.I, "Top edge: {0}, Bottom edge: {1}", FloorTopEdge, FloorBottomEdge);

			int FloorLeftEdge = 0;
			int FloorRightEdge = ConsoleWrapper.WindowWidth - 2;
			DebugWriter.Wdbg(DebugLevel.I, "Left edge: {0}, Right edge: {1}", FloorLeftEdge, FloorRightEdge);

			// First, draw the floor top edge
			for (int x = FloorTopLeftEdge, loopTo = FloorTopRightEdge; x <= loopTo; x++)
			{
				ConsoleWrapper.SetCursorPosition(x, 0);
				DebugWriter.Wdbg(DebugLevel.I, "Drawing floor top edge ({0}, {1})", x, 1);
				TextWriterColor.WritePlain(" ", false);
			}

			// Second, draw the floor bottom edge
			for (int x = FloorBottomLeftEdge, loopTo1 = FloorBottomRightEdge; x <= loopTo1; x++)
			{
				ConsoleWrapper.SetCursorPosition(x, FloorBottomEdge);
				DebugWriter.Wdbg(DebugLevel.I, "Drawing floor bottom edge ({0}, {1})", x, FloorBottomEdge);
				TextWriterColor.WritePlain(" ", false);
			}

			// Third, draw the floor left edge
			for (int y = FloorTopEdge, loopTo2 = FloorBottomEdge; y <= loopTo2; y++)
			{
				ConsoleWrapper.SetCursorPosition(FloorLeftEdge, y);
				DebugWriter.Wdbg(DebugLevel.I, "Drawing floor left edge ({0}, {1})", FloorLeftEdge, y);
				TextWriterColor.WritePlain("  ", false);
			}

			// Finally, draw the floor right edge
			for (int y = FloorTopEdge, loopTo3 = FloorBottomEdge; y <= loopTo3; y++)
			{
				ConsoleWrapper.SetCursorPosition(FloorRightEdge, y);
				DebugWriter.Wdbg(DebugLevel.I, "Drawing floor right edge ({0}, {1})", FloorRightEdge, y);
				TextWriterColor.WritePlain("  ", false);
			}
		}

	}
}