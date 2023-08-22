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

using KS.Kernel.Debugging;
using Terminaux.Sequences.Tools;

namespace KS.Drivers.Console.Consoles
{
    internal class TerminalDebug : BaseConsoleDriver, IConsoleDriver
    {

        public override string DriverName => "Default - Debug";

        public override DriverTypes DriverType => DriverTypes.Console;

        /// <inheritdoc/>
        public override void Write(char value)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "[X = {0}, Y = {1}, x-winmax = {2}, y-winmax = {3}, y-buffmax = {4}] [Message = {5}]",
                                   CursorLeft, CursorTop, WindowWidth, WindowHeight, BufferHeight, value);
            _moved = true;
            base.Write(value);
        }

        /// <inheritdoc/>
        public override void Write(string text)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "[X = {0}, Y = {1}, x-winmax = {2}, y-winmax = {3}, y-buffmax = {4}] [VT = {5}, Seqs = {6}] [Len = {7}] [Message = {8}] [Literal = {9}]",
                                   CursorLeft, CursorTop, WindowWidth, WindowHeight, BufferHeight, 
                                   VtSequenceTools.MatchVTSequences(text).Length > 0, VtSequenceTools.MatchVTSequences(text).Length, 
                                   text.Length, text, VtSequenceTools.FilterVTSequences(text));
            _moved = true;
            base.Write(text);
        }

        /// <inheritdoc/>
        public override void Write(string text, params object[] args)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "[X = {0}, Y = {1}, x-winmax = {2}, y-winmax = {3}, y-buffmax = {4}] [VT = {5}, Seqs = {6}] [Len = {7}] [Message = {8}] [Literal = {9}] [Vars = {10}]",
                                   CursorLeft, CursorTop, WindowWidth, WindowHeight, BufferHeight,
                                   VtSequenceTools.MatchVTSequences(text).Length > 0, VtSequenceTools.MatchVTSequences(text).Length,
                                   text.Length, text, VtSequenceTools.FilterVTSequences(text), args.Length);
            _moved = true;
            base.Write(text, args);
        }

        /// <inheritdoc/>
        public override void WriteLine()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "[X = {0}, Y = {1}, x-winmax = {2}, y-winmax = {3}, y-buffmax = {4}]",
                                   CursorLeft, CursorTop, WindowWidth, WindowHeight, BufferHeight);
            _moved = true;
            base.WriteLine();
        }

        /// <inheritdoc/>
        public override void WriteLine(string text)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "[X = {0}, Y = {1}, x-winmax = {2}, y-winmax = {3}, y-buffmax = {4}] [VT = {5}, Seqs = {6}] [Len = {7}] [Message = {8}] [Literal = {9}]",
                                   CursorLeft, CursorTop, WindowWidth, WindowHeight, BufferHeight,
                                   VtSequenceTools.MatchVTSequences(text).Length > 0, VtSequenceTools.MatchVTSequences(text).Length,
                                   text.Length, text, VtSequenceTools.FilterVTSequences(text));
            _moved = true;
            base.WriteLine(text);
        }

        /// <inheritdoc/>
        public override void WriteLine(string text, params object[] args)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "[X = {0}, Y = {1}, x-winmax = {2}, y-winmax = {3}, y-buffmax = {4}] [VT = {5}, Seqs = {6}] [Len = {7}] [Message = {8}] [Literal = {9}] [Vars = {10}]",
                                   CursorLeft, CursorTop, WindowWidth, WindowHeight, BufferHeight,
                                   VtSequenceTools.MatchVTSequences(text).Length > 0, VtSequenceTools.MatchVTSequences(text).Length,
                                   text.Length, text, VtSequenceTools.FilterVTSequences(text), args.Length);
            _moved = true;
            base.WriteLine(text, args);
        }
    }
}
