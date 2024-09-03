﻿//
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

using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Resources;
using KS.Shell.ShellBase.Commands;
using Newtonsoft.Json.Linq;

namespace KS.TestShell.Commands
{
    class Test_CheckLocalLinesCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var EnglishJson = JToken.Parse(ResourcesManager.GetData("eng.json", ResourcesType.Languages));
            JToken LanguageJson;
            foreach (string LanguageName in LanguageManager.Languages.Keys)
            {
                LanguageJson = JToken.Parse(ResourcesManager.GetData($"{LanguageName}.json", ResourcesType.Languages));
                if (LanguageJson.Count() != EnglishJson.Count())
                {
                    TextWriters.Write(Translate.DoTranslation("Line mismatch in") + " {0}: {1} <> {2}", true, KernelColorTools.ColTypes.Warning, LanguageName, LanguageJson.Count(), EnglishJson.Count());
                }
            }
        }

    }
}