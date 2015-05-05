using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataModel = VirtoCommerce.CustomerModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.CustomerModule.Data.Converters
{
    public static class NoteConverter
    {
        public static coreModel.Note ToCoreModel(this dataModel.Note entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var retVal = new coreModel.Note();
            retVal.InjectFrom(entity);

            return retVal;
        }

        public static dataModel.Note ToDataModel(this coreModel.Note note)
        {
            if (note == null)
                throw new ArgumentNullException("note");

            var retVal = new dataModel.Note();
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
        public static void Patch(this dataModel.Note source, dataModel.Note target)
        {
            var patchInjectionPolicy = new PatchInjection<dataModel.Note>(x => x.Body, x => x.Title);
            target.InjectFrom(patchInjectionPolicy, source);
        }

    }
}
