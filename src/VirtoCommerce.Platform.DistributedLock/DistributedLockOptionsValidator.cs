#nullable enable

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Options;

namespace VirtoCommerce.Platform.DistributedLock;

/// <summary>
/// Rejects <see cref="DistributedLockOptions"/> values that would make locks spin, throw on every wait, or never expire.
/// </summary>
public sealed class DistributedLockOptionsValidator : IValidateOptions<DistributedLockOptions>
{
    public ValidateOptionsResult Validate(string? name, DistributedLockOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var failures = new List<string>();

        if (options.DefaultTimeout < TimeSpan.Zero && options.DefaultTimeout != Timeout.InfiniteTimeSpan)
        {
            failures.Add($"DistributedLock:DefaultTimeout must be non-negative or {Timeout.InfiniteTimeSpan}; got {options.DefaultTimeout}.");
        }

        if (options.Expiry <= TimeSpan.Zero)
        {
            failures.Add($"DistributedLock:Expiry must be positive; got {options.Expiry}.");
        }

        if (options.StartupExpiry <= TimeSpan.Zero)
        {
            failures.Add($"DistributedLock:StartupExpiry must be positive; got {options.StartupExpiry}.");
        }

        if (options.RetryInterval <= TimeSpan.Zero)
        {
            failures.Add($"DistributedLock:RetryInterval must be positive; got {options.RetryInterval}.");
        }

        if (options.MaxRetryInterval < options.RetryInterval)
        {
            failures.Add($"DistributedLock:MaxRetryInterval ({options.MaxRetryInterval}) must not be less than DistributedLock:RetryInterval ({options.RetryInterval}).");
        }

        if (options.WaitTime < 0)
        {
            failures.Add($"DistributedLock:WaitTime must be non-negative; got {options.WaitTime}.");
        }

        return failures.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(failures);
    }
}
