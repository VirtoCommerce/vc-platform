using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Data.Repositories
{
	public interface ICatalogRepository : IRepository
	{
		IQueryable<dataModel.Category> Categories { get; }
		IQueryable<dataModel.Catalog> Catalogs { get; }
		IQueryable<dataModel.Item> Items { get; }
		IQueryable<dataModel.Property> Properties { get; }
		IQueryable<dataModel.Image> Images { get; }
		IQueryable<dataModel.Asset> Assets { get; }
		IQueryable<dataModel.EditorialReview> EditorialReviews { get; }
		IQueryable<dataModel.PropertyValue> PropertyValues { get; }
        IQueryable<dataModel.PropertyDictionaryValue> PropertyDictionaryValues { get; }
        IQueryable<dataModel.ItemRelation> ItemRelations { get; }
		IQueryable<dataModel.CategoryItemRelation> CategoryItemRelations { get; }
		IQueryable<dataModel.Association> Associations { get; }
        IQueryable<dataModel.AssociationGroup> AssociationGroups { get; }
        IQueryable<dataModel.CategoryRelation> CategoryLinks { get; }

		
        string[] GetAllChildrenCategoriesIds(string[] categoryIds);
		dataModel.Catalog GetCatalogById(string catalogId);
		dataModel.Category[] GetCategoriesByIds(string[] categoryIds, moduleModel.CategoryResponseGroup respGroup);
		dataModel.Item[] GetItemByIds(string[] itemIds, moduleModel.ItemResponseGroup respGroup);
        dataModel.Property[] GetAllCatalogProperties(string catalogId);
		dataModel.Property[] GetPropertiesByIds(string[] propIds);
	
		void RemoveItems(string[] ids);
		void RemoveCategories(string[] ids);
		void RemoveCatalogs(string[] ids);

		

	}
}
