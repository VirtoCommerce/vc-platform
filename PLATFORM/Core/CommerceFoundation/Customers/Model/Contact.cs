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
    public class Contact : Member
    {
        public Contact()
        {
            BirthDate = DateTime.Now;
        }

        #region UserProfile members
        string _FullName;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [Required]
        public string FullName
        {
            get { return _FullName; }
            set { SetValue(ref _FullName, () => FullName, value); }
        }

        private string _TimeZone;
        [DataMember]
        [StringLength(32)]
        public string TimeZone
        {
            get { return _TimeZone; }
            set
            {
                SetValue(ref _TimeZone,()=>TimeZone,value);
            }
        }

        private string _DefaultLanguage;
        [DataMember]
        [StringLength(32)]
        public string DefaultLanguage
        {
            get { return _DefaultLanguage; }
            set { SetValue(ref _DefaultLanguage, () => DefaultLanguage, value); }
        }

        DateTime? _BirthDate;
        [DataMember]
        public DateTime? BirthDate
        {
            get { return _BirthDate; }
            set { SetValue(ref _BirthDate, () => BirthDate, value); }
        }

        string _TaxpayerId;
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string TaxpayerId
        {
            get { return _TaxpayerId; }
            set { SetValue(ref _TaxpayerId, () => TaxpayerId, value); }
        }

        string _PreferredDelivery;
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string PreferredDelivery
        {
            get { return _PreferredDelivery; }
            set { SetValue(ref _PreferredDelivery, () => PreferredDelivery, value); }
        }

        string _PreferredCommunication;
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string PreferredCommunication
        {
            get { return _PreferredCommunication; }
            set { SetValue(ref _PreferredCommunication, () => PreferredCommunication, value); }
        }

        private byte[] _Photo;
        [DataMember]
        public byte[] Photo
        {
            get
            {
                return _Photo;
            }
            set
            {
                SetValue(ref _Photo, () => this.Photo, value);
            }
        }

        private string _Salutation;
        [DataMember]
        [StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
        public string Salutation
        {
            get
            {
                return _Salutation;
            }
            set
            {
                SetValue(ref _Salutation, () => this.Salutation, value);
            }
        }

        #endregion

        #region NavigationProperties
      
        private ObservableCollection<Case> _Cases = null;
        [DataMember]
        public ObservableCollection<Case> Cases
        {
            get
            {
                if (_Cases == null)
                    _Cases = new ObservableCollection<Case>();

                return _Cases;
            }
        }

        private ObservableCollection<ContactPropertyValue> _ContactPropertyValues = null;
        [DataMember]
        public ObservableCollection<ContactPropertyValue> ContactPropertyValues
        {
            get
            {
                if (_ContactPropertyValues == null)
                    _ContactPropertyValues = new ObservableCollection<ContactPropertyValue>();

                return _ContactPropertyValues;
            }
			set
			{
				_ContactPropertyValues = value;
			}
        }

        #endregion
    }
}
