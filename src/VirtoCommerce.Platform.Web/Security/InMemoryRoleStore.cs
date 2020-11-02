using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32.SafeHandles;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Web.Security
{
    public class InMemoryRoleStore<TRole> : IQueryableRoleStore<TRole>, IRoleStore<TRole>
      where TRole : Role, new()
    {
        private readonly ICollection<CancellationToken> _cancellationTokens;
        private readonly ICollection<TRole> _roles;

        public InMemoryRoleStore()
        {
            _roles = new List<TRole>();
            _cancellationTokens = new List<CancellationToken>();
        }

        public virtual IQueryable<TRole> Roles => _roles.AsQueryable();

        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null) throw new ArgumentNullException(nameof(role));

            await Task.Run(() => { _roles.Add(role); }, cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null) throw new ArgumentNullException(nameof(role));
            var collectionRole = _roles.FirstOrDefault(x => x.Id.EqualsInvariant(role.Id));

            await Task.Run(() => { collectionRole = role; }, cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null) throw new ArgumentNullException(nameof(role));

            await Task.Run(() => { _roles.Remove(role); }, cancellationToken);

            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null) throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null) throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null) throw new ArgumentNullException(nameof(role));
            role.Name = roleName ?? throw new ArgumentNullException(nameof(roleName));

            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null) throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null) throw new ArgumentNullException(nameof(role));
            role.NormalizedName = normalizedName ?? throw new ArgumentNullException(nameof(normalizedName));

            return Task.CompletedTask;
        }

        public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return Task.FromResult(_roles.FirstOrDefault(u => u.Id == roleId));
        }

        public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var query = _roles.FirstOrDefault(r => r.NormalizedName == normalizedRoleName);

            return Task.FromResult(query);
        }

        public Task SaveChanges(
           CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            _cancellationTokens.Add(cancellationToken);
            return Task.FromResult(cancellationToken);
        }

        public virtual Task CreateAsync(TRole role)
        {
            _roles.Add(role);
            return Task.CompletedTask;
        }

        public virtual Task UpdateAsync(TRole role)
        {
            var collectionRole = _roles.FirstOrDefault(x => x.Id.EqualsInvariant(role.Id));
            collectionRole = role;
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(TRole role) => Task.FromResult(_roles.Remove(role));

        public virtual Task<TRole> FindByIdAsync(string roleId)
        {
            return Task.FromResult(_roles.FirstOrDefault(r => r.Id == roleId));
        }

        public virtual Task<TRole> FindByNameAsync(string roleName)
        {
            return Task.FromResult(_roles.FirstOrDefault(r => r.Name == roleName));
        }

        #region IDisposable

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);
        }

        private bool _disposed;
        private readonly SafeHandle _handle = new SafeFileHandle(IntPtr.Zero, true);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _handle.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}

