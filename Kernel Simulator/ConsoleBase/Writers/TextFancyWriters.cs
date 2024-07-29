//
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

using Textify.Figlet.Utilities.Lines;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.FancyWriters.Tools;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using System;
using System.Collections.Generic;
using System.Threading;
using Terminaux.Colors;
using Terminaux.Base;

namespace Nitrocid.ConsoleBase.Writers
{
    /// <summary>
    /// Fancy text writer wrapper for writing with <see cref="ColorType"/> (<see cref="Terminaux.Writer.FancyWriters"/>)
    /// </summary>
    public static class TextFancyWriters
    {
        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, ColorType BoxBorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxBorderColor, ColorType.Background);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="ColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, ColorType BoxBorderColor, ColorType BackgroundColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxBorderColor, BackgroundColor);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, ColorType BoxBorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, settings, BoxBorderColor, ColorType.Background);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="ColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, ColorType BoxBorderColor, ColorType BackgroundColor)
        {
            try
            {
                // StringBuilder to put out the final rendering text
                string rendered = BorderColor.RenderBorderPlain(Left, Top, InteriorWidth, InteriorHeight, settings);
                TextWriterWhereColor.WriteWhereColorBack(rendered, Left, Top, false, KernelColorTools.GetColor(BoxBorderColor), KernelColorTools.GetColor(BackgroundColor));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, ColorType BoxBorderColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxBorderColor, ColorType.Background, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="ColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, ColorType BoxBorderColor, ColorType BackgroundColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxBorderColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, ColorType BoxBorderColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight, settings, BoxBorderColor, ColorType.Background, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="ColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, ColorType BoxBorderColor, ColorType BackgroundColor, params object[] vars)
        {
            try
            {
                // StringBuilder to put out the final rendering text
                string rendered = BorderColor.RenderBorderPlain(text, Left, Top, InteriorWidth, InteriorHeight, settings, vars);
                TextWriterWhereColor.WriteWhereColorBack(rendered, Left, Top, false, KernelColorTools.GetColor(BoxBorderColor), KernelColorTools.GetColor(BackgroundColor));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the box plainly
        /// </summary>
        /// <param name="Left">Where to place the box horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxColorType">Box color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBox(int Left, int Top, int InteriorWidth, int InteriorHeight, ColorType BoxColorType)
        {
            try
            {
                // Fill the box with spaces inside it
                TextWriterWhereColor.WriteWhereColorBack(BoxColor.RenderBox(Left, Top, InteriorWidth, InteriorHeight), Left, Top, false, KernelColorTools.GetColor(ColorType.NeutralText), KernelColorTools.GetColor(BoxColorType));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, ColorType BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, ColorType.Background);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="ColorType"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, ColorType BoxFrameColor, ColorType BackgroundColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, BackgroundColor);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, ColorType BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, settings, BoxFrameColor, ColorType.Background);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="FrameColor">BoxFrame color from Nitrocid KS's <see cref="ColorType"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, ColorType FrameColor, ColorType BackgroundColor)
        {
            try
            {
                // Render the box frame
                string frame = BoxFrameColor.RenderBoxFrame(Left, Top, InteriorWidth, InteriorHeight, settings);
                TextWriterWhereColor.WriteWhereColorBack(frame, Left, Top, false, KernelColorTools.GetColor(FrameColor), KernelColorTools.GetColor(BackgroundColor));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, ColorType BoxFrameColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, ColorType.Background, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="ColorType"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, ColorType BoxFrameColor, ColorType BackgroundColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, ColorType BoxFrameColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, BoxFrameColor, ColorType.Background, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="FrameColor">BoxFrame color from Nitrocid KS's <see cref="ColorType"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Nitrocid KS's <see cref="ColorType"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, ColorType FrameColor, ColorType BackgroundColor, params object[] vars)
        {
            try
            {
                // Render the box frame
                string frame = BoxFrameColor.RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, vars);
                TextWriterWhereColor.WriteWhereColorBack(frame, Left, Top, false, KernelColorTools.GetColor(FrameColor), KernelColorTools.GetColor(BackgroundColor));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FigletFont FigletFont, string Text, ColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredFigletTextColor.WriteCenteredFigletColorBack(top, FigletFont, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(ColorType.Background), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FigletFont FigletFont, string Text, ColorType colorTypeForeground, ColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredFigletTextColor.WriteCenteredFigletColorBack(top, FigletFont, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FigletFont FigletFont, string Text, ColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredFigletTextColor.WriteCenteredFigletColorBack(FigletFont, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(ColorType.Background), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FigletFont FigletFont, string Text, ColorType colorTypeForeground, ColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredFigletTextColor.WriteCenteredFigletColorBack(FigletFont, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(int top, string Text, ColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredColorBack(top, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(ColorType.Background), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(int top, string Text, ColorType colorTypeForeground, ColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredColorBack(top, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(string Text, ColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredColorBack(Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(ColorType.Background), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(string Text, ColorType colorTypeForeground, ColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredColorBack(Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(int top, string Text, ColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredOneLineColorBack(top, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(ColorType.Background), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(int top, string Text, ColorType colorTypeForeground, ColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredOneLineColorBack(top, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(string Text, ColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredOneLineColorBack(Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(ColorType.Background), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(string Text, ColorType colorTypeForeground, ColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredOneLineColorBack(Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFiglet(string Text, FigletFont FigletFont, ColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WritePlain(FigletColor.RenderFiglet(Text, FigletFont, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(ColorType.Background), leftMargin, rightMargin, Vars), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFiglet(string Text, FigletFont FigletFont, ColorType colorTypeForeground, ColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WritePlain(FigletColor.RenderFiglet(Text, FigletFont, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), leftMargin, rightMargin, Vars), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletWhere(string Text, int Left, int Top, bool Return, FigletFont FigletFont, ColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                KernelColorTools.SetConsoleColorDry(ColTypes);

                // Actually write
                FigletWhereColor.WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, leftMargin, rightMargin, Vars);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletWhere(string Text, int Left, int Top, bool Return, FigletFont FigletFont, ColorType colorTypeForeground, ColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                KernelColorTools.SetConsoleColorDry(colorTypeForeground);
                KernelColorTools.SetConsoleColorDry(colorTypeBackground, true);

                // Actually write
                FigletWhereColor.WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, leftMargin, rightMargin, Vars);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the PowerLine text
        /// </summary>
        /// <param name="Segments">List of PowerLine segments</param>
        /// <param name="EndingColor">A type of colors that will be changed at the end of the transition</param>
        /// <param name="Line">Write new line after writing the segments</param>
        public static void WritePowerLine(List<PowerLineSegment> Segments, ColorType EndingColor, bool Line = false) =>
            PowerLineColor.WritePowerLine(Segments, KernelColorTools.GetColor(EndingColor), Line);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, ColorType ProgressColor, bool DrawBorder = true) =>
            ProgressBarColor.WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, KernelColorTools.GetColor(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, ColorType ProgressColor, bool DrawBorder = true) =>
            ProgressBarColor.WriteProgress(Progress, Left, Top, width, KernelColorTools.GetColor(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, ColorType ProgressColor, ColorType FrameColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, ColorType ProgressColor, ColorType FrameColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, width, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, ColorType ProgressColor, ColorType FrameColor, ColorType BackgroundColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, ProgressColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, ColorType ProgressColor, ColorType FrameColor, ColorType BackgroundColor, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WritePlain(ProgressBarColor.RenderProgress(Progress, Left, Top, width, KernelColorTools.GetColor(ProgressColor), KernelColorTools.GetColor(FrameColor), KernelColorTools.GetColor(BackgroundColor), DrawBorder));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, ColorType ProgressColor, bool DrawBorder = true) =>
            ProgressBarVerticalColor.WriteVerticalProgress(Progress, Left, Top, ConsoleWrapper.WindowHeight - 2, KernelColorTools.GetColor(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="height">Progress bar height</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int height, ColorType ProgressColor, bool DrawBorder = true) =>
            ProgressBarVerticalColor.WriteVerticalProgress(Progress, Left, Top, height, KernelColorTools.GetColor(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, ColorType ProgressColor, ColorType FrameColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, ConsoleWrapper.WindowHeight - 2, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="height">Progress bar height</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int height, ColorType ProgressColor, ColorType FrameColor, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WritePlain(ProgressBarVerticalColor.RenderVerticalProgress(Progress, Left, Top, height, KernelColorTools.GetColor(ProgressColor), KernelColorTools.GetColor(FrameColor), DrawBorder));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, ColorType ColTypes, params object[] Vars) =>
            SeparatorWriterColor.WriteSeparatorColorBack(Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(ColorType.Background), true, Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, ColorType colorTypeForeground, ColorType colorTypeBackground, params object[] Vars) =>
            SeparatorWriterColor.WriteSeparatorColorBack(Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), true, Vars);

        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="colorTypeSeparatorForeground">A type of colors that will be changed for the separator foreground color.</param>
        /// <param name="colorTypeHeaderForeground">A type of colors that will be changed for the header foreground color.</param>
        /// <param name="colorTypeValueForeground">A type of colors that will be changed for the value foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, ColorType colorTypeSeparatorForeground, ColorType colorTypeHeaderForeground, ColorType colorTypeValueForeground, ColorType colorTypeBackground, bool SeparateRows = true, List<CellOptions> CellOptions = null) =>
            TableColor.WriteTable(Headers, Rows, Margin, KernelColorTools.GetColor(colorTypeSeparatorForeground), KernelColorTools.GetColor(colorTypeHeaderForeground), KernelColorTools.GetColor(colorTypeValueForeground), KernelColorTools.GetColor(colorTypeBackground), SeparateRows, CellOptions);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, ColorType sliderColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, sliderColor, ColorType.Separator, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, ColorType sliderColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, sliderColor, ColorType.Separator, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, ColorType sliderColor, ColorType FrameColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, sliderColor, FrameColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, ColorType sliderColor, ColorType FrameColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, sliderColor, FrameColor, ColorType.Background, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, ColorType sliderColor, ColorType FrameColor, ColorType BackgroundColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, sliderColor, FrameColor, BackgroundColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, ColorType sliderColor, ColorType FrameColor, ColorType BackgroundColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, sliderColor, FrameColor, BackgroundColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, BorderSettings settings, ColorType sliderColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, sliderColor, ColorType.Separator, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, ColorType sliderColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, width, settings, sliderColor, ColorType.Separator, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, BorderSettings settings, ColorType sliderColor, ColorType FrameColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, sliderColor, FrameColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, ColorType sliderColor, ColorType FrameColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, width, settings, sliderColor, FrameColor, ColorType.Background, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, BorderSettings settings, ColorType sliderColor, ColorType FrameColor, ColorType BackgroundColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, sliderColor, FrameColor, BackgroundColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, ColorType sliderColor, ColorType FrameColor, ColorType BackgroundColor, int minPos = 0, bool DrawBorder = true) =>
            SliderColor.WriteSliderAbsolute(currPos, maxPos, Left, Top, width, settings, KernelColorTools.GetColor(sliderColor), KernelColorTools.GetColor(FrameColor), KernelColorTools.GetColor(BackgroundColor), minPos, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, ColorType sliderColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, sliderColor, ColorType.Separator, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, ColorType sliderColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, sliderColor, ColorType.Separator, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, ColorType sliderColor, ColorType FrameColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, sliderColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, ColorType sliderColor, ColorType FrameColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, sliderColor, FrameColor, ColorType.Background, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, ColorType sliderColor, ColorType FrameColor, ColorType BackgroundColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, sliderColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, ColorType sliderColor, ColorType FrameColor, ColorType BackgroundColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, sliderColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, BorderSettings settings, ColorType sliderColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, sliderColor, ColorType.Separator, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, ColorType sliderColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, width, settings, sliderColor, ColorType.Separator, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, BorderSettings settings, ColorType sliderColor, ColorType FrameColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, sliderColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, ColorType sliderColor, ColorType FrameColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, width, settings, sliderColor, FrameColor, ColorType.Background, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, BorderSettings settings, ColorType sliderColor, ColorType FrameColor, ColorType BackgroundColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, sliderColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, ColorType sliderColor, ColorType FrameColor, ColorType BackgroundColor, bool DrawBorder = true) =>
            SliderColor.WriteSlider(currPos, maxPos, Left, Top, width, settings, KernelColorTools.GetColor(sliderColor), KernelColorTools.GetColor(FrameColor), KernelColorTools.GetColor(BackgroundColor), DrawBorder);

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, ColorType SliderColor, int minPos = 0, bool DrawBorder = true) =>
            WriteVerticalSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, BorderSettings.GlobalSettings, SliderColor, ColorType.Separator, minPos, DrawBorder);

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="height">Slider height</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, int height, ColorType SliderColor, int minPos = 0, bool DrawBorder = true) =>
            WriteVerticalSliderAbsolute(currPos, maxPos, Left, Top, height, BorderSettings.GlobalSettings, SliderColor, ColorType.Separator, minPos, DrawBorder);

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, ColorType SliderColor, ColorType FrameColor, int minPos = 0, bool DrawBorder = true) =>
            WriteVerticalSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, BorderSettings.GlobalSettings, SliderColor, FrameColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="height">Slider height</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, int height, ColorType SliderColor, ColorType FrameColor, int minPos = 0, bool DrawBorder = true) =>
            WriteVerticalSliderAbsolute(currPos, maxPos, Left, Top, height, BorderSettings.GlobalSettings, SliderColor, FrameColor, ColorType.Background, minPos, DrawBorder);

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, ColorType SliderColor, ColorType FrameColor, ColorType BackgroundColor, int minPos = 0, bool DrawBorder = true) =>
            WriteVerticalSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, BorderSettings.GlobalSettings, SliderColor, FrameColor, BackgroundColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="height">Slider height</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, int height, ColorType SliderColor, ColorType FrameColor, ColorType BackgroundColor, int minPos = 0, bool DrawBorder = true) =>
            WriteVerticalSliderAbsolute(currPos, maxPos, Left, Top, height, BorderSettings.GlobalSettings, SliderColor, FrameColor, BackgroundColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, BorderSettings settings, ColorType SliderColor, int minPos = 0, bool DrawBorder = true) =>
            WriteVerticalSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, settings, SliderColor, ColorType.Separator, minPos, DrawBorder);

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="height">Slider height</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, int height, BorderSettings settings, ColorType SliderColor, int minPos = 0, bool DrawBorder = true) =>
            WriteVerticalSliderAbsolute(currPos, maxPos, Left, Top, height, settings, SliderColor, ColorType.Separator, minPos, DrawBorder);

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, BorderSettings settings, ColorType SliderColor, ColorType FrameColor, int minPos = 0, bool DrawBorder = true) =>
            WriteVerticalSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, settings, SliderColor, FrameColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="height">Slider height</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, int height, BorderSettings settings, ColorType SliderColor, ColorType FrameColor, int minPos = 0, bool DrawBorder = true) =>
            WriteVerticalSliderAbsolute(currPos, maxPos, Left, Top, height, settings, SliderColor, FrameColor, ColorType.Background, minPos, DrawBorder);

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, BorderSettings settings, ColorType SliderColor, ColorType FrameColor, ColorType BackgroundColor, int minPos = 0, bool DrawBorder = true) =>
            WriteVerticalSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, settings, SliderColor, FrameColor, BackgroundColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="height">Slider height</param>
        /// <param name="settings">Border settings</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, int height, BorderSettings settings, ColorType SliderColor, ColorType FrameColor, ColorType BackgroundColor, int minPos = 0, bool DrawBorder = true) =>
            SliderVerticalColor.WriteVerticalSliderAbsolute(currPos, maxPos, Left, Top, height, settings, KernelColorTools.GetColor(SliderColor), KernelColorTools.GetColor(FrameColor), KernelColorTools.GetColor(BackgroundColor), minPos, DrawBorder);

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, ColorType SliderColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, BorderSettings.GlobalSettings, SliderColor, ColorType.Separator, DrawBorder);

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="height">Slider height</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int height, ColorType SliderColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, height, BorderSettings.GlobalSettings, SliderColor, ColorType.Separator, DrawBorder);

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, ColorType SliderColor, ColorType FrameColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, BorderSettings.GlobalSettings, SliderColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="height">Slider height</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int height, ColorType SliderColor, ColorType FrameColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, height, BorderSettings.GlobalSettings, SliderColor, FrameColor, ColorType.Background, DrawBorder);

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, ColorType SliderColor, ColorType FrameColor, ColorType BackgroundColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, BorderSettings.GlobalSettings, SliderColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="height">Slider height</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int height, ColorType SliderColor, ColorType FrameColor, ColorType BackgroundColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, height, BorderSettings.GlobalSettings, SliderColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, BorderSettings settings, ColorType SliderColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, settings, SliderColor, ColorType.Separator, DrawBorder);

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="height">Slider height</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int height, BorderSettings settings, ColorType SliderColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, height, settings, SliderColor, ColorType.Separator, DrawBorder);

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, BorderSettings settings, ColorType SliderColor, ColorType FrameColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, settings, SliderColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="height">Slider height</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int height, BorderSettings settings, ColorType SliderColor, ColorType FrameColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, height, settings, SliderColor, FrameColor, ColorType.Background, DrawBorder);

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, BorderSettings settings, ColorType SliderColor, ColorType FrameColor, ColorType BackgroundColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, settings, SliderColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="height">Slider height</param>
        /// <param name="settings">Border settings</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int height, BorderSettings settings, ColorType SliderColor, ColorType FrameColor, ColorType BackgroundColor, bool DrawBorder = true) =>
            SliderVerticalColor.WriteVerticalSlider(currPos, maxPos, Left, Top, height, settings, KernelColorTools.GetColor(SliderColor), KernelColorTools.GetColor(FrameColor), KernelColorTools.GetColor(BackgroundColor), DrawBorder);
    }
}
