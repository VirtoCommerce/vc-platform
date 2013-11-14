using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.AppConfig.Model
{
    [DataContract]
    [EntitySet("Statistics")]
    [DataServiceKey("StatisticId")]
    public class Statistic:StorageEntity 
    {
        public Statistic()
        {
            _statisticId = GenerateNewKey();
        }


        private string _statisticId;
        [Key]
        [StringLength(128)]
        [DataMember]
        public string StatisticId
        {
            get { return _statisticId; }
            set { SetValue(ref _statisticId, () => StatisticId, value); }
        }


        private string _key;
        [DataMember]
        [StringLength(32)]
        public string Key
        {
            get { return _key; }
            set { SetValue(ref _key, () => Key, value); }
        }


        private string _name;
        [DataMember]
        [StringLength(64)]
        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, () => Name, value); }
        }


        private string _value;
        [DataMember]
        [StringLength(64)]
        public string Value
        {
            get { return _value; }
            set { SetValue(ref _value, () => Value, value); }
        }

    }
}
