using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Customers.Model
{
	[DataContract]
	[EntitySet("Labels")]
	[DataServiceKey("LabelId")]
	public class Label : StorageEntity
	{

		public Label()
		{
			_LabelId = GenerateNewKey();
		}


		private string _LabelId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string LabelId
		{
			get
			{
				return _LabelId;
			}
			set
			{
				SetValue(ref _LabelId, () => this.LabelId, value);
			}
		}


		private string _Name;
		[Required(ErrorMessage = "Label's name can't be empty")]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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


		private string _ImgUrl;
		[DataMember]
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
		public string ImgUrl
		{
			get
			{
				return _ImgUrl;
			}
			set
			{
				SetValue(ref _ImgUrl, () => this.ImgUrl, value);
			}
		}


		private string _Description;
		[Required(ErrorMessage = "Label's description can't be empty")]
		[DataMember]
		[StringLength(512, ErrorMessage = "Only 512 characters allowed.")]
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

        private string _MemberId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string MemberId
        {
            get { return _MemberId; }
            set
            {
                SetValue(ref _MemberId, () => this.MemberId, value);
            }
        }

        [DataMember]
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }



        private string _CaseId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string CaseId
        {
            get { return _CaseId; }
            set
            {
                SetValue(ref _CaseId, () => this.CaseId, value);
            }
        }

        [DataMember]
        [ForeignKey("CaseId")]
        public virtual Case Case { get; set; }
        #endregion

	}
}
