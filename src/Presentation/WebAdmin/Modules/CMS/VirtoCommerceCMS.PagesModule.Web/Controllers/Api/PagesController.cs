using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerceCMS.Data.Models;
using VirtoCommerceCMS.Data.Repositories;

namespace VirtoCommerceCMS.PagesModule.Web.Controllers.Api
{
	[RoutePrefix("api/cms/pages")]
	public class PagesController : ApiController
	{
		private IFileRepository _fileRepository;

		public PagesController(IFileRepository fileRepository)
		{
			_fileRepository = fileRepository;
		}

		//[HttpGet]
		//[ResponseType(typeof(ContentItem[]))]
		//[Route("items")]
		//public IHttpActionResult GetItems(string path)
		//{
		//	var items = _fileRepository.GetContentItems(path);
		//	return Ok(items);
		//}

		//[HttpPost]
		//[Route("save")]
		//public IHttpActionResult SaveItem(string message, string path)
		//{
		//	_fileRepository.SaveContentItem(message, path);
		//	return Ok();
		//}

		//[HttpDelete]
		//[Route("delete")]
		//public IHttpActionResult SaveItem(string path)
		//{
		//	_fileRepository.DeleteContentItem(path);
		//	return Ok();
		//}
	}
}
