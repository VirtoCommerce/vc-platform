using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace VirtoCommerce.Client.Globalization
{
    /// <summary>
    /// Class LocalizeHelper.
    /// </summary>
    public class LocalizeHelper
    {
        #region Fields
        /// <summary>
        /// The string pattern
        /// </summary>
        public const string StringPattern = @"([^""]|\\""|"""")*";
        /// <summary>
        /// The localize pattern
        /// </summary>
        public static string LocalizePattern = String.Format(@"(?<![""\\])""(?<s>{0})"".Localize\(\)", StringPattern);
        /// <summary>
        /// The error message pattern
        /// </summary>
        public static string ErrorMessagePattern = String.Format(@"ErrorMessage\s*=\s*""(?<s>{0})""", StringPattern);
        /// <summary>
        /// The display name pattern
        /// </summary>
        public static string DisplayNamePattern = String.Format(@"DisplayName(""(?<s>{0})"")", StringPattern);
        /// <summary>
        /// The description pattern
        /// </summary>
        public static string DescriptionPattern = String.Format(@"Description(""(?<s>{0})"")", StringPattern); 
        #endregion

        #region Methods

        /// <summary>
        /// Generates the resource.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <param name="patterns">The patterns.</param>
        public static void GenerateResource(string dir, params string[] patterns)
        {
            foreach (var file in GetFiles(dir, "*.cs", "*.ascx", "*.aspx", "*.cshtml"))
            {
                var content = File.ReadAllText(file);
                foreach (var pattern in patterns)
                {
                    foreach (Match match in Regex.Matches(content, pattern))
                    {
                        match.Groups["s"].Value.Localize();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <param name="extensions">The extensions.</param>
        /// <returns>IEnumerable{System.String}.</returns>
        private static IEnumerable<string> GetFiles(string dir, params string[] extensions)
        {
            var list = new List<string>();
            foreach (var each in extensions)
            {
                list.AddRange(Directory.GetFiles(dir, each, SearchOption.AllDirectories));
            }
            return list;
        } 
        #endregion
    }
}
