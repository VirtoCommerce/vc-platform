using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using System.Collections.ObjectModel;

namespace VirtoCommerce.Foundation.Reviews.Model.Management
{
	[DataContract]
	[EntitySet("ReviewSchemas")]
	[DataServiceKey("ReviewSchemaId")]
	public class ReviewSchema : StorageEntity
    {
		public ReviewSchema()
		{
			_ReviewSchemaId = GenerateNewKey();
		}

		private string _ReviewSchemaId;
        [Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string ReviewSchemaId
        {
			get
			{
				return _ReviewSchemaId;
			}
			set
			{
				SetValue(ref _ReviewSchemaId, () => this.ReviewSchemaId, value);
			}
        }

		private string _Title;
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string Title
        {
			get
			{
				return _Title;
			}
			set
			{
				SetValue(ref _Title, () => this.Title, value);
			}
        }

		private string _Description;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string Description
        {
			get
			{
				return _Description;
			}
			set
			{
				SetValue(ref _Description, () => this.Description, value);
			}
        }

		#region Navigation Properties
		ObservableCollection<ReviewFieldSchema> _Fields = null;
		[DataMember]
		public ObservableCollection<ReviewFieldSchema> Fields
		{
			get
			{
				if (_Fields == null)
					_Fields = new ObservableCollection<ReviewFieldSchema>();

				return _Fields;
			}
		} 
		#endregion
    }
}
