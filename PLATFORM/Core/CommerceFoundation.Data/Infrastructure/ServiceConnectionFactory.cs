using System;
using System.Configuration;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Data.Infrastructure
{
	public class ServiceConnectionFactory : IServiceConnectionFactory
	{
		public ServiceConnectionFactory(string baseUrl)
		{
			_baseUrl = baseUrl;
		}

		private readonly string _baseUrl;
		
		public string GetConnectionString(string relativeUri, bool forceHttps = false)
		{
			if (!string.IsNullOrEmpty(_baseUrl))
			{
				if (forceHttps && !_baseUrl.StartsWith("https://"))
				{
					return new Uri(new Uri(_baseUrl.Replace("http://","https://")), relativeUri).AbsoluteUri;
				}
				return new Uri(new Uri(_baseUrl), relativeUri).AbsoluteUri;
			}

			if (relativeUri.StartsWith("http://") || relativeUri.StartsWith("https://"))
			{
				if (forceHttps && !relativeUri.StartsWith("https://"))
				{
					return new Uri(relativeUri.Replace("http://", "https://")).AbsoluteUri;
				}
				return new Uri(relativeUri).AbsoluteUri;
			}

			return null;
		}
	}
}
