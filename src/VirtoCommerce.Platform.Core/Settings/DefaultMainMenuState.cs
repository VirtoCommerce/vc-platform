using System;
using System.Collections.Generic;
using System.Text;

namespace VirtoCommerce.Platform.Core.Settings
{
    /// <summary>
    /// Main menu settings model
    /// </summary>
    public class ItemDefaultMainMenuState
    {
        public string Path { get; set; }
        public bool IsFavorite { get; set; }
        public int Order { get; set; }
    }

    public class DefaultMainMenuState
    {
        public List<ItemDefaultMainMenuState> Items { get; set; }
    }
}
