using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Orders.Model.Countries
{
    [DataContract]
    [EntitySet("Countries")]
    [DataServiceKey("CountryId")]
	public class Country : StorageEntity
    {
        public Country()
		{
           CountryId = GenerateNewKey();
		}

		private string _CountryId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string CountryId
        {
			get
			{
				return _CountryId;
			}
			set
			{
				SetValue(ref _CountryId, () => this.CountryId, value);
			}
        }

		private bool _IsVisible;
		[DataMember]
        public bool IsVisible
        {
			get
			{
				return _IsVisible;
			}
			set
			{
				SetValue(ref _IsVisible, () => this.IsVisible, value);
			}
        }

		private int _Priority;
		[DataMember]
        public int Priority
        {
			get
			{
				return _Priority;
			}
			set
			{
				SetValue(ref _Priority, () => this.Priority, value);
			}
        }

		private string _Name;
		[DataMember]
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
        public string Name
        {
			get
			{
				return _Name;
			}
			set
			{
				SetValue(ref _Name, () => this.Name, value);
			}
        }

		private string _DisplayName;
		[DataMember]
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
        public string DisplayName
        {
			get
			{
				return _DisplayName;
			}
			set
			{
				SetValue(ref _DisplayName, () => this.DisplayName, value);
			}
        }

        ObservableCollection<Region> _Regions;
        [DataMember]
        public ObservableCollection<Region> Regions
        {
            get
            {
                if (_Regions == null)
                    _Regions = new ObservableCollection<Region>();

                return _Regions;
            }
        }
    }
}
