using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Reflection;
using FunctionalTests.Catalogs;
using FunctionalTests.Catalogs.Helpers;
using FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.Catalogs.Model;
using Xunit;
using linq = System.Linq.Expressions;

namespace FunctionalTests.Repository
{
	// [Variant(RepositoryProvider.EntityFramework)]
	[Variant(RepositoryProvider.DataService)]
	public class CatalogQueryScenarios : CatalogTestBase, IDisposable
	{
		#region Tests

		/// <summary>
		/// Test shows that a query can be built dynamically using pure Linq.Expressions.
		/// </summary>
		[RepositoryTheory]
		public void Can_query_item_by_dynamic_expression()
		{
			var repository = GetRepository();
			var items = new Item[] { };
			CreateFullGraphCatalog(repository, ref items, "testcatalog");
			var names = items.Select(x => x.Name).ToArray();

			// the expression is: (x => x.Name == "val1" || x.Name == "val2" || ....)
			var parameter = linq.Expression.Parameter(typeof(Item), "x");
			linq.Expression condition = linq.Expression.Constant(false);
			foreach (var name in names)
			{
				condition = linq.Expression.OrElse(
					condition,
					linq.Expression.Equal(
						linq.Expression.Property(parameter, "Name"),
						linq.Expression.Constant(name)));
			}

			var expression = linq.Expression.Lambda<Func<Item, bool>>(condition, parameter);
			var query = repository.Items.Where(expression);
			var result = query.ToList();

			Assert.NotNull(result);
			Assert.True(result.Count > 0);
		}

		/// <summary>
		/// this test builds complex linq expression tree. The query result is the same as in test Can_query_catalog_items()
		/// </summary>
		[RepositoryTheory]
		public void Can_query_item_by_linq_expression_tree()
		{
			var categoryIds = new List<string>();
			var repository = GetRepository();
			const string catalogId = "testCatalog";

			var catalogBuilder = CatalogBuilder.BuildCatalog(catalogId).WithCategory("category").WithProducts(20);
			for (int i = 0; i < 5; i++)
			{
				catalogBuilder = catalogBuilder.WithCategory("category " + i, "code-" + i);
			}
			var catalog = catalogBuilder.GetCatalog();
			var items = catalogBuilder.GetItems();
			categoryIds.AddRange(catalog.CategoryBases.Select(x => x.CategoryId));

			repository.Add(catalog);
			foreach (var item in items)
			{
				repository.Add(item);
			}

			repository.UnitOfWork.Commit();
			RefreshRepository(ref repository);

			var query = repository.Items;
			// the expression is: expression = x => x.CatalogId == catalogId || x.CategoryId == categoryIds[0] || x.CategoryId == categoryIds[1] || ..
			// the query is: query = repository.Items.Where(item => item.CategoryItemRelations.Any(expression));
			var parameterCIR = linq.Expression.Parameter(typeof(CategoryItemRelation), "x");
			var propertyCategoryId = linq.Expression.Property(parameterCIR, "CategoryId");
			linq.Expression condition = linq.Expression.Equal(linq.Expression.Property(parameterCIR, "CatalogId"), linq.Expression.Constant(catalogId));
			condition = categoryIds.Aggregate(condition, (current, id) => linq.Expression.OrElse(current, linq.Expression.Equal(propertyCategoryId, linq.Expression.Constant(id))));
			var expression = linq.Expression.Lambda<Func<CategoryItemRelation, bool>>(condition, parameterCIR);

			var methodAny = typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static)
				.Where(x => x.Name == "Any")
				.Single(mi => mi.GetParameters().Count() == 2)
				.MakeGenericMethod(typeof(CategoryItemRelation));

			var parameterItem = linq.Expression.Parameter(typeof(Item), "item");
			query = query.Where(
						linq.Expression.Lambda<Func<Item, bool>>(
							linq.Expression.Call(null, methodAny, new linq.Expression[]
							{
								linq.Expression.PropertyOrField(parameterItem, "CategoryItemRelations"), expression
							}),
						new[] { parameterItem }));

			var result = query.ToList();

			Assert.NotNull(result);
			Assert.True(result.Count > 0);
		}

		/// <summary>
		/// test shows that any query can be written if it's supported by OData web API. Filter can be built dynamically as a string.
		/// </summary>
		[RepositoryTheory]
		public void Can_write_any_query_by_AddQueryOption_expression()
		{
			var repository = GetRepository();
			var items = new Item[] { };
			CreateFullGraphCatalog(repository, ref items, "testcatalog");

			var productNames = items.Select(x => x.Name).ToArray();

			var query = repository.Items;

			var query1 = query.Where(x => x.Name == productNames[0] || x.Name == productNames[1]);
			var count1 = query1.Count();
			var result1 = query1.ToList();

			// format query URI
			var filterParams = productNames.Select(x => string.Format("Name eq '{0}'", x));
			var filter = string.Join(" or ", filterParams);
			var queryBase = (DataServiceQuery<Item>)query;
			queryBase = queryBase.AddQueryOption("$filter", filter);
			var count2 = queryBase.Count();
			var result2 = queryBase.ToList();

			Assert.Equal(count1, count2);
			Assert.True(result1.Contains(result2[0]));
		}

		#endregion
	}
}
