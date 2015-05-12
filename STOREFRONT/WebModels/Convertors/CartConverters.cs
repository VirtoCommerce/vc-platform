#region
using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.ApiClient.DataContracts.Cart;
using VirtoCommerce.Web.Models;
using Data = VirtoCommerce.ApiClient.DataContracts.Cart;

#endregion

namespace VirtoCommerce.Web.Convertors
{
    public static class CartConverters
    {
        #region Public Methods and Operators
        public static Data.ShoppingCart AsServiceModel(this Cart cart)
        {
            var ret = new Data.ShoppingCart { Id = cart.Key };

            if (cart.Items != null && cart.Items.Any())
            {
                ret.Items = new List<Data.CartItem>(cart.Items.Select(x => x.AsServiceModel()));
                //cart.Items.ForEach(i => ret.Items.Add(i.AsServiceModel()));
            }
            else
            {
                ret.Items = new List<Data.CartItem>();
            }

            return ret;
        }

        public static Data.CartItem AsServiceModel(this LineItem item)
        {
            var ret = new Data.CartItem
                      {
                          Id = item.Id,
                          ProductCode = item.Handle,
                          ProductId = item.ProductId,
                          Name = item.Title,
                          PlacedPrice = item.Price,
                          ExtendedPrice = item.Price,
                          Quantity = item.Quantity,
                          ImageUrl = item.Image,
                          CategoryId = "fake",
                          CatalogId = "fake"
                      };

            return ret;
        }

        public static Cart AsWebModel(this Data.ShoppingCart cart)
        {
            var ret = new Cart { Key = cart.Id };

            if (cart.Items != null && cart.Items.Any())
            {
                ret.Items.AddRange(cart.Items.Select(x => x.AsWebModel()));
            }

            return ret;
        }

        public static LineItem AsWebModel(this Data.CartItem item)
        {
            var product = new Product
                          {
                              Title = item.Name,
                              Description = item.Name,
                              Excerpt = item.Name,
                              Handle = item.ProductId
                          };

            var variant = new Variant();

            var ret = new LineItem
                      {
                          Id = item.Id,
                          Handle = item.ProductId,
                          ProductId = item.ProductId,
                          Title = item.Name,
                          Price = item.PlacedPrice,
                          Quantity = item.Quantity,
                          Sku = item.ProductCode,
                          VariantId = item.ProductId,
                          Image = item.ImageUrl,
                          Variant = variant,
                          Product = product,
                          Url = String.Format("/products/{0}", item.ProductId)
                      };

            return ret;
        }

        public static CustomerAddress AsCartWebModel(this Address address)
        {
            return new CustomerAddress
            {
                Address1 = address.Line1,
                Address2 = address.Line2,
                City = address.City,
                Company = address.Organization,
                Country = address.CountryName,
                CountryCode = address.CountryCode,
                Province = address.RegionName,
                FirstName = address.FirstName,
                LastName = address.LastName,
                Phone = address.Phone,
                Zip = address.PostalCode
            };
        }

        public static Address AsCartServiceModel(this CustomerAddress customerAddress)
        {
            return new Address
            {
                City = customerAddress.City,
                CountryCode = customerAddress.CountryCode,
                CountryName = customerAddress.Country,
                RegionName = customerAddress.Province,
                FirstName = customerAddress.FirstName,
                LastName = customerAddress.LastName,
                Line1 = customerAddress.Address1,
                Line2 = customerAddress.Address2,
                Organization = customerAddress.Company,
                Phone = customerAddress.Phone,
                PostalCode = customerAddress.Zip,
                Zip = customerAddress.Zip // TODO: Resolve
            };
        }
        #endregion
    }
}