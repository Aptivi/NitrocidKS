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

using Textify.Data.Figlet;
using Nitrocid.ScreensaverPacks.Animations.Glitch;
using System;
using Terminaux.Colors;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Threading;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Kernel.Time;
using Nitrocid.Languages;
using Terminaux.Base;
using Nitrocid.Kernel.Configuration;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for KSX3
    /// </summary>
    public class KSX3Display : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "KSX3";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages =>
            true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            Color black = new(0, 0, 0);
            ColorTools.LoadBackDry(black);
            ConsoleWrapper.CursorVisible = false;
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int step;
            int maxSteps = 13;
            Color green = new(0, 255, 0);
            Color red = new(255, 0, 0);
            Color pink = new(255, 0, 255);
            Color black = new(0, 0, 0);
            Color white = new(255, 255, 255);
            Color selectedColor = Color.Empty;
            string year = "2018";
            var font = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);

            // Start stepping
            for (step = 1; step <= maxSteps; step++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                switch (step)
                {
                    // Step 1: Show figlet text with the following text:
                    //   - 0.0.1 with green color
                    //   - 0.0.16 with red color
                    //   - 0.0.24 with pink color
                    case 1:
                        ColorTools.LoadBackDry(black);
                        int colorSteps = 30;

                        // Get the color thresholds
                        double thresholdGR = green.RGB.R / (double)colorSteps;
                        double thresholdGG = green.RGB.G / (double)colorSteps;
                        double thresholdGB = green.RGB.B / (double)colorSteps;
                        double thresholdRR = red.RGB.R / (double)colorSteps;
                        double thresholdRG = red.RGB.G / (double)colorSteps;
                        double thresholdRB = red.RGB.B / (double)colorSteps;
                        double thresholdPR = pink.RGB.R / (double)colorSteps;
                        double thresholdPG = pink.RGB.G / (double)colorSteps;
                        double thresholdPB = pink.RGB.B / (double)colorSteps;

                        // Now, transition
                        int currentR = 0;
                        int currentG = 0;
                        int currentB = 0;
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR + thresholdGR);
                            currentG = (int)Math.Round(currentG + thresholdGG);
                            currentB = (int)Math.Round(currentB + thresholdGB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(font, "v0.0.1", col, black);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR - thresholdGR);
                            currentG = (int)Math.Round(currentG - thresholdGG);
                            currentB = (int)Math.Round(currentB - thresholdGB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(font, "v0.0.1", col, black);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR + thresholdRR);
                            currentG = (int)Math.Round(currentG + thresholdRG);
                            currentB = (int)Math.Round(currentB + thresholdRB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(font, "v0.0.16", col, black);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR - thresholdRR);
                            currentG = (int)Math.Round(currentG - thresholdRG);
                            currentB = (int)Math.Round(currentB - thresholdRB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(font, "v0.0.16", col, black);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR + thresholdPR);
                            currentG = (int)Math.Round(currentG + thresholdPG);
                            currentB = (int)Math.Round(currentB + thresholdPB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(font, "v0.0.24", col, black);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 2: Console background slowly changes to pink
                    case 2:
                        colorSteps = 30;

                        // Get the color thresholds
                        thresholdPR = pink.RGB.R / (double)colorSteps;
                        thresholdPG = pink.RGB.G / (double)colorSteps;
                        thresholdPB = pink.RGB.B / (double)colorSteps;

                        // Now, transition from target color to black
                        int currentBackR = 0;
                        int currentBackG = 0;
                        int currentBackB = 0;
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentBackR = (int)Math.Round(currentBackR + thresholdPR);
                            currentBackG = (int)Math.Round(currentBackG + thresholdPG);
                            currentBackB = (int)Math.Round(currentBackB + thresholdPB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentBackR, currentBackG, currentBackB);
                            ColorTools.LoadBackDry(col);
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(font, "v0.0.24", pink, col);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 3: Figlet text slowly changes to black
                    case 3:
                        colorSteps = 30;

                        // Get the color thresholds
                        thresholdPR = pink.RGB.R / (double)colorSteps;
                        thresholdPG = pink.RGB.G / (double)colorSteps;
                        thresholdPB = pink.RGB.B / (double)colorSteps;

                        // Now, transition from target color to black
                        currentR = 255;
                        currentG = 0;
                        currentB = 255;
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR - thresholdPR);
                            currentG = (int)Math.Round(currentG - thresholdPG);
                            currentB = (int)Math.Round(currentB - thresholdPB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            ColorTools.LoadBackDry(pink);
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(font, "v0.0.24", col, pink);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 4: X shows in random places
                    case 4:
                        for (int xIter = 0; xIter < 1000; xIter++)
                        {
                            int xwidth = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                            int xheight = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                            TextWriterWhereColor.WriteWhereColorBack("X", xwidth, xheight, black, pink);
                            ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 5: Glitches show in random places
                    case 5:
                        for (int delayed = 0; delayed < 5000; delayed += 10)
                        {
                            ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                            Glitch.GlitchAt();
                        }
                        break;
                    // Step 6: Flash red and pink for a few seconds
                    case 6:
                        int maxFlashes = 200;
                        for (int flashes = 0; flashes <= maxFlashes; flashes++)
                        {
                            bool showRed = flashes % 2 == 0;
                            if (showRed)
                                ColorTools.LoadBackDry(red);
                            else
                                ColorTools.LoadBackDry(pink);
                            ThreadManager.SleepNoBlock(50, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 7: Three random date selectors
                    case 7:
                        ColorTools.LoadBackDry(red);
                        int maxSelections = RandomDriver.Random(100, 150);
                        int oneThirdConsoleWidth = ConsoleWrapper.WindowWidth / 3;
                        int selectorsPositionY = ConsoleWrapper.WindowHeight - 2;
                        for (int selections = 0; selections <= maxSelections; selections++)
                        {
                            ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                            int randomYear1 = RandomDriver.Random(2018, 2024);
                            int randomYear2 = RandomDriver.Random(2018, 2024);
                            int randomYear3 = RandomDriver.Random(2018, 2024);
                            int randomMonth1 = RandomDriver.Random(1, 12);
                            int randomMonth2 = RandomDriver.Random(1, 12);
                            int randomMonth3 = RandomDriver.Random(1, 12);
                            int randomDay1 = RandomDriver.Random(1, 28);
                            int randomDay2 = RandomDriver.Random(1, 28);
                            int randomDay3 = RandomDriver.Random(1, 28);
                            var selectedDate1 = new DateTime(randomYear1, randomMonth1, randomDay1);
                            var selectedDate2 = new DateTime(randomYear2, randomMonth2, randomDay2);
                            var selectedDate3 = new DateTime(randomYear3, randomMonth3, randomDay3);

                            // Show it as a normal font
                            string renderedShort1 = $"    << {TimeDateRenderers.RenderDate(selectedDate1, FormatType.Short)} >>    ";
                            string renderedShort2 = $"    << {TimeDateRenderers.RenderDate(selectedDate2, FormatType.Short)} >>    ";
                            string renderedShort3 = $"    << {TimeDateRenderers.RenderDate(selectedDate3, FormatType.Short)} >>    ";

                            // Now, determine the positions
                            int selector1PositionX = oneThirdConsoleWidth / 2 - renderedShort1.Length / 2;
                            int selector2PositionX = 3 * oneThirdConsoleWidth / 2 - renderedShort2.Length / 2;
                            int selector3PositionX = 5 * oneThirdConsoleWidth / 2 - renderedShort3.Length / 2;

                            // Print the selected values to the console
                            TextWriterWhereColor.WriteWhereColorBack(renderedShort1, selector1PositionX, selectorsPositionY, white, red);
                            TextWriterWhereColor.WriteWhereColorBack(renderedShort2, selector2PositionX, selectorsPositionY, white, red);
                            TextWriterWhereColor.WriteWhereColorBack(renderedShort3, selector3PositionX, selectorsPositionY, white, red);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 8: Show a day selection screen trying to decide between the three target dates
                    case 8:
                        ColorTools.LoadBackDry(black);

                        // Get things ready
                        var targetDates = new[]
                        {
                            new DateTime(2018, 2, 22),
                            new DateTime(2021, 6, 12),
                            new DateTime(2022, 8, 2)
                        };
                        int selectedTargetDate = RandomDriver.RandomIdx(targetDates.Length);
                        var targetDate = targetDates[selectedTargetDate];
                        string renderedTarget = TimeDateRenderers.RenderDate(targetDate, FormatType.Short);
                        string renderedTargetLong = $"     {TimeDateRenderers.RenderDate(targetDate, FormatType.Long)}     ";
                        var s8figFont = FigletTools.GetFigletFont("small");
                        int s8figWidth = FigletTools.GetFigletWidth(renderedTarget, s8figFont) / 2;
                        int s8figHeight = FigletTools.GetFigletHeight(renderedTarget, s8figFont) / 2;
                        int s8consoleX = ConsoleWrapper.WindowWidth / 2 - s8figWidth;
                        int s8consoleY = ConsoleWrapper.WindowHeight / 2 - s8figHeight;
                        CenteredFigletTextColor.WriteCenteredFiglet(s8figFont, "...");

                        // Select a random date
                        for (int delayed = 0; delayed < 15000; delayed += 10)
                        {
                            ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                            int selectedDateIdx = RandomDriver.RandomIdx(targetDates.Length);
                            var selectedDate = targetDates[selectedDateIdx];

                            // Show it as a normal font
                            string renderedLong = $"     {TimeDateRenderers.RenderDate(selectedDate, FormatType.Long)}     ";
                            TextWriterWhereColor.WriteWhereColorBack(renderedLong, ConsoleWrapper.WindowWidth / 2 - renderedLong.Length / 2, s8consoleY + 5, white, black);
                        }

                        // Print the target date
                        switch (selectedTargetDate)
                        {
                            case 0:
                                // February 22nd, 2018 - Nitrocid KS 0.0.1 release
                                FigletWhereColor.WriteFigletWhereColorBack(renderedTarget, s8consoleX, s8consoleY, true, s8figFont, green, black);
                                TextWriterWhereColor.WriteWhereColorBack(renderedTargetLong, ConsoleWrapper.WindowWidth / 2 - renderedTargetLong.Length / 2, s8consoleY + 5, green, black);
                                selectedColor = green;
                                year = "2018";
                                for (int delayed = 0; delayed < 5000; delayed += 10)
                                {
                                    int xwidth = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                                    int xheight = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                                    TextWriterWhereColor.WriteWhereColorBack("X", xwidth, xheight, green, black);
                                    ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                                }
                                break;
                            case 1:
                                // June 12th, 2021 - Nitrocid KS 0.0.16 release
                                FigletWhereColor.WriteFigletWhereColorBack(renderedTarget, s8consoleX, s8consoleY, true, s8figFont, red, black);
                                TextWriterWhereColor.WriteWhereColorBack(renderedTargetLong, ConsoleWrapper.WindowWidth / 2 - renderedTargetLong.Length / 2, s8consoleY + 5, red, black);
                                selectedColor = red;
                                year = "2021";
                                for (int delayed = 0; delayed < 5000; delayed += 10)
                                {
                                    Glitch.GlitchAt();
                                    ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                                }
                                break;
                            case 2:
                                // August 2nd, 2022 - Nitrocid KS 0.0.24 release
                                FigletWhereColor.WriteFigletWhereColorBack(renderedTarget, s8consoleX, s8consoleY, true, s8figFont, pink, black);
                                TextWriterWhereColor.WriteWhereColorBack(renderedTargetLong, ConsoleWrapper.WindowWidth / 2 - renderedTargetLong.Length / 2, s8consoleY + 5, pink, black);
                                TextWriterWhereColor.WriteWhereColorBack("You're lucky!", ConsoleWrapper.WindowWidth / 2 - "You're lucky!".Length / 2, s8consoleY + 6, pink, black);
                                selectedColor = pink;
                                year = "2022";
                                for (int delayed = 0; delayed < 5000; delayed += 10)
                                    ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                                break;
                        }
                        break;
                    // Step 9: Fade from white to selected color to black
                    case 9:
                        colorSteps = 30;

                        // Get the color thresholds
                        double transitionThresholdR = (selectedColor.RGB.R - white.RGB.R) / (double)colorSteps;
                        double transitionThresholdG = (selectedColor.RGB.G - white.RGB.G) / (double)colorSteps;
                        double transitionThresholdB = (selectedColor.RGB.B - white.RGB.B) / (double)colorSteps;
                        double thresholdR = selectedColor.RGB.R / (double)colorSteps;
                        double thresholdG = selectedColor.RGB.G / (double)colorSteps;
                        double thresholdB = selectedColor.RGB.B / (double)colorSteps;

                        // Now, transition from black to the target color
                        currentR = white.RGB.R;
                        currentG = white.RGB.G;
                        currentB = white.RGB.B;
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Add the values according to the threshold
                            currentR = (int)Math.Round(currentR + transitionThresholdR);
                            currentG = (int)Math.Round(currentG + transitionThresholdG);
                            currentB = (int)Math.Round(currentB + transitionThresholdB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            ColorTools.LoadBackDry(col);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Add the values according to the threshold
                            currentR = (int)Math.Round(currentR - thresholdR);
                            currentG = (int)Math.Round(currentG - thresholdG);
                            currentB = (int)Math.Round(currentB - thresholdB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            ColorTools.LoadBackDry(col);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 10: fade the X character in, and use figlet
                    case 10:
                        ColorTools.LoadBackDry(black);
                        colorSteps = 30;

                        // Get the color thresholds
                        thresholdR = selectedColor.RGB.R / (double)colorSteps;
                        thresholdG = selectedColor.RGB.G / (double)colorSteps;
                        thresholdB = selectedColor.RGB.B / (double)colorSteps;

                        // Now, transition from black to the target color
                        currentR = 0;
                        currentG = 0;
                        currentB = 0;
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Add the values according to the threshold
                            currentR = (int)Math.Round(currentR + thresholdR);
                            currentG = (int)Math.Round(currentG + thresholdG);
                            currentB = (int)Math.Round(currentB + thresholdB);

                            // Now, make a color and write the X character using figlet
                            Color col = new(currentR, currentG, currentB);
                            var figFont = FigletTools.GetFigletFont("banner");
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(figFont, "X", col, black);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }

                        // Now, transition from the target color to black
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(selectedColor.RGB.R - thresholdR * currentStep);
                            currentG = (int)Math.Round(selectedColor.RGB.G - thresholdG * currentStep);
                            currentB = (int)Math.Round(selectedColor.RGB.B - thresholdB * currentStep);

                            // Now, make a color and write the X character using figlet
                            Color col = new(currentR, currentG, currentB);
                            var figFont = FigletTools.GetFigletFont("banner");
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(figFont, "X", col, black);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 11: Show the year
                    case 11:
                        string sample = $"{year}|";
                        bool printDone = false;
                        ConsoleWrapper.CursorLeft = 0;
                        ConsoleWrapper.CursorTop = 0;
                        while (!printDone)
                        {
                            // Keep writing the year until it reaches the end
                            for (int currentIdx = 0; currentIdx <= sample.Length - 1 && !printDone; currentIdx++)
                            {
                                // Write the current character
                                TextWriterColor.WriteColorBack(sample[currentIdx].ToString(), false, selectedColor, black);

                                // Sleep
                                ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);

                                // Check to see if we're at the end
                                if (ConsoleWrapper.CursorLeft == ConsoleWrapper.WindowWidth - 1)
                                {
                                    if (ConsoleWrapper.CursorTop == ConsoleWrapper.WindowHeight - 1)
                                    {
                                        // We're at the end. Increment the index or reset to zero
                                        currentIdx++;
                                        if (currentIdx > sample.Length - 1)
                                            currentIdx = 0;

                                        // Write the current character
                                        TextWriterColor.WriteColorBack(sample[currentIdx].ToString(), false, selectedColor, black);

                                        // Reset position
                                        ConsoleWrapper.CursorLeft = 0;
                                        ConsoleWrapper.CursorTop = 0;

                                        // Declare as done
                                        printDone = true;
                                    }
                                    else
                                    {
                                        // We're at the end. Increment the index or reset to zero
                                        currentIdx++;
                                        if (currentIdx > sample.Length - 1)
                                            currentIdx = 0;

                                        // Write the current character
                                        TextWriterColor.WriteColorBack(sample[currentIdx].ToString(), false, selectedColor, black);
                                        TextWriterRaw.Write();
                                    }
                                }
                            }
                        }

                        // Print the big 2018
                        var s4figFont = FigletTools.GetFigletFont("banner");
                        int s4figWidth = FigletTools.GetFigletWidth(year, s4figFont) / 2;
                        int s4figHeight = FigletTools.GetFigletHeight(year, s4figFont) / 2;
                        int s4consoleX = ConsoleWrapper.WindowWidth / 2 - s4figWidth;
                        int s4consoleY = ConsoleWrapper.WindowHeight / 2 - s4figHeight;
                        FigletWhereColor.WriteFigletWhereColorBack(year, s4consoleX, s4consoleY, true, s4figFont, selectedColor, black);
                        ThreadManager.SleepNoBlock(5000, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        break;
                    // Step 12: Fade the console from the color to black
                    case 12:
                        // Fade the console out
                        colorSteps = 30;

                        // Get the color thresholds
                        thresholdR = selectedColor.RGB.R / (double)colorSteps;
                        thresholdG = selectedColor.RGB.G / (double)colorSteps;
                        thresholdB = selectedColor.RGB.B / (double)colorSteps;

                        // Now, transition from target color to black
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(selectedColor.RGB.R - thresholdR * currentStep);
                            currentG = (int)Math.Round(selectedColor.RGB.G - thresholdG * currentStep);
                            currentB = (int)Math.Round(selectedColor.RGB.B - thresholdB * currentStep);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            ColorTools.LoadBackDry(col);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    case 13:
                        string tbc = Translate.DoTranslation("To be continued...").ToUpper();
                        ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, green, black);
                        ThreadManager.SleepNoBlock(40, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, black, black);
                        ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, green, black);
                        ThreadManager.SleepNoBlock(50, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, black, black);
                        ThreadManager.SleepNoBlock(1000, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, green, black);
                        ThreadManager.SleepNoBlock(5000, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        break;
                }
            }

            // Reset
            ConsoleResizeHandler.WasResized();
            ColorTools.LoadBackDry(black);
        }

    }
}
