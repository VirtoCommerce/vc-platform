using System;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using System.Collections.Generic;
using VirtoCommerce.Foundation.Frameworks.Tagging;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.Foundation.Customers.Services
{ 
	public class CaseAlertsService : ICaseAlertsService
	{
		private ICustomerRepository _repository;
		private ICaseAlertsEvaluator _evaluator;

		public CaseAlertsService(ICustomerRepository repository, ICaseAlertsEvaluator evaluator)
		{
			_repository = repository;
			_evaluator = evaluator;
		}

        public CaseAlert[] GetCaseAlerts(ICaseAlertEvaluationContext context)
		{
			return _evaluator.Evaluate(context);
		}
	}
}
