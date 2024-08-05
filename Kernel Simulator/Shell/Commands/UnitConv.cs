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
using System.Data;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
using UnitsNet;

namespace KS.Shell.Commands
{
    class UnitConvCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string UnitType = ListArgsOnly[0];
            int QuantityNum = Convert.ToInt32(ListArgsOnly[1]);
            string SourceUnit = ListArgsOnly[2];
            string TargetUnit = ListArgsOnly[3];
            QuantityInfo[] QuantityInfos = Quantity.Infos.Where(x => (x.Name ?? "") == (UnitType ?? "")).ToArray();
            var TargetUnitInstance = UnitsNetSetup.Default.UnitParser.Parse(TargetUnit, QuantityInfos[0].UnitType);
            var ConvertedUnit = Quantity.Parse(QuantityInfos[0].ValueType, $"{QuantityNum} {SourceUnit}").ToUnit(TargetUnitInstance);
            TextWriters.Write("- {0} => {1}: ", false, KernelColorTools.ColTypes.ListEntry, SourceUnit, TargetUnit);
            TextWriters.Write(ConvertedUnit.ToString(CultureManager.CurrentCult.NumberFormat), true, KernelColorTools.ColTypes.ListValue);
        }

        public override void HelpHelper()
        {
            TextWriters.Write(Translate.DoTranslation("Available unit types and their units:"), true, KernelColorTools.ColTypes.Neutral);
            foreach (QuantityInfo QuantityInfo in Quantity.Infos)
            {
                TextWriters.Write("- {0}:", true, KernelColorTools.ColTypes.ListEntry, QuantityInfo.Name);
                foreach (Enum UnitValues in QuantityInfo.UnitInfos.Select(x => x.Value))
                {
                    TextWriters.Write("  - {0}: ", false, KernelColorTools.ColTypes.ListEntry, string.Join(", ", UnitsNetSetup.Default.UnitAbbreviations.GetDefaultAbbreviation(UnitValues.GetType(), Convert.ToInt32(UnitValues))));
                    TextWriters.Write(UnitValues.ToString(), true, KernelColorTools.ColTypes.ListValue);
                }
            }
        }

    }
}