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

using System.Collections.Generic;
using System.Data;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Encryption;
using KS.Misc.Probers;
using KS.Misc.Screensaver;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using KS.Network.RSS;
using KS.Shell.ShellBase.Shells;
using Terminaux.Base;
using Terminaux.Writer.ConsoleWriters;

namespace KS.Login
{
    public static class Login
    {

        // Variables
        /// <summary>
        /// Username prompt
        /// </summary>
        public static string UsernamePrompt;
        /// <summary>
        /// Password prompt
        /// </summary>
        public static string PasswordPrompt;
        /// <summary>
        /// List of usernames and passwords
        /// </summary>
        internal static Dictionary<string, string> Users = new()
        {
            { "root", Encryption.GetEmptyHash(Encryption.Algorithms.SHA256) }
        };
        /// <summary>
        /// Current username
        /// </summary>
        private static UserInfo CurrentUserInfo;

        /// <summary>
        /// Current username
        /// </summary>
        public static UserInfo CurrentUser => CurrentUserInfo;

        /// <summary>
        /// Prompts user for login information
        /// </summary>
        public static void LoginPrompt()
        {
            while (true)
            {
                // Check to see if the reboot is requested
                if (Flags.RebootRequested | Flags.KernelShutdown)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Reboot has been requested. Exiting...");
                    Flags.RebootRequested = false;
                    return;
                }

                // Fire event PreLogin
                Kernel.Kernel.KernelEventManager.RaisePreLogin();

                // Check to see if there are any users
                if (Users.Count == 0)
                {
                    // Extremely rare state reached
                    DebugWriter.Wdbg(DebugLevel.F, "Shell reached rare state, because userword count is 0.");
                    throw new Kernel.Exceptions.NullUsersException(Translate.DoTranslation("There are no more users remaining in the list."));
                }
                else if (Users.Count == 1 & Users.Keys.ElementAtOrDefault(0) == "root")
                {
                    // Run a first user trigger
                    DebugWriter.Wdbg(DebugLevel.W, "Only root is found. Triggering first user setup...");
                    UserManagement.FirstUserTrigger();
                }

                // Clear console if ClearOnLogin is set to True (If a user has enabled Clear Screen on Login)
                if (Flags.ClearOnLogin == true)
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Clearing screen...");
                    ConsoleWrapper.Clear();
                }

                // Read MOTD and MAL
                MOTDParse.ReadMOTD(MOTDParse.MessageType.MOTD);
                MOTDParse.ReadMOTD(MOTDParse.MessageType.MAL);

                // Show MOTD once
                DebugWriter.Wdbg(DebugLevel.I, "showMOTDOnceFlag = {0}, showMOTD = {1}", Flags.ShowMOTDOnceFlag, Flags.ShowMOTD);
                if (Flags.ShowMOTDOnceFlag == true & Flags.ShowMOTD == true)
                {
                    TextWriters.Write(Kernel.Kernel.NewLine + PlaceParse.ProbePlaces(Kernel.Kernel.MOTDMessage), true, KernelColorTools.ColTypes.Banner);
                }
                Flags.ShowMOTDOnceFlag = false;

                // How do we prompt user to login?
                var UsersList = UserManagement.ListAllUsers();
                if (Flags.ChooseUser)
                {
                    // Generate user list
                    ListWriterColor.WriteList(UsersList);
                    int AnswerUserInt = 0;

                    // Prompt user to choose a user
                    while (AnswerUserInt == 0)
                    {
                        TextWriters.Write(">> ", false, KernelColorTools.ColTypes.Input);
                        string AnswerUserString = Input.ReadLine();

                        // Parse input
                        if (!string.IsNullOrWhiteSpace(AnswerUserString))
                        {
                            if (int.TryParse(AnswerUserString, out AnswerUserInt))
                            {
                                string SelectedUser = UserManagement.SelectUser(AnswerUserInt);
                                DebugWriter.Wdbg(DebugLevel.I, "Username correct. Finding if the user is disabled...");
                                if (!PermissionManagement.HasPermission(SelectedUser, PermissionManagement.PermissionType.Disabled))
                                {
                                    DebugWriter.Wdbg(DebugLevel.I, "User can log in. (User is not in disabled list)");
                                    ShowPasswordPrompt(SelectedUser);
                                }
                                else
                                {
                                    DebugWriter.Wdbg(DebugLevel.W, "User can't log in. (User is in disabled list)");
                                    TextWriters.Write(Translate.DoTranslation("User is disabled."), true, KernelColorTools.ColTypes.Error);
                                    Kernel.Kernel.KernelEventManager.RaiseLoginError(SelectedUser, LoginErrorReasons.Disabled);
                                }
                            }
                            else
                            {
                                TextWriters.Write(Translate.DoTranslation("The answer must be numeric."), true, KernelColorTools.ColTypes.Error);
                            }
                        }
                        else
                        {
                            TextWriters.Write(Translate.DoTranslation("Please enter a user number."), true, KernelColorTools.ColTypes.Error);
                        }
                    }
                }
                else
                {
                    // Generate user list
                    if (Flags.ShowAvailableUsers)
                        TextWriters.Write(Translate.DoTranslation("Available usernames: {0}"), true, KernelColorTools.ColTypes.Neutral, string.Join(", ", UsersList));

                    // Prompt user to login
                    if (!string.IsNullOrWhiteSpace(UsernamePrompt))
                    {
                        TextWriters.Write(PlaceParse.ProbePlaces(UsernamePrompt), false, KernelColorTools.ColTypes.Input);
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("Username: "), false, KernelColorTools.ColTypes.Input);
                    }
                    string answeruser = Input.ReadLine();

