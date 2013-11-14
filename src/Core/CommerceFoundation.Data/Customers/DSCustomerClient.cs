using System;
using System.Linq;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Customers;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Services;

namespace VirtoCommerce.Foundation.Data.Customers
{
    public class DSCustomerClient : DSClientBase, ICustomerRepository
    {
        [InjectionConstructor]
        public DSCustomerClient(ICustomerEntityFactory entityFactory, ISecurityTokenInjector tokenInjector, IServiceConnectionFactory connFactory)
            : base(connFactory.GetConnectionString(CustomerConfiguration.Instance.Connection.DataServiceUri), entityFactory, tokenInjector)
        {

        }

        public DSCustomerClient(Uri serviceUri, ICustomerEntityFactory entityfactory, ISecurityTokenInjector tokenInjector)
            : base(serviceUri, entityfactory, tokenInjector)
        {

        }

        #region ICustomerRepository Members

        public IQueryable<Case> Cases
        {
            get { return GetAsQueryable<Case>(); }
        }

        public IQueryable<CaseTemplate> CaseTemplates
        {
            get { return GetAsQueryable<CaseTemplate>(); }
        }

        public IQueryable<CaseRule> CaseRules
        {
            get { return GetAsQueryable<CaseRule>(); }
        }

        public IQueryable<CaseAlert> CaseAlerts
        {
            get { return GetAsQueryable<CaseAlert>(); }
        }

        public IQueryable<CaseCC> CaseCcs
        {
            get { return GetAsQueryable<CaseCC>(); }
        }

        public IQueryable<CasePropertySet> CasePropertySets
        {
            get { return GetAsQueryable<CasePropertySet>(); }
        }

        public IQueryable<CasePropertyValue> CasePropertyValues
        {
            get { return GetAsQueryable<CasePropertyValue>(); }
        }

        public IQueryable<ContactPropertyValue> ContactPropertyValues
        {
            get { return GetAsQueryable<ContactPropertyValue>(); }
        }

        public IQueryable<Address> Addresses
        {
            get { return GetAsQueryable<Address>(); }
        }

        public IQueryable<Organization> Organizations
        {
            get { return GetAsQueryable<Organization>(); }
        }

        public IQueryable<Email> Emails
        {
            get { return GetAsQueryable<Email>(); }
        }

        public IQueryable<Label> Labels
        {
            get { return GetAsQueryable<Label>(); }
        }

        public IQueryable<Note> Notes
        {
            get { return GetAsQueryable<Note>(); }
        }

        public IQueryable<Phone> Phones
        {
            get { return GetAsQueryable<Phone>(); }
        }

        public IQueryable<KnowledgeBaseArticle> KnowledgeBaseArticles
        {
            get { return GetAsQueryable<KnowledgeBaseArticle>(); }
        }

        public IQueryable<KnowledgeBaseGroup> KnowledgeBaseGroups
        {
            get { return GetAsQueryable<KnowledgeBaseGroup>(); }
        }

        public IQueryable<Member> Members
        {
            get { return GetAsQueryable<Member>(); }
        }

        public IQueryable<CommunicationItem> CommunicationItems
        {
            get { return GetAsQueryable<CommunicationItem>(); }
        }

        public IQueryable<Attachment> Attachments
        {
            get { return GetAsQueryable<Attachment>(); }
        }

        public IQueryable<MemberRelation> MemberRelations
        {
            get { return GetAsQueryable<MemberRelation>(); }
        }
        #endregion


       
    }
}
