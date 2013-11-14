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
    public class SearchIndexMessage : IJobMessage
    {
        [DataMember]
        public string Scope
        {
            get;
            private set;
        }

        [DataMember]
        public OperationType OpType
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

        [DataMember]
        public Partition Partition
        {
            get;
            private set;
        }

        public SearchIndexMessage(string jobId, string scope, string documentType, Partition partition)
        {
            Partition = partition;
            JobId = jobId;
            Scope = scope;
            DocumentType = documentType;
        }

        [DataMember]
        public string JobId
        {
            get;
            private set;
        }
    }
}