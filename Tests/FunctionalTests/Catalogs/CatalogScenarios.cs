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
	public class CatalogScenarios : CatalogTestBase, IDisposable
	{
		[RepositoryTheory]
		public void Can_remove_add_propertyset_PropertySetProperty()
		{
			var repository = GetRepository();
			var items = new Item[] { };
			var catalogId = "testcatalog";

			CreateFullGraphCatalog(repository, ref items, catalogId);

			//Refresh
			//RefreshRepository(ref repository);
			var innerItem = repository.Catalogs.OfType<Catalog>()
				.Expand("PropertySets/PropertySetProperties/Property/PropertyValues")
				.Where(x => x.CatalogId == catalogId).Single();

			var CurrentCatalogProperties = repository.Properties.Expand("PropertyValues").Where(x => x.CatalogId == catalogId).ToList();

			var initialCount = innerItem.PropertySets[0].PropertySetProperties.Count;

			// remove Property form propertySet
			var removePropertySetProperty = innerItem.PropertySets[0].PropertySetProperties.First(x => x.PropertyId == CurrentCatalogProperties[0].PropertyId);
			innerItem.PropertySets[0].PropertySetProperties.Remove(removePropertySetProperty);
			//repository.Remove(removePropertySetProperty); // SHOULD THIS LINE BE REDUNDANT?

			// save
			try
			{
				repository.UnitOfWork.Commit();
			}
			catch (Exception ex)
			{
				throw ex;
			}


			var property0 = new Property() { Name = "test property", CatalogId = catalogId };
			repository.Add(property0);
			repository.UnitOfWork.Commit();
			//Refresh
			RefreshRepository(ref repository);
			innerItem = repository.Catalogs.OfType<Catalog>()
				.Expand("PropertySets/PropertySetProperties/Property/PropertyValues")
				.Where(x => x.CatalogId == catalogId).Single();

			Assert.Equal(initialCount - 1, innerItem.PropertySets[0].PropertySetProperties.Count);

			// add Property to propertySet
			innerItem.PropertySets[0].PropertySetProperties.Add(new PropertySetProperty() { PropertyId = property0.PropertyId });

			// save
			repository.UnitOfWork.Commit();
			//Refresh
			RefreshRepository(ref repository);
			innerItem = repository.Catalogs.OfType<Catalog>()
				.Expand("PropertySets/PropertySetProperties/Property/PropertyValues")
				.Where(x => x.CatalogId == catalogId).Single();

			Assert.Equal(initialCount + 0, innerItem.PropertySets[0].PropertySetProperties.Count);
		}

		[RepositoryTheory]
		public void Can_delete_category_with_cascade()
		{
			var repository = GetRepository();

			var items = new Item[] { };
			const string catalogId = "testCatalog";

			CreateFullGraphCatalog(repository, ref items, catalogId);

			var allCategories = repository.Categories
									.Where(x => x.CatalogId == catalogId && x.ParentCategoryId == null)
									.ToList();
			var innerItem = allCategories.FirstOrDefault();

			// create a sub category
			var subcategory = new Category
				{
					Code = "a-code",
					Name = "categoryName",
					StartDate = DateTime.Today,
					CatalogId = catalogId,
					ParentCategoryId = innerItem.CategoryId
				};
			repository.Add(subcategory);

			var linkedCategory = new LinkedCategory
				{
					Code = "l-code",
					CatalogId = catalogId,
					LinkedCatalogId = catalogId,
					LinkedCategoryId = innerItem.CategoryId
				};

			repository.Add(linkedCategory);
			repository.UnitOfWork.Commit();

			RefreshRepository(ref repository);

			var categoryRemove = repository.Categories.Where(x => x.CategoryId == innerItem.CategoryId).FirstOrDefault();

			repository.Remove(categoryRemove);
			repository.UnitOfWork.Commit();


			// Assert passed.
		}

		[RepositoryTheory]
		public void Can_delete_catalog_with_cascade()
		{
			var repository = GetRepository();

			var items = new Item[] { };
			const string catalogId = "testCatalog";

			CreateFullGraphCatalog(repository, ref items, catalogId);

			// create an AssociationGroup
			var group = new AssociationGroup
			{
				Name = "Name",
				ItemId = items[0].ItemId
			};
			repository.Add(group);

			var groupItem = new Association
			{
				AssociationType = "Required",
				AssociationGroupId = group.AssociationGroupId,
				ItemId = items[1].ItemId
			};
			repository.Add(groupItem);
			repository.UnitOfWork.Commit();

			RefreshRepository(ref repository);

			var innerItem = repository.Catalogs.Where(x => x.CatalogId == catalogId).Single();

			repository.Remove(innerItem);
			repository.UnitOfWork.Commit();

			var associationCount = repository.Associations.Count();
			var itemCount = repository.Items.Count();
			Assert.Equal(0, associationCount);
			Assert.Equal(0, itemCount);
		}

		[RepositoryTheory]
		[Variant(RepositoryProvider.DataService)]
		public void Can_query_catalog_items()
		{
			var repository = GetRepository();

			var items = new Item[] { };
			const string catalogId = "testCatalog";

			var catalogBuilder = CatalogBuilder.BuildCatalog(catalogId).WithCategory("category").WithProducts(200);
			for (int i = 0; i < 10; i++)
			{
				catalogBuilder = catalogBuilder.WithCategory("category " + i, "code-" + i);
			}
			var catalog = catalogBuilder.GetCatalog();
			items = catalogBuilder.GetItems();

			repository.Add(catalog);
			foreach (var item in items)
			{
				repository.Add(item);
			}

			repository.UnitOfWork.Commit();

			RefreshRepository(ref repository);

			var beginTime = DateTime.Now;
			var query = repository.Items.Where(i => i.CategoryItemRelations.Any(x => x.CatalogId == catalogId || x.Category.LinkedCategories.Any(y => y.CatalogId == catalogId)));

			var overallCount = query.Count();
			var orderedItems = query.OrderBy(x => x.ItemId);
			items = orderedItems.Skip(0).Take(25).ToList().ToArray();

			var queryTime = DateTime.Now - beginTime;
			Assert.True(queryTime < new TimeSpan(0, 0, 1, 2));
		}
	}
}
