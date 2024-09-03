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

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.ManPages;
using KS.Misc.Reflection;
using KS.Misc.Screensaver.Customized;
using KS.Misc.Splash;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Modifications
{
    public static class ModParser
    {

        /// <summary>
        /// Gets the mod instance from compiled assembly
        /// </summary>
        /// <param name="Assembly">An assembly</param>
        public static IScript GetModInstance(Assembly Assembly)
        {
            foreach (Type t in Assembly.GetTypes())
            {
                if (t.GetInterface(typeof(IScript).Name) is not null)
                    return (IScript)Assembly.CreateInstance(t.FullName);
            }
            return null;
        }

        /// <summary>
        /// Starts to parse the mod, and configures it so it can be used
        /// </summary>
        /// <param name="modFile">Mod file name with extension. It should end with .dll</param>
        public static void ParseMod(string modFile)
        {
            string ModPath = Paths.GetKernelPath(KernelPathType.Mods);
            if (modFile.EndsWith(".dll"))
            {
                // Mod is a dynamic DLL
                try
                {
                    // Check to see if the DLL is actually a mod
                    var script = GetModInstance(Assembly.LoadFrom(ModPath + modFile));

                    // Check to see if the DLL is actually a screensaver
                    if (script is null)
                        CustomSaverParser.ParseCustomSaver(ModPath + modFile);

                    // If we didn't find anything, abort
                    if (script is null)
                        throw new InvalidModException(Translate.DoTranslation("The modfile is invalid."));

                    // Finalize the mod
                    FinalizeMods(script, modFile);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", modFile, ex.Message);
                    DebugWriter.WStkTrc(ex);
                    SplashReport.ReportProgress(Translate.DoTranslation("Mod can't be loaded because of the following: "), 0, KernelColorTools.ColTypes.Error);
                    foreach (Exception LoaderException in ex.LoaderExceptions)
                    {
                        DebugWriter.Wdbg(DebugLevel.E, "Loader exception: {0}", LoaderException.Message);
                        DebugWriter.WStkTrc(LoaderException);
                        SplashReport.ReportProgress(LoaderException.Message, 0, KernelColorTools.ColTypes.Error);
                    }
                    SplashReport.ReportProgress(Translate.DoTranslation("Contact the vendor of the mod to upgrade the mod to the compatible version."), 0, KernelColorTools.ColTypes.Error);
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", modFile, ex.Message);
                    DebugWriter.WStkTrc(ex);
                    SplashReport.ReportProgress(Translate.DoTranslation("Mod can't be loaded because of the following: ") + ex.Message, 0, KernelColorTools.ColTypes.Error);
                }
            }
            else
            {
                // Ignore unsupported files
                DebugWriter.Wdbg(DebugLevel.W, "Unsupported file type for mod file {0}.", modFile);
            }
        }

        /// <summary>
        /// Configures the mod so it can be used
        /// </summary>
        /// <param name="script">Instance of script</param>
        /// <param name="modFile">Mod file name with extension. It should end with .dll</param>
        public static void FinalizeMods(IScript script, string modFile)
        {
            var ModParts = new Dictionary<string, PartInfo>();
            ModInfo ModInstance;
            PartInfo PartInstance;

            // Try to finalize mod
            if (script is not null)
            {
                string ModPath = Paths.GetKernelPath(KernelPathType.Mods);
                Kernel.Kernel.KernelEventManager.RaiseModParsed(modFile);
                try
                {
                    // Add mod dependencies folder (if any) to the private appdomain lookup folder
                    string ModDepPath = ModPath + "Deps/" + Path.GetFileNameWithoutExtension(modFile) + "-" + FileVersionInfo.GetVersionInfo(ModPath + modFile).FileVersion + "/";
                    AssemblyLookup.AddPathToAssemblySearchPath(ModDepPath);

                    // Start the mod
                    script.StartMod();
                    DebugWriter.Wdbg(DebugLevel.I, "script.StartMod() initialized. Mod name: {0} | Mod part: {1} | Version: {2}", script.Name, script.ModPart, script.Version);

                    // See if the mod has part name
                    if (string.IsNullOrWhiteSpace(script.ModPart))
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "No part name for {0}", modFile);
                        SplashReport.ReportProgress(Translate.DoTranslation("Mod {0} does not have the part name. Mod parsing failed. Review the source code."), 0, KernelColorTools.ColTypes.Error, modFile);
                        return;
                    }

                    // See if the commands in a mod are valid
                    if (script.Commands is not null)
                    {
                        foreach (string Command in script.Commands.Keys)
                        {
                            if (string.IsNullOrWhiteSpace(Command))
                            {
                                DebugWriter.Wdbg(DebugLevel.W, "No command for {0}", modFile);
                                SplashReport.ReportProgress(Translate.DoTranslation("Mod {0} has invalid command. Mod parsing failed. Review the source code."), 0, KernelColorTools.ColTypes.Error, modFile);
                                return;
                            }
                        }
                    }

                    // See if the mod has name
                    string ModName = script.Name;
                    if (string.IsNullOrWhiteSpace(ModName))
                    {
                        // Mod has no name! Give it a file name.
                        ModName = modFile;
                        DebugWriter.Wdbg(DebugLevel.W, "No name for {0}", modFile);
                        SplashReport.ReportProgress(Translate.DoTranslation("Mod {0} does not have the name. Review the source code."), 0, KernelColorTools.ColTypes.Warning, modFile);
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "There is a name for {0}", modFile);
                    }
                    DebugWriter.Wdbg(DebugLevel.I, "Mod name: {0}", ModName);

                    // Check to see if there is a part under the same name.
                    var Parts = ModManager.Mods.ContainsKey(ModName) ? ModManager.Mods[ModName].ModParts : ModParts;
                    DebugWriter.Wdbg(DebugLevel.I, "Adding mod part {0}...", script.ModPart);
                    if (Parts.ContainsKey(script.ModPart))
                    {
                        // Append the number to the end of the name
                        DebugWriter.Wdbg(DebugLevel.W, "There is a conflict with {0}. Appending item number...", script.ModPart);
                        script.ModPart = $"{script.ModPart} [{Parts.Count}]";
                    }

                    // Now, add the part
                    PartInstance = new PartInfo(ModName, script.ModPart, modFile, Filesystem.NeutralizePath(modFile, ModPath), script);
                    Parts.Add(script.ModPart, PartInstance);
                    ModInstance = new ModInfo(ModName, modFile, Filesystem.NeutralizePath(modFile, ModPath), Parts, script.Version);
                    if (!ModManager.Mods.ContainsKey(ModName))
                        ModManager.Mods.Add(ModName, ModInstance);

                    // See if the mod has version
                    if (string.IsNullOrWhiteSpace(script.Version) & !string.IsNullOrWhiteSpace(script.Name))
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "{0}.Version = \"\" | {0}.Name = {1}", modFile, script.Name);
                        SplashReport.ReportProgress(Translate.DoTranslation("Mod {0} does not have the version."), 0, KernelColorTools.ColTypes.Warning, script.Name);
                    }
                    else if (!string.IsNullOrWhiteSpace(script.Name) & !string.IsNullOrWhiteSpace(script.Version))
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "{0}.Version = {2} | {0}.Name = {1}", modFile, script.Name, script.Version);
                        SplashReport.ReportProgress(Translate.DoTranslation("{0} v{1} started") + " ({2})", 0, KernelColorTools.ColTypes.Success, script.Name, script.Version, script.ModPart);
                    }

                    // Process the commands that are defined in a mod
                    if (script.Commands is not null)
                    {
                        for (int i = 0, loopTo = script.Commands.Keys.Count - 1; i <= loopTo; i++)
                        {
                            // See if the command conflicts with pre-existing shell commands
                            string Command = script.Commands.Keys.ElementAtOrDefault(i);
                            string ActualCommand = Command;
                            CommandType CommandType = (CommandType)script.Commands.Values.ElementAtOrDefault(i).Type;
                            DebugWriter.Wdbg(DebugLevel.I, "Command type: {0}", CommandType);
                            if (CommandManager.IsCommandFound(Command, (ShellType)CommandType) | ModManager.ListModCommands((ShellType)CommandType).ContainsKey(Command))
                            {
                                DebugWriter.Wdbg(DebugLevel.W, "Command {0} conflicts with available shell commands or mod commands. Appending \"-{1}-{2}\" to end of command...", Command, script.Name, script.ModPart);
                                Command += $"-{script.Name}-{script.ModPart}";
                            }

                            // See if mod can be added to command list
                            if (!string.IsNullOrEmpty(Command))
                            {
                                if (string.IsNullOrEmpty(script.Commands[ActualCommand].HelpDefinition))
                                {
                                    SplashReport.ReportProgress(Translate.DoTranslation("No definition for command {0}."), 0, KernelColorTools.ColTypes.Warning, Command);
                                    DebugWriter.Wdbg(DebugLevel.W, "{0}.Def = Nothing, {0}.Def = \"Command defined by {1} ({2})\"", Command, script.Name, script.ModPart);
                                    script.Commands[ActualCommand].HelpDefinition = Translate.DoTranslation("Command defined by ") + script.Name + " (" + script.ModPart + ")";
                                }

                                // Now, add the command to the mod list
                                DebugWriter.Wdbg(DebugLevel.I, "Adding command {0} for {1}...", Command, CommandType.ToString());
                                if (!ModManager.ListModCommands((ShellType)CommandType).ContainsKey(Command))
                                    ModManager.ListModCommands((ShellType)CommandType).Add(Command, script.Commands[ActualCommand]);
                                var cmd = script.Commands[ActualCommand];
                                script.Commands.Remove(ActualCommand);
                                script.Commands.Add(Command, cmd);
                            }
                        }
                    }

                    // Check for accompanying manual pages for mods
                    string ModManualPath = Filesystem.NeutralizePath(modFile + ".manual", ModPath);
                    if (Checking.FolderExists(ModManualPath))
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Found manual page collection in {0}", ModManualPath);
                        foreach (string ModManualFile in Directory.EnumerateFiles(ModManualPath, "*.man", SearchOption.AllDirectories))
                            PageParser.InitMan(ModManualFile);
                    }

                    // Raise event
                    Kernel.Kernel.KernelEventManager.RaiseModFinalized(modFile);
                }
                catch (Exception ex)
                {
                    Kernel.Kernel.KernelEventManager.RaiseModFinalizationFailed(modFile, ex.Message);
                    DebugWriter.WStkTrc(ex);
                    SplashReport.ReportProgress(Translate.DoTranslation("Failed to finalize mod {0}: {1}"), 0, KernelColorTools.ColTypes.Error, modFile, ex.Message);
                }
            }
            else
            {
                Kernel.Kernel.KernelEventManager.RaiseModParseError(modFile);
            }
        }

    }
}