using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Reviews.Model.Management
{
	[DataContract]
	[EntitySet("ReviewFieldSchema")]
	[DataServiceKey("ReviewFieldSchemaId")]
	public class ReviewFieldSchema : StorageEntity
    {
		public ReviewFieldSchema()
		{
			_ReviewFieldSchemaId = GenerateNewKey();
		}

		private string _ReviewFieldSchemaId;
        [Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string ReviewFieldSchemaId
        {
			get
			{
				return _ReviewFieldSchemaId;
			}
			set
			{
				SetValue(ref _ReviewFieldSchemaId, () => this.ReviewFieldSchemaId, value);
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
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
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

		private int _FieldType;
		[DataMember]
        public int FieldType
        {
			get
			{
				return _FieldType;
			}
			set
			{
				SetValue(ref _FieldType, () => this.FieldType, value);
			}
        } // enum: Raiting, Text, MultilineText, Choice

		private bool _Required;
		[DataMember]
        public bool Required
        {
			get
			{
				return _Required;
			}
			set
			{
				SetValue(ref _Required, () => this.Required, value);
			}
        }

		private string _Choices;
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string Choices
        {
			get
			{
				return _Choices;
			}
			set
			{
				SetValue(ref _Choices, () => this.Choices, value);
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

		private int _MaxLength;
		[DataMember]
        public int MaxLength
        {
			get
			{
				return _MaxLength;
			}
			set
			{
				SetValue(ref _MaxLength, () => this.MaxLength, value);
			}
        }

		#region Navigation Properties

		private string _ReviewSchemaId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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

		[DataMember]
		public virtual ReviewSchema ReviewSchema { get; set; }
		#endregion
    }
}
