using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Data.Repositories
{
	public interface IFoundationCatalogRepository : ICatalogRepository
	{
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
