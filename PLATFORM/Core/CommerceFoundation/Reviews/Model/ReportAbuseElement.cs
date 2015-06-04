using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Foundation.Reviews.Model
{
	[DataContract]
	[EntitySet("ReportAbuseElements")]
	public class ReportAbuseElement : ReportElementBase
	{
        private string _Comment;
        [DataMember]
        [StringLength(1024, ErrorMessage = "Only 1024 characters allowed.")]
        public string Comment
        {
            get
            {
                return _Comment;
            }
            set
            {
                SetValue(ref _Comment, () => this.Comment, value);
            }
        }

        private string _Email;
        [DataMember]
        [Required]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                SetValue(ref _Email, () => this.Email, value);
            }
        }

        private string _Reason;
        [DataMember]
        [Required]
        [StringLength(16, ErrorMessage = "Only 128 characters allowed.")]
        public string Reason
        {
            get
            {
                return _Reason;
            }
            set
            {
                SetValue(ref _Reason, () => this.Reason, value);
            }
        }
	
	}
}
