using System;
using System.Linq;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.Customers;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;

namespace VirtoCommerce.Client
{
    public class UserClient
    {
        #region Cache Constants
        public const string CustomerCacheKey = "C:C:{0}";
        #endregion

        #region Private Variables
        private readonly bool _isEnabled;
        private readonly ISecurityRepository _securityRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICacheRepository _cacheRepository;
        private readonly ICustomerSessionService _customerSession;
        #endregion

        public UserClient(ISecurityRepository securityRepository, ICustomerRepository customerRepository, ICustomerSessionService customerSession, ICacheRepository cacheRepository)
        {
            _securityRepository = securityRepository;
            _customerRepository = customerRepository;
            _cacheRepository = cacheRepository;
            _customerSession = customerSession;
            _isEnabled = CustomerConfiguration.Instance.Cache.IsEnabled;
        }

        #region Account Management
        public virtual Contact NewContact(string memberId = "")
        {
            var contact = new Contact { MemberId = memberId };

            if (String.IsNullOrEmpty(memberId))
            {
                var session = _customerSession.CustomerSession;

                if (session != null && !String.IsNullOrEmpty(session.CustomerId))
                {
                    contact.MemberId = session.CustomerId;
                }
            }

            if (String.IsNullOrEmpty(contact.MemberId))
            {
                throw new ArgumentNullException("memberId");
            }

            _customerRepository.Add(contact);

            return contact;
        }

		/// <summary>
		/// Creates the contact.
		/// </summary>
		/// <param name="contact">The contact.</param>
        public virtual void CreateContact(Contact contact)
        {
            _customerRepository.Add(contact);
            SaveCustomerChanges();
        }

		/// <summary>
		/// Creates the account.
		/// </summary>
		/// <param name="account">The account.</param>
		public virtual void CreateAccount(Account account)
		{
			_securityRepository.Add(account);
		    SaveSecurityChanges();
		}

        /// <summary>
        /// Saves the changes made to security repository.
        /// </summary>
        /// <returns></returns>
        public virtual int SaveSecurityChanges()
        {
            return _securityRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Saves the changes made to customer repository.
        /// </summary>
        /// <returns></returns>
        public virtual int SaveCustomerChanges(string memberId = null)
        {
            //Remove from cache
            if (!string.IsNullOrEmpty(memberId))
            {
                Helper.Remove(string.Format(CustomerCacheKey, memberId));
            }
            return _customerRepository.UnitOfWork.Commit();
        }

        public virtual void MarkUpdate(StorageEntity entity)
        {
            _customerRepository.Update(entity);
        }

        /// <summary>
        /// Determines whether the specified user name is authorized.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="account">The account.</param>
        /// <param name="types">The types.</param>
        /// <returns>
        ///   <c>true</c> if the specified user name is authorized; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAuthorized(string userName, out Account account, params RegisterType[] types)
        {
            var isAuthorized = true;

            var usr = GetAccountByUserName(userName, types);

            if (usr == null)
            {
                isAuthorized = false;
            }
            else // check if user is approved and can access the store
            {
                // check state
                if (usr.AccountState != AccountState.Approved.GetHashCode())
                {
                    isAuthorized = false;
                }
            }

            account = null;
            if (isAuthorized)
            {
                account = usr;
            }

            return isAuthorized;
        }

        public Account GetAccountByUserName(string userName, params RegisterType[] types)
        {
            var typesInt = new int[] { };
            if (types != null)
            {
                typesInt = (from t in types select t.GetHashCode()).ToArray();
            }

            var account = _securityRepository.Accounts.FirstOrDefault(x => x.UserName == userName && (typesInt.Count() == 0 || typesInt.Contains(x.RegisterType)));

            return account;
        }

        public Role[] GetAllMemberRoles(string memberId)
        {
            return _securityRepository.Accounts
                .Where(x => x.MemberId == memberId)
                .SelectMany(x => x.RoleAssignments)
                .Select(x => x.Role)
                .Distinct()
                .ToArray();
        }

        public Role[] GetAllRoles()
        {
            return _securityRepository.Roles.ToArray();
        }

        #endregion

        #region Customer Management
        public Contact GetCurrentCustomer(bool useCache = true)
        {
            var session = _customerSession.CustomerSession;

            if (session != null && !String.IsNullOrEmpty(session.CustomerId))
            {
                return GetCustomer(session.CustomerId, useCache);
            }

            return null;
        }

        public Contact GetCustomer(string memberId, bool useCache = true)
        {
            return Helper.Get(
                CacheHelper.CreateCacheKey(Constants.UserCachePrefix, string.Format(CustomerCacheKey, memberId)),
                () => LoadCustomer(memberId),
                CustomerConfiguration.Instance.Cache.CustomerTimeout,
                _isEnabled && useCache);
        }

        private Contact LoadCustomer(string customerId)
        {
            var contact = _customerRepository.Members.OfType<Contact>()
                .Where(x => x.MemberId == customerId)
                .Expand(x => x.Emails)
                .Expand(x => x.Addresses)
                .Expand(x => x.Contracts)
				.Expand(x => x.ContactPropertyValues)
                .FirstOrDefault();
            return contact;
        }

        /// <summary>
        /// Gets the user address.
        /// </summary>
        /// <param name="addressId">The address id.</param>
        /// <returns></returns>
        public Address GetUserAddress(string addressId)
        {
            var contact = GetCurrentCustomer();
            if (contact == null)
                return null;

            return contact.Addresses != null ? contact.Addresses.FirstOrDefault(addr => addr.AddressId.Equals(addressId, StringComparison.OrdinalIgnoreCase)) : null;
        }

        public Address[] GetUserAddresses()
        {
            var contact = GetCurrentCustomer();
            if (contact == null)
                return null;

            return contact.Addresses != null ? contact.Addresses.ToArray() : null;
        }
        #endregion

        #region Organization Methods
        public Organization GetOrganizationById(string memberId)
        {
            return _customerRepository.Members.OfType<Organization>().FirstOrDefault(x => x.MemberId == memberId);
        }

        public Address GetCompanyAddress(string addressId, string memberId)
        {
            var org = GetOrganizationById(memberId);
            return org == null ? null : org.Addresses.FirstOrDefault(addr => addr.AddressId.Equals(addressId, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the organizations for current user.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Organization> GetOrganizationsForCurrentUser()
        {
            var u = GetCurrentCustomer();

            if (u == null)
            {
                return null;
            }

            var repo = _customerRepository;

            return (from m in repo.Members
                    join r in repo.MemberRelations on m.MemberId equals r.AncestorId
                    where r.DescendantId == u.MemberId
                    select m).OfType<Organization>();
        }

        /// <summary>
        /// Gets the organization for current user.
        /// </summary>
        /// <returns></returns>
        public Organization GetOrganizationForCurrentUser()
        {
            var orgs = GetOrganizationsForCurrentUser();

            return orgs != null ? orgs.SingleOrDefault() : null;
        }

        #endregion

        CacheHelper _cacheHelper;
        public CacheHelper Helper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }
    }

}
