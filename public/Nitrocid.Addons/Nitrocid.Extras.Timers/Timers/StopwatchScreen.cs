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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Languages;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Nitrocid.Drivers.RNG;
using Terminaux.Inputs;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Colors.Data;
using Terminaux.Writer.CyclicWriters;

namespace Nitrocid.Extras.Timers.Timers
{
    /// <summary>
    /// Stopwatch CLI module
    /// </summary>
    public static class StopwatchScreen
    {

        internal static List<LapDisplayInfo> Laps = [];
        internal static Color? LapColor;
        internal static Stopwatch Stopwatch = new();
        internal static Stopwatch LappedStopwatch = new();
        internal static bool running;
        private readonly static Keybinding[] keyBindings =
        [
            new( /* Localizable */ "Start or stop", ConsoleKey.Enter),
            new( /* Localizable */ "Lap", ConsoleKey.L),
            new( /* Localizable */ "Lap list", ConsoleKey.L, ConsoleModifiers.Shift),
            new( /* Localizable */ "Reset", ConsoleKey.R),
            new( /* Localizable */ "Exit", ConsoleKey.Escape),
        ];

        /// <summary>
        /// Opens the stopwatch screen
        /// </summary>
        public static void OpenStopwatch()
        {
            Screen watchScreen = new();
            ScreenPart watchScreenPart = new();
            ScreenTools.SetCurrent(watchScreen);
            ColorTools.LoadBack();
            string status = Translate.DoTranslation("Stopwatch is ready.");

            // Set the random lap color
            int RedValue = RandomDriver.Random(255);
            int GreenValue = RandomDriver.Random(255);
            int BlueValue = RandomDriver.Random(255);
            LapColor = new Color(RedValue, GreenValue, BlueValue);

            // Add a dynamic text that shows you the time dynamically
            watchScreenPart.AddDynamicText(() =>
            {
                // If resized, clear the console
                ConsoleWrapper.CursorVisible = false;
                var builder = new StringBuilder();

                // Populate the positions for time
                string LapsText = Translate.DoTranslation("Lap");
                int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
                int HalfHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
                var elapsed = Stopwatch.Elapsed;
                string elapsedString = elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult);
                int TimeLeftPosition = (int)Math.Round(HalfWidth * 1.5d - elapsedString.Length / 2d);
                int TimeTopPosition = HalfHeight - 1;
                int LapsCurrentLapLeftPosition = 2;
                int LapsCurrentLapTopPosition = ConsoleWrapper.WindowHeight - 3;

                // Print the keybindings
                int KeysTextTopPosition = ConsoleWrapper.WindowHeight - 1;
                var keybindings = new Keybindings()
                {
                    KeybindingList = keyBindings,
                    Left = 0,
                    Top = KeysTextTopPosition,
                    Width = ConsoleWrapper.WindowWidth - 1,
                };
                builder.Append(keybindings.Render());

                // Print the time interval and the current lap
                builder.Append(
                    TextWriterWhereColor.RenderWhereColorBack(elapsedString, TimeLeftPosition, TimeTopPosition, true, LapColor, KernelColorTools.GetColor(KernelColorType.Background)) +
                    TextWriterWhereColor.RenderWhereColorBack(LapsText + " {0}: {1}", LapsCurrentLapLeftPosition, LapsCurrentLapTopPosition, true, LapColor, KernelColorTools.GetColor(KernelColorType.Background), Laps.Count + 1, LappedStopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult))
                );

                // Also, print the time difference of the last lap if required
                if (Laps.Count > 1)
                {
                    var firstLastLap = Laps[^1];
                    var secondLastLap = Laps[^2];
                    int lapTopPosition = HalfHeight + 1;
                    var diff = secondLastLap.LapInterval - firstLastLap.LapInterval;
                    bool slower = diff < TimeSpan.Zero;
                    string elapsedDiff = diff.ToString((slower ? "\\+" : "\\-") + @"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult);
                    Color finalLapColor = slower ? new Color(ConsoleColors.Red) : new Color(ConsoleColors.Lime);
                    builder.Append(
                        TextWriterWhereColor.RenderWhereColorBack(elapsedDiff, TimeLeftPosition, lapTopPosition, true, finalLapColor, KernelColorTools.GetColor(KernelColorType.Background))
                    );
                }

                // Print the border
                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                var lapsBoxFrame = new BoxFrame()
                {
                    Left = 0,
                    Top = SeparatorMinimumHeight,
                    InteriorWidth = SeparatorHalfConsoleWidthInterior,
                    InteriorHeight = SeparatorMaximumHeightInterior,
                    FrameColor = ColorTools.GetGray(),
                    BackgroundColor = KernelColorTools.GetColor(KernelColorType.Background),
                };
                var stopwatchBoxFrame = new BoxFrame()
                {
                    Left = SeparatorHalfConsoleWidth,
                    Top = SeparatorMinimumHeight,
                    InteriorWidth = SeparatorHalfConsoleWidthInterior + (ConsoleWrapper.WindowWidth % 2 != 0 ? 1 : 0),
                    InteriorHeight = SeparatorMaximumHeightInterior,
                    FrameColor = ColorTools.GetGray(),
                    BackgroundColor = KernelColorTools.GetColor(KernelColorType.Background),
                };
                builder.Append(
                    lapsBoxFrame.Render() +
                    stopwatchBoxFrame.Render()
                );

                // Print informational messages
                builder.Append(
                    TextWriterWhereColor.RenderWhereColorBack(status, 0, 0, false, KernelColorTools.GetColor(KernelColorType.NeutralText), KernelColorTools.GetColor(KernelColorType.Background)) +
                    ConsoleClearing.GetClearLineToRightSequence()
                );

                // Print the laps list
                int LapsLapsListLeftPosition = 2;
                int LapsLapsListTopPosition = 2;
                int LapsListEndBorder = ConsoleWrapper.WindowHeight - 6;
                var LapsListBuilder = new StringBuilder();
                int BorderDifference = Laps.Count - LapsListEndBorder;
                if (BorderDifference < 0)
                    BorderDifference = 0;
                for (int LapIndex = BorderDifference; LapIndex <= Laps.Count - 1; LapIndex++)
                {
                    var Lap = Laps[LapIndex];
                    LapsListBuilder.AppendLine(Lap.LapColor.VTSequenceForeground + Translate.DoTranslation("Lap") + $" {LapIndex + 1}: {Lap.LapInterval.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult)}");
                }
                builder.Append(
                    TextWriterWhereColor.RenderWhereColorBack(LapsListBuilder.ToString(), LapsLapsListLeftPosition, LapsLapsListTopPosition, true, LapColor, KernelColorTools.GetColor(KernelColorType.Background))
                );

                // Return the resultant buffer
                return builder.ToString();
            });

