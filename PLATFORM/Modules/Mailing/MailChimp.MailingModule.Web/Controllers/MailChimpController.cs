using System;
using System.Web.Http;
using MailChimp.MailingModule.Web.Services;

namespace MailChimp.MailingModule.Web.Controllers
{
    [RoutePrefix("api/mailing")]
    public class MailChimpController : ApiController
    {
        private readonly string _mailChimpApiKey;
        private readonly IMailing _mailing;

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
