using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications
{
    public enum CommunicationItemState
    {
        NotModified, //item has no any modifications yet. 
        Appended,    //item was added. 
        Modified,    //item was edited.
        Deleted      //item mark to delete.
    }
}
