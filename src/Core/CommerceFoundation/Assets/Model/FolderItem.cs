using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Assets.Model
{
	[DataContract]
	[EntitySet("FolderItems")]
	[DataServiceKey("FolderItemId")]
	public class FolderItem : StorageEntity
	{
		private string _FolderItemId;
		[StringLength(1024)]
		[DataMember]
		public string FolderItemId
		{
			get
			{
				return _FolderItemId;
			}
			set
			{
				SetValue(ref _FolderItemId, () => this.FolderItemId, value);
			}
		}


		private string _BlobKey;
		[DataMember]
		[StringLength(1024)]
		public string BlobKey
		{
			get
			{
				return _BlobKey;
			}
			set
			{
				SetValue(ref _BlobKey, () => this.BlobKey, value);
			}
		}

		private string _Name;
		[StringLength(256)]
		[DataMember]
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

		private string _ContentLanguage;
		[StringLength(128)]
		[DataMember]
		public string ContentLanguage
		{
			get
			{
				return _ContentLanguage;
			}
			set
			{
				SetValue(ref _ContentLanguage, () => this.ContentLanguage, value);
			}
		}

		private string _ContentEncoding;
		[StringLength(128)]
		[DataMember]
		public string ContentEncoding
		{
			get
			{
				return _ContentEncoding;
			}
			set
			{
				SetValue(ref _ContentEncoding, () => this.ContentEncoding, value);
			}
		}

		private string _ContentType;
		[StringLength(128)]
		[DataMember]
		public string ContentType
		{
			get
			{
				return _ContentType;
			}
			set
			{
				SetValue(ref _ContentType, () => this.ContentType, value);
			}
		}

		private string _SmallData;
		[DataMember]
		public byte[] SmallData
		{
			get
			{
				byte[] retVal = null;
				var base64 = _SmallData;
				if (!string.IsNullOrEmpty(base64))
				{
					retVal = Convert.FromBase64String(base64);
				}
				return retVal;

			}
			set
			{
				string newValue = null;
				if (value != null)
				{
					newValue = Convert.ToBase64String(value);
				}
				SetValue(ref _SmallData, () => this.SmallData, newValue);
			}
		}

		private long _ContentLength;
		[DataMember]
		public long ContentLength
		{
			get
			{
				return _ContentLength;
			}
			set
			{
				SetValue(ref _ContentLength, () => this.ContentLength, value);
			}
		}

		#region Navigation Properties

		private string _FolderId;
		[ForeignKey("Folder")]
		[StringLength(512)]
		[DataMember]
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
		public virtual Folder Folder { get; set; }
		#endregion
	}
}
