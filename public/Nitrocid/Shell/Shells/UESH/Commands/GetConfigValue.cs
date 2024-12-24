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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using System.Linq;
using Terminaux.Writer.CyclicWriters;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Gets a configuration value
    /// </summary>
    /// <remarks>
    /// This command prints a configuration value.
    /// </remarks>
    class GetConfigValueCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var configs = Config.GetKernelConfigs();
            string configName = parameters.ArgumentsList[0];
            string varName = parameters.ArgumentsList[1];
            if (ConfigTools.IsCustomSettingRegistered(configName))
            {
                var config = configs.Single((bkc) => bkc.GetType().Name == configName);
                var keys = ConfigTools.GetSettingsKeys(config);
                if (keys.Any((sk) => sk.Variable == varName))
                {
                    var key = ConfigTools.GetSettingsKey(config, varName);
                    var value = ConfigTools.GetValueFromEntry(key, config);
                    var keyName = new ListEntry()
                    {
                        Entry = Translate.DoTranslation("Key name"),
                        Value = key.Name,
                    };
                    var keyDesc = new ListEntry()
                    {
                        Entry = Translate.DoTranslation("Key description"),
                        Value = key.Description,
                    };
                    var keyType = new ListEntry()
                    {
                        Entry = Translate.DoTranslation("Key type"),
                        Value = $"{key.Type}",
                    };
                    var keyVar = new ListEntry()
                    {
                        Entry = Translate.DoTranslation("Key variable"),
                        Value = $"{key.Variable} [{value}]",
                    };
                    TextWriterRaw.WriteRaw(
                        keyName.Render() +
                        keyDesc.Render() +
                        keyType.Render() +
                        keyVar.Render()
                    );
                    variableValue = $"{value}";
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Key not found."), KernelColorType.Error);
                    return 28;
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Config not found."), KernelColorType.Error);
                return 28;
            }
            return 0;
        }

        public override void HelpHelper()
        {
            var names = Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray();
            TextWriters.WriteListEntry(Translate.DoTranslation("Available configuration types"), string.Join(", ", names), KernelColorType.ListEntry, KernelColorType.ListValue);
        }

    }
}
