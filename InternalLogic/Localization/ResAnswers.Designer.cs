﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TwitchClips.InternalLogic.Localization {
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
    public class ResAnswers {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResAnswers() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AoSBuilderProject.Controllers.GeneralData.Resources.ResAnswersEng", typeof(ResAnswers).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong request parameters..
        /// </summary>
        public static string BadRequest {
            get {
                return ResourceManager.GetString("BadRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t access this data with current authorization..
        /// </summary>
        public static string CantAccess {
            get {
                return ResourceManager.GetString("CantAccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New entity created successfully..
        /// </summary>
        public static string Created {
            get {
                return ResourceManager.GetString("Created", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Provided data already exist..
        /// </summary>
        public static string EmailUsernameExist {
            get {
                return ResourceManager.GetString("EmailUsernameExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while processing request..
        /// </summary>
        public static string ErrorRequest {
            get {
                return ResourceManager.GetString("ErrorRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t get any results from database..
        /// </summary>
        public static string NotFoundNullContext {
            get {
                return ResourceManager.GetString("NotFoundNullContext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t get specific result from database..
        /// </summary>
        public static string NotFoundNullEntity {
            get {
                return ResourceManager.GetString("NotFoundNullEntity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Request handled successfully..
        /// </summary>
        public static string Success {
            get {
                return ResourceManager.GetString("Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User doesn&apos;t exist in database..
        /// </summary>
        public static string UserNotFound {
            get {
                return ResourceManager.GetString("UserNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to String dont match regular expression:.
        /// </summary>
        public static string ValidationErrorRegEx {
            get {
                return ResourceManager.GetString("ValidationErrorRegEx", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while proccessing JWT token to get values..
        /// </summary>
        public static string WrongJWT {
            get {
                return ResourceManager.GetString("WrongJWT", resourceCulture);
            }
        }
    }
}
