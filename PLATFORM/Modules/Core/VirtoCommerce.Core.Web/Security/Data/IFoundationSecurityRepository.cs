using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;

namespace VirtoCommerce.CoreModule.Web.Security
{
    public interface IFoundationSecurityRepository : ISecurityRepository
    {
         Account GetAccountByName(string userName);
    }
}
