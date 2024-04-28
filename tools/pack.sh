#!/bin/bash

#    Kernel Simulator  Copyright (C) 2018-2021  Aptivi
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

# This script builds KS and packs the artifacts. Use when you have MSBuild installed.
ksversion=$(grep "<Version>" ../Directory.Build.props | cut -d "<" -f 2 | cut -d ">" -f 2)

# Check for dependencies
zippath=`which zip`
if [ ! $? == 0 ]; then
	echo zip is not found.
	exit 1
fi
shapath=`which sha256sum`
if [ ! $? == 0 ]; then
	echo sha256sum is not found.
	exit 1
fi

# Pack binary
echo Packing binary...
(cd "../Kernel Simulator/KSBuild/net48/" && "$zippath" -r /tmp/$ksversion-bin.zip . && cd -) >> ~/tmp/buildandpack.log
(cd "../Kernel Simulator/KSBuild/net8.0/" && "$zippath" -r /tmp/$ksversion-bin-dotnet.zip . && cd -) >> ~/tmp/buildandpack.log
if [ ! $? == 0 ]; then
	echo Packing failed.
	exit 1
fi

# Inform success
"$shapath" /tmp/$ksversion-bin.zip >> hashsums.txt
"$shapath" /tmp/$ksversion-bin-dotnet.zip >> hashsums.txt
mv /tmp/$ksversion-bin.zip .
mv /tmp/$ksversion-bin-dotnet.zip .
cp "../Kernel Simulator/KSBuild/net48/Kernel Simulator.pdb" ./$ksversion.pdb
cp "../Kernel Simulator/KSBuild/net8.0/Kernel Simulator.pdb" ./$ksversion-dotnet.pdb
echo Build and pack successful.
exit 0
