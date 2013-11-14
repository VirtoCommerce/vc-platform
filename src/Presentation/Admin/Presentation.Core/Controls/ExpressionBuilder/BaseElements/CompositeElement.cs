using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Linq;
using System.Collections.Generic;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
	[Serializable]
	public class CompositeElement : ExpressionElement
	{
		public bool IsAllowChildren
        {
            get
            {
                return AvailableChildrenGetters.Any();
            }
        }
		
        private ObservableCollection<ExpressionElement> _headerElements;
        public ObservableCollection<ExpressionElement> HeaderElements
        {
            get { return _headerElements ?? (_headerElements = new ObservableCollection<ExpressionElement>()); }
        }
		
        private ObservableCollection<ExpressionElement> _children;
        public ObservableCollection<ExpressionElement> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new ObservableCollection<ExpressionElement>();
                    _children.CollectionChanged += Children_CollectionChanged;
                }
                return _children;
            }
        }



        /// <summary>
        /// label for adding a new child
        /// </summary>
        string _newChildLabel;
        public string NewChildLabel
        {
            get { return _newChildLabel; }
            set { _newChildLabel = value; OnPropertyChanged("NewChildLabel"); }
        }

		string _errorMessage;
		public string ErrorMessage
		{
			get { return _errorMessage; }
			set { _errorMessage = value; OnPropertyChanged("ErrorMessage"); }
		}

		private bool _isChildrenRequired;
		public bool IsChildrenRequired
		{
			get { return _isChildrenRequired; }
			set { _isChildrenRequired = value; }
		}

        public ExpressionElement[] AvailableChildren
        {
            get
            {
                return AvailableChildrenGetters.Select(x => x()).ToArray();
            }
        }
        [NonSerialized]
        private List<Func<ExpressionElement>> _availableChildrenGetters;
        public List<Func<ExpressionElement>> AvailableChildrenGetters
        {
            get { return _availableChildrenGetters ?? (_availableChildrenGetters = new List<Func<ExpressionElement>>()); }
        }

        [NonSerialized]
        private DelegateCommand<ExpressionElement> _deleteOperatorCommand;
        public DelegateCommand<ExpressionElement> DeleteOperatorCommand
        {
            get { return _deleteOperatorCommand ?? (_deleteOperatorCommand = new DelegateCommand<ExpressionElement>(RaiseDeleteOperatorInteractionRequest)); }
        }
		
        #region ICloneable Members

        public override object Clone()
        {
            var cloned = (CompositeElement)MemberwiseClone();

            if (IsAllowChildren)
            {
                cloned._children = null;
                Children.ToList().ForEach(x => cloned.Children.Add((ExpressionElement)x.Clone()));

                cloned._deleteOperatorCommand = null;
            }

            cloned._headerElements = null;
            HeaderElements.ToList().ForEach(x => cloned.HeaderElements.Add((ExpressionElement)x.Clone()));

            return cloned;
        }

        #endregion

        #region ExpressionElement overrides

        public override bool Validate()
        {
	        var result = true; //HeaderElements.All(x => x.Validate());
            if (result && IsAllowChildren)
            {
				result = (IsChildrenRequired && Children.Any()) || !IsChildrenRequired;
                result = result && Children.All(x => x.Validate());
            }
			
            return result;
        }

        #endregion

        #region private members

        private void RaiseDeleteOperatorInteractionRequest(ExpressionElement item)
        {
            Children.Remove(item);
	        IsValid = Validate();
        }

        private void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IsValid = Validate();
        }
        #endregion
    }

}
