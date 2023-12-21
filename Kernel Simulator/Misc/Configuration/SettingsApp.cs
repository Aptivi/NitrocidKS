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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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

using System.Reflection;
using System.Text.RegularExpressions;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Screensaver.Customized;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Resources;
using Newtonsoft.Json.Linq;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Selector;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles.Selection;
using TermSeparator = Terminaux.Writer.FancyWriters.SeparatorWriterColor;

namespace KS.Misc.Configuration
{
    public static class SettingsApp
    {

        private static SettingsType CurrentSettingsType = SettingsType.Normal;

        /// <summary>
        /// Main page
        /// </summary>
        public static void OpenMainPage(SettingsType SettingsType)
        {
            var PromptFinished = default(bool);
            int AnswerInt;
            var SettingsToken = OpenSettingsResource(SettingsType);
            int MaxSections = SettingsToken.Count();
            CurrentSettingsType = SettingsType;

            while (!PromptFinished)
            {
                ConsoleWrapper.Clear();

                // Populate answers
                var inputs = new List<InputChoiceInfo>();
                var inputsAlt = new List<InputChoiceInfo>() { new($"{MaxSections + 1}", Translate.DoTranslation("Find a Setting")), new($"{MaxSections + 2}", Translate.DoTranslation("Save Settings")), new($"{MaxSections + 3}", Translate.DoTranslation("Save Settings As")), new($"{MaxSections + 4}", Translate.DoTranslation("Load Settings From")), new($"{MaxSections + 5}", Translate.DoTranslation("Exit")) };
                for (int SectionIndex = 0, loopTo = MaxSections - 1; SectionIndex <= loopTo; SectionIndex++)
                {
                    JProperty Section = (JProperty)SettingsToken.ToList()[SectionIndex];
                    if (SettingsType != SettingsType.Normal)
                    {
                        inputs.Add(new InputChoiceInfo($"{SectionIndex + 1}", Section.Name + "..."));
                    }
                    else
                    {
                        inputs.Add(new InputChoiceInfo($"{SectionIndex + 1}", Translate.DoTranslation(Section.Name + " Settings...")));
                    }
                }

                // Prompt for selection
                AnswerInt = SelectionStyle.PromptSelection(TermSeparator.RenderSeparator(Translate.DoTranslation("Welcome to Settings!"), true) + Kernel.Kernel.NewLine + Kernel.Kernel.NewLine + Translate.DoTranslation("Select section:"), inputs, inputsAlt);

                // Check for input
                DebugWriter.Wdbg(DebugLevel.I, "Succeeded. Checking the answer if it points to the right direction...");
                if (AnswerInt >= 1 & AnswerInt <= MaxSections)
                {
                    // The selected answer is a section
                    JProperty SelectedSection = (JProperty)SettingsToken.ToList()[AnswerInt - 1];
                    DebugWriter.Wdbg(DebugLevel.I, "Opening section {0}...", SelectedSection.Name);
                    OpenSection(SelectedSection.Name, SettingsToken);
                }
                else if (AnswerInt == MaxSections + 1)
                {
                    // The selected answer is "Find a Setting"
                    VariableFinder(SettingsToken);
                }
                else if (AnswerInt == MaxSections + 2)
                {
                    // The selected answer is "Save Settings"
                    DebugWriter.Wdbg(DebugLevel.I, "Saving settings...");
                    try
                    {
                        Config.CreateConfig();
                        CustomSaverTools.SaveCustomSaverSettings();
                    }
                    catch (Exception ex)
                    {
                        TextWriterColor.Write(ex.Message, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                        DebugWriter.WStkTrc(ex);
                        ConsoleBase.Inputs.Input.DetectKeypress();
                    }
                }
                else if (AnswerInt == MaxSections + 3)
                {
                    // The selected answer is "Save Settings As"
                    TextWriterColor.Write(Translate.DoTranslation("Where do you want to save the current kernel settings?"), true, KernelColorTools.ColTypes.Question);
                    string Location = Filesystem.NeutralizePath(ConsoleBase.Inputs.Input.ReadLine());
                    if (!Checking.FileExists(Location))
                    {
                        try
                        {
                            Config.CreateConfig(Location);
                        }
                        catch (Exception ex)
                        {
                            TextWriterColor.Write(ex.Message, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                            DebugWriter.WStkTrc(ex);
                            ConsoleBase.Inputs.Input.DetectKeypress();
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Can't save kernel settings on top of existing file."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                        ConsoleBase.Inputs.Input.DetectKeypress();
                    }
                }
                else if (AnswerInt == MaxSections + 4)
                {
                    // The selected answer is "Load Settings From"
                    TextWriterColor.Write(Translate.DoTranslation("Where do you want to load the current kernel settings from?"), true, KernelColorTools.ColTypes.Question);
                    string Location = Filesystem.NeutralizePath(ConsoleBase.Inputs.Input.ReadLine());
                    if (Checking.FileExists(Location))
                    {
                        try
                        {
                            Config.ReadConfig(Location);
                            Config.CreateConfig();
                        }
                        catch (Exception ex)
                        {
                            TextWriterColor.Write(ex.Message, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                            DebugWriter.WStkTrc(ex);
                            ConsoleBase.Inputs.Input.DetectKeypress();
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("File not found."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                        ConsoleBase.Inputs.Input.DetectKeypress();
                    }
                }
                else if (AnswerInt == MaxSections + 5)
                {
                    // The selected answer is "Exit"
                    DebugWriter.Wdbg(DebugLevel.W, "Exiting...");
                    PromptFinished = true;
                    ConsoleWrapper.Clear();
                }
                else
                {
                    // Invalid selection
                    DebugWriter.Wdbg(DebugLevel.W, "Option is not valid. Returning...");
                    TextWriterColor.Write(Translate.DoTranslation("Specified option {0} is invalid."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), AnswerInt);
                    TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                    ConsoleBase.Inputs.Input.DetectKeypress();
                }
            }
        }

        /// <summary>
        /// Open section
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="SettingsToken">Settings token</param>
        public static void OpenSection(string Section, JToken SettingsToken)
        {
            try
            {
                // General variables
                var SectionFinished = default(bool);
                int AnswerInt;
                var SectionTokenGeneral = SettingsToken[Section];
                var SectionToken = SectionTokenGeneral["Keys"];
                var SectionDescription = SectionTokenGeneral["Desc"];
                int MaxOptions = SectionToken.Count();

                while (!SectionFinished)
                {

                    var inputs = new List<InputChoiceInfo>();
                    var inputsAlt = new List<InputChoiceInfo>();

                    // List options
                    for (int SectionIndex = 0, loopTo = MaxOptions - 1; SectionIndex <= loopTo; SectionIndex++)
                    {
                        var Setting = SectionToken[SectionIndex];
                        object CurrentValue = "Unknown";
                        string Variable = (string)Setting["Variable"];
                        string VariableProperty = (string)Setting["VariableProperty"];
                        SettingsKeyType VariableType = (SettingsKeyType)Convert.ToInt32(Enum.Parse(typeof(SettingsKeyType), (string)Setting["Type"]));

                        // Print the option
                        if (VariableType == SettingsKeyType.SMaskedString)
                        {
                            // Don't print the default value! We don't want to reveal passwords.
                            inputs.Add(new InputChoiceInfo($"{SectionIndex + 1}", Translate.DoTranslation((string)Setting["Name"])));
                        }
                        else
                        {
                            // Determine how to get the current value
                            if (VariableProperty is null)
                            {
                                if (FieldManager.CheckField(Variable))
                                {
                                    // We're dealing with the field, get the value from it
                                    CurrentValue = FieldManager.GetValue(Variable);
                                }
                                else if (PropertyManager.CheckProperty(Variable))
                                {
                                    // We're dealing with the property, get the value from it
                                    CurrentValue = PropertyManager.GetPropertyValue(Variable);
                                }

                                // Get the plain sequence from the color
                                if (CurrentValue is Color valueColor)
                                {
                                    CurrentValue = valueColor.PlainSequence;
                                }
                            }
                            else
                            {
                                // Get the property value from variable
                                CurrentValue = PropertyManager.GetPropertyValueInVariable(Variable, VariableProperty);
                            }
                            inputs.Add(new InputChoiceInfo($"{SectionIndex + 1}", Translate.DoTranslation((string)Setting["Name"]) + $" [{CurrentValue}]"));
                        }
                    }

                    // Check to see if we need to preview the screensaver
                    if (CurrentSettingsType == SettingsType.Screensaver)
                    {
                        inputsAlt.Add(new InputChoiceInfo($"{MaxOptions + 1}", Translate.DoTranslation("Preview screensaver")));
                        inputsAlt.Add(new InputChoiceInfo($"{MaxOptions + 2}", Translate.DoTranslation("Go Back...")));
                    }
                    else
                    {
                        inputsAlt.Add(new InputChoiceInfo($"{MaxOptions + 1}", Translate.DoTranslation("Go Back...")));
                    }
                    DebugWriter.Wdbg(DebugLevel.W, "Section {0} has {1} selections.", Section, MaxOptions);

                    // Prompt for selection
                    AnswerInt = SelectionStyle.PromptSelection(TermSeparator.RenderSeparator(Translate.DoTranslation(Section + " Settings..."), true) + Kernel.Kernel.NewLine + Kernel.Kernel.NewLine + Translate.DoTranslation((string)SectionDescription), inputs, inputsAlt);

                    // Check for answer
                    DebugWriter.Wdbg(DebugLevel.I, "Succeeded. Checking the answer if it points to the right direction...");
                    if (AnswerInt >= 1 & AnswerInt <= MaxOptions)
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Opening key {0} from section {1}...", AnswerInt, Section);
                        OpenKey(Section, AnswerInt, SettingsToken);
                    }
                    else if (AnswerInt == MaxOptions + 1 & CurrentSettingsType == SettingsType.Screensaver)
                    {
                        // Preview screensaver
                        DebugWriter.Wdbg(DebugLevel.I, "User requested screensaver preview.");
                        Screensaver.Screensaver.ShowSavers(Section);
                    }
                    else if (AnswerInt == MaxOptions + 1 | AnswerInt == MaxOptions + 2 & CurrentSettingsType == SettingsType.Screensaver)
                    {
                        // Go Back...
                        DebugWriter.Wdbg(DebugLevel.I, "User requested exit. Returning...");
                        SectionFinished = true;
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Option is not valid. Returning...");
                        TextWriterColor.Write(Translate.DoTranslation("Specified option {0} is invalid."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), AnswerInt);
                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                        ConsoleBase.Inputs.Input.DetectKeypress();
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleWrapper.Clear();
                DebugWriter.Wdbg(DebugLevel.I, "Error trying to open section: {0}", ex.Message);
                SeparatorWriterColor.WriteSeparator("???", true);
                TextWriterColor.Write(Kernel.Kernel.NewLine + "X) " + Translate.DoTranslation("Invalid section entered. Please go back."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                TextWriterColor.Write("X) " + Translate.DoTranslation("If you're sure that you've opened the right section, check this message out:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                TextWriterColor.Write("X) " + ex.Message, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                ConsoleBase.Inputs.Input.DetectKeypress();
            }
        }

        /// <summary>
        /// Open a key.
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="KeyNumber">Key number</param>
        /// <param name="SettingsToken">Settings token</param>
        public static void OpenKey(string Section, int KeyNumber, JToken SettingsToken)
        {
            try
            {
                // Section and key
                var SectionTokenGeneral = SettingsToken[Section];
                var SectionToken = SectionTokenGeneral["Keys"];
                var KeyToken = SectionToken.ToList()[KeyNumber - 1];
                int MaxKeyOptions = 0;

                // Key properties
                string KeyName = (string)KeyToken["Name"];
                string KeyDescription = (string)KeyToken["Description"];
                SettingsKeyType KeyType = (SettingsKeyType)Convert.ToInt32(Enum.Parse(typeof(SettingsKeyType), (string)KeyToken["Type"]));
                string KeyVar = (string)KeyToken["Variable"];
                object KeyValue = "";
                object KeyDefaultValue = "";
                var KeyFinished = default(bool);

                // Integer slider properties
                int IntSliderMinimumValue = (int)(KeyToken["MinimumValue"] ?? 0);
                int IntSliderMaximumValue = (int)(KeyToken["MaximumValue"] ?? 100);

                // Selection properties
                string KeyVarProperty = (string)KeyToken["VariableProperty"];
                bool SelectionEnum = (bool)(KeyToken["IsEnumeration"] ?? false);
                string SelectionEnumAssembly = (string)KeyToken["EnumerationAssembly"];
                bool SelectionEnumInternal = (bool)(KeyToken["EnumerationInternal"] ?? false);
                bool SelectionEnumZeroBased = (bool)(KeyToken["EnumerationZeroBased"] ?? false);

                // Variant properties
                object VariantValue = "";
                bool VariantFunctionSetsValue = (bool)(KeyToken["VariantFunctionSetsValue"] ?? false);
                string VariantFunction = (string)KeyToken["VariantFunction"];

                // Color properties
                object ColorValue = "";

                // List properties
                string ListJoinString = (string)KeyToken["Delimiter"];
                string ListJoinStringVariable = (string)KeyToken["DelimiterVariable"];
                string ListFunctionName = (string)KeyToken["SelectionFunctionName"];
                string ListFunctionType = (string)KeyToken["SelectionFunctionType"];
                bool ListIsPathCurrentPath = (bool)(KeyToken["IsPathCurrentPath"] ?? false);
                KernelPathType ListValuePathType = (KernelPathType)Convert.ToInt32(KeyToken["ValuePathType"] is not null ? Enum.Parse(typeof(KernelPathType), (string)KeyToken["ValuePathType"]) : KernelPathType.Mods);
                var TargetList = default(IEnumerable<object>);
                var SelectFrom = default(IEnumerable<object>);
                var Selections = default(Array);
                bool NeutralizePaths = (bool)(KeyToken["IsValuePath"] ?? false);
                string NeutralizeRootPath = ListIsPathCurrentPath ? CurrentDirectory.CurrentDir : Paths.GetKernelPath(ListValuePathType);

                // Inputs
                string AnswerString = "";

                var PressedKey = default(ConsoleKey);
                var FinalBool = default(bool);
                while (!KeyFinished)
                {
                    ConsoleWrapper.Clear();

                    // Make an introductory banner
                    SeparatorWriterColor.WriteSeparator(Translate.DoTranslation(Section + " Settings...") + " > " + Translate.DoTranslation(KeyName), true);
                    TextWriterColor.Write(Kernel.Kernel.NewLine + Translate.DoTranslation(KeyDescription), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));

                    // See how to get the value
                    if (!(KeyType == SettingsKeyType.SUnknown))
                    {
                        if (KeyType == SettingsKeyType.SSelection)
                        {
                            if (SelectionEnum)
                            {
                                if (SelectionEnumInternal)
                                {
                                    // Apparently, we need to have a full assembly name for getting types.
                                    SelectFrom = Type.GetType("KS." + KeyToken["Enumeration"].ToString() + ", " + Assembly.GetExecutingAssembly().FullName).GetEnumNames();
                                    Selections = Type.GetType("KS." + KeyToken["Enumeration"].ToString() + ", " + Assembly.GetExecutingAssembly().FullName).GetEnumValues();
                                    MaxKeyOptions = SelectFrom.Count();
                                }
                                else
                                {
                                    SelectFrom = Type.GetType(KeyToken["Enumeration"].ToString() + ", " + SelectionEnumAssembly).GetEnumNames();
                                    Selections = Type.GetType(KeyToken["Enumeration"].ToString() + ", " + SelectionEnumAssembly).GetEnumValues();
                                    MaxKeyOptions = SelectFrom.Count();
                                }
                            }
                            else
                            {
                                SelectFrom = (IEnumerable<object>)MethodManager.GetMethod((string)KeyToken["SelectionFunctionName"]).Invoke(KeyToken["SelectionFunctionType"], null);
                                MaxKeyOptions = SelectFrom.Count();
                            }
                        }
                        else if (KeyType == SettingsKeyType.SList)
                        {
                            TargetList = (IEnumerable<object>)MethodManager.GetMethod(ListFunctionName).Invoke(ListFunctionType, null);
                        }
                        if (KeyVarProperty is null)
                        {
                            if (FieldManager.CheckField(KeyVar))
                            {
                                // We're dealing with the field, get the value from it
                                KeyDefaultValue = FieldManager.GetValue(KeyVar);
                            }
                            else if (PropertyManager.CheckProperty(KeyVar))
                            {
                                // We're dealing with the property, get the value from it
                                KeyDefaultValue = PropertyManager.GetPropertyValue(KeyVar);
                            }

                            // Get the plain sequence from the color
                            if (KeyDefaultValue is Color valueColor)
                            {
                                KeyDefaultValue = valueColor.PlainSequence;
                            }
                        }
                        else
                        {
                            // Get the property value from variable
                            KeyDefaultValue = PropertyManager.GetPropertyValueInVariable(KeyVar, KeyVarProperty);
                        }
                    }

                    // If the type is boolean, write the two options
                    if (KeyType == SettingsKeyType.SBoolean)
                    {
                        TextWriterColor.WritePlain("", true);
                        MaxKeyOptions = 2;
                        TextWriterColor.Write(" 1) " + Translate.DoTranslation("Enable"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
                        TextWriterColor.Write(" 2) " + Translate.DoTranslation("Disable"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
                    }
                    TextWriterColor.WritePlain("", true);

                    // If the type is a color, initialize the color wheel
                    if (KeyType == SettingsKeyType.SColor)
                    {
                        ColorValue = ColorSelector.OpenColorSelector(new Color(KeyDefaultValue.ToString()));
                    }

                    // Write the list from the current items
                    if (KeyType == SettingsKeyType.SSelection)
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Current items:"), true, KernelColorTools.ColTypes.ListTitle);
                        ListWriterColor.WriteList(SelectFrom);
                        TextWriterColor.WritePlain("", true);
                    }
                    else if (KeyType == SettingsKeyType.SList)
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Current items:"), true, KernelColorTools.ColTypes.ListTitle);
                        ListWriterColor.WriteList(TargetList);
                        TextWriterColor.WritePlain("", true);
                    }

                    // Add an option to go back.
                    if (!(KeyType == SettingsKeyType.SVariant) & !(KeyType == SettingsKeyType.SInt) & !(KeyType == SettingsKeyType.SString) & !(KeyType == SettingsKeyType.SList) & !(KeyType == SettingsKeyType.SMaskedString) & !(KeyType == SettingsKeyType.SChar) & !(KeyType == SettingsKeyType.SIntSlider))
                    {
                        TextWriterColor.Write(" {0}) " + Translate.DoTranslation("Go Back...") + Kernel.Kernel.NewLine, true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.BackOption), MaxKeyOptions + 1);
                    }
                    else if (KeyType == SettingsKeyType.SList)
                    {
                        TextWriterColor.Write(Kernel.Kernel.NewLine + " q) " + Translate.DoTranslation("Save Changes...") + Kernel.Kernel.NewLine, true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), MaxKeyOptions + 1);
                    }

                    // Print debugging info
                    DebugWriter.Wdbg(DebugLevel.W, "Key {0} in section {1} has {2} selections.", KeyNumber, Section, MaxKeyOptions);
                    DebugWriter.Wdbg(DebugLevel.W, "Target variable: {0}, Key Type: {1}, Key value: {2}, Variant Value: {3}", KeyVar, KeyType, KeyValue, VariantValue);

                    // Prompt user
                    if (KeyType == SettingsKeyType.SVariant)
                    {
                        if (VariantFunctionSetsValue)
                        {
                            MethodManager.GetMethod(VariantFunction).Invoke(null, null);
                        }
                        else
                        {
                            TextWriterColor.Write("> ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
                            VariantValue = ConsoleBase.Inputs.Input.ReadLine();
                            if (NeutralizePaths)
                                VariantValue = Filesystem.NeutralizePath(Convert.ToString(VariantValue), NeutralizeRootPath);
                            DebugWriter.Wdbg(DebugLevel.I, "User answered {0}", VariantValue);
                        }
                    }
                    else if (KeyType == SettingsKeyType.SBoolean)
                    {
                        if (Convert.ToBoolean(FieldManager.GetValue(KeyVar)))
                        {
                            AnswerString = "2";
                        }
                        else
                        {
                            AnswerString = "1";
                        }
                        DebugWriter.Wdbg(DebugLevel.I, "User answered {0}", AnswerString);
                    }
                    else if (!(KeyType == SettingsKeyType.SVariant) & !(KeyType == SettingsKeyType.SColor))
                    {
                        if (KeyType == SettingsKeyType.SList)
                        {
                            TextWriterColor.Write("> ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
                            while (AnswerString != "q")
                            {
                                AnswerString = ConsoleBase.Inputs.Input.ReadLine();
                                if (!(AnswerString == "q"))
                                {
                                    if (NeutralizePaths)
                                        AnswerString = Filesystem.NeutralizePath(AnswerString, NeutralizeRootPath);
                                    if (!AnswerString.StartsWith("-"))
                                    {
                                        // We're not removing an item!
                                        TargetList = TargetList.Append(AnswerString);
                                    }
                                    else
                                    {
                                        // We're removing an item.
                                        var DeletedItems = Enumerable.Empty<object>();
                                        DeletedItems = DeletedItems.Append(AnswerString.Substring(1));
                                        TargetList = TargetList.Except(DeletedItems);
                                    }
                                    DebugWriter.Wdbg(DebugLevel.I, "Added answer {0} to list.", AnswerString);
                                    TextWriterColor.Write("> ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
                                }
                            }
                        }
                        else
                        {
                            // Make a prompt
                            if (KeyType == SettingsKeyType.SUnknown | KeyType == SettingsKeyType.SMaskedString)
                            {
                                TextWriterColor.Write("> ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
                            }
                            else if (!(KeyType == SettingsKeyType.SIntSlider))
                            {
                                TextWriterColor.Write("[{0}] > ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input), KeyDefaultValue);
                            }

                            // Select how to present input
                            if (KeyType == SettingsKeyType.SMaskedString)
                            {
                                AnswerString = ConsoleBase.Inputs.Input.ReadLineNoInput();
                            }
                            else if (KeyType == SettingsKeyType.SChar)
                            {
                                AnswerString = Convert.ToString(ConsoleBase.Inputs.Input.DetectKeypress().KeyChar);
                            }
                            else if (KeyType == SettingsKeyType.SIntSlider)
                            {
                                int CurrentValue = Convert.ToInt32(KeyDefaultValue);
                                ConsoleWrapper.CursorVisible = false;
                                while (PressedKey != ConsoleKey.Enter)
                                {
                                    // Draw the progress bar
                                    ProgressBarColor.WriteProgress(100d * (CurrentValue / (double)IntSliderMaximumValue), 4, ConsoleWrapper.WindowHeight - 4);

                                    // Show the current value
                                    TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Current value:") + " {0} / {1} - {2}" + Convert.ToString(Color255.GetEsc()) + "[0K", 5, ConsoleWrapper.WindowHeight - 5, false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), CurrentValue, IntSliderMinimumValue, IntSliderMaximumValue);

                                    // Parse the user input
                                    PressedKey = ConsoleBase.Inputs.Input.DetectKeypress().Key;
                                    switch (PressedKey)
                                    {
                                        case ConsoleKey.LeftArrow:
                                            {
                                                if (CurrentValue > IntSliderMinimumValue)
                                                    CurrentValue -= 1;
                                                break;
                                            }
                                        case ConsoleKey.RightArrow:
                                            {
                                                if (CurrentValue < IntSliderMaximumValue)
                                                    CurrentValue += 1;
                                                break;
                                            }
                                        case ConsoleKey.Enter:
                                            {
                                                AnswerString = CurrentValue.ToString();
                                                ConsoleWrapper.CursorVisible = true;
                                                break;
                                            }
                                    }
                                }
                            }
                            else
                            {
                                AnswerString = ConsoleBase.Inputs.Input.ReadLine();
                            }

                            // Neutralize answer path if required
                            if (NeutralizePaths)
                                AnswerString = Filesystem.NeutralizePath(AnswerString, NeutralizeRootPath);
                            DebugWriter.Wdbg(DebugLevel.I, "User answered {0}", AnswerString);
                        }
                    }

                    // Check for input
                    DebugWriter.Wdbg(DebugLevel.I, "Is the answer numeric? {0}", StringQuery.IsStringNumeric(AnswerString));
                    if (int.TryParse(AnswerString, out int AnswerInt))
                    {
                        // The answer is numeric! Now, check for types
                        switch (KeyType)
                        {
                            case SettingsKeyType.SBoolean:
                                {
                                    // We're dealing with boolean
                                    DebugWriter.Wdbg(DebugLevel.I, "Answer is numeric and key is of the Boolean type.");
                                    if (AnswerInt >= 1 & AnswerInt <= MaxKeyOptions)
                                    {
                                        DebugWriter.Wdbg(DebugLevel.I, "Translating {0} to the boolean equivalent...", AnswerInt);
                                        KeyFinished = true;

                                        // Set boolean
                                        switch (AnswerInt)
                                        {
                                            case 1: // True
                                                {
                                                    DebugWriter.Wdbg(DebugLevel.I, "Setting to True...");
                                                    FinalBool = true;
                                                    break;
                                                }
                                            case 2: // False
                                                {
                                                    DebugWriter.Wdbg(DebugLevel.I, "Setting to False...");
                                                    FinalBool = false;
                                                    break;
                                                }
                                        }

                                        // Now, set the value
                                        if (FieldManager.CheckField(KeyVar))
                                        {
                                            // We're dealing with the field
                                            FieldManager.SetValue(KeyVar, FinalBool, true);
                                        }
                                        else if (PropertyManager.CheckProperty(KeyVar))
                                        {
                                            // We're dealing with the property
                                            PropertyManager.SetPropertyValue(KeyVar, FinalBool);
                                        }
                                    }
                                    else if (AnswerInt == MaxKeyOptions + 1) // Go Back...
                                    {
                                        DebugWriter.Wdbg(DebugLevel.I, "User requested exit. Returning...");
                                        KeyFinished = true;
                                    }
                                    else
                                    {
                                        DebugWriter.Wdbg(DebugLevel.W, "Option is not valid. Returning...");
                                        TextWriterColor.Write(Translate.DoTranslation("Specified option {0} is invalid."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), AnswerInt);
                                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                                        ConsoleBase.Inputs.Input.DetectKeypress();
                                    }

                                    break;
                                }
                            case SettingsKeyType.SSelection:
                                {
                                    // We're dealing with selection
                                    DebugWriter.Wdbg(DebugLevel.I, "Answer is numeric and key is of the selection type.");
                                    int AnswerIndex = AnswerInt - 1;
                                    if (AnswerInt == MaxKeyOptions + 1) // Go Back...
                                    {
                                        DebugWriter.Wdbg(DebugLevel.I, "User requested exit. Returning...");
                                        KeyFinished = true;
                                    }
                                    else if (AnswerInt > 0)
                                    {
                                        if (Selections is not null)
                                        {
                                            DebugWriter.Wdbg(DebugLevel.I, "Setting variable {0} to item index {1}...", KeyVar, AnswerInt);
                                            KeyFinished = true;

                                            // Now, set the value
                                            if (FieldManager.CheckField(KeyVar))
                                            {
                                                // We're dealing with the field
                                                FieldManager.SetValue(KeyVar, Selections.GetValue(AnswerIndex), true);
                                            }
                                            else if (PropertyManager.CheckProperty(KeyVar))
                                            {
                                                // We're dealing with the property
                                                PropertyManager.SetPropertyValue(KeyVar, Selections.GetValue(AnswerIndex));
                                            }
                                        }
                                        else if (!(AnswerInt > MaxKeyOptions))
                                        {
                                            object FinalValue;
                                            if (!SelectionEnum)
                                            {
                                                DebugWriter.Wdbg(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt);
                                                KeyFinished = true;
                                                FinalValue = SelectFrom.ElementAtOrDefault(AnswerInt - 1);
                                            }
                                            else
                                            {
                                                DebugWriter.Wdbg(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt);
                                                KeyFinished = true;
                                                FinalValue = AnswerInt;
                                            }

                                            // Now, set the value
                                            if (FieldManager.CheckField(KeyVar))
                                            {
                                                // We're dealing with the field
                                                FieldManager.SetValue(KeyVar, FinalValue, true);
                                            }
                                            else if (PropertyManager.CheckProperty(KeyVar))
                                            {
                                                // We're dealing with the property
                                                PropertyManager.SetPropertyValue(KeyVar, FinalValue);
                                            }
                                        }
                                        else
                                        {
                                            DebugWriter.Wdbg(DebugLevel.W, "Answer is not valid.");
                                            TextWriterColor.Write(Translate.DoTranslation("The answer may not exceed the entries shown."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                                            TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                                            ConsoleBase.Inputs.Input.DetectKeypress();
                                        }
                                    }
                                    else if (AnswerInt == 0 & !SelectionEnumZeroBased)
                                    {
                                        DebugWriter.Wdbg(DebugLevel.W, "Zero is not allowed.");
                                        TextWriterColor.Write(Translate.DoTranslation("The answer may not be zero."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                                        ConsoleBase.Inputs.Input.DetectKeypress();
                                    }
                                    else
                                    {
                                        DebugWriter.Wdbg(DebugLevel.W, "Negative values are disallowed.");
                                        TextWriterColor.Write(Translate.DoTranslation("The answer may not be negative."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                                        ConsoleBase.Inputs.Input.DetectKeypress();
                                    }

                                    break;
                                }
                            case SettingsKeyType.SInt:
                                {
                                    // We're dealing with integers
                                    DebugWriter.Wdbg(DebugLevel.I, "Answer is numeric and key is of the integer type.");
                                    int AnswerIndex = AnswerInt - 1;
                                    if (AnswerInt >= 0)
                                    {
                                        DebugWriter.Wdbg(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt);
                                        KeyFinished = true;

                                        // Now, set the value
                                        if (FieldManager.CheckField(KeyVar))
                                        {
                                            // We're dealing with the field
                                            FieldManager.SetValue(KeyVar, AnswerInt, true);
                                        }
                                        else if (PropertyManager.CheckProperty(KeyVar))
                                        {
                                            // We're dealing with the property
                                            PropertyManager.SetPropertyValue(KeyVar, AnswerInt);
                                        }
                                    }
                                    else
                                    {
                                        DebugWriter.Wdbg(DebugLevel.W, "Negative values are disallowed.");
                                        TextWriterColor.Write(Translate.DoTranslation("The answer may not be negative."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                                        ConsoleBase.Inputs.Input.DetectKeypress();
                                    }

                                    break;
                                }
                            case SettingsKeyType.SIntSlider:
                                {
                                    // We're dealing with integers with limits
                                    DebugWriter.Wdbg(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt);
                                    KeyFinished = true;

                                    // Now, set the value
                                    if (FieldManager.CheckField(KeyVar))
                                    {
                                        // We're dealing with the field
                                        FieldManager.SetValue(KeyVar, AnswerInt, true);
                                    }
                                    else if (PropertyManager.CheckProperty(KeyVar))
                                    {
                                        // We're dealing with the property
                                        PropertyManager.SetPropertyValue(KeyVar, AnswerInt);
                                    }

                                    break;
                                }
                        }
                    }
                    else
                    {
                        switch (KeyType)
                        {
                            case SettingsKeyType.SString:
                            case SettingsKeyType.SMaskedString:
                            case SettingsKeyType.SChar:
                                {
                                    DebugWriter.Wdbg(DebugLevel.I, "Answer is not numeric and key is of the String or Char (inferred from keytype {0}) type. Setting variable...", KeyType.ToString());

                                    // Check to see if written answer is empty
                                    if (string.IsNullOrWhiteSpace(AnswerString))
                                    {
                                        DebugWriter.Wdbg(DebugLevel.I, "Answer is nothing. Setting to {0}...", KeyValue);
                                        AnswerString = Convert.ToString(KeyValue);
                                    }

                                    // Check to see if the user intended to clear the variable to make it consist of nothing
                                    if (AnswerString.ToLower() == "/clear")
                                    {
                                        DebugWriter.Wdbg(DebugLevel.I, "User requested clear.");
                                        AnswerString = "";
                                    }

                                    // Set the value
                                    KeyFinished = true;
                                    if (FieldManager.CheckField(KeyVar, true))
                                    {
                                        // We're dealing with the field
                                        FieldManager.SetValue(KeyVar, AnswerString, true);
                                    }
                                    else if (PropertyManager.CheckProperty(KeyVar))
                                    {
                                        // We're dealing with the property
                                        PropertyManager.SetPropertyValue(KeyVar, AnswerString);
                                    }

                                    break;
                                }
                            case SettingsKeyType.SList:
                                {
                                    string FinalDelimiter;
                                    DebugWriter.Wdbg(DebugLevel.I, "Answer is not numeric and key is of the List type. Adding answers to the list...");
                                    KeyFinished = true;

                                    // Get the delimiter
                                    if (ListJoinString is null)
                                    {
                                        FinalDelimiter = Convert.ToString(FieldManager.GetValue(ListJoinStringVariable));
                                    }
                                    else
                                    {
                                        FinalDelimiter = ListJoinString;
                                    }

                                    // Now, set the value
                                    string JoinedString = string.Join(FinalDelimiter, TargetList);
                                    if (FieldManager.CheckField(KeyVar))
                                    {
                                        // We're dealing with the field
                                        FieldManager.SetValue(KeyVar, JoinedString, true);
                                    }
                                    else if (PropertyManager.CheckProperty(KeyVar))
                                    {
                                        // We're dealing with the property
                                        PropertyManager.SetPropertyValue(KeyVar, JoinedString);
                                    }

                                    break;
                                }
                            case SettingsKeyType.SVariant:
                                {
                                    if (!VariantFunctionSetsValue)
                                    {
                                        // Now, set the value
                                        if (FieldManager.CheckField(KeyVar))
                                        {
                                            // We're dealing with the field
                                            FieldManager.SetValue(KeyVar, VariantValue, true);
                                        }
                                        else if (PropertyManager.CheckProperty(KeyVar))
                                        {
                                            // We're dealing with the property
                                            PropertyManager.SetPropertyValue(KeyVar, VariantValue);
                                        }
                                    }
                                    KeyFinished = true;
                                    break;
                                }
                            case SettingsKeyType.SColor:
                                {
                                    object FinalColor;
                                    if (FieldManager.GetField(KeyVar).FieldType == typeof(Color))
                                    {
                                        FinalColor = new Color(ColorValue.ToString());
                                    }
                                    else
                                    {
                                        FinalColor = ColorValue.ToString();
                                    }

                                    // Now, set the value
                                    if (FieldManager.CheckField(KeyVar))
                                    {
                                        // We're dealing with the field
                                        FieldManager.SetValue(KeyVar, FinalColor, true);
                                    }
                                    else if (PropertyManager.CheckProperty(KeyVar))
                                    {
                                        // We're dealing with the property
                                        PropertyManager.SetPropertyValue(KeyVar, FinalColor);
                                    }
                                    KeyFinished = true;
                                    break;
                                }
                            case SettingsKeyType.SUnknown:
                                {
                                    DebugWriter.Wdbg(DebugLevel.I, "User requested exit. Returning...");
                                    KeyFinished = true;
                                    break;
                                }

                            default:
                                {
                                    DebugWriter.Wdbg(DebugLevel.W, "Answer is not valid.");
                                    TextWriterColor.Write(Translate.DoTranslation("The answer is invalid. Check to make sure that the answer is numeric for config entries that need numbers as answers."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                                    TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                                    ConsoleBase.Inputs.Input.DetectKeypress();
                                    break;
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleWrapper.Clear();
                DebugWriter.Wdbg(DebugLevel.I, "Error trying to open section: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation(Section + " Settings...") + " > ???", true);
                TextWriterColor.Write(Kernel.Kernel.NewLine + "X) " + Translate.DoTranslation("Invalid section entered. Please go back."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                TextWriterColor.Write("X) " + Translate.DoTranslation("If you're sure that you've opened the right section, check this message out:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                TextWriterColor.Write("X) " + ex.Message, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                ConsoleBase.Inputs.Input.DetectKeypress();
            }
        }

        /// <summary>
        /// A sub for variable finding prompt
        /// </summary>
        public static void VariableFinder(JToken SettingsToken)
        {
            string SearchFor;
            List<string> Results;

            // Prompt the user
            TextWriterColor.Write(Translate.DoTranslation("Write what do you want to search for."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
            DebugWriter.Wdbg(DebugLevel.I, "Prompting user for searching...");
            TextWriterColor.Write(">> ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
            SearchFor = ConsoleBase.Inputs.Input.ReadLine();

            // Search for the setting
            Results = FindSetting(SearchFor, SettingsToken);

            // Write the settings
            if (!(Results.Count == 0))
            {
                // Populate answers
                var inputs = new List<InputChoiceInfo>();
                var inputsAlt = new List<InputChoiceInfo>() { new($"{Results.Count + 1}", Translate.DoTranslation("Go Back...")) };
                for (int resultIndex = 0, loopTo = Results.Count - 1; resultIndex <= loopTo; resultIndex++)
                {
                    string result = Results[resultIndex];
                    inputs.Add(new InputChoiceInfo($"{resultIndex + 1}", result));
                }

                // Prompt for selection
                int AnswerInt = SelectionStyle.PromptSelection(TermSeparator.RenderSeparator(Translate.DoTranslation("Welcome to Settings!"), true) + Kernel.Kernel.NewLine + Kernel.Kernel.NewLine + Translate.DoTranslation("Select section:"), inputs, inputsAlt);

                // Parse the input and go to setting
                int ChosenSettingIndex = AnswerInt - 1;
                string ChosenSetting = Results[ChosenSettingIndex];
                int SectionIndex = Convert.ToInt32(ChosenSetting.AsSpan().Slice(1, ChosenSetting.IndexOf("/") - 1).ToString()) - 1;
                int KeyNumber = Convert.ToInt32(ChosenSetting.AsSpan().Slice(ChosenSetting.IndexOf("/") + 1, ChosenSetting.IndexOf("]") - (ChosenSetting.IndexOf("/") + 1)).ToString());
                JProperty Section = (JProperty)SettingsToken.ToList()[SectionIndex];
                string SectionName = Section.Name;
                OpenKey(SectionName, KeyNumber, SettingsToken);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Nothing is found. Make sure that you've written the setting correctly."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                ConsoleBase.Inputs.Input.DetectKeypress();
            }
        }

        /// <summary>
        /// Finds a setting with the matching pattern
        /// </summary>
        public static List<string> FindSetting(string Pattern, JToken SettingsToken)
        {
            var Results = new List<string>();

            // Search the settings for the given pattern
            try
            {
                for (int SectionIndex = 0, loopTo = SettingsToken.Count() - 1; SectionIndex <= loopTo; SectionIndex++)
                {
                    var SectionToken = SettingsToken.ToList()[SectionIndex];
                    for (int SettingIndex = 0, loopTo1 = SectionToken.Count() - 1; SettingIndex <= loopTo1; SettingIndex++)
                    {
                        var SettingToken = SectionToken.ToList()[SettingIndex]["Keys"];
                        for (int KeyIndex = 0, loopTo2 = SettingToken.Count() - 1; KeyIndex <= loopTo2; KeyIndex++)
                        {
                            string KeyName = Translate.DoTranslation((string)SettingToken.ToList()[KeyIndex]["Name"]);
                            if (Regex.IsMatch(KeyName, Pattern, RegexOptions.IgnoreCase))
                            {
                                Results.Add($"[{SectionIndex + 1}/{KeyIndex + 1}] {KeyName}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed to find setting {0}: {1}", Pattern, ex.Message);
                DebugWriter.WStkTrc(ex);
            }

            // Return the results
            return Results;
        }

        /// <summary>
        /// Checks all the settings variables to see if they can be parsed
        /// </summary>
        public static Dictionary<string, bool> CheckSettingsVariables()
        {
            var SettingsToken = JToken.Parse(KernelResources.SettingsEntries);
            var SaverSettingsToken = JToken.Parse(KernelResources.ScreensaverSettingsEntries);
            var SplashSettingsToken = JToken.Parse(KernelResources.SplashSettingsEntries);
            JToken[] Tokens = [SettingsToken, SaverSettingsToken, SplashSettingsToken];
            var Results = new Dictionary<string, bool>();

            // Parse all the settings
            foreach (JToken Token in Tokens)
            {
                foreach (JProperty Section in Token.Cast<JProperty>())
                {
                    var SectionToken = Token[Section.Name];
                    foreach (JToken Key in SectionToken["Keys"])
                    {
                        string KeyName = (string)Key["Name"];
                        string KeyVariable = (string)Key["Variable"];
                        string KeyEnumeration = (string)Key["Enumeration"];
                        bool KeyEnumerationInternal = (bool)(Key["EnumerationInternal"] ?? false);
                        string KeyEnumerationAssembly = (string)Key["EnumerationAssembly"];
                        bool KeyFound;

                        // Check the variable
                        KeyFound = FieldManager.CheckField(KeyVariable) | PropertyManager.CheckProperty(KeyVariable);
                        Results.Add($"{KeyName}, {KeyVariable}", KeyFound);

                        // Check the enumeration
                        if (KeyEnumeration is not null)
                        {
                            bool Result;
                            if (KeyEnumerationInternal)
                            {
                                // Apparently, we need to have a full assembly name for getting types.
                                Result = Type.GetType("KS." + KeyEnumeration + ", " + Assembly.GetExecutingAssembly().FullName) is not null;
                            }
                            else
                            {
                                Result = Type.GetType(KeyEnumeration + ", " + KeyEnumerationAssembly) is not null;
                            }
                            Results.Add($"{KeyName}, {KeyVariable}, {KeyEnumeration}", Result);
                        }
                    }
                }
            }

            // Return the results
            return Results;
        }

        /// <summary>
        /// Open the settings resource
        /// </summary>
        /// <param name="SettingsType">The settings type</param>
        private static JToken OpenSettingsResource(SettingsType SettingsType)
        {
            switch (SettingsType)
            {
                case SettingsType.Normal:
                    {
                        return JToken.Parse(KernelResources.SettingsEntries);
                    }
                case SettingsType.Screensaver:
                    {
                        return JToken.Parse(KernelResources.ScreensaverSettingsEntries);
                    }
                case SettingsType.Splash:
                    {
                        return JToken.Parse(KernelResources.SplashSettingsEntries);
                    }

                default:
                    {
                        return JToken.Parse(KernelResources.SettingsEntries);
                    }
            }
        }

    }
}
