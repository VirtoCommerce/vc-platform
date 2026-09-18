using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Core.Security.SignInLog;

namespace VirtoCommerce.Platform.Security.SignInLog;

public class UserSignInLogService : IUserSignInLogService
{
    private readonly IScopedServiceFactory<ISecurityRepository> _repositoryFactory;

    public UserSignInLogService(IScopedServiceFactory<ISecurityRepository> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public virtual async Task SaveChanges(IList<UserSignInLog> records, CancellationToken cancellationToken = default)
    {
        if (records is null || records.Count == 0)
        {
            return;
        }

        var pkMap = new PrimaryKeyResolvingMap();

        using var scopedRepository = _repositoryFactory.Create();
        var repository = scopedRepository.Service;

        foreach (var record in records)
        {
            repository.Add(AbstractTypeFactory<UserSignInLogEntity>.TryCreateInstance().FromModel(record, pkMap));
        }

        await repository.UnitOfWork.CommitAsync();
    }

    /// <summary>
    /// Deletes a batch of expired rows with a single statement. Audit rows are never edited, so
    /// there is nothing to load, track or map - ExecuteDeleteAsync does the whole batch server-side.
    /// </summary>
    public virtual async Task<int> DeleteOlderThan(DateTime cutoff, int batchSize, CancellationToken cancellationToken = default)
    {
        using var scopedRepository = _repositoryFactory.Create();
        var repository = scopedRepository.Service;

        return await BuildExpiredQuery(repository, cutoff, batchSize).ExecuteDeleteAsync(cancellationToken);
    }

    /// <summary>
    /// Which rows a cleanup batch will remove. Separate from the delete itself so the selection
    /// can be tested without a database - ExecuteDeleteAsync needs a real provider.
    /// </summary>
    protected virtual IQueryable<UserSignInLogEntity> BuildExpiredQuery(ISecurityRepository repository, DateTime cutoff, int batchSize)
    {
        return repository.UserSignInLogs
            .Where(x => x.CreatedDate < cutoff)
            .OrderBy(x => x.CreatedDate)
            .Take(batchSize);
    }
}
