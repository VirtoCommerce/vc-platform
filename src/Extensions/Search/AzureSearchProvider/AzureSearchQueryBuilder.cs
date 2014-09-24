using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Search.Providers.Azure
{
    using RedDog.Search.Model;

    using VirtoCommerce.Foundation.Catalogs.Search;
    using VirtoCommerce.Foundation.Search;

    public class AzureSearchQueryBuilder : ISearchQueryBuilder
    {
        public object BuildQuery(ISearchCriteria criteria)
        {
            var builder = new SearchQuery();
            var filterBuilder = new StringBuilder();

            builder.Skip(criteria.StartingRecord);
            builder.Top(criteria.RecordsToRetrieve);
            builder.Count(true);

            #region Sorting

            // Add sort order
            if (criteria.Sort != null)
            {
                var fields = criteria.Sort.GetSort();
                foreach (var field in fields)
                {
                    builder.OrderBy = String.Format("{0}{1}{2}", String.IsNullOrEmpty(builder.OrderBy) ? "" : builder.OrderBy + ",", field.FieldName, field.IsDescending ? " desc" : "");
                }
            }

            #endregion

            #region CatalogItemSearchCriteria
            if (criteria is CatalogItemSearchCriteria)
            {
                var c = criteria as CatalogItemSearchCriteria;

                if (!String.IsNullOrEmpty(c.SearchPhrase))
                {
                    builder.Query = c.SearchPhrase;
                }


                /*
                if (c.StartDateFrom.HasValue)
                {
                    mainQuery.Must(m => m
                        .Range(r => r.Field("startdate").From(c.StartDateFrom.Value.ToString("s")))
                   );
                }
                 * */

                /*
                if (c.EndDate.HasValue)
                {
                    mainQuery.Must(m => m
                        .Range(r => r.Field("enddate").From(c.EndDate.Value.ToString("s")))
                   );
                }
                 * */
                /*
                mainQuery.Must(m => m.Term(t => t.Field("__hidden").Value("false")));

                if (c.Outlines != null && c.Outlines.Count > 0)
                    AddQuery("__outline", mainQuery, c.Outlines);

                if (!String.IsNullOrEmpty(c.SearchPhrase))
                {
                    var contentField = string.Format("__content_{0}", c.Locale.ToLower());
                    AddQueryString(mainQuery, c, "__content", contentField);
                }

                 * */

                if (!String.IsNullOrEmpty(c.Catalog))
                {
                    //filterBuilder.AppendFormat("catalog eq '{0}'", c.Catalog);
                }

                builder.Filter = filterBuilder.ToString();

            }
            #endregion

            return builder;
        }

        protected void AddQueryString(
            SearchQuery query,
            CatalogItemSearchCriteria filter,
            params string[] fields)
        {
            
        }
    }
}
