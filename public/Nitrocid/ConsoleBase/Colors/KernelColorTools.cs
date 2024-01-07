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
using Nitrocid.ConsoleBase.Themes;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Terminaux.Colors;
using Terminaux.Colors.Transformation;

namespace Nitrocid.ConsoleBase.Colors
{
    /// <summary>
    /// Color tools module
    /// </summary>
    public static class KernelColorTools
    {

        // Variables for colors used by previous versions of the kernel.
        internal static Dictionary<KernelColorType, Color> KernelColors = PopulateColorsDefault();

        // Variables for accent background and foreground colors
        internal static Color accentForegroundColor = GetColor(KernelColorType.Warning);
        internal static Color accentBackgroundColor = GetColor(KernelColorType.Background);

        /// <summary>
        /// Enables color blindness
        /// </summary>
        public static bool ColorBlind
        {
            get => Config.MainConfig.ColorBlind;
            set => Config.MainConfig.ColorBlind = value;
        }

        /// <summary>
        /// Enables simple color blindness using the Vienot 1999 formula (may not be accurate for tritanopia)
        /// </summary>
        public static bool ColorBlindSimple
        {
            get => Config.MainConfig.ColorBlindSimple;
            set => Config.MainConfig.ColorBlindSimple = value;
        }

        /// <summary>
        /// Color blindness transformation formula
        /// </summary>
        public static TransformationFormula BlindnessTransformationFormula
        {
            get => (TransformationFormula)Config.MainConfig.BlindnessTransformationFormula;
            set => Config.MainConfig.BlindnessTransformationFormula = (int)value;
        }

        /// <summary>
        /// Color blindness severity
        /// </summary>
        public static double BlindnessSeverity
        {
            get => Config.MainConfig.BlindnessSeverity;
            set => Config.MainConfig.BlindnessSeverity = value;
        }

        /// <summary>
        /// Gets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        public static Color GetColor(KernelColorType type)
        {
            string plainColorSeq = KernelColors[type].PlainSequence;
            DebugWriter.WriteDebug(DebugLevel.I, "Getting color type {0}: {1}", type.ToString(), plainColorSeq);
            return new(plainColorSeq);
        }

        /// <summary>
        /// Sets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        /// <param name="color">Color to be set</param>
        public static Color SetColor(KernelColorType type, Color color)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Setting color type {0} to color sequence {1}...", type.ToString(), color.PlainSequence);
            return KernelColors[type] = color;
        }

        /// <summary>
        /// Populate the empty color dictionary
        /// </summary>
        public static Dictionary<KernelColorType, Color> PopulateColorsEmpty() =>
            PopulateColors(KernelColorPopulationType.Empty);

        /// <summary>
        /// Populate the default color dictionary
        /// </summary>
        public static Dictionary<KernelColorType, Color> PopulateColorsDefault() =>
            PopulateColors(KernelColorPopulationType.Default);

        /// <summary>
        /// Populate the current color dictionary
        /// </summary>
        public static Dictionary<KernelColorType, Color> PopulateColorsCurrent() =>
            PopulateColors(KernelColorPopulationType.Current);

        private static Dictionary<KernelColorType, Color> PopulateColors(KernelColorPopulationType populationType)
        {
            Dictionary<KernelColorType, Color> colors = [];
            ThemeInfo themeInfo = default;

            // Select population type
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(KernelColorType)).Length; typeIndex++)
            {
                // Necessary variables
                KernelColorType type = KernelColorType.NeutralText;
                Color color = Color.Empty;

                // Now, change the two above variables depending on the type.
                switch (populationType)
                {
                    case KernelColorPopulationType.Empty:
                        // Population type is empty colors
                        type = (KernelColorType)Enum.Parse(typeof(KernelColorType), typeIndex.ToString());
                        color = type != KernelColorType.Background ? new Color(ConsoleColors.White) : Color.Empty;
                        break;
                    case KernelColorPopulationType.Default:
                        // Population type is default colors
                        themeInfo ??= new();
                        type = (KernelColorType)Enum.Parse(typeof(KernelColorType), typeIndex.ToString());
                        color = themeInfo.GetColor(type);
                        DebugWriter.WriteDebug(DebugLevel.I, "[DEFAULT] Adding color type {0} with color {1}...", type, color.PlainSequence);
                        break;
                    case KernelColorPopulationType.Current:
                        // Population type is current colors
                        type = (KernelColorType)Enum.Parse(typeof(KernelColorType), typeIndex.ToString());
                        color = GetColor(type);
                        DebugWriter.WriteDebug(DebugLevel.I, "[CURRENT] Adding color type {0} with color {1}...", type, color.PlainSequence);
                        break;
                }
                colors.Add(type, color);
            }

            // Return it
            DebugWriter.WriteDebug(DebugLevel.I, "Populated {0} colors.", colors.Count);
            return colors;
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="resetBack">If the color is not a background, do we reset the background color?</param>
        public static void SetConsoleColor(KernelColorType colorType, bool resetBack = true) =>
            SetConsoleColor(colorType, false, resetBack);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <param name="resetBack">If the color is not a background, do we reset the background color?</param>
        public static void SetConsoleColor(KernelColorType colorType, bool Background, bool resetBack = true)
        {
            ColorTools.SetConsoleColor(GetColor(colorType), Background);
            if (!Background && resetBack)
                ColorTools.SetConsoleColor(GetColor(KernelColorType.Background), true);
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(KernelColorType colorType) =>
            TrySetConsoleColor(colorType, false);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(KernelColorType colorType, bool Background)
        {
            try
            {
                SetConsoleColor(colorType, Background);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
