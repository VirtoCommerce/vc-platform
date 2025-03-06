using System;
using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
    /// <summary>
    /// A <see cref="DbContextWithTriggers"/> derived class with UtcDateTimeConverter for all DateTime properties. 
    /// </summary>
    public class DbContextBase : DbContextWithTriggers
    {
        public const int CultureNameLength = Length16;
        public const int IdLength = Length128;
        public const int UrlLength = Length2048;
        public const int UserNameLength = Length64;

        public const int Length16 = 16;
        public const int Length32 = 32;
        public const int Length64 = 64;
        public const int Length128 = 128;
        public const int Length256 = 256;
        public const int Length512 = 512;
        public const int Length1024 = 1024;
        public const int Length2048 = 2048;

        public DbContextBase()
        {
        }

        public DbContextBase(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        public DbContextBase(DbContextOptions options)
            : base(options)
        {
        }

        public DbContextBase(IServiceProvider serviceProvider, DbContextOptions options)
            : base(serviceProvider, options)
        {
        }


        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
        }
    }
}
