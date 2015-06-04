using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace VirtoCommerce.Foundation.Customers.Model
{
    public enum AttachmentType
    {
        /// <summary>
        /// Изображение
        /// </summary>
        [Description("image file")]
        Image,

        /// <summary>
        /// Документ
        /// </summary>
        [Description("document")]
        Document
    }
}
