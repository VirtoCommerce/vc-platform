using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Marketing.Model.DynamicContent
{
	[DataContract]
	[EntitySet("DynamicContentFolders")]
	[DataServiceKey("DynamicContentFolderId")]
	public class DynamicContentFolder : StorageEntity
	{
		public DynamicContentFolder()
		{
			_dynamicContentFolderId = GenerateNewKey();
		}

		private string _dynamicContentFolderId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string DynamicContentFolderId
		{
			get { return _dynamicContentFolderId; }
			set { SetValue(ref _dynamicContentFolderId, () => DynamicContentFolderId, value); }
		}

		private string _name;
		[DataMember]
		[Required]
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

		private string _imageUrl;
		[DataMember]
		[StringLength(2048)]
		public string ImageUrl
		{
			get { return _imageUrl; }
			set { SetValue(ref _imageUrl, () => ImageUrl, value); }
		}

		#region Navigation Properties
		private string _ParentFolderId;
		[StringLength(128)]
		[DataMember]
		[ForeignKey("ParentFolder")]
		public string ParentFolderId
		{
			get
			{
				return _ParentFolderId;
			}
			set
			{
				SetValue(ref _ParentFolderId, () => this.ParentFolderId, value);
			}
		}

		[DataMember]
		public virtual DynamicContentFolder ParentFolder { get; set; }

		ObservableCollection<DynamicContentItem> _contentItems = null;
		[DataMember]
		public virtual ObservableCollection<DynamicContentItem> ContentItems
		{
			get
			{
				if (_contentItems == null)
					_contentItems = new ObservableCollection<DynamicContentItem>();

				return _contentItems;
			}
			set
			{
				_contentItems = value;
			}
		}

		ObservableCollection<DynamicContentFolder> _contentPlaces = null;
		[DataMember]
		public virtual ObservableCollection<DynamicContentFolder> ContentPlaces
		{
			get
			{
				if (_contentPlaces == null)
					_contentPlaces = new ObservableCollection<DynamicContentFolder>();

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
