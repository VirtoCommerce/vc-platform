using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model
{
    public interface IHasSettings
    {
        ICollection<SettingEntry> Settings { get; }
    }
}
