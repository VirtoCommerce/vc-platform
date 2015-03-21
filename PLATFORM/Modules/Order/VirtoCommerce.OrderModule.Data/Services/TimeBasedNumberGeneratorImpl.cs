using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Services;

namespace VirtoCommerce.OrderModule.Data.Services
{
	public class TimeBasedNumberGeneratorImpl : IOperationNumberGenerator
	{
		#region IOperationNumberGenerator Members

		public string GenerateNumber(Domain.Order.Model.Operation operation)
		{
			var now = DateTime.UtcNow;
			var retVal = operation.GetType().Name.Substring(0, 2).ToUpper() + now.DayOfYear.ToString("000") + now.TimeOfDay.Minutes.ToString("00");
			return retVal;
		}

		#endregion
	}
}
