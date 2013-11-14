using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Customers.Model
{
    /// <summary>
    /// Stores relations between contacts and organizations.
    /// </summary>
    [DataContract]
    [EntitySet("MemberRelations")]
    [DataServiceKey("MemberRelationId")]
    public class MemberRelation : StorageEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRelation" /> class.
        /// </summary>
        public MemberRelation()
		{
            _MemberRelationId = GenerateNewKey();
		}

        private string _MemberRelationId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string MemberRelationId
		{
			get
			{
                return _MemberRelationId;
			}
			set
			{
                SetValue(ref _MemberRelationId, () => this.MemberRelationId, value);
			}
		}

        private int _AncestorSequence;
        /// <summary>
        /// Gets or sets the ancestor sequence. A number to indicate whether the ancestor is the parent, 
        /// grandparent, great grandparent, and so on for the descendant. 
        /// 1 means parent, 2 means grand parent, and so on. 
        /// For the Top Organization, it does not have a parent, so its sequence is 0.
        /// </summary>
        /// <value>
        /// The ancestor sequence.
        /// </value>
        [DataMember]
        [Required]
        public int AncestorSequence
        {
            get
            {
                return _AncestorSequence;
            }
            set
            {
                SetValue(ref _AncestorSequence, () => this.AncestorSequence, value);
            }
        }

        #region NavigationProperties
        private string _AncestorId;
        /// <summary>
        /// Gets or sets the ancestor member id.
        /// </summary>
        /// <value>
        /// The ancestor id.
        /// </value>
        [DataMember]
        [ForeignKey("Ancestor")]
        [Required]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string AncestorId
        {
            get { return _AncestorId; }
            set
            {
                SetValue(ref _AncestorId, () => this.AncestorId, value);
            }
        }

        [DataMember]
        public virtual Member Ancestor { get; set; }

        private string _DescendantId;
        /// <summary>
        /// Gets or sets the descendant member id.
        /// </summary>
        /// <value>
        /// The descendant id.
        /// </value>
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string DescendantId
        {
            get { return _DescendantId; }
            set
            {
                SetValue(ref _DescendantId, () => this.DescendantId, value);
            }
        }

        [DataMember]
        [ForeignKey("DescendantId")]
        [Parent]
        public virtual Member Descendant { get; set; }
        #endregion
    }
}
