﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using Terminaux.Inputs.Styles.Selection;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terminaux.Reader;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles;
using Nitrocid.Kernel.Exceptions;

namespace Nitrocid.Kernel.Configuration.Settings.KeyInputs
{
    internal class SelectionSettingsKeyInput : ISettingsKeyInput
    {
        bool SelectionEnum;
        string? SelectionEnumAssembly;
        bool SelectionEnumInternal;
        bool SelectionFunctionDict;
        string? ListFunctionName;
        bool SelectionEnumZeroBased;
        Type? SelectionEnumType = default;
        IEnumerable<object>? SelectFrom;
        string[]? selectFallbacks;
        object? Selections;

        public object? PromptForSet(SettingsKey key, object? KeyDefaultValue, out bool bail)
        {
            PopulateInfo(key);

            // Populate items
            if (SelectFrom is null)
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("There is nothing to select from."));
            int MaxKeyOptions = SelectFrom.Count();
            var items = new List<(string, string)>();
            var altItems = new List<(string, string)>()
            {
                ($"{MaxKeyOptions + 1}", Translate.DoTranslation("Go Back..."))
            };

            // Since there is no way to index the SelectFrom enumerable, we have to manually initialize a counter. Ugly!
            int itemCount = 1;
            foreach (var item in SelectFrom)
            {
                items.Add(($"{itemCount}", item.ToString() ?? ""));
                itemCount++;
            }

            // Prompt user and check for input
            string keyName = Translate.DoTranslation(key.Name);
            string keyDesc = Translate.DoTranslation(key.Description);
            string finalSection = SettingsApp.RenderHeader(keyName, keyDesc);
            int Answer = SelectionStyle.PromptSelection(finalSection,
                InputChoiceTools.GetInputChoices([.. items]),
                InputChoiceTools.GetInputChoices([.. altItems]));
            bail = true;
            return Answer;
        }

        public object? TranslateStringValue(SettingsKey key, string value)
        {
            PopulateInfo(key);
            if (SelectFrom is null)
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("There is nothing to select from."));

            if (string.IsNullOrEmpty(value))
                return 0;
            if (int.TryParse(value, out int answer))
                return answer > 0 && answer <= SelectFrom.Count() ? answer : 0;
            return 0;
        }

        public object? TranslateStringValueWithDefault(SettingsKey key, string value, object? KeyDefaultValue)
        {
            PopulateInfo(key);
            if (SelectFrom is null)
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("There is nothing to select from."));

            if (string.IsNullOrEmpty(value))
                return (int?)KeyDefaultValue;
            if (int.TryParse(value, out int answer))
                return answer > 0 && answer <= SelectFrom.Count() ? answer : (int?)KeyDefaultValue;
            return (int?)KeyDefaultValue;
        }

        public void SetValue(SettingsKey key, object? value, BaseKernelConfig configType)
        {
            PopulateInfo(key);
            if (SelectFrom is null)
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("There is nothing to select from."));

            // We're dealing with selection
            DebugWriter.WriteDebug(DebugLevel.I, "Answer is numeric and key is of the selection type.");
            if (value is not int AnswerInt)
            {
                SettingsAppTools.SetPropertyValue(key.Variable, value, configType);
                return;
            }

            // Now, check for input
            int MaxKeyOptions = SelectFrom.Count();
            int AnswerIndex = AnswerInt - 1;
            if (AnswerInt == MaxKeyOptions + 1 || AnswerInt == -1) // Go Back...
            {
                DebugWriter.WriteDebug(DebugLevel.I, "User requested exit. Returning...");
                return;
            }
            else if (AnswerInt > 0)
            {
                if (Selections is IEnumerable<object> selectionsArray)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0} to item index {1}...", key.Variable, AnswerInt);

                    // Now, set the value
                    SettingsAppTools.SetPropertyValue(key.Variable, selectionsArray.ToArray()[AnswerIndex], configType);
                }
                else if (AnswerInt <= MaxKeyOptions)
                {
                    object? FinalValue;
                    DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0} to {1}...", key.Variable, AnswerInt);
                    FinalValue = SelectFrom.ElementAtOrDefault(AnswerInt - 1);
                    if (SelectionEnum && SelectionEnumType is not null)
                        FinalValue = Enum.Parse(SelectionEnumType, FinalValue?.ToString() ?? "");

                    // Now, set the value
                    SettingsAppTools.SetPropertyValue(key.Variable, FinalValue, configType);
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Answer is not valid.");
                    TextWriters.Write(Translate.DoTranslation("The answer may not exceed the entries shown."), true, KernelColorType.Error);
                    TextWriters.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                    Input.ReadKey();
                }
            }
            else if (AnswerInt == 0 & !SelectionEnumZeroBased)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Zero is not allowed.");
                TextWriters.Write(Translate.DoTranslation("The answer may not be zero."), true, KernelColorType.Error);
                TextWriters.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                Input.ReadKey();
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Negative values are disallowed.");
                TextWriters.Write(Translate.DoTranslation("The answer may not be negative."), true, KernelColorType.Error);
                TextWriters.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                Input.ReadKey();
            }
        }

        private void PopulateInfo(SettingsKey key)
        {
            SelectionEnum = key.IsEnumeration;
            SelectionEnumAssembly = key.EnumerationAssembly;
            SelectionEnumInternal = key.EnumerationInternal;
            SelectionFunctionDict = key.IsSelectionFunctionDict;
            ListFunctionName = key.SelectionFunctionName;
            SelectionEnumZeroBased = key.EnumerationZeroBased;
            selectFallbacks = key.SelectionFallback;
            SelectionEnumType = default;

            // Determine which list we're going to select
            if (SelectionEnum)
            {
                var enumeration = key.Enumeration;
                if (SelectionEnumInternal)
                {
                    // Apparently, we need to have a full assembly name for getting types.
                    SelectionEnumType = Type.GetType($"{KernelMain.rootNameSpace}.{enumeration}, {Assembly.GetExecutingAssembly().FullName}") ??
                        throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't get type from") + $" {enumeration}");
                    SelectFrom = SelectionEnumType.GetEnumNames();
                    Selections = SelectionEnumType.GetEnumValues();
                }
                else
                {
                    SelectionEnumType = Type.GetType(enumeration + ", " + SelectionEnumAssembly) ??
                        throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't get type from") + $" {enumeration}");
                    SelectFrom = SelectionEnumType.GetEnumNames();
                    Selections = SelectionEnumType.GetEnumValues();
                }
            }
            else
            {
                var listObj = MethodManager.InvokeMethodStatic(ListFunctionName);
                if (SelectionFunctionDict)
                {
                    if (listObj is null || listObj is IEnumerable<object> objs && !objs.Any())
                        SelectFrom = selectFallbacks;
                    else
                        SelectFrom = (IEnumerable<object>)((IDictionary)listObj).Keys;
                }
                else
                {
                    if (listObj is null || listObj is IEnumerable<object> objs && !objs.Any())
                        SelectFrom = selectFallbacks;
                    else
                        SelectFrom = (IEnumerable<object>)listObj;
                }
            }
        }

    }
}
