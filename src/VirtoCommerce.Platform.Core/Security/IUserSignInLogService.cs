using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security;

public interface IUserSignInLogService
{
    /// <summary>Insert a batch of audit rows.</summary>
    Task SaveChangesAsync(IList<UserSignInLog> records);

    /// <summary>Delete rows created before <paramref name="cutoff"/>. Returns the number deleted.</summary>
    Task<int> DeleteOlderThanAsync(DateTime cutoff, int batchSize);
}
