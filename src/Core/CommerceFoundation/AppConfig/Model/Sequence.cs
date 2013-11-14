using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.AppConfig.Model
{
    [DataContract]
    [EntitySet("Sequence")]
    [DataServiceKey("ObjectType")]
    public class Sequence : StorageEntity
    {
        private string _objectType;
        [Key]
        [StringLength(256)]
        [DataMember]
        public string ObjectType
        {
            get { return _objectType; }
            set { SetValue(ref _objectType, () => ObjectType, value); }
        }

        private int _value;
        [DataMember]
		[Required]
        public int Value
        {
			get { return _value; }
			set { SetValue(ref _value, () => Value, value); }
        }

    }
}
