using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using VirtoCommerce.Domain.Mailing.Services;
using VirtoCommerce.Framework.Web.Settings;

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
				throw new ArgumentNullException("MailChimpApiKey");

			_mailing = mailing;
            _mailChimpApiKey = mailChimpApiKey;
		}        
	}
}
