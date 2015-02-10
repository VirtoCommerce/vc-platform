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
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.CustomerModule.Web.Converters;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using webModel = VirtoCommerce.CustomerModule.Web.Model;

namespace VirtoCommerce.CustomerModule.Web.Controllers.Api
{
	[RoutePrefix("api/contacts")]
	public class ContactController : ApiController
	{
		private readonly IContactService _contactService;
		private readonly IContactSearchService _contactSearchService;
		public ContactController(IContactService contactService, IContactSearchService contactSearchService)
		{
			_contactSearchService = contactSearchService;
			_contactService = contactService;
		}

		// GET: api/contacts?q=ddd&site=site1&start=0&count=20
		[HttpGet]
		[ResponseType(typeof(webModel.SearchResult))]
		[Route("")]
		public IHttpActionResult Search([ModelBinder(typeof(SearchCriteriaBinder))] webModel.SearchCriteria criteria)
		{
			var retVal = _contactSearchService.Search(criteria.ToCoreModel());
			return Ok(retVal.ToWebModel());
		}


		// GET: api/contacts/{id}
		[HttpGet]
		[ResponseType(typeof(webModel.Contact))]
		[Route("{id}")]
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
		[Route("")]
		public IHttpActionResult Create(webModel.Contact contact)
		{
			var retVal = _contactService.Create(contact.ToCoreModel());
			return Ok(retVal.ToWebModel());
		}

		// PUT: api/contacts
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("")]
		public IHttpActionResult Update(webModel.Contact contact)
		{
			_contactService.Update(new coreModel.Contact[] { contact.ToCoreModel() });
			return StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: api/contacts?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("")]
		public IHttpActionResult Delete([FromUri] string[] ids)
		{
			_contactService.Delete(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}
	}
}
