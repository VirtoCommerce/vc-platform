using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Infrastructure;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Security
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private readonly TimeSpan _refreshTokenLifeTime;
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshTokenProvider(TimeSpan refreshTokenLifeTime, IRefreshTokenService refreshTokenService)
        {
            _refreshTokenLifeTime = refreshTokenLifeTime;
            _refreshTokenService = refreshTokenService;
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            CreateAsync(context).RunSynchronously();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var refreshTokenId = Guid.NewGuid().ToString("n");
            var now = DateTime.UtcNow;
            var token = new RefreshToken
            {
                Id = refreshTokenId.GetHash<SHA256CryptoServiceProvider>(),
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = now,
                ExpiresUtc = now + _refreshTokenLifeTime
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            await _refreshTokenService.AddAsync(token);
            context.SetToken(refreshTokenId);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            ReceiveAsync(context).RunSynchronously();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var hashedTokenId = context.Token.GetHash<SHA256CryptoServiceProvider>();

            var refreshToken = await _refreshTokenService.GetByIdAsync(hashedTokenId);
            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                await _refreshTokenService.DeleteAsync(new[] { hashedTokenId });
            }
        }
    }
}
