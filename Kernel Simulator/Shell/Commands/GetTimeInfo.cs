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
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
using KS.TimeDate;

namespace KS.Shell.Commands
{
    class GetTimeInfoCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (DateTime.TryParse(ListArgs[0], out DateTime DateTimeInfo))
            {
                TextWriters.Write("-- " + Translate.DoTranslation("Information for") + " {0} --" + Kernel.Kernel.NewLine, true, KernelColorTools.ColTypes.Neutral, TimeDateRenderers.Render(DateTimeInfo));
                TextWriters.Write(Translate.DoTranslation("Milliseconds:") + " {0}", true, KernelColorTools.ColTypes.Neutral, DateTimeInfo.Millisecond);
                TextWriters.Write(Translate.DoTranslation("Seconds:") + " {0}", true, KernelColorTools.ColTypes.Neutral, DateTimeInfo.Second);
                TextWriters.Write(Translate.DoTranslation("Minutes:") + " {0}", true, KernelColorTools.ColTypes.Neutral, DateTimeInfo.Minute);
                TextWriters.Write(Translate.DoTranslation("Hours:") + " {0}", true, KernelColorTools.ColTypes.Neutral, DateTimeInfo.Hour);
                TextWriters.Write(Translate.DoTranslation("Days:") + " {0}", true, KernelColorTools.ColTypes.Neutral, DateTimeInfo.Day);
                TextWriters.Write(Translate.DoTranslation("Months:") + " {0}", true, KernelColorTools.ColTypes.Neutral, DateTimeInfo.Month);
                TextWriters.Write(Translate.DoTranslation("Year:") + " {0}" + Kernel.Kernel.NewLine, true, KernelColorTools.ColTypes.Neutral, DateTimeInfo.Year);
                TextWriters.Write(Translate.DoTranslation("Date:") + " {0}", true, KernelColorTools.ColTypes.Neutral, TimeDateRenderers.RenderDate(DateTimeInfo));
                TextWriters.Write(Translate.DoTranslation("Time:") + " {0}" + Kernel.Kernel.NewLine, true, KernelColorTools.ColTypes.Neutral, TimeDateRenderers.RenderTime(DateTimeInfo));
                TextWriters.Write(Translate.DoTranslation("Day of Year:") + " {0}", true, KernelColorTools.ColTypes.Neutral, DateTimeInfo.DayOfYear);
                TextWriters.Write(Translate.DoTranslation("Day of Week:") + " {0}" + Kernel.Kernel.NewLine, true, KernelColorTools.ColTypes.Neutral, DateTimeInfo.DayOfWeek.ToString());
                TextWriters.Write(Translate.DoTranslation("Binary:") + " {0}", true, KernelColorTools.ColTypes.Neutral, DateTimeInfo.ToBinary());
                TextWriters.Write(Translate.DoTranslation("Local Time:") + " {0}", true, KernelColorTools.ColTypes.Neutral, TimeDateRenderers.Render(DateTimeInfo.ToLocalTime()));
                TextWriters.Write(Translate.DoTranslation("Universal Time:") + " {0}", true, KernelColorTools.ColTypes.Neutral, TimeDateRenderers.Render(DateTimeInfo.ToUniversalTime()));
                TextWriters.Write(Translate.DoTranslation("Unix Time:") + " {0}", true, KernelColorTools.ColTypes.Neutral, TimeDateConverters.DateToUnix(DateTimeInfo));
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Failed to parse date information for") + " {0}. " + Translate.DoTranslation("Ensure that the format is correct."), true, KernelColorTools.ColTypes.Error, ListArgs[0]);
            }
        }

    }
}
