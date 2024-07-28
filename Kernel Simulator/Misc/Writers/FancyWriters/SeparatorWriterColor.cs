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
using KS.ConsoleBase.Colors;
using Terminaux.Colors;
using TermSeparator = Terminaux.Writer.FancyWriters.SeparatorWriterColor;

namespace KS.Misc.Writers.FancyWriters
{
    public static class SeparatorWriterColor
    {

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, params object[] Vars)
        {
            TermSeparator.WriteSeparator(Text, true, Vars);
        }

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, KernelColorTools.ColTypes ColTypes, params object[] Vars)
        {
            TermSeparator.WriteSeparatorColor(Text, KernelColorTools.GetConsoleColor(ColTypes), true, Vars);
        }

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, KernelColorTools.ColTypes colorTypeForeground, KernelColorTools.ColTypes colorTypeBackground, params object[] Vars)
        {
            TermSeparator.WriteSeparatorColorBack(Text, KernelColorTools.GetConsoleColor(colorTypeForeground), KernelColorTools.GetConsoleColor(colorTypeBackground), true, Vars);
        }

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, ConsoleColor Color, params object[] Vars)
        {
            TermSeparator.WriteSeparatorColor(Text, (Color)Color, true, Vars);
        }

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor, params object[] Vars)
        {
            TermSeparator.WriteSeparatorColorBack(Text, (Color)ForegroundColor, (Color)BackgroundColor, true, Vars);
        }

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, Color Color, params object[] Vars)
        {
            TermSeparator.WriteSeparatorColor(Text, Color, true, Vars);
        }

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            TermSeparator.WriteSeparatorColorBack(Text, ForegroundColor, BackgroundColor, true, Vars);
        }

    }
}
