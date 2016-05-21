﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Platform.Data.Infrastructure.Interceptors
{
    public interface IInterceptor
    {
        void Before(InterceptionContext context);
        void After(InterceptionContext context);
    }
}
