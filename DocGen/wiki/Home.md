# Kernel Simulator

Kernel Simulator is an application which simulates our future-planned kernel as we've imagined it. It has all the usual applications, including filesystem manipulation, kernel mods, basic scripting, and many more awesome features.

## How to boot with GRILO?

Place all Kernel Simulator files to `%localappdata%/GRILO/Bootables/ks/` in Windows or `~/.config/grilo/Bootables/ks`.

## List of supported versions

Here are all the listed versions of Kernel Simulator which is either supported or out of support. You can also see the release date and the expected end of life date.

> [!IMPORTANT]
> If your kernel has the status of Reached EOL, you must upgrade to the supported version. Otherwise, you can get support from us as long as the version you're running is supported.

### Compatibility warnings

Each revision of the kernel API has several incompatibilities that break the existing mods. The examples are:

- KS 0.0.16.x series or later is not backwards-compatible with the 0.0.15.x series or below. To remedy this issue for kernel configurations, use the KSConverter application supplied within Kernel Simulator.
- Second-generation versions of KS are not backwards-compatible with the first-generation versions. Upgrade your mods to support the latest API changes, making changes if necessary.
