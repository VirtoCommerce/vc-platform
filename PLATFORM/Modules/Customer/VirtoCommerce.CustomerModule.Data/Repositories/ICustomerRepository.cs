﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerModule.Data.Repositories
{
    public interface ICustomerRepository : IRepository
    {
        IQueryable<Member> Members { get; }
        IQueryable<Address> Addresses { get; }
        IQueryable<Organization> Organizations { get; }
        IQueryable<Contact> Contacts { get; }
        IQueryable<Email> Emails { get; }
        IQueryable<Note> Notes { get; }
        IQueryable<Phone> Phones { get; }
        IQueryable<MemberRelation> MemberRelations { get; }

        Member[] GetMembersByIds(string[] ids);
    }
}
