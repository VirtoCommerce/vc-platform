using System;
using System.Collections.ObjectModel;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.ChangeLog
{
	public static class SecurityConverters
	{
		public static OperationLog ToCoreModel(this OperationLogEntity entity)
		{
			var retVal = new OperationLog();
			retVal.InjectFrom(entity);
			retVal.OperationType = (EntryState)Enum.Parse(typeof(EntryState), entity.OperationType);
			return retVal;
		}

		public static OperationLogEntity ToDataModel(this OperationLog log)
		{
			var retVal = new OperationLogEntity();
			retVal.InjectFrom(log);
			retVal.OperationType = log.OperationType.ToString();
			return retVal;
		}

		public static void Patch(this OperationLog source, OperationLog target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjection = new PatchInjection<OperationLog>(x => x.ModifiedBy, x => x.ModifiedDate);
			target.InjectFrom(patchInjection, source);
		}

	}
}
