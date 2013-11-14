using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.AppConfig.Model
{
	[DataContract]
	[EntitySet("Localizations")]
	[DataServiceKey("Name", "LanguageCode", "Category")] //this call generates  compiler warning CS3016: Arrays as attribute arguments is not CLS-compliant
	[CLSCompliant(false)]
	public class Localization : StorageEntity
	{
		public Localization()
		{
			Category = string.Empty;
		}

		private string _name;
		[Key, Column(Order = 0)]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string Name
		{
			get { return _name; }
			set { SetValue(ref _name, () => Name, value); }
		}

		private string _languageCode;
		[Key, Column(Order = 1)]
		[StringLength(5, ErrorMessage = "Only 5 characters allowed.")]
		[DataMember]
		public string LanguageCode
		{
			get { return _languageCode; }
			set { SetValue(ref _languageCode, () => LanguageCode, value); }
		}

		private string _category;
		[Key, Column(Order = 2)]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string Category
		{
			get { return _category; }
			set { SetValue(ref _category, () => Category, value); }
		}

		private string _value;
		[DataMember]
		[Required]
		public string Value
		{
			get { return _value; }
			set { SetValue(ref _value, () => Value, value); }
		}
	}
}
