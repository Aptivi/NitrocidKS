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
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Configuration;
using KS.Misc.Screensaver.Customized;
using KS.Misc.Screensaver.Displays;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace KS.Misc.Screensaver
{
    public static class Screensaver
    {

        // Public Variables
        public static bool LockMode;
        public static bool InSaver;
        public static bool ScreensaverDebug;
        public static string DefSaverName = "matrix";
        public static int ScrnTimeout = 300000;
        public static bool PasswordLock = true;
        public static readonly ConsoleColor[] colors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));        // 15 Console Colors
        public static readonly ConsoleColors[] colors255 = (ConsoleColors[])Enum.GetValues(typeof(ConsoleColors));  // 255 Console Colors

        // Private variables
        internal static Dictionary<string, BaseScreensaver> Screensavers = new()
        {
            { "matrixbleed", new MatrixBleedDisplay() },
            { "plain", new PlainDisplay() },
            { "analogclock", new AnalogClockDisplay() },
            { "aurora", new AuroraDisplay() },
            { "barrot", new BarRotDisplay() },
            { "barwave", new BarWaveDisplay() },
            { "beatfader", new BeatFaderDisplay() },
            { "beatpulse", new BeatPulseDisplay() },
            { "beatedgepulse", new BeatEdgePulseDisplay() },
            { "bloom", new BloomDisplay() },
            { "bouncingblock", new BouncingBlockDisplay() },
            { "bouncingtext", new BouncingTextDisplay() },
            { "boxgrid", new BoxGridDisplay() },
            { "bsod", new BSODDisplay() },
            { "colorbleed", new ColorBleedDisplay() },
            { "colormix", new ColorMixDisplay() },
            { "dancelines", new DanceLinesDisplay() },
            { "dateandtime", new DateAndTimeDisplay() },
            { "diamond", new DiamondDisplay() },
            { "disco", new DiscoDisplay() },
            { "dissolve", new DissolveDisplay() },
            { "doorshift", new DoorShiftDisplay() },
            { "edgepulse", new EdgePulseDisplay() },
            { "equalizer", new EqualizerDisplay() },
            { "excalibeats", new ExcaliBeatsDisplay() },
            { "fader", new FaderDisplay() },
            { "faderback", new FaderBackDisplay() },
            { "fallingline", new FallingLineDisplay() },
            { "figlet", new FigletDisplay() },
            { "fillfade", new FillFadeDisplay() },
            { "fireworks", new FireworksDisplay() },
            { "flashcolor", new FlashColorDisplay() },
            { "flashtext", new FlashTextDisplay() },
            { "glitch", new GlitchDisplay() },
            { "glittercolor", new GlitterColorDisplay() },
            { "glittermatrix", new GlitterMatrixDisplay() },
            { "gradient", new GradientDisplay() },
            { "gradientbloom", new GradientBloomDisplay() },
            { "gradientrot", new GradientRotDisplay() },
            { "hueback", new HueBackDisplay() },
            { "huebackgradient", new HueBackGradientDisplay() },
            { "indeterminate", new IndeterminateDisplay() },
            { "ksx", new KSXDisplay() },
            { "ksx2", new KSX2Display() },
            { "ksx3", new KSX3Display() },
            { "ksxtheend", new KSXTheEndDisplay() },
            { "laserbeams", new LaserBeamsDisplay() },
            { "letterscatter", new LetterScatterDisplay() },
            { "lighter", new LighterDisplay() },
            { "lightning", new LightningDisplay() },
            { "lines", new LinesDisplay() },
            { "linotypo", new LinotypoDisplay() },
            { "marquee", new MarqueeDisplay() },
            { "matrix", new MatrixDisplay() },
            { "mazer", new MazerDisplay() },
            { "memdump", new MemdumpDisplay() },
            { "mesmerize", new MesmerizeDisplay() },
            { "multilines", new MultiLinesDisplay() },
            { "newyear", new NewYearDisplay() },
            { "noise", new NoiseDisplay() },
            { "numberscatter", new NumberScatterDisplay() },
            { "particles", new ParticlesDisplay() },
            { "progressclock", new ProgressClockDisplay() },
            { "pulse", new PulseDisplay() },
            { "ramp", new RampDisplay() },
            { "simplematrix", new SimpleMatrixDisplay() },
            { "siren", new SirenDisplay() },
            { "skycomet", new SkyCometDisplay() },
            { "snakefill", new SnakeFillDisplay() },
            { "speckles", new SpecklesDisplay() },
            { "spin", new SpinDisplay() },
            { "spotwrite", new SpotWriteDisplay() },
            { "spray", new SprayDisplay() },
            { "squarecorner", new SquareCornerDisplay() },
            { "stackbox", new StackBoxDisplay() },
            { "starfield", new StarfieldDisplay() },
            { "starfieldwarp", new StarfieldWarpDisplay() },
            { "swivel", new SwivelDisplay() },
            { "text", new TextDisplay() },
            { "textbox", new TextBoxDisplay() },
            { "textwander", new TextWanderDisplay() },
            { "twospins", new TwoSpinsDisplay() },
            { "typewriter", new TypewriterDisplay() },
            { "typo", new TypoDisplay() },
            { "wave", new WaveDisplay() },
            { "windowslogo", new WindowsLogoDisplay() },
            { "wipe", new WipeDisplay() },
            { "wordhasher", new WordHasherDisplay() },
            { "wordhasherwrite", new WordHasherWriteDisplay() },
            { "zebrashift", new ZebraShiftDisplay() },
        };
        internal static AutoResetEvent SaverAutoReset = new(false);

        /// <summary>
        /// Shows the screensaver
        /// </summary>
        /// <param name="saver">A specified screensaver</param>
        public static void ShowSavers(string saver)
        {
            try
            {
                InSaver = true;
                Flags.ScrnTimeReached = true;
                Kernel.Kernel.KernelEventManager.RaisePreShowScreensaver(saver);
                DebugWriter.Wdbg(DebugLevel.I, "Requested screensaver: {0}", saver);
                if (Screensavers.ContainsKey(saver.ToLower()))
                {
                    saver = saver.ToLower();
                    var BaseSaver = Screensavers[saver];
                    ScreensaverDisplayer.ScreensaverDisplayerThread.Start(BaseSaver);
                    DebugWriter.Wdbg(DebugLevel.I, "{0} started", saver);
                    Input.DetectKeypress();
                    ScreensaverDisplayer.ScreensaverDisplayerThread.Stop();
                    SaverAutoReset.WaitOne();
                }
                else if (CustomSaverTools.CustomSavers.ContainsKey(saver))
                {
                    // Only one custom screensaver can be used.
                    ScreensaverDisplayer.ScreensaverDisplayerThread.Start(new CustomDisplay(CustomSaverTools.CustomSavers[saver].ScreensaverBase));
                    DebugWriter.Wdbg(DebugLevel.I, "Custom screensaver {0} started", saver);
                    Input.DetectKeypress();
                    ScreensaverDisplayer.ScreensaverDisplayerThread.Stop();
                    SaverAutoReset.WaitOne();
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("The requested screensaver {0} is not found."), true, KernelColorTools.ColTypes.Error, saver);
                    DebugWriter.Wdbg(DebugLevel.I, "Screensaver {0} not found in the dictionary.", saver);
                }

                // Raise event
                DebugWriter.Wdbg(DebugLevel.I, "Screensaver really stopped.");
                Kernel.Kernel.KernelEventManager.RaisePostShowScreensaver(saver);
            }
            catch (InvalidOperationException ex)
            {
                TextWriters.Write(Translate.DoTranslation("Error when trying to start screensaver, because of an invalid operation."), true, KernelColorTools.ColTypes.Error);
                DebugWriter.WStkTrc(ex);
            }
            catch (Exception ex)
            {
                TextWriters.Write(Translate.DoTranslation("Error when trying to start screensaver:") + " {0}", true, KernelColorTools.ColTypes.Error, ex.Message);
                DebugWriter.WStkTrc(ex);
            }
            finally
            {
                InSaver = false;
                Flags.ScrnTimeReached = false;
            }
        }

        /// <summary>
        /// Locks the screen. The password will be required when unlocking, depending on the kernel settings.
        /// </summary>
        public static void LockScreen()
        {
            LockMode = true;
            ShowSavers(DefSaverName);
            Kernel.Kernel.KernelEventManager.RaisePreUnlock(DefSaverName);
            if (PasswordLock)
            {
                Login.Login.ShowPasswordPrompt(Login.Login.CurrentUser.Username);
            }
            else
            {
                LockMode = false;
            }
        }

        /// <summary>
        /// Sets the default screensaver
        /// </summary>
        /// <param name="saver">Specified screensaver</param>
        public static void SetDefaultScreensaver(string saver)
        {
            saver = saver.ToLower();
            if (Screensavers.ContainsKey(saver) | CustomSaverTools.CustomSavers.ContainsKey(saver))
            {
                DebugWriter.Wdbg(DebugLevel.I, "{0} is found. Setting it to default...", saver);
                DefSaverName = saver;
                var Token = ConfigTools.GetConfigCategory(Config.ConfigCategory.Screensaver);
                ConfigTools.SetConfigValue(Config.ConfigCategory.Screensaver, Token, "Screensaver", saver);
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.W, "{0} is not found.", saver);
                throw new Kernel.Exceptions.NoSuchScreensaverException(Translate.DoTranslation("Screensaver {0} not found in database. Check the name and try again."), saver);
            }
        }

        /// <summary>
        /// Gets a screensaver instance from loaded assembly
        /// </summary>
        /// <param name="Assembly">An assembly</param>
        public static BaseScreensaver GetScreensaverInstance(Assembly Assembly)
        {
            foreach (Type t in Assembly.GetTypes())
            {
                if (t.GetInterface(typeof(IScreensaver).Name) is not null)
                    return (BaseScreensaver)Assembly.CreateInstance(t.FullName);
            }
            return null;
        }

        /// <summary>
        /// Screensaver error handler
        /// </summary>
        internal static void HandleSaverError(Exception Exception)
        {
            if (Exception is not null)
            {
                DebugWriter.Wdbg(DebugLevel.W, "Screensaver experienced an error: {0}.", Exception.Message);
                DebugWriter.WStkTrc(Exception);
                HandleSaverCancel();
                TextWriters.Write(Translate.DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), true, KernelColorTools.ColTypes.Error, Exception.Message);
            }
        }

        /// <summary>
        /// Screensaver cancellation handler
        /// </summary>
        internal static void HandleSaverCancel()
        {
            DebugWriter.Wdbg(DebugLevel.W, "Cancellation is pending. Cleaning everything up...");
            KernelColorTools.LoadBack();
            ConsoleWrapper.CursorVisible = true;
            DebugWriter.Wdbg(DebugLevel.I, "All clean. Screensaver stopped.");
            SaverAutoReset.Set();
        }

    }
}
