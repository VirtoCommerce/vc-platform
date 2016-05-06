using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RestSharp;
using VirtoCommerce.Client.Client;
using VirtoCommerce.Client.Model;

namespace VirtoCommerce.Client.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ICustomerManagementModuleApi
    {
        #region Synchronous Operations
        /// <summary>
        /// Create contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns>VirtoCommerceDomainCustomerModelContact</returns>
        VirtoCommerceDomainCustomerModelContact CustomerModuleCreateContact (VirtoCommerceDomainCustomerModelContact contact);

        /// <summary>
        /// Create contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelContact</returns>
        ApiResponse<VirtoCommerceDomainCustomerModelContact> CustomerModuleCreateContactWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact);
        /// <summary>
        /// Create new member (can be any object inherited from Member type)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns>VirtoCommerceDomainCustomerModelMember</returns>
        VirtoCommerceDomainCustomerModelMember CustomerModuleCreateMember (VirtoCommerceDomainCustomerModelMember member);

        /// <summary>
        /// Create new member (can be any object inherited from Member type)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelMember</returns>
        ApiResponse<VirtoCommerceDomainCustomerModelMember> CustomerModuleCreateMemberWithHttpInfo (VirtoCommerceDomainCustomerModelMember member);
        /// <summary>
        /// Create organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns>VirtoCommerceDomainCustomerModelOrganization</returns>
        VirtoCommerceDomainCustomerModelOrganization CustomerModuleCreateOrganization (VirtoCommerceDomainCustomerModelOrganization organization);

        /// <summary>
        /// Create organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelOrganization</returns>
        ApiResponse<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleCreateOrganizationWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization);
        /// <summary>
        /// Delete contacts
        /// </summary>
        /// <remarks>
        /// Delete contacts by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of contacts ids</param>
        /// <returns></returns>
        void CustomerModuleDeleteContacts (List<string> ids);

        /// <summary>
        /// Delete contacts
        /// </summary>
        /// <remarks>
        /// Delete contacts by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of contacts ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CustomerModuleDeleteContactsWithHttpInfo (List<string> ids);
        /// <summary>
        /// Delete members
        /// </summary>
        /// <remarks>
        /// Delete members by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of members ids</param>
        /// <returns></returns>
        void CustomerModuleDeleteMembers (List<string> ids);

        /// <summary>
        /// Delete members
        /// </summary>
        /// <remarks>
        /// Delete members by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of members ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CustomerModuleDeleteMembersWithHttpInfo (List<string> ids);
        /// <summary>
        /// Delete organizations
        /// </summary>
        /// <remarks>
        /// Delete organizations by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of organizations ids</param>
        /// <returns></returns>
        void CustomerModuleDeleteOrganizations (List<string> ids);

        /// <summary>
        /// Delete organizations
        /// </summary>
        /// <remarks>
        /// Delete organizations by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of organizations ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CustomerModuleDeleteOrganizationsWithHttpInfo (List<string> ids);
        /// <summary>
        /// Get contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Contact ID</param>
        /// <returns>VirtoCommerceDomainCustomerModelContact</returns>
        VirtoCommerceDomainCustomerModelContact CustomerModuleGetContactById (string id);

        /// <summary>
        /// Get contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Contact ID</param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelContact</returns>
        ApiResponse<VirtoCommerceDomainCustomerModelContact> CustomerModuleGetContactByIdWithHttpInfo (string id);
        /// <summary>
        /// Get member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">member id</param>
        /// <returns>VirtoCommerceDomainCustomerModelMember</returns>
        VirtoCommerceDomainCustomerModelMember CustomerModuleGetMemberById (string id);

        /// <summary>
        /// Get member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">member id</param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelMember</returns>
        ApiResponse<VirtoCommerceDomainCustomerModelMember> CustomerModuleGetMemberByIdWithHttpInfo (string id);
        /// <summary>
        /// Get organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Organization id</param>
        /// <returns>VirtoCommerceDomainCustomerModelOrganization</returns>
        VirtoCommerceDomainCustomerModelOrganization CustomerModuleGetOrganizationById (string id);

        /// <summary>
        /// Get organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Organization id</param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelOrganization</returns>
        ApiResponse<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleGetOrganizationByIdWithHttpInfo (string id);
        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>
        /// Get array of all organizations.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;</returns>
        List<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleListOrganizations ();

        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>
        /// Get array of all organizations.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;</returns>
        ApiResponse<List<VirtoCommerceDomainCustomerModelOrganization>> CustomerModuleListOrganizationsWithHttpInfo ();
        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>
        /// Get array of members satisfied search criteria.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">concrete instance of SearchCriteria type type will be created by using PolymorphicMemberSearchCriteriaJsonConverter</param>
        /// <returns>VirtoCommerceDomainCustomerModelMembersSearchResult</returns>
        VirtoCommerceDomainCustomerModelMembersSearchResult CustomerModuleSearch (VirtoCommerceDomainCustomerModelMembersSearchCriteria criteria);

        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>
        /// Get array of members satisfied search criteria.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">concrete instance of SearchCriteria type type will be created by using PolymorphicMemberSearchCriteriaJsonConverter</param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelMembersSearchResult</returns>
        ApiResponse<VirtoCommerceDomainCustomerModelMembersSearchResult> CustomerModuleSearchWithHttpInfo (VirtoCommerceDomainCustomerModelMembersSearchCriteria criteria);
        /// <summary>
        /// Update contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns></returns>
        void CustomerModuleUpdateContact (VirtoCommerceDomainCustomerModelContact contact);

        /// <summary>
        /// Update contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CustomerModuleUpdateContactWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact);
        /// <summary>
        /// Update member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns></returns>
        void CustomerModuleUpdateMember (VirtoCommerceDomainCustomerModelMember member);

        /// <summary>
        /// Update member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CustomerModuleUpdateMemberWithHttpInfo (VirtoCommerceDomainCustomerModelMember member);
        /// <summary>
        /// Update organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns></returns>
        void CustomerModuleUpdateOrganization (VirtoCommerceDomainCustomerModelOrganization organization);

        /// <summary>
        /// Update organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CustomerModuleUpdateOrganizationWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Create contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelContact</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelContact> CustomerModuleCreateContactAsync (VirtoCommerceDomainCustomerModelContact contact);

        /// <summary>
        /// Create contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelContact)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelContact>> CustomerModuleCreateContactAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact);
        /// <summary>
        /// Create new member (can be any object inherited from Member type)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelMember</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelMember> CustomerModuleCreateMemberAsync (VirtoCommerceDomainCustomerModelMember member);

        /// <summary>
        /// Create new member (can be any object inherited from Member type)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelMember)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelMember>> CustomerModuleCreateMemberAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelMember member);
        /// <summary>
        /// Create organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelOrganization</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleCreateOrganizationAsync (VirtoCommerceDomainCustomerModelOrganization organization);

        /// <summary>
        /// Create organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelOrganization)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelOrganization>> CustomerModuleCreateOrganizationAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization);
        /// <summary>
        /// Delete contacts
        /// </summary>
        /// <remarks>
        /// Delete contacts by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of contacts ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CustomerModuleDeleteContactsAsync (List<string> ids);

        /// <summary>
        /// Delete contacts
        /// </summary>
        /// <remarks>
        /// Delete contacts by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of contacts ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleDeleteContactsAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Delete members
        /// </summary>
        /// <remarks>
        /// Delete members by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of members ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CustomerModuleDeleteMembersAsync (List<string> ids);

        /// <summary>
        /// Delete members
        /// </summary>
        /// <remarks>
        /// Delete members by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of members ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleDeleteMembersAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Delete organizations
        /// </summary>
        /// <remarks>
        /// Delete organizations by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of organizations ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CustomerModuleDeleteOrganizationsAsync (List<string> ids);

        /// <summary>
        /// Delete organizations
        /// </summary>
        /// <remarks>
        /// Delete organizations by given array of ids.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of organizations ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleDeleteOrganizationsAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Get contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Contact ID</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelContact</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelContact> CustomerModuleGetContactByIdAsync (string id);

        /// <summary>
        /// Get contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Contact ID</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelContact)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelContact>> CustomerModuleGetContactByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">member id</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelMember</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelMember> CustomerModuleGetMemberByIdAsync (string id);

        /// <summary>
        /// Get member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">member id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelMember)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelMember>> CustomerModuleGetMemberByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Organization id</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelOrganization</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleGetOrganizationByIdAsync (string id);

        /// <summary>
        /// Get organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Organization id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelOrganization)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelOrganization>> CustomerModuleGetOrganizationByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>
        /// Get array of all organizations.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceDomainCustomerModelOrganization>> CustomerModuleListOrganizationsAsync ();

        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>
        /// Get array of all organizations.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainCustomerModelOrganization>>> CustomerModuleListOrganizationsAsyncWithHttpInfo ();
        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>
        /// Get array of members satisfied search criteria.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">concrete instance of SearchCriteria type type will be created by using PolymorphicMemberSearchCriteriaJsonConverter</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelMembersSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelMembersSearchResult> CustomerModuleSearchAsync (VirtoCommerceDomainCustomerModelMembersSearchCriteria criteria);

        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>
        /// Get array of members satisfied search criteria.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">concrete instance of SearchCriteria type type will be created by using PolymorphicMemberSearchCriteriaJsonConverter</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelMembersSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelMembersSearchResult>> CustomerModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelMembersSearchCriteria criteria);
        /// <summary>
        /// Update contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CustomerModuleUpdateContactAsync (VirtoCommerceDomainCustomerModelContact contact);

        /// <summary>
        /// Update contact
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateContactAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact);
        /// <summary>
        /// Update member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CustomerModuleUpdateMemberAsync (VirtoCommerceDomainCustomerModelMember member);

        /// <summary>
        /// Update member
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateMemberAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelMember member);
        /// <summary>
        /// Update organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CustomerModuleUpdateOrganizationAsync (VirtoCommerceDomainCustomerModelOrganization organization);

        /// <summary>
        /// Update organization
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateOrganizationAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization);
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CustomerManagementModuleApi : ICustomerManagementModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerManagementModuleApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public CustomerManagementModuleApi(Configuration configuration)
        {
            if (configuration == null) // use the default one in Configuration
                this.Configuration = Configuration.Default;
            else
                this.Configuration = configuration;

            // ensure API client has configuration ready
            if (Configuration.ApiClient.Configuration == null)
            {
                this.Configuration.ApiClient.Configuration = this.Configuration;
            }
        }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public String GetBasePath()
        {
            return this.Configuration.ApiClient.RestClient.BaseUrl.ToString();
        }

        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        [Obsolete("SetBasePath is deprecated, please do 'Configuraiton.ApiClient = new ApiClient(\"http://new-path\")' instead.")]
        public void SetBasePath(String basePath)
        {
            // do nothing
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public Configuration Configuration {get; set;}

        /// <summary>
        /// Gets the default header.
        /// </summary>
        /// <returns>Dictionary of HTTP header</returns>
        [Obsolete("DefaultHeader is deprecated, please use Configuration.DefaultHeader instead.")]
        public Dictionary<String, String> DefaultHeader()
        {
            return this.Configuration.DefaultHeader;
        }

        /// <summary>
        /// Add default header.
        /// </summary>
        /// <param name="key">Header field name.</param>
        /// <param name="value">Header field value.</param>
        /// <returns></returns>
        [Obsolete("AddDefaultHeader is deprecated, please use Configuration.AddDefaultHeader instead.")]
        public void AddDefaultHeader(string key, string value)
        {
            this.Configuration.AddDefaultHeader(key, value);
        }

        /// <summary>
        /// Create contact 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns>VirtoCommerceDomainCustomerModelContact</returns>
        public VirtoCommerceDomainCustomerModelContact CustomerModuleCreateContact (VirtoCommerceDomainCustomerModelContact contact)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelContact> localVarResponse = CustomerModuleCreateContactWithHttpInfo(contact);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create contact 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelContact</returns>
        public ApiResponse< VirtoCommerceDomainCustomerModelContact > CustomerModuleCreateContactWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact)
        {
            // verify the required parameter 'contact' is set
            if (contact == null)
                throw new ApiException(400, "Missing required parameter 'contact' when calling CustomerManagementModuleApi->CustomerModuleCreateContact");

            var localVarPath = "/api/contacts";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (contact.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(contact); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contact; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleCreateContact: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleCreateContact: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelContact>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelContact) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainCustomerModelContact)));
            
        }

        /// <summary>
        /// Create contact 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelContact</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelContact> CustomerModuleCreateContactAsync (VirtoCommerceDomainCustomerModelContact contact)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelContact> localVarResponse = await CustomerModuleCreateContactAsyncWithHttpInfo(contact);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create contact 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelContact)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelContact>> CustomerModuleCreateContactAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact)
        {
            // verify the required parameter 'contact' is set
            if (contact == null)
                throw new ApiException(400, "Missing required parameter 'contact' when calling CustomerManagementModuleApi->CustomerModuleCreateContact");

            var localVarPath = "/api/contacts";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (contact.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(contact); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contact; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleCreateContact: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleCreateContact: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelContact>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelContact) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainCustomerModelContact)));
            
        }

        /// <summary>
        /// Create new member (can be any object inherited from Member type) 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns>VirtoCommerceDomainCustomerModelMember</returns>
        public VirtoCommerceDomainCustomerModelMember CustomerModuleCreateMember (VirtoCommerceDomainCustomerModelMember member)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelMember> localVarResponse = CustomerModuleCreateMemberWithHttpInfo(member);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create new member (can be any object inherited from Member type) 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelMember</returns>
        public ApiResponse< VirtoCommerceDomainCustomerModelMember > CustomerModuleCreateMemberWithHttpInfo (VirtoCommerceDomainCustomerModelMember member)
        {
            // verify the required parameter 'member' is set
            if (member == null)
                throw new ApiException(400, "Missing required parameter 'member' when calling CustomerManagementModuleApi->CustomerModuleCreateMember");

            var localVarPath = "/api/members";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (member.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(member); // http body (model) parameter
            }
            else
            {
                localVarPostBody = member; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleCreateMember: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleCreateMember: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelMember>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelMember) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainCustomerModelMember)));
            
        }

        /// <summary>
        /// Create new member (can be any object inherited from Member type) 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelMember</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelMember> CustomerModuleCreateMemberAsync (VirtoCommerceDomainCustomerModelMember member)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelMember> localVarResponse = await CustomerModuleCreateMemberAsyncWithHttpInfo(member);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create new member (can be any object inherited from Member type) 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelMember)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelMember>> CustomerModuleCreateMemberAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelMember member)
        {
            // verify the required parameter 'member' is set
            if (member == null)
                throw new ApiException(400, "Missing required parameter 'member' when calling CustomerManagementModuleApi->CustomerModuleCreateMember");

            var localVarPath = "/api/members";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (member.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(member); // http body (model) parameter
            }
            else
            {
                localVarPostBody = member; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleCreateMember: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleCreateMember: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelMember>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelMember) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainCustomerModelMember)));
            
        }

        /// <summary>
        /// Create organization 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns>VirtoCommerceDomainCustomerModelOrganization</returns>
        public VirtoCommerceDomainCustomerModelOrganization CustomerModuleCreateOrganization (VirtoCommerceDomainCustomerModelOrganization organization)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelOrganization> localVarResponse = CustomerModuleCreateOrganizationWithHttpInfo(organization);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create organization 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelOrganization</returns>
        public ApiResponse< VirtoCommerceDomainCustomerModelOrganization > CustomerModuleCreateOrganizationWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization)
        {
            // verify the required parameter 'organization' is set
            if (organization == null)
                throw new ApiException(400, "Missing required parameter 'organization' when calling CustomerManagementModuleApi->CustomerModuleCreateOrganization");

            var localVarPath = "/api/organizations";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (organization.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(organization); // http body (model) parameter
            }
            else
            {
                localVarPostBody = organization; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleCreateOrganization: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleCreateOrganization: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelOrganization>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelOrganization) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainCustomerModelOrganization)));
            
        }

        /// <summary>
        /// Create organization 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelOrganization</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleCreateOrganizationAsync (VirtoCommerceDomainCustomerModelOrganization organization)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelOrganization> localVarResponse = await CustomerModuleCreateOrganizationAsyncWithHttpInfo(organization);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create organization 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelOrganization)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelOrganization>> CustomerModuleCreateOrganizationAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization)
        {
            // verify the required parameter 'organization' is set
            if (organization == null)
                throw new ApiException(400, "Missing required parameter 'organization' when calling CustomerManagementModuleApi->CustomerModuleCreateOrganization");

            var localVarPath = "/api/organizations";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (organization.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(organization); // http body (model) parameter
            }
            else
            {
                localVarPostBody = organization; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleCreateOrganization: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleCreateOrganization: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelOrganization>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelOrganization) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainCustomerModelOrganization)));
            
        }

        /// <summary>
        /// Delete contacts Delete contacts by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of contacts ids</param>
        /// <returns></returns>
        public void CustomerModuleDeleteContacts (List<string> ids)
        {
             CustomerModuleDeleteContactsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete contacts Delete contacts by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of contacts ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CustomerModuleDeleteContactsWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerManagementModuleApi->CustomerModuleDeleteContacts");

            var localVarPath = "/api/contacts";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleDeleteContacts: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleDeleteContacts: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete contacts Delete contacts by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of contacts ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CustomerModuleDeleteContactsAsync (List<string> ids)
        {
             await CustomerModuleDeleteContactsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete contacts Delete contacts by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of contacts ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleDeleteContactsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerManagementModuleApi->CustomerModuleDeleteContacts");

            var localVarPath = "/api/contacts";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleDeleteContacts: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleDeleteContacts: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete members Delete members by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of members ids</param>
        /// <returns></returns>
        public void CustomerModuleDeleteMembers (List<string> ids)
        {
             CustomerModuleDeleteMembersWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete members Delete members by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of members ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CustomerModuleDeleteMembersWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerManagementModuleApi->CustomerModuleDeleteMembers");

            var localVarPath = "/api/members";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleDeleteMembers: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleDeleteMembers: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete members Delete members by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of members ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CustomerModuleDeleteMembersAsync (List<string> ids)
        {
             await CustomerModuleDeleteMembersAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete members Delete members by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of members ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleDeleteMembersAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerManagementModuleApi->CustomerModuleDeleteMembers");

            var localVarPath = "/api/members";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleDeleteMembers: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleDeleteMembers: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete organizations Delete organizations by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of organizations ids</param>
        /// <returns></returns>
        public void CustomerModuleDeleteOrganizations (List<string> ids)
        {
             CustomerModuleDeleteOrganizationsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete organizations Delete organizations by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of organizations ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CustomerModuleDeleteOrganizationsWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerManagementModuleApi->CustomerModuleDeleteOrganizations");

            var localVarPath = "/api/organizations";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleDeleteOrganizations: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleDeleteOrganizations: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete organizations Delete organizations by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of organizations ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CustomerModuleDeleteOrganizationsAsync (List<string> ids)
        {
             await CustomerModuleDeleteOrganizationsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete organizations Delete organizations by given array of ids.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">An array of organizations ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleDeleteOrganizationsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CustomerManagementModuleApi->CustomerModuleDeleteOrganizations");

            var localVarPath = "/api/organizations";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleDeleteOrganizations: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleDeleteOrganizations: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Get contact 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Contact ID</param>
        /// <returns>VirtoCommerceDomainCustomerModelContact</returns>
        public VirtoCommerceDomainCustomerModelContact CustomerModuleGetContactById (string id)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelContact> localVarResponse = CustomerModuleGetContactByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get contact 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Contact ID</param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelContact</returns>
        public ApiResponse< VirtoCommerceDomainCustomerModelContact > CustomerModuleGetContactByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CustomerManagementModuleApi->CustomerModuleGetContactById");

            var localVarPath = "/api/contacts/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleGetContactById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleGetContactById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelContact>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelContact) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainCustomerModelContact)));
            
        }

        /// <summary>
        /// Get contact 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Contact ID</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelContact</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelContact> CustomerModuleGetContactByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelContact> localVarResponse = await CustomerModuleGetContactByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get contact 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Contact ID</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelContact)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelContact>> CustomerModuleGetContactByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CustomerManagementModuleApi->CustomerModuleGetContactById");

            var localVarPath = "/api/contacts/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleGetContactById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleGetContactById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelContact>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelContact) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainCustomerModelContact)));
            
        }

        /// <summary>
        /// Get member 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">member id</param>
        /// <returns>VirtoCommerceDomainCustomerModelMember</returns>
        public VirtoCommerceDomainCustomerModelMember CustomerModuleGetMemberById (string id)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelMember> localVarResponse = CustomerModuleGetMemberByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get member 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">member id</param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelMember</returns>
        public ApiResponse< VirtoCommerceDomainCustomerModelMember > CustomerModuleGetMemberByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CustomerManagementModuleApi->CustomerModuleGetMemberById");

            var localVarPath = "/api/members/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleGetMemberById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleGetMemberById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelMember>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelMember) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainCustomerModelMember)));
            
        }

        /// <summary>
        /// Get member 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">member id</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelMember</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelMember> CustomerModuleGetMemberByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelMember> localVarResponse = await CustomerModuleGetMemberByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get member 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">member id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelMember)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelMember>> CustomerModuleGetMemberByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CustomerManagementModuleApi->CustomerModuleGetMemberById");

            var localVarPath = "/api/members/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleGetMemberById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleGetMemberById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelMember>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelMember) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainCustomerModelMember)));
            
        }

        /// <summary>
        /// Get organization 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Organization id</param>
        /// <returns>VirtoCommerceDomainCustomerModelOrganization</returns>
        public VirtoCommerceDomainCustomerModelOrganization CustomerModuleGetOrganizationById (string id)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelOrganization> localVarResponse = CustomerModuleGetOrganizationByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get organization 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Organization id</param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelOrganization</returns>
        public ApiResponse< VirtoCommerceDomainCustomerModelOrganization > CustomerModuleGetOrganizationByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CustomerManagementModuleApi->CustomerModuleGetOrganizationById");

            var localVarPath = "/api/organizations/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleGetOrganizationById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleGetOrganizationById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelOrganization>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelOrganization) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainCustomerModelOrganization)));
            
        }

        /// <summary>
        /// Get organization 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Organization id</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelOrganization</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleGetOrganizationByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelOrganization> localVarResponse = await CustomerModuleGetOrganizationByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get organization 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Organization id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelOrganization)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelOrganization>> CustomerModuleGetOrganizationByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CustomerManagementModuleApi->CustomerModuleGetOrganizationById");

            var localVarPath = "/api/organizations/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleGetOrganizationById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleGetOrganizationById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelOrganization>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelOrganization) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainCustomerModelOrganization)));
            
        }

        /// <summary>
        /// Get organizations Get array of all organizations.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;</returns>
        public List<VirtoCommerceDomainCustomerModelOrganization> CustomerModuleListOrganizations ()
        {
             ApiResponse<List<VirtoCommerceDomainCustomerModelOrganization>> localVarResponse = CustomerModuleListOrganizationsWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get organizations Get array of all organizations.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;</returns>
        public ApiResponse< List<VirtoCommerceDomainCustomerModelOrganization> > CustomerModuleListOrganizationsWithHttpInfo ()
        {

            var localVarPath = "/api/members/organizations";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleListOrganizations: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleListOrganizations: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceDomainCustomerModelOrganization>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainCustomerModelOrganization>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceDomainCustomerModelOrganization>)));
            
        }

        /// <summary>
        /// Get organizations Get array of all organizations.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceDomainCustomerModelOrganization>> CustomerModuleListOrganizationsAsync ()
        {
             ApiResponse<List<VirtoCommerceDomainCustomerModelOrganization>> localVarResponse = await CustomerModuleListOrganizationsAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get organizations Get array of all organizations.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceDomainCustomerModelOrganization&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceDomainCustomerModelOrganization>>> CustomerModuleListOrganizationsAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/members/organizations";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleListOrganizations: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleListOrganizations: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceDomainCustomerModelOrganization>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceDomainCustomerModelOrganization>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceDomainCustomerModelOrganization>)));
            
        }

        /// <summary>
        /// Get members Get array of members satisfied search criteria.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">concrete instance of SearchCriteria type type will be created by using PolymorphicMemberSearchCriteriaJsonConverter</param>
        /// <returns>VirtoCommerceDomainCustomerModelMembersSearchResult</returns>
        public VirtoCommerceDomainCustomerModelMembersSearchResult CustomerModuleSearch (VirtoCommerceDomainCustomerModelMembersSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelMembersSearchResult> localVarResponse = CustomerModuleSearchWithHttpInfo(criteria);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get members Get array of members satisfied search criteria.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">concrete instance of SearchCriteria type type will be created by using PolymorphicMemberSearchCriteriaJsonConverter</param>
        /// <returns>ApiResponse of VirtoCommerceDomainCustomerModelMembersSearchResult</returns>
        public ApiResponse< VirtoCommerceDomainCustomerModelMembersSearchResult > CustomerModuleSearchWithHttpInfo (VirtoCommerceDomainCustomerModelMembersSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling CustomerManagementModuleApi->CustomerModuleSearch");

            var localVarPath = "/api/members/search";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelMembersSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelMembersSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainCustomerModelMembersSearchResult)));
            
        }

        /// <summary>
        /// Get members Get array of members satisfied search criteria.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">concrete instance of SearchCriteria type type will be created by using PolymorphicMemberSearchCriteriaJsonConverter</param>
        /// <returns>Task of VirtoCommerceDomainCustomerModelMembersSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainCustomerModelMembersSearchResult> CustomerModuleSearchAsync (VirtoCommerceDomainCustomerModelMembersSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceDomainCustomerModelMembersSearchResult> localVarResponse = await CustomerModuleSearchAsyncWithHttpInfo(criteria);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get members Get array of members satisfied search criteria.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">concrete instance of SearchCriteria type type will be created by using PolymorphicMemberSearchCriteriaJsonConverter</param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainCustomerModelMembersSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainCustomerModelMembersSearchResult>> CustomerModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelMembersSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling CustomerManagementModuleApi->CustomerModuleSearch");

            var localVarPath = "/api/members/search";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainCustomerModelMembersSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainCustomerModelMembersSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceDomainCustomerModelMembersSearchResult)));
            
        }

        /// <summary>
        /// Update contact 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns></returns>
        public void CustomerModuleUpdateContact (VirtoCommerceDomainCustomerModelContact contact)
        {
             CustomerModuleUpdateContactWithHttpInfo(contact);
        }

        /// <summary>
        /// Update contact 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CustomerModuleUpdateContactWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact)
        {
            // verify the required parameter 'contact' is set
            if (contact == null)
                throw new ApiException(400, "Missing required parameter 'contact' when calling CustomerManagementModuleApi->CustomerModuleUpdateContact");

            var localVarPath = "/api/contacts";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (contact.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(contact); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contact; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleUpdateContact: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleUpdateContact: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update contact 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CustomerModuleUpdateContactAsync (VirtoCommerceDomainCustomerModelContact contact)
        {
             await CustomerModuleUpdateContactAsyncWithHttpInfo(contact);

        }

        /// <summary>
        /// Update contact 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contact"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateContactAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelContact contact)
        {
            // verify the required parameter 'contact' is set
            if (contact == null)
                throw new ApiException(400, "Missing required parameter 'contact' when calling CustomerManagementModuleApi->CustomerModuleUpdateContact");

            var localVarPath = "/api/contacts";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (contact.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(contact); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contact; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleUpdateContact: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleUpdateContact: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update member 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns></returns>
        public void CustomerModuleUpdateMember (VirtoCommerceDomainCustomerModelMember member)
        {
             CustomerModuleUpdateMemberWithHttpInfo(member);
        }

        /// <summary>
        /// Update member 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CustomerModuleUpdateMemberWithHttpInfo (VirtoCommerceDomainCustomerModelMember member)
        {
            // verify the required parameter 'member' is set
            if (member == null)
                throw new ApiException(400, "Missing required parameter 'member' when calling CustomerManagementModuleApi->CustomerModuleUpdateMember");

            var localVarPath = "/api/members";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (member.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(member); // http body (model) parameter
            }
            else
            {
                localVarPostBody = member; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleUpdateMember: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleUpdateMember: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update member 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CustomerModuleUpdateMemberAsync (VirtoCommerceDomainCustomerModelMember member)
        {
             await CustomerModuleUpdateMemberAsyncWithHttpInfo(member);

        }

        /// <summary>
        /// Update member 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateMemberAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelMember member)
        {
            // verify the required parameter 'member' is set
            if (member == null)
                throw new ApiException(400, "Missing required parameter 'member' when calling CustomerManagementModuleApi->CustomerModuleUpdateMember");

            var localVarPath = "/api/members";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (member.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(member); // http body (model) parameter
            }
            else
            {
                localVarPostBody = member; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleUpdateMember: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleUpdateMember: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update organization 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns></returns>
        public void CustomerModuleUpdateOrganization (VirtoCommerceDomainCustomerModelOrganization organization)
        {
             CustomerModuleUpdateOrganizationWithHttpInfo(organization);
        }

        /// <summary>
        /// Update organization 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CustomerModuleUpdateOrganizationWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization)
        {
            // verify the required parameter 'organization' is set
            if (organization == null)
                throw new ApiException(400, "Missing required parameter 'organization' when calling CustomerManagementModuleApi->CustomerModuleUpdateOrganization");

            var localVarPath = "/api/organizations";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (organization.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(organization); // http body (model) parameter
            }
            else
            {
                localVarPostBody = organization; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleUpdateOrganization: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleUpdateOrganization: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update organization 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CustomerModuleUpdateOrganizationAsync (VirtoCommerceDomainCustomerModelOrganization organization)
        {
             await CustomerModuleUpdateOrganizationAsyncWithHttpInfo(organization);

        }

        /// <summary>
        /// Update organization 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="organization"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CustomerModuleUpdateOrganizationAsyncWithHttpInfo (VirtoCommerceDomainCustomerModelOrganization organization)
        {
            // verify the required parameter 'organization' is set
            if (organization == null)
                throw new ApiException(400, "Missing required parameter 'organization' when calling CustomerManagementModuleApi->CustomerModuleUpdateOrganization");

            var localVarPath = "/api/organizations";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (organization.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(organization); // http body (model) parameter
            }
            else
            {
                localVarPostBody = organization; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleUpdateOrganization: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CustomerModuleUpdateOrganization: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

    }
}
