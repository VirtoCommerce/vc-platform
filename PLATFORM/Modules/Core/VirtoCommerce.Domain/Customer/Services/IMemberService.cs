using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Customer.Services
{
    /// <summary>
    /// Abstraction for member CRUD operations
    /// </summary>
    public interface IMemberService
    {
        Member[] GetByIds(string[] memberIds, string[] memberTypes = null);
        void CreateOrUpdate(Member[] members);
        void Delete(string[] ids, string[] memberTypes = null);
    }
}
