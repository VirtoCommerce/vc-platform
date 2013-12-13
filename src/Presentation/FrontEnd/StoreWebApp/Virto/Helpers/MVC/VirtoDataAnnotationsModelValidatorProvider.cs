using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VirtoCommerce.Client.Globalization;


namespace VirtoCommerce.Web.Virto.Helpers.MVC
{
	/// <summary>
	/// Class VirtoDataAnnotationsModelValidatorProvider. Used to override default behavior to Localize model errors.
	/// </summary>
	public class VirtoDataAnnotationsModelValidatorProvider : DataAnnotationsModelValidatorProvider
	{
		/// <summary>
		/// Class ModelValidatorWrapper.
		/// </summary>
		private class ModelValidatorWrapper : ModelValidator
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="ModelValidatorWrapper" /> class.
			/// </summary>
			/// <param name="modelValidator">The model validator.</param>
			/// <param name="metadata">The metadata.</param>
			/// <param name="controllerContext">The controller context.</param>
			public ModelValidatorWrapper(ModelValidator modelValidator, ModelMetadata metadata, ControllerContext controllerContext)
				: base(metadata, controllerContext)
			{
				InnerValidator = modelValidator;
			}

			/// <summary>
			/// Gets or sets the inner validator.
			/// </summary>
			/// <value>The inner validator.</value>
			private ModelValidator InnerValidator { get; set; }

			/// <summary>
			/// When implemented in a derived class, returns metadata for client validation.
			/// </summary>
			/// <returns>The metadata for client validation.</returns>
			public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
			{
				foreach (var item in InnerValidator.GetClientValidationRules())
				{
					item.ErrorMessage = item.ErrorMessage.Localize();
					yield return item;
				}

			}
			/// <summary>
			/// When implemented in a derived class, validates the object.
			/// </summary>
			/// <param name="container">The container.</param>
			/// <returns>A list of validation results.</returns>
			public override IEnumerable<ModelValidationResult> Validate(object container)
			{
				foreach (var item in InnerValidator.Validate(container))
				{
					item.Message = item.Message.Localize();
					yield return item;
				}
			}
		}

		/// <summary>
		/// Gets a list of validators.
		/// </summary>
		/// <param name="metadata">The metadata.</param>
		/// <param name="context">The context.</param>
		/// <param name="attributes">The list of validation attributes.</param>
		/// <returns>A list of validators.</returns>
		protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context, IEnumerable<Attribute> attributes)
		{
			return base.GetValidators(metadata, context, attributes)
				.Select(validator => new ModelValidatorWrapper(validator, metadata, context));
		}
	}
}