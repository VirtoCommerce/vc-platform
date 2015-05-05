using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.SearchModule.Data.Provides.Elastic
{
    public class ESDocument : Dictionary<string, object>
    {
        public object Id { 
            get
            {
                if (this.ContainsKey("__key"))
                    return this["__key"];

                return null;
            }
            set
            {
                if (this.ContainsKey("__key"))
                    this["__key"] = value;
                else
                    this.Add("__key", value);
            }
        }
    }
}
