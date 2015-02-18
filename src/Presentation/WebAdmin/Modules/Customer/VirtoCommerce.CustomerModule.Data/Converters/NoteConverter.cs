using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundationModel = VirtoCommerce.Foundation.Customers.Model;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;

namespace VirtoCommerce.CustomerModule.Data.Converters
{
	public static class NoteConverter
	{
		public static coreModel.Note ToCoreModel(this foundationModel.Note entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new coreModel.Note();
			retVal.InjectFrom(entity);
			retVal.CreatedDate = entity.Created ?? DateTime.UtcNow;
			retVal.CreatedBy = entity.AuthorName;
			retVal.ModifiedBy = entity.ModifierName;
			return retVal;
		}

		public static foundationModel.Note ToFoundation(this coreModel.Note note)
		{
			if (note == null)
				throw new ArgumentNullException("note");

			var retVal = new foundationModel.Note();
			retVal.InjectFrom(note);
			retVal.AuthorName = note.CreatedBy;
			retVal.ModifierName = note.ModifiedBy;
			return retVal;
		}


		/// <summary>
		/// Patch 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundationModel.Note source, foundationModel.Note target)
		{
			var patchInjectionPolicy = new PatchInjection<foundationModel.Note>(x => x.Body, x => x.Title);
			target.InjectFrom(patchInjectionPolicy, source);
		}

	}
}
