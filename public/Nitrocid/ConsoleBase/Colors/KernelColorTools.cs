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

using System;
using System.Collections.Generic;
using KS.ConsoleBase.Themes;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using Terminaux.Colors;
using Terminaux.Colors.Accessibility;

namespace KS.ConsoleBase.Colors
{
    /// <summary>
    /// Color tools module
    /// </summary>
    public static class KernelColorTools
    {

        // Variables for colors used by previous versions of the kernel.
        internal static Dictionary<KernelColorType, Color> KernelColors = PopulateColorsDefault();

        // Cache variables for background and foreground colors
        internal static Color currentForegroundColor = new(ConsoleColors.White);
        internal static Color currentBackgroundColor = Color.Empty;

        /// <summary>
        /// Enables color blindness
        /// </summary>
        public static bool ColorBlind =>
            Config.MainConfig.ColorBlind;

        /// <summary>
        /// Enables simple color blindness using the Vienot 1999 formula (may not be accurate for tritanopia)
        /// </summary>
        public static bool ColorBlindSimple =>
            Config.MainConfig.ColorBlindSimple;

        /// <summary>
        /// Color blindness deficiency
        /// </summary>
        public static Deficiency BlindnessDeficiency =>
            (Deficiency)Config.MainConfig.BlindnessDeficiency;

        /// <summary>
        /// Color blindness severity
        /// </summary>
        public static double BlindnessSeverity =>
            Config.MainConfig.BlindnessSeverity;

        /// <summary>
        /// Current foreground color
        /// </summary>
        public static Color CurrentForegroundColor =>
            currentForegroundColor;

        /// <summary>
        /// Current background color
        /// </summary>
        public static Color CurrentBackgroundColor =>
            currentBackgroundColor;

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
            PopulateColors(0);

        /// <summary>
        /// Populate the default color dictionary
        /// </summary>
        public static Dictionary<KernelColorType, Color> PopulateColorsDefault() =>
            PopulateColors(1);

        /// <summary>
        /// Populate the current color dictionary
        /// </summary>
        public static Dictionary<KernelColorType, Color> PopulateColorsCurrent() =>
            PopulateColors(2);

