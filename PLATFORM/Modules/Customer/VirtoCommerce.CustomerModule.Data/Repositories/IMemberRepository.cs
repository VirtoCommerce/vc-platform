using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerModule.Data.Repositories
{
    public interface IMemberRepository : IRepository
    {
        IQueryable<MemberDataEntity> Members { get; }
        IQueryable<AddressDataEntity> Addresses { get; }
        IQueryable<EmailDataEntity> Emails { get; }
        IQueryable<NoteDataEntity> Notes { get; }
        IQueryable<PhoneDataEntity> Phones { get; }
        IQueryable<MemberRelationDataEntity> MemberRelations { get; }


        MemberDataEntity[] GetMembersByIds(string[] ids, string[] memberTypes = null);
        void RemoveMembersByIds(string[] ids, string[] memberTypes = null);
    }
}
