﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VisioAddin.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.9.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"""{\""Servers\"":[{\""Name\"":\""www.visio-shapes.com\"",\""Url\"":\""https://www.visio-shapes.com\"",\""User\"":\""\"",\""Password\"":\""\""},{\""Name\"":\""localhost\"",\""Url\"":\""http://127.0.0.1:5000\"",\""User\"":\""\"",\""Password\"":\""\""}],\""CurrentServer\"":\""www.visio-shapes.com\""}""")]
        public string ServerSettings {
            get {
                return ((string)(this["ServerSettings"]));
            }
            set {
                this["ServerSettings"] = value;
            }
        }
    }
}