        private static Dictionary<KernelColorType, Color> PopulateColors(int populationType)
        {
            Dictionary<KernelColorType, Color> colors = new();

            // Select population type
            switch (populationType)
            {
                case 0:
                    // Population type is empty colors
                    for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(KernelColorType)).Length; typeIndex++)
                    {
                        KernelColorType type = (KernelColorType)Enum.Parse(typeof(KernelColorType), typeIndex.ToString());
                        Color color = type != KernelColorType.Background ? new Color(ConsoleColors.White) : Color.Empty;
                        colors.Add(type, color);
                    }
                    break;
                case 1:
                    // Population type is default colors
                    ThemeInfo themeInfo = new();
                    for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(KernelColorType)).Length; typeIndex++)
                    {
                        KernelColorType type = (KernelColorType)Enum.Parse(typeof(KernelColorType), typeIndex.ToString());
                        Color color = themeInfo.GetColor(type);
                        DebugWriter.WriteDebug(DebugLevel.I, "Adding color type {0} with color {1}...", type, color.PlainSequence);
                        colors.Add(type, color);
                    }
                    break;
                case 2:
                    // Population type is current colors
                    for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(KernelColorType)).Length; typeIndex++)
                    {
                        KernelColorType type = (KernelColorType)Enum.Parse(typeof(KernelColorType), typeIndex.ToString());
                        Color color = GetColor(type);
                        DebugWriter.WriteDebug(DebugLevel.I, "Adding color type {0} with color {1}...", type, color.PlainSequence);
                        colors.Add(type, color);
                    }
                    break;
            }

            // Return it
            DebugWriter.WriteDebug(DebugLevel.I, "Populated {0} colors.", colors.Count);
            return colors;
        }

        /// <summary>
        /// Loads the background
        /// </summary>
        public static void LoadBack() =>
            LoadBack(GetColor(KernelColorType.Background));

        /// <summary>
        /// Loads the background
        /// </summary>
        /// <param name="ColorSequence">Color sequence used to load background</param>
        public static void LoadBack(Color ColorSequence)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Filling background with background color {0}", ColorSequence.PlainSequence);
                SetConsoleColor(ColorSequence, true);
                ConsoleWrapper.Clear();
                DebugWriter.WriteDebug(DebugLevel.I, "Set background color!");
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to set background: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Gets the gray color according to the brightness of the background color
        /// </summary>
        public static Color GetGray()
        {
            if (GetColor(KernelColorType.Background).IsBright)
            {
                var color = GetColor(KernelColorType.NeutralText);
                DebugWriter.WriteDebug(DebugLevel.I, "Background color is bright! Returning neutral text color {0}...", color.PlainSequence);
                return color;
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Background color is bright! Returning gray...");
                return new Color(ConsoleColors.Gray);
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        public static void SetConsoleColor(KernelColorType colorType) => 
            SetConsoleColor(colorType, false);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        public static void SetConsoleColor(KernelColorType colorType, bool Background)
        {
            SetConsoleColor(GetColor(colorType), Background);
            if (!Background)
                SetConsoleColor(GetColor(KernelColorType.Background), true);
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        public static void SetConsoleColor(Color ColorSequence, bool Background = false)
        {
            if (ColorSequence is null)
                throw new KernelException(KernelExceptionType.Color, nameof(ColorSequence));

            // Set background
            if (Background)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Setting console background color to {0}...", ColorSequence.PlainSequence);
                TextWriterColor.WritePlain(ColorSequence.VTSequenceBackground, false);
                currentBackgroundColor = ColorSequence;
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Setting console foreground color to {0}...", ColorSequence.PlainSequence);
                TextWriterColor.WritePlain(ColorSequence.VTSequenceForeground, false);
                currentForegroundColor = ColorSequence;
            }
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

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(Color ColorSequence, bool Background)
        {
            try
            {
                SetConsoleColor(ColorSequence, Background);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, or a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(string ColorSpecifier)
        {
            try
            {
                var ColorInstance = new Color(ColorSpecifier);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="ColorNum">The color number</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(int ColorNum)
        {
            try
            {
                var ColorInstance = new Color(ColorNum);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(int R, int G, int B)
        {
            try
            {
                var ColorInstance = new Color(R, G, B);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Gets a random color instance
        /// </summary>
        /// <param name="type">Color type to generate</param>
        /// <param name="selectBlack">Whether to select the black color or not</param>
        /// <returns>A color instance</returns>
        public static Color GetRandomColor(ColorType type, bool selectBlack = true) =>
            GetRandomColor(type,
                selectBlack ? 0 : 1, type != ColorType._16Color ? 255 : 15,
                selectBlack ? 0 : 1, 255,
                selectBlack ? 0 : 1, 255,
                selectBlack ? 0 : 1, 255);

        /// <summary>
        /// Gets a random color instance
        /// </summary>
        /// <param name="type">Color type to generate</param>
        /// <param name="minColor">The minimum color level</param>
        /// <param name="maxColor">The maximum color level</param>
        /// <param name="minColorR">The minimum red color level</param>
        /// <param name="maxColorR">The maximum red color level</param>
        /// <param name="minColorG">The minimum green color level</param>
        /// <param name="maxColorG">The maximum green color level</param>
        /// <param name="minColorB">The minimum blue color level</param>
        /// <param name="maxColorB">The maximum blue color level</param>
        /// <returns>A color instance</returns>
        public static Color GetRandomColor(ColorType type, int minColor, int maxColor, int minColorR, int maxColorR, int minColorG, int maxColorG, int minColorB, int maxColorB)
        {
            switch (type)
            {
                case ColorType._16Color:
                    int colorNum = RandomDriver.Random(minColor, maxColor);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", colorNum);
                    return new Color(colorNum);
                case ColorType._255Color:
                    int colorNum2 = RandomDriver.Random(minColor, maxColor);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", colorNum2);
                    return new Color(colorNum2);
                case ColorType.TrueColor:
                    int colorNumR = RandomDriver.Random(minColorR, maxColorR);
                    int colorNumG = RandomDriver.Random(minColorG, maxColorG);
                    int colorNumB = RandomDriver.Random(minColorB, maxColorB);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", colorNumR, colorNumG, colorNumB);
                    return new Color(colorNumR, colorNumG, colorNumB);
                default:
                    return Color.Empty;
            }
        }

    }
}