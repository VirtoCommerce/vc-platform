using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Search
{
    [DataContract]
    public class ResultDocumentSet : IDocumentSet
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int TotalCount
        {
            get;set;
        }

        [DataMember]
        public object[] Properties
        {
            get;set;
        }

        [DataMember]
        public IDocument[] Documents
        {
            get;
            set;
        }
    }
}
