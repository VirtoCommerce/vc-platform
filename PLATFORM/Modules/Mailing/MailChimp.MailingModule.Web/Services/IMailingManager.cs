using System.Collections.Generic;

namespace MailChimp.MailingModule.Web.Services
{
    public interface IMailingManager
    {
        void RegisterMailing(IMailing mailingModule);
        IEnumerable<IMailing> Mailings { get; }
    }
}
