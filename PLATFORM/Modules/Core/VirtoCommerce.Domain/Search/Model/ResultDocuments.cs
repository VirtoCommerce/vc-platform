namespace VirtoCommerce.Domain.Search.Model
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
