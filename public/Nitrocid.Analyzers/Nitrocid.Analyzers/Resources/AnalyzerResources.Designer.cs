﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nitrocid.Analyzers.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class AnalyzerResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AnalyzerResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Nitrocid.Analyzers.Resources.AnalyzerResources", typeof(AnalyzerResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SetConsoleColor(Color, true) not only brings better color support provided by the appropriate VT sequences, but it can also use true color. Console.BackgroundColor only handles 16 colors..
        /// </summary>
        internal static string ConsoleBackColorUsageAnalyzerDescription {
            get {
                return ResourceManager.GetString("ConsoleBackColorUsageAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Caller uses Console.BackgroundColor instead of SetConsoleColor(Color, true).
        /// </summary>
        internal static string ConsoleBackColorUsageAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("ConsoleBackColorUsageAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use SetConsoleColor(Color, true) instead of Console.BackgroundColor.
        /// </summary>
        internal static string ConsoleBackColorUsageAnalyzerTitle {
            get {
                return ResourceManager.GetString("ConsoleBackColorUsageAnalyzerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SetConsoleColor(Color) not only brings better color support provided by the appropriate VT sequences, but it can also use true color. Console.ForegroundColor only handles 16 colors..
        /// </summary>
        internal static string ConsoleForeColorUsageAnalyzerDescription {
            get {
                return ResourceManager.GetString("ConsoleForeColorUsageAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Caller uses Console.ForegroundColor instead of SetConsoleColor(Color).
        /// </summary>
        internal static string ConsoleForeColorUsageAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("ConsoleForeColorUsageAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use SetConsoleColor(Color) instead of Console.ForegroundColor.
        /// </summary>
        internal static string ConsoleForeColorUsageAnalyzerTitle {
            get {
                return ResourceManager.GetString("ConsoleForeColorUsageAnalyzerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ReadLine() provided by the input helper from Nitrocid allows you to seamlessly read a user input with settings provided by Terminaux..
        /// </summary>
        internal static string ConsoleReadLineUsageAnalyzerDescription {
            get {
                return ResourceManager.GetString("ConsoleReadLineUsageAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Caller uses Console.ReadLine instead of ReadLine().
        /// </summary>
        internal static string ConsoleReadLineUsageAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("ConsoleReadLineUsageAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use ReadLine() instead of Console.ReadLine.
        /// </summary>
        internal static string ConsoleReadLineUsageAnalyzerTitle {
            get {
                return ResourceManager.GetString("ConsoleReadLineUsageAnalyzerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SetTitle() uses the VT sequence to set the title, while Console.Title works in certain conditions..
        /// </summary>
        internal static string ConsoleTitleUsageAnalyzerDescription {
            get {
                return ResourceManager.GetString("ConsoleTitleUsageAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Caller uses Console.Title instead of SetTitle().
        /// </summary>
        internal static string ConsoleTitleUsageAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("ConsoleTitleUsageAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use SetTitle() instead of Console.Title.
        /// </summary>
        internal static string ConsoleTitleUsageAnalyzerTitle {
            get {
                return ResourceManager.GetString("ConsoleTitleUsageAnalyzerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ConsoleWrapper makes sure that your console is not a dumb console. This class is a wrapper for the Console class so that it works cross-platform, while Console contains some platform-dependent APIs..
        /// </summary>
        internal static string ConsoleWrapperUsageAnalyzerDescription {
            get {
                return ResourceManager.GetString("ConsoleWrapperUsageAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Caller uses Console instead of ConsoleWrapper.
        /// </summary>
        internal static string ConsoleWrapperUsageAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("ConsoleWrapperUsageAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use ConsoleWrapper instead of Console.
        /// </summary>
        internal static string ConsoleWrapperUsageAnalyzerTitle {
            get {
                return ResourceManager.GetString("ConsoleWrapperUsageAnalyzerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TextWriterColor.Write() contains workarounds for VT sequences needed for Linux hosts to properly report the correct position post-write. Its overloads also allow you to specify the color and the line writing..
        /// </summary>
        internal static string ConsoleWriteLineUsageAnalyzerDescription {
            get {
                return ResourceManager.GetString("ConsoleWriteLineUsageAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Caller uses Console.WriteLine instead of TWC.Write().
        /// </summary>
        internal static string ConsoleWriteLineUsageAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("ConsoleWriteLineUsageAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use TWC.Write() instead of Console.WriteLine.
        /// </summary>
        internal static string ConsoleWriteLineUsageAnalyzerTitle {
            get {
                return ResourceManager.GetString("ConsoleWriteLineUsageAnalyzerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TextWriterColor.Write() contains workarounds for VT sequences needed for Linux hosts to properly report the correct position post-write. Its overloads also allow you to specify the color and the line writing..
        /// </summary>
        internal static string ConsoleWriteUsageAnalyzerDescription {
            get {
                return ResourceManager.GetString("ConsoleWriteUsageAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Caller uses Console.Write instead of TWC.Write().
        /// </summary>
        internal static string ConsoleWriteUsageAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("ConsoleWriteUsageAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use TWC.Write() instead of Console.Write.
        /// </summary>
        internal static string ConsoleWriteUsageAnalyzerTitle {
            get {
                return ResourceManager.GetString("ConsoleWriteUsageAnalyzerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TextTools.FormatString() uses the error handler to handle unknown formatting errors and returns the unformatted string if such errors happen, but string.Format() immediately throws..
        /// </summary>
        internal static string StringFormatAnalyzerDescription {
            get {
                return ResourceManager.GetString("StringFormatAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Caller uses string.Format() instead of TextTools.FormatString().
        /// </summary>
        internal static string StringFormatAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("StringFormatAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use TextTools.FormatString() instead of string.Format().
        /// </summary>
        internal static string StringFormatAnalyzerTitle {
            get {
                return ResourceManager.GetString("StringFormatAnalyzerTitle", resourceCulture);
            }
        }
    }
}
