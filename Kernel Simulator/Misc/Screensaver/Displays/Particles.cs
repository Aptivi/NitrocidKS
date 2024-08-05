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

using Terminaux.Writer.ConsoleWriters;
using KS.Misc.Reflection;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Particles
    /// </summary>
    public static class ParticlesSettings
    {
        private static bool particlesTrueColor = true;
        private static int particlesDelay = 1;
        private static int particlesDensity = 25;
        private static int particlesMinimumRedColorLevel = 0;
        private static int particlesMinimumGreenColorLevel = 0;
        private static int particlesMinimumBlueColorLevel = 0;
        private static int particlesMinimumColorLevel = 0;
        private static int particlesMaximumRedColorLevel = 255;
        private static int particlesMaximumGreenColorLevel = 255;
        private static int particlesMaximumBlueColorLevel = 255;
        private static int particlesMaximumColorLevel = 255;

        /// <summary>
        /// [Particles] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool ParticlesTrueColor
        {
            get
            {
                return particlesTrueColor;
            }
            set
            {
                particlesTrueColor = value;
            }
        }
        /// <summary>
        /// [Particles] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int ParticlesDelay
        {
            get
            {
                return particlesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                particlesDelay = value;
            }
        }
        /// <summary>
        /// [Particles] How dense are the particles?
        /// </summary>
        public static int ParticlesDensity
        {
            get
            {
                return particlesDensity;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                particlesDensity = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum red color level (true color)
        /// </summary>
        public static int ParticlesMinimumRedColorLevel
        {
            get
            {
                return particlesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                particlesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum green color level (true color)
        /// </summary>
        public static int ParticlesMinimumGreenColorLevel
        {
            get
            {
                return particlesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                particlesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum blue color level (true color)
        /// </summary>
        public static int ParticlesMinimumBlueColorLevel
        {
            get
            {
                return particlesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                particlesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ParticlesMinimumColorLevel
        {
            get
            {
                return particlesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                particlesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum red color level (true color)
        /// </summary>
        public static int ParticlesMaximumRedColorLevel
        {
            get
            {
                return particlesMaximumRedColorLevel;
            }
            set
            {
                if (value <= particlesMinimumRedColorLevel)
                    value = particlesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                particlesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum green color level (true color)
        /// </summary>
        public static int ParticlesMaximumGreenColorLevel
        {
            get
            {
                return particlesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= particlesMinimumGreenColorLevel)
                    value = particlesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                particlesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum blue color level (true color)
        /// </summary>
        public static int ParticlesMaximumBlueColorLevel
        {
            get
            {
                return particlesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= particlesMinimumBlueColorLevel)
                    value = particlesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                particlesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ParticlesMaximumColorLevel
        {
            get
            {
                return particlesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= particlesMinimumColorLevel)
                    value = particlesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                particlesMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Particles
    /// </summary>
    public class ParticlesDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Particles";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Get how many particles we need to draw
            int cells = ConsoleWrapper.WindowWidth * ConsoleWrapper.WindowHeight;
            int particlesNeeded = cells / ParticlesSettings.ParticlesDensity;
            var particlesBuffer = new StringBuilder();
            for (int i = 0; i < particlesNeeded; i++)
            {
                // Make a particle color
                Color ColorStorage;
                if (ParticlesSettings.ParticlesTrueColor)
                {
                    int RedColorNum = RandomDriver.Random(ParticlesSettings.ParticlesMinimumRedColorLevel, ParticlesSettings.ParticlesMaximumRedColorLevel);
                    int GreenColorNum = RandomDriver.Random(ParticlesSettings.ParticlesMinimumGreenColorLevel, ParticlesSettings.ParticlesMaximumGreenColorLevel);
                    int BlueColorNum = RandomDriver.Random(ParticlesSettings.ParticlesMinimumBlueColorLevel, ParticlesSettings.ParticlesMaximumBlueColorLevel);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                    ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                    particlesBuffer.Append(ColorStorage.VTSequenceBackgroundTrueColor);
                }
                else
                {
                    int ColorNum = RandomDriver.Random(ParticlesSettings.ParticlesMinimumColorLevel, ParticlesSettings.ParticlesMaximumColorLevel);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                    ColorStorage = new Color(ColorNum);
                    particlesBuffer.Append(ColorStorage.VTSequenceBackgroundTrueColor);
                }

                // Select position to draw the particles
                int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
                particlesBuffer.Append(CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 1));
                particlesBuffer.Append(' ');
            }
            TextWriterRaw.WritePlain(particlesBuffer.ToString(), false);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(ParticlesSettings.ParticlesDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            ColorTools.LoadBack();
        }

    }
}
