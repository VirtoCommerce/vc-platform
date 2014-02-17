using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Importing.Model
{
	public abstract class EntityImporterBase: IEntityImporter
	{		
		#region IEntityImporter

		public string Name { get; set; }

		private List<ImportProperty> _systemProperties = new List<ImportProperty>();
		public List<ImportProperty> SystemProperties
		{
			get
			{
				return _systemProperties;
			}
			set
			{
				_systemProperties = value;
			}
		}

		public virtual string[] SystemPropertyNames
		{
			get 
			{
				return SystemProperties.Select(x => x.Name).ToArray();
			}
		}

		public virtual string[] SystemPropertyDisplayNames
		{
			get
			{
				return SystemProperties.Select(x => x.DisplayName).ToArray();
			}
		}

		public abstract string Import(string catalogId, string propertySetId, ImportItem[] systemValues,
		                              ImportItem[] customValues, IRepository repository);
		
		#endregion

		protected void AddSystemProperty(ImportProperty systemProperty)
		{
			SystemProperties.Add(systemProperty);
		}

		protected void AddSystemProperties(params ImportProperty[] systemProperties)
		{
			foreach (ImportProperty prop in systemProperties)
			{
				AddSystemProperty(prop);
			}
		}
		
		protected ImportAction GetAction(string action)
		{
			var retVal = ImportAction.Insert;
			switch (action)
			{
				case "Delete":
					retVal = ImportAction.Delete;
					break;
				case "Insert & Update":
					retVal = ImportAction.InsertAndReplace;
					break;
				case "Update":
					retVal = ImportAction.Update;
					break;
			}

			return retVal;
		}

		protected static void SetPropertyValue(object poco, PropertyInfo property, string value)
		{
			if (property != null)
			{
				// .. here goes code to change type to the necessary one ..
				var type = property.PropertyType;
				object setValue = null;
				

				if (!string.IsNullOrEmpty(value))
				{
					if (type == typeof(DateTime))
						setValue = DateTime.Parse(value);
					if (type == typeof(int))
						setValue = Convert.ToInt32(value);
					if (type == typeof(decimal))
						setValue = Convert.ToDecimal(value.Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat);
					if (type == typeof(decimal?))
						setValue = Convert.ToDecimal(value.Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat);
					if (type == typeof(string))
						setValue = value;
					if (type == typeof(bool))
						setValue = Convert.ToBoolean(value);
				}

				property.SetValue(poco, setValue);
			}
		}
	}
}
