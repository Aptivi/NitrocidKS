﻿
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.FancyWriters;

namespace KS.Misc.Screensaver
{
    /// <summary>
    /// Base screensaver class
    /// </summary>
    public abstract class BaseScreensaver : IScreensaver
    {

        /// <summary>
        /// Screensaver name
        /// </summary>
        public virtual string ScreensaverName { get; set; } = "BaseScreensaver";

        /// <summary>
        /// Screensaver settings
        /// </summary>
        public virtual Dictionary<string, object> ScreensaverSettings { get; set; }
        /// <summary>
        /// Whether the screensaver contains flashing images
        /// </summary>
        public virtual bool ScreensaverContainsFlashingImages { get; set; } = false;

        /// <summary>
        /// Shows the seizure warning before the preparation
        /// </summary>
        public virtual void ScreensaverSeizureWarning()
        {
            ConsoleBase.ConsoleWrapper.Clear();
            InfoBoxColor.WriteInfoBox(
                Translate.DoTranslation("Photosensitive seizure warning") + CharManager.NewLine + CharManager.NewLine +
                Translate.DoTranslation("This screensaver may contain flashing images and fast-paced animations that may cause seizures for the photosensitive. If you're sure to show this screensaver, press any key."),
                ColorSeq.ConsoleColors.White, ColorSeq.ConsoleColors.Red);
        }

        /// <summary>
        /// Screensaver preparation logic
        /// </summary>
        public virtual void ScreensaverPreparation()
        {
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.ForegroundColor = ConsoleColor.White;
            ConsoleBase.ConsoleWrapper.Clear();
            ConsoleBase.ConsoleWrapper.CursorVisible = false;
        }

        /// <summary>
        /// Screensaver logic
        /// </summary>
        public virtual void ScreensaverLogic() => ThreadManager.SleepNoBlock(10L, ScreensaverDisplayer.ScreensaverDisplayerThread);

        /// <summary>
        /// Screensaver outro
        /// </summary>
        public virtual void ScreensaverOutro() { }
    }
}
