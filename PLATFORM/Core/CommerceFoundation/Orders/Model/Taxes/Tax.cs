using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Orders.Model.Taxes
{
	[DataContract]
	[EntitySet("Taxes")]
	[DataServiceKey("TaxId")]
	public class Tax : StorageEntity
	{
		public Tax()
		{
			TaxId = GenerateNewKey();
		}

		private string _TaxId;
		[DataMember]
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string TaxId
		{
			get
			{
				return _TaxId;
			}
			set
			{
				SetValue(ref _TaxId, () => this.TaxId, value);
			}
		}

		private string _Name;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [Required]
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

		private int _TaxType;
        /// <summary>
        /// Gets or sets the type of the tax. Refer to TaxTypes enum, can be either Sales or Shipping.
        /// </summary>
        /// <value>
        /// The type of the tax.
        /// </value>
		[DataMember]
        [Required]
		public int TaxType
		{
			get
			{
				return _TaxType;
			}
			set
			{
				SetValue(ref _TaxType, () => this.TaxType, value);
			}
		}

		private int _SortOrder;
		[DataMember]
        [Required]
		public int SortOrder
		{
			get
			{
				return _SortOrder;
			}
			set
			{
				SetValue(ref _SortOrder, () => this.SortOrder, value);
			}
		}

        #region Navigation Properties

        ObservableCollection<TaxValue> _values;
		[DataMember]
		public ObservableCollection<TaxValue> TaxValues
		{
			get
			{
				if (_values == null)
					_values = new ObservableCollection<TaxValue>();

				return _values;
			}
		}

        ObservableCollection<TaxLanguage> _TaxLanguages;
        [DataMember]
        public ObservableCollection<TaxLanguage> TaxLanguages
        {
            get
            {
                if (_TaxLanguages == null)
                    _TaxLanguages = new ObservableCollection<TaxLanguage>();

                return _TaxLanguages;
            }
        }

        #endregion

    }
}
