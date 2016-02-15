using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Notification : Drop
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }
    }
}