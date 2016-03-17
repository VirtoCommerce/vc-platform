using System;
using System.ComponentModel;
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
        private readonly ISecurityService _securityService;

        public CustomerModuleController(IMemberService memberService, ISecurityService securityService)
        {
            _memberService = memberService;
            _securityService = securityService;
        }

        /// <summary>
        /// Get organizations
        /// </summary>
        /// <remarks>Get array of all organizations.</remarks>
        [HttpGet]
        [ResponseType(typeof(coreModel.Organization[]))]
        [Route("organizations")]
        public IHttpActionResult ListOrganizations()
        {
            var searchCriteria = new coreModel.SearchCriteria
            {
                MemberType = typeof(coreModel.Organization).Name,
                DeepSearch = true,
                Take = int.MaxValue
            };
            var result = _memberService.SearchMembers(searchCriteria);

            return Ok(result.Members);
        }

        /// <summary>
        /// Get members
        /// </summary>
        /// <remarks>Get array of members satisfied search criteria.</remarks>
        /// <param name="criteria">Search criteria</param>
        [HttpPost]
        [ResponseType(typeof(coreModel.SearchResult))]
        [Route("members/search")]
        public IHttpActionResult Search(coreModel.SearchCriteria criteria)
        {
            var result = _memberService.SearchMembers(criteria);

            return Ok(result);
        }

        /// <summary>
        /// Get member
        /// </summary>
        /// <param name="id">member id</param>
        [HttpGet]
        [ResponseType(typeof(coreModel.Member))]
        [Route("members/{id}")]
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
        /// Create member
        /// </summary>
        [HttpPost]
        [ResponseType(typeof(coreModel.Member))]
        [Route("members")]
        [CheckPermission(Permission = CustomerPredefinedPermissions.Create)]
        public IHttpActionResult CreateMember(coreModel.Member member)
        {
            _memberService.CreateOrUpdate(new [] { member });
            var retVal = _memberService.GetByIds(new[] { member.Id }).FirstOrDefault();
            return Ok(retVal);
        }

        /// <summary>
        /// Update member
        /// </summary>
        /// <response code="204">Operation completed.</response>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("members")]
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
        [ResponseType(typeof(void))]
        [Route("members")]
        [CheckPermission(Permission = CustomerPredefinedPermissions.Delete)]
       
        public IHttpActionResult DeleteMembers([FromUri] string[] ids)
        {
            _memberService.Delete(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }



        /// <summary>
        /// Create contact
        /// </summary>
        [HttpPost]
        [ResponseType(typeof(coreModel.Contact))]
        [Route("contacts")]
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
        [ResponseType(typeof(void))]
        [Route("contacts")]
        [CheckPermission(Permission = CustomerPredefinedPermissions.Update)]
        public IHttpActionResult UpdateContact(coreModel.Contact contact)
        {
            return UpdateMember(contact);
        }

    

        /// <summary>
        /// Create organization
        /// </summary>
        [HttpPost]
        [ResponseType(typeof(coreModel.Organization))]
        [Route("organizations")]
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
        [ResponseType(typeof(void))]
        [Route("organizations")]
        [CheckPermission(Permission = CustomerPredefinedPermissions.Update)]
        public IHttpActionResult UpdateOrganization(coreModel.Organization organization)
        {
            return UpdateMember(organization);
        }



        #region Obsolete members

        /// <summary>
        /// Delete organizations
        /// </summary>
        /// <remarks>Delete organizations by given array of ids.</remarks>
        /// <param name="ids">An array of organizations ids</param>
        /// <response code="204">Operation completed.</response>
        [Obsolete("Use DeleteMembers instead")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("organizations")]
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
        [Obsolete("Use DeleteMembers instead")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("contacts")]
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
        [Obsolete("Use GetMemberById  instead")]
        [HttpGet]
        [ResponseType(typeof(coreModel.Organization))]
        [Route("organizations/{id}")]
        public IHttpActionResult GetOrganizationById(string id)
        {
            return GetMemberById(id);
        }

        /// <summary>
        /// Get contact
        /// </summary>
        /// <param name="id">Contact ID</param>
        [Obsolete("Use GetMemberById instead")]
        [HttpGet]
        [ResponseType(typeof(coreModel.Contact))]
        [Route("contacts/{id}")]
        public IHttpActionResult GetContactById(string id)
        {
            return GetMemberById(id);
        }



        #endregion
    }
}
