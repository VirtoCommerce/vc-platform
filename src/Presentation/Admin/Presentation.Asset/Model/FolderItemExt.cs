using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Assets.Model;
using VirtoCommerce.ManagementClient.Asset.ViewModel;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Asset.Model
{
	public class FolderItemExt : FolderItem
	{
		private string _FileSystemPath;
		public string FileSystemPath
		{
			get
			{
				return _FileSystemPath;
			}
			set
			{
				SetValue(ref _FileSystemPath, () => this.FileSystemPath, value);
			}
		}

		public IFolderItemViewModel ViewModel { get; set; }
	}
}
