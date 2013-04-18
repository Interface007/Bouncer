﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sem.GenericHelpers.IO.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sem.GenericHelpers.IO.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Access to the path &apos;{0}&apos; was denied..
        /// </summary>
        internal static string Error_AccessDenied_Path {
            get {
                return ResourceManager.GetString("Error_AccessDenied_Path", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot create &apos;{0}&apos; because a file or directory with the same name already exists..
        /// </summary>
        internal static string Error_AlreadyExists {
            get {
                return ResourceManager.GetString("Error_AlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find a part of the path &apos;{0}&apos;..
        /// </summary>
        internal static string Error_DirectoryNotFound {
            get {
                return ResourceManager.GetString("Error_DirectoryNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find the drive &apos;{0}&apos;. The drive might not be ready or might not be mapped..
        /// </summary>
        internal static string Error_DriveNotFound {
            get {
                return ResourceManager.GetString("Error_DriveNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file &apos;{0}&apos; already exists..
        /// </summary>
        internal static string Error_FileAlreadyExists {
            get {
                return ResourceManager.GetString("Error_FileAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified stream name contains invalid characters..
        /// </summary>
        internal static string Error_InvalidFileChars {
            get {
                return ResourceManager.GetString("Error_InvalidFileChars", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified mode &apos;{0}&apos; is not supported..
        /// </summary>
        internal static string Error_InvalidMode {
            get {
                return ResourceManager.GetString("Error_InvalidMode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified file name &apos;{0}&apos; is not a disk-based file..
        /// </summary>
        internal static string Error_NonFile {
            get {
                return ResourceManager.GetString("Error_NonFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The process cannot access the file &apos;{0}&apos; because it is being used by another process..
        /// </summary>
        internal static string Error_SharingViolation {
            get {
                return ResourceManager.GetString("Error_SharingViolation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified alternate data stream &apos;{0}&apos; already exists on file &apos;{1}&apos;..
        /// </summary>
        internal static string Error_StreamExists {
            get {
                return ResourceManager.GetString("Error_StreamExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified alternate data stream &apos;{0}&apos; does not exist on file &apos;{1}&apos;..
        /// </summary>
        internal static string Error_StreamNotFound {
            get {
                return ResourceManager.GetString("Error_StreamNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown error: {0}.
        /// </summary>
        internal static string Error_UnknownError {
            get {
                return ResourceManager.GetString("Error_UnknownError", resourceCulture);
            }
        }
    }
}
