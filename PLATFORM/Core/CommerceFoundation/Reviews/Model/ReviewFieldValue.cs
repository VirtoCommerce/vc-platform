using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;


namespace VirtoCommerce.Foundation.Reviews.Model
{
    [DataContract]
	[EntitySet("ReviewFieldValues")]
    [DataServiceKey("ReviewFieldValueId")]
	public class ReviewFieldValue : StorageEntity
    {

        public ReviewFieldValue()
        {
            ReviewFieldValueId = GenerateNewKey();
        }

		private string _reviewFieldValueId;
        [Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string ReviewFieldValueId
        {
            get
            {
				return _reviewFieldValueId;
            }
            set
            {
				_reviewFieldValueId = value;
				OnPropertyChanged("ReviewFieldValueId");
            }
        }

		private string _reviewId;
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string ReviewId
        {
            get
            {
				return _reviewId;
            }
            set
            {
				_reviewId = value;
				OnPropertyChanged("ReviewId");
            }
        }

		private string _name;
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string Name
        {
            get
            {
				return _name;
            }
            set
            {
				_name = value;
				OnPropertyChanged("Name");
            }
        }

		private string _value;
		[DataMember]
        [StringLength(1024, ErrorMessage = "Only 1024 characters allowed.")]
        public string Value
        {
            get
            {
				return _value;
            }
            set
            {
				_value = value;
				OnPropertyChanged("Value");
            }
        }

		[DataMember]
        [Parent]
        [ForeignKey("ReviewId")]
		public Review Review
		{
			get;
			set;
		}
    }
}
