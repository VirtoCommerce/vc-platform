using System.Linq;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Customers.Repositories
{
    public interface ICustomerRepository : IRepository
    {
        IQueryable<Member> Members { get; }

        IQueryable<Case> Cases { get; }
        IQueryable<CaseTemplate> CaseTemplates { get; }
        IQueryable<CaseRule> CaseRules { get; }
		IQueryable<CaseAlert> CaseAlerts { get; }
        IQueryable<CasePropertySet> CasePropertySets { get; }
        IQueryable<CasePropertyValue> CasePropertyValues { get; }
        IQueryable<ContactPropertyValue> ContactPropertyValues { get; }
        IQueryable<CaseCC> CaseCcs { get; }
        IQueryable<Address> Addresses { get; }
        IQueryable<CommunicationItem> CommunicationItems { get; }
        IQueryable<Attachment> Attachments { get; }

        IQueryable<Organization> Organizations { get; }
        IQueryable<Email> Emails { get; }
        IQueryable<Label> Labels { get; }
        IQueryable<Note> Notes { get; }
        IQueryable<Phone> Phones { get; }
        IQueryable<KnowledgeBaseArticle> KnowledgeBaseArticles { get; }
        IQueryable<KnowledgeBaseGroup> KnowledgeBaseGroups { get; }

        IQueryable<MemberRelation> MemberRelations { get; }
    }
}
