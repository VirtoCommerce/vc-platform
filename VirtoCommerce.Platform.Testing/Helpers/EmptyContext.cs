using System.Data.Common;
using System.Data.Entity;

namespace VirtoCommerce.Platform.Testing.Helpers
{
    public class EmptyContext : DbContext
    {
        static EmptyContext()
        {
            Database.SetInitializer<EmptyContext>(null);
        }

        public EmptyContext(DbConnection connection)
            : base(connection, false)
        {
        }
    }
}
