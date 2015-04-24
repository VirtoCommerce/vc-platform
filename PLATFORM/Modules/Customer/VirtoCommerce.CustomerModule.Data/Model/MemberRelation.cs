using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CustomerModule.Data.Model
{
    /// <summary>
    /// Stores relations between contacts and organizations.
    /// </summary>
    public class MemberRelation : Entity
    {
        /// <summary>
        /// Gets or sets the ancestor sequence. A number to indicate whether the ancestor is the parent, 
        /// grandparent, great grandparent, and so on for the descendant. 
        /// 1 means parent, 2 means grand parent, and so on. 
        /// For the Top Organization, it does not have a parent, so its sequence is 0.
        /// </summary>
        /// <value>
        /// The ancestor sequence.
        /// </value>
        [Required]
		public int AncestorSequence { get; set; }

        #region NavigationProperties
        /// <summary>
        /// Gets or sets the ancestor member id.
        /// </summary>
        /// <value>
        /// The ancestor id.
        /// </value>
        [ForeignKey("Ancestor")]
        [Required]
		[StringLength(128)]
		public string AncestorId { get; set; }

        public virtual Member Ancestor { get; set; }

        /// <summary>
        /// Gets or sets the descendant member id.
        /// </summary>
        /// <value>
        /// The descendant id.
        /// </value>
 		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string DescendantId { get; set; }

        [ForeignKey("DescendantId")]
        [Parent]
        public virtual Member Descendant { get; set; }
        #endregion
    }
}
