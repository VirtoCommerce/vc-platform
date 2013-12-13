#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using VirtoCommerce.Client.Extensions;

namespace VirtoCommerce.Client
{
    /// <summary>
    /// Class Settings.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// The is host by IIS
        /// </summary>
        static readonly bool isHostByIIS;
        /// <summary>
        /// Initializes static members of the <see cref="Settings"/> class.
        /// </summary>
        static Settings()
        {
            isHostByIIS = AppDomain.CurrentDomain.FriendlyName.ToUpper().StartsWith("/LM/W3SVC/");
        }
        /// <summary>
        /// Gets the base directory.
        /// </summary>
        /// <value>The base directory.</value>
        public static string BaseDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }
        }
        /// <summary>
        /// Gets the bin directory.
        /// </summary>
        /// <value>The bin directory.</value>
        public static string BinDirectory
        {
            get 
			{
	            return IsWebApplication ? 
					AppDomain.CurrentDomain.SetupInformation.PrivateBinPath : 
					AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }
        }
        /// <summary>
        /// Gets the components directory.
        /// </summary>
        /// <value>The components directory.</value>
        public static string ComponentsDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Components";
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is web application.
        /// </summary>
        /// <value><c>true</c> if this instance is web application; otherwise, <c>false</c>.</value>
        public static bool IsWebApplication
        {
            get
            {
                return System.IO.Path.GetFileName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile).EqualsOrNullEmpty("Web.config", StringComparison.OrdinalIgnoreCase);
            }
        }
        /// <summary>
        /// Gets a value indicating whether this instance is host by IIS.
        /// </summary>
        /// <value><c>true</c> if this instance is host by IIS; otherwise, <c>false</c>.</value>
        public static bool IsHostByIIS
        {
            get
            {
                return isHostByIIS;
            }
        }
    }


}
