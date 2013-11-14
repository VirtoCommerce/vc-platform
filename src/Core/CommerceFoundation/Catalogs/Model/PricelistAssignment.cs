using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[DataServiceKey("PricelistAssignmentId")]
	[EntitySet("PricelistAssignments")]
	public class PricelistAssignment : StorageEntity
	{
		public PricelistAssignment()
		{
			_PricelistAssignmentId = GenerateNewKey();
		}

		private string _PricelistAssignmentId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string PricelistAssignmentId
		{
			get
			{
				return _PricelistAssignmentId;
			}
			set
			{
				SetValue(ref _PricelistAssignmentId, () => this.PricelistAssignmentId, value);
			}
		}

		private string _Name;
		[DataMember]
		[StringLength(128)]
		[Required(ErrorMessage = "Field 'Name' is required.")]
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
		[DataMember]
		[StringLength(512)]
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

		private int _Priority;
		[DataMember]
		public int Priority
		{
			get
			{
				return _Priority;
			}
			set
			{
				SetValue(ref _Priority, () => this.Priority, value);
			}
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
		private string _PricelistId;
		[DataMember]
		[StringLength(128)]
		[Required(ErrorMessage = "Field 'Price List' is required.")]
		public string PricelistId
		{
			get
			{
				return _PricelistId;
			}
			set
			{
				SetValue(ref _PricelistId, () => this.PricelistId, value);
			}
		}

		[DataMember]
		public virtual Pricelist Pricelist { get; set; }

		private string _CatalogId;
		[DataMember]
		[StringLength(128)]
		[Required(ErrorMessage = "Field 'Catalog' is required.")]
		public string CatalogId
		{
			get
			{
				return _CatalogId;
			}
			set
			{
				SetValue(ref _CatalogId, () => this.CatalogId, value);
			}
		}

		[DataMember]
		public virtual CatalogBase Catalog { get; set; }
		#endregion

	}
}
