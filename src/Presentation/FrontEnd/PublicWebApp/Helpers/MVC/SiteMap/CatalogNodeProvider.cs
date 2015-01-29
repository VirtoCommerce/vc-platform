using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MvcSiteMapProvider;
using VirtoCommerce.ApiWebClient.Extensions.Routing;
using VirtoCommerce.ApiWebClient.Globalization;
using VirtoCommerce.ApiWebClient.Helpers;

namespace VirtoCommerce.Web.Helpers.MVC.SiteMap
{
    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Extensions;

    public class CatalogNodeProvider : DynamicNodeProviderBase
    {

        public const string TitleAttributeFormat = "Title-{0}";  
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var session = StoreHelper.CustomerSession;

            var nodes = new List<DynamicNode>();
            var order = 0;
            var client = ClientContext.Clients.CreateBrowseClient(session.StoreId, session.Language);
            var categories = Task.Run(() => client.GetCategoriesAsync()).Result;

            foreach (var category in categories.Items)
            {

                var pNode = new DynamicNode
                {
                    Action = "Category",
                    Title = category.Name,
                    Key = category.Id,
                    Order = order++,
                    ParentKey = category.ParentId,
                    RouteValues = new Dictionary<string, object> { { Constants.Category, category.Id } },
                    PreservedRouteParameters = new[] { Constants.Language, Constants.Store },
                     
                };

                //Add category translations for each language in store
                var store = StoreHelper.StoreClient.GetCurrentStore();
                foreach (var lang in store.Languages)
                {
                    var culture = CultureInfo.CreateSpecificCulture(lang);
                    pNode.Attributes.Add(string.Format(TitleAttributeFormat, culture.Name), category.Name);
                }

                nodes.Add(pNode);

                #region This is needed only for demo purposes
                if (category.Code.Equals("audio-mp3", StringComparison.OrdinalIgnoreCase))
                {
                    pNode.Attributes.Add("Template", "MegaMenu");
                    nodes.Add(new DynamicNode
                    {
                        Action = "Category",
                        Title = "Audio & MP3".Localize(),
                        Key = category.Id + "AUDIO",
                        ParentKey = category.Id,
                        ImageUrl = "~/Content/themes/default/images/menu/mobiles.png",
                        RouteValues = new Dictionary<string, object> { { Constants.Category, category.Id } },
                    });

                    nodes.Add(new DynamicNode
                    {
                        Action = "Category",
                        Title = "Samsung".Localize(),
                        ParentKey = category.Id + "AUDIO",
                        RouteValues = new Dictionary<string, object> { { Constants.Category, category.Id }, { "f_Brand", "samsung" } },
                    });
                    nodes.Add(new DynamicNode
                    {
                        Action = "Category",
                        Title = "Sony".Localize(),
                        ParentKey = category.Id + "AUDIO",
                        RouteValues = new Dictionary<string, object> { { Constants.Category, category.Id }, { "f_Brand", "sony" } },
                    });
                    nodes.Add(new DynamicNode
                    {
                        Action = "Category",
                        Title = "Apple".Localize(),
                        ParentKey = category.Id + "AUDIO",
                        RouteValues = new Dictionary<string, object> { { Constants.Category, category.Id }, { "f_Brand", "apple" } },
                    });

                    var computersCat = categories.Items.FirstOrDefault(
                        c => c.Code.Equals("computers-tablets", StringComparison.OrdinalIgnoreCase));

                    if (computersCat != null)
                    {

                        nodes.Add(new DynamicNode
                        {
                            Action = "Category",
                            Title = "Computers & Tablets".Localize(),
                            Key = category.Id + "COMPUTERS",
                            ParentKey = category.Id,
                            ImageUrl = "~/Content/themes/default/images/menu/computers.png",
                            RouteValues = new Dictionary<string, object> { { Constants.Category, computersCat.Id} },
                        });

                        nodes.Add(new DynamicNode
                        {
                            Action = "Category",
                            Title = "Samsung".Localize(),
                            ParentKey = category.Id + "COMPUTERS",
                            RouteValues =
                                new Dictionary<string, object>
                                {
                                    {Constants.Category, computersCat.Id},
                                    {"f_Brand", "samsung"}
                                },
                        });

                        nodes.Add(new DynamicNode
                        {
                            Action = "Category",
                            Title = "Sony".Localize(),
                            ParentKey = category.Id + "COMPUTERS",
                            RouteValues =
                                new Dictionary<string, object>
                                {
                                    {Constants.Category, computersCat.Id},
                                    {"f_Brand", "sony"}
                                },
                        });

                        nodes.Add(new DynamicNode
                        {
                            Action = "Category",
                            Title = "Apple".Localize(),
                            ParentKey = category.Id + "COMPUTERS",
                            RouteValues =
                                new Dictionary<string, object>
                                {
                                    {Constants.Category, computersCat.Id},
                                    {"f_Brand", "apple"}
                                },
                        });
                    }

                    var camCat = categories.Items.FirstOrDefault(
                        c => c.Code.Equals("cameras", StringComparison.OrdinalIgnoreCase));

                    if (camCat != null)
                    {

                        nodes.Add(new DynamicNode
                        {
                            Action = "Category",
                            Title = "Cameras".Localize(),
                            Key = category.Id + "CAMERAS",
                            ParentKey = category.Id,
                            ImageUrl = "~/Content/themes/default/images/menu/Cameras.png",
                            RouteValues = new Dictionary<string, object> { { Constants.Category, camCat.Id} },
                        });

                        nodes.Add(new DynamicNode
                        {
                            Action = "Category",
                            Title = "Samsung".Localize(),
                            ParentKey = category.Id + "CAMERAS",
                            RouteValues =
                                new Dictionary<string, object> { { Constants.Category, camCat.Id }, { "f_Brand", "samsung" } },
                        });
                        nodes.Add(new DynamicNode
                        {
                            Action = "Category",
                            Title = "Sony".Localize(),
                            ParentKey = category.Id + "CAMERAS",
                            RouteValues =
                                new Dictionary<string, object> { { Constants.Category, camCat.Id }, { "f_Brand", "sony" } },
                        });
                    }

                     var tvCat = categories.Items.FirstOrDefault(
                        c => c.Code.Equals("tv-video", StringComparison.OrdinalIgnoreCase));

                    if (tvCat != null)
                    {

                        nodes.Add(new DynamicNode
                        {
                            Action = "Category",
                            Title = "TV & Video".Localize(),
                            Key = category.Id + "TVVIDEO",
                            ParentKey = category.Id,
                            ImageUrl = "~/Content/themes/default/images/menu/accessories.png",
                            RouteValues = new Dictionary<string, object> { { Constants.Category, tvCat.Id} },
                        });

                        nodes.Add(new DynamicNode
                        {
                            Action = "Category",
                            Title = "Samsung".Localize(),
                            ParentKey = category.Id + "TVVIDEO",
                            RouteValues =
                                new Dictionary<string, object>
                                {
                                    {Constants.Category, tvCat.Id},
                                    {"f_Brand", "samsung"}
                                },
                        });
                        nodes.Add(new DynamicNode
                        {
                            Action = "Category",
                            Title = "Sony".Localize(),
                            ParentKey = category.Id + "TVVIDEO",
                            RouteValues =
                                new Dictionary<string, object> { { Constants.Category, tvCat.Id }, { "f_Brand", "sony" } },
                        });
                    }
                }
                #endregion
            }

            return nodes;

        }

    }
}