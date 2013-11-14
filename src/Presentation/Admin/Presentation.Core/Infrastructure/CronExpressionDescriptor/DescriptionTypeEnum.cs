using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.CronExpressionDescriptor
{
	public enum DescriptionTypeEnum
	{
		FULL,
		TIMEOFDAY,
		SECONDS,
		MINUTES,
		HOURS,
		DAYOFWEEK,
		MONTH,
		DAYOFMONTH
	}
}
