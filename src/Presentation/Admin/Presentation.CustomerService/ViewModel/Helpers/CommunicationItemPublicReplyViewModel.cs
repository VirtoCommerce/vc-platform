using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Helpers
{
    public class CommunicationItemPublicReplyViewModel:CommunicationItemViewModel
    {
        public CommunicationItemPublicReplyViewModel()
            : base()
        {
            this.Type = CommunicationItemType.PublicReply;
        }

    }
}
