using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Customer.Model;

namespace VirtoCommerce.CustomerModule.Data.Model
{
	public class NoteDataEntity : AuditableEntity
	{

		[StringLength(128)]
		public string AuthorName { get; set; }

		[StringLength(128)]
		public string ModifierName { get; set; }

		[StringLength(128)]
		public string Title { get; set; }

		public string Body { get; set; }

		public bool IsSticky { get; set; }

		#region Navigation Properties
		public string MemberId { get; set; }
        public virtual MemberDataEntity Member { get; set; }

        #endregion


        public virtual Note ToNote(Note note)
        {
            if (note == null)
                throw new ArgumentNullException("note");

            note.InjectFrom(this);
            return note;
        }

        public virtual NoteDataEntity FromNote(Note note)
        {
            if (note == null)
                throw new ArgumentNullException("note");

            this.InjectFrom(note);
            this.AuthorName = note.CreatedBy;
            this.ModifierName = note.ModifiedBy;
            return this;
        }

        public virtual void Patch(NoteDataEntity target)
        {
            var patchInjectionPolicy = new PatchInjection<NoteDataEntity>(x => x.Body, x => x.Title);
            target.InjectFrom(patchInjectionPolicy, this);
        }
	}
}
