using System;
using System.Web.Http;
using VirtoCommerce.Domain.Mailing.Services;

namespace MailChimp.MailingModule.Web.Controllers
{
    [RoutePrefix("api/mailing")]
    public class MailChimpController : ApiController
    {
        private string _mailChimpApiKey;
        private IMailing _mailing;

        public MailChimpController(IMailing mailing, string mailChimpApiKey)
        {

            if (mailing == null)
                throw new ArgumentNullException("mailing");

            if (string.IsNullOrEmpty(mailChimpApiKey))
                throw new ArgumentNullException("mailChimpApiKey");

            _mailing = mailing;
            _mailChimpApiKey = mailChimpApiKey;
        }
    }
}
