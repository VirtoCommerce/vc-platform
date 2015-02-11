using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Framework.Web.Settings;
using VirtoCommerceCMS.Data.Repositories;
using VirtoCommerceCMS.ThemeModule.Web.Models;

namespace VirtoCommerceCMS.ThemeModule.Web.Controllers.Api
{
	[RoutePrefix("api/cms/theme")]
	public class ThemeController : ApiController
	{
		private IFileRepository _fileRepository;

		public ThemeController(Func<string, IFileRepository> factory, ISettingsManager manager)
		{
			if (factory == null)
				throw new ArgumentNullException("factory");

			if (manager == null)
				throw new ArgumentNullException("manager");

			var choosenRepository = manager.GetValue("VirtoCommerceCMS.ThemeModule.MainProperties.ThemesRepositoryType", string.Empty);

			var fileRepository = factory.Invoke(choosenRepository);
			_fileRepository = fileRepository;
		}

		[HttpGet]
		[ResponseType(typeof(ContentItem[]))]
		[Route("items")]
		public IHttpActionResult GetItems(string path)
		{
			var items = _fileRepository.GetContentItems(path);

			var retValItems = items.Select(i => i.ToWebModel());

			return Ok(retValItems.ToArray());
		}

		[HttpGet]
		[ResponseType(typeof(ContentItem))]
		[Route("item")]
		public IHttpActionResult GetItem(string path)
		{
			var item = _fileRepository.GetContentItem(path);
			return Ok(item.ToWebModel());
		}

		[HttpPost]
		[Route("save")]
		public IHttpActionResult SaveItem(ContentItem item)
		{
			var domainItem = item.ToDomainModel();
			_fileRepository.SaveContentItem(domainItem);
			return Ok();
		}

		[HttpDelete]
		[Route("delete")]
		public IHttpActionResult DeleteItem(ContentItem item)
		{
			var domainItem = item.ToDomainModel();
			_fileRepository.DeleteContentItem(domainItem);
			return Ok();
		}
	}
}
