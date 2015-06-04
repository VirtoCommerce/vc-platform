using System;
using MailChimp.MailingModule.Web.Services;

namespace MailChimp.MailingModule.Web.Managers
{
    public class MailChimpMailingImpl : IMailing
    {
        private readonly string _apiAccessToken;
        private readonly string _apiDataCenter;
        private readonly string _mailChimpSubscribeList;
        private readonly string _code;
        private readonly string _description;
        private readonly string _logoUrl;

        public MailChimpMailingImpl(string apiAccessToken, string apiDataCenter, string mailChimpSubscribeList, string code, string description, string logoUrl)
        {
            _apiAccessToken = apiAccessToken;
            _apiDataCenter = apiDataCenter;
            _code = code;
            _description = description;
            _logoUrl = logoUrl;
            _mailChimpSubscribeList = mailChimpSubscribeList;
        }

        public string AccessToken
        {
            get { return _apiAccessToken; }
        }

        public string DataCenter
        {
            get { return _apiDataCenter; }
        }

        public string SubscribersListId
        {
            get { return _mailChimpSubscribeList; }
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