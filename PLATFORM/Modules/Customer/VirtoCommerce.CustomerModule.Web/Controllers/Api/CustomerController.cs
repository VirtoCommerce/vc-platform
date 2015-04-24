using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CustomerModule.Web.Binders;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.CustomerModule.Web.Converters;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using webModel = VirtoCommerce.CustomerModule.Web.Model;

namespace VirtoCommerce.CustomerModule.Web.Controllers.Api
{
	[RoutePrefix("api")]
	public class CustomerController : ApiController
	{
		private readonly IContactService _contactService;
		private readonly IOrganizationService _organizationService;
		private readonly ICustomerSearchService _contactSearchService;
		public CustomerController(IContactService contactService, IOrganizationService organizationService, ICustomerSearchService contactSearchService)
		{
			_contactSearchService = contactSearchService;
			_organizationService = organizationService;
			_contactService = contactService;
		}

		// GET: api/organizations
		[HttpGet]
		[ResponseType(typeof(webModel.Organization[]))]
		[Route("organizations")]
		public IHttpActionResult ListOrganizations()
		{
			var retVal = _organizationService.List().ToArray();
			return Ok(retVal);
		}

		// GET: api/members?q=ddd&organization=org1&start=0&count=20
		[HttpGet]
		[ResponseType(typeof(webModel.SearchResult))]
		[Route("members")]
		public IHttpActionResult Search([ModelBinder(typeof(SearchCriteriaBinder))] webModel.SearchCriteria criteria)
		{
			var result = _contactSearchService.Search(criteria.ToCoreModel());
	
			var retVal = new webModel.SearchResult();

			var start = criteria.Start;
			var count = criteria.Count;

			// all organizations
			var organizations = result.Organizations.Select(x => x.ToWebModel());
			var contacts = result.Contacts.Select(x => x.ToWebModel());

			retVal.TotalCount = organizations.Count() + result.TotalCount;
			retVal.Members.AddRange(organizations.Skip(start).Take(count));

			count -= organizations.Count();

			retVal.Members.AddRange(contacts.Take(count));

			return Ok(retVal);
		}


		// GET: api/contacts/{id}
		[HttpGet]
		[ResponseType(typeof(webModel.Contact))]
		[Route("contacts/{id}")]
		public IHttpActionResult GetContactById(string id)
		{
			var retVal = _contactService.GetById(id);
			if(retVal == null)
			{
				return NotFound();
			}
			return Ok(retVal.ToWebModel());
		}


		// POST: api/contacts
		[HttpPost]
		[ResponseType(typeof(webModel.Contact))]
		[Route("contacts")]
		public IHttpActionResult CreateContact(webModel.Contact contact)
		{
			var retVal = _contactService.Create(contact.ToCoreModel());
			return Ok(retVal.ToWebModel());
		}

		// PUT: api/contacts
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("contacts")]
		public IHttpActionResult UpdateContact(webModel.Contact contact)
		{
			_contactService.Update(new coreModel.Contact[] { contact.ToCoreModel() });
			return StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: api/contacts?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("contacts")]
		public IHttpActionResult DeleteContacts([FromUri] string[] ids)
		{
			_contactService.Delete(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}


		// GET: api/organizations/{id}
		[HttpGet]
		[ResponseType(typeof(webModel.Organization))]
		[Route("organizations/{id}")]
		public IHttpActionResult GetOrganizationById(string id)
		{
			var retVal =  _organizationService.GetById(id);
			if (retVal == null)
			{
				return NotFound();
			}
			return Ok(retVal.ToWebModel());
		}


		// POST: api/organizations
		[HttpPost]
		[ResponseType(typeof(webModel.Organization))]
		[Route("organizations")]
		public IHttpActionResult CreateOrganization(webModel.Organization organization)
		{
			var retVal = _organizationService.Create(organization.ToCoreModel());
			return Ok(retVal.ToWebModel());
		}

		// PUT: api/organizations
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("organizations")]
		public IHttpActionResult UpdateOrganization(webModel.Organization organization)
		{
			_organizationService.Update(new coreModel.Organization[] { organization.ToCoreModel() });
			return StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: api/organizations?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("organizations")]
		public IHttpActionResult DeleteOrganizations([FromUri] string[] ids)
		{
			_organizationService.Delete(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}
	}
}
