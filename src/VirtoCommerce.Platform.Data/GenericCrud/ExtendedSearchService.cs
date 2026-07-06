using System;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.GenericCrud;
using Microsoft.Extensions.Options;

namespace VirtoCommerce.Platform.Data.GenericCrud
{
    /// <summary>
    /// A <see cref="SearchService{TCriteria, TResult, TModel, TEntity}"/> that opts in to the optimized
    /// "search all" path by implementing <see cref="IExtendedSearchService{TCriteria, TResult, TModel}"/>.
    /// The members are inherited from the base as-is; derive from this class only when the result membership
    /// is fully defined by <see cref="SearchService{TCriteria, TResult, TModel, TEntity}.BuildQuery"/>/
    /// <see cref="SearchService{TCriteria, TResult, TModel, TEntity}.GetOrderedQueryAsync"/>
    /// (i.e. <see cref="SearchService{TCriteria, TResult, TModel, TEntity}.SearchAsync"/> is not overridden
    /// in a way that changes which records are returned). Otherwise stay on
    /// <see cref="SearchService{TCriteria, TResult, TModel, TEntity}"/>, or override the "search all" members
    /// with an implementation consistent with the custom search.
    /// </summary>
    /// <typeparam name="TCriteria">Search criteria type (a descendant of <see cref="SearchCriteriaBase"/>)</typeparam>
    /// <typeparam name="TResult">Search result (<see cref="GenericSearchResult{TModel}"/>)</typeparam>
    /// <typeparam name="TModel">The type of service layer model</typeparam>
    /// <typeparam name="TEntity">The type of data access layer entity (EF) </typeparam>
    public abstract class ExtendedSearchService<TCriteria, TResult, TModel, TEntity>
        : SearchService<TCriteria, TResult, TModel, TEntity>, IExtendedSearchService<TCriteria, TResult, TModel>
        where TCriteria : SearchCriteriaBase
        where TResult : GenericSearchResult<TModel>
        where TModel : IEntity, ICloneable
        where TEntity : class, IEntity, IDataEntity<TEntity, TModel>
    {
        protected ExtendedSearchService(
            Func<IRepository> repositoryFactory,
            IPlatformMemoryCache platformMemoryCache,
            ICrudService<TModel> crudService,
            IOptions<CrudOptions> crudOptions)
            : base(repositoryFactory, platformMemoryCache, crudService, crudOptions)
        {
        }
    }
}
