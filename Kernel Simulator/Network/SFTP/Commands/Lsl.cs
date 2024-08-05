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
using KS.Files.Folders;
using KS.Kernel;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;

namespace KS.Network.SFTP.Commands
{
    class SFTP_LslCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            bool ShowFileDetails = ListSwitchesOnly.Contains("-showdetails") || Listing.ShowFileDetailsList;
            bool SuppressUnauthorizedMessage = ListSwitchesOnly.Contains("-suppressmessages") || Flags.SuppressUnauthorizedMessages;
            if ((((ListArgsOnly?.Length) is { } arg1 ? arg1 == 0 : (bool?)null) | ListArgsOnly is null) == true)
            {
                Listing.List(SFTPShellCommon.SFTPCurrDirect, ShowFileDetails, SuppressUnauthorizedMessage);
            }
            else
            {
                foreach (string Directory in ListArgsOnly)
                {
                    string direct = Files.Filesystem.NeutralizePath(Directory);
                    Listing.List(direct, ShowFileDetails, SuppressUnauthorizedMessage);
                }
            }
        }

        public override void HelpHelper()
        {
            TextWriters.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, KernelColorTools.ColTypes.Neutral);
            TextWriters.Write("  -showdetails: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Shows the file details in the list"), true, KernelColorTools.ColTypes.ListValue);
            TextWriters.Write("  -suppressmessages: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Suppresses the annoying \"permission denied\" messages"), true, KernelColorTools.ColTypes.ListValue);
        }

    }
}