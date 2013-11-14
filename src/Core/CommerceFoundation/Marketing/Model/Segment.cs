using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	[DataContract]
	[EntitySet("Segments")]
	[DataServiceKey("SegmentId")]
	public class Segment : StorageEntity
	{
		public Segment()
		{
			_SegmentId = GenerateNewKey();
		}

		private string _SegmentId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string SegmentId
		{
			get
			{
				return _SegmentId;
			}
			set
			{
				SetValue(ref _SegmentId, () => this.SegmentId, value);
			}
		}

		#region Navigation Properties

		private string _SegmentSetId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[ForeignKey("SegmentSet")]
		public string SegmentSetId
		{
			get
			{
				return _SegmentSetId;
			}
			set
			{
				SetValue(ref _SegmentSetId, () => this.SegmentSetId, value);
			}
		}

		[DataMember]
		public virtual SegmentSet SegmentSet { get; set; }
		#endregion
	}
}
