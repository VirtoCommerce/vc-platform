using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.OData;
using VirtoCommerce.MerchandisingModule.Model;

namespace VirtoCommerce.MerchandisingModule.Services
{
    public interface IMerchantItemService
    {
        void Create(string categoryId, Product product);
        void Update(Delta<Product> delta);
        void Create(string categoryId, string productId, ProductVariation variation);
        void Update(Delta<ProductVariation> delta);
        T GetItem<T>(string itemId, ItemResponseGroups responseGroup) where T : CatalogItem;
        void Delete(string id);
    }
}
