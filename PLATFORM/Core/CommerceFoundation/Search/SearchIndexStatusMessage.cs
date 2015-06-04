using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks.CQRS;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Search.Model;

namespace VirtoCommerce.Foundation.Search
{
    [DataContract]
    public class SearchIndexStatusMessage : IMessage
    {
        [DataMember]
        public string Scope
        {
            get;
            private set;
        }

        [DataMember]
        public BuildStatus Status
        {
            get;
            private set;
        }

        [DataMember]
        public string DocumentType
        {
            get;
            private set;
        }

        public SearchIndexStatusMessage(string scope, string documentType, BuildStatus status)
        {
            Status = status;
            Scope = scope;
            DocumentType = documentType;
        }
    }
}