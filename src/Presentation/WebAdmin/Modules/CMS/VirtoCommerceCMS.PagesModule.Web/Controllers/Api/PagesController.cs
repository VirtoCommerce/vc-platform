namespace VirtoCommerce.PagesModule.Web.Controllers.Api
{
    #region

    using System.Web.Http;

    using VirtoCommerce.Content.Data.Repositories;

    #endregion

    [RoutePrefix("api/cms/pages")]
    public class PagesController : ApiController
    {
        #region Fields

        private IFileRepository _fileRepository;

        #endregion

        #region Constructors and Destructors

        public PagesController(IFileRepository fileRepository)
        {
            this._fileRepository = fileRepository;
        }

        #endregion

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