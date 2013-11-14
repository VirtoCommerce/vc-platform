using System;
using System.ComponentModel;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
	[Serializable]
	public class ExpressionElement : INotifyPropertyChanged, ICloneable
	{
		/// <summary>
		/// Display Name for choosing this item in menu
		/// </summary>
		public string DisplayName { get; set; }

		bool _isValid;
		public bool IsValid
		{
			get { return _isValid; }
			set { if (_isValid != value) { _isValid = value; OnPropertyChanged("IsValid"); } }
		}

		protected ExpressionElement()
		{
			PropertyChanged += ExpressionElement_PropertyChanged;
		}

		void ExpressionElement_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			IsValid = Validate();
		}

		public virtual bool Validate()
		{
			return true;
		}

		#region INotifyPropertyChanged Members
		[NonSerialized]
		private PropertyChangedEventHandler _propertyChanged;

		public event PropertyChangedEventHandler PropertyChanged
		{
			add { _propertyChanged += value; }
			remove { if (_propertyChanged != null) _propertyChanged -= value; }
		}

		protected void OnPropertyChanged(string propertyName)
		{
			var handler = _propertyChanged;
			if (handler != null)
			{
				var e = new PropertyChangedEventArgs(propertyName);
				handler(this, e);
			}
		}

		#endregion

		#region ICloneable Members

		public virtual object Clone()
		{
			PropertyChanged -= ExpressionElement_PropertyChanged;
			var cloned = (ExpressionElement)MemberwiseClone();
			cloned.PropertyChanged += cloned.ExpressionElement_PropertyChanged;
			return cloned;
		}

		#endregion
	}

}
