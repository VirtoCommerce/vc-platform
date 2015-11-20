using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using foundation = VirtoCommerce.CatalogModule.Data.Model;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Data.Repositories
{
	public interface ICatalogRepository : IRepository
	{
		IQueryable<foundation.Category> Categories { get; }
		IQueryable<foundation.Catalog> Catalogs { get; }
		IQueryable<foundation.Item> Items { get; }
		IQueryable<foundation.Property> Properties { get; }
		IQueryable<foundation.Image> Images { get; }
		IQueryable<foundation.Asset> Assets { get; }
		IQueryable<foundation.EditorialReview> EditorialReviews { get; }
		IQueryable<foundation.PropertyValue> PropertyValues { get; }
        IQueryable<foundation.PropertyDictionaryValue> PropertyDictionaryValues { get; }
        IQueryable<foundation.ItemRelation> ItemRelations { get; }
		IQueryable<foundation.CategoryItemRelation> CategoryItemRelations { get; }
		IQueryable<foundation.Association> Associations { get; }
        IQueryable<foundation.AssociationGroup> AssociationGroups { get; }
        IQueryable<foundation.CategoryRelation> CategoryLinks { get; }

		foundation.Category[] GetAllCategoryParents(foundation.Category categoryId);
        string[] GetAllChildrenCategoriesIds(string[] categoryIds);
		foundation.Catalog GetCatalogById(string catalogId);
		foundation.Category GetCategoryById(string categoryId);
		foundation.Category[] GetCategoriesByIds(string[] categoryIds);
		foundation.Item[] GetItemByIds(string[] itemIds, moduleModel.ItemResponseGroup respGroup);
		foundation.Property[] GetPropertiesByIds(string[] propIds);
		foundation.Catalog GetPropertyCatalog(string propId);
		foundation.Category GetPropertyCategory(string propId);
		foundation.Property[] GetAllCategoryProperties(foundation.Category category);
	
		void RemoveItems(string[] ids);
		void RemoveCategories(string[] ids);
		void RemoveCatalogs(string[] ids);

		

	}
}
