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

using System.Threading;
using Terminaux.Colors;
using Terminaux.Sequences;
using Textify.Data.Figlet;
using System;
using System.Text;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Debugging;
using Terminaux.Writer.FancyWriters;
using Nitrocid.Languages;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Nitrocid.Kernel.Configuration;
using Terminaux.Colors.Transformation.Contrast;
using Terminaux.Base.Extensions;

namespace Nitrocid.Misc.Splash.Splashes
{
    class SplashWelcome : BaseSplash, ISplash
    {

        private bool cleared = false;
        private int dotStep = 0;
        private int currMs = 0;

        // Standalone splash information
        public override string SplashName => "Welcome";

        // Actual logic
        public override string Opening(SplashContext context)
        {
            var builder = new StringBuilder();
            if (ConsoleResizeHandler.WasResized(true))
                cleared = false;
            if (!cleared)
            {
                cleared = true;
                builder.Append(
                    base.Opening(context)
                );
            }

            // Populate some text
            string text =
                (context == SplashContext.Preboot ?
                 Translate.DoTranslation("Please wait") :
                 Translate.DoTranslation("Loading"))
                .ToUpper();
            string bottomText =
                context == SplashContext.Preboot ? Translate.DoTranslation("Please wait while the kernel is initializing") :
                context == SplashContext.ShuttingDown ? Translate.DoTranslation("Please wait while the kernel is shutting down") :
                context == SplashContext.Rebooting ? Translate.DoTranslation("Please wait while the kernel is restarting") :
                $"{Translate.DoTranslation("Starting")} {KernelReleaseInfo.ConsoleTitle}";
            bottomText +=
                KernelEntry.SafeMode ? $" - {Translate.DoTranslation("Safe Mode")}"  :
                KernelEntry.Maintenance ? $" - {Translate.DoTranslation("Maintenance Mode")}" :
                KernelEntry.DebugMode ? $" - {Translate.DoTranslation("Debug Mode")}" :
                "";

            // Write a glorious Welcome screen
            Color col = KernelColorTools.GetColor(KernelColorType.Stage);
            var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
            int consoleY = (ConsoleWrapper.WindowHeight / 2) + FigletTools.GetFigletHeight(text, figFont);
            builder.Append(
                col.VTSequenceForeground +
                CenteredFigletTextColor.RenderCenteredFiglet(figFont, text) +
                CenteredTextColor.RenderCenteredOneLine(consoleY - 1, bottomText)
            );
            return builder.ToString();
        }

