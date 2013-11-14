using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[DataServiceKey("PackageId")]
	[EntitySet("Packagings")]
	public class Packaging : StorageEntity
	{
		public Packaging()
		{
			_PackageId = GenerateNewKey();
		}

		private string _PackageId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string PackageId
		{
			get
			{
				return _PackageId;
			}
			set
			{
				SetValue(ref _PackageId, () => this.PackageId, value);
			}
		}

		private string _Name;
		[StringLength(128)]
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

		private string _Description;
		[Required]
		[StringLength(512)]
		[DataMember]
		public string Description
		{
			get
			{
				return _Description;
			}
			set
			{
				SetValue(ref _Description, () => this.Description, value);
			}
		}

		private decimal _Width;
		[DataMember]
		public decimal Width
		{
			get
			{
				return _Width;
			}
			set
			{
				SetValue(ref _Width, () => this.Width, value);
			}
		}

		private decimal _Height;
		[DataMember]
		public decimal Height
		{
			get
			{
				return _Height;
			}
			set
			{
				SetValue(ref _Height, () => this.Height, value);
			}
		}

        private decimal _Depth;
		[DataMember]
		public decimal Depth
		{
			get
			{
                return _Depth;
			}
			set
			{
                SetValue(ref _Depth, () => this.Depth, value);
			}
		}

		private int _LengthMeasure;
		/// <summary>
		/// Gets or sets the length measure. Can be inches, millimeter and so on.
		/// </summary>
		/// <value>
		/// The length measure.
		/// </value>
		[DataMember]
		public int LengthMeasure
		{
			get
			{
				return _LengthMeasure;
			}
			set
			{
				SetValue(ref _LengthMeasure, () => this.LengthMeasure, value);
			}
		}
	}
}
