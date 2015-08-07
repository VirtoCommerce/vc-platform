using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	/// <summary>
	/// Theme asset folder
	/// </summary>
	public class ThemeAssetFolder
	{
		/// <summary>
		/// Theme asset folder name
		/// </summary>
		public string FolderName { get; set; }

		private Collection<ThemeAsset> _assets;

		/// <summary>
		/// Theme assets
		/// </summary>
		public Collection<ThemeAsset> Assets { get { return _assets ?? (_assets = new Collection<ThemeAsset>()); } }
	}
}