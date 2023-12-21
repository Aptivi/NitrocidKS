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
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

namespace KS.Shell.Commands
{
    class ChLangCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListSwitchesOnly.Contains("-list"))
            {
                TextWriterColor.Write(Translate.DoTranslation("Available languages:"), true, KernelColorTools.ColTypes.ListTitle);
                foreach (string Language in LanguageManager.Languages.Keys)
                {
                    TextWriterColor.Write("- {0}: ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), Language);
                    TextWriterColor.Write(LanguageManager.Languages[Language].FullLanguageName, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
                }
            }
            else
            {
                bool AlwaysTransliterated = default, AlwaysTranslated = default, Force = default;
                if (ListSwitchesOnly.Contains("-alwaystransliterated"))
                    AlwaysTransliterated = true;
                if (ListSwitchesOnly.Contains("-alwaystranslated"))
                    AlwaysTranslated = true; // -alwaystransliterated has higher priority.
                if (ListSwitchesOnly.Contains("-force"))
                    Force = true;
                LanguageManager.PromptForSetLang(ListArgsOnly[0], Force, AlwaysTransliterated, AlwaysTranslated);
            }
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
            TextWriterColor.Write("  -alwaystransliterated: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Always use the transliterated version"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
            TextWriterColor.Write("  -alwaystranslated: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Always use the translated version"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
            TextWriterColor.Write("  -force: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Force switching language"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
            TextWriterColor.Write("  -list: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
            TextWriterColor.Write(Translate.DoTranslation("Lists available languages"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
        }

    }
}