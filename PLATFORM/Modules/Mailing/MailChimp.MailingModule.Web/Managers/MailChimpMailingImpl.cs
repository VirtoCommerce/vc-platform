using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.MailChimp.Mailing.Services;

namespace MailChimp.MailingModule.Web.Managers
{
	public class MailChimpMailingImpl : IMailing
	{
		private string _apiKey;

		private string _code;
		private string _description;
		private string _logoUrl;

		public MailChimpMailingImpl(string apiKey, string code, string description, string logoUrl)
		{
			if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentNullException("apiKey");

			_apiKey = apiKey;

			_code = code;
			_description = description;
			_logoUrl = logoUrl;
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