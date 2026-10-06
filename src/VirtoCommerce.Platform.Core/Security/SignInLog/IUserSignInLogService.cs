using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security.SignInLog;

public interface IUserSignInLogService
{
    /// <summary>Insert a batch of audit rows.</summary>
    Task SaveChanges(IList<UserSignInLog> records, CancellationToken cancellationToken = default);

    /// <summary>Delete rows created before <paramref name="cutoff"/>. Returns the number deleted.</summary>
    Task<int> DeleteOlderThan(DateTime cutoff, int batchSize, CancellationToken cancellationToken = default);
}
