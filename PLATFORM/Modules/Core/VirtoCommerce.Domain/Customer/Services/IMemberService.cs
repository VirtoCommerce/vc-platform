using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Customer.Services
{
    public interface IMemberService
    {
        Member TryCreateMember(string memberType);
        MembersSearchResult SearchMembers(MembersSearchCriteria criteria);
        Member[] GetByIds(string[] memberIds);
        void CreateOrUpdate(Member[] members);
        void Delete(string[] ids);
    }
}
