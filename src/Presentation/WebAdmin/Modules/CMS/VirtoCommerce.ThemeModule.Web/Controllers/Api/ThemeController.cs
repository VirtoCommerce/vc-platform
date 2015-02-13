namespace VirtoCommerce.ThemeModule.Web.Controllers.Api
{
    #region

    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;

    using VirtoCommerce.Content.Data.Repositories;
    using VirtoCommerce.Framework.Web.Settings;
    using VirtoCommerce.ThemeModule.Web.Converters;
    using VirtoCommerce.ThemeModule.Web.Models;

    #endregion

    [RoutePrefix("api/cms/theme")]
    public class ThemeController : ApiController
    {
        #region Fields

        private readonly IFileRepository _fileRepository;

        #endregion

        #region Constructors and Destructors

        public ThemeController(Func<string, IFileRepository> factory, ISettingsManager manager)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            var choosenRepository = manager.GetValue(
                "VirtoCommerce.ThemeModule.MainProperties.ThemesRepositoryType",
                string.Empty);

            var fileRepository = factory.Invoke(choosenRepository);
            this._fileRepository = fileRepository;
        }

        #endregion

        #region Public Methods and Operators

        [HttpDelete]
        [Route("delete")]
        public IHttpActionResult DeleteItem(ContentItem item)
        {
            var domainItem = item.ToDomainModel();
            this._fileRepository.DeleteContentItem(domainItem);
            return this.Ok();
        }

        [HttpGet]
        [ResponseType(typeof(ContentItem))]
        [Route("item")]
        public IHttpActionResult GetItem(string path)
        {
            var item = this._fileRepository.GetContentItem(path);
            return this.Ok(item.ToWebModel());
        }

        [HttpGet]
        [ResponseType(typeof(ContentItem[]))]
        [Route("items")]
        public IHttpActionResult GetItems(string path)
        {
            var items = this._fileRepository.GetContentItems(path);

            var retValItems = items.Select(i => i.ToWebModel());

            return this.Ok(retValItems.ToArray());
        }

        [HttpPost]
        [Route("save")]
        public IHttpActionResult SaveItem(ContentItem item)
        {
            var domainItem = item.ToDomainModel();
            this._fileRepository.SaveContentItem(domainItem);
            return this.Ok();
        }

        #endregion
    }
}