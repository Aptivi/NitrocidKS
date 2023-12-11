﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows the link to Nitrocid KS's wiki and its API doc for mods.
    /// </summary>
    class UserManualCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write(Translate.DoTranslation("To access the user manual for general uses of Nitrocid, please visit the below link:"));
            TextWriters.Write(
                KernelPlatform.IsOnUnix() ?
                $"    {CharManager.GetEsc()}]8;;https://aptivi.gitbook.io/kernel-simulator-manual/{CharManager.GetEsc()}\\Manual page{CharManager.GetEsc()}]8;;{CharManager.GetEsc()}\\" :
                "    https://aptivi.gitbook.io/kernel-simulator-manual/",
                true, KernelColorType.Tip
            );
            TextWriterColor.Write(Translate.DoTranslation("To access the developer's manual for updated ways of making kernel modifications and screensavers, please visit the below link:"));
            TextWriters.Write(
                KernelPlatform.IsOnUnix() ?
                $"    {CharManager.GetEsc()}]8;;https://aptivi.github.io/NitrocidKS{CharManager.GetEsc()}\\API documentation{CharManager.GetEsc()}]8;;{CharManager.GetEsc()}\\" :
                "    https://aptivi.github.io/NitrocidKS",
                true, KernelColorType.Tip);
            return 0;
        }

        public override int ExecuteDumb(CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write(Translate.DoTranslation("To access the user manual for general uses of Nitrocid, please visit the below link:"));
            TextWriters.Write("    https://aptivi.gitbook.io/kernel-simulator-manual/", true, KernelColorType.Tip);
            TextWriterColor.Write(Translate.DoTranslation("To access the developer's manual for updated ways of making kernel modifications and screensavers, please visit the below link:"));
            TextWriters.Write("    https://aptivi.github.io/NitrocidKS", true, KernelColorType.Tip);
            return 0;
        }

    }
}
