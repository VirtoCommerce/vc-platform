using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Marketing.Model.DynamicContent
{
	[DataContract]
	[EntitySet("DynamicContentPlaces")]
	[DataServiceKey("DynamicContentPlaceId")]
	public class DynamicContentPlace : StorageEntity
	{
		public DynamicContentPlace()
		{
			_dynamicContentPlaceId = GenerateNewKey();
		}

		private string _dynamicContentPlaceId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string DynamicContentPlaceId
		{
			get { return _dynamicContentPlaceId; }
			set { SetValue(ref _dynamicContentPlaceId, () => DynamicContentPlaceId, value); }
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
		private string _FolderId;
		[StringLength(128)]
		[DataMember]
		[ForeignKey("Folder")]
		public string FolderId
		{
			get
			{
				return _FolderId;
			}
			set
			{
				SetValue(ref _FolderId, () => this.FolderId, value);
			}
		}

		[DataMember]
		public virtual DynamicContentFolder Folder { get; set; }
		#endregion
	}
}