                    // Parse input
                    if (answeruser.Contains(" "))
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Spaces found in username.");
                        TextWriters.Write(Translate.DoTranslation("Spaces are not allowed."), true, KernelColorTools.ColTypes.Error);
                        Kernel.Kernel.KernelEventManager.RaiseLoginError(answeruser, LoginErrorReasons.Spaces);
                    }
                    else if (answeruser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray()) != -1)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Unknown characters found in username.");
                        TextWriters.Write(Translate.DoTranslation("Special characters are not allowed."), true, KernelColorTools.ColTypes.Error);
                        Kernel.Kernel.KernelEventManager.RaiseLoginError(answeruser, LoginErrorReasons.SpecialCharacters);
                    }
                    else if (Users.ContainsKey(answeruser))
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Username correct. Finding if the user is disabled...");
                        if (!PermissionManagement.HasPermission(answeruser, PermissionManagement.PermissionType.Disabled))
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "User can log in. (User is not in disabled list)");
                            ShowPasswordPrompt(answeruser);
                        }
                        else
                        {
                            DebugWriter.Wdbg(DebugLevel.W, "User can't log in. (User is in disabled list)");
                            TextWriters.Write(Translate.DoTranslation("User is disabled."), true, KernelColorTools.ColTypes.Error);
                            Kernel.Kernel.KernelEventManager.RaiseLoginError(answeruser, LoginErrorReasons.Disabled);
                        }
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.E, "Username not found.");
                        TextWriters.Write(Translate.DoTranslation("Wrong username."), true, KernelColorTools.ColTypes.Error);
                        Kernel.Kernel.KernelEventManager.RaiseLoginError(answeruser, LoginErrorReasons.NotFound);
                    }
                }
            }
        }

        /// <summary>
        /// Prompts user for password
        /// </summary>
        /// <param name="usernamerequested">A username that is about to be logged in</param>
        public static void ShowPasswordPrompt(string usernamerequested)
        {
            // Prompts user to enter a user's password
            while (true)
            {
                // Check to see if reboot is requested
                if (Flags.RebootRequested | Flags.KernelShutdown)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Reboot has been requested. Exiting...");
                    Flags.RebootRequested = false;
                    return;
                }

                // Get the password from dictionary
                string UserPassword = Users[usernamerequested];

                // Check if there's a password
                if (!((UserPassword ?? "") == (Encryption.GetEmptyHash(Encryption.Algorithms.SHA256) ?? ""))) // No password
                {
                    // Wait for input
                    DebugWriter.Wdbg(DebugLevel.I, "Password not empty");
                    if (!string.IsNullOrWhiteSpace(PasswordPrompt))
                    {
                        TextWriters.Write(PlaceParse.ProbePlaces(PasswordPrompt), false, KernelColorTools.ColTypes.Input);
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("{0}'s password: "), false, KernelColorTools.ColTypes.Input, usernamerequested);
                    }

                    // Get input
                    string answerpass = Input.ReadLineNoInput();

                    // Compute password hash
                    DebugWriter.Wdbg(DebugLevel.I, "Computing written password hash...");
                    answerpass = Encryption.GetEncryptedString(answerpass, Encryption.Algorithms.SHA256);

                    // Parse password input
                    if (Users.TryGetValue(usernamerequested, out UserPassword) && (UserPassword ?? "") == (answerpass ?? ""))
                    {
                        // Log-in instantly
                        DebugWriter.Wdbg(DebugLevel.I, "Password written correctly. Entering shell...");
                        SignIn(usernamerequested);
                        return;
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Passowrd written wrong...");
                        TextWriters.Write(Translate.DoTranslation("Wrong password."), true, KernelColorTools.ColTypes.Error);
                        Kernel.Kernel.KernelEventManager.RaiseLoginError(usernamerequested, LoginErrorReasons.WrongPassword);
                        if (!Flags.Maintenance)
                        {
                            if (!Screensaver.LockMode)
                            {
                                return;
                            }
                        }
                    }
                }
                else
                {
                    // Log-in instantly
                    DebugWriter.Wdbg(DebugLevel.I, "Password is empty");
                    SignIn(usernamerequested);
                    return;
                }
            }

        }

        /// <summary>
        /// Signs in to the username
        /// </summary>
        /// <param name="signedInUser">A specified username</param>
        internal static void SignIn(string signedInUser)
        {
            // Release lock
            if (Screensaver.LockMode)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Releasing lock and getting back to shell...");
                Screensaver.LockMode = false;
                Kernel.Kernel.KernelEventManager.RaisePostUnlock(Screensaver.DefSaverName);
                return;
            }

            // Notifies the kernel that the user has signed in
            Flags.LoggedIn = true;

            // Sign in to user.
            CurrentUserInfo = new UserInfo(signedInUser);
            if (Screensaver.LockMode == true)
                Screensaver.LockMode = false;
            DebugWriter.Wdbg(DebugLevel.I, "Lock released.");
            Flags.ShowMOTDOnceFlag = true;
            if (Flags.ShowMAL)
                TextWriters.Write(PlaceParse.ProbePlaces(Kernel.Kernel.MAL), true, KernelColorTools.ColTypes.Banner);
            RSSTools.ShowHeadlineLogin();

            // Fire event PostLogin
            Kernel.Kernel.KernelEventManager.RaisePostLogin(CurrentUser.Username);

            // Initialize shell
            DebugWriter.Wdbg(DebugLevel.I, "Shell is being initialized...");
            ShellStart.StartShellForced(ShellType.Shell);
            ShellStart.PurgeShells();
        }

    }
}