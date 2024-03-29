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

using Figletize.Utilities;
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

namespace Nitrocid.ConsoleBase.Writers
{
    /// <summary>
    /// Fancy text writer wrapper for writing with <see cref="KernelColorType"/> (<see cref="Terminaux.Writer.FancyWriters"/>)
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
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxBorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxBorderColor, KernelColorType.Background);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxBorderColor, KernelColorType BackgroundColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxBorderColor, BackgroundColor);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for border</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for border</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for border</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for border</param>
        /// <param name="UpperFrameChar">Upper frame character for border</param>
        /// <param name="LowerFrameChar">Lower frame character for border</param>
        /// <param name="LeftFrameChar">Left frame character for border</param>
        /// <param name="RightFrameChar">Right frame character for border</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType BoxBorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BoxBorderColor, KernelColorType.Background);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for border</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for border</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for border</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for border</param>
        /// <param name="UpperFrameChar">Upper frame character for border</param>
        /// <param name="LowerFrameChar">Lower frame character for border</param>
        /// <param name="LeftFrameChar">Left frame character for border</param>
        /// <param name="RightFrameChar">Right frame character for border</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType BoxBorderColor, KernelColorType BackgroundColor)
        {
            try
            {
                // StringBuilder to put out the final rendering text
                string rendered = BorderColor.RenderBorderPlain(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
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
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxBorderColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxBorderColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxBorderColor, KernelColorType BackgroundColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxBorderColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for border</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for border</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for border</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for border</param>
        /// <param name="UpperFrameChar">Upper frame character for border</param>
        /// <param name="LowerFrameChar">Lower frame character for border</param>
        /// <param name="LeftFrameChar">Left frame character for border</param>
        /// <param name="RightFrameChar">Right frame character for border</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType BoxBorderColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BoxBorderColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for border</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for border</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for border</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for border</param>
        /// <param name="UpperFrameChar">Upper frame character for border</param>
        /// <param name="LowerFrameChar">Lower frame character for border</param>
        /// <param name="LeftFrameChar">Left frame character for border</param>
        /// <param name="RightFrameChar">Right frame character for border</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType BoxBorderColor, KernelColorType BackgroundColor, params object[] vars)
        {
            try
            {
                // StringBuilder to put out the final rendering text
                string rendered = BorderColor.RenderBorderPlain(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, vars);
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
        /// <param name="BoxColorType">Box color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBox(int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxColorType)
        {
            try
            {
                // Fill the box with spaces inside it
                TextWriterWhereColor.WriteWhereColorBack(BoxColor.RenderBox(Left, Top, InteriorWidth, InteriorHeight), Left, Top, false, KernelColorTools.GetColor(KernelColorType.NeutralText), KernelColorTools.GetColor(BoxColorType));
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
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxFrameColor, KernelColorType.Background);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxFrameColor, KernelColorType BackgroundColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxFrameColor, BackgroundColor);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BoxFrameColor, KernelColorType.Background);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        /// <param name="FrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType FrameColor, KernelColorType BackgroundColor)
        {
            try
            {
                // Render the box frame
                string frame = BoxFrameColor.RenderBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
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
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxFrameColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxFrameColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxFrameColor, KernelColorType BackgroundColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxFrameColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType BoxFrameColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BoxFrameColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        /// <param name="FrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType FrameColor, KernelColorType BackgroundColor, params object[] vars)
        {
            try
            {
                // Render the box frame
                string frame = BoxFrameColor.RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, vars);
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
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FigletizeFont FigletFont, string Text, KernelColorType ColTypes, params object[] Vars) =>
            CenteredFigletTextColor.WriteCenteredFigletColorBack(top, FigletFont, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FigletizeFont FigletFont, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            CenteredFigletTextColor.WriteCenteredFigletColorBack(top, FigletFont, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FigletizeFont FigletFont, string Text, KernelColorType ColTypes, params object[] Vars) =>
            CenteredFigletTextColor.WriteCenteredFigletColorBack(FigletFont, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FigletizeFont FigletFont, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            CenteredFigletTextColor.WriteCenteredFigletColorBack(FigletFont, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(int top, string Text, KernelColorType ColTypes, params object[] Vars) =>
            CenteredTextColor.WriteCenteredColorBack(top, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(int top, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            CenteredTextColor.WriteCenteredColorBack(top, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(string Text, KernelColorType ColTypes, params object[] Vars) =>
            CenteredTextColor.WriteCenteredColorBack(Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            CenteredTextColor.WriteCenteredColorBack(Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(int top, string Text, KernelColorType ColTypes, params object[] Vars) =>
            CenteredTextColor.WriteCenteredOneLineColorBack(top, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(int top, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            CenteredTextColor.WriteCenteredOneLineColorBack(top, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(string Text, KernelColorType ColTypes, params object[] Vars) =>
            CenteredTextColor.WriteCenteredOneLineColorBack(Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            CenteredTextColor.WriteCenteredOneLineColorBack(Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFiglet(string Text, FigletizeFont FigletFont, KernelColorType ColTypes, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WritePlain(FigletColor.RenderFigletPlain(Text, FigletFont, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars), false);
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
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFiglet(string Text, FigletizeFont FigletFont, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WritePlain(FigletColor.RenderFigletPlain(Text, FigletFont, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars), false);
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
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletWhere(string Text, int Left, int Top, bool Return, FigletizeFont FigletFont, KernelColorType ColTypes, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                KernelColorTools.SetConsoleColorDry(ColTypes);

                // Actually write
                FigletWhereColor.WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, Vars);
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
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletWhere(string Text, int Left, int Top, bool Return, FigletizeFont FigletFont, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                KernelColorTools.SetConsoleColorDry(colorTypeForeground);
                KernelColorTools.SetConsoleColorDry(colorTypeBackground, true);

                // Actually write
                FigletWhereColor.WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, Vars);
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
        public static void WritePowerLine(List<PowerLineSegment> Segments, KernelColorType EndingColor, bool Line = false) =>
            PowerLineColor.WritePowerLine(Segments, KernelColorTools.GetColor(EndingColor), Line);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, KernelColorType ProgressColor, bool DrawBorder = true, bool Targeted = false) =>
            ProgressBarColor.WriteProgress(Progress, Left, Top, 10, 0, KernelColorTools.GetColor(ProgressColor), ColorTools.GetGray(), DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="WidthOffset">Width offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, int WidthOffset, KernelColorType ProgressColor, bool DrawBorder = true, bool Targeted = false) =>
            ProgressBarColor.WriteProgress(Progress, Left, Top, WidthOffset, 0, KernelColorTools.GetColor(ProgressColor), ColorTools.GetGray(), DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, KernelColorType ProgressColor, bool DrawBorder = true, bool Targeted = false) =>
            ProgressBarColor.WriteProgress(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, KernelColorTools.GetColor(ProgressColor), ColorTools.GetGray(), DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, KernelColorType ProgressColor, KernelColorType FrameColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, 10, 0, ProgressColor, FrameColor, DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="WidthOffset">Width offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, int WidthOffset, KernelColorType ProgressColor, KernelColorType FrameColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, WidthOffset, 0, ProgressColor, FrameColor, DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, KernelColorType ProgressColor, KernelColorType FrameColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, ProgressColor, FrameColor, KernelColorType.Background, DrawBorder, Targeted);

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
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, KernelColorType ProgressColor, KernelColorType FrameColor, KernelColorType BackgroundColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, 10, 0, ProgressColor, FrameColor, BackgroundColor, DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="WidthOffset">Width offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, int WidthOffset, KernelColorType ProgressColor, KernelColorType FrameColor, KernelColorType BackgroundColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, WidthOffset, 0, ProgressColor, FrameColor, BackgroundColor, DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, KernelColorType ProgressColor, KernelColorType FrameColor, KernelColorType BackgroundColor, bool DrawBorder = true, bool Targeted = false)
        {
            try
            {
                TextWriterRaw.WritePlain(ProgressBarColor.RenderProgress(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, KernelColorTools.GetColor(ProgressColor), KernelColorTools.GetColor(FrameColor), KernelColorTools.GetColor(BackgroundColor), DrawBorder, Targeted));
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
        public static void WriteVerticalProgress(double Progress, int Left, int Top, KernelColorType ProgressColor, bool DrawBorder = true) =>
            ProgressBarVerticalColor.WriteVerticalProgress(Progress, Left, Top, 2, 0, KernelColorTools.GetColor(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int HeightOffset, KernelColorType ProgressColor, bool DrawBorder = true) =>
            ProgressBarVerticalColor.WriteVerticalProgress(Progress, Left, Top, HeightOffset, 0, KernelColorTools.GetColor(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, KernelColorType ProgressColor, bool DrawBorder = true) =>
            ProgressBarVerticalColor.WriteVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, KernelColorTools.GetColor(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, KernelColorType ProgressColor, KernelColorType FrameColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, 2, 0, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int HeightOffset, KernelColorType ProgressColor, KernelColorType FrameColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, HeightOffset, 0, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, KernelColorType ProgressColor, KernelColorType FrameColor, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WritePlain(ProgressBarVerticalColor.RenderVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, KernelColorTools.GetColor(ProgressColor), KernelColorTools.GetColor(FrameColor), DrawBorder));
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
        public static void WriteSeparator(string Text, KernelColorType ColTypes, params object[] Vars) =>
            SeparatorWriterColor.WriteSeparatorColorBack(Text, true, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            SeparatorWriterColor.WriteSeparatorColorBack(Text, true, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool PrintSuffix, KernelColorType ColTypes, params object[] Vars) =>
            SeparatorWriterColor.WriteSeparatorColorBack(Text, PrintSuffix, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool PrintSuffix, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            SeparatorWriterColor.WriteSeparatorColorBack(Text, PrintSuffix, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

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
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, KernelColorType colorTypeSeparatorForeground, KernelColorType colorTypeHeaderForeground, KernelColorType colorTypeValueForeground, KernelColorType colorTypeBackground, bool SeparateRows = true, List<CellOptions> CellOptions = null) =>
            TableColor.WriteTable(Headers, Rows, Margin, KernelColorTools.GetColor(colorTypeSeparatorForeground), KernelColorTools.GetColor(colorTypeHeaderForeground), KernelColorTools.GetColor(colorTypeValueForeground), KernelColorTools.GetColor(colorTypeBackground), SeparateRows, CellOptions);
    }
}
