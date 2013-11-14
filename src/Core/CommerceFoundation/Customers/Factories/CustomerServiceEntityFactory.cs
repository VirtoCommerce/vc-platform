using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Customers.Factories
{
    public class CustomerEntityFactory : FactoryBase, ICustomerEntityFactory
    {
        public CustomerEntityFactory()
        {
            RegisterStorageType(typeof(Address), "Address");
            RegisterStorageType(typeof(Attachment), "Attachment");
            RegisterStorageType(typeof(Case), "Case");
            RegisterStorageType(typeof(CaseTemplate), "CaseTemplate");
            RegisterStorageType(typeof(CaseTemplateProperty), "CaseTemplateProperty");
            RegisterStorageType(typeof(CaseRule), "CaseRule");
            RegisterStorageType(typeof(CaseAlert), "CaseAlert");
            RegisterStorageType(typeof(CaseProperty), "CaseProperty");
            RegisterStorageType(typeof(CasePropertyValue), "CasePropertyValue");
            RegisterStorageType(typeof(CaseCC),"CaseCC");
            RegisterStorageType(typeof(Contact), "Contact");
            RegisterStorageType(typeof(Member), "Member");
            RegisterStorageType(typeof(Email), "Email");
            RegisterStorageType(typeof(Label), "Label");
            RegisterStorageType(typeof(Note), "Note");
            RegisterStorageType(typeof(Phone), "Phone");
            RegisterStorageType(typeof(CommunicationItem), "CommunicatonItem");
            RegisterStorageType(typeof(EmailItem), "EmailItem");
            RegisterStorageType(typeof(PhoneCallItem), "PhoneCallItem");
            RegisterStorageType(typeof(PublicReplyItem),"PublicReplyItem");
            RegisterStorageType(typeof(Organization), "Organization");
            RegisterStorageType(typeof(KnowledgeBaseArticle), "KnowledgeBaseArticle");
            RegisterStorageType(typeof(KnowledgeBaseGroup), "KnowledgeBaseGroup");
            RegisterStorageType(typeof(Contract), "Contract");
            RegisterStorageType(typeof(MemberRelation), "MemberRelation");
        }
    }
}
