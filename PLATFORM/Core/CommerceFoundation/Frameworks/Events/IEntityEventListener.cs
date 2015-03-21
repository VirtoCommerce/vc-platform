using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks.Events
{
    public interface IEntityEventListener
    {
        void EntityBeforeSaved(object sender, EntityEventArgs e);
        void EntityAfterSaved(object sender, EntityEventArgs e);
    }
}
