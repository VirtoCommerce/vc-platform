using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks.Logging.Factories
{
    public class LogOperationFactory : FactoryBase, ILogOperationFactory
    {
        public LogOperationFactory()
		{
            RegisterStorageType(typeof(LogOperation), "LogOperation");
		}
    }
}
