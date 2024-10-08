#!/bin/bash

#    Nitrocid KS  Copyright (C) 2018-2021  Aptivi
#
#    This file is part of Nitrocid KS
#
#    Nitrocid KS is free software: you can redistribute it and/or modify
#    it under the terms of the GNU General Public License as published by
#    the Free Software Foundation, either version 3 of the License, or
#    (at your option) any later version.
#
#    Nitrocid KS is distributed in the hope that it will be useful,
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
cd "../public/Nitrocid/KSBuild/net8.0/" && "$zippath" -r /tmp/$ksversion-bin.zip . && cd -
cd "../public/Nitrocid/KSBuild/net8.0/" && "$zippath" -r /tmp/$ksversion-bin-lite.zip . -x "./Addons/*" && cd -
cd "../public/Nitrocid/KSBuild/net8.0/Addons/" && "$zippath" -r /tmp/$ksversion-addons.zip . && cd -
cd "../public/Nitrocid/KSAnalyzer/netstandard2.0/" && "$zippath" -r /tmp/$ksversion-analyzers.zip . && cd -
cd "../public/Nitrocid/KSAnalyzer/net8.0/" && "$zippath" -r /tmp/$ksversion-mod-analyzer.zip . && cd -
if [ ! $? == 0 ]; then
	echo Packing failed.
	exit 1
fi

# Inform success
mv /tmp/$ksversion-bin.zip .
mv /tmp/$ksversion-bin-lite.zip .
mv /tmp/$ksversion-addons.zip .
mv /tmp/$ksversion-analyzers.zip .
mv /tmp/$ksversion-mod-analyzer.zip .
cp changes.chg $ksversion-changes.chg

echo Build and pack successful.
exit 0
