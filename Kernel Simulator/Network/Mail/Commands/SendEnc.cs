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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Files.Querying;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using KS.Network.Mail.Transfer;
using KS.Shell.ShellBase.Commands;
using MimeKit;

namespace KS.Network.Mail.Commands
{
    class Mail_SendEncCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string Receiver, Subject;
            var Body = new BodyBuilder();

            // Prompt for receiver e-mail address
            TextWriters.Write(Translate.DoTranslation("Enter recipient mail address:") + " ", false, KernelColorTools.ColTypes.Input);
            Receiver = Input.ReadLine();
            DebugWriter.Wdbg(DebugLevel.I, "Recipient: {0}", Receiver);

            // Check for mail format
            if (Receiver.Contains("@") & Receiver.Substring(Receiver.IndexOf("@")).Contains("."))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Mail format satisfied. Contains \"@\" and contains \".\" in the second part after the \"@\" symbol.");

                // Prompt for subject
                TextWriters.Write(Translate.DoTranslation("Enter the subject:") + " ", false, KernelColorTools.ColTypes.Input);
                Subject = Input.ReadLine(false);
                DebugWriter.Wdbg(DebugLevel.I, "Subject: {0} ({1} chars)", Subject, Subject.Length);

                // Prompt for body
                TextWriters.Write(Translate.DoTranslation("Enter your message below. Write \"EOF\" to confirm."), true, KernelColorTools.ColTypes.Input);
                string BodyLine = "";
                while (!(BodyLine.ToUpper() == "EOF"))
                {
                    BodyLine = Input.ReadLine();
                    if (!(BodyLine.ToUpper() == "EOF"))
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Body line: {0} ({1} chars)", BodyLine, BodyLine.Length);
                        Body.TextBody += BodyLine + Kernel.Kernel.NewLine;
                        DebugWriter.Wdbg(DebugLevel.I, "Body length: {0} chars", Body.TextBody.Length);
                    }
                }

                TextWriters.Write(Translate.DoTranslation("Enter file paths to attachments. Press ENTER on a blank path to confirm."), true, KernelColorTools.ColTypes.Neutral);
                string PathLine = " ";
                while (!string.IsNullOrEmpty(PathLine))
                {
                    TextWriters.Write("> ", false, KernelColorTools.ColTypes.Input);
                    PathLine = Input.ReadLine(false);
                    if (!string.IsNullOrEmpty(PathLine))
                    {
                        PathLine = Filesystem.NeutralizePath(PathLine);
                        DebugWriter.Wdbg(DebugLevel.I, "Path line: {0} ({1} chars)", PathLine, PathLine.Length);
                        if (Checking.FileExists(PathLine))
                        {
                            Body.Attachments.Add(PathLine);
                        }
                    }
                }

                // Send the message
                TextWriters.Write(Translate.DoTranslation("Sending message..."), true, KernelColorTools.ColTypes.Progress);
                if (MailTransfer.MailSendEncryptedMessage(Receiver, Subject, Body.ToMessageBody()))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Message sent.");
                    TextWriters.Write(Translate.DoTranslation("Message sent."), true, KernelColorTools.ColTypes.Success);
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.E, "See debug output to find what's wrong.");
                    TextWriters.Write(Translate.DoTranslation("Error sending message."), true, KernelColorTools.ColTypes.Error);
                }
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.E, "Mail format unsatisfied." + Receiver);
                TextWriters.Write(Translate.DoTranslation("Invalid e-mail address. Make sure you've written the address correctly and that it matches the format of the example shown:") + " john.s@example.com", true, KernelColorTools.ColTypes.Error);
            }
        }

    }
}