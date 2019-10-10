using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

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
        /// Search phrase language 
        /// </summary>
        public string LanguageCode { get; set; }

        public string Sort { get; set; }

        [JsonIgnore]
        public virtual IList<SortInfo> SortInfos => SortInfo.Parse(Sort).ToArray();


        public int Skip { get; set; }
        public int Take { get; set; } = 20;
    }
}
