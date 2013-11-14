using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Presentation.CustomerService.Infrastructure.Communications;

namespace Presentation.CustomerService.Infrastructure.Communications
{
    public class Attachment
    {
        public string FileName;
        public int Size;
        public AttachmentType Type;
        public string Url;
    }
}
