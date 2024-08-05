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
using System.Globalization;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
using KS.TimeDate;

namespace KS.TestShell.Commands
{
    class Test_DCalendCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgs[0] == "Gregorian")
            {
                TextWriters.Write(TimeDateRenderers.RenderDate(new CultureInfo("en-US")), true, KernelColorTools.ColTypes.Neutral);
            }
            else if (ListArgs[0] == "Hijri")
            {
                var Cult = new CultureInfo("ar");
                Cult.DateTimeFormat.Calendar = new HijriCalendar();
                TextWriters.Write(TimeDateRenderers.RenderDate(Cult), true, KernelColorTools.ColTypes.Neutral);
            }
            else if (ListArgs[0] == "Persian")
            {
                TextWriters.Write(TimeDateRenderers.RenderDate(new CultureInfo("fa")), true, KernelColorTools.ColTypes.Neutral);
            }
            else if (ListArgs[0] == "Saudi-Hijri")
            {
                TextWriters.Write(TimeDateRenderers.RenderDate(new CultureInfo("ar-SA")), true, KernelColorTools.ColTypes.Neutral);
            }
            else if (ListArgs[0] == "Thai-Buddhist")
            {
                TextWriters.Write(TimeDateRenderers.RenderDate(new CultureInfo("th-TH")), true, KernelColorTools.ColTypes.Neutral);
            }
        }

    }
}