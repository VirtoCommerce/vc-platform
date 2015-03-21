using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Assets.Model
{
	[DataContract]
	[EntitySet("Folders")]
	[DataServiceKey("FolderId")]
	public class Folder : StorageEntity
	{
		private string _FolderId;
		[Key]
		[StringLength(128)]
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


		#region Navigation Properties

		private string _ParentFolderId;
		[DataMember]
		[StringLength(128)]
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

		public virtual Folder ParentFolder { get; set; }

		private ObservableCollection<Folder> _Subfolders = null;
		[DataMember]
		public virtual ObservableCollection<Folder> Subfolders
		{
			get
			{
				if (_Subfolders == null)
					_Subfolders = new ObservableCollection<Folder>();

				return _Subfolders;
			}
		}

		private ObservableCollection<FolderItem> _Items = null;
		[DataMember]
		public virtual ObservableCollection<FolderItem> FolderItems
		{
			get
			{
				if (_Items == null)
					_Items = new ObservableCollection<FolderItem>();

				return _Items;
			}
		}

		#endregion
	}
}
