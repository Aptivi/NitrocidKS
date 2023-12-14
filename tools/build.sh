#!/bin/bash

#    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
#
#    This file is part of Kernel Simulator
#
#    Kernel Simulator is free software: you can redistribute it and/or modify
#    it under the terms of the GNU General Public License as published by
#    the Free Software Foundation, either version 3 of the License, or
#    (at your option) any later version.
#
#    Kernel Simulator is distributed in the hope that it will be useful,
#    but WITHOUT ANY WARRANTY; without even the implied warranty of
#    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
#    GNU General Public License for more details.
#
#    You should have received a copy of the GNU General Public License
#    along with this program.  If not, see <https://www.gnu.org/licenses/>.

# This script builds KS. Use when you have dotnet installed.
ksversion=$(cat version)

# Check for dependencies
dotnetpath=`which dotnet`
if [ ! $? == 0 ]; then
	echo dotnet is not found.
	exit 1
fi

# Build KS
echo Building KS...
"$dotnetpath" msbuild "../Kernel Simulator.sln" -t:restore -p:Configuration=Release-dotnet
"$dotnetpath" msbuild "../Kernel Simulator.sln" -p:Configuration=Release-dotnet
"$dotnetpath" msbuild "../Kernel Simulator.sln" -t:restore -p:Configuration=Release
"$dotnetpath" msbuild "../Kernel Simulator.sln" -p:Configuration=Release
if [ ! $? == 0 ]; then
	echo Build failed.
	exit 1
fi

# Inform success
echo Build successful.
exit 0
