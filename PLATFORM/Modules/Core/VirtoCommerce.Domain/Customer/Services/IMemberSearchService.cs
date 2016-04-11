using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Model;

namespace VirtoCommerce.Domain.Customer.Services
{
    public interface IMemberSearchService
    {
        MembersSearchResult SearchMembers(MembersSearchCriteria criteria);
    }
}
