using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Mailing.Services
{
    public interface IMailingManager
    {
        void RegisterMailing(IMailing mailingModule);
        IEnumerable<IMailing> Mailings { get; }
    }
}
