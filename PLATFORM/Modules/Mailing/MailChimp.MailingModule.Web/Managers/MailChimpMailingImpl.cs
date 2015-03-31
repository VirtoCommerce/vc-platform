using System;
using MailChimp.MailingModule.Web.Services;

namespace MailChimp.MailingModule.Web.Managers
{
    public class MailChimpMailingImpl : IMailing
    {
        private readonly string _apiKey;
        private readonly string _mailChimpSubscribeList;
        private readonly string _code;
        private readonly string _description;
        private readonly string _logoUrl;

        public MailChimpMailingImpl(string apiKey, string mailChimpSubscribeList, string code, string description, string logoUrl)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentNullException("apiKey");

            if (string.IsNullOrEmpty(mailChimpSubscribeList))
                throw new ArgumentNullException("mailChimpSubscribeList");

            _apiKey = apiKey;

            _code = code;
            _description = description;
            _logoUrl = logoUrl;
            _mailChimpSubscribeList = mailChimpSubscribeList;
        }

        public string Code
        {
            get { return _code; }
        }

        public string Description
        {
            get { return _description; }
        }

        public string LogoUrl
        {
            get { return _logoUrl; }
        }
    }
}