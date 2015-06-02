using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Platform.Tests.Bases;

namespace VirtoCommerce.Content.Tests
{
    public class ContentScenarios : FunctionalTestBase, IDisposable
    {
        public void Can_query_content_lists()
        {
            /*
            var lists = _menuService.GetListsByStoreId(storeId);
            Assert.True(queryTime < new TimeSpan(0, 0, 1, 2));
             * */
        }

        protected IContentRepository GetContentRepository()
        {
            EnsureDatabaseInitialized(() => new DatabaseContentRepositoryImpl(), () => Database.SetInitializer(new SqlContentDatabaseInitializer()));
            return new DatabaseContentRepositoryImpl();
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                // Ensure LocalDb databases are deleted after use so that LocalDb doesn't throw if
                // the temp location in which they are stored is later cleaned.
                using (var context = new DatabaseContentRepositoryImpl())
                {
                    context.Database.Delete();
                }
            }
            finally
            {
                //AppDomain.CurrentDomain.SetData("DataDirectory", _previousDataDirectory);
            }
        }

        #endregion
    }
}
