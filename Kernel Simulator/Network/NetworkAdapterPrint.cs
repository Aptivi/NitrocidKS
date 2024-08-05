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
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;

namespace KS.Network
{
    public static class NetworkAdapterPrint
    {

        /// <summary>
        /// Print each of adapters' properties to the console.
        /// </summary>
        public static void PrintAdapterProperties()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            bool NoV4 = default, NoV6 = default, Failed = default;
            var gp = IPGlobalProperties.GetIPGlobalProperties();
            var gs4 = gp.GetIPv4GlobalStatistics();
            long adapterNumber = 0L;

            // Probe for adapter capabilities
            var p = default(IPv4InterfaceProperties);
            var p6 = default(IPv6InterfaceProperties);
            foreach (NetworkInterface adapter in adapters)
            {
                adapterNumber += 1L;
                TextWriters.Write("==========================================", true, KernelColorTools.ColTypes.Neutral);

                // See if it supports IPv6
                if (!adapter.Supports(NetworkInterfaceComponent.IPv6))
                {
                    DebugWriter.Wdbg(DebugLevel.W, "{0} doesn't support IPv6. Trying to get information about IPv4.", adapter.Description);
                    TextWriters.Write(Translate.DoTranslation("Adapter {0} doesn't support IPv6. Continuing..."), true, KernelColorTools.ColTypes.Error, adapter.Description);
                    NoV6 = true;
                }

                // See if it supports IPv4
                if (!adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    DebugWriter.Wdbg(DebugLevel.E, "{0} doesn't support IPv4.", adapter.Description);
                    TextWriters.Write(Translate.DoTranslation("Adapter {0} doesn't support IPv4. Probe failed."), true, KernelColorTools.ColTypes.Error, adapter.Description);
                    NoV4 = true;
                }

                // Get adapter IPv(4/6) properties
                if (NetworkAdapter.IsInternetAdapter(adapter) & !NoV4)
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Adapter type of {0}: {1}", adapter.Description, adapter.NetworkInterfaceType.ToString());
                    var adapterProperties = adapter.GetIPProperties();
                    var s = adapter.GetIPv4Statistics();
                    try
                    {
                        p = adapterProperties.GetIPv4Properties();
                        p6 = adapterProperties.GetIPv6Properties();
                    }
                    catch (NetworkInformationException ex)
                    {
                        if (p6 is null)
                        {
                            DebugWriter.Wdbg(DebugLevel.W, "Failed to get IPv6 properties.");
                            TextWriters.Write(Translate.DoTranslation("Failed to get IPv6 properties for adapter {0}. Continuing..."), true, KernelColorTools.ColTypes.Error, adapter.Description);
                        }
                        if (p is null)
                        {
                            DebugWriter.Wdbg(DebugLevel.E, "Failed to get IPv4 properties.");
                            TextWriters.Write(Translate.DoTranslation("Failed to get properties for adapter {0}"), true, KernelColorTools.ColTypes.Error, adapter.Description);
                            Failed = true;
                        }
                        DebugWriter.WStkTrc(ex);
                    }

                    // Check if statistics is nothing
                    if (s is null)
                    {
                        DebugWriter.Wdbg(DebugLevel.E, "Failed to get statistics.");
                        TextWriters.Write(Translate.DoTranslation("Failed to get statistics for adapter {0}"), true, KernelColorTools.ColTypes.Error, adapter.Description);
                        Failed = true;
                    }

                    // Print adapter infos if not failed
                    if (!Failed)
                    {
                        PrintAdapterIPv4Info(adapter, p, s, adapterNumber);
                        // Additionally, print adapter IPv6 info if available
                        if (!NoV6)
                        {
                            PrintAdapterIPv6Info(adapter, p6, adapterNumber);
                        }
                    }
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Adapter {0} doesn't belong in netinfo because the type is {1}", adapter.Description, adapter.NetworkInterfaceType);
                }
            }

