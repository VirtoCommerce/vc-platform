using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Data.Services;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Frameworks
{
    /// <summary>
    /// Base class for model classes. Implements basic validation and adds additional properties: Created, LastModified date fields.
    /// You do not have to inherit from this class.
    /// </summary>
	[DataContract(IsReference = true)]
	[IgnoreProperties("Error")]
	public abstract class StorageEntity : INotifyPropertyChanging, INotifyPropertyChanged, IValidatable, IModifiedDateTimeFields
	{
        /// <summary>
        /// Generates the new key.
        /// </summary>
        /// <returns></returns>
		public string GenerateNewKey()
		{
			return Guid.NewGuid().ToString();
		}

		#region INotifyPropertyChanging Members

		public event PropertyChangingEventHandler PropertyChanging;

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region IValidatable Members

		private Dictionary<string, Tuple<PropertyInfo, ValidationAttribute[]>> _validators = new Dictionary<string, Tuple<PropertyInfo, ValidationAttribute[]>>();
		

		private Dictionary<string, string> _errors = new Dictionary<string, string>();
        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
		[DataMember]
		[NotMapped]
		public Dictionary<string, string> Errors
		{
			get
			{
				return _errors ?? (_errors = new Dictionary<string, string>());
			}
			set
			{
				_errors = value;
			}
		}

        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="doNotifyChanges">if set to <c>true</c> [do notify changes].</param>
		public virtual void SetError(string propertyName, string errorMessage, bool doNotifyChanges)
		{
			if (string.IsNullOrEmpty(errorMessage))
			{
				doNotifyChanges = Errors.Remove(propertyName) && doNotifyChanges;
			}
			else
			{
				// checking to avoid infinite loop
				if (Errors.ContainsKey(propertyName) && Errors[propertyName] == errorMessage)
					doNotifyChanges = false;
				else
					Errors[propertyName] = errorMessage;
			}

			if (doNotifyChanges)
			{
				//OnPropertyChanged(propertyName);
				var tmp = PropertyChanged;
				if (tmp != null)
				{
					tmp(this, new PropertyChangedEventArgs(propertyName));
				}
			}
		}

        /// <summary>
        /// Clears the error.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
		public virtual void ClearError(string propertyName)
		{
			SetError(propertyName, string.Empty, true);
		}

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns></returns>
		public virtual bool Validate()
		{
			return Validate(true);
		}

        /// <summary>
        /// Prepares the validators.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns></returns>
		private bool PrepareValidators(string propertyName, out PropertyInfo propertyInfo)
		{
			bool value;
			if (_validators==null)
				_validators=new Dictionary<string, Tuple<PropertyInfo, ValidationAttribute[]>>();
			Tuple<PropertyInfo, ValidationAttribute[]> cachedValue;
			var isInCache = _validators.TryGetValue(propertyName, out cachedValue);
			if (isInCache) // check cache
			{
				Debug.Assert(cachedValue!=null);
				value = cachedValue.Item2.Length > 0;
				propertyInfo = cachedValue.Item1;
			}
			else
			{
				propertyInfo = GetType().GetProperty(propertyName);
				Debug.Assert(propertyInfo != null);
				value = AddToValidators(propertyInfo);
			}
			return value;
		}

        /// <summary>
        /// Prepares the validators.
        /// </summary>
		private void PrepareValidators()
		{
			var properties = GetType().GetProperties();
			foreach (var propertyInfo in properties)
			{
			    if (_validators == null)
			    {
			        _validators = new Dictionary<string, Tuple<PropertyInfo, ValidationAttribute[]>>();
			    }

			    Tuple<PropertyInfo, ValidationAttribute[]> cachedValue;
				var isInCache = _validators.TryGetValue(propertyInfo.Name, out cachedValue);
			    if (!isInCache) // check cache
			    {
			        AddToValidators(propertyInfo);
			    }
			}
		}

        /// <summary>
        /// Adds to validators.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns></returns>
		private bool AddToValidators(PropertyInfo propertyInfo)
		{
			bool value=false;
			var list = propertyInfo.GetCustomAttributes(typeof(Attribute), true);
			var validations = new List<ValidationAttribute>();

			RequiredAttribute required = null;
			bool isForeignKeyAttribute = false;
			foreach (var a in list)
			{
				if (a is ForeignKeyAttribute && isForeignKeyAttribute == false)
				{
					isForeignKeyAttribute = true;
				}
				else if (a is RequiredAttribute)
				{
					required = (RequiredAttribute)a;
				}
				else if (a is ValidationAttribute)
				{
					validations.Add((ValidationAttribute)a);
				}
			}
			if (!isForeignKeyAttribute && required!=null)
			{
				validations.Add(required);
			}

			if (validations.Count == 0)
			{
				_validators.Add(propertyInfo.Name, new Tuple<PropertyInfo, ValidationAttribute[]>(propertyInfo, new ValidationAttribute[] { }));
			}
			else
			{
				_validators.Add(propertyInfo.Name, new Tuple<PropertyInfo, ValidationAttribute[]>(propertyInfo,validations.ToArray()));
				value = true;
			}
			return value;
		}

        /// <summary>
        /// Validates the specified do notify changes.
        /// </summary>
        /// <param name="doNotifyChanges">if set to <c>true</c> [do notify changes].</param>
        /// <returns></returns>
		public virtual bool Validate(bool doNotifyChanges)
		{
			PrepareValidators();

			// CheckValidationState() changes _errors - saving unchanged value
			var result = _errors.Count == 0;

			var notEmptyValidators = _validators.Where(validator => validator.Value != null && validator.Value.Item2.Length > 0)
				.Select(validator => validator.Value.Item1);
            if (doNotifyChanges)
            {
                // calling ToList() in order to validate every property
                result = notEmptyValidators
                             .Select(validator => CheckValidationState(validator, true)).ToList().All(x => x) && result;
            }
            else
            {
                result = notEmptyValidators.All(validator => CheckValidationState(validator, false)) && result;
            }

            return result;
		}

		#region IDataErrorInfo
        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
		[DoNotSerializeAttribute]
		[NotMapped]
		public virtual string Error
		{
			get
			{
				return null;
			}
		}

        /// <summary>
        /// used by binding validation
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        [NotMapped]
		public virtual string this[string columnName]
		{
			get
			{
				return _errors.ContainsKey(columnName) ? _errors[columnName] : null;
			}
		}


        /// <summary>
        /// Used by explicit validation
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="doNotifyChanges">should UI be notified if error found</param>
        /// <returns></returns>
		protected virtual bool CheckValidationState(PropertyInfo propertyInfo, bool doNotifyChanges)
		{
			//var p = GetType().GetProperty(propertyName);
			var propertyValue = propertyInfo.GetValue(this, null);

			var errorMessages = _validators[propertyInfo.Name].Item2
				.Where(v => !v.IsValid(propertyValue))
				.Select(v => string.IsNullOrEmpty(v.ErrorMessage) ? ((v is RequiredAttribute) ?
					("Field '" + propertyInfo.Name + "' is required.") : ("Field '" + propertyInfo.Name + "' is not valid")) : v.ErrorMessage);

			var finalError = string.Join(Environment.NewLine, errorMessages);
			SetError(propertyInfo.Name, finalError, doNotifyChanges);

			var result = string.IsNullOrEmpty(finalError);
			return result;
		}

		#endregion

		#region private members

		#endregion
		#endregion


        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
		protected virtual void OnPropertyChanged(string propertyName)
		{
			var tmp = PropertyChanged;

			if (tmp != null)
			{
				tmp(this, new PropertyChangedEventArgs(propertyName));
			}
            if (Configuration.EnableEntityValidation)
            {
                PropertyInfo propertyInfo;
                bool hasValidators = PrepareValidators(propertyName, out propertyInfo);
                if (hasValidators)
                {
                    CheckValidationState(propertyInfo, false);
                }
            }
		}

		protected virtual void OnPropertyChanging(string propertyName)
		{
			var tmp = PropertyChanging;
			if (tmp != null)
			{
				tmp(this, new PropertyChangingEventArgs(propertyName));
			}
		}

        /// <summary>
        /// Sets the value, using simple string format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">The property value.</param>
		protected void SetValue<T>(ref T field, string propertyName, T propertyValue)
		{
			OnPropertyChanging(propertyName);
			field = propertyValue;
            OnPropertyChanged(propertyName);
		}

        /// <summary>
        /// Sets the value using expression format. Preferred way to reduce misspelling.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="propertyValue">The property value.</param>
		protected void SetValue<T, TProperty>(ref T field, Expression<Func<TProperty>> expression, T propertyValue)
		{
			var propertyName = GetPropertyName(expression);
            OnPropertyChanging(propertyName);
			field = propertyValue;
            OnPropertyChanged(propertyName);
		}

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
		protected string GetPropertyName<TProperty>(Expression<Func<TProperty>> expression)
		{
			var memberExpression = expression.Body as MemberExpression;
			if (memberExpression == null)
			{
				var unaryExpression = expression.Body as UnaryExpression;
				if (unaryExpression != null)
				{
					memberExpression = unaryExpression.Operand as MemberExpression;
					if (memberExpression == null)
						throw new NotImplementedException();
				}
				else
					throw new NotImplementedException();
			}

			var propertyName = memberExpression.Member.Name;
			return propertyName;
		}

        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        /// <value>
        /// The last modified.
        /// </value>
		[DataMember]
		public DateTime? LastModified
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
		[DataMember]
		public DateTime? Created
		{
			get;
			set;
		}
	}
}
