using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Tests.Helpers
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