            // Print general IPv4 and IPv6 information
            if (Flags.GeneralNetworkInformation)
            {
                TextWriters.Write("==========================================", true, KernelColorTools.ColTypes.Neutral);
                PrintGeneralNetInfo(gs4);
            }
        }

        /// <summary>
        /// Prints IPv4 info for adapter
        /// </summary>
        /// <param name="NInterface">A network interface or adapter</param>
        /// <param name="Properties">Network properties</param>
        /// <param name="Statistics">Network statistics</param>
        /// <param name="AdapterNumber">Reference adapter number</param>
        public static void PrintAdapterIPv4Info(NetworkInterface NInterface, IPv4InterfaceProperties Properties, IPv4InterfaceStatistics Statistics, long AdapterNumber)
        {
            if (Flags.ExtensiveAdapterInformation)
            {
                TextWriters.Write(Translate.DoTranslation("IPv4 information:") + Kernel.Kernel.NewLine +
                    Translate.DoTranslation("Adapter Number:") + " {0}" + Kernel.Kernel.NewLine +
                    Translate.DoTranslation("Adapter Name:") + " {1}" + Kernel.Kernel.NewLine +
                    Translate.DoTranslation("Maximum Transmission Unit: {2} Units") + Kernel.Kernel.NewLine +
                    Translate.DoTranslation("Non-unicast packets:") + " {3}/{4}" + Kernel.Kernel.NewLine +
                    Translate.DoTranslation("Unicast packets:") + " {5}/{6}" + Kernel.Kernel.NewLine +
                    Translate.DoTranslation("Error incoming/outgoing packets:") + " {7}/{8}",
                    true, KernelColorTools.ColTypes.Neutral,
                    AdapterNumber, NInterface.Description, Properties.Mtu, Statistics.NonUnicastPacketsSent,
                    Statistics.NonUnicastPacketsReceived, Statistics.UnicastPacketsSent,
                    Statistics.UnicastPacketsReceived, Statistics.IncomingPacketsWithErrors,
                    Statistics.OutgoingPacketsWithErrors
                );
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("IPv4 information:") + Kernel.Kernel.NewLine +
                    Translate.DoTranslation("Adapter Number:") + " {0}" + Kernel.Kernel.NewLine +
                    Translate.DoTranslation("Adapter Name:") + " {1}",
                    true, KernelColorTools.ColTypes.Neutral,
                    AdapterNumber, NInterface.Description
                );
            }
        }

        /// <summary>
        /// Prints IPv6 info for adapter
        /// </summary>
        /// <param name="NInterface">A network interface or adapter</param>
        /// <param name="Properties">Network properties</param>
        /// <param name="AdapterNumber">Reference adapter number</param>
        public static void PrintAdapterIPv6Info(NetworkInterface NInterface, IPv6InterfaceProperties Properties, long AdapterNumber)
        {
            if (Flags.ExtensiveAdapterInformation)
            {
                TextWriters.Write(Translate.DoTranslation("IPv6 information:") + Kernel.Kernel.NewLine + Translate.DoTranslation("Adapter Number:") + " {0}" + Kernel.Kernel.NewLine + Translate.DoTranslation("Adapter Name:") + " {1}" + Kernel.Kernel.NewLine + Translate.DoTranslation("Maximum Transmission Unit: {2} Units"), true, KernelColorTools.ColTypes.Neutral, AdapterNumber, NInterface.Description, Properties.Mtu);
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("IPv6 information:") + Kernel.Kernel.NewLine + Translate.DoTranslation("Adapter Number:") + " {0}" + Kernel.Kernel.NewLine + Translate.DoTranslation("Adapter Name:") + " {1}", true, KernelColorTools.ColTypes.Neutral, AdapterNumber, NInterface.Description);
            }
        }

        /// <summary>
        /// Prints general network info
        /// </summary>
        /// <param name="IPv4Stat">IPv4 general statistics</param>
        public static void PrintGeneralNetInfo(IPGlobalStatistics IPv4Stat)
        {
            TextWriters.Write(
                Translate.DoTranslation("General IPv4 properties") + Kernel.Kernel.NewLine +
                Translate.DoTranslation("Packets (inbound):") + " {0}/{1}" + Kernel.Kernel.NewLine +
                Translate.DoTranslation("Packets (outbound):") + " {2}/{3}" + Kernel.Kernel.NewLine +
                Translate.DoTranslation("Errors in received packets:") + " {4}/{5}/{6}",
                true, KernelColorTools.ColTypes.Neutral,
                IPv4Stat.ReceivedPackets, IPv4Stat.ReceivedPacketsDelivered,
                IPv4Stat.OutputPacketRequests, IPv4Stat.OutputPacketsDiscarded,
                IPv4Stat.ReceivedPacketsWithAddressErrors, IPv4Stat.ReceivedPacketsWithHeadersErrors,
                IPv4Stat.ReceivedPacketsWithUnknownProtocol
            );
        }

    }
}
