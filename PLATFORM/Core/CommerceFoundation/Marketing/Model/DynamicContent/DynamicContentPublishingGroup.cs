using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Marketing.Model.DynamicContent
{
	[DataContract]
	[EntitySet("DynamicContentPublishingGroups")]
	[DataServiceKey("DynamicContentPublishingGroupId")]
	public class DynamicContentPublishingGroup : StorageEntity
	{
		public DynamicContentPublishingGroup()
		{
			_dynamicContentPublishingGroupId = GenerateNewKey();
		}

		private string _dynamicContentPublishingGroupId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string DynamicContentPublishingGroupId
		{
			get { return _dynamicContentPublishingGroupId; }
			set { SetValue(ref _dynamicContentPublishingGroupId, () => DynamicContentPublishingGroupId, value); }
		}

		private string _name;
		[DataMember]
		[Required(ErrorMessage = "Field 'Name' is required.")]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Name
		{
			get { return _name; }
			set { SetValue(ref _name, () => Name, value); }
		}

		private string _description;
		[DataMember]
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
		public string Description
		{
			get { return _description; }
			set { SetValue(ref _description, () => Description, value); }
		}

		private int _priority;
		[DataMember]
		public int Priority
		{
			get { return _priority; }
			set { SetValue(ref _priority, () => Priority, value); }
		}

		private bool _isActive;
		[DataMember]
		public bool IsActive
		{
			get { return _isActive; }
			set { SetValue(ref _isActive, () => IsActive, value); }
		}

		private DateTime? _startDate;
		[DataMember]
		public DateTime? StartDate
		{
			get { return _startDate; }
			set { SetValue(ref _startDate, () => StartDate, value); }
		}

		private DateTime? _endDate;
		[DataMember]
		public DateTime? EndDate
		{
			get { return _endDate; }
			set { SetValue(ref _endDate, () => EndDate, value); }
		}

		private string _conditionExpression;
		[DataMember]
		public string ConditionExpression
		{
			get { return _conditionExpression; }
			set { SetValue(ref _conditionExpression, () => ConditionExpression, value); }
		}

		private string _predicateVisualTreeSerialized;
		[DataMember]
		public string PredicateVisualTreeSerialized
		{
			get { return _predicateVisualTreeSerialized; }
			set { SetValue(ref _predicateVisualTreeSerialized, () => PredicateVisualTreeSerialized, value); }
		}

		#region Navigation Properties

		ObservableCollection<PublishingGroupContentItem> _contentItems = null;
		[DataMember]
		public virtual ObservableCollection<PublishingGroupContentItem> ContentItems
		{
			get
			{
				if (_contentItems == null)
					_contentItems = new ObservableCollection<PublishingGroupContentItem>();

				return _contentItems;
			}
			set
			{
				_contentItems = value;
			}
		}

		ObservableCollection<PublishingGroupContentPlace> _contentPlaces = null;
		[DataMember]
		public virtual ObservableCollection<PublishingGroupContentPlace> ContentPlaces
		{
			get
			{
				if (_contentPlaces == null)
					_contentPlaces = new ObservableCollection<PublishingGroupContentPlace>();

				return _contentPlaces;
			}
			set
			{
				_contentPlaces = value;
			}
		}
		#endregion
	}
}
