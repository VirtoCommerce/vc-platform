using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	[DataContract]
	[EntitySet("SegmentSets")]
	[DataServiceKey("SegmentSetId")]
	public class SegmentSet : StorageEntity
	{
		public SegmentSet()
		{
			_SegmentSetId = GenerateNewKey();
		}

		private string _SegmentSetId;
		[Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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

		private string _Name;
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

		#region Navigation Properties

		private ObservableCollection<Segment> _segments;
		[DataMember]
		public ObservableCollection<Segment> Segments
		{
			get
			{
				if (_segments == null)
					_segments = new ObservableCollection<Segment>();

				return _segments;
			}
		}

		#endregion
	}
}
