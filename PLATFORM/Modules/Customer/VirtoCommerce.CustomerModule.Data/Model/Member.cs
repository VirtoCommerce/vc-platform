using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerModule.Data.Model
{
	public abstract class Member : AuditableEntity
	{
		public Member()
		{
			Notes = new ObservableCollection<Note>();
			Addresses = new ObservableCollection<Address>();
			MemberRelations = new ObservableCollection<MemberRelation>();
			Phones = new ObservableCollection<Phone>();
			Emails = new ObservableCollection<Email>();
		}

		#region NavigationProperties

		public ObservableCollection<Note> Notes { get; set; }

		public ObservableCollection<Address> Addresses { get; set; }

		public ObservableCollection<MemberRelation> MemberRelations { get; set; }

		public ObservableCollection<Phone> Phones { get; set; }

		public ObservableCollection<Email> Emails { get; set; }

	    #endregion
	}
}
