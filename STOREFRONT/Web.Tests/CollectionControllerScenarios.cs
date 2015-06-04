#region
using System;
using VirtoCommerce.Web.Controllers;
using Xunit;

#endregion

namespace Web.Tests
{
    public class CollectionControllerScenarios
    {
        #region Public Methods and Operators
        [Fact]
        public void Can_retrieve_all_collections()
        {
            var controller = new CollectionsController();
            var allCollections = controller.AllAsync(String.Empty);
        }
        #endregion
    }
}