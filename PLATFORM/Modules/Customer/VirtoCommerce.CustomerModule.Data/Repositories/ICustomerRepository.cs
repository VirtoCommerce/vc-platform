using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerModule.Data.Repositories
{
    public interface ICustomerRepository : IMemberRepository
    {
        IQueryable<OrganizationDataEntity> Organizations { get; }
        IQueryable<ContactDataEntity> Contacts { get; }
        IQueryable<VendorDataEntity> Vendors { get; }
        IQueryable<EmployeeDataEntity> Employees { get; }
    
    }
}
