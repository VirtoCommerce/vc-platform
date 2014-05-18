using VirtoCommerce.Client.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Core
{
    public static class RegionNames
    {
        /// <summary>
        /// Main application region
        /// </summary>
        public const string MainRegion = "MainRegion";

        /// <summary>
        /// Main application menu
        /// </summary>
        public const string MenuRegion = "MenuRegion";

        /// <summary>
        /// Wizard steps region
        /// </summary>
        public const string WizardStepsRegion = "WizardStepsRegion";


        public const string CaptionRegion = "CaptionRegion";

        public static string Localize(this string source)
        {
            return source.Localize(null, LocalizationScope.DefaultCategory);
        }
    }
}
