#
#    Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
#

OUTPUTS = public/Nitrocid/KSBuild public/*/obj public/*/*/obj private/*/bin private/*/obj debian/kernel-simulator
OUTPUT = public/Nitrocid/KSBuild/net8.0
BINARIES = assets/ks-n assets/ks-jl
MANUALS = assets/*.1
DESKTOPS = assets/ks.desktop
BRANDINGS = public/Nitrocid/OfficialAppIcon-KernelSimulator-512.png
ARCH := $(shell if [ `uname -m` = "x86_64" ]; then echo "linux-x64"; else echo "linux-arm64"; fi)

.PHONY: all debian-install

# General use

all: all-online

all-online:
	$(MAKE) -C tools invoke-build

# Below targets are for Debian packaging only

debian-all-offline:
	$(MAKE) -C tools invoke-build-offline

debian-init-offline:
	$(MAKE) -C tools debian-invoke-init-offline

debian-install:
	mkdir -m 755 -p debian/kernel-simulator/usr/bin debian/kernel-simulator/usr/lib/ks debian/kernel-simulator/usr/share/applications
	install -m 755 -t debian/kernel-simulator/usr/bin/ $(BINARIES)
	install -m 755 -t debian/ $(MANUALS)
	find $(OUTPUT) -mindepth 1 -type d -exec sh -c 'mkdir -p -m 755 "debian/kernel-simulator/usr/lib/ks/$$(realpath --relative-to $(OUTPUT) "$$0")"' {} \;
	find $(OUTPUT) -mindepth 1 -type f -exec sh -c 'install -m 644 -t "debian/kernel-simulator/usr/lib/ks/$$(dirname $$(realpath --relative-to $(OUTPUT) "$$0"))" "$$0"' {} \;
	install -m 755 -t debian/kernel-simulator/usr/share/applications/ $(DESKTOPS)
	install -m 755 -t debian/kernel-simulator/usr/lib/ks/ $(BRANDINGS)
	mv debian/kernel-simulator/usr/bin/ks-n debian/kernel-simulator/usr/bin/ks
	find debian/kernel-simulator/usr/lib/ks/runtimes/ -mindepth 1 -maxdepth 1 -not -name $(ARCH) -type d -exec rm -rf {} \;
	find debian/kernel-simulator/usr/lib/ks/Addons/Extras.BassBoom/runtimes/ -mindepth 1 -maxdepth 1 -not -name $(ARCH) -type d -exec rm -rf {} \;
	find debian/kernel-simulator/usr/lib/ks/Addons/Extras.GitShell/runtimes/ -mindepth 1 -maxdepth 1 -not -name $(ARCH) -type d -exec rm -rf {} \;
	find debian/kernel-simulator/usr/lib/ks/Addons/Extras.Contacts/runtimes/ -mindepth 1 -maxdepth 1 -not -name $(ARCH) -type d -exec rm -rf {} \;
	find debian/kernel-simulator/usr/lib/ks/Addons/Extras.MailShell/runtimes/ -mindepth 1 -maxdepth 1 -not -name $(ARCH) -type d -exec rm -rf {} \;
	find debian/kernel-simulator/usr/lib/ks/Addons/Extras.SqlShell/runtimes/ -mindepth 1 -maxdepth 1 -not -name $(ARCH) -type d -exec rm -rf {} \;

echo-arch:
	@echo $(ARCH)

clean:
	rm -rf $(OUTPUTS)

# This makefile is just a wrapper for tools scripts.
