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

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Splash;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using SpecProbe;
using SpecProbe.Parts.Types;
using Terminaux.Writer.FancyWriters;
using SpecProbe.Parts;

namespace KS.Hardware
{
    public static class HardwareList
    {

        /// <summary>
        /// Lists simple information about hardware
        /// </summary>
        public static void ListHardware()
        {
            var processors = HardwareProbe.ProbeProcessor();
            var memory = HardwareProbe.ProbePcMemory();
            var graphics = HardwareProbe.ProbeGraphics();
            var hardDrives = HardwareProbe.ProbeHardDrive();

            // Verify the types
            if (processors is not ProcessorPart[] procDict)
            {
                SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Failed to parse the CPU info. Ensure that it's a valid info list."), 0, KernelColorTools.ColTypes.Error);
                return;
            }
            if (memory is not MemoryPart[] memList)
            {
                SplashReport.ReportProgress("RAM: " + Translate.DoTranslation("Failed to parse the RAM info. Ensure that it's a valid info list."), 0, KernelColorTools.ColTypes.Error);
                return;
            }
            if (graphics is not VideoPart[] gpuDict)
            {
                SplashReport.ReportProgress("GPU: " + Translate.DoTranslation("Failed to parse the GPU info. Ensure that it's a valid info list."), 0, KernelColorTools.ColTypes.Error);
                return;
            }
            if (hardDrives is not HardDiskPart[] hddDict)
            {
                SplashReport.ReportProgress("HDD: " + Translate.DoTranslation("Failed to parse the HDD info. Ensure that it's a valid info list."), 0, KernelColorTools.ColTypes.Error);
                return;
            }

            // Print information about the probed hardware, starting from the CPU info
            foreach (var cpu in procDict)
            {
                SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Processor name:") + " {0}", 0, KernelColorTools.ColTypes.Neutral, cpu.Name);
                SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Processor clock speed:") + " {0} MHz", 0, KernelColorTools.ColTypes.Neutral, $"{cpu.Speed}");
                SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Total number of processors:") + $" {cpu.LogicalCores} ({cpu.ProcessorCores} x{cpu.Cores})", 3, KernelColorTools.ColTypes.Neutral);
            }
            PrintErrors(HardwarePartType.Processor);

            // Print RAM info
            var mem = memList[0];
            SplashReport.ReportProgress("RAM: " + Translate.DoTranslation("Total memory:") + " {0}", 2, KernelColorTools.ColTypes.Neutral, $"{mem.TotalMemory}");
            PrintErrors(HardwarePartType.Memory);

            // GPU info
            foreach (var gpu in gpuDict)
                SplashReport.ReportProgress("GPU: " + Translate.DoTranslation("Graphics card:") + " {0}", 0, KernelColorTools.ColTypes.Neutral, gpu.VideoCardName);
            PrintErrors(HardwarePartType.Video);

            // Drive Info
            foreach (var hdd in hddDict)
            {
                SplashReport.ReportProgress("HDD: " + Translate.DoTranslation("Disk size:") + " {0}", 0, KernelColorTools.ColTypes.Neutral, $"{hdd.HardDiskSize}");

                // Partition info
                foreach (var part in hdd.Partitions)
                    SplashReport.ReportProgress("HDD [{0}]: " + Translate.DoTranslation("Partition size:") + " {1}", 0, KernelColorTools.ColTypes.Neutral, $"{hdd.HardDiskNumber}", $"{part.PartitionSize}");
            }
            PrintErrors(HardwarePartType.HardDisk);
        }

        /// <summary>
        /// Lists information about hardware
        /// </summary>
        /// <param name="HardwareType">Hadrware type defined by SpecProbe. If "all", prints all information.</param>
        public static void ListHardware(string HardwareType)
        {
            (string, HardwarePartType)[] supportedTypes =
            [
                ("CPU", HardwarePartType.Processor),
                ("RAM", HardwarePartType.Memory),
                ("HDD", HardwarePartType.HardDisk),
                ("GPU", HardwarePartType.Video)
            ];
            if (HardwareType == "all")
            {
                foreach (var supportedType in supportedTypes)
                {
                    ListHardwareInternal(supportedType.Item1);
                    PrintErrors(supportedType.Item2);
                }
            }
            else
            {
                ListHardwareInternal(HardwareType);
                foreach (var supportedType in supportedTypes)
                    PrintErrors(supportedType.Item2);
            }
        }

