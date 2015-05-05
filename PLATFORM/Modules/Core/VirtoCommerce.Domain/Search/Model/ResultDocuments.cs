using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Domain.Search
{
    public class ResultDocumentSet : IDocumentSet
    {
        public string Name { get; set; }

        public int TotalCount
        {
            get;set;
        }

        public object[] Properties
        {
            get;set;
        }

        public IDocument[] Documents
        {
            get;
            set;
        }
    }
}
