using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Security.Services;

public class UserSignInLogService : IUserSignInLogService
{
    private readonly Func<ISecurityRepository> _repositoryFactory;

    public UserSignInLogService(Func<ISecurityRepository> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public virtual async Task SaveChangesAsync(IList<UserSignInLog> records)
    {
        if (records is null || records.Count == 0)
        {
            return;
        }

        var pkMap = new PrimaryKeyResolvingMap();

        using var repository = _repositoryFactory();

        foreach (var record in records)
        {
            repository.Add(AbstractTypeFactory<UserSignInLogEntity>.TryCreateInstance().FromModel(record, pkMap));
        }

        await repository.UnitOfWork.CommitAsync();
    }

    public virtual async Task<int> DeleteOlderThanAsync(DateTime cutoff, int batchSize)
    {
        using var repository = _repositoryFactory();

        var expired = await repository.UserSignInLogs
            .Where(x => x.CreatedDate < cutoff)
            .OrderBy(x => x.CreatedDate)
            .Take(batchSize)
            .ToListAsync();

        if (expired.Count == 0)
        {
            return 0;
        }

        foreach (var entity in expired)
        {
            repository.Remove(entity);
        }

        await repository.UnitOfWork.CommitAsync();

        return expired.Count;
    }
}