        private static void ListHardwareInternal(string hardwareType)
        {
            SeparatorWriterColor.WriteSeparator(hardwareType, true);
            switch (hardwareType)
            {
                case "CPU":
                    {
                        var hardwareList = HardwareProbe.ProbeProcessor() as ProcessorPart[];
                        if (hardwareList is not null)
                        {
                            foreach (var processor in hardwareList)
                            {
                                TextWriters.Write(Translate.DoTranslation("Processor name:"), false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write($" {processor.Name}", true, KernelColorTools.ColTypes.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Processor vendor:"), false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write($" {processor.Vendor} [CPUID: {processor.CpuidVendor}]", true, KernelColorTools.ColTypes.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Clock speed:"), false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write($" {processor.Speed} MHz", true, KernelColorTools.ColTypes.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Total cores:"), false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write($" {processor.LogicalCores} ({processor.ProcessorCores} x{processor.Cores})", true, KernelColorTools.ColTypes.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Cache sizes:"), false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write($" {processor.L1CacheSize} L1, {processor.L2CacheSize} L2, {processor.L3CacheSize} L3", true, KernelColorTools.ColTypes.ListValue);
                            }
                        }
                        break;
                    }
                case "RAM":
                    {
                        var hardwareList = HardwareProbe.ProbePcMemory() as MemoryPart[];
                        if (hardwareList is not null)
                        {
                            foreach (var ram in hardwareList)
                            {
                                TextWriters.Write(Translate.DoTranslation("Total usable memory:"), false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write($" {ram.TotalMemory}", true, KernelColorTools.ColTypes.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Total memory:"), false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write($" {ram.TotalPhysicalMemory}", true, KernelColorTools.ColTypes.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Reserved memory:"), false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write($" {ram.SystemReservedMemory}", true, KernelColorTools.ColTypes.ListValue);
                            }
                        }
                        break;
                    }
                case "HDD":
                    {
                        var hardwareList = HardwareProbe.ProbeHardDrive() as HardDiskPart[];
                        if (hardwareList is not null)
                        {
                            foreach (var hdd in hardwareList)
                            {
                                TextWriters.Write(Translate.DoTranslation("Disk number:"), false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write($" {hdd.HardDiskNumber}", true, KernelColorTools.ColTypes.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Disk size:"), false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write($" {hdd.HardDiskSize}", true, KernelColorTools.ColTypes.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Partitions:"), false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write($" {hdd.PartitionCount}", true, KernelColorTools.ColTypes.ListValue);
                                foreach (var part in hdd.Partitions)
                                {
                                    TextWriters.Write(Translate.DoTranslation("Partition number:"), false, KernelColorTools.ColTypes.ListEntry);
                                    TextWriters.Write($" {part.PartitionNumber}", true, KernelColorTools.ColTypes.ListValue);
                                    TextWriters.Write(Translate.DoTranslation("Partition size:"), false, KernelColorTools.ColTypes.ListEntry);
                                    TextWriters.Write($" {part.PartitionSize}", true, KernelColorTools.ColTypes.ListValue);
                                }
                            }
                        }
                        break;
                    }
                case "GPU":
                    {
                        var hardwareList = HardwareProbe.ProbeGraphics() as VideoPart[];
                        if (hardwareList is not null)
                        {
                            foreach (var gpu in hardwareList)
                            {
                                TextWriters.Write(Translate.DoTranslation("Graphics card name:"), false, KernelColorTools.ColTypes.ListEntry);
                                TextWriters.Write($" {gpu.VideoCardName}", true, KernelColorTools.ColTypes.ListValue);
                            }
                        }
                        break;
                    }
                default:
                    TextWriters.Write(Translate.DoTranslation("Either the hardware type {0} is not probed, or is not valid."), true, KernelColorTools.ColTypes.Error, hardwareType);
                    break;
            }
        }

        private static void PrintErrors(HardwarePartType type)
        {
            var errors = HardwareProber.GetParseExceptions(type);
            if (errors.Length > 0)
            {
                SplashReport.ReportProgress(Translate.DoTranslation("SpecProbe failed to parse some of the hardware. Below are the errors reported by SpecProbe:"), 0, KernelColorTools.ColTypes.Error);
                DebugWriter.Wdbg(DebugLevel.E, "SpecProbe failed to parse hardware due to the following errors:");
                foreach (var error in errors)
                {
                    DebugWriter.Wdbg(DebugLevel.E, $"- {error.Message}");
                    DebugWriter.WStkTrc(error);
                    SplashReport.ReportProgress(error.Message, 0, KernelColorTools.ColTypes.Error);
                }
            }
        }

    }
}
