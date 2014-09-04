using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MvcSiteMapProvider;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Web.Client.Extensions;
using VirtoCommerce.Web.Client.Extensions.Routing;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Virto.Helpers.MVC.SiteMap
{
    public class CatalogNodeProvider : DynamicNodeProviderBase
    {

        public const string TitleAttributeFormat = "Title-{0}";  
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var catalog = CatalogHelper.CatalogClient.GetCatalog(StoreHelper.CustomerSession.CatalogId);
            var nodes = new List<DynamicNode>();
            int order = 0;
            var categories = catalog.CategoryBases.OfType<Category>().Where(x => x.IsActive).ToList();
            foreach (var category in categories.OrderByDescending(x => x.Priority).ThenBy(x => x.Name))
            {

                var pNode = new DynamicNode
                {
                    Action = "Display",
                    Title = category.DisplayName(),
                    Key = category.CategoryId,
                    Order = order++,
                    ParentKey = category.ParentCategoryId,
                    RouteValues = new Dictionary<string, object> { { Constants.Category, category.CategoryId } },
                    PreservedRouteParameters = new[] { Constants.Language, Constants.Store },
                     
                };

                //Add category translations for each language in store
                var store = StoreHelper.StoreClient.GetCurrentStore();
                foreach (var lang in store.Languages)
                {
                    var culture = CultureInfo.CreateSpecificCulture(lang.LanguageCode);
                    pNode.Attributes.Add(string.Format(TitleAttributeFormat, culture.Name),
                        category.DisplayName(culture.Name));
                }

                nodes.Add(pNode);

                #region This is needed only for demo purposes
                if (category.Code.Equals("audio-mp3", StringComparison.OrdinalIgnoreCase))
                {
                    pNode.Attributes.Add("Template", "MegaMenu");
                    nodes.Add(new DynamicNode
                    {
                        Action = "Display",
                        Title = "Audio & MP3".Localize(),
                        Key = category.CategoryId + "AUDIO",
                        ParentKey = category.CategoryId,
                        ImageUrl = "~/Content/themes/default/images/menu/mobiles.png",
                        RouteValues = new Dictionary<string, object> { { Constants.Category, category.CategoryId } },
                    });

                    nodes.Add(new DynamicNode
                    {
                        Action = "Display",
                        Title = "Samsung".Localize(),
                        ParentKey = category.CategoryId + "AUDIO",
                        RouteValues = new Dictionary<string, object> { { Constants.Category, category.CategoryId }, { "f_Brand", "samsung" } },
                    });
                    nodes.Add(new DynamicNode
                    {
                        Action = "Display",
                        Title = "Sony".Localize(),
                        ParentKey = category.CategoryId + "AUDIO",
                        RouteValues = new Dictionary<string, object> { { Constants.Category, category.CategoryId }, { "f_Brand", "sony" } },
                    });
                    nodes.Add(new DynamicNode
                    {
                        Action = "Display",
                        Title = "Apple".Localize(),
                        ParentKey = category.CategoryId + "AUDIO",
                        RouteValues = new Dictionary<string, object> { { Constants.Category, category.CategoryId }, { "f_Brand", "apple" } },
                    });

                    var computersCat = categories.FirstOrDefault(
                        c => c.Code.Equals("computers-tablets", StringComparison.OrdinalIgnoreCase));

                    if (computersCat != null)
                    {

                        nodes.Add(new DynamicNode
                        {
                            Action = "Display",
                            Title = "Computers & Tablets".Localize(),
                            Key = category.CategoryId + "COMPUTERS",
                            ParentKey = category.CategoryId,
                            ImageUrl = "~/Content/themes/default/images/menu/computers.png",
                            RouteValues = new Dictionary<string, object> { { Constants.Category, computersCat.CategoryId} },
                        });

                        nodes.Add(new DynamicNode
                        {
                            Action = "Display",
                            Title = "Samsung".Localize(),
                            ParentKey = category.CategoryId + "COMPUTERS",
                            RouteValues =
                                new Dictionary<string, object>
                                {
                                    {Constants.Category, computersCat.CategoryId},
                                    {"f_Brand", "samsung"}
                                },
                        });

                        nodes.Add(new DynamicNode
                        {
                            Action = "Display",
                            Title = "Sony".Localize(),
                            ParentKey = category.CategoryId + "COMPUTERS",
                            RouteValues =
                                new Dictionary<string, object>
                                {
                                    {Constants.Category, computersCat.CategoryId},
                                    {"f_Brand", "sony"}
                                },
                        });

                        nodes.Add(new DynamicNode
                        {
                            Action = "Display",
                            Title = "Apple".Localize(),
                            ParentKey = category.CategoryId + "COMPUTERS",
                            RouteValues =
                                new Dictionary<string, object>
                                {
                                    {Constants.Category, computersCat.CategoryId},
                                    {"f_Brand", "apple"}
                                },
                        });
                    }

                    var camCat = categories.FirstOrDefault(
                        c => c.Code.Equals("cameras", StringComparison.OrdinalIgnoreCase));

                    if (camCat != null)
                    {

                        nodes.Add(new DynamicNode
                        {
                            Action = "Display",
                            Title = "Cameras".Localize(),
                            Key = category.CategoryId + "CAMERAS",
                            ParentKey = category.CategoryId,
                            ImageUrl = "~/Content/themes/default/images/menu/Cameras.png",
                            RouteValues = new Dictionary<string, object> { { Constants.Category, camCat.CategoryId} },
                        });

                        nodes.Add(new DynamicNode
                        {
                            Action = "Display",
                            Title = "Samsung".Localize(),
                            ParentKey = category.CategoryId + "CAMERAS",
                            RouteValues =
                                new Dictionary<string, object> { { Constants.Category, camCat.CategoryId }, { "f_Brand", "samsung" } },
                        });
                        nodes.Add(new DynamicNode
                        {
                            Action = "Display",
                            Title = "Sony".Localize(),
                            ParentKey = category.CategoryId + "CAMERAS",
                            RouteValues =
                                new Dictionary<string, object> { { Constants.Category, camCat.CategoryId }, { "f_Brand", "sony" } },
                        });
                    }

                     var tvCat = categories.FirstOrDefault(
                        c => c.Code.Equals("tv-video", StringComparison.OrdinalIgnoreCase));

                    if (tvCat != null)
                    {

                        nodes.Add(new DynamicNode
                        {
                            Action = "Display",
                            Title = "TV & Video".Localize(),
                            Key = category.CategoryId + "TVVIDEO",
                            ParentKey = category.CategoryId,
                            ImageUrl = "~/Content/themes/default/images/menu/accessories.png",
                            RouteValues = new Dictionary<string, object> { { Constants.Category, tvCat.CategoryId} },
                        });

                        nodes.Add(new DynamicNode
                        {
                            Action = "Display",
                            Title = "Samsung".Localize(),
                            ParentKey = category.CategoryId + "TVVIDEO",
                            RouteValues =
                                new Dictionary<string, object>
                                {
                                    {Constants.Category, tvCat.CategoryId},
                                    {"f_Brand", "samsung"}
                                },
                        });
                        nodes.Add(new DynamicNode
                        {
                            Action = "Display",
                            Title = "Sony".Localize(),
                            ParentKey = category.CategoryId + "TVVIDEO",
                            RouteValues =
                                new Dictionary<string, object> { { Constants.Category, tvCat.CategoryId }, { "f_Brand", "sony" } },
                        });
                    }
                }
                #endregion
            }

            return nodes;

        }

    }
}