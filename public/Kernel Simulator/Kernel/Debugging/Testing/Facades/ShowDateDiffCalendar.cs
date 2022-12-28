﻿
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.TimeDate;
using System.Globalization;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class ShowDateDiffCalendar : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests printing date using different calendars");
        public override void Run()
        {
            string Text = Input.ReadLine(Translate.DoTranslation("Write a calendar type:") + " ", "");
            switch (Text)
            {
                case "Gregorian":
                    TextWriterColor.Write(TimeDateRenderers.RenderDate(new CultureInfo("en-US")));
                    break;
                case "Hijri":
                    var Cult = new CultureInfo("ar");
                    Cult.DateTimeFormat.Calendar = new HijriCalendar();
                    TextWriterColor.Write(TimeDateRenderers.RenderDate(Cult));
                    break;
                case "Persian":
                    TextWriterColor.Write(TimeDateRenderers.RenderDate(new CultureInfo("fa")));
                    break;
                case "Saudi-Hijri":
                    TextWriterColor.Write(TimeDateRenderers.RenderDate(new CultureInfo("ar-SA")));
                    break;
                case "Thai-Buddhist":
                    TextWriterColor.Write(TimeDateRenderers.RenderDate(new CultureInfo("th-TH")));
                    break;
            }
        }
    }
}
