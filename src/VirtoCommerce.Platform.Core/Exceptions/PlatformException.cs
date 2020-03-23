﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Exceptions
{
    /// <summary>
    /// Main platform exception type
    /// </summary>
    public class PlatformException : Exception
    {
        public PlatformException(string message)
            : base(message)
        {
        }

        public PlatformException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
