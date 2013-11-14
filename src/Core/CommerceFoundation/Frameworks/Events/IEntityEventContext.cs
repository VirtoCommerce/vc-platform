using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks.Events
{
    public interface IEntityEventContext
    {
        void RaiseBeforeEvent(object sender, EntityEventArgs args);
        void RaiseAfterEvent(object sender, EntityEventArgs args);
        void Subscribe(IEntityEventListener listener);
    }
}
