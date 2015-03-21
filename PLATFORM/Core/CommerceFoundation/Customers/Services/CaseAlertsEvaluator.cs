using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ExpressionSerialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Customers.Model;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Customers.Factories;
using System.IO;
using VirtoCommerce.Foundation.Frameworks.Common;


namespace VirtoCommerce.Foundation.Customers.Services
{
	public class CaseAlertsEvaluator : ICaseAlertsEvaluator
	{
		#region Private Variables
		private ICustomerRepository _repository;
		//private IEvaluationPolicy[] _policies;

		#endregion

		#region .ctor
		public CaseAlertsEvaluator(ICustomerRepository repository)
		{
			_repository = repository;
			//_policies = policies;

		}

		//public CaseAlertsEvaluator(ICustomerRepository repository, IEvaluationPolicy[] policies)
		//{
		//	_repository = repository;
		//	_policies = policies;

		//}
		#endregion

		#region Properties
		private static ExpressionSerializer _expressionSerializer;
		[CLSCompliant(false)]
		public static ExpressionSerializer ExpressionSerializer
		{
			get
			{
				if (_expressionSerializer == null)
				{
					var typeResolver = new TypeResolver(assemblies: AppDomain.CurrentDomain.GetAssemblies(), knownTypes: null);
					_expressionSerializer = new ExpressionSerializer(typeResolver, null);
				}
				return _expressionSerializer;

			}
		}
		#endregion

		#region ICaseAlertsEvaluator Members

		public CaseAlert[] Evaluate(ICaseAlertEvaluationContext context)
		{
			CaseAlert[] retVal = null;

			//if (!(context.ContextObject is Dictionary<string, object>))
			//	throw new ArgumentException("context.ContextObject must be a Dictionary");

			var contextCase = context.CurrentCase;

			if (contextCase != null)
			{
				var query = GetCaseRules();

				if (query != null)
				{
					query = query.Where(rule => rule.Status == (int)CaseRuleStatus.Active);

					// sort content by type and priority
					query = query.OrderByDescending(rule => rule.Priority);

					//Evaluate query
					var current = query.ToArray();

					//Evaluate condition expression
					Func<CaseRule, bool> conditionPredicate = (x) =>
					{
						var condition = DeserializeExpression<Func<IEvaluationContext, bool>>(x.PredicateSerialized);
						return condition(context);
					};

					current = current.Where(x => string.IsNullOrEmpty(x.PredicateSerialized) || conditionPredicate(x)).ToArray();

					var list = new List<CaseAlert>();

					var serializedCase = SerializeCase(contextCase);

					current.ToList().ForEach(x => 
						{
							x.Alerts.ToList().ForEach(alert =>
								{
									if (!String.IsNullOrEmpty(alert.XslTemplate))
									{
										alert.HtmlBody = TransformXML.TransformXml(serializedCase, alert.XslTemplate);
									}
									if (!string.IsNullOrEmpty(alert.RedirectUrl))
									{
										alert.HtmlBody = alert.RedirectUrl.
											Replace(ContextRedirectVariables.CaseId, contextCase.CaseId).
											Replace(ContextRedirectVariables.ContactId, contextCase.ContactId);
									}
									list.Add(alert);
								});
						});

					if (list.Count > 0)
						retVal = list.ToArray();
				}
			}

			return retVal;
		}

		#endregion

		#region private methods

		private static T DeserializeExpression<T>(string expression)
		{
			var typeResolver = new TypeResolver(assemblies: AppDomain.CurrentDomain.GetAssemblies(), knownTypes: null);
			var expressionSerializer = new ExpressionSerializer(typeResolver, null);

			var xElement = XElement.Parse(expression);
			var conditionExpression = ExpressionSerializer.Deserialize<T>(xElement);
			var retVal = conditionExpression.Compile();
			return retVal;
		}

		private IQueryable<CaseRule> GetCaseRules()
		{
			var query = _repository.CaseRules.ExpandAll();
			return query.AsQueryable();
		}

		private static string SerializeCase(object obj)
		{
			DataContractSerializer serializer = new DataContractSerializer(obj.GetType(),
				(new CustomerEntityFactory() as IKnownSerializationTypes).GetKnownTypes(),
				maxItemsInObjectGraph: 0x7FFF,
				ignoreExtensionDataObject: false,
				preserveObjectReferences: true,
				dataContractSurrogate: null);
			MemoryStream memoryStream = new MemoryStream();
			serializer.WriteObject(memoryStream, obj);
			return Encoding.UTF8.GetString(memoryStream.GetBuffer());
		}

		#endregion
	} //class
} //namespace
