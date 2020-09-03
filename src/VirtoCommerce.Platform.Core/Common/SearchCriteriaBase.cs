using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Platform.Core.Common
{
    public abstract class SearchCriteriaBase : ValueObject
    {
        public string ResponseGroup { get; set; }

        /// <summary>
        /// Search object type
        /// </summary>
        public virtual string ObjectType { get; set; }

        private IList<string> _objectTypes;
        public virtual IList<string> ObjectTypes
        {
            get
            {
                if (_objectTypes == null && !string.IsNullOrEmpty(ObjectType))
                {
                    _objectTypes = new[] { ObjectType };
                }
                return _objectTypes;
            }
            set
            {
                _objectTypes = value;
            }
        }

        public IList<string> ObjectIds { get; set; }

        /// <summary>
        /// Search phrase
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// Property is left for backward compatibility
        /// </summary>
        public string SearchPhrase
        {
            get { return Keyword; }
            set { Keyword = value; }
        }

        /// <summary>
        /// Search phrase language 
        /// </summary>
        public string LanguageCode { get; set; }

        public string Sort { get; set; }

        private IList<SortInfo> _sortInfos;
        public virtual IList<SortInfo> SortInfos
        {
            get
            {
                if (_sortInfos == null)
                {
                    _sortInfos = SortInfo.Parse(Sort).ToList();
                }
                return _sortInfos;
            }
        }


        public int Skip { get; set; }
        public int Take { get; set; } = 20;
    }
}
