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

namespace KS.Scripting.Conditions
{
    public interface ICondition
    {

        /// <summary>
        /// Specifies the condition name
        /// </summary>
        string ConditionName { get; }
        /// <summary>
        /// Specifies where the condition should be located. Beware that it starts from 1.
        /// </summary>
        int ConditionPosition { get; }
        /// <summary>
        /// How many arguments are required (counting the condition itself)? Beware that it starts from 1.
        /// </summary>
        int ConditionRequiredArguments { get; }
        /// <summary>
        /// Checks whether the condition is satisfied
        /// </summary>
        bool IsConditionSatisfied(string FirstVariable, string SecondVariable);
        /// <summary>
        /// Checks whether the condition is satisfiedfor more than two variables
        /// </summary>
        bool IsConditionSatisfied(string[] Variables);

    }
}