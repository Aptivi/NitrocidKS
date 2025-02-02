﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using System.Threading;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Kernel.Events;
using Terminaux.Base;
using Terminaux.Colors;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Misc.Audio;
using BassBoom.Basolia;
using BassBoom.Basolia.File;
using BassBoom.Basolia.Playback;

namespace Nitrocid.Misc.Screensaver
{
    /// <summary>
    /// Screensaver display module
    /// </summary>
    internal static class ScreensaverDisplayer
    {

        internal readonly static KernelThread ScreensaverDisplayerThread = new("Screensaver display thread", false, (ss) => DisplayScreensaver((BaseScreensaver?)ss));
        internal readonly static KernelThread ScreensaverAmbienceThread = new("Screensaver ambience thread", false, ScreensaverAmbience);

        internal static BaseScreensaver? displayingSaver;

        /// <summary>
        /// Displays the screensaver from the screensaver base
        /// </summary>
        /// <param name="Screensaver">Screensaver base containing information about the screensaver</param>
        internal static void DisplayScreensaver(BaseScreensaver? Screensaver)
        {
            if (Screensaver is null)
                throw new KernelException(KernelExceptionType.ScreensaverManagement, Translate.DoTranslation("Screensaver instance is not specified"));
            bool initialVisible = ConsoleWrapper.CursorVisible;
            bool initialBack = ColorTools.AllowBackground;
            bool initialPalette = ColorTools.GlobalSettings.UseTerminalPalette;
            try
            {
                // Preparations
                displayingSaver = Screensaver;
                ColorTools.AllowBackground = true;
                ColorTools.GlobalSettings.UseTerminalPalette = false;
                Screensaver.ScreensaverPreparation();

                // Play the ambience sound if needed
                if (Config.MainConfig.EnableAmbientSoundFx)
                    ScreensaverAmbienceThread.Start();

                // Execute the actual screensaver logic
                while (!ScreensaverDisplayerThread.IsStopping)
                {
                    if (ConsoleWrapper.CursorVisible)
                        ConsoleWrapper.CursorVisible = false;
                    Screensaver.ScreensaverLogic();
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Screensaver is stopping due to user request...");
            }
            catch (Exception ex)
            {
                ScreensaverManager.HandleSaverError(ex, initialVisible);
            }
            finally
            {
                Screensaver.ScreensaverOutro();
                ScreensaverManager.HandleSaverCancel(initialVisible);
                ColorTools.AllowBackground = initialBack;
                ColorTools.GlobalSettings.UseTerminalPalette = initialPalette;
                KernelColorTools.LoadBackground();
            }
        }

        internal static void BailFromScreensaver()
        {
            if (ScreensaverManager.InSaver)
            {
                ScreensaverDisplayerThread.Stop(false);
                ScreensaverManager.SaverAutoReset.WaitOne();

                // Raise event
                DebugWriter.WriteDebug(DebugLevel.I, "Screensaver really stopped.");
                EventsManager.FireEvent(EventType.PostShowScreensaver);
                ScreensaverManager.inSaver = false;
                ScreensaverManager.ScrnTimeReached = false;
                ScreensaverDisplayerThread.Regen();
            }
        }

        internal static void ScreensaverAmbience()
        {
            try
            {
                var basoliaMedia = new BasoliaMedia();
                DebugWriter.WriteDebug(DebugLevel.I, $"Screensaver ambience starting with theme {Config.MainConfig.AudioCueThemeName}");
                
                // Open the ambient SFX stream
                var ambientFxType = Config.MainConfig.EnableAmbientSoundFxIntense ? AudioCueType.AmbienceIdle : AudioCueType.Ambience;
                var cue = AudioCuesTools.GetAudioCue();
                FileTools.OpenFrom(basoliaMedia, cue.GetStream(ambientFxType));

                // Repeatedly play it
                while (!ScreensaverDisplayerThread.IsStopping)
                {
                    try
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, $"Restarting screensaver ambience {ambientFxType} from {Config.MainConfig.AudioCueThemeName}...");
                        PlaybackTools.PlayAsync(basoliaMedia);
                        if (!SpinWait.SpinUntil(() => PlaybackTools.GetState(basoliaMedia) == PlaybackState.Playing, 15000))
                            throw new KernelException(KernelExceptionType.AudioCue, Translate.DoTranslation("Can't play sound because of timeout."), KernelColorType.Error);
                        while (PlaybackTools.GetState(basoliaMedia) == PlaybackState.Playing && !ScreensaverDisplayerThread.IsStopping) ;
                    }
                    catch (ThreadInterruptedException)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Screensaver ambience is stopping due to user request...");
                        break;
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, $"Screensaver ambience is stopping due to failure. {ex.Message}");
                        DebugWriter.WriteDebugStackTrace(ex);
                        break;
                    }
                }
                if (PlaybackTools.GetState(basoliaMedia) != PlaybackState.Stopped)
                    PlaybackTools.Stop(basoliaMedia);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Screensaver ambience is stopping due to failure. {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }
    }
}