        public override string Display(SplashContext context)
        {
            var builder = new StringBuilder();
            try
            {
                // Check to see if we need progress or dots
                if (!Config.SplashConfig.WelcomeShowProgress)
                {
                    Color firstColor = KernelColorTools.GetColor(KernelColorType.Background).Brightness == ColorBrightness.Light ? new(ConsoleColors.Black) : new(ConsoleColors.White);
                    Color secondColor = KernelColorTools.GetColor(KernelColorType.Success);
                    Color firstDotColor = dotStep >= 1 ? secondColor : firstColor;
                    Color secondDotColor = dotStep >= 2 ? secondColor : firstColor;
                    Color thirdDotColor = dotStep >= 3 ? secondColor : firstColor;
                    Color fourthDotColor = dotStep >= 4 ? secondColor : firstColor;
                    Color fifthDotColor = dotStep >= 5 ? secondColor : firstColor;

                    // Append a millisecond to the counter
                    bool noAppend = true;
                    currMs++;
                    if (currMs >= 10)
                    {
                        noAppend = false;
                        currMs = 0;
                    }

                    // Write the three dots
                    string dots =
                        $"{firstDotColor.VTSequenceForeground}* " +
                        $"{secondDotColor.VTSequenceForeground}* " +
                        $"{thirdDotColor.VTSequenceForeground}* " +
                        $"{fourthDotColor.VTSequenceForeground}* " +
                        $"{fifthDotColor.VTSequenceForeground}*";
                    int dotsPosX = ConsoleWrapper.WindowWidth / 2 - VtSequenceTools.FilterVTSequences(dots).Length / 2;
                    int dotsPosY = ConsoleWrapper.WindowHeight - 2;
                    builder.Append(TextWriterWhereColor.RenderWhere(dots, dotsPosX, dotsPosY));
                    if (!noAppend)
                    {
                        dotStep++;
                        if (dotStep > 5)
                            dotStep = 0;
                    }
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
            return builder.ToString();
        }

        public override string Closing(SplashContext context, out bool delayRequired)
        {
            var builder = new StringBuilder();
            currMs = 0;
            dotStep = 0;
            cleared = false;
            builder.Append(
                base.Opening(context)
            );
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing...");

            if (context == SplashContext.Showcase ||
                context == SplashContext.Preboot)
            {
                delayRequired = false;
                return builder.ToString();
            }

            // Write a glorious Welcome screen
            Color col = KernelColorTools.GetColor(KernelColorType.Stage);
            string text =
                (context == SplashContext.StartingUp ?
                 Translate.DoTranslation("Welcome!") :
                 Translate.DoTranslation("Goodbye!"))
                .ToUpper();
            var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
            int consoleY = (ConsoleWrapper.WindowHeight / 2) + FigletTools.GetFigletHeight(text, figFont);
            builder.Append(
                col.VTSequenceForeground +
                CenteredFigletTextColor.RenderCenteredFiglet(figFont, text) +
                CenteredTextColor.RenderCenteredOneLine(consoleY - 1, KernelReleaseInfo.ConsoleTitle)
            );
            delayRequired =
                context == SplashContext.ShuttingDown && Config.MainConfig.DelayOnShutdown ||
                context != SplashContext.ShuttingDown && context != SplashContext.Rebooting;
            if ((context == SplashContext.ShuttingDown || context == SplashContext.Rebooting) && Config.MainConfig.BeepOnShutdown)
                ConsoleWrapper.Beep();
            return builder.ToString();
        }

        public override string Report(int Progress, string ProgressReport, params object[] Vars) =>
            ReportProgress(Progress, ProgressReport, KernelColorType.Stage, Vars);

        public override string ReportWarning(int Progress, string WarningReport, Exception? ExceptionInfo, params object[] Vars) =>
            ReportProgress(Progress, WarningReport, KernelColorType.Warning, Vars);

        public override string ReportError(int Progress, string ErrorReport, Exception? ExceptionInfo, params object[] Vars) =>
            ReportProgress(Progress, ErrorReport, KernelColorType.Error, Vars);

        private string ReportProgress(int Progress, string ProgressReport, KernelColorType colorType, params object[] Vars)
        {
            var builder = new StringBuilder();
            Color col = KernelColorTools.GetColor(colorType);
            string text =
                (SplashManager.CurrentSplashContext == SplashContext.StartingUp ?
                 Translate.DoTranslation("Welcome!") :
                 Translate.DoTranslation("Goodbye!"))
                .ToUpper();
            var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
            int figHeight = FigletTools.GetFigletHeight(text, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            builder.Append(
                col.VTSequenceForeground +
                TextWriterWhereColor.RenderWhere(ConsoleClearing.GetClearLineToRightSequence(), 0, consoleY - 2, true, Vars) +
                CenteredTextColor.RenderCenteredOneLine(consoleY - 2, $"{Progress}% - {ProgressReport}", Vars: Vars)
            );

            if (Config.SplashConfig.WelcomeShowProgress)
            {
                int posX = 2;
                int posY = ConsoleWrapper.WindowHeight - 4;
                builder.Append(
                    ProgressBarColor.RenderProgressPlain(Progress, posX, posY, ConsoleWrapper.WindowWidth - 6)
                );
            }
            return builder.ToString();
        }

    }
}
