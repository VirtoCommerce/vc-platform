namespace VirtoCommerce.ApiClient.DataContracts
{
    public static class SeoUrlKeywordTypes
    {
        public static readonly SeoUrlKeywordType Category = new SeoUrlKeywordType("category");
        public static readonly SeoUrlKeywordType Item = new SeoUrlKeywordType("item");
        public static readonly SeoUrlKeywordType Store = new SeoUrlKeywordType("store");
    }

    public struct SeoUrlKeywordType
    {
        public readonly string Name;

        public SeoUrlKeywordType(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }

        #region Overrides of ValueType

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param>
        public override bool Equals(object obj)
        {
            if (obj is SeoUrlKeywordType)
            {
                return ((SeoUrlKeywordType)obj).Name.Equals(this.Name);
            }

            return base.Equals(obj);
        }

        #endregion
    }
}
