using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Customer;

namespace VirtoCommerce.Storefront.Model.Order.Events
{
    /// <summary>
    /// Event generated when user logined to storefront
    /// </summary>
    public class UserLoginEvent
    {
        public UserLoginEvent(WorkContext workContext, CustomerInfo prevUser, CustomerInfo newUser)
        {
            WorkContext = workContext;
            PrevUser = prevUser;
            NewUser = newUser;
        }

        public WorkContext WorkContext { get; set; }
        public CustomerInfo PrevUser { get; set; }
        public CustomerInfo NewUser { get; set; }
    }
}
