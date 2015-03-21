using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

using System.Data.Services.Common;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Customers.Model
{
	[DataContract]
	[EntitySet("Members")]
	[DataServiceKey("MemberId")]
	public abstract class Member : StorageEntity
	{
		public Member()
		{
			_MemberId = GenerateNewKey();
		}

		private string _MemberId;
		[Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string MemberId
		{
			get
			{
				return _MemberId;
			}
			set
			{
				SetValue(ref _MemberId, () => this.MemberId, value);
			}
		}

        #region NavigationProperties
        private ObservableCollection<Note> _Notes = null;
        [DataMember]
        public ObservableCollection<Note> Notes
        {
            get
            {
                if (_Notes == null)
                    _Notes = new ObservableCollection<Note>();

                return _Notes;
            }
			set
			{
				_Notes = value;
			}
        }

        private ObservableCollection<Address> _Adresses = null;
        [DataMember]
        public ObservableCollection<Address> Addresses
        {
            get
            {
                if (_Adresses == null)
                    _Adresses = new ObservableCollection<Address>();

                return _Adresses;
            }
			set
			{
				_Adresses = value;
			}
        }

        ObservableCollection<Contract> _Contracts = null;
        [DataMember]
        public ObservableCollection<Contract> Contracts
        {
            get
            {
                if (_Contracts == null)
                    _Contracts = new ObservableCollection<Contract>();

                return _Contracts;
            }
			set
			{
				_Contracts = value;
			}
        }

        ObservableCollection<MemberRelation> _MemberRelations = null;
        [DataMember]
        public ObservableCollection<MemberRelation> MemberRelations
        {
            get
            {
                if (_MemberRelations == null)
                    _MemberRelations = new ObservableCollection<MemberRelation>();

                return _MemberRelations;
            }
			set
			{
				_MemberRelations = value;
			}
        }

        private ObservableCollection<Phone> _Phones = null;
        [DataMember]
        public ObservableCollection<Phone> Phones
        {
            get
            {
                if (_Phones == null)
                    _Phones = new ObservableCollection<Phone>();

                return _Phones;
            }
			set
			{
				_Phones = value;
			}
        }

	    private ObservableCollection<Email> _Emails = null;
	    [DataMember]
	    public ObservableCollection<Email> Emails
	    {
	        get
	        {
	            if(_Emails==null)
                    _Emails=new ObservableCollection<Email>();
	            return _Emails;
	        }
			set
			{
				_Emails = value;
			}
	    }


        private ObservableCollection<Label> _Labels = null;
        [DataMember]
        public ObservableCollection<Label> Labels
        {
            get
            {
                if (_Labels == null)
                    _Labels = new ObservableCollection<Label>();

                return _Labels;
            }
			set
			{
				_Labels = value;
			}
        }

	    #endregion
	}
}
