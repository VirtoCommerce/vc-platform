using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CartModule.Data
{
    using VirtoCommerce.CartModule.Data.Repositories;
    using VirtoCommerce.Foundation.Data.Infrastructure;

    public class CartDatabaseInitializer : SetupDatabaseInitializer<CartRepositoryImpl, Migrations.Configuration>
    {
    }
}
