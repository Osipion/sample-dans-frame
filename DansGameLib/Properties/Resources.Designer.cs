﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DansGameLib.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DansGameLib.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to CurrentCharacter.
        /// </summary>
        internal static string CharacterFileName {
            get {
                return ResourceManager.GetString("CharacterFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap GameScreen {
            get {
                object obj = ResourceManager.GetObject("GameScreen", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;configuration&gt;
        ///  &lt;log4net&gt;
        ///    &lt;appender name=&quot;RollingFileAppender&quot; type=&quot;log4net.Appender.RollingFileAppender&quot;&gt;
        ///      &lt;file value=&quot;test_dans_game_core_log.txt&quot; /&gt;
        ///      &lt;appendToFile value=&quot;true&quot; /&gt;
        ///      &lt;rollingStyle value=&quot;Size&quot; /&gt;
        ///      &lt;maxSizeRollBackups value=&quot;5&quot; /&gt;
        ///      &lt;maximumFileSize value=&quot;10MB&quot; /&gt;
        ///      &lt;staticLogFileName value=&quot;true&quot; /&gt;
        ///      &lt;layout type=&quot;log4net.Layout.PatternLayout&quot;&gt;
        ///        &lt;conversionPattern value=&quot;%date [%thread] %level %logger{1} - %message%newline&quot; /&gt;
        ///    [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string test_logging_config {
            get {
                return ResourceManager.GetString("test_logging_config", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;TypeRegister xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot; xmlns:xsd=&quot;http://www.w3.org/2001/XMLSchema&quot;&gt;
        ///  &lt;Entries&gt;
        ///    &lt;TypeRegisterEntry CreationIndex=&quot;0&quot;&gt;
        ///      &lt;Value&gt;
        ///        &lt;IsSingleton&gt;true&lt;/IsSingleton&gt;
        ///        &lt;InterfaceType&gt;DansGameCore.Serialization.ISerializableCharacter, DansGameCore, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null&lt;/InterfaceType&gt;
        ///        &lt;ClassType&gt;DansGameCore.Serialization.SerializableCharacter, DansGameCore, Ver [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string TypeRegistry {
            get {
                return ResourceManager.GetString("TypeRegistry", resourceCulture);
            }
        }
    }
}
