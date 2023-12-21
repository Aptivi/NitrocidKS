

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

using KS.ConsoleBase.Colors;
using NUnit.Framework;
using Shouldly;
using Terminaux.Colors;

namespace KSTests
{

	[TestFixture]
	public class ColorSettingTests
	{

		/// <summary>
    /// Tests setting colors
    /// </summary>
		[Test]
		[Description("Setting")]
		public void TestSetColors()
		{
			KernelColorTools.SetColors(((int)ConsoleColors.White).ToString(), ((int)ConsoleColors.White).ToString(), ((int)ConsoleColors.Yellow).ToString(), ((int)ConsoleColors.Red).ToString(), ((int)ConsoleColors.DarkGreen).ToString(), ((int)ConsoleColors.Green).ToString(), ((int)ConsoleColors.Black).ToString(), ((int)ConsoleColors.Gray).ToString(), ((int)ConsoleColors.DarkYellow).ToString(), ((int)ConsoleColors.DarkGray).ToString(), ((int)ConsoleColors.Green).ToString(), ((int)ConsoleColors.Red).ToString(), ((int)ConsoleColors.Yellow).ToString(), ((int)ConsoleColors.DarkYellow).ToString(), ((int)ConsoleColors.Green).ToString(), ((int)ConsoleColors.White).ToString(), ((int)ConsoleColors.Gray).ToString(), ((int)ConsoleColors.DarkYellow).ToString(), ((int)ConsoleColors.Red).ToString(), ((int)ConsoleColors.Yellow).ToString(), ((int)ConsoleColors.Green).ToString(), ((int)ConsoleColors.Gray).ToString(), ((int)ConsoleColors.Gray).ToString(), ((int)ConsoleColors.White).ToString(), ((int)ConsoleColors.Gray).ToString(), ((int)ConsoleColors.White).ToString(), ((int)ConsoleColors.Yellow).ToString(), ((int)ConsoleColors.Gray).ToString(), ((int)ConsoleColors.DarkYellow).ToString(), ((int)ConsoleColors.DarkRed).ToString(), ((int)ConsoleColors.White).ToString(), ((int)ConsoleColors.Yellow).ToString(), ((int)ConsoleColors.Red).ToString(), ((int)ConsoleColors.DarkGray).ToString(), ((int)ConsoleColors.White).ToString(), ((int)ConsoleColors.Gray).ToString(), ((int)ConsoleColors.Yellow).ToString(), ((int)ConsoleColors.DarkGreen).ToString());       // Input Color
																																																																																																																																																																																																																																																																																																																																																																																																				   // License Color
																																																																																																																																																																																																																																																																																																																																																																																																				   // Continuable Kernel Error Color
																																																																																																																																																																																																																																																																																																																																																																																																				   // Uncontinuable Kernel Error Color
																																																																																																																																																																																																																																																																																																																																																																																																				   // Host name color
																																																																																																																																																																																																																																																																																																																																																																																																				   // User name color
																																																																																																																																																																																																																																																																																																																																																																																																				   // Background color
																																																																																																																																																																																																																																																																																																																																																																																																				   // Neutral text
																																																																																																																																																																																																																																																																																																																																																																																																				   // List entry text
																																																																																																																																																																																																																																																																																																																																																																																																				   // List value text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Stage text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Error text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Warning text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Option text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Banner text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Notification title text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Notification description text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Notification progress text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Notification failure text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Question text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Success text
																																																																																																																																																																																																																																																																																																																																																																																																				   // User dollar sign on shell text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Tip text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Separator text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Separator color
																																																																																																																																																																																																																																																																																																																																																																																																				   // List title text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Development warning text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Stage time text
																																																																																																																																																																																																																																																																																																																																																																																																				   // General progress text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Back option text
																																																																																																																																																																																																																																																																																																																																																																																																				   // Low priority notification border color
																																																																																																																																																																																																																																																																																																																																																																																																				   // Medium priority notification border color
																																																																																																																																																																																																																																																																																																																																																																																																				   // High priority notification border color
																																																																																																																																																																																																																																																																																																																																																																																																				   // Table separator
																																																																																																																																																																																																																																																																																																																																																																																																				   // Table header
																																																																																																																																																																																																																																																																																																																																																																																																				   // Table value
																																																																																																																																																																																																																																																																																																																																																																																																				   // Selected option
																																																																																																																																																																																																																																																																																																																																																																																																				   // Alternative option
			KernelColorTools.InputColor.ShouldBeEquivalentTo(new Color(ConsoleColors.White));
			KernelColorTools.LicenseColor.ShouldBeEquivalentTo(new Color(ConsoleColors.White));
			KernelColorTools.ContKernelErrorColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Yellow));
			KernelColorTools.UncontKernelErrorColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Red));
			KernelColorTools.HostNameShellColor.ShouldBeEquivalentTo(new Color(ConsoleColors.DarkGreen));
			KernelColorTools.UserNameShellColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Green));
			KernelColorTools.BackgroundColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Black));
			KernelColorTools.NeutralTextColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Gray));
			KernelColorTools.ListEntryColor.ShouldBeEquivalentTo(new Color(ConsoleColors.DarkYellow));
			KernelColorTools.ListValueColor.ShouldBeEquivalentTo(new Color(ConsoleColors.DarkGray));
			KernelColorTools.StageColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Green));
			KernelColorTools.ErrorColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Red));
			KernelColorTools.WarningColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Yellow));
			KernelColorTools.OptionColor.ShouldBeEquivalentTo(new Color(ConsoleColors.DarkYellow));
			KernelColorTools.BannerColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Green));
			KernelColorTools.NotificationTitleColor.ShouldBeEquivalentTo(new Color(ConsoleColors.White));
			KernelColorTools.NotificationDescriptionColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Gray));
			KernelColorTools.NotificationProgressColor.ShouldBeEquivalentTo(new Color(ConsoleColors.DarkYellow));
			KernelColorTools.NotificationFailureColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Red));
			KernelColorTools.QuestionColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Yellow));
			KernelColorTools.SuccessColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Green));
			KernelColorTools.UserDollarColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Gray));
			KernelColorTools.TipColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Gray));
			KernelColorTools.SeparatorTextColor.ShouldBeEquivalentTo(new Color(ConsoleColors.White));
			KernelColorTools.SeparatorColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Gray));
			KernelColorTools.ListTitleColor.ShouldBeEquivalentTo(new Color(ConsoleColors.White));
			KernelColorTools.DevelopmentWarningColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Yellow));
			KernelColorTools.StageTimeColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Gray));
			KernelColorTools.ProgressColor.ShouldBeEquivalentTo(new Color(ConsoleColors.DarkYellow));
			KernelColorTools.LowPriorityBorderColor.ShouldBeEquivalentTo(new Color(ConsoleColors.White));
			KernelColorTools.MediumPriorityBorderColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Yellow));
			KernelColorTools.HighPriorityBorderColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Red));
			KernelColorTools.TableSeparatorColor.ShouldBeEquivalentTo(new Color(ConsoleColors.DarkGray));
			KernelColorTools.TableHeaderColor.ShouldBeEquivalentTo(new Color(ConsoleColors.White));
			KernelColorTools.TableValueColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Gray));
			KernelColorTools.SelectedOptionColor.ShouldBeEquivalentTo(new Color(ConsoleColors.Yellow));
			KernelColorTools.AlternativeOptionColor.ShouldBeEquivalentTo(new Color(ConsoleColors.DarkGreen));
		}

	}
}