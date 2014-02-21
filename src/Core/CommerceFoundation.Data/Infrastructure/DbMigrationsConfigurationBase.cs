using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Data.Infrastructure
{
    public class DbMigrationsConfigurationBase<TContext> : DbMigrationsConfiguration<TContext> where TContext : System.Data.Entity.DbContext
    {
    }
}
