#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using VirtoCommerce.Web.Models.Extensions;
using services = VirtoCommerce.ApiClient.DataContracts.Lists;
using VirtoCommerce.Web.Models.Lists;
#endregion

namespace VirtoCommerce.Web.Models.Convertors
{
    public static class ListConverters
    {
        #region Public Methods and Operators
        public static LinkLists AsWebModel(this IEnumerable<services.LinkList> lists)
        {
            var listsModel = new LinkLists(lists.Select(l=>l.AsWebModel()));
            return listsModel;
        }

        public static LinkList AsWebModel(this services.LinkList list)
        {
            var listModel = new LinkList();
            listModel.InjectFrom(list);
            listModel.Handle = list.Name;
            listModel.Links = list.Links.Select(l => l.AsWebModel());
            return listModel;
        }

        public static Link AsWebModel(this services.Link link)
        {
            var linkModel = new Link();
            linkModel.InjectFrom(link);
            linkModel.Url = link.Url.ToAbsoluteUrl();
            linkModel.Handle = link.Title;
            linkModel.Active = link.IsActive;
            return linkModel;
        }
        #endregion
    }
}