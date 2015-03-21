using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace VirtoCommerce.Foundation.Security.Swt
{
	public class SimpleWebToken
	{
		public string RawToken { get; private set; }
		public Uri Issuer { get; private set; }
		public Uri Audience { get; private set; }
		public DateTime ExpiresOn { get; private set; }
		public List<KeyValuePair<string, string>> Claims { get; private set; }


		public static string Create(Uri issuer, Uri audience, DateTime expiresOn, string signatureKey, params Claim[] claims)
		{
			if (issuer == null)
				throw new ArgumentNullException("issuer");
			if (audience == null)
				throw new ArgumentNullException("audience");

			var builder = new StringBuilder();

			AppendSwtValue(builder, SwtConstants.Issuer, issuer.AbsoluteUri);
			AppendSwtValue(builder, SwtConstants.Audience, audience.AbsoluteUri);
			AppendSwtValue(builder, SwtConstants.ExpiresOn, ConvertToEpochTime(expiresOn));

			if (claims != null)
			{
				foreach (var claim in claims)
					AppendSwtValue(builder, claim.Type, claim.Value);
			}

			// Add signature
			var signature = CalculateSignature(builder.ToString(), signatureKey);
			AppendSwtValue(builder, SwtConstants.HmacSha256, signature);

			return builder.ToString();
		}


		public static SimpleWebToken Parse(string rawToken)
		{
			if (rawToken == null)
				throw new ArgumentNullException("rawToken");

			var token = new SimpleWebToken();

			token.RawToken = rawToken;

			var elements = HttpUtility.ParseQueryString(rawToken);
			var valueSeparator = new[] { ',' };
			var claims = new List<KeyValuePair<string, string>>();

			foreach (var key in elements.AllKeys)
			{
				var elementValue = elements[key];

				switch (key)
				{
					case SwtConstants.Issuer:
						token.Issuer = new Uri(elementValue);
						break;
					case SwtConstants.Audience:
						token.Audience = new Uri(elementValue);
						break;
					case SwtConstants.ExpiresOn:
						token.ExpiresOn = ConvertFromEpochTime(elementValue);
						break;
					case SwtConstants.HmacSha256:
						break;
					default:
						foreach (var value in elementValue.Split(valueSeparator, StringSplitOptions.RemoveEmptyEntries))
							claims.Add(new KeyValuePair<string, string>(key, value));
						break;
				}
			}

			token.Claims = claims;

			return token;
		}

		public bool IsValidSignature(string signatureKey)
		{
			var result = false;

			const string separator = "&" + SwtConstants.HmacSha256 + "=";
			var elements = RawToken.Split(new string[] { separator }, StringSplitOptions.None);

			if (elements.Length == 2)
			{
				var validSignature = HttpUtility.UrlEncode(CalculateSignature(elements[0], signatureKey));
				result = string.Equals(validSignature, elements[1], StringComparison.InvariantCulture);
			}

			return result;
		}


		private static void AppendSwtValue(StringBuilder builder, string name, string value)
		{
			if (builder.Length > 0)
				builder.Append("&");

			builder.Append(HttpUtility.UrlEncode(name));
			builder.Append("=");
			builder.Append(HttpUtility.UrlEncode(value));
		}

		private static string ConvertToEpochTime(DateTime dateTime)
		{
			var date = dateTime.ToUniversalTime();
			var timeSpan = date - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

			return Convert.ToUInt64(timeSpan.TotalSeconds).ToString(CultureInfo.InvariantCulture);
		}

		private static DateTime ConvertFromEpochTime(string epochTime)
		{
			var secondsSince1970 = ulong.Parse(epochTime, NumberStyles.Integer, CultureInfo.InvariantCulture);
			return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secondsSince1970);
		}

		private static string CalculateSignature(string data, string signatureKey)
		{
			using (var hashAlgorithm = new HMACSHA256(Convert.FromBase64String(signatureKey)))
			{
				var signature = hashAlgorithm.ComputeHash(Encoding.ASCII.GetBytes(data));
				return Convert.ToBase64String(signature);
			}
		}
	}
}
