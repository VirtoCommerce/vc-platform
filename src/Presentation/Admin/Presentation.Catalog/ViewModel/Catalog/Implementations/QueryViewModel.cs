using System;
using Microsoft.Practices.Prism.Commands;
using RequestEngine;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class QueryViewModel : ViewModelBase, IQueryViewModel
	{
		public QueryViewModel(ItemFilter item)
		{
			InnerItem = item;

			QueryHelpCommand = new DelegateCommand(RaiseQueryHelpInteractionRequest);
			QueryCheckCommand = new DelegateCommand(RaiseQueryCheckInteractionRequest);
		}

		string _MessageHeader;
		public string MessageHeader
		{
			get { return _MessageHeader; }
			set { _MessageHeader = value; OnPropertyChanged("MessageHeader"); }
		}

		string _MessageToUser;
		public string MessageText
		{
			get { return _MessageToUser; }
			private set { _MessageToUser = value; OnPropertyChanged("MessageText"); }
		}

		public DelegateCommand QueryHelpCommand { get; private set; }
		public DelegateCommand QueryCheckCommand { get; private set; }

		#region IPropertyAttributeViewModel

		public ItemFilter InnerItem { get; private set; }

		public bool Validate()
		{
			return !string.IsNullOrEmpty(InnerItem.Name) && IsQueryValid;
		}

		public linq.Expression<Func<Item, bool>> GetCompleteExpression()
		{
			// Func == Expression.Compile();
			return ExpressionBuilder.BuildExpression<Item, bool>(InnerItem.StringExpression);
		}

		#endregion

		private void RaiseQueryHelpInteractionRequest()
		{
			MessageHeader = "Help".Localize();
			MessageText = "Enter LINQ where clause for a catalog item.\nE.g., 'Catalog.Name = \"Sony\"' filters all items, which belong to a catalog named 'Sony'.".Localize();
		}

		private void RaiseQueryCheckInteractionRequest()
		{
			bool result;
			string errorText;
			try
			{
				result = ExpressionBuilder.BuildExpression<Item, bool>(InnerItem.StringExpression) != null;
				errorText = null;
			}
			catch (Exception ex)
			{
				result = false;
				errorText = ex.Message;
			}

			MessageHeader = "Filter check".Localize();
			if (result)
				MessageText = "Valid".Localize();
			else
				MessageText = string.Format("An error found:\n{0}".Localize(), errorText);
		}

		private bool IsQueryValid
		{
			get
			{
				bool result;
				try
				{
					result = ExpressionBuilder.BuildExpression<Item, bool>(InnerItem.StringExpression) != null;
				}
				catch
				{
					result = false;
				}
				return result;
			}
		}
	}
}
