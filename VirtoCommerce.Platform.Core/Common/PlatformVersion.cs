using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class PlatformVersion
    {
        public static SemanticVersion CurrentVersion
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();

                if (!Version.TryParse(FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion, out var version))
                {
                    throw new FormatException("version");
                }
                return new SemanticVersion(version);
            }
        }
    }
}
