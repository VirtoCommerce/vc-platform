using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;

namespace VirtoCommerce.Web.Views.Engines.Liquid
{
    public interface ITemplateParser
    {
        Template Parse(ViewLocationResult location);
    }
}
