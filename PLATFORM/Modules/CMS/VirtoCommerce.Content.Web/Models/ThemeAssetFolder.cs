using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	public class ThemeAssetFolder
	{
		public string FolderName { get; set; }

		private Collection<ThemeAsset> _assets;
		public Collection<ThemeAsset> Assets { get { return _assets ?? (_assets = new Collection<ThemeAsset>()); } }
	}
}