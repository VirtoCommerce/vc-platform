using System;
using System.Linq;
using FunctionalTests.Catalogs.Helpers;
using FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using Xunit;

namespace FunctionalTests.Catalogs
{
	[Variant(RepositoryProvider.EntityFramework)]
	[Variant(RepositoryProvider.DataService)]
	public class ItemScenarios : CatalogTestBase, IDisposable
	{
		#region Tests

		[RepositoryTheory]
		public void Can_create_associationGroup_with_association()
		{
			var repository = GetRepository();
			var items = new Item[] { };
			CreateFullGraphCatalog(repository, ref items, "testcatalog");

			//Refresh
			RefreshRepository(ref repository);
			var itemId = items[0].ItemId;
			var innerItem = repository.Items.Expand(x => x.AssociationGroups).Where(x => x.ItemId == itemId).Single();

			// create AssociationGroup
			var ag = new AssociationGroup()
			{
				Name = "name"
			};

			innerItem.AssociationGroups.Add(ag);

			// create Association
			itemId = items[1].ItemId;
			var a = new Association
				{
					AssociationType = "optional",
					//CatalogItem = repository.Items.Where(x => x.ItemId == itemId).Single() //USING THIS WORKS ONLY ON EF directly
					ItemId = itemId
				};
			ag.Associations.Add(a);

			// save
			repository.UnitOfWork.Commit();

			//RefreshClient
			RefreshRepository(ref repository);

			innerItem = repository.Items.Expand("AssociationGroups/Associations").Where(x => x.ItemId == innerItem.ItemId).Single();
			ag = innerItem.AssociationGroups.Single(x => x.AssociationGroupId == ag.AssociationGroupId);

			Assert.NotNull(innerItem);
			Assert.True(innerItem.AssociationGroups.Any());
			Assert.True(innerItem.AssociationGroups.Any(x => x.AssociationGroupId == ag.AssociationGroupId));
			Assert.True(ag.Associations.Any());
			Assert.True(ag.Associations.Any(x => x.AssociationId == a.AssociationId));
		}

		[RepositoryTheory]
		public void Can_delete_itemasset()
		{
			var repository = GetRepository();
			var items = new Item[] { };
			CreateFullGraphCatalog(repository, ref items, "testcatalog");

			//RefreshClient
			RefreshRepository(ref repository);

			var InnerItem = repository.Items
				.Expand(x => x.ItemAssets)
				.Where(x => x.ItemAssets.Any()).FirstOrDefault();

			var initialCount = InnerItem.ItemAssets.Count;
			var removingItem = InnerItem.ItemAssets[0];
			//repository.Remove(removingItem); // uncomment to pass
			InnerItem.ItemAssets.Remove(removingItem);

			// save
			repository.UnitOfWork.Commit();

			InnerItem = repository.Items
				.Expand(x => x.ItemAssets)
				.Where(x => x.ItemId == InnerItem.ItemId).Single();
			Assert.Equal(initialCount - 1, InnerItem.ItemAssets.Count);
		}

		#endregion
	}
}
