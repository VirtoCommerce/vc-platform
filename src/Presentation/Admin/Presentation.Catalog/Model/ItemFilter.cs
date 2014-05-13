using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VirtoCommerce.Foundation.Catalogs.Model;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.Catalog.Model
{
	public class ItemFilter : INotifyPropertyChanged
	{
		private string _Name;
		public string Name
		{
			get { return _Name; }
			set { _Name = value; OnPropertyChanged(); }
		}

		public linq.Expression<Func<Item, bool>> Expression { get; set; }
		public string StringExpression { get; set; }

		//Implementation of INotifyPropertyChanged Interface
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
