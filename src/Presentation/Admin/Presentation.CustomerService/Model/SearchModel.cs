using Microsoft.Practices.Prism.Commands;
using System.Collections.Generic;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Customers.Model.Enumerations;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.Model
{
    public class SearchModel
    {
        #region Dependencies

        private readonly string _id;
        private readonly SearchResultType _resultType;
        private readonly ICustomersCommonViewModel _parentViewModel;
        private readonly IViewModelsFactory<ICustomersDetailViewModel> _customersDetailVmFactory;

        #endregion

        public DelegateCommand OpenItemCommand { get; private set; }

        public SearchModel(SearchResultType resultType, string id, ICustomersCommonViewModel parentViewModel, IViewModelsFactory<ICustomersDetailViewModel> customersDetailVmFactory)
        {
            _resultType = resultType;
            _id = id;
            _parentViewModel = parentViewModel;
            _customersDetailVmFactory = customersDetailVmFactory;

            OpenItemCommand = new DelegateCommand(RaiseOpenItemInteractionRequest);
        }

        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string CaseContactId { get; set; }
        public string Type { get { return _resultType.ToString(); } }


        private void RaiseOpenItemInteractionRequest()
        {
            var parameters = new List<KeyValuePair<string, object>>();

            var customer = new Contact();
            var caseItem = new Case();
            if (_resultType == SearchResultType.Case)
            {
                caseItem.CaseId = _id;
                if (string.IsNullOrWhiteSpace(CaseContactId))
                {
                    parameters.Add(new KeyValuePair<string, object>("contactAction", ContactActionState.New));
                }
                else
                {
                    customer.MemberId = CaseContactId;
                    parameters.Add(new KeyValuePair<string, object>("contactAction", ContactActionState.Open));
                }

                parameters.Add(new KeyValuePair<string, object>("caseAction", CaseActionState.Open));
                parameters.Add(new KeyValuePair<string, object>("isContactOnlyShow", false));
            }
            else
            {
                customer.MemberId = _id;
                parameters.Add(new KeyValuePair<string, object>("contactAction", ContactActionState.Open));
                parameters.Add(new KeyValuePair<string, object>("caseAction", CaseActionState.None));
                parameters.Add(new KeyValuePair<string, object>("isContactOnlyShow", true));
            }
            parameters.Add(new KeyValuePair<string, object>("parentViewModel", _parentViewModel));
            parameters.Add(new KeyValuePair<string, object>("innerCase", caseItem));
            parameters.Add(new KeyValuePair<string, object>("innerContact", customer));

            var itemVM = _customersDetailVmFactory.GetViewModelInstance(parameters.ToArray());

            var openTracking = (IOpenTracking)itemVM;
            openTracking.OpenItemCommand.Execute();
        }
    }

    public enum SearchResultType
    {
        Case,
        Contact
    }
}
