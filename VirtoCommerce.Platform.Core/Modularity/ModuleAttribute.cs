using System;

namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Indicates that the class should be considered a named module using the
    /// provided module name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ModuleAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the module should be loaded at startup. 
        /// </summary>
        /// When <see langword="true"/> (default value), it indicates that this module should be loaded at startup. 
        /// Otherwise you should explicitly load this module on demand.
        /// <value>A <see cref="bool"/> value.</value>
        [Obsolete("StartupLoaded has been replaced by the OnDemand property.")]
        public bool StartupLoaded
        {
            get { return !OnDemand; }
            set { OnDemand = !value; }
        }


        /// <summary>
        /// Gets or sets the value indicating whether the module should be loaded OnDemand.
        /// </summary>
        /// When <see langword="false"/> (default value), it indicates the module should be loaded as soon as it's dependencies are satisfied.
        /// Otherwise you should explicitily load this module via the <see cref="ModuleManager"/>.
        public bool OnDemand { get; set; }
    }
}
