using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Catalog.Model
{
    public class Asset : AuditableEntity, ISeoSupport, ILanguageSupport, IInheritable, ICloneable
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Group { get; set; }
        public string MimeType { get; set; }
        public long Size { get; set; }
        public byte[] BinaryData { get; set; }


        #region ISeoSupport Members
        public string SeoObjectType { get { return GetType().Name; } }
        public ICollection<SeoInfo> SeoInfos { get; set; }
        #endregion

        #region ILanguageSupport Members
        public string LanguageCode { get; set; }
        #endregion

        #region IInheritable Members
        public bool IsInherited { get; set; }
        #endregion

        #region ICloneable members
        public object Clone()
        {
            var retVal = new Asset();
            retVal.CreatedBy = CreatedBy;
            retVal.CreatedDate = CreatedDate;
            retVal.ModifiedBy = ModifiedBy;
            retVal.ModifiedDate = ModifiedDate;

            retVal.Name = Name;
            retVal.Url = Url;
            retVal.Group = Group;
            retVal.LanguageCode = LanguageCode;
            retVal.MimeType = MimeType;
            retVal.Size = Size;

            retVal.IsInherited = IsInherited;
            if (SeoInfos != null)
            {
                retVal.SeoInfos = SeoInfos.Select(x => x.Clone()).OfType<SeoInfo>().ToList();
            }
            return retVal;
        }
        #endregion
    }
}
