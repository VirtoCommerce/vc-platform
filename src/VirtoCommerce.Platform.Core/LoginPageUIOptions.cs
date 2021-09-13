using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core
{
    public class LoginPageUIOptions
    {
        public string BackgroundUrl { get; set; }

        public string PatternUrl { get; set; }

        public string Preset { get; set; }

        public List<LoginPageUIOptionPreset> Presets { get; set; } = new List<LoginPageUIOptionPreset>();

        public class LoginPageUIOptionPreset
        {
            public string Name { get; set; }

            public string BackgroundUrl { get; set; }

            public string PatternUrl { get; set; }
        }
    }
}
