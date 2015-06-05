using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Catalog.Model;
using Xunit;

namespace VirtoCommerce.SearchModule.Tests
{
	public class OutlineBuilderTest
	{
		[Fact]
		public void LinkParentToCatalog()
		{
			var rootCategory = GetCategory("catalog", "root");
			var childCategory = GetCategory("catalog", "child");
			var vrootCategory = GetCategory("vcatalog", "vroot");
			var vchildCategory = GetCategory("vcatalog", "vchild");
			vchildCategory.Parents = new Category[] { vrootCategory };
			//root/child
			//root -> vcatalog/vroot/vchild
			childCategory.Parents = new Category[] { rootCategory };
			rootCategory.Links = new CategoryLink[] { new CategoryLink { CatalogId = "vcatalog", CategoryId = "vchild", Category = vchildCategory } };
			
			var outline = GetOutlines(childCategory);
			outline = GetOutlines(rootCategory);
		}

	
		private string[] GetOutlines(Category category)
		{
			var retVal = new List<string>();
			var stringBuilder = new StringBuilder();
			
			//first direct outline
			var outline = new string[] { category.CatalogId }.Concat(category.Parents.Select(x => x.Id)).Concat(new string[] { category.Id });
			retVal.Add(String.Join("/", outline));
			//Next direct links (need remove directory id from outline for displaying products in mapped virtual category)
			foreach(var link in category.Links)
			{
				outline = new string[] { link.CatalogId };
				if(link.CategoryId != null)
				{
					outline = outline.Concat(link.Category.Parents.Select(x => x.Id)).Concat(new string[] { link.CategoryId });
				}
				retVal.Add(String.Join("/", outline));
			}

			//Parent category links 
			foreach (var parent in category.Parents)
			{
				foreach (var link in parent.Links)
				{
					outline = new string[] { link.CatalogId };
					if (link.CategoryId != null)
					{
						outline = outline.Concat(link.Category.Parents.Select(x => x.Id)).Concat(new string[] { link.CategoryId });
					}
					outline = outline.Concat(new string[] { parent.Id });
					retVal.Add(String.Join("/", outline));
				}
			}

			return retVal.ToArray();

		}

		private Category GetCategory(string catalog, string id)
		{
			return new Category { CatalogId = catalog, Id = id, Links = new List<CategoryLink>(), Parents = new Category[] {} };
		}
	}
}
