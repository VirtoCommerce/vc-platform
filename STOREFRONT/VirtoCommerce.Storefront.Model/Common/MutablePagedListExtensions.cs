namespace VirtoCommerce.Storefront.Model.Common
{
    public static class MutablePagedListExtensions
    {
        /// <summary>
        /// Loads current page and returns total items count.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int GetTotalCount<T>(this IMutablePagedList<T> list)
        {
            var result = 0;

            if (list != null)
            {
                using (list.GetEnumerator()) { }
                result = list.TotalItemCount;
            }

            return result;
        }
    }
}
