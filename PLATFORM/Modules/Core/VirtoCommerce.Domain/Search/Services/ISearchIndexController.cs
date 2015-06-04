namespace VirtoCommerce.Domain.Search.Services
{
    public interface ISearchIndexController
    {
        void Process(string scope, string documentType, bool rebuild);
    }
}
