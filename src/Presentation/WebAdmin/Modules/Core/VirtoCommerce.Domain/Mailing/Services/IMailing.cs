using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Mailing.Services
{
    public interface IMailing
    {
        string Code { get; }
        string Description { get; }
        string LogoUrl { get; }
    }
}
