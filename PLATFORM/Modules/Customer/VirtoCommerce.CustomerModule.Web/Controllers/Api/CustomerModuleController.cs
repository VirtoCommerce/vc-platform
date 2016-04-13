using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CustomerModule.Web.Security;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Security;
using coreModel = VirtoCommerce.Domain.Customer.Model;

namespace VirtoCommerce.CustomerModule.Web.Controllers.Api
{
    [RoutePrefix("api")]
    [CheckPermission(Permission = CustomerPredefinedPermissions.Read)]
    public class CustomerModuleController : ApiController
    {
        private readonly IMemberService _memberService;
        private readonly IMemberSearchService _memberSearchService;
        private readonly ISecurityService _securityService;

        public CustomerModuleController(IMemberService memberService, IMemberSearchService memberSearchService, ISecurityService securityService)
        {
            _memberService = memberService;
            _securityService = securityService;
            _memberSearchService = memberSearchService;
        }

        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>Get array of all organizations.</remarks>
        [HttpGet]
        [Route("members/organizations")]
        [ResponseType(typeof(coreModel.Organization[]))]
        public IHttpActionResult ListOrganizations()
        {
            var searchCriteria = new coreModel.MembersSearchCriteria
            {
                MemberType = typeof(coreModel.Organization).Name,
                DeepSearch = true,
                Take = int.MaxValue
            };
            var result = _memberSearchService.SearchMembers(searchCriteria);

            return Ok(result.Members);
        }

        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>Get array of members satisfied search criteria.</remarks>
        /// <param name="criteria">concrete instance of SearchCriteria type type will be created by using PolymorphicMemberSearchCriteriaJsonConverter</param>
        [HttpPost]
        [Route("members/search")]
        [ResponseType(typeof(coreModel.MembersSearchResult))]
        public IHttpActionResult Search(coreModel.MembersSearchCriteria criteria)
        {
            var result = _memberSearchService.SearchMembers(criteria);

            return Ok(result);
        }

        /// <summary>
        /// Get member
        /// </summary>
        /// <param name="id">member id</param>
        [HttpGet]
        [Route("members/{id}")]
        [ResponseType(typeof(coreModel.Member))]
        public IHttpActionResult GetMemberById(string id)
        {
            var retVal = _memberService.GetByIds(new[] { id }).FirstOrDefault();
            if (retVal != null)
            {
                return Ok(retVal);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Create new member (can be any object inherited from Member type)
        /// </summary>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <returns></returns>
        [HttpPost]
        [Route("members")]
        [ResponseType(typeof(coreModel.Member))]
        [CheckPermission(Permission = CustomerPredefinedPermissions.Create)]
        public IHttpActionResult CreateMember([FromBody]coreModel.Member member)
        {
            _memberService.CreateOrUpdate(new[] { member });
            var retVal = _memberService.GetByIds(new[] { member.Id }).FirstOrDefault();
            return Ok(retVal);
        }

        /// <summary>
        /// Update member
        /// </summary>
        /// <param name="member">concrete instance of abstract member type will be created by using PolymorphicMemberJsonConverter</param>
        /// <response code="204">Operation completed.</response>
        [HttpPut]
        [Route("members")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = CustomerPredefinedPermissions.Update)]
        public IHttpActionResult UpdateMember(coreModel.Member member)
        {
            _memberService.CreateOrUpdate(new[] { member });
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Delete members
        /// </summary>
        /// <remarks>Delete members by given array of ids.</remarks>
        /// <param name="ids">An array of members ids</param>
        /// <response code="204">Operation completed.</response>
        [HttpDelete]
        [Route("members")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = CustomerPredefinedPermissions.Delete)]

        public IHttpActionResult DeleteMembers([FromUri] string[] ids)
        {
            _memberService.Delete(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }

        #region Special members for storefront C# API client  (because it not support polymorph types)

        /// <summary>
        /// Create contact
        /// </summary>
        [HttpPost]
        [Route("contacts")]
        [ResponseType(typeof(coreModel.Contact))]
        [CheckPermission(Permission = CustomerPredefinedPermissions.Create)]
        public IHttpActionResult CreateContact(coreModel.Contact contact)
        {
            return CreateMember(contact);
        }

        /// <summary>
        /// Update contact
        /// </summary>
        /// <response code="204">Operation completed.</response>
        [HttpPut]
        [Route("contacts")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = CustomerPredefinedPermissions.Update)]
        public IHttpActionResult UpdateContact(coreModel.Contact contact)
        {
            return UpdateMember(contact);
        }

        /// <summary>
        /// Create organization
        /// </summary>
        [HttpPost]
        [Route("organizations")]
        [ResponseType(typeof(coreModel.Organization))]
        [CheckPermission(Permission = CustomerPredefinedPermissions.Create)]
        public IHttpActionResult CreateOrganization(coreModel.Organization organization)
        {
            return CreateMember(organization);
        }

        /// <summary>
        /// Update organization
        /// </summary>
        /// <response code="204">Operation completed.</response>
        [HttpPut]
        [Route("organizations")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = CustomerPredefinedPermissions.Update)]
        public IHttpActionResult UpdateOrganization(coreModel.Organization organization)
        {
            return UpdateMember(organization);
        }

        /// <summary>
        /// Delete organizations
        /// </summary>
        /// <remarks>Delete organizations by given array of ids.</remarks>
        /// <param name="ids">An array of organizations ids</param>
        /// <response code="204">Operation completed.</response>
        [HttpDelete]
        [Route("organizations")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = CustomerPredefinedPermissions.Delete)]
        public IHttpActionResult DeleteOrganizations([FromUri] string[] ids)
        {
            return DeleteMembers(ids);
        }

        /// <summary>
        /// Delete contacts
        /// </summary>
        /// <remarks>Delete contacts by given array of ids.</remarks>
        /// <param name="ids">An array of contacts ids</param>
        /// <response code="204">Operation completed.</response>
        [HttpDelete]
        [Route("contacts")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = CustomerPredefinedPermissions.Delete)]
        public IHttpActionResult DeleteContacts([FromUri] string[] ids)
        {
            return DeleteMembers(ids);
        }



        /// <summary>
        /// Get organization
        /// </summary>
        /// <param name="id">Organization id</param>
        /// <response code="200"></response>
        /// <response code="404">Organization not found.</response>
        [HttpGet]
        [Route("organizations/{id}")]
        [ResponseType(typeof(coreModel.Organization))]
        public IHttpActionResult GetOrganizationById(string id)
        {
            return GetMemberById(id);
        }

        /// <summary>
        /// Get contact
        /// </summary>
        /// <param name="id">Contact ID</param>
        [HttpGet]
        [Route("contacts/{id}")]
        [ResponseType(typeof(coreModel.Contact))]
        public IHttpActionResult GetContactById(string id)
        {
            return GetMemberById(id);
        }

        #endregion
    }
}
