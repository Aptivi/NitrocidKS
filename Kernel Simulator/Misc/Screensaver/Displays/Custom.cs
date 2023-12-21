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

using System.Collections.Generic;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;
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

namespace KS.Misc.Screensaver.Displays
{
    public class CustomDisplay : BaseScreensaver, IScreensaver
    {

        private BaseScreensaver CustomSaver { get; set; }

        // WARNING: Please refrain from using ICustomSaver; use IScreensaver instead, which is more dynamic.
        // This implementation doesn't call PostDisplay().
        public override string ScreensaverName { get; set; } = "Custom";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleWrapper.CursorVisible = false;
            DebugWriter.Wdbg(DebugLevel.I, "Entered CustomSaver.ScreensaverPreparation().");
            CustomSaver.ScreensaverPreparation();
            DebugWriter.Wdbg(DebugLevel.I, "Exited CustomSaver.ScreensaverPreparation().");
        }

        public override void ScreensaverLogic()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Entered CustomSaver.ScreensaverLogic().");
            CustomSaver.ScreensaverLogic();
            DebugWriter.Wdbg(DebugLevel.I, "Exited CustomSaver.ScreensaverLogic().");
        }

        public CustomDisplay(BaseScreensaver CustomSaver)
        {
            this.CustomSaver = CustomSaver;
        }

    }
}