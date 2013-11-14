using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.AppConfig.Model
{


    [DataContract]
    [EntitySet("DisplayTemplateMappings")]
    [DataServiceKey("DisplayTemplateMappingId")]
    public class DisplayTemplateMapping : StorageEntity 
    {
		public DisplayTemplateMapping()
        {
			DisplayTemplateMappingId = GenerateNewKey();
        }


		private string _displayTemplateMappingId;
        [Key]
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string DisplayTemplateMappingId
        {
			get { return _displayTemplateMappingId; }
            set
            {
				SetValue(ref _displayTemplateMappingId, () => DisplayTemplateMappingId, value);
            }
        }

		private string _name;
		[DataMember]
		[StringLength(128)]
		public string Name
		{
			get { return _name; }
			set { SetValue(ref _name, () => Name, value); }
		}

		private string _description;
		[DataMember]
		[StringLength(256)]
		public string Description
		{
			get { return _description; }
			set { SetValue(ref _description, () => Description, value); }
		}
		
		private int _targetType;

		/// <summary>
		/// available values in TargetTypes enumeration
		/// </summary>
        [DataMember]
        public int TargetType
        {
			get { return _targetType; }
            set
            {
				SetValue(ref _targetType, () => TargetType, value);
            }
        }

		
		private string _displayTemplate;
        [DataMember, Required, StringLength(512)]
        public string DisplayTemplate
        {
			get { return _displayTemplate; }
            set
            {
				SetValue(ref _displayTemplate, () => DisplayTemplate, value);
            }
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

		private string _PredicateSerialized;
		[DataMember]
		public string PredicateSerialized
		{
			get
			{
				return _PredicateSerialized;
			}
			set
			{
				SetValue(ref _PredicateSerialized, () => this.PredicateSerialized, value);
			}
		}

		private string _PredicateVisualTreeSerialized;
		[DataMember]
		public string PredicateVisualTreeSerialized
		{
			get
			{
				return _PredicateVisualTreeSerialized;
			}
			set
			{
				SetValue(ref _PredicateVisualTreeSerialized, () => this.PredicateVisualTreeSerialized, value);
			}
		}
    }
}
