﻿//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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

using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.ShellBase.Switches;
using NUnit.Framework;
using Shouldly;

namespace Nitrocid.Tests.Shell.ShellBase.Commands
{

    [TestFixture]
    public class CommandInfoInitializationTests
    {

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.FTPShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.HTTPShell)]
        [TestCase(ShellType.JsonShell)]
        [TestCase(ShellType.MailShell)]
        [TestCase(ShellType.RSSShell)]
        [TestCase(ShellType.SFTPShell)]
        [TestCase(ShellType.SqlShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgSimple(ShellType type)
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", type, "Help page",
                    new[]
                    {
                        new CommandArgumentInfo()
                    }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.FTPShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.HTTPShell)]
        [TestCase(ShellType.JsonShell)]
        [TestCase(ShellType.MailShell)]
        [TestCase(ShellType.RSSShell)]
        [TestCase(ShellType.SFTPShell)]
        [TestCase(ShellType.SqlShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArg(ShellType type)
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", type, "Help page",
                    new[]
                    {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "testarg")
                        })
                    }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.FTPShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.HTTPShell)]
        [TestCase(ShellType.JsonShell)]
        [TestCase(ShellType.MailShell)]
        [TestCase(ShellType.RSSShell)]
        [TestCase(ShellType.SFTPShell)]
        [TestCase(ShellType.SqlShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitch(ShellType type)
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", type, "Help page",
                    new[]
                    {
                        new CommandArgumentInfo(new[]
                        {
                            new SwitchInfo("s", "Simple help")
                        })
                    }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
            
            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.FTPShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.HTTPShell)]
        [TestCase(ShellType.JsonShell)]
        [TestCase(ShellType.MailShell)]
        [TestCase(ShellType.RSSShell)]
        [TestCase(ShellType.SFTPShell)]
        [TestCase(ShellType.SqlShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchAndArg(ShellType type)
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", type, "Help page",
                    new[]
                    {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "testarg")
                        }, new[]
                        {
                            new SwitchInfo("s", "Simple help")
                        })
                    }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.FTPShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.HTTPShell)]
        [TestCase(ShellType.JsonShell)]
        [TestCase(ShellType.MailShell)]
        [TestCase(ShellType.RSSShell)]
        [TestCase(ShellType.SFTPShell)]
        [TestCase(ShellType.SqlShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchWithOptions(ShellType type)
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", type, "Help page",
                    new[]
                    {
                        new CommandArgumentInfo(new[]
                        {
                            new SwitchInfo("s", "Simple help", new SwitchOptions
                            {
                                AcceptsValues = false,
                            })
                        })
                    }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
            
            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.FTPShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.HTTPShell)]
        [TestCase(ShellType.JsonShell)]
        [TestCase(ShellType.MailShell)]
        [TestCase(ShellType.RSSShell)]
        [TestCase(ShellType.SFTPShell)]
        [TestCase(ShellType.SqlShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchWithOptionsAndArg(ShellType type)
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", type, "Help page",
                    new[]
                    {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "testarg")
                        }, new[]
                        {
                            new SwitchInfo("s", "Simple help", new SwitchOptions
                            {
                                AcceptsValues = false,
                            })
                        })
                    }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("FTPShell")]
        [TestCase("HexShell")]
        [TestCase("HTTPShell")]
        [TestCase("JsonShell")]
        [TestCase("MailShell")]
        [TestCase("RSSShell")]
        [TestCase("SFTPShell")]
        [TestCase("SqlShell")]
        [TestCase("TextShell")]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgSimple(string type)
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", type, "Help page",
                    new[]
                    {
                        new CommandArgumentInfo()
                    }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type);
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("FTPShell")]
        [TestCase("HexShell")]
        [TestCase("HTTPShell")]
        [TestCase("JsonShell")]
        [TestCase("MailShell")]
        [TestCase("RSSShell")]
        [TestCase("SFTPShell")]
        [TestCase("SqlShell")]
        [TestCase("TextShell")]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArg(string type)
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", type, "Help page",
                    new[]
                    {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "testarg")
                        })
                    }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type);
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("FTPShell")]
        [TestCase("HexShell")]
        [TestCase("HTTPShell")]
        [TestCase("JsonShell")]
        [TestCase("MailShell")]
        [TestCase("RSSShell")]
        [TestCase("SFTPShell")]
        [TestCase("SqlShell")]
        [TestCase("TextShell")]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitch(string type)
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", type, "Help page",
                    new[]
                    {
                        new CommandArgumentInfo(new[]
                        {
                            new SwitchInfo("s", "Simple help")
                        })
                    }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type);
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
            
            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("FTPShell")]
        [TestCase("HexShell")]
        [TestCase("HTTPShell")]
        [TestCase("JsonShell")]
        [TestCase("MailShell")]
        [TestCase("RSSShell")]
        [TestCase("SFTPShell")]
        [TestCase("SqlShell")]
        [TestCase("TextShell")]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchAndArg(string type)
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", type, "Help page",
                    new[]
                    {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "testarg")
                        }, new[]
                        {
                            new SwitchInfo("s", "Simple help")
                        })
                    }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type);
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("FTPShell")]
        [TestCase("HexShell")]
        [TestCase("HTTPShell")]
        [TestCase("JsonShell")]
        [TestCase("MailShell")]
        [TestCase("RSSShell")]
        [TestCase("SFTPShell")]
        [TestCase("SqlShell")]
        [TestCase("TextShell")]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchWithOptions(string type)
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", type, "Help page",
                    new[]
                    {
                        new CommandArgumentInfo(new[]
                        {
                            new SwitchInfo("s", "Simple help", new SwitchOptions
                            {
                                AcceptsValues = false,
                            })
                        })
                    }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type);
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
            
            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("FTPShell")]
        [TestCase("HexShell")]
        [TestCase("HTTPShell")]
        [TestCase("JsonShell")]
        [TestCase("MailShell")]
        [TestCase("RSSShell")]
        [TestCase("SFTPShell")]
        [TestCase("SqlShell")]
        [TestCase("TextShell")]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchWithOptionsAndArg(string type)
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", type, "Help page",
                    new[]
                    {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "testarg")
                        }, new[]
                        {
                            new SwitchInfo("s", "Simple help", new SwitchOptions
                            {
                                AcceptsValues = false,
                            })
                        })
                    }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type);
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.FTPShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.HTTPShell)]
        [TestCase(ShellType.JsonShell)]
        [TestCase(ShellType.MailShell)]
        [TestCase(ShellType.RSSShell)]
        [TestCase(ShellType.SFTPShell)]
        [TestCase(ShellType.SqlShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgSimpleMultiCommandArgumentInfo(ShellType type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page", 
                new[] {
                    new CommandArgumentInfo(),
                    new CommandArgumentInfo()
                }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.FTPShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.HTTPShell)]
        [TestCase(ShellType.JsonShell)]
        [TestCase(ShellType.MailShell)]
        [TestCase(ShellType.RSSShell)]
        [TestCase(ShellType.SFTPShell)]
        [TestCase(ShellType.SqlShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgMultiCommandArgumentInfo(ShellType type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page",
                new[] {
                    new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "testarg")
                        }),
                    new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "testme"),
                            new CommandArgumentPart(false, "path"),
                        }),
                }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeTrue();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(1);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.FTPShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.HTTPShell)]
        [TestCase(ShellType.JsonShell)]
        [TestCase(ShellType.MailShell)]
        [TestCase(ShellType.RSSShell)]
        [TestCase(ShellType.SFTPShell)]
        [TestCase(ShellType.SqlShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchMultiCommandArgumentInfo(ShellType type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page",
                new[] {
                    new CommandArgumentInfo(new[]
                    {
                        new SwitchInfo("s", "Simple help")
                    }),
                    new CommandArgumentInfo(new[]
                    {
                        new SwitchInfo("c", "Complicated help")
                    }),
                }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
            
            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            var @switch2 = CommandInstance.CommandArgumentInfo[1].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeTrue();
            @switch2.SwitchName.ShouldBe("c");
            @switch2.HelpDefinition.ShouldBe("Complicated help");
            @switch2.IsRequired.ShouldBeFalse();
            @switch2.ArgumentsRequired.ShouldBeFalse();
            @switch2.ConflictsWith.ShouldBeEmpty();
            @switch2.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch2.AcceptsValues.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.FTPShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.HTTPShell)]
        [TestCase(ShellType.JsonShell)]
        [TestCase(ShellType.MailShell)]
        [TestCase(ShellType.RSSShell)]
        [TestCase(ShellType.SFTPShell)]
        [TestCase(ShellType.SqlShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchAndArgMultiCommandArgumentInfo(ShellType type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page",
                new[] {
                    new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "testarg")
                        }, new[]
                        {
                            new SwitchInfo("s", "Simple help")
                        }),
                    new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "testme"),
                            new CommandArgumentPart(false, "path"),
                        }, new[]
                        {
                            new SwitchInfo("c", "Complicated help")
                        }),
                }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeTrue();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(1);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            var @switch2 = CommandInstance.CommandArgumentInfo[1].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeTrue();
            @switch2.SwitchName.ShouldBe("c");
            @switch2.HelpDefinition.ShouldBe("Complicated help");
            @switch2.IsRequired.ShouldBeFalse();
            @switch2.ArgumentsRequired.ShouldBeFalse();
            @switch2.ConflictsWith.ShouldBeEmpty();
            @switch2.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch2.AcceptsValues.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.FTPShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.HTTPShell)]
        [TestCase(ShellType.JsonShell)]
        [TestCase(ShellType.MailShell)]
        [TestCase(ShellType.RSSShell)]
        [TestCase(ShellType.SFTPShell)]
        [TestCase(ShellType.SqlShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchWithOptionsMultiCommandArgumentInfo(ShellType type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page",
                new[] {
                    new CommandArgumentInfo(new[]
                    {
                        new SwitchInfo("s", "Simple help", new SwitchOptions
                        {
                            AcceptsValues = false,
                        })
                    }),
                    new CommandArgumentInfo(new[]
                    {
                        new SwitchInfo("c", "Complicated help", new SwitchOptions
                        {
                            AcceptsValues = false,
                            IsRequired = true,
                        })
                    }),
                }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
            
            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            var @switch2 = CommandInstance.CommandArgumentInfo[1].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeFalse();
            @switch2.SwitchName.ShouldBe("c");
            @switch2.HelpDefinition.ShouldBe("Complicated help");
            @switch2.IsRequired.ShouldBeTrue();
            @switch2.ArgumentsRequired.ShouldBeFalse();
            @switch2.ConflictsWith.ShouldBeEmpty();
            @switch2.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch2.AcceptsValues.ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.FTPShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.HTTPShell)]
        [TestCase(ShellType.JsonShell)]
        [TestCase(ShellType.MailShell)]
        [TestCase(ShellType.RSSShell)]
        [TestCase(ShellType.SFTPShell)]
        [TestCase(ShellType.SqlShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchWithOptionsAndArgMultiCommandArgumentInfo(ShellType type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page",
                new[] {
                    new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "testarg")
                        }, new[]
                        {
                            new SwitchInfo("s", "Simple help", new SwitchOptions
                            {
                                AcceptsValues = false,
                            })
                        }),
                    new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "testme"),
                            new CommandArgumentPart(false, "path"),
                        }, new[]
                        {
                            new SwitchInfo("c", "Complicated help", new SwitchOptions
                            {
                                AcceptsValues = false,
                                IsRequired = true,
                            })
                        }),
                }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeTrue();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(1);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            var @switch2 = CommandInstance.CommandArgumentInfo[1].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeFalse();
            @switch2.SwitchName.ShouldBe("c");
            @switch2.HelpDefinition.ShouldBe("Complicated help");
            @switch2.IsRequired.ShouldBeTrue();
            @switch2.ArgumentsRequired.ShouldBeFalse();
            @switch2.ConflictsWith.ShouldBeEmpty();
            @switch2.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch2.AcceptsValues.ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("FTPShell")]
        [TestCase("HexShell")]
        [TestCase("HTTPShell")]
        [TestCase("JsonShell")]
        [TestCase("MailShell")]
        [TestCase("RSSShell")]
        [TestCase("SFTPShell")]
        [TestCase("SqlShell")]
        [TestCase("TextShell")]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgSimpleMultiCommandArgumentInfo(string type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page",
                new[] {
                    new CommandArgumentInfo(),
                    new CommandArgumentInfo()
                }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("FTPShell")]
        [TestCase("HexShell")]
        [TestCase("HTTPShell")]
        [TestCase("JsonShell")]
        [TestCase("MailShell")]
        [TestCase("RSSShell")]
        [TestCase("SFTPShell")]
        [TestCase("SqlShell")]
        [TestCase("TextShell")]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgMultiCommandArgumentInfo(string type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page",
                new[] {
                    new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "testarg")
                        }),
                    new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "testme"),
                            new CommandArgumentPart(false, "path"),
                        }),
                }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeTrue();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(1);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("FTPShell")]
        [TestCase("HexShell")]
        [TestCase("HTTPShell")]
        [TestCase("JsonShell")]
        [TestCase("MailShell")]
        [TestCase("RSSShell")]
        [TestCase("SFTPShell")]
        [TestCase("SqlShell")]
        [TestCase("TextShell")]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchMultiCommandArgumentInfo(string type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page",
                new[] {
                    new CommandArgumentInfo(new[]
                    {
                        new SwitchInfo("s", "Simple help")
                    }),
                    new CommandArgumentInfo(new[]
                    {
                        new SwitchInfo("c", "Complicated help")
                    }),
                }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            var @switch2 = CommandInstance.CommandArgumentInfo[1].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeTrue();
            @switch2.SwitchName.ShouldBe("c");
            @switch2.HelpDefinition.ShouldBe("Complicated help");
            @switch2.IsRequired.ShouldBeFalse();
            @switch2.ArgumentsRequired.ShouldBeFalse();
            @switch2.ConflictsWith.ShouldBeEmpty();
            @switch2.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch2.AcceptsValues.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("FTPShell")]
        [TestCase("HexShell")]
        [TestCase("HTTPShell")]
        [TestCase("JsonShell")]
        [TestCase("MailShell")]
        [TestCase("RSSShell")]
        [TestCase("SFTPShell")]
        [TestCase("SqlShell")]
        [TestCase("TextShell")]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchAndArgMultiCommandArgumentInfo(string type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page",
                new[] {
                    new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "testarg")
                        }, new[]
                        {
                            new SwitchInfo("s", "Simple help")
                        }),
                    new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "testme"),
                            new CommandArgumentPart(false, "path"),
                        }, new[]
                        {
                            new SwitchInfo("c", "Complicated help")
                        }),
                }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeTrue();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(1);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            var @switch2 = CommandInstance.CommandArgumentInfo[1].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeTrue();
            @switch2.SwitchName.ShouldBe("c");
            @switch2.HelpDefinition.ShouldBe("Complicated help");
            @switch2.IsRequired.ShouldBeFalse();
            @switch2.ArgumentsRequired.ShouldBeFalse();
            @switch2.ConflictsWith.ShouldBeEmpty();
            @switch2.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch2.AcceptsValues.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("FTPShell")]
        [TestCase("HexShell")]
        [TestCase("HTTPShell")]
        [TestCase("JsonShell")]
        [TestCase("MailShell")]
        [TestCase("RSSShell")]
        [TestCase("SFTPShell")]
        [TestCase("SqlShell")]
        [TestCase("TextShell")]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchWithOptionsMultiCommandArgumentInfo(string type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page",
                new[] {
                    new CommandArgumentInfo(new[]
                    {
                        new SwitchInfo("s", "Simple help", new SwitchOptions
                        {
                            AcceptsValues = false,
                        })
                    }),
                    new CommandArgumentInfo(new[]
                    {
                        new SwitchInfo("c", "Complicated help", new SwitchOptions
                        {
                            AcceptsValues = false,
                            IsRequired = true,
                        })
                    }),
                }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(0);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            var @switch2 = CommandInstance.CommandArgumentInfo[1].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeFalse();
            @switch2.SwitchName.ShouldBe("c");
            @switch2.HelpDefinition.ShouldBe("Complicated help");
            @switch2.IsRequired.ShouldBeTrue();
            @switch2.ArgumentsRequired.ShouldBeFalse();
            @switch2.ConflictsWith.ShouldBeEmpty();
            @switch2.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch2.AcceptsValues.ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("FTPShell")]
        [TestCase("HexShell")]
        [TestCase("HTTPShell")]
        [TestCase("JsonShell")]
        [TestCase("MailShell")]
        [TestCase("RSSShell")]
        [TestCase("SFTPShell")]
        [TestCase("SqlShell")]
        [TestCase("TextShell")]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchWithOptionsAndArgMultiCommandArgumentInfo(string type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page",
                new[] {
                    new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "testarg")
                        }, new[]
                        {
                            new SwitchInfo("s", "Simple help", new SwitchOptions
                            {
                                AcceptsValues = false,
                            })
                        }),
                    new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "testme"),
                            new CommandArgumentPart(false, "path"),
                        }, new[]
                        {
                            new SwitchInfo("c", "Complicated help", new SwitchOptions
                            {
                                AcceptsValues = false,
                                IsRequired = true,
                            })
                        }),
                }, null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeTrue();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(1);
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            var @switch2 = CommandInstance.CommandArgumentInfo[1].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeFalse();
            @switch2.SwitchName.ShouldBe("c");
            @switch2.HelpDefinition.ShouldBe("Complicated help");
            @switch2.IsRequired.ShouldBeTrue();
            @switch2.ArgumentsRequired.ShouldBeFalse();
            @switch2.ConflictsWith.ShouldBeEmpty();
            @switch2.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch2.AcceptsValues.ShouldBeFalse();
        }

    }
}
