﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nitrocid.Extras.NameGen.Resources {
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
    internal class AddonResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AddonResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Nitrocid.Extras.NameGen.Resources.AddonResources", typeof(AddonResources).Assembly);
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
        ///   Looks up a localized string similar to [
        ///    {
        ///        &quot;Name&quot;: &quot;PersonLookup&quot;,
        ///        &quot;Desc&quot;: &quot;Settings for this screensaver are available here.&quot;,
        ///        &quot;Path&quot;: &quot;Screensaver.PersonLookup&quot;,
        ///        &quot;Keys&quot;: [
        ///            {
        ///                &quot;Name&quot;: &quot;Delay in Milliseconds&quot;,
        ///                &quot;Type&quot;: &quot;SInt&quot;,
        ///                &quot;Variable&quot;: &quot;PersonLookupDelay&quot;,
        ///                &quot;Description&quot;: &quot;How many milliseconds to wait before making the next write?&quot;
        ///            },
        ///            {
        ///                &quot;Name&quot;: &quot;New Screen Delay in Milliseconds&quot;,
        ///  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string NameGenSaverSettings {
            get {
                return ResourceManager.GetString("NameGenSaverSettings", resourceCulture);
            }
        }
    }
}
