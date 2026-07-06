using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.GenericCrud;

/// <summary>
/// A search service that, in addition to the paged <see cref="ISearchService{TCriteria, TResult, TModel}.SearchAsync"/>,
/// can enumerate all data matching the criteria in an optimized way (e.g. a single identifiers query
/// without a count query and without offset paging, followed by loading models by identifiers in batches).
/// <para>
/// Enabling the optimized path is opt-in. A service built on the generic <c>SearchService</c> enables it by
/// deriving from <c>ExtendedSearchService</c> (which implements this interface) instead — appropriate only when
/// the result membership is fully defined by the service's query building. A service that implements
/// <c>ISearchService</c> directly enables it by declaring this interface and providing its own members. Either way,
/// a service whose <c>SearchAsync</c> uses non-standard search logic must implement these members consistently with it.
/// </para>
/// <para>
/// Versioning policy: any member added to this interface in the future must be declared with a default
/// implementation expressed via the existing members, so that existing implementations keep compiling and loading.
/// </para>
/// </summary>
/// <typeparam name="TCriteria">Search criteria type (a descendant of <see cref="SearchCriteriaBase"/>)</typeparam>
/// <typeparam name="TResult">Search result (<see cref="GenericSearchResult{TModel}"/>)</typeparam>
/// <typeparam name="TModel">The type of service layer model</typeparam>
public interface IExtendedSearchService<TCriteria, TResult, TModel> : ISearchService<TCriteria, TResult, TModel>
    where TCriteria : SearchCriteriaBase
    where TResult : GenericSearchResult<TModel>
    where TModel : IEntity
{
    /// <summary>
    /// Returns all models matching the criteria as a single list.
    /// Intended for reading a complete, reasonably sized data set into memory
    /// (e.g. all organizations the user is assigned to).
    /// <see cref="SearchCriteriaBase.Skip"/> and <see cref="SearchCriteriaBase.Take"/> are ignored.
    /// </summary>
    /// <param name="criteria">Search criteria.</param>
    /// <param name="clone">If false, returns data from the cache without cloning. This consumes less memory, but the returned data must not be modified.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IList<TModel>> SearchAllAsync(TCriteria criteria, bool clone = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns identifiers of all records matching the criteria with a single query,
    /// without a count query and without offset paging, ordered by the criteria sort expression.
    /// <see cref="SearchCriteriaBase.Skip"/> and <see cref="SearchCriteriaBase.Take"/> are ignored.
    /// </summary>
    /// <param name="criteria">Search criteria.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IList<string>> SearchAllIdsAsync(TCriteria criteria, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the total number of records matching the criteria with a single count query.
    /// <see cref="SearchCriteriaBase.Skip"/> and <see cref="SearchCriteriaBase.Take"/> are ignored.
    /// </summary>
    /// <param name="criteria">Search criteria.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<int> CountAllAsync(TCriteria criteria, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns true if at least one record matches the criteria, using a database-level exists check
    /// rather than a count.
    /// <see cref="SearchCriteriaBase.Skip"/> and <see cref="SearchCriteriaBase.Take"/> are ignored.
    /// </summary>
    /// <param name="criteria">Search criteria.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<bool> ExistsAsync(TCriteria criteria, CancellationToken cancellationToken = default);
}
