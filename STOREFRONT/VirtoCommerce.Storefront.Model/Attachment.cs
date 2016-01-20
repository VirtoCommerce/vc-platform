﻿using System;

namespace VirtoCommerce.Storefront.Model
{
    public class Attachment
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string MimeType { get; set; }

        public long? Size { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public string Id { get; set; }
    }
}