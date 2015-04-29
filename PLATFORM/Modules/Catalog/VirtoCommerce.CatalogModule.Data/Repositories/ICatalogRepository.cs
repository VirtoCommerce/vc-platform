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
		IQueryable<foundation.CategoryBase> Categories { get; }
		IQueryable<foundation.CatalogBase> Catalogs { get; }
		IQueryable<foundation.Item> Items { get; }
		IQueryable<foundation.Property> Properties { get; }
		IQueryable<foundation.PropertySet> PropertySets { get; }
		IQueryable<foundation.ItemRelation> ItemRelations { get; }
		IQueryable<foundation.CategoryItemRelation> CategoryItemRelations { get; }
		IQueryable<foundation.Association> Associations { get; }
		IQueryable<foundation.SeoUrlKeyword> SeoUrlKeywords { get; }

		foundation.SeoUrlKeyword[] GetAllSeoInformation(string id);
		foundation.Category[] GetAllCategoryParents(foundation.Category categoryId);
		foundation.CatalogBase GetCatalogById(string catalogId);
		foundation.Category GetCategoryById(string categoryId);
		foundation.Item[] GetItemByIds(string[] itemIds, moduleModel.ItemResponseGroup respGroup);
		foundation.Item[] GetAllItemVariations(string itemId);
        Dictionary<string, IEnumerable<foundation.Item>> GetAllItemsVariations(string[] itemIds);
		foundation.Property[] GetPropertiesByIds(string[] propIds);
		foundation.Catalog GetPropertyCatalog(string propId);
		foundation.Category GetPropertyCategory(string propId);
		foundation.LinkedCategory[] GetCategoryLinks(string categoryId);
		foundation.LinkedCategory[] GetCatalogLinks(string catalogId);
		foundation.Property[] GetCatalogProperties(foundation.CatalogBase catalog);
		foundation.Property[] GetAllCategoryProperties(foundation.Category category);
		void SetItemCategoryRelation(foundation.Item item, foundation.Category category);
		void SetVariationRelation(foundation.Item item, string mainProductId);
		void SwitchProductToMain(foundation.Item item);
		void SetCatalogProperty(foundation.Catalog catalog, foundation.Property property);
		void SetCategoryProperty(foundation.Category category, foundation.Property property);
		void RemoveItems(string[] itemIds);
		

	}
}