            watchScreen.AddBufferedPart("Stopwatch Update", watchScreenPart);
            bool exiting = false;
            while (!exiting)
            {
                ScreenTools.Render(watchScreen);

                // Check to see if the timer is running to continually update the timer render
                ConsoleKeyInfo KeysKeypress = default;
                if (running)
                {
                    // Wait for a keypress
                    if (ConsoleWrapper.KeyAvailable)
                        KeysKeypress = Input.ReadKey();
                }
                else
                {
                    // Wait for a keypress
                    KeysKeypress = Input.ReadKey();
                }

                // Check for a keypress
                switch (KeysKeypress.Key)
                {
                    case ConsoleKey.Enter:
                        {
                            if (LappedStopwatch.IsRunning)
                                LappedStopwatch.Stop();
                            else
                                LappedStopwatch.Start();
                            if (Stopwatch.IsRunning)
                                Stopwatch.Stop();
                            else
                                Stopwatch.Start();
                            running = Stopwatch.IsRunning;
                            status = Translate.DoTranslation("Stopwatch running!");
                            break;
                        }
                    case ConsoleKey.L:
                        {
                            if (LappedStopwatch.IsRunning)
                            {
                                if (KeysKeypress.Modifiers == ConsoleModifiers.Shift)
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal(GenerateLapList());
                                    watchScreen.RequireRefresh();
                                    break;
                                }
                                var Lap = new LapDisplayInfo(LapColor, LappedStopwatch.Elapsed);
                                Laps.Add(Lap);
                                LappedStopwatch.Restart();

                                // Select random color
                                RedValue = RandomDriver.Random(255);
                                GreenValue = RandomDriver.Random(255);
                                BlueValue = RandomDriver.Random(255);
                                LapColor = new Color(RedValue, GreenValue, BlueValue);
                                status = Translate.DoTranslation("New lap!") + $" {Lap.LapInterval}";
                            }
                            break;
                        }
                    case ConsoleKey.R:
                        {
                            if (LappedStopwatch.IsRunning)
                                LappedStopwatch.Reset();
                            if (Stopwatch.IsRunning)
                                Stopwatch.Reset();
                            running = false;
                            watchScreen.RequireRefresh();

                            // Clear the laps
                            Laps.Clear();
                            LapColor = KernelColorTools.GetColor(KernelColorType.NeutralText);
                            status = Translate.DoTranslation("Stopwatch is ready.");
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            if (LappedStopwatch.IsRunning)
                                LappedStopwatch.Reset();
                            if (Stopwatch.IsRunning)
                                Stopwatch.Reset();
                            LapColor = KernelColorTools.GetColor(KernelColorType.NeutralText);
                            exiting = true;
                            break;
                        }
                }
            }

            // Clear for cleanliness
            ScreenTools.UnsetCurrent(watchScreen);
            running = false;
            Laps.Clear();
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = true;
        }

        private static string GenerateLapList()
        {
            var lapsListBuilder = new StringBuilder();
            for (int i = 0; i < Laps.Count; i++)
            {
                LapDisplayInfo? lap = Laps[i];
                lapsListBuilder.AppendLine(lap.LapColor.VTSequenceForeground + Translate.DoTranslation("Lap") + $" {i + 1}: {lap.LapInterval.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult)}");
            }
            if (Laps.Count == 0)
                lapsListBuilder.AppendLine(Translate.DoTranslation("No laps yet..."));
            return lapsListBuilder.ToString();
        }
    }
}
