﻿
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Threading;
using KS.Kernel.Debugging;
using KS.ConsoleBase;
using KS.Languages;
using KS.Kernel;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using Terminaux.Colors;
using Terminaux.Sequences.Tools;
using KS.ConsoleBase.Colors;
using Figletize;
using System;

namespace KS.Misc.Splash.Splashes
{
    class SplashWelcome : BaseSplash, ISplash
    {

        // Standalone splash information
        public override string SplashName => "Welcome";

        public override bool SplashDisplaysProgress => true;

        // Actual logic
        public override void Opening()
        {
            base.Opening();

            // Write a glorious Welcome screen
            Color col = KernelColorTools.GetColor(KernelColorType.Stage);
            string text = Translate.DoTranslation("Loading").ToUpper();
            var figFont = FigletTools.GetFigletFont("banner3");
            int figWidth = FigletTools.GetFigletWidth(text, figFont) / 2;
            int figHeight = FigletTools.GetFigletHeight(text, figFont) / 2;
            int consoleX, consoleY;
            if (figWidth >= ConsoleWrapper.WindowWidth || figHeight >= ConsoleWrapper.WindowHeight)
            {
                // The figlet won't fit, so use small text
                consoleX = (ConsoleWrapper.WindowWidth / 2) - (text.Length / 2);
                consoleY = ConsoleWrapper.WindowHeight / 2;
                TextWriterWhereColor.WriteWhere(text, consoleX, consoleY, true, col);
            }
            else
            {
                // Write the figlet.
                consoleX = (ConsoleWrapper.WindowWidth / 2) - figWidth;
                consoleY = (ConsoleWrapper.WindowHeight / 2) - figHeight;
                FigletWhereColor.WriteFigletWhere(text, consoleX, consoleY, true, figFont, col);
                consoleY += figHeight * 2;
            }
            CenteredTextColor.WriteCentered(consoleY + 2, Translate.DoTranslation("Starting") + $" {KernelReleaseInfo.ConsoleTitle}...", col);
        }

        public override void Display()
        {
            try
            {
                Color firstColor = KernelColorTools.GetColor(KernelColorType.Background).IsBright ? new(ConsoleColors.Black) : new(ConsoleColors.White);
                Color secondColor = KernelColorTools.GetColor(KernelColorType.Success);
                int dotStep = 0;
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                while (!SplashClosing)
                {
                    Color firstDotColor  = dotStep >= 1 ? secondColor : firstColor;
                    Color secondDotColor = dotStep >= 2 ? secondColor : firstColor;
                    Color thirdDotColor  = dotStep >= 3 ? secondColor : firstColor;

                    // Write the three dots
                    string dots = $"{firstDotColor.VTSequenceForeground}* {secondDotColor.VTSequenceForeground}* {thirdDotColor.VTSequenceForeground}*";
                    int dotsPosX = (ConsoleWrapper.WindowWidth / 2) - (VtSequenceTools.FilterVTSequences(dots).Length / 2);
                    int dotsPosY = ConsoleWrapper.WindowHeight - 2;
                    TextWriterWhereColor.WriteWhere(dots, dotsPosX, dotsPosY);
                    Thread.Sleep(500);
                    dotStep++;
                    if (dotStep > 3)
                        dotStep = 0;
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
        }

        public override void Closing()
        {
            ConsoleWrapper.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing...");

            // Write a glorious Welcome screen
            Color col = KernelColorTools.GetColor(KernelColorType.Stage);
            string text = Translate.DoTranslation("Welcome!").ToUpper();
            var figFont = FigletTools.GetFigletFont("banner3");
            var figFontFallback = FigletTools.GetFigletFont("small");
            int figWidth = FigletTools.GetFigletWidth(text, figFont) / 2;
            int figHeight = FigletTools.GetFigletHeight(text, figFont) / 2;
            int figWidthFallback = FigletTools.GetFigletWidth(text, figFontFallback) / 2;
            int figHeightFallback = FigletTools.GetFigletHeight(text, figFontFallback) / 2;
            int consoleX = (ConsoleWrapper.WindowWidth / 2) - figWidth;
            int consoleY = (ConsoleWrapper.WindowHeight / 2) - figHeight;
            if (consoleX < 0 || consoleY < 0)
            {
                // The figlet won't fit, so use small text
                consoleX = (ConsoleWrapper.WindowWidth / 2) - figWidthFallback;
                consoleY = (ConsoleWrapper.WindowHeight / 2) - figHeightFallback;
                if (consoleX < 0 || consoleY < 0)
                {
                    // The fallback figlet also won't fit, so use smaller text
                    consoleX = (ConsoleWrapper.WindowWidth / 2) - (text.Length / 2);
                    consoleY = ConsoleWrapper.WindowHeight / 2;
                    TextWriterWhereColor.WriteWhere(text, consoleX, consoleY, true, col);
                }
                else
                {
                    // Write the figlet.
                    FigletWhereColor.WriteFigletWhere(text, consoleX, consoleY, true, figFontFallback, col);
                    consoleY += figHeightFallback * 2;
                }
            }
            else
            {
                // Write the figlet.
                FigletWhereColor.WriteFigletWhere(text, consoleX, consoleY, true, figFont, col);
                consoleY += figHeight * 2;
            }
            consoleX = (ConsoleWrapper.WindowWidth / 2) - (KernelReleaseInfo.ConsoleTitle.Length / 2);
            TextWriterWhereColor.WriteWhere(KernelReleaseInfo.ConsoleTitle, consoleX, consoleY + 2, true, col);
            Thread.Sleep(3000);

            // Clear the console
            ConsoleWrapper.Clear();
        }

        public override void Report(int Progress, string ProgressReport, params object[] Vars)
        {
            Color col = KernelColorTools.GetColor(KernelColorType.Stage);
            string text = Translate.DoTranslation("Welcome!").ToUpper();
            var figFont = FigletTools.GetFigletFont("banner3");
            int figHeight = FigletTools.GetFigletHeight(text, figFont) / 2;
            int consoleX = (ConsoleWrapper.WindowWidth / 2) - (ProgressReport.Length / 2);
            int consoleY = (ConsoleWrapper.WindowHeight / 2) - figHeight;
            TextWriterWhereColor.WriteWhere(ConsoleExtensions.GetClearLineToRightSequence(), 0, consoleY - 2, true, col, Vars);
            TextWriterWhereColor.WriteWhere(ProgressReport, consoleX, consoleY - 2, true, col, Vars);
        }

        public override void ReportWarning(int Progress, string WarningReport, Exception ExceptionInfo, params object[] Vars) =>
            Report(Progress, WarningReport, Vars);

        public override void ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars) =>
            Report(Progress, ErrorReport, Vars);

    }
}
