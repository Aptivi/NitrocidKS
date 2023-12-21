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

using System.IO;
using System.Linq;
using KS.Files;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.Misc.Configuration
{
    public static class ConfigTools
    {

        /// <summary>
        /// Reloads config
        /// </summary>
        public static void ReloadConfig()
        {
            Kernel.Kernel.KernelEventManager.RaisePreReloadConfig();
            Config.InitializeConfig();
            Kernel.Kernel.KernelEventManager.RaisePostReloadConfig();
        }

        /// <summary>
        /// Reloads config
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryReloadConfig()
        {
            try
            {
                ReloadConfig();
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed to reload config: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the config needs repair and repairs it as necessary.
        /// </summary>
        /// <returns>True if the config is repaired; False if no repairs done; Throws exceptions if unsuccessful.</returns>
        public static bool RepairConfig()
        {
            // Variables
            var FixesNeeded = default(bool);
            var PristineConfigObject = Config.GetNewConfigObject();

            // General sections
            int ExpectedSections = PristineConfigObject.Count;
            int ExpectedGeneralKeys = PristineConfigObject["General"].Count();
            int ExpectedColorsKeys = PristineConfigObject["Colors"].Count();
            int ExpectedHardwareKeys = PristineConfigObject["Hardware"].Count();
            int ExpectedLoginKeys = PristineConfigObject["Login"].Count();
            int ExpectedShellKeys = PristineConfigObject["Shell"].Count();
            int ExpectedFilesystemKeys = PristineConfigObject["Filesystem"].Count();
            int ExpectedNetworkKeys = PristineConfigObject["Network"].Count();
            int ExpectedScreensaverKeys = PristineConfigObject["Screensaver"].Count();
            int ExpectedSplashKeys = PristineConfigObject["Splash"].Count();
            int ExpectedMiscKeys = PristineConfigObject["Misc"].Count();

            // Individual screensaver keys
            int ExpectedScreensaverColorMixKeys = PristineConfigObject["Screensaver"]["ColorMix"].Count();
            int ExpectedScreensaverDiscoKeys = PristineConfigObject["Screensaver"]["Disco"].Count();
            int ExpectedScreensaverGlitterColorKeys = PristineConfigObject["Screensaver"]["GlitterColor"].Count();
            int ExpectedScreensaverLinesKeys = PristineConfigObject["Screensaver"]["Lines"].Count();
            int ExpectedScreensaverDissolveKeys = PristineConfigObject["Screensaver"]["Dissolve"].Count();
            int ExpectedScreensaverBouncingBlockKeys = PristineConfigObject["Screensaver"]["BouncingBlock"].Count();
            int ExpectedScreensaverBouncingTextKeys = PristineConfigObject["Screensaver"]["BouncingText"].Count();
            int ExpectedScreensaverProgressClockKeys = PristineConfigObject["Screensaver"]["ProgressClock"].Count();
            int ExpectedScreensaverLighterKeys = PristineConfigObject["Screensaver"]["Lighter"].Count();
            int ExpectedScreensaverWipeKeys = PristineConfigObject["Screensaver"]["Wipe"].Count();
            int ExpectedScreensaverMatrixKeys = PristineConfigObject["Screensaver"]["Matrix"].Count();
            int ExpectedScreensaverGlitterMatrixKeys = PristineConfigObject["Screensaver"]["GlitterMatrix"].Count();
            int ExpectedScreensaverFaderKeys = PristineConfigObject["Screensaver"]["Fader"].Count();
            int ExpectedScreensaverFaderBackKeys = PristineConfigObject["Screensaver"]["FaderBack"].Count();
            int ExpectedScreensaverBeatFaderKeys = PristineConfigObject["Screensaver"]["BeatFader"].Count();
            int ExpectedScreensaverTypoKeys = PristineConfigObject["Screensaver"]["Typo"].Count();
            int ExpectedScreensaverMarqueeKeys = PristineConfigObject["Screensaver"]["Marquee"].Count();
            int ExpectedScreensaverLinotypoKeys = PristineConfigObject["Screensaver"]["Linotypo"].Count();
            int ExpectedScreensaverTypewriterKeys = PristineConfigObject["Screensaver"]["Typewriter"].Count();
            int ExpectedScreensaverFlashColorKeys = PristineConfigObject["Screensaver"]["FlashColor"].Count();
            int ExpectedScreensaverSpotWriteKeys = PristineConfigObject["Screensaver"]["SpotWrite"].Count();
            int ExpectedScreensaverRampKeys = PristineConfigObject["Screensaver"]["Ramp"].Count();
            int ExpectedScreensaverStackBoxKeys = PristineConfigObject["Screensaver"]["StackBox"].Count();
            int ExpectedScreensaverSnakerKeys = PristineConfigObject["Screensaver"]["Snaker"].Count();
            int ExpectedScreensaverBarRotKeys = PristineConfigObject["Screensaver"]["BarRot"].Count();
            int ExpectedScreensaverFireworksKeys = PristineConfigObject["Screensaver"]["Fireworks"].Count();
            int ExpectedScreensaverFigletKeys = PristineConfigObject["Screensaver"]["Figlet"].Count();

            // Individual splash keys
            int ExpectedSplashSimpleKeys = PristineConfigObject["Splash"]["Simple"].Count();
            int ExpectedSplashProgressKeys = PristineConfigObject["Splash"]["Progress"].Count();

            // Check for missing sections
            if (Config.ConfigToken.Count != ExpectedSections)
            {
                DebugWriter.Wdbg(DebugLevel.W, "Missing sections. Config fix needed set to true.");
                FixesNeeded = true;
            }
            if (Config.ConfigToken["Screensaver"] is not null)
            {
                if (Config.ConfigToken["Screensaver"].Count() != ExpectedScreensaverKeys)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Missing sections and/or keys in Screensaver. Config fix needed set to true.");
                    FixesNeeded = true;
                }
            }
            if (Config.ConfigToken["Splash"] is not null)
            {
                if (Config.ConfigToken["Splash"].Count() != ExpectedSplashKeys)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Missing sections and/or keys in Splash. Config fix needed set to true.");
                    FixesNeeded = true;
                }
            }

            // Now, check for missing keys in each section that ARE available.
            if (Config.ConfigToken["General"] is not null)
            {
                if (Config.ConfigToken["General"].Count() != ExpectedGeneralKeys)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Missing keys in General. Config fix needed set to true.");
                    FixesNeeded = true;
                }
            }
            if (Config.ConfigToken["Colors"] is not null)
            {
                if (Config.ConfigToken["Colors"].Count() != ExpectedColorsKeys)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Missing keys in  Config fix needed set to true.");
                    FixesNeeded = true;
                }
            }
            if (Config.ConfigToken["Hardware"] is not null)
            {
                if (Config.ConfigToken["Hardware"].Count() != ExpectedHardwareKeys)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Hardware. Config fix needed set to true.");
                    FixesNeeded = true;
                }
            }
            if (Config.ConfigToken["Login"] is not null)
            {
                if (Config.ConfigToken["Login"].Count() != ExpectedLoginKeys)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Login. Config fix needed set to true.");
                    FixesNeeded = true;
                }
            }
            if (Config.ConfigToken["Shell"] is not null)
            {
                if (Config.ConfigToken["Shell"].Count() != ExpectedShellKeys)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Shell. Config fix needed set to true.");
                    FixesNeeded = true;
                }
            }
            if (Config.ConfigToken["Filesystem"] is not null)
            {
                if (Config.ConfigToken["Filesystem"].Count() != ExpectedFilesystemKeys)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Filesystem. Config fix needed set to true.");
                    FixesNeeded = true;
                }
            }
            if (Config.ConfigToken["Network"] is not null)
            {
                if (Config.ConfigToken["Network"].Count() != ExpectedNetworkKeys)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Network. Config fix needed set to true.");
                    FixesNeeded = true;
                }
            }
            if (Config.ConfigToken["Screensaver"] is not null)
            {
                if (Config.ConfigToken["Screensaver"]["ColorMix"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["ColorMix"].Count() != ExpectedScreensaverColorMixKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > ColorMix. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["Disco"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["Disco"].Count() != ExpectedScreensaverDiscoKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > Disco. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["GlitterColor"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["GlitterColor"].Count() != ExpectedScreensaverGlitterColorKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > GlitterColor. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["Lines"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["Lines"].Count() != ExpectedScreensaverLinesKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > Lines. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["Dissolve"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["Dissolve"].Count() != ExpectedScreensaverDissolveKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > Dissolve. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["BouncingBlock"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["BouncingBlock"].Count() != ExpectedScreensaverBouncingBlockKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > BouncingBlock. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["BouncingText"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["BouncingText"].Count() != ExpectedScreensaverBouncingTextKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > BouncingText. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["ProgressClock"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["ProgressClock"].Count() != ExpectedScreensaverProgressClockKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > ProgressClock. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["Lighter"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["Lighter"].Count() != ExpectedScreensaverLighterKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > Lighter. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["Wipe"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["Wipe"].Count() != ExpectedScreensaverWipeKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > Wipe. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["Matrix"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["Matrix"].Count() != ExpectedScreensaverMatrixKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > Matrix. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["GlitterMatrix"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["GlitterMatrix"].Count() != ExpectedScreensaverGlitterMatrixKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > GlitterMatrix. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["Fader"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["Fader"].Count() != ExpectedScreensaverFaderKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > Fader. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["FaderBack"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["FaderBack"].Count() != ExpectedScreensaverFaderBackKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > FaderBack. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["BeatFader"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["BeatFader"].Count() != ExpectedScreensaverBeatFaderKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > BeatFader. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["Typo"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["Typo"].Count() != ExpectedScreensaverTypoKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > Typo. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["Marquee"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["Marquee"].Count() != ExpectedScreensaverMarqueeKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > Marquee. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["Linotypo"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["Linotypo"].Count() != ExpectedScreensaverLinotypoKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > Linotypo. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["Typewriter"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["Typewriter"].Count() != ExpectedScreensaverTypewriterKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > Typewriter. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["FlashColor"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["FlashColor"].Count() != ExpectedScreensaverFlashColorKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > FlashColor. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["SpotWrite"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["SpotWrite"].Count() != ExpectedScreensaverSpotWriteKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > SpotWrite. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["Ramp"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["Ramp"].Count() != ExpectedScreensaverRampKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > Ramp. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["StackBox"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["StackBox"].Count() != ExpectedScreensaverStackBoxKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > StackBox. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["Snaker"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["Snaker"].Count() != ExpectedScreensaverSnakerKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > Snaker. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["BarRot"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["BarRot"].Count() != ExpectedScreensaverBarRotKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > BarRot. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["Fireworks"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["Fireworks"].Count() != ExpectedScreensaverFireworksKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > Fireworks. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Screensaver"]["Figlet"] is not null)
                {
                    if (Config.ConfigToken["Screensaver"]["Figlet"].Count() != ExpectedScreensaverFigletKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Screensaver > Figlet. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
            }
            if (Config.ConfigToken["Splash"] is not null)
            {
                if (Config.ConfigToken["Splash"]["Simple"] is not null)
                {
                    if (Config.ConfigToken["Splash"]["Simple"].Count() != ExpectedSplashSimpleKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Splash > Simple. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
                if (Config.ConfigToken["Splash"]["Progress"] is not null)
                {
                    if (Config.ConfigToken["Splash"]["Progress"].Count() != ExpectedSplashProgressKeys)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Splash > Progress. Config fix needed set to true.");
                        FixesNeeded = true;
                    }
                }
            }
            if (Config.ConfigToken["Misc"] is not null)
            {
                if (Config.ConfigToken["Misc"].Count() != ExpectedMiscKeys)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Missing keys in Misc. Config fix needed set to true.");
                    FixesNeeded = true;
                }
            }

            // If the fixes are needed, try to remake config with parsed values
            if (FixesNeeded)
                Config.CreateConfig();
            return FixesNeeded;
        }

        /// <summary>
        /// Gets the JSON token for specific configuration category with an optional sub-category
        /// </summary>
        /// <param name="ConfigCategory">Config category</param>
        /// <param name="ConfigSubCategoryName">Sub-category name. Should be an Object. Currently used for screensavers</param>
        public static JToken GetConfigCategory(Config.ConfigCategory ConfigCategory, string ConfigSubCategoryName = "")
        {
            // Try to parse the config category
            DebugWriter.Wdbg(DebugLevel.I, "Parsing config category {0}...", ConfigCategory);
            if (Enum.TryParse(((int)ConfigCategory).ToString(), out ConfigCategory))
            {
                // We got a valid category. Now, get the token for the specific category
                DebugWriter.Wdbg(DebugLevel.I, "Category {0} found! Parsing sub-category {1} ({2})...", ConfigCategory, ConfigSubCategoryName, ConfigSubCategoryName.Length);
                var CategoryToken = Config.ConfigToken[ConfigCategory.ToString()];

                // Try to get the sub-category token and check to see if it's found or not
                var SubCategoryToken = CategoryToken[ConfigSubCategoryName];
                if (!string.IsNullOrWhiteSpace(ConfigSubCategoryName) & SubCategoryToken is not null)
                {
                    // We got the subcategory! Check to see if it's really a sub-category (Object) or not
                    DebugWriter.Wdbg(DebugLevel.I, "Sub-category {0} found! Is it really the sub-category? Type = {1}", ConfigSubCategoryName, SubCategoryToken.Type);
                    if (SubCategoryToken.Type == JTokenType.Object)
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "It is really a sub-category!");
                        return SubCategoryToken;
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "It is not really a sub-category. Returning master category...");
                        return CategoryToken;
                    }
                }
                else
                {
                    // We only got the full category.
                    DebugWriter.Wdbg(DebugLevel.I, "Returning master category...");
                    return CategoryToken;
                }
            }
            else
            {
                // We didn't get a category.
                DebugWriter.Wdbg(DebugLevel.E, "Category {0} not found!", ConfigCategory);
                throw new Kernel.Exceptions.ConfigException(Translate.DoTranslation("Config category {0} not found."), ConfigCategory);
            }
        }

        /// <summary>
        /// Sets the value of an entry in a category.
        /// </summary>
        /// <param name="ConfigCategory">Config category</param>
        /// <param name="ConfigEntryName">Config entry name.</param>
        /// <param name="ConfigValue">Config entry value to install</param>
        public static void SetConfigValue(Config.ConfigCategory ConfigCategory, string ConfigEntryName, JToken ConfigValue)
        {
            SetConfigValue(ConfigCategory, GetConfigCategory(ConfigCategory), ConfigEntryName, ConfigValue);
        }

        /// <summary>
        /// Sets the value of an entry in a category.
        /// </summary>
        /// <param name="ConfigCategory">Config category</param>
        /// <param name="ConfigEntryName">Config entry name.</param>
        /// <param name="ConfigValue">Config entry value to install</param>
        public static void SetConfigValue(Config.ConfigCategory ConfigCategory, string ConfigSubCategoryName, string ConfigEntryName, JToken ConfigValue)
        {
            SetConfigValue(ConfigCategory, GetConfigCategory(ConfigCategory, ConfigSubCategoryName), ConfigEntryName, ConfigValue);
        }

        /// <summary>
        /// Sets the value of an entry in a category.
        /// </summary>
        /// <param name="ConfigCategory">Config category</param>
        /// <param name="ConfigCategoryToken">Config category or sub-category token (You can get it from <see cref="GetConfigCategory(Config.ConfigCategory, string)"/></param>
        /// <param name="ConfigEntryName">Config entry name.</param>
        /// <param name="ConfigValue">Config entry value to install</param>
        public static void SetConfigValue(Config.ConfigCategory ConfigCategory, JToken ConfigCategoryToken, string ConfigEntryName, JToken ConfigValue)
        {
            // Try to parse the config category
            DebugWriter.Wdbg(DebugLevel.I, "Parsing config category {0}...", ConfigCategory);
            if (Enum.TryParse(((int)ConfigCategory).ToString(), out ConfigCategory))
            {
                // We have a valid category. Now, find the config entry property in the token
                DebugWriter.Wdbg(DebugLevel.I, "Parsing config entry {0}...", ConfigEntryName);
                _ = Config.ConfigToken[ConfigCategory.ToString()];
                if (ConfigCategoryToken[ConfigEntryName] is not null)
                {
                    // Assign the new value to it and write the changes to the token and the config file. Don't worry, debuggers, when you set the value like below,
                    // it will automatically save the changes to ConfigToken as in three lines above
                    DebugWriter.Wdbg(DebugLevel.E, "Entry {0} found! Setting value...", ConfigEntryName);
                    ConfigCategoryToken[ConfigEntryName] = ConfigValue;

                    // Write the changes to the config file
                    File.WriteAllText(Paths.GetKernelPath(KernelPathType.Configuration), JsonConvert.SerializeObject(Config.ConfigToken, Formatting.Indented));
                }
                else
                {
                    // We didn't get an entry.
                    DebugWriter.Wdbg(DebugLevel.E, "Entry {0} not found!", ConfigEntryName);
                    throw new Kernel.Exceptions.ConfigException(Translate.DoTranslation("Config entry {0} not found."), ConfigEntryName);
                }
            }
            else
            {
                // We didn't get a category.
                DebugWriter.Wdbg(DebugLevel.E, "Category {0} not found!", ConfigCategory);
                throw new Kernel.Exceptions.ConfigException(Translate.DoTranslation("Config category {0} not found."), ConfigCategory);
            }
        }

        /// <summary>
        /// Gets the value of an entry in a category.
        /// </summary>
        /// <param name="ConfigCategory">Config category</param>
        /// <param name="ConfigEntryName">Config entry name.</param>
        public static JToken GetConfigValue(Config.ConfigCategory ConfigCategory, string ConfigEntryName)
        {
            return GetConfigValue(ConfigCategory, GetConfigCategory(ConfigCategory), ConfigEntryName);
        }

        /// <summary>
        /// Gets the value of an entry in a category.
        /// </summary>
        /// <param name="ConfigCategory">Config category</param>
        /// <param name="ConfigEntryName">Config entry name.</param>
        public static JToken GetConfigValue(Config.ConfigCategory ConfigCategory, string ConfigSubCategoryName, string ConfigEntryName)
        {
            return GetConfigValue(ConfigCategory, GetConfigCategory(ConfigCategory, ConfigSubCategoryName), ConfigEntryName);
        }

        /// <summary>
        /// Gets the value of an entry in a category.
        /// </summary>
        /// <param name="ConfigCategory">Config category</param>
        /// <param name="ConfigCategoryToken">Config category or sub-category token (You can get it from <see cref="GetConfigCategory(Config.ConfigCategory, string)"/></param>
        /// <param name="ConfigEntryName">Config entry name.</param>
        public static JToken GetConfigValue(Config.ConfigCategory ConfigCategory, JToken ConfigCategoryToken, string ConfigEntryName)
        {
            // Try to parse the config category
            DebugWriter.Wdbg(DebugLevel.I, "Parsing config category {0}...", ConfigCategory);
            if (Enum.TryParse(((int)ConfigCategory).ToString(), out ConfigCategory))
            {
                // We have a valid category. Now, find the config entry property in the token
                DebugWriter.Wdbg(DebugLevel.I, "Parsing config entry {0}...", ConfigEntryName);
                _ = Config.ConfigToken[ConfigCategory.ToString()];
                if (ConfigCategoryToken[ConfigEntryName] is not null)
                {
                    // We got the appropriate value! Return it.
                    DebugWriter.Wdbg(DebugLevel.E, "Entry {0} found! Getting value...", ConfigEntryName);
                    return ConfigCategoryToken[ConfigEntryName];
                }
                else
                {
                    // We didn't get an entry.
                    DebugWriter.Wdbg(DebugLevel.E, "Entry {0} not found!", ConfigEntryName);
                    throw new Kernel.Exceptions.ConfigException(Translate.DoTranslation("Config entry {0} not found."), ConfigEntryName);
                }
            }
            else
            {
                // We didn't get a category.
                DebugWriter.Wdbg(DebugLevel.E, "Category {0} not found!", ConfigCategory);
                throw new Kernel.Exceptions.ConfigException(Translate.DoTranslation("Config category {0} not found."), ConfigCategory);
            }
        }

    }
}
