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

using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
using Terminaux.Writer.FancyWriters;

namespace KS.Shell.Commands
{
    class LangManCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (!Flags.SafeMode)
            {
                string CommandMode = ListArgsOnly[0].ToLower();
                string TargetLanguage = "";
                string TargetLanguagePath = "";
                string LanguageListTerm = "";

                // These command modes require two arguments to be passed, so re-check here and there. Optional arguments also lie there.
                switch (CommandMode ?? "")
                {
                    case "reload":
                    case "load":
                    case "unload":
                        {
                            if (ListArgsOnly.Length > 1)
                            {
                                TargetLanguage = ListArgsOnly[1];
                                TargetLanguagePath = Filesystem.NeutralizePath(TargetLanguage + ".json", Paths.GetKernelPath(KernelPathType.CustomLanguages));
                                if (!(Parsing.TryParsePath(TargetLanguagePath) && Checking.FileExists(TargetLanguagePath)) & !LanguageManager.Languages.ContainsKey(TargetLanguage))
                                {
                                    TextWriters.Write(Translate.DoTranslation("Language not found or file has invalid characters."), true, KernelColorTools.ColTypes.Error);
                                    return;
                                }
                            }
                            else
                            {
                                TextWriters.Write(Translate.DoTranslation("Language is not specified."), true, KernelColorTools.ColTypes.Error);
                                return;
                            }

                            break;
                        }
                    case "list":
                        {
                            if (ListArgsOnly.Length > 1)
                            {
                                LanguageListTerm = ListArgsOnly[1];
                            }

                            break;
                        }
                }

                // Now, the actual logic
                switch (CommandMode ?? "")
                {
                    case "reload":
                        {
                            LanguageManager.UninstallCustomLanguage(TargetLanguagePath);
                            LanguageManager.InstallCustomLanguage(TargetLanguagePath);
                            break;
                        }
                    case "load":
                        {
                            LanguageManager.InstallCustomLanguage(TargetLanguage);
                            break;
                        }
                    case "unload":
                        {
                            LanguageManager.UninstallCustomLanguage(TargetLanguage);
                            break;
                        }
                    case "list":
                        {
                            foreach (string Language in LanguageManager.ListLanguages(LanguageListTerm).Keys)
                            {
                                SeparatorWriterColor.WriteSeparator(Language, true);
                                TextWriters.Write("- " + Translate.DoTranslation("Language short name:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write(LanguageManager.Languages[Language].ThreeLetterLanguageName, true, KernelColorTools.ColTypes.ListValue);
                                TextWriters.Write("- " + Translate.DoTranslation("Language full name:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write(LanguageManager.Languages[Language].FullLanguageName, true, KernelColorTools.ColTypes.ListValue);
                                TextWriters.Write("- " + Translate.DoTranslation("Language transliterable:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write($"{LanguageManager.Languages[Language].Transliterable}", true, KernelColorTools.ColTypes.ListValue);
                                TextWriters.Write("- " + Translate.DoTranslation("Custom language:") + " ", false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write($"{LanguageManager.Languages[Language].Custom}", true, KernelColorTools.ColTypes.ListValue);
                            }

                            break;
                        }
                    case "reloadall":
                        {
                            LanguageManager.UninstallCustomLanguages();
                            LanguageManager.InstallCustomLanguages();
                            break;
                        }

                    default:
                        {
                            TextWriters.Write(Translate.DoTranslation("Invalid command {0}. Check the usage below:"), true, KernelColorTools.ColTypes.Error, CommandMode);
                            HelpSystem.ShowHelp("langman");
                            break;
                        }
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Language management is disabled in safe mode."), true, KernelColorTools.ColTypes.Error);
            }
        }

    }
}
